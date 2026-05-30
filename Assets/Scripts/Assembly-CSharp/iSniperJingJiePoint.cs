using UnityEngine;

public class iSniperJingJiePoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "JingJie";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = JingJieColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
