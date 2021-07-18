using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TutorialTextHook : MonoBehaviour
{
	public void Process(string SetText, float SetLength)
	{
		this.isKilled = false;
		this.currentText = SetText;
		this.timePerLetter = SetLength / (float)this.currentText.Length * 0.84f;
		base.StartCoroutine(this.typeText());
	}

	public void HardShow()
	{
		this.isKilled = true;
		this.myText.text = this.currentText;
		this.myText2.text = this.currentText;
	}

	public void Clear()
	{
		this.isKilled = true;
		this.myText.text = string.Empty;
		this.myText2.text = string.Empty;
		this.currentText = string.Empty;
	}

	private IEnumerator typeText()
	{
		foreach (char letter in this.currentText.ToCharArray())
		{
			if (!this.isKilled)
			{
				Text text = this.myText;
				text.text += letter;
				Text text2 = this.myText2;
				text2.text += letter;
			}
			yield return 0;
			yield return new WaitForSeconds(this.timePerLetter);
			if (this.isKilled)
			{
				break;
			}
		}
		yield break;
	}

	private void Awake()
	{
		TutorialTextHook.Ins = this;
		this.myText = base.GetComponent<Text>();
	}

	public static TutorialTextHook Ins;

	[SerializeField]
	private Text myText;

	[SerializeField]
	private Text myText2;

	private float timePerLetter;

	private string currentText;

	private bool isKilled;
}
