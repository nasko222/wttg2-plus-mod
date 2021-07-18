using System;

[Serializable]
public class BaseControllerData : DataObject
{
	public BaseControllerData(int setID) : base(setID)
	{
	}

	public int MyState { get; set; }

	public bool Active { get; set; }

	public float POSX { get; set; }

	public float POSY { get; set; }

	public float POSZ { get; set; }

	public float ROTX { get; set; }

	public float ROTY { get; set; }

	public float ROTZ { get; set; }
}
