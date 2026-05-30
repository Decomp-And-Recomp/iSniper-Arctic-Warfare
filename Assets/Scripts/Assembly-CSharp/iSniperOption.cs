using UnityEngine;

public class iSniperOption : MonoBehaviour
{
	private iSniperOptionUIHelper uiHelper;

	private void Start()
	{
		uiHelper = base.gameObject.AddComponent<iSniperOptionUIHelper>() as iSniperOptionUIHelper;
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.Menu);
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (uiHelper.m_UIManagerRef.HandleInput(touch))
			{
			}
		}
	}
}
