using UnityEngine;

public class ApplicationPauseScript : MonoBehaviour
{
	public iSniperGameState m_GameState;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
	}

	private void Update()
	{
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			FlurryPlugin.logEvent("CurrentPlayerCash", m_GameState.m_iPlayerCash.ToString(), "iPlayerCash");
			FlurryPlugin.logEvent("PlayerLevel", m_GameState.m_iPlayerLevel.ToString(), "Level");
			FlurryPlugin.logEvent("GameProgres", m_GameState.m_iArcDaysNum.ToString(), "InfiniteMode");
			FlurryPlugin.logEvent("GameProgres", m_GameState.m_iPlayerCurrentScene.ToString(), "StoryMode");
		}
		else
		{
			FlurryPlugin.logEvent("LauchGame", "ApplicationPause", "LauchType");
		}
	}
}
