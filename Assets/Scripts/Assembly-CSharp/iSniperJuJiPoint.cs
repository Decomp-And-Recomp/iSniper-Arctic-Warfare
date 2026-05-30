using UnityEngine;

public class iSniperJuJiPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "JuJi";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = JuJiColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
