using System.Collections.Generic;
using System.Drawing;

namespace yanglegeyang.components {
	public class SpaceManager {
		//Dictionary<X坐标点, Dictionary<层数, LIst<Y坐标点Integer, FruitObject>>>
        static Dictionary<int, Dictionary<int, List<FruitObject>>> _leavlDataX1 = new Dictionary<int, Dictionary<int, List<FruitObject>>>();
        static Dictionary<int, Dictionary<int, List<FruitObject>>> _leavlDataX2 = new Dictionary<int, Dictionary<int, List<FruitObject>>>();
        static Dictionary<int, Dictionary<int, List<FruitObject>>> _leavlDataX3 = new Dictionary<int, Dictionary<int, List<FruitObject>>>();
        static Dictionary<int, Dictionary<int, List<FruitObject>>> _leavlDataY1 = new Dictionary<int, Dictionary<int, List<FruitObject>>>();
        static Dictionary<int, Dictionary<int, List<FruitObject>>> _leavlDataY2 = new Dictionary<int, Dictionary<int, List<FruitObject>>>();
        static Dictionary<int, Dictionary<int, List<FruitObject>>> _leavlDataY3 = new Dictionary<int, Dictionary<int, List<FruitObject>>>();

        /// <summary>
        /// 记录组件坐标重叠数据
        /// </summary>
        public static void Rectangle(FruitObject fruitObject)
        {
            Rectangle bounds = fruitObject.Fruits.Bounds;
            if (fruitObject.LeftFold)
            {
                bounds.X = 0; bounds.Y = 0;
            }
            if (fruitObject.RightFold)
            {
                bounds.X = 1; bounds.Y = 1;
            }

            int x1 = bounds.X;
            int x2 = bounds.X + FruitObject.DefaultWidth / 2;
            int x3 = bounds.X + FruitObject.DefaultHeight;
            int y1 = bounds.Y;
            int y2 = bounds.Y + FruitObject.DefaultHeight / 2;
            int y3 = bounds.Y + FruitObject.DefaultHeight;

            PutToCache(x1, fruitObject, _leavlDataX1);
            PutToCache(x2, fruitObject, _leavlDataX2);
            PutToCache(x3, fruitObject, _leavlDataX3);
            PutToCache(y1, fruitObject, _leavlDataY1);
            PutToCache(y2, fruitObject, _leavlDataY2);
            PutToCache(y3, fruitObject, _leavlDataY3);

            UpdateCompontFlag(fruitObject, false, bounds);
        }

        private static void PutToCache(int point, FruitObject fruitObject, Dictionary<int, Dictionary<int, List<FruitObject>>> LEAVL_DATA)
        {
            Dictionary<int, List<FruitObject>> hashLevel;
            LEAVL_DATA.TryGetValue(fruitObject.Level, out hashLevel);

            if (hashLevel == null)
            {
                hashLevel = new Dictionary<int, List<FruitObject>>();
            }

            List<FruitObject> objects;
            hashLevel.TryGetValue(point, out objects);

            if (objects == null)
            {
                objects = new List<FruitObject>();
            }

            objects.Add(fruitObject);
            hashLevel[point] = objects;
            LEAVL_DATA[fruitObject.Level] = hashLevel;
        }

        /**
         * 消除卡片时，计算下一层点亮的卡片
         * @param fruitObject
         */
        public static void RemoveCompontFlag(FruitObject fruitObject)
        {
            Rectangle bounds = fruitObject.Fruits.Bounds;
            if (fruitObject.LeftFold)
            {
                bounds.X = 0;
                bounds.Y = 0;
            }
            if (fruitObject.RightFold)
            {
                bounds.X = 1;
                bounds.Y = 1;
            }
            int x1 = bounds.X;
            int x2 = bounds.X + FruitObject.DefaultWidth / 2;
            int x3 = bounds.X + FruitObject.DefaultHeight;
            int y1 = bounds.Y;
            int y2 = bounds.Y + FruitObject.DefaultHeight / 2;
            int y3 = bounds.Y + FruitObject.DefaultHeight;
            DeleteLevelFlag(x1, fruitObject, _leavlDataX1);
            DeleteLevelFlag(x2, fruitObject, _leavlDataX2);
            DeleteLevelFlag(x3, fruitObject, _leavlDataX3);
            DeleteLevelFlag(y1, fruitObject, _leavlDataY1);
            DeleteLevelFlag(y2, fruitObject, _leavlDataY2);
            DeleteLevelFlag(y3, fruitObject, _leavlDataY3);
            UpdateCompontFlag(fruitObject, true, bounds);
        }
        private static void DeleteLevelFlag(int point, FruitObject fruitObject, Dictionary<int, Dictionary<int, List<FruitObject>>> LEAVL_DATA)
        {
            Dictionary<int, List<FruitObject>> hashLevel = null;
            if (LEAVL_DATA.TryGetValue(fruitObject.Level, out hashLevel))
            {
                List<FruitObject> objects = null;
                if (hashLevel.TryGetValue(point, out objects))
                {
                    objects.Remove(fruitObject);
                    if (objects.Count == 0)
                    {
                        hashLevel.Remove(point);
                        if (hashLevel.Count == 0)
                        {
                            LEAVL_DATA.Remove(fruitObject.Level);
                        }
                        else
                        {
                            LEAVL_DATA[fruitObject.Level] = hashLevel;
                        }
                    }
                }
                else
                {
                    hashLevel.Remove(point);
                    if (hashLevel.Count == 0)
                    {
                        LEAVL_DATA.Remove(fruitObject.Level);
                    }
                    else
                    {
                        LEAVL_DATA[fruitObject.Level] = hashLevel;
                    }
                }
            }
        }

