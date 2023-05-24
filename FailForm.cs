using System;
using System.Drawing;
using System.Windows.Forms;

namespace yanglegeyang {
	public partial class FailForm : Form {
		public FailForm() {
			InitializeComponent();
		}
		private void fail_Load(object sender, EventArgs e)
        {

        }
        public int num=0;
        //public bool isButtonClicked;

        public void button1_Click(object sender, EventArgs e)
        {
            
            axWindowsMediaPlayer1.Visible = true;
            // 设置视频文件的路径
            string videoPath = "./static/1.mp4";

            // 隐藏其他控件
            foreach (Control control in this.Controls)
            {
                if (control != axWindowsMediaPlayer1)
                {
                    control.Visible = false;
                }
            }
            // 设置 AxWindowsMediaPlayer 控件的背景颜色与父容器背景颜色相同
            axWindowsMediaPlayer1.uiMode = "none"; // 隐藏默认的播放器界面
            axWindowsMediaPlayer1.BackColor = this.BackColor;

            axWindowsMediaPlayer1.stretchToFit = true;
            // 设置 AxWindowsMediaPlayer 控件的位置和大小
            axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            axWindowsMediaPlayer1.Size = this.ClientSize;
           


            // 播放视频
            axWindowsMediaPlayer1.URL = videoPath;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.PlayStateChange += (s, args) =>
            {
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    // 视频播放完成后关闭窗体
                    this.Close();
                }
            };
            
        }

        public void button2_Click(object sender, EventArgs e)
        {
            //isButtonClicked=true;
            Application.Exit();

        }
        private Point _mPoint;


        private void fail_MouseDown(object sender, MouseEventArgs e)
        {
            _mPoint = new Point(e.X, e.Y);
        }

        private void fail_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - _mPoint.X, this.Location.Y + e.Y - _mPoint.Y);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
	}
}