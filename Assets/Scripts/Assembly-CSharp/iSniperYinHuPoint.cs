using UnityEngine;

public class iSniperYinHuPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "YinHu";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = YinHuColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		if (m_Path.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < m_Path.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Path[i].transform.position, 0.5f);
			if (m_Path.Length - 1 != i)
			{
				Gizmos.DrawLine(m_Path[i].transform.position, m_Path[i + 1].transform.position);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = YinHuColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.green;
		if (m_Path.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < m_Path.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Path[i].transform.position, 0.5f);
			if (m_Path.Length - 1 != i)
			{
				Gizmos.DrawLine(m_Path[i].transform.position, m_Path[i + 1].transform.position);
			}
		}
	}
}
