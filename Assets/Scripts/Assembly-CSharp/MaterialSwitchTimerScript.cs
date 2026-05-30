using UnityEngine;

public class MaterialSwitchTimerScript : MonoBehaviour
{
	public Material m_Material1;

	public Material m_Material2;

	public float m_fSwitchGap = 0.2f;

	public bool m_bIsFirst = true;

	private float m_iTimerCounter;

	private void Awake()
	{
		m_iTimerCounter = 0f;
		base.GetComponent<Renderer>().enabled = false;
	}

	private void Start()
	{
		m_Material1 = Object.Instantiate(m_Material1) as Material;
		m_Material2 = Object.Instantiate(m_Material2) as Material;
	}

	private void Update()
	{
		m_iTimerCounter += Time.deltaTime;
		if (m_iTimerCounter > m_fSwitchGap)
		{
			m_iTimerCounter = 0f;
			m_bIsFirst = !m_bIsFirst;
			base.GetComponent<Renderer>().material = ((!m_bIsFirst) ? m_Material2 : m_Material1);
		}
	}

	public void Show(bool bVisible)
	{
		base.GetComponent<Renderer>().enabled = bVisible;
	}
}
