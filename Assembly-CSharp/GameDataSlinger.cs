using System;

public static class GameDataSlinger
{
	public const int TIMER_POOL_COUNT = 10;

	public const int DOS_TWEEN_POOL_COUNT = 5;

	public const int DEFAULT_MOUSE_SENS = 2;

	public const float PENDING_CURRENCY_UPDATE_DELAY = 5f;

	public const float STARTING_DOS_COIN = 10f;

	public const float LEET_MODE_DOS_COIN = 1f;

	public const float DOS_COIN_REQUIRED_FOR_NOIR_TUNNEL = 250f;

	public const float DEFAULT_THREAT_TIME = 450f;

	public const string GAME_DATA_FILE_NAME = "WTTG2.gd";

	public const string OPTION_DATA_FILE_NAME = "WTTG2OPTDATA.gd";

	public const int OPTION_DATA_ID = 12;

	public const string CONTROLLER_INPUT_HORZ = "Horizontal";

	public const string CONTROLLER_INPUT_VERT = "Vertical";

	public const string CONTROLLER_INPUT_RUN = "Run";

	public const string CONTROLLER_INPUT_DUCK = "Duck";

	public const string CONTROLLER_INPUT_DESK_HORZ = "HorizontalDesk";

	public const string CONTROLLER_INPUT_RETURN = "Return";

	public const string CONTROLLER_INPUT_HEAD_TILT = "HeadTilt";

	public const string CONTROLLER_INPUT_FLASH_LIGHT = "FlashLight";

	public const string CONTROLLER_INPUT_LEFT_CLICK = "LeftClick";

	public const string CONTROLLER_INPUT_RIGHT_CLICK = "RightClick";

	public const string CONTROLLER_INPUT_CANCEL = "Cancel";

	public const string CONTROLLER_INPUT_UP = "Up";

	public const string CONTROLLER_INPUT_DOWN = "Down";

	public const string CONTROLLER_INPUT_LEFT = "Left";

	public const string CONTROLLER_INPUT_RIGHT = "Right";

	public const string CONTROLLER_INPUT_LEFT_CLICK_WEIGHTED = "LeftClickWeighted";

	public const string CONTROLLER_INPUT_ALPHA_ONE = "AlphaOne";

	public const string CONTROLLER_INPUT_ALPHA_TWO = "AlphaTwo";

	public const float WINDOW_MIN_WIDTH = 335f;

	public const float WINDOW_MIN_HEIGHT = 75f;

	public const float UI_DESKTOP_TOP_BAR_HEIGHT = 41f;

	public const float UI_DESKTOP_BOT_BAR_HEIGHT = 40f;

	public const float UI_WIFI_MENU_OPT_HEIGHT = 24f;

	public const float UI_WIFI_MENU_OPT_WIDTH = 219f;

	public const float UI_MOTION_SENSOR_MENU_OPT_HEIGHT = 24f;

	public const float UI_MOTION_SENSOR_MENU_OPT_WIDTH = 219f;

	public const float UI_CURRENCY_MENU_REMOTE_VPN_HEIGHT = 22f;

	public const float UI_VPN_MENU_OPT_HEIGHT = 24f;

	public const float UI_ANN_BOOKMARK_TAB_WIDTH = 237f;

	public const float UI_ANN_BOOKMARK_TAB_HEIGHT = 28f;

	public const float UI_ZERO_DAY_PRODUCT_HEIGHT = 126f;

	public const float UI_TERMINAL_LINE_HEIGHT = 20f;

	public const float UI_HACKING_STACK_PUSHER_GRID_BLOCK_WIDTH = 40f;

	public const float UI_HACKING_STACK_PUSHER_GRID_BLOCK_HEIGHT = 40f;

	public const float UI_HACKING_STACK_PUSHER_POPER_BLOCK_WIDTH = 32f;

	public const float UI_HACKING_STACK_PUSHER_POPER_BLOCK_HEIGHT = 32f;

	public const float UI_HACKING_NODE_HEX_OBJECT_WIDTH = 50f;

	public const float UI_HACKING_NODE_HEX_OBJECT_HEIGHT = 50f;

	public const float UI_HACKING_NODE_HEX_OBJECT_SPACING = 10f;

	public const AUDIO_HUB COMPUTER_AUDIO_HUB = AUDIO_HUB.COMPUTER_HUB;

	public const AUDIO_LAYER COMPUTER_AUDIO_LAYER_NAME = AUDIO_LAYER.COMPUTER_SFX;

	public const float COMPUTER_HUB_AUDIO_MIN = 0.6f;

	public const float COMPUTER_HUB_AUDIO_MAX = 1f;

	public const AUDIO_LAYER HACKING_AUDIO_LAYER = AUDIO_LAYER.HACKING_SFX;

	public const float COMPUTER_HACKING_ANI1 = 1.65f;

	public const float COMPUTER_HACKING_ANI2 = 0.75f;

	public const float COMPUTER_HACKING_ANI3 = 2.3f;

	public const float COMPUTER_HACKING_ANI4 = 3f;

	public const float GETTING_HACKED_LEEWAY_TIME = 30f;

	public const float PLAYER_BUSY_LEEYWAY_TIME = 5f;

	public const float PLAYER_UNKNOWN_LOCATION_LEEYWAY_TIME = 10f;

	public const float AUDIO_MUFFLE_OUTSIDE_LAYER_AMOUNT = 0f;
}
