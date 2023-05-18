using System;
using System.Drawing;
using System.Windows.Forms;
using yanglegeyang.drager;
using System.Drawing.Drawing2D;

namespace yanglegeyang.components {
	/// <summary>
	/// 卡片对象
	/// </summary>
	public class Fruits : ShapeDrager {
		// 卡片上放置的图形
		private Image _image;
		// 卡片名称
		public string ImageName { get; set; }
		// 透明度
		private float _alpha = 0.65f;

		public float Alpha {
			get => _alpha;
			set => _alpha = value;
		}
		
		private Color _bgColor = Color.FromArgb(138, 158, 58);

		public Fruits(Image image, string imageName) {
			this._image = image;
			this.ImageName = imageName;
		}

		protected override void OnPaint(PaintEventArgs e) {
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Pen pen = new Pen(Color.Black, 2);
			g.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
			

			Matrix matrix = new Matrix();
			matrix.RotateAt((float) (Rotation * 180 / Math.PI), new PointF(Width / 2f, Height / 2f));
			g.Transform = matrix;
			// 绘制底色
			g.FillRectangle(new SolidBrush(_bgColor), new Rectangle(0, 0, Width, Height));
			// 绘制白色
			g.FillRectangle(Brushes.White, new Rectangle(0, 0, Width, Height - 5));
			// 绘制外边框
			g.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
			// 绘制图片
			if (_image != null) {
				int imageWidth = _image.Width;
				int imageHeight = _image.Height;
				int x = (Width - imageWidth) / 2;
				int y = (Height - 5 - imageHeight) / 2;
				g.DrawImage(_image, new Rectangle(x, y, imageWidth, imageHeight));
			}
			if (_alpha < 1) {
				g.CompositingMode = CompositingMode.SourceOver;
				g.CompositingQuality = CompositingQuality.HighQuality;
				
				g.FillRectangle(new SolidBrush(Color.FromArgb((int) Math.Round(Alpha * 255), Color.Gray)),
					ClientRectangle);
			}
			base.OnPaint(e);
		}

	}
}