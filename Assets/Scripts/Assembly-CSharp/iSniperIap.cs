using UnityEngine;

public class iSniperIap : MonoBehaviour
{
	private iSniperIapUIHelper m_uiHelper;

	private void Start()
	{
		m_uiHelper = base.gameObject.AddComponent<iSniperIapUIHelper>() as iSniperIapUIHelper;
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.Menu);
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (m_uiHelper.m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}
}
