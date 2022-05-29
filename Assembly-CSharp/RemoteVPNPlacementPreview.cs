using System;
using UnityEngine;
using UnityEngine.UI;

public class RemoteVPNPlacementPreview : MonoBehaviour
{
	public void GoRed()
	{
		Color red = Color.red;
		this.myMat.SetColor("_EmissionColor", red);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNZeroBar;
	}

	public void GoOrange()
	{
		Color yellow = Color.yellow;
		this.myMat.SetColor("_EmissionColor", yellow);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNOneBar;
	}

	public void GoGreen()
	{
		Color green = Color.green;
		this.myMat.SetColor("_EmissionColor", green);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNTwoBar;
	}

	private void Awake()
	{
	}

	public void GoBlue()
	{
		Color blue = Color.blue;
		this.myMat.SetColor("_EmissionColor", blue);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNTwoBar;
	}

	public void GoRedGlitchy(int intense)
	{
		if (UnityEngine.Random.Range(1, intense) != 3)
		{
			Color red = Color.red;
			this.myMat.SetColor("_EmissionColor", red);
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNZeroBar;
			return;
		}
		Color yellow = Color.yellow;
		this.myMat.SetColor("_EmissionColor", yellow);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNOneBar;
	}

	public void GoOrangeGlitchy(int intense)
	{
		if (UnityEngine.Random.Range(1, intense) == 2)
		{
			Color red = Color.red;
			this.myMat.SetColor("_EmissionColor", red);
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNZeroBar;
			return;
		}
		if (UnityEngine.Random.Range(1, intense) == 3)
		{
			Color green = Color.green;
			this.myMat.SetColor("_EmissionColor", green);
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNTwoBar;
			return;
		}
		Color yellow = Color.yellow;
		this.myMat.SetColor("_EmissionColor", yellow);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNOneBar;
	}

	public void GoGreenGlitchy(int intense)
	{
		if (UnityEngine.Random.Range(1, intense) != 3)
		{
			Color green = Color.green;
			this.myMat.SetColor("_EmissionColor", green);
			LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNTwoBar;
			return;
		}
		Color yellow = Color.yellow;
		this.myMat.SetColor("_EmissionColor", yellow);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNOneBar;
	}

	public void GoOOrange()
	{
		Color value = new Color32(byte.MaxValue, 165, 0, 1);
		this.myMat.SetColor("_EmissionColor", value);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNOneBar;
	}

	public void GoLime()
	{
		Color value = new Color32(191, byte.MaxValue, 0, 1);
		this.myMat.SetColor("_EmissionColor", value);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNTwoBar;
	}

	public void GoBlack()
	{
		Color black = Color.black;
		this.myMat.SetColor("_EmissionColor", black);
		LookUp.PlayerUI.RemoteVPNIcon.GetComponent<Image>().sprite = LookUp.PlayerUI.RemoteVPNZeroBar;
	}

	[SerializeField]
	private Material myMat;
}
