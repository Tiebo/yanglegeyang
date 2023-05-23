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

		public static void Fold_update() {
			FoldQueue.Dequeue();
			FoldQueue.Peek().SetFlag(true);
		}

		public static void Add_fold_list(FruitObject fruitObject) {
			FoldQueue.Enqueue(fruitObject);
		}

		public static void Add_level_fruit(FruitObject fruitObject) {
			var level = fruitObject.Level;

			if (AllLevelFruits[level] == null)
				AllLevelFruits[level] = new OneLevelFruits();


			fruitObject.SetFlag(Judge_top(fruitObject));
			AllLevelFruits[level].LevelFruit.Add(fruitObject);
		}

		public static void update_flag(FruitObject fruitObject) {
			int level = fruitObject.Level;

			for (int i = level + 1; i < 10; i++) {
				try {
					if (AllLevelFruits[i].LevelFruit.Count == 0) {
						break;
					}

					AllLevelFruits[i].LevelFruit.ForEach(v => {
						var isTop = Judge_top(v);
						if (v.Flag != isTop) {
							v.SetFlag(isTop);
						}
					});
				}
				catch (Exception e) {
					// ignored
				}
			}
		}

		public static bool Judge_top(FruitObject fruitObject) {
			int level = fruitObject.Level;
			// 顶层的一定可以被点击
			if (level == 0) return true;
			/*
			 *	1 2 3
			 *	4 5 6
			 *	7 8 9
			 */
			var r1 = new Rectangle(fruitObject.Fruits.Location,
				new Size(FruitObject.DefaultWidth, FruitObject.DefaultHeight));

			for (int i = level - 1; i >= 0; i--) {
				if (AllLevelFruits[i] == null) continue;

				foreach (var fruit in AllLevelFruits[i].LevelFruit) {
					/*
					 *	1 2 3
					 *	4 5 6
					 *	7 8 9
					 */
					var r2 = new Rectangle(fruit.Fruits.Location,
						new Size(FruitObject.DefaultWidth, FruitObject.DefaultHeight));
					// 判断矩形碰撞
					if (r1.IntersectsWith(r2)) {
						return false;
					}

					if (r1.Contains(r2)) {
						return false;
					}
				}
			}

			return true;
		}

		public static void remove_level_fruit(FruitObject fruitObject) {
			var level = fruitObject.Level;
			AllLevelFruits[level].LevelFruit.Remove(fruitObject);
			Console.WriteLine($@"删除了第{level}层, 坐标：{fruitObject.Fruits.Location}");
		}

		public static int GetNumber() {
			int sum = 0;
			foreach (var one in AllLevelFruits) {
				if (one?.LevelFruit == null) continue;

				sum += one.LevelFruit.Count;
			}

			return sum;
		}
	}
}