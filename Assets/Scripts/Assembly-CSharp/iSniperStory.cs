using UnityEngine;

public class iSniperStory : MonoBehaviour
{
	private iSniperStoryUIHelper m_uiHelper;

	private void Start()
	{
		m_uiHelper = base.gameObject.AddComponent<iSniperStoryUIHelper>() as iSniperStoryUIHelper;
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		if (!(null != m_uiHelper.m_UIManagerRef))
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
