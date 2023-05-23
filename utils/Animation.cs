using System;
using System.Drawing;
using System.Windows.Forms;
using yanglegeyang.components;

namespace yanglegeyang.utils
{
	public class Animation
	{
		private Control control;
		private Timer timer;
		private int steps;
		private int currentStep;
		private int _frameRate;
		private Point start;
		private Point end;

		public Animation(Control control, Point start, Point end, int duration)
		{
			this.control = control;
			this.steps = duration * 60 / 1000;
			this.currentStep = 0;
			this.start = start;
			this.end = end;
			this.timer = new Timer();

			this.timer.Interval = 1000 / 60;
			this.timer.Tick += Timer_Tick;
		}

		public void Start()
		{
			// control.SetBounds(start.X, start.Y, FruitObject.DefaultWidth, FruitObject.DefaultHeight); 
			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			currentStep++;
			if (currentStep > steps)
			{
				timer.Stop();
				control.SetBounds(end.X, end.Y, FruitObject.DefaultWidth, FruitObject.DefaultHeight);
			}
			else
			{
				int x = start.X + (end.X - start.X) * currentStep / steps;
				int y = start.Y + (end.Y - start.Y) * currentStep / steps;
				control.SetBounds(x, y, FruitObject.DefaultWidth, FruitObject.DefaultHeight); 
			}
		}
	}
}