        /**
         * 更新底层节点的状态,false修改为不可点，反之可点
         * @param fruitObject
         */
        private static void UpdateCompontFlag(FruitObject fruitObject, bool flag, Rectangle bounds)
        {
            int level = fruitObject.Level;
            int x1 = bounds.X;
            int x2 = bounds.X + FruitObject.DefaultWidth / 2;
            int x3 = bounds.X + FruitObject.DefaultHeight;
            int y1 = bounds.Y;
            int y2 = bounds.Y + FruitObject.DefaultHeight / 2;
            int y3 = bounds.Y + FruitObject.DefaultHeight;
            for (int i = 0; i < level; i++)
            {
                UpdateLevelFlag(x1, x2, x3, y1, y2, y3, x1, i, _leavlDataX1, flag);
                UpdateLevelFlag(x1, x2, x3, y1, y2, y3, x2, i, _leavlDataX2, flag);
                UpdateLevelFlag(x1, x2, x3, y1, y2, y3, x3, i, _leavlDataX3, flag);
                UpdateLevelFlag(x1, x2, x3, y1, y2, y3, y1, i, _leavlDataY1, flag);
                UpdateLevelFlag(x1, x2, x3, y1, y2, y3, y2, i, _leavlDataY2, flag);
                UpdateLevelFlag(x1, x2, x3, y1, y2, y3, y3, i, _leavlDataY3, flag);
            }
        }

        /***
         * 
         * 方法是用于更新指定层级和坐标的卡片对象的状态的
         * 
         */
        private static void UpdateLevelFlag(int x1, int x2, int x3, int y1, int y2, int y3, int point, int level, Dictionary<int, Dictionary<int, List<FruitObject>>> LEAVL_DATA, bool flag)
        {
            Dictionary<int, List<FruitObject>> hashLevel = null;
            if (LEAVL_DATA.TryGetValue(level, out hashLevel))
            {
                List<FruitObject> objects = null;
                if (hashLevel.TryGetValue(point, out objects))
                {
                    if (flag) // 当更新的时候需要强制检查
                    {
                        flag = !LEAVL_DATA.ContainsKey(level + 1); // 上一层都没了，可以点击
                    }
                    // 上一层存在，但是没有遮挡可以点击
                    if (!flag)
                    {
                        flag = !(IsExsit(x1, level + 1, _leavlDataX1)
                            && IsExsit(x2, level + 1, _leavlDataX2)
                            && IsExsit(x3, level + 1, _leavlDataX3)
                            && IsExsit(y1, level + 1, _leavlDataY1)
                            && IsExsit(y2, level + 1, _leavlDataY2)
                            && IsExsit(y3, level + 1, _leavlDataY3));
                    }
                    foreach (var obj in objects)
                    {
                        obj.Flag = flag;
                    }
                }
            }
        }
        /***
         * 判断指定层级和坐标是否存在对象
         */
        private static bool IsExsit(int point, int level, Dictionary<int, Dictionary<int, List<FruitObject>>> LEAVL_DATA)
        {
            Dictionary<int, List<FruitObject>> hashLevel = null;
            if (LEAVL_DATA.TryGetValue(level, out hashLevel))
            {
                List<FruitObject> objects = null;
                if (hashLevel.TryGetValue(point, out objects))
                {
                    return objects != null && objects.Count > 0;
                }
            }
            return false;
        }
	}
}