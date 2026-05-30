using UnityEngine;

public class iSniperYouJiPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "YouJi";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = YouJiColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
