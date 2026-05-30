using UnityEngine;

public class iSniperPlayVideo : MonoBehaviour
{
	private float count;

	private bool bLoad;

	private void Start()
	{
	}

	private void Update()
	{
		if (!bLoad)
		{
			count -= Time.deltaTime;
			if (count <= 0f)
			{
				Application.LoadLevel("iSniper.Load");
				bLoad = true;
			}
		}
	}
}
