using System;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public static class FileLocations
	{
		public static FileLocations.CEFDirs Dirs
		{
			get
			{
				FileLocations.CEFDirs result;
				if ((result = FileLocations._dirs) == null)
				{
					result = (FileLocations._dirs = FileLocations.GetCEFDirs());
				}
				return result;
			}
		}

		private static FileLocations.CEFDirs GetCEFDirs()
		{
			string text = Application.dataPath + "/Plugins";
			return new FileLocations.CEFDirs
			{
				resourcesPath = text,
				binariesPath = Application.dataPath + "/../",
				localesPath = text + "/locales",
				subprocessFile = text + "/ZFGameBrowser.exe",
				logFile = Application.dataPath + "/output_log.txt"
			};
		}

		public const string SlaveExecutable = "ZFGameBrowser";

		private static FileLocations.CEFDirs _dirs;

		public class CEFDirs
		{
			public string resourcesPath;

			public string binariesPath;

			public string localesPath;

			public string subprocessFile;

			public string logFile;
		}
	}
}
