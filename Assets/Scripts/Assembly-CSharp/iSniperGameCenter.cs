using UnityEngine;

public class iSniperGameCenter : MonoBehaviour
{
	private iSniperGameState m_GameState;

	private iSniperGunCenter m_GunCenter;

	private bool m_bIsNeedSaveData;

	private void Start()
	{
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		m_GunCenter = iSniperGameApp.GetInstance().m_GunCenter;
	}

	public void SentInfo2GameCenter()
	{
	}

	public void CheckIsSuccess()
	{
	}
}
