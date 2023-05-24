using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace yanglegeyang {
	public partial class HomeForm : Form {
		
		private WaveOutEvent waveOut;
		public static GameObject Game;
		
		public HomeForm() {
			string filePath = "./static/3.mp3";
			
			waveOut = new WaveOutEvent();
			var mp3FileReader = new Mp3FileReader(filePath);

			waveOut.Init(mp3FileReader);
			
			waveOut.Play();
			InitializeComponent();
			SetFormStyle();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			HomeForm.Game = new GameObject(1);
			this.Hide();
			waveOut.Stop();
			Game.ShowDialog();
		}
		private Point _mPoint;
    

		private void Form2_MouseDown(object sender, MouseEventArgs e)
		{
			_mPoint = new Point(e.X, e.Y);
		}

		private void Form2_MouseMove(object sender, MouseEventArgs e)
		{

			if (e.Button == MouseButtons.Left)
			{
				this.Location = new Point(this.Location.X + e.X - _mPoint.X, this.Location.Y + e.Y - _mPoint.Y);
			}
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void pictureBox2_MouseCaptureChanged(object sender, EventArgs e)
		{

		}

		private void pictureBox2_MouseLeave(object sender, EventArgs e)
		{
			pictureBox2.BackColor = Color.Transparent;
		}

		private void pictureBox2_MouseEnter(object sender, EventArgs e)
		{
			pictureBox2.BackColor = Color.Red;
		}

		private void Form2_Load(object sender, EventArgs e) {
			
		}
		private void SetFormStyle()
		{
			// 隐藏默认的窗口边框
			this.FormBorderStyle = FormBorderStyle.None;

			// 设置窗口的透明色
			this.TransparencyKey = Color.Black;

			// 设置窗口的背景为 3D 效果
			this.BackColor = Color.Black;
			this.Paint += Form2_Paint;
		}

		private void Form2_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.Raised);
		}

	}
}