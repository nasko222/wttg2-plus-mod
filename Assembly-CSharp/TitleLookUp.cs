using System;
using UnityEngine;

public class TitleLookUp : MonoBehaviour
{
	private void Awake()
	{
		TitleLookUp.Ins = this;
	}

	public static TitleLookUp Ins;

	public AudioFileDefinition PresentSubMenuSFX;

	public AudioFileDefinition TitleMenuHoverSFX;

	public AudioFileDefinition TitleMenuClickSFX;
}
