using UnityEngine;

public class iSniperYinBaoPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "YinBao";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = YinBaoColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
