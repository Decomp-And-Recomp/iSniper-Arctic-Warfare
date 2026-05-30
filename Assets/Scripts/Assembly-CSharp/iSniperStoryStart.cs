using UnityEngine;

public class iSniperStoryStart : MonoBehaviour
{
	private iSniperStoryStartUIHelper m_uiHelper;

	private void Start()
	{
		m_uiHelper = base.gameObject.AddComponent<iSniperStoryStartUIHelper>() as iSniperStoryStartUIHelper;
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
		iSniperGameApp.GetInstance().m_GameState.ResetGameData();
	}

	private void Update()
	{
		if (!(null != m_uiHelper) || !(null != m_uiHelper.m_UIManagerRef))
		{
			return;
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (m_uiHelper.m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}
}
