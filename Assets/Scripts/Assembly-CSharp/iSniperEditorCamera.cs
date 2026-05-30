using UnityEngine;

public class iSniperEditorCamera : MonoBehaviour
{
	private Camera m_Camera;

	private GameObject m_sphere;

	private void Start()
	{
		m_Camera = base.gameObject.GetComponent("Camera") as Camera;
		m_sphere = GameObject.Find("Sphere");
		m_Camera.transform.eulerAngles = Vector3.zero;
		m_Camera.transform.LookAt(m_sphere.transform.position);
		m_Camera.nearClipPlane = 1f;
		m_Camera.farClipPlane = 1000f;
	}

	private void Update()
	{
		m_Camera.transform.LookAt(m_sphere.transform.position);
	}
}
