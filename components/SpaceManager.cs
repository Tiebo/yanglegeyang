using System;
using System.Collections.Generic;
using System.Drawing;

namespace yanglegeyang.components {
	public class SpaceManager {
		//Dictionary<X坐标点, Dictionary<层数, LIst<Y坐标点Integer, FruitObject>>>
		private static readonly Dictionary<int, Dictionary<int, List<FruitObject>>> LeavlDataX1 =
			new Dictionary<int, Dictionary<int, List<FruitObject>>>();

		private static readonly Dictionary<int, Dictionary<int, List<FruitObject>>> LeavlDataX2 =
			new Dictionary<int, Dictionary<int, List<FruitObject>>>();

		private static readonly Dictionary<int, Dictionary<int, List<FruitObject>>> LeavlDataX3 =
			new Dictionary<int, Dictionary<int, List<FruitObject>>>();

		private static readonly Dictionary<int, Dictionary<int, List<FruitObject>>> LeavlDataY1 =
			new Dictionary<int, Dictionary<int, List<FruitObject>>>();

		private static readonly Dictionary<int, Dictionary<int, List<FruitObject>>> LeavlDataY2 =
			new Dictionary<int, Dictionary<int, List<FruitObject>>>();

		private static readonly Dictionary<int, Dictionary<int, List<FruitObject>>> LeavlDataY3 =
			new Dictionary<int, Dictionary<int, List<FruitObject>>>();

		/// <summary>
		/// 记录组件坐标重叠数据
		/// </summary>
		public static void Rectangle(FruitObject fruitObject) {
			Rectangle bounds = fruitObject.Fruits.Bounds;
			
			int x1 = fruitObject.X;
			int x2 = fruitObject.X + FruitObject.DefaultWidth / 2;
			int x3 = fruitObject.X + FruitObject.DefaultHeight;
			int y1 = fruitObject.Y;
			int y2 = fruitObject.Y + FruitObject.DefaultHeight / 2;
			int y3 = fruitObject.Y + FruitObject.DefaultHeight;

			PutToCache(x1, fruitObject, LeavlDataX1);
			PutToCache(x2, fruitObject, LeavlDataX2);
			PutToCache(x3, fruitObject, LeavlDataX3);
			PutToCache(y1, fruitObject, LeavlDataY1);
			PutToCache(y2, fruitObject, LeavlDataY2);
			PutToCache(y3, fruitObject, LeavlDataY3);

			UpdateCompontFlag(fruitObject, false, bounds);
		}

		private static void PutToCache(int point, FruitObject fruitObject,
			Dictionary<int, Dictionary<int, List<FruitObject>>> leavlData) {
			Dictionary<int, List<FruitObject>> hashLevel;
			leavlData.TryGetValue(fruitObject.Level, out hashLevel);

			if (hashLevel == null) {
				hashLevel = new Dictionary<int, List<FruitObject>>();
			}

			List<FruitObject> objects;
			hashLevel.TryGetValue(point, out objects);

			if (objects == null) {
				objects = new List<FruitObject>();
			}

			objects.Add(fruitObject);
			hashLevel[point] = objects;
			leavlData[fruitObject.Level] = hashLevel;
		}

		/**
         * 消除卡片时，计算下一层点亮的卡片
         * @param fruitObject
         */
		public static void RemoveCompontFlag(FruitObject fruitObject) {
			int x1 = fruitObject.X;
			int x2 = fruitObject.X + FruitObject.DefaultWidth / 2;
			int x3 = fruitObject.X + FruitObject.DefaultHeight;
			int y1 = fruitObject.Y;
			int y2 = fruitObject.Y + FruitObject.DefaultHeight / 2;
			int y3 = fruitObject.Y + FruitObject.DefaultHeight;
			DeleteLevelFlag(x1, fruitObject, LeavlDataX1);
			DeleteLevelFlag(x2, fruitObject, LeavlDataX2);
			DeleteLevelFlag(x3, fruitObject, LeavlDataX3);
			DeleteLevelFlag(y1, fruitObject, LeavlDataY1);
			DeleteLevelFlag(y2, fruitObject, LeavlDataY2);
			DeleteLevelFlag(y3, fruitObject, LeavlDataY3);

			UpdateCompontFlag(fruitObject, true, new Rectangle());
		}

