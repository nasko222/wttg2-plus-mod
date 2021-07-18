using System;
using UnityEngine;
using UnityEngine.UI;

public class DeadOrNotChatObject : MonoBehaviour
{
	public float Height
	{
		get
		{
			return this.myHeight;
		}
	}

	public float CurrentY
	{
		get
		{
			return this.myRT.anchoredPosition.y;
		}
	}

	public void SoftBuild()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.myRT.anchoredPosition = new Vector2(8f, -35f);
	}

	public void Build(string ChatText)
	{
		string text = "User" + UnityEngine.Random.Range(101, 999).ToString() + ":";
		string text2 = "        " + ChatText;
		this.myHeight = MagicSlinger.GetStringHeight(text2, this.chatFont, 14, this.chatRT.sizeDelta);
		this.chatRT.sizeDelta = new Vector2(this.chatRT.sizeDelta.x, this.myHeight);
		this.userNameText.text = text;
		this.chatText.text = text2;
		this.myRT.sizeDelta = new Vector2(this.myRT.sizeDelta.x, this.myHeight);
	}

	public void MoveUp(float SetY)
	{
		this.myRT.anchoredPosition = new Vector2(8f, SetY);
	}

	private void Awake()
	{
	}

	[SerializeField]
	private Text userNameText;

	[SerializeField]
	private Text chatText;

	[SerializeField]
	private RectTransform chatRT;

	[SerializeField]
	private Font chatFont;

	private RectTransform myRT;

	private float myHeight;
}
