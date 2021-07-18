using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager
{
	static DataManager()
	{
		DataManager._dataLookUp = new DataLookUp();
		DataManager._optionLookUp = new DataLookUp();
		if (File.Exists(DataManager._gameFileName))
		{
			DataManager._continueFile = true;
			FileStream fileStream = File.Open(DataManager._gameFileName, FileMode.Open);
			DataManager._dataLookUp = (DataLookUp)DataManager._bf.Deserialize(fileStream);
			fileStream.Close();
		}
		if (File.Exists(DataManager._optionFileName))
		{
			DataManager._savedOptions = true;
			FileStream fileStream2 = File.Open(DataManager._optionFileName, FileMode.Open);
			DataManager._optionLookUp = (DataLookUp)DataManager._bf.Deserialize(fileStream2);
			fileStream2.Close();
		}
	}

	public static bool LockSave
	{
		get
		{
			return DataManager._lockSave;
		}
		set
		{
			DataManager._lockSave = value;
		}
	}

	public static bool ContinuedGame
	{
		get
		{
			return DataManager._continueFile;
		}
	}

	public static bool SavedOptions
	{
		get
		{
			return DataManager._savedOptions;
		}
	}

	public static bool LeetMode
	{
		get
		{
			return DataManager._leetMode;
		}
		set
		{
			DataManager._leetMode = value;
		}
	}

	public static void Save<T>(T DataToSave) where T : class, IDataObject
	{
		if (!DataManager._lockSave && StateManager.PlayerState != PLAYER_STATE.BUSY)
		{
			DataContainer dataContainer;
			if (DataManager._dataLookUp.Data.TryGetValue(typeof(T), out dataContainer))
			{
				dataContainer.Add<T>(DataToSave.ID, DataToSave);
				DataManager._dataLookUp.Data[typeof(T)] = dataContainer;
				return;
			}
			dataContainer = new DataContainer();
			dataContainer.Add<T>(DataToSave.ID, DataToSave);
			DataManager._dataLookUp.Data.Add(typeof(T), dataContainer);
		}
	}

	public static void SaveOption<T>(T DataToSave) where T : class, IDataObject
	{
		DataContainer dataContainer;
		if (DataManager._optionLookUp.Data.TryGetValue(typeof(T), out dataContainer))
		{
			dataContainer.Add<T>(DataToSave.ID, DataToSave);
			DataManager._optionLookUp.Data[typeof(T)] = dataContainer;
			return;
		}
		dataContainer = new DataContainer();
		dataContainer.Add<T>(DataToSave.ID, DataToSave);
		DataManager._optionLookUp.Data.Add(typeof(T), dataContainer);
	}

	public static T Load<T>(int LookUpID) where T : class, IDataObject
	{
		DataContainer dataContainer;
		if (!DataManager._dataLookUp.Data.TryGetValue(typeof(T), out dataContainer))
		{
			return default(T);
		}
		IDataObject dataObject;
		if (dataContainer.MyData.TryGetValue(LookUpID, out dataObject))
		{
			return (T)((object)dataObject);
		}
		return default(T);
	}

	public static T LoadOption<T>(int LookUpID) where T : class, IDataObject
	{
		DataContainer dataContainer;
		if (!DataManager._optionLookUp.Data.TryGetValue(typeof(T), out dataContainer))
		{
			return default(T);
		}
		IDataObject dataObject;
		if (dataContainer.MyData.TryGetValue(LookUpID, out dataObject))
		{
			return (T)((object)dataObject);
		}
		return default(T);
	}

	public static void Reset()
	{
		DataManager._lockSave = false;
		DataManager._leetMode = false;
		DataManager._dataLookUp = new DataLookUp();
		DataManager._optionLookUp = new DataLookUp();
		if (File.Exists(DataManager._gameFileName))
		{
			DataManager._continueFile = true;
			FileStream fileStream = File.Open(DataManager._gameFileName, FileMode.Open);
			DataManager._dataLookUp = (DataLookUp)DataManager._bf.Deserialize(fileStream);
			fileStream.Close();
		}
		if (File.Exists(DataManager._optionFileName))
		{
			DataManager._savedOptions = true;
			FileStream fileStream2 = File.Open(DataManager._optionFileName, FileMode.Open);
			DataManager._optionLookUp = (DataLookUp)DataManager._bf.Deserialize(fileStream2);
			fileStream2.Close();
		}
	}

	public static void ClearGameData()
	{
		DataManager._continueFile = false;
		DataManager._dataLookUp = new DataLookUp();
		FileSlinger.wildDeleteFile("WTTG2TI.gd");
	}

	public static void WriteData()
	{
		if (!DataManager._leetMode && DataManager._leetMode)
		{
			FileStream fileStream = File.Create(DataManager._gameFileName);
			DataManager._bf.Serialize(fileStream, DataManager._dataLookUp);
			fileStream.Close();
		}
	}

	public static void WriteOptionData()
	{
		FileStream fileStream = File.Create(DataManager._optionFileName);
		DataManager._bf.Serialize(fileStream, DataManager._optionLookUp);
		fileStream.Close();
	}

	private static bool _lockSave;

	private static bool _continueFile;

	private static bool _savedOptions;

	private static bool _leetMode;

	private static DataLookUp _dataLookUp;

	private static DataLookUp _optionLookUp;

	private static BinaryFormatter _bf = new BinaryFormatter();

	private static string _gameFileName = Application.persistentDataPath + "/WTTG2TI.gd";

	private static string _optionFileName = Application.persistentDataPath + "/WTTG2OPTDATA.gd";
}
