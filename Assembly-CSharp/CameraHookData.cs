using System;

[Serializable]
public class CameraHookData : DataObject
{
	public CameraHookData(int setID) : base(setID)
	{
	}

	public float POSX { get; set; }

	public float POSY { get; set; }

	public float POSZ { get; set; }

	public float ROTX { get; set; }

	public float ROTY { get; set; }

	public float ROTZ { get; set; }

	public float FOV { get; set; }
}
