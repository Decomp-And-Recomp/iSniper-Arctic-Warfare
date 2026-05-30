using UnityEngine;

public class iSniperMusicMgr
{
	public enum MusicType
	{
		No = 0,
		Mission = 1,
		Fight = 2,
		Hill = 3,
		Menu = 4,
		Metal = 5,
		Snow = 6,
		Storm = 7,
		Story = 8
	}

	private static iSniperMusicMgr instance;

	private MusicType m_MusicType;

	private GameObject m_MusicObj;

	public static iSniperMusicMgr Instance()
	{
		if (instance == null)
		{
			instance = new iSniperMusicMgr();
		}
		return instance;
	}

	public void PlayMusic(MusicType type)
	{
		if (type == m_MusicType)
		{
			return;
		}
		if (null != m_MusicObj)
		{
			m_MusicObj.GetComponent<AudioSource>().Stop();
			Object.Destroy(m_MusicObj);
			m_MusicObj = null;
		}
		m_MusicType = type;
		string path = "iSniper3D/Music/" + type.ToString() + "_Prefab";
		GameObject gameObject = Resources.Load<GameObject>(path);
		Debug.Log(gameObject);
		if (gameObject == null)
		{
			return;
		}
		m_MusicObj = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		if (!(m_MusicObj == null))
		{
			Object.DontDestroyOnLoad(m_MusicObj);
			GameObject gameObject2 = GameObject.Find("Main Camera");
			m_MusicObj.transform.position = gameObject2.transform.position;
			m_MusicObj.name = "MusicObj";
			m_MusicObj.GetComponent<AudioSource>().loop = true;
			if (iSniperGameApp.GetInstance().m_GameState.m_bMusicOn)
			{
				m_MusicObj.GetComponent<AudioSource>().volume = 1f;
			}
			else
			{
				m_MusicObj.GetComponent<AudioSource>().volume = 0f;
			}
			m_MusicObj.GetComponent<AudioSource>().Play();
		}
	}

	public void TurnMusic()
	{
		if (!(m_MusicObj == null))
		{
			if (iSniperGameApp.GetInstance().m_GameState.m_bMusicOn)
			{
				m_MusicObj.GetComponent<AudioSource>().volume = 1f;
			}
			else
			{
				m_MusicObj.GetComponent<AudioSource>().volume = 0f;
			}
		}
	}
}
