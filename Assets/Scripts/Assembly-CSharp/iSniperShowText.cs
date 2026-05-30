using UnityEngine;

public class iSniperShowText : MonoBehaviour
{
	private iSniperShowTextUIHelper m_uiHelper;

	private void Start()
	{
		m_uiHelper = base.gameObject.AddComponent<iSniperShowTextUIHelper>() as iSniperShowTextUIHelper;
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
	}
}
