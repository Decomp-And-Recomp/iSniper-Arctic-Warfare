using UnityEngine;

public class iSniperLoading : MonoBehaviour
{
	private iSniperLoadingUIHelper uiHelper;

	private void Start()
	{
		uiHelper = base.gameObject.AddComponent<iSniperLoadingUIHelper>() as iSniperLoadingUIHelper;
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
	}
}
