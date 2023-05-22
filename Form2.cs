using System;
using System.Drawing;
using System.Windows.Forms;

namespace yanglegeyang {
	public partial class Form2 : Form {
		public Form2() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Form1 frm = new Form1();
			this.Hide();
			frm.ShowDialog();
		}
		private Point mPoint;
    

		private void Form2_MouseDown(object sender, MouseEventArgs e)
		{
			mPoint = new Point(e.X, e.Y);
		}

		private void Form2_MouseMove(object sender, MouseEventArgs e)
		{

			if (e.Button == MouseButtons.Left)
			{
				this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
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
	}
}