		private static void DeleteLevelFlag(int point, FruitObject fruitObject,
			Dictionary<int, Dictionary<int, List<FruitObject>>> leavlData) {
			if (!leavlData.TryGetValue(fruitObject.Level, out var hashLevel)) return;
			if (hashLevel.TryGetValue(point, out var objects)) {
				objects.Remove(fruitObject);
				if (objects.Count != 0) return;

				hashLevel.Remove(point);
				if (hashLevel.Count == 0) {
					leavlData.Remove(fruitObject.Level);
				}
				else {
					leavlData[fruitObject.Level] = hashLevel;
				}
			}
			else {
				hashLevel.Remove(point);
				if (hashLevel.Count == 0) {
					leavlData.Remove(fruitObject.Level);
				}
				else {
					leavlData[fruitObject.Level] = hashLevel;
				}
			}
		}

		/**
         * 更新底层节点的状态,false修改为不可点，反之可点
         * @param fruitObject
         */
		private static void UpdateCompontFlag(FruitObject fruitObject, bool flag, Rectangle bounds) {
			int level = fruitObject.Level;
			int x1 = fruitObject.X;
			int x2 = fruitObject.X + FruitObject.DefaultWidth / 2;
			int x3 = fruitObject.X + FruitObject.DefaultHeight;
			int y1 = fruitObject.Y;
			int y2 = fruitObject.Y + FruitObject.DefaultHeight / 2;
			int y3 = fruitObject.Y + FruitObject.DefaultHeight;
			for (int i = 0; i < level; i++) {
				UpdateLevelFlag(x1, x2, x3, y1, y2, y3, x1, i, LeavlDataX1, flag);
				UpdateLevelFlag(x1, x2, x3, y1, y2, y3, x2, i, LeavlDataX2, flag);
				UpdateLevelFlag(x1, x2, x3, y1, y2, y3, x3, i, LeavlDataX3, flag);
				UpdateLevelFlag(x1, x2, x3, y1, y2, y3, y1, i, LeavlDataY1, flag);
				UpdateLevelFlag(x1, x2, x3, y1, y2, y3, y2, i, LeavlDataY2, flag);
				UpdateLevelFlag(x1, x2, x3, y1, y2, y3, y3, i, LeavlDataY3, flag);
			}
		}

		/***
		 * 
		 * 方法是用于更新指定层级和坐标的卡片对象的状态的
		 * 
		 */
		private static void UpdateLevelFlag(int x1, int x2, int x3, int y1, int y2, int y3, int point, int level,
			Dictionary<int, Dictionary<int, List<FruitObject>>> leavlData, bool flag) {
			if (!leavlData.TryGetValue(level, out var hashLevel)) return;

			if (!hashLevel.TryGetValue(point, out var objects)) return;

			if (flag) // 当更新的时候需要强制检查
			{
				flag = !leavlData.ContainsKey(level + 1); // 上一层都没了，可以点击
			}
			else {
				// 上一层存在，但是没有遮挡可以点击
				flag = !(IsExsit(x1, level + 1, LeavlDataX1)
				         && IsExsit(x2, level + 1, LeavlDataX2)
				         && IsExsit(x3, level + 1, LeavlDataX3)
				         && IsExsit(y1, level + 1, LeavlDataY1)
				         && IsExsit(y2, level + 1, LeavlDataY2)
				         && IsExsit(y3, level + 1, LeavlDataY3));
			}

			foreach (var obj in objects) {
				obj.SetFlag(flag);
			}
		}

		/***
		 * 判断指定层级和坐标是否存在对象
		 */
		private static bool IsExsit(int point, int level,
			Dictionary<int, Dictionary<int, List<FruitObject>>> leavlData) {
			if (!leavlData.TryGetValue(level, out var hashLevel)) return false;

			if (hashLevel.TryGetValue(point, out var objects)) {
				return objects != null && objects.Count > 0;
			}

			return false;
		}
	}
}