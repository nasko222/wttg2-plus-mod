using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesktopUI : MonoBehaviour
{
	private void Awake()
	{
		LookUp.DesktopUI = this;
	}

	public GraphicRaycaster DesktopGraphicRaycaster;

	public RectTransform DRAG_PLANE;

	public RectTransform WINDOW_HOLDER;

	public GameObject WIFI_ICON;

	public List<Sprite> WIFI_SPRITES;

	public GameObject WIFI_MENU;

	public ScrollRect NOTES_WINDOW_CONTENT;

	public RectTransform NOTES_WINDOW_OBJECT_HOLDER;

	public GameObject NOTES_NOTES_OBJECT;

	public ScrollRect SOURCE_WINDOW_CONTENT;

	public RectTransform SOURCE_WINDOW_OBJECT_HOLDER;

	public GameObject SOURCE_NOTES_OBJECT;

	public GameObject ANN_WINDOW_BROWSER_OBJECT;

	public AnnBTNBehaviour ANN_WINDOW_HOME_BTN;

	public AnnBTNBehaviour ANN_WINDOW_BACK_BTN;

	public AnnBTNBehaviour ANN_WINDOW_FORWARD_BTN;

	public AnnBTNBehaviour ANN_WINDOW_REFRESH_BTN;

	public AnnBTNBehaviour ANN_WINDOW_CODE_BTN;

	public Image ANN_WINDOW_LOADING_BAR;

	public InputField ANN_WINDOW_URL_BOX;

	public AnnBookmarkBehaviour ANN_WINDOW_BOOKMARK_BTN;

	public Image ANN_KEY_CUE;

	public GameObject ANN_WINDOW_BOOKMARKS_BTN;

	public GameObject ANN_WINDOW_BOOKMARKS_MENU;

	public GameObject ANN_WINDOW_BOOKMARKS_MENU_TAB_HOLDER;

	public GameObject ANN_WINDOW_BOOKMARKS_MENU_SCROLL_BAR;

	public GameObject ANN_WINDOW_BOOKMARKS_TAB_OBJECT;

	public CanvasGroup ANN_WINDOW_GLOBE;

	public GameObject ZERO_DAY_PRODUCTS_HOLDER;

	public RectTransform ZERO_DAY_PRODUCTS_CONTENT_HOLDER;

	public GameObject ZERO_DAY_PRODUCT_OBJECT;

	public GameObject ZERO_DAY_OFF_LINE_HOLDER;

	public GameObject SHADOW_MARKET_PRODUCTS_HOLDER;

	public RectTransform SHADOW_MARKET_PRODUCTS_CONTENT_HOLDER;

	public GameObject SHADOW_MARKET_PRODUCT_OBJECT;

	public GameObject SHADOW_MARKET_OFF_LINE_HOLDER;

	public GameObject MIN_WINDOW_TAB_OBJECT;

	public GameObject MIN_WINDOW_TAB_HOLDER;

	public GameObject DIALOG_HOLDER;

	public GameObject DIALOG_BG_OBJECT;

	public RectTransform MOTION_SENSOR_MENU;

	public CanvasGroup MOTION_SENSOR_MENU_ICON_IDLE;

	public CanvasGroup MOTION_SENSOR_MENU_ICON_ACTIVE;

	public RectTransform CURRENCY_MENU;

	public RectTransform VPN_MENU;

	public RectTransform TEXT_DOC_ICONS_PARENT;

	public InputField NOIR_TUNNEL_MASTER_KEY_INPUT;

	public Button NOIR_TUNNEL_UNLOCK_BUTTON;

	public Text NOIR_TUNNEL_COST1;

	public Text NOIR_TUNNEL_COST2;

	public GameObject TUT_BROWSER;

	public GameObject VWIPE_DIALOG_HOLDER;

	public Text VWIPE_INFO_TEXT;

	public Text VWIPE_VIRUS_FOUND_TEXT;

	public Image VWIPE_PROGRESS_BAR;
}
