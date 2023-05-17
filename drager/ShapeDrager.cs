using System.Drawing;
using System.Windows.Forms;

namespace yanglegeyang.drager {
	public abstract class ShapeDrager : Panel
	{
		// 旋转的角度
		double rotation = 0;
		// 是否图标模式
		bool iconType = false;

		public ShapeDrager(Control parent) : this(parent, false)
		{
			
		}

		public ShapeDrager(Control parent, bool icontype)
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
		}

		public double Rotation => rotation;
	}
}