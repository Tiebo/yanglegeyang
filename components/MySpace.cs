using System;
using System.Collections.Generic;

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

		public static void init_level_fruit() {
			var one = AllLevelFruits[9];
			foreach (var fruit in one.LevelFruit) {
				fruit.SetFlag(true);
			}
		}
		
		public static bool Judge_top(FruitObject fruitObject) {
			int level = fruitObject.Level;
			// 顶层的一定可以被点击
			if (level == 1) return true;
			
			int x = fruitObject.X;
			int y = fruitObject.Y;
			
			return false;
		}
	}
}