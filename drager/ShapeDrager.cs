using System.Drawing;
using System.Windows.Forms;

namespace yanglegeyang.drager {
	public abstract class ShapeDrager : Panel
	{
		// 旋转的角度
		double rotation = 0;
		// 是否图标模式
		bool iconType = false;

		public ShapeDrager() : this(false)
		{
			
		}

		public ShapeDrager(bool icontype)
		{
			this.iconType = icontype;
			this.TabStop = true;
			if (!iconType)
			{
				this.Cursor = Cursors.SizeAll;
			}
			else
			{
				this.Cursor = Cursors.Hand;
			}
			InitShape();
		}

		private void InitShape()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
			this.BackgroundImageLayout = ImageLayout.Stretch;
			this.Width = 80;
			this.Height = 80;
			// 设置双缓冲
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
			// 设置图片居中
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		public double Rotation => rotation;
	}
}