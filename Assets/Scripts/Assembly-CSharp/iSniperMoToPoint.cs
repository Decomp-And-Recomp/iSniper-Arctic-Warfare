using UnityEngine;

public class iSniperMoToPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "MoTo";
		if (m_Path.Length != 0)
		{
			m_bCanMove2Player = true;
		}
		else
		{
			m_bCanMove2Player = false;
		}
	}

	private void OnDrawGizmos()
	{
		if (m_Path.Length != 0)
		{
			m_bCanMove2Player = true;
		}
		else
		{
			m_bCanMove2Player = false;
		}
		Gizmos.color = MoToColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		for (int i = 0; i < m_Range.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Range[i].transform.position, 0.5f);
			if (i == m_Range.Length - 1)
			{
				Gizmos.DrawLine(m_Range[i].transform.position, m_Range[0].transform.position);
			}
			else
			{
				Gizmos.DrawLine(m_Range[i].transform.position, m_Range[i + 1].transform.position);
			}
		}
		if (m_Path.Length == 0)
		{
			return;
		}
		for (int j = 0; j < m_Range.Length; j++)
		{
			Gizmos.DrawLine(m_Range[j].transform.position, m_Path[0].transform.position);
		}
		for (int k = 0; k < m_Path.Length; k++)
		{
			Gizmos.DrawWireSphere(m_Path[k].transform.position, 0.5f);
			if (k != 0)
			{
				Gizmos.DrawLine(m_Path[k - 1].transform.position, m_Path[k].transform.position);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = MoToColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.green;
		for (int i = 0; i < m_Range.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Range[i].transform.position, 0.5f);
			if (i == m_Range.Length - 1)
			{
				Gizmos.DrawLine(m_Range[i].transform.position, m_Range[0].transform.position);
			}
			else
			{
				Gizmos.DrawLine(m_Range[i].transform.position, m_Range[i + 1].transform.position);
			}
		}
		if (m_Path.Length == 0)
		{
			return;
		}
		for (int j = 0; j < m_Range.Length; j++)
		{
			Gizmos.DrawLine(m_Range[j].transform.position, m_Path[0].transform.position);
		}
		for (int k = 0; k < m_Path.Length; k++)
		{
			Gizmos.DrawWireSphere(m_Path[k].transform.position, 0.5f);
			if (k != 0)
			{
				Gizmos.DrawLine(m_Path[k - 1].transform.position, m_Path[k].transform.position);
			}
		}
	}
}
