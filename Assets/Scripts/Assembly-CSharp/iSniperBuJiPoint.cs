using UnityEngine;

public class iSniperBuJiPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "BuJi";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = BuJiColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
