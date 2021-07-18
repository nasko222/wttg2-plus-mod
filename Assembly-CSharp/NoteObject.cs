using System;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour
{
	public void BuildMe(string setText, SOFTWARE_PRODUCTS SetType)
	{
		this.extends.x = WindowManager.Get(SetType).Window.GetComponent<RectTransform>().sizeDelta.x - 12f;
		this.extends.y = WindowManager.Get(SetType).Window.GetComponent<RectTransform>().sizeDelta.y - 89f;
		this.noteField.text = setText;
		this.mySize.x = base.GetComponent<RectTransform>().sizeDelta.x;
		this.mySize.y = MagicSlinger.GetStringHeight(setText, this.noteText.font, this.noteText.fontSize, this.extends) + 10f;
		base.GetComponent<RectTransform>().sizeDelta = this.mySize;
	}

	public InputField noteField;

	public Text noteText;

	private Vector2 extends = Vector2.zero;

	private Vector2 mySize = Vector2.zero;
}
