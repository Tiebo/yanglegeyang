using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System;

namespace yanglegeyang.container {
	public partial class ImageControl : Panel, IScaleFunction {
		// Backgroud color
		Color backgroundColor = Color.White;

		// Spacing
		int step = 40;

		// Scaling, each step +-5
		int scale = 100;

		// Background color
		Color bgColor = Color.FromArgb(195, 254, 139);

		// Background icon color
		Color grassColor = Color.FromArgb(95, 154, 39);
		Random random = new Random();

		public ImageControl() {
			this.BorderStyle = BorderStyle.None;
			this.Margin = new Padding(20);
			this.BackColor = Color.White;
		}

		protected override void OnPaint(PaintEventArgs e) {
			Graphics g = e.Graphics;
			base.OnPaint(e);

			ClearPane(g);

			// Apply scaling factor
			Matrix transformMatrix = g.Transform.Clone();
			g.ScaleTransform((float) scale / 100, (float) scale / 100);

			g.Clear(backgroundColor);
			DrawX(g);

			g.Transform = transformMatrix;
		}

		private void ClearPane(Graphics g) {
			g.Clear(this.BackColor);
		}

		public void Reset() {
			scale = 100;
			Scale(0);
		}

		public void Scale(int step) {
			scale += step;
			if (scale < 20) {
				scale = 20;
			}

			if (scale > 500) {
				scale = 500;
			}

			ApplyZoom();
		}

		private void ApplyZoom() {
			this.Size = new Size((int) (scale / 100f * this.Width), (int) (scale / 100f * this.Height));
			this.Invalidate();
		}

		private void DrawX(Graphics g) {
			int size = 20;
			int width = (int) (this.Width / (scale / 100f));
			int height = (int) (this.Height / (scale / 100f));
			int skip = (int) (step * scale / 100f);
			Graphics graphics = g;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			// Background color
			graphics.Clear(bgColor);

			// Set pen color and size
			Pen pen = new Pen(grassColor, 2);

			// Randomly draw different-sized shapes
			for (int i = 0; i < width; i += skip) {
				graphics.DrawRectangle(pen, random.Next(width), random.Next(height), random.Next(size),
					random.Next(size));
				
				try {
					graphics.DrawArc(pen, random.Next(width), random.Next(height), 15, 15,
						random.Next(360), random.Next(360));
				}
				catch (Exception e) {
					Console.WriteLine(e);
					throw;
				}
				graphics.DrawEllipse(pen, random.Next(width), random.Next(height), random.Next(size),
					random.Next(size));
				DrawRoundedRect(graphics, pen, random.Next(width), random.Next(height), random.Next(size),
					random.Next(size), random.Next(10, 50));
			}
		}

		private void DrawRoundedRect(Graphics g, Pen pen, float x, float y, float width, float height, float radius) {
			float diameter = radius * 2;
			SizeF sizeF = new SizeF(diameter, diameter);
			RectangleF arc = new RectangleF(x, y, sizeF.Width, sizeF.Height);
			g.DrawArc(pen, arc, 180, 90);
			arc.X += width - diameter;
			g.DrawArc(pen, arc, 270, 90);
			arc.Y += height - diameter;
			g.DrawArc(pen, arc, 0, 90);
			arc.X -= width - diameter;
			g.DrawArc(pen, arc, 90, 90);
			g.DrawLine(pen, x + radius, y, x + width - radius, y);
			g.DrawLine(pen, x + radius, y + height, x + width - radius, y + height);
			g.DrawLine(pen, x, y + radius, x, y + height - radius);
			g.DrawLine(pen, x + width, y + radius, x + width, y + height - radius);
		}
	}
}