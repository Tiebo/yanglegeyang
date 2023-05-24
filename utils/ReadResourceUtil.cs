﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Policy;

namespace yanglegeyang.utils {
	public class ReadResourceUtil {

		static ReadResourceUtil() {
		}

		public static Uri GetUri(string name) {
			var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
			if (resource == null) {
				try {
					var parentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					var staticDirectory = Path.Combine(parentDirectory, "static");
					if (Directory.Exists(staticDirectory)) {
						var filePath = Path.Combine(parentDirectory, name);
						var uri = new Uri(filePath);
						return uri;
					}
					else {
						var grandparentDirectory = Directory.GetParent(parentDirectory).Parent.FullName;
						var filePath = Path.Combine(grandparentDirectory, name);
						var uri = new Uri(filePath);
						return uri;
					}
				}
				catch (Exception e) {
					Console.WriteLine(e.Message);
					return null;
				}
			}

			return new Uri(resource.ToString());
		}

		private static String ReadSkinPath() {
			return "/static/skins/a1";
		}

		public static List<string> ReadSkin() {
			string path = ReadSkinPath();
			List<string> list = new List<string>();
			try {
				Uri url = GetUri("./" + path);
				if (url != null) {
					try {
						DirectoryInfo folder = new DirectoryInfo(url.LocalPath);
						foreach (FileInfo file in folder.GetFiles()) {
							list.Add(file.FullName);
						}
					}
					catch (Exception ex) {
						Console.WriteLine(ex.Message);
					}
				}
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
			}

			return list;
		}
	}
}