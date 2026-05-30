using UnityEngine;

public class iSniperLoad : MonoBehaviour
{
	private iSniperLoadUIHelper uiHelper;

	private void Start()
	{
		uiHelper = base.gameObject.AddComponent<iSniperLoadUIHelper>() as iSniperLoadUIHelper;
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
	}
}
