using System;
using System.Drawing;
using System.Windows.Forms;
using yanglegeyang.container;

namespace yanglegeyang.components {
	public class FruitObject {
		public static readonly int DefaultWidth = 80;
		public static readonly int DefaultHeight = 80;

		private static Random _random = new Random();

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
		
		private bool _flag = false; //是否可以点击

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

		public void Show(ImageControl imageControl1, int initX, int initY) {
			this._imageControl = imageControl1;
			// 随机生成开始坐标偏移量，实现上下层错落有致的视觉感
			bool randomWidth = _random.Next(10) % 2 == 0;
			bool randomHeight = _random.Next(10) % 2 == 0;
			int pointX = initX + X * DefaultWidth + (randomWidth ? DefaultWidth / 2 : 0);
			int pointY = initY + Y * DefaultHeight + (randomHeight ? DefaultHeight / 2 : 0);
			// 设置卡片显示在背景面板中位置
			Fruits.SetBounds(pointX, pointY, DefaultWidth, DefaultHeight);
			// 记录卡片的空间信息
			MySpace.Add_level_fruit(this);
			// SpaceManager.Rectangle(this);
			imageControl1.Controls.Add(Fruits);
			AddClick();
		}

		public void ShowFold(ImageControl imageControl1, int initX, int initY, int offset) {
			this._imageControl = imageControl1;
			// 随机生成开始坐标偏移量，实现上下层错落有致的视觉感
			int pointX = initX + X * DefaultWidth + offset;
			int pointY = initY + Y * DefaultHeight - DefaultHeight / 4;
			// 设置卡片显示在背景面板中位置
			Fruits.SetBounds(pointX, pointY, DefaultWidth, DefaultHeight);
			// 记录卡片的空间信息
			MySpace.Add_fold_list(this);
			_imageControl.Controls.Add(Fruits);
			AddClick();
		}

		
		private void F_MouseClick(object sender, MouseEventArgs e) {
			if (_flag)
			{
				_cardSlotControl.AddSlot(this);
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
			if (MySpace.FoldQueue.Contains(this))
				MySpace.Fold_update();
			Rectangle visibleRect = Fruits.DisplayRectangle;
			SpaceManager.RemoveCompontFlag(this);
			_imageControl.Controls.Remove(Fruits);
			_imageControl.Invalidate(visibleRect);
		}

		public void RemoveCardSlotCantainer() {
			_cardSlotControl.Controls.Remove(Fruits);
		}

		public void SetFlag(bool value) {
			Fruits.Alpha = value ? 1f : 0.6f;
			this._flag = value;
		}
	}
}