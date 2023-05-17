using System;
using System.Drawing;
using System.Windows.Forms;
using yanglegeyang.container;

namespace yanglegeyang.components {
	public class FruitObject {
		public static readonly int DefaultWidth = 100;
		public static readonly int DefaultHeight = 100;

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
		public String ImageName { get; set; } //名称，非常重要，安装名称来判断是否为同一类型
		
		private bool flag = true; //是否可以点击

		public bool Flag {
			get => flag;
			set {
				if (this.flag != value) {
					//需要更新透明度
					Fruits.SetAlpha(value ? 1 : 0.65f);
				}
				this.flag = value;
			}
		}

		public bool LeftFold; // 是否为遮挡元素
		
		public bool RightFold; // 是否为遮挡元素
		
		ImageControl imageControl;
		
		CardSlotControl cardSlotControl;

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
			this.cardSlotControl = cardSlotCantainer;
			this.Fruits.Cursor = Cursors.Arrow;
		}

		public void Show(ImageControl imageControl, int initX, int initY) {
			this.imageControl = imageControl;
			// 随机生成开始坐标偏移量，实现上下层错落有致的视觉感
			bool randomWidth = _random.Next(10) % 2 == 0;
			bool randomHeight = _random.Next(10) % 2 == 0;
			int pointX = initX + X * DefaultWidth + (randomWidth ? DefaultWidth / 2 : 0);
			int pointY = initY + Y * DefaultHeight + (randomHeight ? DefaultHeight / 2 : 0);
			// 设置卡片显示在背景面板中位置
			Fruits.Location = new Point(pointX, pointY);
			// 记录卡片的空间信息
			SpaceManager.Rectangle(this);
			imageControl.Controls.Add(Fruits);
			AddClick();
		}

		public void ShowFold(ImageControl imageControl1, int initX, int initY, int offset, bool isLeft) {
			this.imageControl = imageControl1;
			// 随机生成开始坐标偏移量，实现上下层错落有致的视觉感
			int pointX = initX + X * DefaultWidth + offset;
			int pointY = initY + Y * DefaultHeight - DefaultHeight / 4;
			if (isLeft) {
				this.LeftFold = true;
			}
			else {
				this.RightFold = true;
			}

			// 设置卡片显示在背景面板中位置
			Fruits.SetBounds(pointX, pointY, DefaultWidth, DefaultHeight);
			// 记录卡片的空间信息
			SpaceManager.Rectangle(this);
			imageControl.Controls.Add(Fruits);
			AddClick();
		}

		/**
	     * 卡片点击事件：点击后添加到验卡区
	     */
		public void AddClick() {
			Fruits.MouseClick += (sender, e) =>
			{
				if (flag)
				{
					cardSlotControl.AddSlot(this);
				}
			};
		}

		public void RemoveImageCantainer() {
			Rectangle visibleRect = Fruits.DisplayRectangle;
			SpaceManager.RemoveCompontFlag(this);
			imageControl.Controls.Remove(Fruits);
			imageControl.Invalidate(visibleRect);
		}

		public void RemoveCardSlotCantainer() {
			cardSlotControl.Controls.Remove(Fruits);
		}
	}
}