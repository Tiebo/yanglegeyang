using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System;

namespace yanglegeyang.container {
	public partial class ImageControl : Panel, IScaleFunction {
		// Backgroud color
		Color _backgroundColor = Color.White;

		// Spacing
		int _step = 40;

		// Scaling, each step +-5
		int _scale = 100;

		// Background color
		Color _bgColor = Color.FromArgb(195, 254, 139);

		// Background icon color
		Color _grassColor = Color.FromArgb(95, 154, 39);
		
		Random _random = new Random();

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
			g.ScaleTransform((float) _scale / 100, (float) _scale / 100);

			g.Clear(_backgroundColor);
			DrawX(g);

			g.Transform = transformMatrix;
		}

		private void ClearPane(Graphics g) {
			g.Clear(this.BackColor);
		}

		public void Reset() {
			_scale = 100;
			Scale(0);
		}

		public void Scale(int step1) {
			_scale += step1;
			if (_scale < 20) {
				_scale = 20;
			}

			if (_scale > 500) {
				_scale = 500;
			}

			ApplyZoom();
		}

		private void ApplyZoom() {
			this.Size = new Size((int) (_scale / 100f * this.Width), (int) (_scale / 100f * this.Height));
			this.Invalidate();
		}

		private void DrawX(Graphics g) {
			int size = 20;
			int width = (int) (this.Width / (_scale / 100f));
			int height = (int) (this.Height / (_scale / 100f));
			int skip = (int) (_step * _scale / 100f);
			Graphics graphics = g;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			// Background color
			graphics.Clear(_bgColor);

			// Set pen color and size
			Pen pen = new Pen(_grassColor, 2);

			// Randomly draw different-sized shapes
			for (int i = 0; i < width; i += skip) {
				graphics.DrawRectangle(pen, _random.Next(width), _random.Next(height), _random.Next(size),
					_random.Next(size));
				
				try {
					graphics.DrawArc(pen, _random.Next(width), _random.Next(height), 15, 15,
						_random.Next(360), _random.Next(360));
				}
				catch (Exception e) {
					Console.WriteLine(e);
					throw;
				}
				graphics.DrawEllipse(pen, _random.Next(width), _random.Next(height), _random.Next(size),
					_random.Next(size));
				DrawRoundedRect(graphics, pen, _random.Next(width), _random.Next(height), _random.Next(size),
					_random.Next(size), _random.Next(10, 50));
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