using UnityEngine;

public class iSniperWeaponShop : MonoBehaviour
{
	private iSniperWeaponShopUIHelper m_uiHelper;

	private void Start()
	{
		m_uiHelper = base.gameObject.AddComponent<iSniperWeaponShopUIHelper>() as iSniperWeaponShopUIHelper;
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
