using System.Xml;
using UnityEngine;

public class iSniperSceneCfg : MonoBehaviour
{
	public TextAsset m_XmlFile;

	public TextAsset m_XmlFileIn;

	public string m_strTime;

	public string m_strAddress;

	public string m_strDescribe;

	public string m_strScene;

	private void Awake()
	{
		XmlDocument xmlDocument = new XmlDocument();
		if (!iSniperGameApp.GetInstance().m_GameState.m_bArcadeIsLock && !iSniperGameApp.GetInstance().m_GameState.m_bStoryMode)
		{
			xmlDocument.LoadXml(m_XmlFileIn.text);
		}
		else
		{
			xmlDocument.LoadXml(m_XmlFile.text);
		}
		XmlNode documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement;
		m_strTime = xmlElement.GetAttribute("Time").Trim();
		m_strAddress = xmlElement.GetAttribute("Address").Trim();
		m_strDescribe = xmlElement.GetAttribute("Describe").Trim();
		m_strScene = xmlElement.GetAttribute("Scene").Trim();
		if (Utils.IsRetina())
		{
			m_strScene += "_HD";
		}
	}
}
