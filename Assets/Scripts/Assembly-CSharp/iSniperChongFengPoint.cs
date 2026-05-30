using UnityEngine;

public class iSniperChongFengPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "ChongFeng";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = ChongFengColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		for (int i = 0; i < m_Path.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Path[i].transform.position, 0.5f);
			if (i == 0)
			{
				Gizmos.DrawLine(base.transform.position, m_Path[i].transform.position);
			}
			else
			{
				Gizmos.DrawLine(m_Path[i - 1].transform.position, m_Path[i].transform.position);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = ChongFengColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.green;
		for (int i = 0; i < m_Path.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Path[i].transform.position, 0.5f);
			if (i == 0)
			{
				Gizmos.DrawLine(base.transform.position, m_Path[i].transform.position);
			}
			else
			{
				Gizmos.DrawLine(m_Path[i - 1].transform.position, m_Path[i].transform.position);
			}
		}
	}
}
