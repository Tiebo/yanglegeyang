using System;
using System.Collections.Generic;
using System.Drawing;

namespace yanglegeyang.components {
	public class OneLevelFruits {
		private List<FruitObject> _levelFruit = new List<FruitObject>();

		public List<FruitObject> LevelFruit {
			get => _levelFruit;
			set => _levelFruit = value;
		}
	}

	public class MySpace {
		public static Queue<FruitObject> FoldQueue = new Queue<FruitObject>();

		public static OneLevelFruits[] AllLevelFruits = new OneLevelFruits[10];

		public static string Fold_update() {
			FoldQueue.Dequeue();
			FoldQueue.Peek().SetFlag(true);
			return "success";
		}

		public static void Add_fold_list(FruitObject fruitObject) {
			FoldQueue.Enqueue(fruitObject);
		}

		public static void Add_level_fruit(FruitObject fruitObject) {
			var level = fruitObject.Level;
			if (Judge_top(fruitObject)) {
				fruitObject.SetFlag(true);
			}

			if (AllLevelFruits[level] == null)
				AllLevelFruits[level] = new OneLevelFruits();
			AllLevelFruits[level].LevelFruit.Add(fruitObject);
		}

		public static void update_flag(FruitObject fruitObject) {
			int level = fruitObject.Level;
			for (int i = level; i < 10; i++) {
				if (AllLevelFruits[level].LevelFruit.Count == 0) {
					break;
				}

				AllLevelFruits[i].LevelFruit.ForEach(v => v.SetFlag(Judge_top(v)));
			}
		}

		public static bool Judge_top(FruitObject fruitObject) {
			int level = fruitObject.Level;
			// 顶层的一定可以被点击
			if (level == 0) return true;
			var bounds = fruitObject.Fruits.Bounds;
			int x1 = bounds.X;
			int y1 = bounds.Y;
			/*
			 *	1 2 3
			 *	4 5 6
			 *	7 8 9
			 */

			var r1 = new Rectangle(new Point(x1, y1), new Size(FruitObject.DefaultWidth, FruitObject.DefaultHeight));

			for (int i = level - 1; i >= 1; i--) {
				if (AllLevelFruits[i] == null) continue;

				foreach (var fruit in AllLevelFruits[i].LevelFruit) {
					var bounds1 = fruit.Fruits.Bounds;
					/*
					 *	1 2 3
					 *	4 5 6
					 *	7 8 9
					 */

					var r2 = new Rectangle(new Point(bounds1.X, bounds1.Y),
						new Size(FruitObject.DefaultWidth, FruitObject.DefaultHeight));
					// 判断矩形碰撞	
					if (r1.IntersectsWith(r2)) {
						return false;
					}
				}
			}

			return true;
		}

		public static void remove_level_fruit(FruitObject fruitObject) {
			var level = fruitObject.Level;
			AllLevelFruits[level].LevelFruit.Remove(fruitObject);
		}
	}
}