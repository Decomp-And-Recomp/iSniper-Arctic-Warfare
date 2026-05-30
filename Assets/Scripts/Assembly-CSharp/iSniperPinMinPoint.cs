using UnityEngine;

public class iSniperPinMinPoint : iSniperEnemyPoint
{
	private void Awake()
	{
		m_strType = "PinMin";
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = PinMinColor;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
