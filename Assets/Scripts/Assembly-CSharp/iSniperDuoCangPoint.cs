using UnityEngine;

public class iSniperDuoCangPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "DuoCang";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = DuoCangColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
