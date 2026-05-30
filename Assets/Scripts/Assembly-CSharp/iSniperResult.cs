using UnityEngine;

public class iSniperResult : MonoBehaviour
{
	private iSniperResultUIHelper m_uiHelper;

	private void Start()
	{
		m_uiHelper = base.gameObject.AddComponent<iSniperResultUIHelper>() as iSniperResultUIHelper;
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
