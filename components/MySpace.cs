using System;
using System.Collections.Generic;
using System.Drawing;

namespace yanglegeyang.components {
	public class MySpace {
		public static Queue<FruitObject> FoldQueue = new Queue<FruitObject>();

		public static Dictionary<int, List<FruitObject>> AllLevelFruits = new Dictionary<int, List<FruitObject>>();

		public static int AllFruitsLength = 0;
		public static void Fold_update() {
			FoldQueue.Dequeue();
			FoldQueue.Peek().SetFlag(true);
		}

		public static void Add_fold_list(FruitObject fruitObject) {
			FoldQueue.Enqueue(fruitObject);
		}

		public static void Add_level_fruit(FruitObject fruitObject) {
			var level = fruitObject.Level;
			if (!AllLevelFruits.ContainsKey(level))
				AllLevelFruits[level] = new List<FruitObject>();
			
			fruitObject.SetFlag(Judge_top(fruitObject));
			AllLevelFruits[level].Add(fruitObject);
			AllFruitsLength++;
		}

		public static void update_flag(FruitObject fruitObject) {
			int level = fruitObject.Level;

			for (int i = level + 1; i < 10; i++) {
				try {
					if (!AllLevelFruits.ContainsKey(i)) continue;

					AllLevelFruits[i].ForEach(v => {
						var isTop = Judge_top(v);
						if (v.Flag != isTop) {
							v.SetFlag(isTop);
						}
					});
				}
				catch (NullReferenceException e) {
					// ignored
				}
			}
		}

		public static bool Judge_top(FruitObject fruitObject) {
			int level = fruitObject.Level;
			// 顶层的一定可以被点击
			if (level == 0) return true;
			
			var r1 = new Rectangle(fruitObject.Fruits.Location,
				new Size(FruitObject.DefaultWidth, FruitObject.DefaultHeight));

			for (int i = level - 1; i >= 0; i--) {
				if (!AllLevelFruits.ContainsKey(i)) continue;

				foreach (var f in AllLevelFruits[i]) {
					
					var r2 = new Rectangle(f.Fruits.Location,
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
			AllLevelFruits[level].Remove(fruitObject);
			AllFruitsLength--;
		}

		public static int GetLength() {
			return AllFruitsLength;
		}
	}
}