using System;
using System.Drawing;

namespace yanglegeyang {
	public class Test {
		public static void TestMain() {
			Rectangle r1 = new Rectangle(new Point(160, 70), new Size(80, 80));
			Rectangle r2 = new Rectangle(new Point(120, 110), new Size(80, 80));

			Console.WriteLine(r1.IntersectsWith(r2));
		}
	}
}