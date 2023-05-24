using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using yanglegeyang.components;
using yanglegeyang.utils;

namespace yanglegeyang.container {
	public partial class CardSlotControl : Panel {
		static SoundPlayer audioClip = null;
		static SoundPlayer failClip = null;

		static CardSlotControl() {
			audioClip = new SoundPlayer(); // 在此之前确保已经创建了 audioClip 对象的实例
			failClip = new SoundPlayer(); // 在此之前确保已经创建了 audioClip 对象的实例

			string audioPath = "./static/audio/win.wav";
			string failPath = "./static/audio/fail.wav";
			if (File.Exists(audioPath)) {
				using (Stream stream = File.OpenRead(audioPath)) {
					audioClip.Stream = stream;
					audioClip.Load();
				}

				using (Stream stream = File.OpenRead(failPath)) {
					failClip.Stream = stream;
					failClip.Load();
				}
			}
			else {
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
		bool isOver;

		Color bgColor = Color.FromArgb(157, 97, 27);

		Color borderColor = Color.FromArgb(198, 128, 48);

		private ImageControl _imageControl;

		public List<FruitObject> Slots = new List<FruitObject>();
		
		public int InitX;
		public int InitY;

		public CardSlotControl(ImageControl imageContainer, int initX, int initY) {
			this.BorderStyle = BorderStyle.None;
			this.InitY = initY;
			this.InitX = initX;
			this.InitY += 20;
			this.InitX -= borderSize;
			this.Size = new Size(FruitObject.DefaultWidth * slot + step * 2 + borderSize * 2,
				FruitObject.DefaultHeight + step * 2 + borderSize * 2);
			this.Location = new Point(InitX, InitY);
			imageContainer.Controls.Add(this);
			this._imageControl = imageContainer;
			this.BackColor = Color.Transparent;
			this.DoubleBuffered = true;
		}

		// 点击添加
		public Dictionary<string, int> AddSlot(FruitObject obj) {
			if (isOver)
				return null;
			Slots.Add(obj);
	
			// 从背景中删除
			obj.RemoveImageContainer();
			obj.Fruits.IsSlot = true;
			
			// 如果属于翻牌区
			if (MySpace.FoldQueue.Contains(obj)) {
				MySpace.Fold_update();
			}

			// 从层级中删除该卡片
			MySpace.remove_level_fruit(obj);
			// 重新绘制底层
			MySpace.update_flag(obj);
			// 排序验卡区中的图片
			Slots = Slots.OrderBy(x => x.ImageName).ToList();

			// 判断是第几个元素
			var idx = Slots.FindIndex(f => f.Equals(obj));
			int pointX = step + (idx) * FruitObject.DefaultWidth + borderSize / 2;
			// 3张图片的判断，如果有直接消除，思路是：分组后看每组数量是否超过3张如果超过则消除
			var groups = Slots.GroupBy(x => x.ImageName);
			foreach (var group in groups) {
				List<FruitObject> objects = group.ToList();
				if (objects.Count == 3) {
					audioClip.Play();
					// 消除的元素直接从集合中删除
					foreach (FruitObject fruitObject in objects) {
						fruitObject.RemoveCardSlotContainer();
						fruitObject.RemoveImageContainer();
					}

					Slots.RemoveAll(x => objects.Contains(x));
					idx = -1;
				}
			}

			// 新添加的卡片，显示到验卡区
			Redraw();

			// 判断游戏是否结束
			if (Slots.Count == slot) {
				isOver = true;
				failClip.Play();
				HomeForm.Game.waveOut.Stop();
				int currentFormWidth = this.Width;
				int currentFormHeight = this.Height;
				int currentFormX = this.Location.X;
				int currentFormY = this.Location.Y;
				FailForm f = new FailForm();
				// 计算 f 窗体在当前窗体中居中所需的位置
				int fWidth = f.Width;
				int fHeight = f.Height;
				int fX = currentFormX + (currentFormWidth - fWidth) / 2;
				int fY = currentFormY + (currentFormHeight - fHeight) / 2;

				// 设置 f 窗体的位置为居中位置
				f.StartPosition = FormStartPosition.Manual;
				f.Location = new Point(fX, fY);
				
				f.FormClosed += (s, args) =>
				{
					// 清空卡槽
					Slots.Clear();
					// 继续游戏的代码
					isOver = false;

					// 继续游戏的代码
					isOver = false;

					// 绘制卡槽
					Redraw();

					HomeForm.Game.waveOut.Play();
					Invalidate();
				};
				
				f.ShowDialog();
				return null;
			}

			if (Slots.Count == 0 && MySpace.GetLength() <= 0) {
				isOver = true;
				HomeForm.Game.Hide();
				HomeForm.Game.waveOut.Stop();
				HomeForm.Game = new GameObject(2);
				HomeForm.Game.ShowDialog();
				return null;
			}

			if (idx == -1) return null;

			return new Dictionary<string, int> {
				["x"] = pointX + InitX,
				["y"] = borderSize + InitY
			};
		}

		public void Redraw() {
			this.Controls.Clear();
			for (int i = 0; i < Slots.Count; i++) {
				FruitObject fruitObject = Slots[i];
				int pointX = step + i * FruitObject.DefaultWidth + borderSize / 2;

				if (fruitObject.Fruits.Location.Y == 10) {
					int sx = fruitObject.Fruits.Bounds.X, sy = fruitObject.Fruits.Bounds.Y;
					Animation animation = new Animation(fruitObject.Fruits, new Point(sx, sy),
						new Point(pointX, borderSize), 100);
					animation.Start();
				}
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