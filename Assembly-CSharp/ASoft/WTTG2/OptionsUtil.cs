using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ASoft.WTTG2
{
	public class OptionsUtil : MonoBehaviour
	{
		public static void BuildOptionsButton(string Text, string PlayerPrefsID, int PlayerPrefsDefault, float VerticalPos, Action ApplyON = null, Action ApplyOFF = null)
		{
			Transform transform = GameObject.Find("NuidtyTitle").transform;
			Transform transform2 = GameObject.Find("GameOptions").transform;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
			bool onOff = false;
			gameObject.transform.position = new Vector2(transform.position.x - 200f, transform.position.y - VerticalPos);
			gameObject.GetComponent<TextMeshProUGUI>().text = Text;
			gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
			gameObject.transform.SetParent(transform2);
			GameObject gameObject2 = GameObject.Find("NudityOnButton");
			GameObject ButtonOn = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
			ButtonOn.transform.position = new Vector2(gameObject2.transform.position.x - 100f, gameObject2.transform.position.y - (VerticalPos + 2f));
			ButtonOn.transform.SetParent(transform2);
			GameObject gameObject3 = GameObject.Find("NudityOffButton");
			GameObject ButtonOff = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
			ButtonOff.transform.position = new Vector2(gameObject3.transform.position.x - 100f, gameObject3.transform.position.y - (VerticalPos + 2f));
			ButtonOff.transform.SetParent(transform2);
			ButtonOn.GetComponent<OptionsMenuBTN>().ClickAction = null;
			ButtonOn.GetComponent<OptionsMenuBTN>().ClickAction = new UnityEvent();
			ButtonOn.GetComponent<OptionsMenuBTN>().ClickAction.AddListener(delegate()
			{
				ButtonOff.GetComponent<OptionsMenuBTN>().Clear();
				onOff = true;
			});
			ButtonOff.GetComponent<OptionsMenuBTN>().ClickAction = null;
			ButtonOff.GetComponent<OptionsMenuBTN>().ClickAction = new UnityEvent();
			ButtonOff.GetComponent<OptionsMenuBTN>().ClickAction.AddListener(delegate()
			{
				ButtonOn.GetComponent<OptionsMenuBTN>().Clear();
				onOff = false;
			});
			if (!PlayerPrefs.HasKey(PlayerPrefsID))
			{
				if (PlayerPrefsDefault == 1)
				{
					ButtonOn.GetComponent<OptionsMenuBTN>().SetActive();
					onOff = true;
				}
				else if (PlayerPrefsDefault == 0)
				{
					ButtonOff.GetComponent<OptionsMenuBTN>().SetActive();
					onOff = false;
				}
				PlayerPrefs.SetInt(PlayerPrefsID, PlayerPrefsDefault);
			}
			else if (PlayerPrefs.GetInt(PlayerPrefsID) == 1)
			{
				ButtonOn.GetComponent<OptionsMenuBTN>().SetActive();
				onOff = true;
				PlayerPrefs.SetInt(PlayerPrefsID, 1);
			}
			else if (PlayerPrefs.GetInt(PlayerPrefsID) == 0)
			{
				ButtonOff.GetComponent<OptionsMenuBTN>().SetActive();
				onOff = false;
				PlayerPrefs.SetInt(PlayerPrefsID, 0);
			}
			TitleOptionsMenuHook.SettingsApplied.Event += delegate()
			{
				if (onOff)
				{
					PlayerPrefs.SetInt(PlayerPrefsID, 1);
					if (ApplyON != null)
					{
						ApplyON();
						return;
					}
				}
				else
				{
					PlayerPrefs.SetInt(PlayerPrefsID, 0);
					if (ApplyOFF != null)
					{
						ApplyOFF();
					}
				}
			};
		}

		public static void BuildOptionsButton2(string Text, string PlayerPrefsID, int PlayerPrefsDefault, float VerticalPos, Action ApplyON = null, Action ApplyOFF = null)
		{
			Transform transform = GameObject.Find("NuidtyTitle").transform;
			Transform transform2 = GameObject.Find("GameOptions").transform;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("NuidtyTitle"));
			bool onOff = false;
			gameObject.transform.position = new Vector2(transform.position.x + 200f, transform.position.y - VerticalPos);
			gameObject.GetComponent<TextMeshProUGUI>().text = Text;
			gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(225f, 40f);
			gameObject.transform.SetParent(transform2);
			GameObject gameObject2 = GameObject.Find("NudityOnButton");
			GameObject ButtonOn = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
			ButtonOn.transform.position = new Vector2(gameObject2.transform.position.x + 300f, gameObject2.transform.position.y - (VerticalPos + 2f));
			ButtonOn.transform.SetParent(transform2);
			GameObject gameObject3 = GameObject.Find("NudityOffButton");
			GameObject ButtonOff = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
			ButtonOff.transform.position = new Vector2(gameObject3.transform.position.x + 300f, gameObject3.transform.position.y - (VerticalPos + 2f));
			ButtonOff.transform.SetParent(transform2);
			ButtonOn.GetComponent<OptionsMenuBTN>().ClickAction = null;
			ButtonOn.GetComponent<OptionsMenuBTN>().ClickAction = new UnityEvent();
			ButtonOn.GetComponent<OptionsMenuBTN>().ClickAction.AddListener(delegate()
			{
				ButtonOff.GetComponent<OptionsMenuBTN>().Clear();
				onOff = true;
			});
			ButtonOff.GetComponent<OptionsMenuBTN>().ClickAction = null;
			ButtonOff.GetComponent<OptionsMenuBTN>().ClickAction = new UnityEvent();
			ButtonOff.GetComponent<OptionsMenuBTN>().ClickAction.AddListener(delegate()
			{
				ButtonOn.GetComponent<OptionsMenuBTN>().Clear();
				onOff = false;
			});
			if (!PlayerPrefs.HasKey(PlayerPrefsID))
			{
				if (PlayerPrefsDefault == 1)
				{
					ButtonOn.GetComponent<OptionsMenuBTN>().SetActive();
					onOff = true;
				}
				else if (PlayerPrefsDefault == 0)
				{
					ButtonOff.GetComponent<OptionsMenuBTN>().SetActive();
					onOff = false;
				}
				PlayerPrefs.SetInt(PlayerPrefsID, PlayerPrefsDefault);
			}
			else if (PlayerPrefs.GetInt(PlayerPrefsID) == 1)
			{
				ButtonOn.GetComponent<OptionsMenuBTN>().SetActive();
				onOff = true;
				PlayerPrefs.SetInt(PlayerPrefsID, 1);
			}
			else if (PlayerPrefs.GetInt(PlayerPrefsID) == 0)
			{
				ButtonOff.GetComponent<OptionsMenuBTN>().SetActive();
				onOff = false;
				PlayerPrefs.SetInt(PlayerPrefsID, 0);
			}
			TitleOptionsMenuHook.SettingsApplied.Event += delegate()
			{
				if (onOff)
				{
					PlayerPrefs.SetInt(PlayerPrefsID, 1);
					if (ApplyON != null)
					{
						ApplyON();
						return;
					}
				}
				else
				{
					PlayerPrefs.SetInt(PlayerPrefsID, 0);
					if (ApplyOFF != null)
					{
						ApplyOFF();
					}
				}
			};
		}
	}
}
