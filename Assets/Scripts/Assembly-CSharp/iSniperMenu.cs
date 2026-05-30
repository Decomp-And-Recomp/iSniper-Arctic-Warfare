using UnityEngine;

public class iSniperMenu : MonoBehaviour
{
	private iSniperMenuUIHelper uiHelper;

	private void Start()
	{
		uiHelper = base.gameObject.AddComponent<iSniperMenuUIHelper>() as iSniperMenuUIHelper;
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.Menu);
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		if (!(null != uiHelper.m_UIManagerRef))
		{
			return;
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (uiHelper.m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}
}
