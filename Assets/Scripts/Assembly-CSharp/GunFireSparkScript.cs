using UnityEngine;

public class GunFireSparkScript : MonoBehaviour
{
	public float m_fLifeTime = 0.1f;

	private float m_fStartTime;

	private bool m_bIncrease;

	private Vector3 m_NormalScale;

	private void Start()
	{
		m_fStartTime = 0f;
		m_bIncrease = true;
		m_NormalScale = base.transform.localScale;
	}

	private void Update()
	{
		float num = m_fLifeTime / 2f;
		if (m_bIncrease)
		{
			m_fStartTime += Time.deltaTime;
			if (m_fStartTime >= num)
			{
				m_fStartTime = num;
				m_bIncrease = false;
			}
		}
		else
		{
			m_fStartTime -= Time.deltaTime;
			if (m_fStartTime <= 0f)
			{
				m_fStartTime = 0f;
			}
		}
		base.transform.localScale = Vector3.Lerp(Vector3.zero, m_NormalScale, m_fStartTime / num);
		if (!m_bIncrease && m_fStartTime <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
