using UnityEngine;

public class FollowObjectScript : MonoBehaviour
{
	public GameObject m_FollowObject;

	public Vector3 m_Offset = Vector3.zero;

	private void Start()
	{
	}

	private void Update()
	{
		if (m_FollowObject != null)
		{
			base.transform.position = m_FollowObject.transform.position + m_Offset;
		}
	}
}
