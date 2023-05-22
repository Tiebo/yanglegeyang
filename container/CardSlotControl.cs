using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using yanglegeyang.components;

namespace yanglegeyang.container {
	public partial class CardSlotControl : Panel {
		static SoundPlayer audioClip = null;
		static SoundPlayer failClip = null;
		
		static CardSlotControl() {
			audioClip = new SoundPlayer(); // 在此之前确保已经创建了 audioClip 对象的实例
			failClip = new SoundPlayer(); // 在此之前确保已经创建了 audioClip 对象的实例

			string audioPath = "./static/audio/win.wav";
			string failPath = "./static/audio/fail.wav";
			if (File.Exists(audioPath))
			{
				using (Stream stream = File.OpenRead(audioPath))
				{
					audioClip.Stream = stream;
					audioClip.Load();
				}
				
				using (Stream stream = File.OpenRead(failPath))
				{
					failClip.Stream = stream;
					failClip.Load();
				}
			}
			else
			{
				Console.WriteLine($@"Audio file not found: {audioPath}");
			}
		}
	
		// 卡片间隔
		int step = 5;

		// 内外底色的间隔
		int borderSize = 10;

		// 卡片最大数
		int slot = 7;

		// 是否结束
		bool isOver = false;

		Color bgColor = Color.FromArgb(157, 97, 27);

		Color borderColor = Color.FromArgb(198, 128, 48);

		List<FruitObject> slots = new List<FruitObject>();
		int _initX;
		int _initY;

		public CardSlotControl(ImageControl imageContainer, int initX, int initY) {
			this._initY += 20;
			this._initX += -borderSize;
			this.BorderStyle = BorderStyle.None;
			this.BackColor = Color.Transparent;
			this.DoubleBuffered = true;
			this._initY = initY;
			this._initX = initX;
			this.Size = new Size(FruitObject.DefaultWidth * slot + step * 2 + borderSize * 2,
				FruitObject.DefaultHeight + step * 2 + borderSize * 2);
			this.Location = new Point(_initX, _initY);
			imageContainer.Controls.Add(this);
		}

		// 点击添加
		public void AddSlot(FruitObject obj) {
			if (isOver)
				return;

			slots.Add(obj);
			// 验卡区的卡片删除点击事件
			obj.RemoveImageCantainer();
			// 从层级中删除该卡片
			Console.WriteLine(obj.Level);
			MySpace.remove_level_fruit(obj);
			// 重新绘制底层
			MySpace.update_flag(obj);
			// Retrieve the delegate list from the MouseClick event handler.
			obj.RemoveClick();
			// 排序验卡区中的图片
			slots = slots.OrderBy(x => x.ImageName).ToList();
			// 3张图片的判断，如果有直接消除，思路是：分组后看每组数量是否超过3张如果超过则消除
			var groups = slots.GroupBy(x => x.ImageName);
			foreach (var group in groups) {
				List<FruitObject> objects = group.ToList();
				if (objects.Count == 3) {
					Console.WriteLine(@"消除图片");
					audioClip.Play();
					// 消除的元素直接从集合中删除
					foreach (FruitObject fruitObject in objects) {
						fruitObject.RemoveCardSlotCantainer();
					}

					slots.RemoveAll(x => objects.Contains(x));
				}
			}

			// 新添加的卡片，显示到验卡区
			Redraw();
			// 判断游戏是否结束
			if (slots.Count == slot) {
				isOver = true;
				failClip.Play();
				MessageBox.Show(this.Parent, @"Game Over：槽满了", @"Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void Redraw() {
			this.Controls.Clear();
			for (int i = 0; i < slots.Count; i++) {
				FruitObject fruitObject = slots[i];
				int pointX = step + i * FruitObject.DefaultWidth + borderSize / 2;
				fruitObject.Fruits.Location = new Point(pointX, borderSize);
				this.Controls.Add(fruitObject.Fruits);
			}

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);
			var g2d = e.Graphics;
			g2d.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			var basicStroke = new Pen(Color.Black, borderSize);
			g2d.DrawRectangle(basicStroke, 0, 0, (int) (this.Size.Width - borderSize),
				(int) (this.Size.Height - borderSize));

			// 绘制第1层底色
			var bgBrush = new SolidBrush(bgColor);
			g2d.FillRectangle(bgBrush, borderSize / 2, borderSize / 2, (int) (this.Size.Width - 1 - borderSize),
				(int) (this.Size.Height - 1 - borderSize));

			// 绘制第2层底色
			var borderColor1 = new SolidBrush(this.borderColor);
			g2d.FillRectangle(borderColor1, borderSize, borderSize, (int) (this.Size.Width - 1 - borderSize * 2),
				(int) (this.Size.Height - 1 - borderSize * 2));
		}
	}
}