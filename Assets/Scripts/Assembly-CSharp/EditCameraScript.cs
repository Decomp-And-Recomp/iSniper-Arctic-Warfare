using UnityEngine;

public class EditCameraScript : MonoBehaviour
{
	private Camera m_Camera;

	private GameObject m_sphere;

	public Vector3[] m_Position = new Vector3[4];

	public Vector3[] m_LookAt = new Vector3[4];

	public int index;

	private void Start()
	{
		m_Camera = base.gameObject.GetComponent("Camera") as Camera;
		m_sphere = GameObject.Find("Sphere");
		if (1 <= index && index <= 4)
		{
			m_Camera.transform.position = m_Position[index - 1];
			m_sphere.transform.position = m_LookAt[index - 1];
		}
		m_Camera.transform.eulerAngles = Vector3.zero;
		m_Camera.transform.LookAt(m_sphere.transform.position);
		m_Camera.fieldOfView = 45f;
		m_Camera.nearClipPlane = 0.1f;
		m_Camera.farClipPlane = 1000f;
	}

	private void Update()
	{
		m_Camera.transform.LookAt(m_sphere.transform.position);
	}
}
