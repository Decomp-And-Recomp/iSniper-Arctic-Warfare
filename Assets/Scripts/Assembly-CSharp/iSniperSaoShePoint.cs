using UnityEngine;

public class iSniperSaoShePoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "SaoShe";
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
		Gizmos.color = SaoSheColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.blue;
		for (int i = 0; i < m_Range.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Range[i].transform.position, 0.5f);
			Gizmos.DrawLine(base.transform.position, m_Range[i].transform.position);
		}
		if (m_Path.Length == 0)
		{
			return;
		}
		Gizmos.DrawLine(base.transform.position, m_Path[0].transform.position);
		for (int j = 0; j < m_Range.Length; j++)
		{
			Gizmos.DrawLine(m_Range[j].transform.position, m_Path[0].transform.position);
			if (j == 1 || j == 3)
			{
				Gizmos.DrawLine(m_Range[j].transform.position, m_Path[2].transform.position);
			}
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
		Gizmos.color = SaoSheColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = Color.green;
		for (int i = 0; i < m_Range.Length; i++)
		{
			Gizmos.DrawWireSphere(m_Range[i].transform.position, 0.5f);
			Gizmos.DrawLine(base.transform.position, m_Range[i].transform.position);
		}
		if (m_Path.Length == 0)
		{
			return;
		}
		Gizmos.DrawLine(base.transform.position, m_Path[0].transform.position);
		for (int j = 0; j < m_Range.Length; j++)
		{
			Gizmos.DrawLine(m_Range[j].transform.position, m_Path[0].transform.position);
			if (j == 1 || j == 3)
			{
				Gizmos.DrawLine(m_Range[j].transform.position, m_Path[2].transform.position);
			}
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
