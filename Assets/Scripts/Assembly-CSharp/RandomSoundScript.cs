using UnityEngine;

public class RandomSoundScript : MonoBehaviour
{
	private void Start()
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			int childCount = base.transform.GetChildCount();
			if (childCount != 0)
			{
				int num = Random.Range(0, 1000);
				int index = num % childCount;
				base.transform.GetChild(index).gameObject.GetComponent<AudioSource>().Play();
			}
		}
	}
}
