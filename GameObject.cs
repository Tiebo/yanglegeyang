using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NAudio.Wave;
using yanglegeyang.components;
using yanglegeyang.container;
using yanglegeyang.utils;

namespace yanglegeyang {
	public partial class GameObject : Form {
		// 背景界面
		private ImageControl _imageControl = new ImageControl();

		public WaveOutEvent waveOut;

		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		public GameObject(int level) {
			string filePath = "./static/2.mp3";
			waveOut = new WaveOutEvent();
			var mp3FileReader = new Mp3FileReader(filePath);
			waveOut.Init(mp3FileReader);
			waveOut.Play();

			AllocConsole();
			_imageControl.Dock = DockStyle.Fill;
			this.Controls.Add(_imageControl);
			InitializeComponent();

			StartGame(level);
		}


		private int _maxLevel = 10; //多少层
		private int _maxWidth = 6; // 跨度个数
		private int _maxHeight = 5; // 最大宽度
		private int _maxFlop = 40; //翻牌数量;

		/// <summary>
		/// 开始游戏，返回值为是否通关
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public void StartGame(int level) {
			Random random = new Random();

			if (level == 1) {
				_maxLevel = 4;
				_maxWidth = 3;
				_maxHeight = 3;
				List<string> list = ReadResourceUtil.ReadSkin();

				var range = list.GetRange(1, 4);
				Console.WriteLine(range.Count);

				// 绘制卡槽
				int initX = 100;
				int initY = 50;

				CardSlotControl cardSlotControl = new CardSlotControl(_imageControl,
					initX, initY + FruitObject.DefaultHeight * (_maxHeight + 2));

				// 随机生成卡片集合：打乱顺序
				var fruitObjects = new List<FruitObject>();

				foreach (var tmp in range) {
					try {
						Bitmap bufferedImage = new Bitmap(Image.FromFile(tmp));
						for (int i = 0; i < 9; i++) {
							var size = fruitObjects.Count - 1;
							var fruits = new Fruits(bufferedImage, tmp);
							var index = 0;
							if (size > 5) {
								index = random.Next(size);
							}

							fruitObjects.Insert(index, new FruitObject(cardSlotControl, fruits, 0, 0, 0));
						}
					}
					catch (IOException e) {
						Console.WriteLine(e);
					}
				}


				// 给每个对象设置坐标
				int idx = 0;
				for (int i = 0; i < _maxLevel; i++) {
					for (int x = 0; x < _maxWidth; x++) {
						for (int y = 0; y < _maxHeight; y++) {
							var fruitObject = fruitObjects[idx++];
							fruitObject.X = x;
							fruitObject.Y = y;
							fruitObject.Level = i;

							int p = i % 4;
							if (p == 0) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 - p * 10,
									initY - FruitObject.DefaultHeight / 4, true, true);
							}
							else if (p == 1) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 + p * 10,
									initY - FruitObject.DefaultHeight / 4, false, false);
							}
							else if (p == 2) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 - p * 10,
									initY - FruitObject.DefaultHeight / 4, true, false);
							}
							else if (p == 3) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 + p * 10,
									initY - FruitObject.DefaultHeight / 4, false, true);
							}
						}
					}
				}
			}
			else if (level == 2) {
				_maxLevel = 10;
				_maxWidth = 6;
				_maxHeight = 5;
				_maxFlop = 40;


				List<string> list = ReadResourceUtil.ReadSkin();
				// 第二关
				Console.WriteLine($@"{_maxLevel}层, {_maxWidth}跨, {_maxHeight}宽, {_maxFlop}翻牌");
				int typeSize = list.Count;
				Console.WriteLine($@"种类数量：{typeSize}");
				// 求得每种种类的个数
				int groupNumber = (int) Math.Ceiling((_maxLevel * _maxWidth * _maxHeight + _maxFlop) / (3f * typeSize));
				Console.WriteLine(@"每种组数：" + groupNumber);
				int groupCount = groupNumber * 3;
				Console.WriteLine(@"每种总数：" + groupCount);
				Console.WriteLine(@"共计数量：" + (typeSize * groupCount + _maxFlop));

				// 绘制卡槽
				int initX = 100;
				int initY = 50;

				CardSlotControl cardSlotControl = new CardSlotControl(_imageControl,
					initX + ((_maxWidth - 7) * FruitObject.DefaultWidth) / 2,
					initY + FruitObject.DefaultHeight * (_maxHeight + 2));

				// 随机生成卡片集合：打乱顺序
				List<FruitObject> fruitObjects = new List<FruitObject>();

				foreach (string tmp in list) {
					try {
						Bitmap bufferedImage = new Bitmap(Image.FromFile(tmp));
						int count = groupCount + (_maxFlop > 0 ? random.Next(_maxFlop) : 0);
						for (int i = 0; i < count; i++) {
							var size = fruitObjects.Count - 1;
							var fruits = new Fruits(bufferedImage, tmp);
							var index = 0;
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

				// 给每个对象设置坐标
				int idx = 0;
				for (int i = 0; i < _maxLevel; i++) {
					for (int x = 0; x < _maxWidth; x++) {
						for (int y = 0; y < _maxHeight; y++) {
							var next = random.Next(1, 10);
							if (next < 3) continue;
							var fruitObject = fruitObjects[idx++];
							fruitObject.X = x;
							fruitObject.Y = y;
							fruitObject.Level = i;
							int p = i % 4;
							if (p == 0) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 - p * 20,
									initY - FruitObject.DefaultHeight / 4, true, true);
							}
							else if (p == 1) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 + p * 20,
									initY - FruitObject.DefaultHeight / 4, false, false);
							}
							else if (p == 2) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 - p * 20,
									initY - FruitObject.DefaultHeight / 4, true, false);
							}
							else if (p == 3) {
								fruitObject.Show(_imageControl, initX - FruitObject.DefaultWidth / 4 + p * 20,
									initY - FruitObject.DefaultHeight / 4, false, true);
							}
						}
					}
				}
				Console.WriteLine(@"重叠数量：" + idx);
				// 绘制翻牌区
				int fSize = fruitObjects.Count;
				if (idx < fSize) {
					int step = 5;
					int lenght = 80;
					for (int i = 0; i < lenght; i++) {
						if (idx >= fSize) break;
						FruitObject fruitObject = fruitObjects[idx++];
						fruitObject.X = _maxWidth - 1;
						fruitObject.Y = _maxHeight + 1;
						fruitObject.Level = i;
						if (i == 0) fruitObject.SetFlag(true);
						fruitObject.ShowFold(_imageControl, initX - (lenght * step), initY, i * step);
					}

					Console.WriteLine(@"翻牌区数：" + lenght + "\t" + idx);
				}
				
			}
		}
	}
}