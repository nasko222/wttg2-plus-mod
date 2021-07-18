using System;

[Serializable]
public class IconData : DataObject
{
	public IconData(int SetID) : base(SetID)
	{
	}

	public Vect2 MyPOS { get; set; }
}
