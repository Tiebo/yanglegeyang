using System;
using System.Drawing;
using System.Windows.Forms;

namespace yanglegeyang.utils {
	public class AnimationHelper {
		private Timer _animationTimer = new Timer();
		private double _velocity = 0.0;
		private Point _location = Point.Empty;
		private double _force = 0.01; //0.69;
		private double _drag = 0.8;
		private Control _control;
		//private AxShockwaveFlash control;
		private int _targetPos=0;

		public AnimationHelper(int interval=5)
		{
			_animationTimer.Interval = interval;
			_animationTimer.Tick += delegate
			{
				if (_control == null) return;
				int currentPos = _control.Location.X;
				if (Math.Abs(currentPos - _targetPos) <= 5)
				{
					_control.Location = new Point(_targetPos, _control.Location.Y);
					_animationTimer.Stop();
				}
				int dx = _targetPos - currentPos;
				_velocity += _force * dx;
				_velocity *= _drag;
				if (Math.Abs(_velocity) < 5)
				{
					if (_velocity > 0)
						_velocity = 5;
					else
						_velocity = -5;
				}
				_control.Location = new Point(currentPos + (int)_velocity, _control.Location.Y);
			};
			_animationTimer.Start();
		}

		public void MoveXEx(Control control, int targetPos, Action completeWith = null)
		{
			this._control = control;
			this._targetPos = targetPos;
			_velocity = 0;
			_animationTimer.Start();
		}
	}
}