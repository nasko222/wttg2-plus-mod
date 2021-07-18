using System;
using System.IO;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	internal class EditorWebResources : WebResources
	{
		public EditorWebResources()
		{
			this.basePath = Path.GetDirectoryName(Application.dataPath) + "/BrowserAssets";
		}

		public override byte[] GetData(string path)
		{
			byte[] result;
			try
			{
				result = File.ReadAllBytes(this.basePath + path);
			}
			catch (Exception ex)
			{
				if (!(ex is FileNotFoundException) && !(ex is DirectoryNotFoundException))
				{
					throw;
				}
				result = null;
			}
			return result;
		}

		protected string basePath;
	}
}
