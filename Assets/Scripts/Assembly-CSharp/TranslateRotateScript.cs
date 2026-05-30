using UnityEngine;

public class TranslateRotateScript : MonoBehaviour
{
	public Vector3 m_BeginPosition;

	public GameObject m_MarkObject;

	public GameObject m_NpcObject;

	private float m_fEndTime = 0.5f;

	private float m_fBeginTime;

	private int m_iRotateDirection;

	public float m_fTimeout;

	private float m_fStartTime;

	private float m_fAngle;

	private bool m_bShow;

	private iSniperGameScene m_GameScene;

	private AudioSource m_Audio;

	private void Start()
	{
		m_fBeginTime = 0f;
		m_iRotateDirection = ((Random.Range(0, 2) != 1) ? 1 : (-1));
		m_fStartTime = 0f;
		m_fAngle = 0f;
		m_bShow = false;
		base.GetComponent<Renderer>().enabled = false;
		m_GameScene = GameObject.Find("Main Camera").GetComponent<iSniperGame>().m_GameScene;
	}

	private void Update()
	{
		if (!m_bShow)
		{
			m_fStartTime += Time.deltaTime;
			if (m_fStartTime > m_fTimeout)
			{
				m_bShow = true;
				RandomSoundPlay();
			}
		}
		if (!m_bShow)
		{
			return;
		}
		base.GetComponent<Renderer>().enabled = !m_GameScene.m_UIHelper.IsAim();
		if (null != m_Audio)
		{
			if (base.GetComponent<Renderer>().enabled)
			{
				m_Audio.volume = 1f;
			}
			else
			{
				m_Audio.volume = 0f;
			}
		}
		m_fBeginTime += Time.deltaTime;
		float num = m_fBeginTime / m_fEndTime;
		m_fAngle += (float)m_iRotateDirection * m_fBeginTime * 3f;
		base.gameObject.transform.position = Vector3.Lerp(m_BeginPosition, m_MarkObject.transform.position, num);
		base.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, num));
		base.gameObject.transform.RotateAroundLocal(base.gameObject.transform.forward, m_fAngle);
		base.gameObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, num);
		if (num >= 1f)
		{
			iSniperNpcShell iSniperNpcShell2 = m_NpcObject.GetComponent("iSniperNpcShell") as iSniperNpcShell;
			iSniperNpcShell2.m_iSniperNpcRef.SetMarkShow();
			Object.Destroy(base.gameObject);
		}
	}

	private void RandomSoundPlay()
	{
		if (!iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			return;
		}
		int childCount = base.transform.GetChildCount();
		if (childCount != 0)
		{
			int num = Random.Range(0, 1000);
			int index = num % childCount;
			m_Audio = base.transform.GetChild(index).gameObject.GetComponent<AudioSource>();
			m_Audio.Play();
			if (base.GetComponent<Renderer>().enabled)
			{
				m_Audio.volume = 1f;
			}
			else
			{
				m_Audio.volume = 0f;
			}
		}
	}
}
