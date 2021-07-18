using System;
using Steamworks;
using UnityEngine;

internal class SteamStatsAndAchievements : MonoBehaviour
{
	private void OnEnable()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		this.m_GameID = new CGameID(SteamUtils.GetAppID());
		this.m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.m_UserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored));
		this.m_bRequestedStats = false;
		this.m_bStatsValid = false;
	}

	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!this.m_bRequestedStats)
		{
			if (!SteamManager.Initialized)
			{
				this.m_bRequestedStats = true;
				return;
			}
			bool bRequestedStats = SteamUserStats.RequestCurrentStats();
			this.m_bRequestedStats = bRequestedStats;
		}
		if (!this.m_bStatsValid)
		{
			return;
		}
		foreach (SteamStatsAndAchievements.Achievement_t achievement_t in this.m_Achievements)
		{
			if (!achievement_t.m_bAchieved)
			{
				switch (achievement_t.m_eAchievementID)
				{
				case SteamStatsAndAchievements.Achievement.ACH_WIN_ONE_GAME:
					if (this.m_nTotalNumWins != 0)
					{
						this.UnlockAchievement(achievement_t);
					}
					break;
				case SteamStatsAndAchievements.Achievement.ACH_WIN_100_GAMES:
					if (this.m_nTotalNumWins >= 100)
					{
						this.UnlockAchievement(achievement_t);
					}
					break;
				case SteamStatsAndAchievements.Achievement.ACH_TRAVEL_FAR_ACCUM:
					if (this.m_flTotalFeetTraveled >= 5280f)
					{
						this.UnlockAchievement(achievement_t);
					}
					break;
				case SteamStatsAndAchievements.Achievement.ACH_TRAVEL_FAR_SINGLE:
					if (this.m_flGameFeetTraveled >= 500f)
					{
						this.UnlockAchievement(achievement_t);
					}
					break;
				}
			}
		}
		if (this.m_bStoreStats)
		{
			SteamUserStats.SetStat("NumGames", this.m_nTotalGamesPlayed);
			SteamUserStats.SetStat("NumWins", this.m_nTotalNumWins);
			SteamUserStats.SetStat("NumLosses", this.m_nTotalNumLosses);
			SteamUserStats.SetStat("FeetTraveled", this.m_flTotalFeetTraveled);
			SteamUserStats.SetStat("MaxFeetTraveled", this.m_flMaxFeetTraveled);
			SteamUserStats.UpdateAvgRateStat("AverageSpeed", this.m_flGameFeetTraveled, this.m_flGameDurationSeconds);
			SteamUserStats.GetStat("AverageSpeed", out this.m_flAverageSpeed);
			bool flag = SteamUserStats.StoreStats();
			this.m_bStoreStats = !flag;
		}
	}

	public void AddDistanceTraveled(float flDistance)
	{
		this.m_flGameFeetTraveled += flDistance;
	}

	private void UnlockAchievement(SteamStatsAndAchievements.Achievement_t achievement)
	{
		achievement.m_bAchieved = true;
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
		this.m_bStoreStats = true;
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_eResult == EResult.k_EResultOK)
			{
				Debug.Log("Received stats and achievements from Steam\n");
				this.m_bStatsValid = true;
				foreach (SteamStatsAndAchievements.Achievement_t achievement_t in this.m_Achievements)
				{
					bool achievement = SteamUserStats.GetAchievement(achievement_t.m_eAchievementID.ToString(), out achievement_t.m_bAchieved);
					if (achievement)
					{
						achievement_t.m_strName = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "name");
						achievement_t.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_eAchievementID.ToString(), "desc");
					}
					else
					{
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement_t.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
					}
				}
				SteamUserStats.GetStat("NumGames", out this.m_nTotalGamesPlayed);
				SteamUserStats.GetStat("NumWins", out this.m_nTotalNumWins);
				SteamUserStats.GetStat("NumLosses", out this.m_nTotalNumLosses);
				SteamUserStats.GetStat("FeetTraveled", out this.m_flTotalFeetTraveled);
				SteamUserStats.GetStat("MaxFeetTraveled", out this.m_flMaxFeetTraveled);
				SteamUserStats.GetStat("AverageSpeed", out this.m_flAverageSpeed);
			}
			else
			{
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_eResult == EResult.k_EResultOK)
			{
				Debug.Log("StoreStats - success");
			}
			else if (pCallback.m_eResult == EResult.k_EResultInvalidParam)
			{
				Debug.Log("StoreStats - some failed to validate");
				this.OnUserStatsReceived(new UserStatsReceived_t
				{
					m_eResult = EResult.k_EResultOK,
					m_nGameID = (ulong)this.m_GameID
				});
			}
			else
			{
				Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if ((ulong)this.m_GameID == pCallback.m_nGameID)
		{
			if (pCallback.m_nMaxProgress == 0u)
			{
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else
			{
				Debug.Log(string.Concat(new object[]
				{
					"Achievement '",
					pCallback.m_rgchAchievementName,
					"' progress callback, (",
					pCallback.m_nCurProgress,
					",",
					pCallback.m_nMaxProgress,
					")"
				}));
			}
		}
	}

	public void Render()
	{
		if (!SteamManager.Initialized)
		{
			GUILayout.Label("Steamworks not Initialized", new GUILayoutOption[0]);
			return;
		}
		GUILayout.Label("m_ulTickCountGameStart: " + this.m_ulTickCountGameStart, new GUILayoutOption[0]);
		GUILayout.Label("m_flGameDurationSeconds: " + this.m_flGameDurationSeconds, new GUILayoutOption[0]);
		GUILayout.Label("m_flGameFeetTraveled: " + this.m_flGameFeetTraveled, new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUILayout.Label("NumGames: " + this.m_nTotalGamesPlayed, new GUILayoutOption[0]);
		GUILayout.Label("NumWins: " + this.m_nTotalNumWins, new GUILayoutOption[0]);
		GUILayout.Label("NumLosses: " + this.m_nTotalNumLosses, new GUILayoutOption[0]);
		GUILayout.Label("FeetTraveled: " + this.m_flTotalFeetTraveled, new GUILayoutOption[0]);
		GUILayout.Label("MaxFeetTraveled: " + this.m_flMaxFeetTraveled, new GUILayoutOption[0]);
		GUILayout.Label("AverageSpeed: " + this.m_flAverageSpeed, new GUILayoutOption[0]);
		GUILayout.BeginArea(new Rect((float)(Screen.width - 300), 0f, 300f, 800f));
		foreach (SteamStatsAndAchievements.Achievement_t achievement_t in this.m_Achievements)
		{
			GUILayout.Label(achievement_t.m_eAchievementID.ToString(), new GUILayoutOption[0]);
			GUILayout.Label(achievement_t.m_strName + " - " + achievement_t.m_strDescription, new GUILayoutOption[0]);
			GUILayout.Label("Achieved: " + achievement_t.m_bAchieved, new GUILayoutOption[0]);
			GUILayout.Space(20f);
		}
		if (GUILayout.Button("RESET STATS AND ACHIEVEMENTS", new GUILayoutOption[0]))
		{
			SteamUserStats.ResetAllStats(true);
			SteamUserStats.RequestCurrentStats();
		}
		GUILayout.EndArea();
	}

	private SteamStatsAndAchievements.Achievement_t[] m_Achievements = new SteamStatsAndAchievements.Achievement_t[]
	{
		new SteamStatsAndAchievements.Achievement_t(SteamStatsAndAchievements.Achievement.ACH_WIN_ONE_GAME, "Winner", string.Empty),
		new SteamStatsAndAchievements.Achievement_t(SteamStatsAndAchievements.Achievement.ACH_WIN_100_GAMES, "Champion", string.Empty),
		new SteamStatsAndAchievements.Achievement_t(SteamStatsAndAchievements.Achievement.ACH_TRAVEL_FAR_ACCUM, "Interstellar", string.Empty),
		new SteamStatsAndAchievements.Achievement_t(SteamStatsAndAchievements.Achievement.ACH_TRAVEL_FAR_SINGLE, "Orbiter", string.Empty)
	};

	private CGameID m_GameID;

	private bool m_bRequestedStats;

	private bool m_bStatsValid;

	private bool m_bStoreStats;

	private float m_flGameFeetTraveled;

	private float m_ulTickCountGameStart;

	private double m_flGameDurationSeconds;

	private int m_nTotalGamesPlayed;

	private int m_nTotalNumWins;

	private int m_nTotalNumLosses;

	private float m_flTotalFeetTraveled;

	private float m_flMaxFeetTraveled;

	private float m_flAverageSpeed;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	protected Callback<UserStatsStored_t> m_UserStatsStored;

	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	private enum Achievement
	{
		ACH_WIN_ONE_GAME,
		ACH_WIN_100_GAMES,
		ACH_HEAVY_FIRE,
		ACH_TRAVEL_FAR_ACCUM,
		ACH_TRAVEL_FAR_SINGLE
	}

	private class Achievement_t
	{
		public Achievement_t(SteamStatsAndAchievements.Achievement achievementID, string name, string desc)
		{
			this.m_eAchievementID = achievementID;
			this.m_strName = name;
			this.m_strDescription = desc;
			this.m_bAchieved = false;
		}

		public SteamStatsAndAchievements.Achievement m_eAchievementID;

		public string m_strName;

		public string m_strDescription;

		public bool m_bAchieved;
	}
}
