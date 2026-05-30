using UnityEngine;

public class iSniperZhuangJiaPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "ZhuangJia";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = ZhuangJiaColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		if (m_Path.Length != 0)
		{
			Gizmos.DrawWireSphere(m_Path[0].transform.position, 0.5f);
			Gizmos.DrawWireSphere(m_Path[1].transform.position, 0.5f);
			Gizmos.DrawLine(base.transform.position, m_Path[0].transform.position);
			Gizmos.DrawLine(m_Path[0].transform.position, m_Path[1].transform.position);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = ZhuangJiaColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.green;
		if (m_Path.Length != 0)
		{
			Gizmos.DrawWireSphere(m_Path[0].transform.position, 0.5f);
			Gizmos.DrawWireSphere(m_Path[1].transform.position, 0.5f);
			Gizmos.DrawLine(base.transform.position, m_Path[0].transform.position);
			Gizmos.DrawLine(m_Path[0].transform.position, m_Path[1].transform.position);
		}
	}
}
