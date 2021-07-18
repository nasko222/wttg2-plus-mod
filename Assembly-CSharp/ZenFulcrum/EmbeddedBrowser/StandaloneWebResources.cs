using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class StandaloneWebResources : WebResources
	{
		public StandaloneWebResources(string dataFile)
		{
			this.dataFile = dataFile;
		}

		public void LoadIndex()
		{
			using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(this.dataFile)))
			{
				string a = binaryReader.ReadString();
				if (a != "zfbRes_v1")
				{
					throw new Exception("Invalid web resource file");
				}
				int num = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					StandaloneWebResources.IndexEntry value = new StandaloneWebResources.IndexEntry
					{
						name = binaryReader.ReadString(),
						offset = binaryReader.ReadInt64(),
						length = binaryReader.ReadInt32()
					};
					this.toc[value.name] = value;
				}
			}
		}

		public override byte[] GetData(string path)
		{
			StandaloneWebResources.IndexEntry indexEntry;
			if (!this.toc.TryGetValue(path, out indexEntry))
			{
				return null;
			}
			byte[] result;
			using (FileStream fileStream = File.OpenRead(this.dataFile))
			{
				fileStream.Seek(indexEntry.offset, SeekOrigin.Begin);
				byte[] array = new byte[indexEntry.length];
				int num = fileStream.Read(array, 0, indexEntry.length);
				if (num != array.Length)
				{
					throw new Exception("Insufficient data for file");
				}
				result = array;
			}
			return result;
		}

		public void WriteData(Dictionary<string, byte[]> files)
		{
			Dictionary<string, StandaloneWebResources.IndexEntry> dictionary = new Dictionary<string, StandaloneWebResources.IndexEntry>();
			using (FileStream fileStream = File.OpenWrite(this.dataFile))
			{
				BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8);
				binaryWriter.Write("zfbRes_v1");
				binaryWriter.Write(files.Count);
				long position = fileStream.Position;
				foreach (KeyValuePair<string, byte[]> keyValuePair in files)
				{
					binaryWriter.Write(keyValuePair.Key);
					binaryWriter.Write(0L);
					binaryWriter.Write(0);
				}
				foreach (KeyValuePair<string, byte[]> keyValuePair2 in files)
				{
					byte[] value = keyValuePair2.Value;
					StandaloneWebResources.IndexEntry value2 = new StandaloneWebResources.IndexEntry
					{
						name = keyValuePair2.Key,
						length = keyValuePair2.Value.Length,
						offset = fileStream.Position
					};
					binaryWriter.Write(value);
					dictionary[keyValuePair2.Key] = value2;
				}
				binaryWriter.Seek((int)position, SeekOrigin.Begin);
				foreach (KeyValuePair<string, byte[]> keyValuePair3 in files)
				{
					StandaloneWebResources.IndexEntry indexEntry = dictionary[keyValuePair3.Key];
					binaryWriter.Write(keyValuePair3.Key);
					binaryWriter.Write(indexEntry.offset);
					binaryWriter.Write(indexEntry.length);
				}
			}
		}

		private const string FileHeader = "zfbRes_v1";

		protected Dictionary<string, StandaloneWebResources.IndexEntry> toc = new Dictionary<string, StandaloneWebResources.IndexEntry>();

		protected string dataFile;

		public const string DefaultPath = "Resources/browser_assets";

		public struct IndexEntry
		{
			public string name;

			public long offset;

			public int length;
		}
	}
}
