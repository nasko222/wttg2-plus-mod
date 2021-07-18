using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileSlinger
{
	public static void wildSaveFile<T>(string fileName, T wildDataType)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Application.persistentDataPath + "/" + fileName);
		binaryFormatter.Serialize(fileStream, wildDataType);
		fileStream.Close();
	}

	public static bool wildLoadFile<T>(string fileName, out T returnWildData)
	{
		if (File.Exists(Application.persistentDataPath + "/" + fileName))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
			returnWildData = (T)((object)binaryFormatter.Deserialize(fileStream));
			fileStream.Close();
			return true;
		}
		returnWildData = default(T);
		return false;
	}

	public static void wildDeleteFile(string fileName = "defaultSave.gd")
	{
		File.Delete(Application.persistentDataPath + "/" + fileName);
	}

	public static string readWebSiteFile(string fileName = "")
	{
		string result = string.Empty;
		string[] array = Application.dataPath.Split(new string[]
		{
			"/"
		}, StringSplitOptions.None);
		string text = string.Empty;
		for (int i = 0; i < array.Length - 1; i++)
		{
			text = text + array[i] + "/";
		}
		text = text + "BrowserAssets/" + fileName;
		if (File.Exists(text))
		{
			StreamReader streamReader = new StreamReader(text);
			result = streamReader.ReadToEnd();
			streamReader.Close();
		}
		return result;
	}
}
