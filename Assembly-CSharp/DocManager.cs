using System;
using UnityEngine;

public class DocManager : MonoBehaviour
{
	public void OpenDocument(DocDefinition TheDocument)
	{
	}

	private void Awake()
	{
		DocManager.Ins = this;
	}

	public static DocManager Ins;
}
