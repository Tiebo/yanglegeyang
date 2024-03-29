﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace yanglegeyang.components {
	/// <summary>
	/// 卡片对象
	/// </summary>
	public class Fruits : Panel {
		// 卡片上放置的图形
		public Image Image;

		// 卡片名称
		public string ImageName { get; set; }

		// 透明度
		private float _alpha = 0.65f;

		public bool IsSlot { get; set; }
		
		public bool IsMove { get; set; }

		public float Alpha {
			get => _alpha;
			set => _alpha = value;
		}

		private Color _bgColor = Color.FromArgb(138, 158, 58);

		public Fruits(Image image, string imageName) {
			this.Image = image;
			this.ImageName = imageName;
			InitShape();
		}

		private void InitShape() {
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
			this.BackgroundImageLayout = ImageLayout.Stretch;
			this.Width = 80;
			this.Height = 80;
			// 设置双缓冲
			this.SetStyle(
				ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint,
				true);
			this.DoubleBuffered = true;
			// 设置图片居中
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		protected override void OnPaint(PaintEventArgs e) {
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Pen pen = new Pen(Color.Black, 2);

			g.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
			// 绘制底色
			g.FillRectangle(new SolidBrush(_bgColor), new Rectangle(0, 0, Width, Height));
			// 绘制白色
			g.FillRectangle(Brushes.White, new Rectangle(0, 0, Width, Height - 5));
			// 绘制外边框
			g.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
			// 绘制图片
			if (Image != null) {
				int imageWidth = Image.Width;
				int imageHeight = Image.Height;
				int x = (Width - imageWidth) / 2;
				int y = (Height - 5 - imageHeight) / 2;
				g.DrawImage(Image, new Rectangle(x, y, imageWidth, imageHeight));
			}

			if (_alpha < 1) {
				g.FillRectangle(new SolidBrush(Color.FromArgb((int) Math.Round(Alpha * 255), Color.Gray)),
					new Rectangle(0, 0, Width, Height - 5));
			}

			base.OnPaint(e);
		}

		protected override void OnMouseHover(EventArgs e) {
			if (Math.Abs(this._alpha - 1f) > 0.1) return;
			if (IsSlot) return;
			
			this.Width = 90;
			this.Height = 90;
			Invalidate();
			base.OnMouseHover(e);
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			if (Math.Abs(this._alpha - 1f) > 0.1) return;
			if (IsSlot) return;
			
			this.Width = 90;
			this.Height = 90;
			Invalidate();
			base.OnMouseHover(e);
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e) {
			if (Math.Abs(this._alpha - 1f) > 0.1) return;
			if (IsSlot) return;
			
			this.Width = 80;
			this.Height = 80;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.SuspendLayout();
			// 
			// Fruits
			// 
			this.BackColor = System.Drawing.Color.Transparent;
			this.ResumeLayout(false);
		}
	}
}