using UnityEngine;

public class FollowObjectXZScript : MonoBehaviour
{
	public GameObject m_FollowObjectY;

	public GameObject m_FollowObjectXZ;

	public Vector3 m_Offset = Vector3.zero;

	private void Start()
	{
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		position.x = m_FollowObjectXZ.transform.position.x;
		position.z = m_FollowObjectXZ.transform.position.z;
		position.y = m_FollowObjectY.transform.position.y + m_Offset.y;
		base.transform.position = position;
	}
}
