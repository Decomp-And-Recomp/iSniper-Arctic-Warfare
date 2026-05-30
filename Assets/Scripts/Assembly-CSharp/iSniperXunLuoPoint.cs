using UnityEngine;

public class iSniperXunLuoPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "XunLuo";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = XunLuoColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.DrawWireSphere(base.transform.position, m_fMinDistance);
		Gizmos.DrawWireSphere(base.transform.position, m_fMaxDistance);
	}
}
