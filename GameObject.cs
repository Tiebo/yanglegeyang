using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using yanglegeyang.components;
using yanglegeyang.container;
using yanglegeyang.utils;

namespace yanglegeyang {
	public partial class GameObject : Form {
		// 背景界面
		private ImageControl _imageControl = new ImageControl();

		public WaveOutEvent waveOut;

		private CardSlotControl _cardSlotControl;

		public int GameLevel = 1;

		public bool ListNumber1 = false;
		public bool ListNumber2 = false;

		public GameObject(int level) {
			this.GameLevel = level;
			
			string filePath = "./static/2.mp3";
			waveOut = new WaveOutEvent();
			var mp3FileReader = new Mp3FileReader(filePath);
			waveOut.Init(mp3FileReader);
			waveOut.Play();

			_imageControl.Dock = DockStyle.Fill;
			this.Controls.Add(_imageControl);
			InitializeComponent();
			this._imageControl.SendToBack();
			if (GameLevel == 1) {
				this.button1.Visible = false;
				this.button2.Visible = false;
			}
			else {
				this.button1.Visible = true;
				this.button2.Visible = true;
			}
			StartGame(GameLevel);
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
				_maxLevel = 2;
				_maxWidth = 3;
				_maxHeight = 3;
				List<string> list = ReadResourceUtil.ReadSkin();

				var range = list.GetRange(1, 3);

				// 绘制卡槽
				int initX = 100;
				int initY = 50;

				_cardSlotControl = new CardSlotControl(_imageControl,
					initX, initY + FruitObject.DefaultHeight * (_maxHeight + 2));

				// 随机生成卡片集合：打乱顺序
				var fruitObjects = new List<FruitObject>();

				foreach (var tmp in range) {
					try {
						Bitmap bufferedImage = new Bitmap(Image.FromFile(tmp));
						for (int i = 0; i < 6; i++) {
							var size = fruitObjects.Count - 1;
							var fruits = new Fruits(bufferedImage, tmp);
							var index = 0;
							if (size > 3) {
								index = random.Next(size);
							}

							fruitObjects.Insert(index, new FruitObject(_cardSlotControl, fruits, 0, 0, 0));
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
							int colGap = 40, rowGap = 40;

							int fx = initX + FruitObject.DefaultWidth + x * colGap + i * 10;
							int fy = initY + FruitObject.DefaultHeight / 4 + y * rowGap + i * 10;

							fruitObject.Show(_imageControl, fx,
								fy, false, false);
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
				int typeSize = list.Count;
				// 求得每种种类的个数
				int groupNumber = (int) Math.Ceiling((_maxLevel * _maxWidth * _maxHeight + _maxFlop) / (3f * typeSize));
				int groupCount = groupNumber * 3;

				// 绘制卡槽
				int initX = 100;
				int initY = 50;

				_cardSlotControl = new CardSlotControl(_imageControl,
					initX + ((_maxWidth - 7) * FruitObject.DefaultWidth) / 2,
					initY + FruitObject.DefaultHeight * (_maxHeight + 2) + 80);

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

							fruitObjects.Insert(index, new FruitObject(_cardSlotControl, fruits, 0, 0, 0));
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
				}
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			
			if (ListNumber2) {
				MessageBox.Show(@"一局只能使用一次", @"错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			Random random = new Random();
			
			List<string> list = ReadResourceUtil.ReadSkin();
			var oldObjects = MySpace.AllLevelFruits;

			for (int i = 0; i < oldObjects.Count; i++) {
				oldObjects[i].ForEach(f => {
					var imgName = list[random.Next(list.Count)];
					
					Bitmap bufferedImage = new Bitmap(Image.FromFile(imgName));

					f.Fruits.Image = bufferedImage;
					f.Fruits.ImageName = imgName;
					f.ImageName = imgName;
					
					f.Fruits.Invalidate();
				});
			}

			ListNumber2 = true;
		}

		private bool _isPlay = true;

		private void pictureBox1_Click(object sender, EventArgs e) {
			if (_isPlay) {
				waveOut.Pause();
				button3.BackgroundImage = Properties.Resources.pause;
			}
			else {
				waveOut.Play();
				button3.BackgroundImage = Properties.Resources.play;
			}

			_isPlay = !_isPlay;
		}

		private void button1_Click(object sender, EventArgs e) {
			if (_cardSlotControl.Slots.Count == 0) {
				MessageBox.Show(@"当前卡槽无卡片", @"错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (ListNumber1) {
				MessageBox.Show(@"一局只能使用一次", @"错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			
			List<FruitObject> slots = _cardSlotControl.Slots.GetRange(0, _cardSlotControl.Slots.Count);
			int fx = 200, fy = 600;
			
			for (int i = slots.Count - 1; i >= 0; i --) {
				if (slots.Count - i > 3) break;
				FruitObject f = slots[i];
				// 从卡槽中删除该元素
				_cardSlotControl.Slots.Remove(f);
				_cardSlotControl.Controls.Remove(f.Fruits);
				
				Rectangle visibleRect = f.Fruits.DisplayRectangle;
				_imageControl.Controls.Add(f.Fruits);
				
				MySpace.AllLevelFruits[0].Add(f);
				
				f.SetFlag(true);
				f.Fruits.IsSlot = false;
				f.Fruits.IsMove = true;
				f.Fruits.SetBounds(fx, fy, FruitObject.DefaultWidth, FruitObject.DefaultHeight);

				f.Fruits.Invalidate();
				fx += FruitObject.DefaultHeight + 20;
				_imageControl.Invalidate(visibleRect);
			}

			ListNumber1 = true;
		}
	}
}