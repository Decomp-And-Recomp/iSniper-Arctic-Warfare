using System.Collections;
using UnityEngine;

public class iSniperExplosionCheck : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(CheckScene());
	}

	private IEnumerator CheckScene()
	{
		yield return new WaitForSeconds(0.1f);
		GameObject.Find("Main Camera").GetComponent<iSniperGame>().m_GameScene.Explosion(base.transform.position);
	}
}
