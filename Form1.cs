using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using yanglegeyang.components;
using yanglegeyang.container;
using yanglegeyang.utils;

namespace yanglegeyang {
	public partial class Form1 : Form {
		// 背景界面
		private ImageControl _imageControl = new ImageControl();

		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		public Form1() {
			AllocConsole();
			_imageControl.Dock = DockStyle.Fill;
			this.Controls.Add(_imageControl);
			InitializeComponent();

			InitContent();
		}


		private const int MaxLevel = 10; //多少层
		private const int MaxWidth = 6; // 跨度个数
		private const int MaxHeight = 5; // 最大宽度
		private int _maxFlop = 60; //翻牌数量;

		public void InitContent() {
			Random random = new Random();
			Console.WriteLine($@"{MaxLevel}层, {MaxWidth}跨, {MaxHeight}宽, {_maxFlop}翻牌");
			List<string> list = ReadResourceUtil.ReadSkin(true);
			int typeSize = list.Count;
			Console.WriteLine($@"种类数量：{typeSize}");
			// 求得每种种类的个数
			int groupNumber = (int) Math.Ceiling((MaxLevel * MaxWidth * MaxHeight + _maxFlop) / (3f * typeSize));
			Console.WriteLine(@"每种组数：" + groupNumber);
			int groupCount = groupNumber * 3;
			Console.WriteLine(@"每种总数：" + groupCount);
			Console.WriteLine(@"共计数量：" + (typeSize * groupCount + _maxFlop));

			// 绘制卡槽
			int initX = 100;
			int initY = 50;

			CardSlotControl cardSlotControl = new CardSlotControl(_imageControl,
				initX + ((MaxWidth - 7) * FruitObject.DefaultWidth) / 2,
				+initY + FruitObject.DefaultHeight * (MaxHeight + 2));

			// 随机生成卡片集合：打乱顺序
			List<FruitObject> fruitObjects = new List<FruitObject>();
			foreach (string tmp in list) {
				try {
					Bitmap bufferedImage = new Bitmap(Image.FromFile(tmp));
					int count = groupCount + (_maxFlop > 0 ? random.Next(_maxFlop) : 0);
					for (int i = 0; i < count; i++) {
						int size = fruitObjects.Count - 1;
						Fruits fruits = new Fruits(_imageControl, bufferedImage, tmp);
						int index = 0;
						if (size > 10) {
							index = random.Next(size);
						}

						fruitObjects.Insert(index, new FruitObject(cardSlotControl, fruits, 0, 0, 0));
						_maxFlop--;
					}
				}
				catch (IOException e) {
					Console.WriteLine(e);
				}
			}

			Console.WriteLine(@"实际数量：" + fruitObjects.Count);
			// 给每个对象设置坐标
			int idx = 0;
			for (int i = 0; i < MaxLevel; i++) {
				for (int x = 0; x < MaxWidth; x++) {
					for (int y = 0; y < MaxHeight; y++) {
						FruitObject fruitObject;
						fruitObject = fruitObjects[idx++];
						fruitObject.X = x;
						fruitObject.Y = y;
						fruitObject.Level = i;
						fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4,
							initY - FruitObject.DefaultHeight / 4);
					}
				}
			}

			Console.WriteLine(@"重叠数量：" + idx);
			// 绘制翻牌区
			int fSize = fruitObjects.Count;
			if (idx < fSize) {
				int step = 5;
				int lenght = fSize - idx;
				for (int i = 0; i < lenght; i++) // 绘制右边
				{
					FruitObject fruitObject = fruitObjects[idx++];
					fruitObject.X = MaxWidth - 1;
					fruitObject.Y = MaxHeight + 1;
					fruitObject.Level = i;
					fruitObject.ShowFold(_imageControl, initX - (lenght * step), initY, i * step, false);
				}

				Console.WriteLine(@"右翻牌区数：" + lenght + "\t" + idx);
				 lenght = (fSize - idx) / 2;
				for (int i = 0; i < lenght; i++) // 绘制左边
				{
					FruitObject fruitObject = fruitObjects[idx++];
					fruitObject.X = 0;
					fruitObject.Y = MaxHeight + 1;
					fruitObject.Level = i;
					fruitObject.ShowFold(_imageControl, initX + (lenght * step), initY, -i * step, false);
				}

				Console.WriteLine(@"左翻牌区数：" + lenght + "\t" + idx);
				
			}
		}

		private void ReDrawImagePanel() {
			int leftX = 15;
			int width = this.Width - leftX - 15;
			int height = this.Height - leftX - 15;
		}
	}
}