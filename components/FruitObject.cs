using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using yanglegeyang.container;

namespace yanglegeyang.components {
	public class FruitObject {
		public static readonly int DefaultWidth = 80;
		public static readonly int DefaultHeight = 80;

		private static Random _random = new Random();

		private static SoundPlayer audioClip;

		// 存放的卡片
		public Fruits Fruits { get; set; }


		// 所在层的x序号
		public int X { get; set; }

		// 所在层的y序号
		public int Y { get; set; }

		// 所在层序号
		public int Level { get; set; }

		// 卡片名称,有点重复了，后续优化
		public string ImageName { get; set; } //名称，非常重要，安装名称来判断是否为同一类型

		public bool Flag = false; //是否可以点击

		ImageControl _imageControl;

		CardSlotControl _cardSlotControl;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="cardSlotCantainer"></param>
		/// <param name="fruits"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="level"></param>
		public FruitObject(CardSlotControl cardSlotCantainer, Fruits fruits, int x, int y, int level) {
			this.Fruits = fruits;


			this.X = x;
			this.Y = y;
			this.Level = level;
			this.ImageName = fruits.ImageName;
			this._cardSlotControl = cardSlotCantainer;
			this.Fruits.Cursor = Cursors.Hand;
		}

		static FruitObject() {
			audioClip = new SoundPlayer(); // 在此之前确保已经创建了 audioClip 对象的实例

			string audioPath = "./static/audio/click.wav";
			if (File.Exists(audioPath)) {
				using (Stream stream = File.OpenRead(audioPath)) {
					audioClip.Stream = stream;
					audioClip.Load();
				}
			}
			else {
				Console.WriteLine($@"Audio file not found: {audioPath}");
			}
		}

		public void Show(ImageControl imageControl1, int initX, int initY, bool rx, bool ry) {
			this._imageControl = imageControl1;
			// 随机生成开始坐标偏移量，实现上下层错落有致的视觉感
			bool randomX = rx;
			bool randomY = ry;

			int pointX = initX + X * DefaultWidth + 
			             (randomX ? DefaultWidth / 2 : 0);
			int pointY = initY + Y * DefaultHeight + 
			             (randomY ? DefaultHeight / 2 : 0);

			// 设置卡片显示在背景面板中位置
			Fruits.SetBounds(pointX, pointY, DefaultWidth, DefaultHeight);
			// 记录卡片的空间信息
			imageControl1.Controls.Add(Fruits);
			MySpace.Add_level_fruit(this);
			AddClick();
		}

		public void ShowFold(ImageControl imageControl1, int initX, int initY, int offset) {
			this._imageControl = imageControl1;

			int pointX = initX + X * DefaultWidth + offset;
			int pointY = initY + Y * DefaultHeight - DefaultHeight / 4;
			// 设置卡片显示在背景面板中位置
			Fruits.SetBounds(pointX, pointY, DefaultWidth, DefaultHeight);
			// 记录卡片的空间信息
			MySpace.Add_fold_list(this);
			_imageControl.Controls.Add(Fruits);
			AddClick();
		}

		private Point _targetPosition;
		private Timer _timer;
		private Point _oldLocation;

		public void DrawAnimation(int tx, int ty) {
			_imageControl.Controls.Add(this.Fruits);
			this.Fruits.BringToFront();
			// 设置目标位置
			_targetPosition = new Point(tx, ty);

			// 创建一个Timer
			_timer = new Timer();
			_timer.Interval = 1000 / 60;
			_timer.Tick += Update;
			_timer.Start();
		}

		private float t;

		private void Update(object sender, EventArgs e) {
			// 使用线性插值算法计算新的位置
			t += 0.2f;
			int x = (int) Lerp(_oldLocation.X, _targetPosition.X);
			int y = (int) Lerp(_oldLocation.Y, _targetPosition.Y);
			this.Fruits.Location = new Point(x, y);

			// 如果已经到达目标位置，停止Timer
			if (t >= 1) {
				_timer.Stop();
				this.Fruits.Location = new Point(this.Fruits.Location.X - _cardSlotControl.InitX,
					this.Fruits.Location.Y - _cardSlotControl.InitY);
				_cardSlotControl.Controls.Add(this.Fruits);
			}
		}

		private float Lerp(float start, float end) {
			return (1 - t) * start + t * end;
		}

		private void F_MouseClick(object sender, MouseEventArgs e) {
			if (Flag) {
				this.Fruits.Width = 80;
				this.Fruits.Height = 80;
				audioClip.Play();
				
				_oldLocation = this.Fruits.Location;
				
				this.Fruits.Visible = false;
				
				var res = _cardSlotControl.AddSlot(this);
				
				this.Fruits.Location = _oldLocation;
				
				this.Fruits.Visible = true;
				if (res != null) {
					DrawAnimation(res["x"], res["y"]);
				}
			}
		}

		/**
	     * 卡片点击事件：点击后添加到验卡区
	     */
		public void AddClick() {
			Fruits.MouseClick += F_MouseClick;
		}

		public void RemoveClick() {
			Fruits.MouseClick -= F_MouseClick;
		}

		public void RemoveImageCantainer() {
			Rectangle visibleRect = Fruits.DisplayRectangle;
			_imageControl.Controls.Remove(Fruits);
			_imageControl.Invalidate(visibleRect);
		}

		public void RemoveCardSlotCantainer() {
			_cardSlotControl.Controls.Remove(Fruits);
		}

		public void SetFlag(bool value) {
			Fruits.Alpha = value ? 1f : 0.65f;
			this.Flag = value;
			Fruits.Invalidate();
		}
	}
}