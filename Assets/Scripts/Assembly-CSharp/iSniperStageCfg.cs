using System;
using System.Collections;
using System.Xml;
using UnityEngine;

public class iSniperStageCfg : MonoBehaviour
{
	public TextAsset m_XmlFile;

	public TextAsset m_XmlFileIn;

	public int m_iMaxEnemyNum;

	public int m_iKillEnemySum;

	public float m_fRefreshEnemyTimeGap;

	public float m_fMaxObserverTime;

	public bool m_bMusicOn;

	public int m_iMaxPinMinNum;

	public float m_fRefreshPinMinTimeGap;

	public int m_iMaxBuJiNum;

	public int m_iBuJiDisappear;

	public int m_iAddHeath;

	public int m_iAddTime;

	public float m_fMinCameraDistance = 10f;

	public float m_fMaxCameraDistance = 20f;

	public ArrayList m_MovieStartTexts;

	public ArrayList m_MovieEndTexts;

	public Hashtable m_EnemyProperty;

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
		m_iMaxEnemyNum = int.Parse(xmlElement.GetAttribute("MaxEnemyNum").Trim());
		m_iKillEnemySum = int.Parse(xmlElement.GetAttribute("KillEnemySum").Trim());
		m_fRefreshEnemyTimeGap = float.Parse(xmlElement.GetAttribute("RefreshEnemyGap").Trim());
		m_fMaxObserverTime = float.Parse(xmlElement.GetAttribute("MaxObserverTime").Trim());
		if (xmlElement.HasAttribute("MaxPinMinNum"))
		{
			m_iMaxPinMinNum = int.Parse(xmlElement.GetAttribute("MaxPinMinNum").Trim());
		}
		if (xmlElement.HasAttribute("RefreshPinMinGap"))
		{
			m_fRefreshPinMinTimeGap = float.Parse(xmlElement.GetAttribute("RefreshPinMinGap").Trim());
		}
		if (xmlElement.HasAttribute("MusicOn"))
		{
			m_bMusicOn = bool.Parse(xmlElement.GetAttribute("MusicOn").Trim());
		}
		else
		{
			m_bMusicOn = false;
		}
		m_MovieStartTexts = new ArrayList();
		m_MovieEndTexts = new ArrayList();
		m_EnemyProperty = new Hashtable();
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("MovieStartText" == childNode.Name)
			{
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					XmlElement xmlElement2 = (XmlElement)childNode2;
					m_MovieStartTexts.Add(xmlElement2.GetAttribute("Value").Trim());
				}
			}
			else if ("MovieEndText" == childNode.Name)
			{
				foreach (XmlNode childNode3 in childNode.ChildNodes)
				{
					XmlElement xmlElement3 = (XmlElement)childNode3;
					m_MovieEndTexts.Add(xmlElement3.GetAttribute("Value").Trim());
				}
			}
			else if ("EnemyProperty" == childNode.Name)
			{
				foreach (XmlNode childNode4 in childNode.ChildNodes)
				{
					XmlElement xmlElement4 = (XmlElement)childNode4;
					iSniperEnemyProperty iSniperEnemyProperty2 = new iSniperEnemyProperty();
					iSniperEnemyProperty2.m_strName = childNode4.Name;
					iSniperEnemyProperty2.m_iScore = int.Parse(xmlElement4.GetAttribute("Score").Trim());
					iSniperEnemyProperty2.m_iExperience = int.Parse(xmlElement4.GetAttribute("Exp").Trim());
					iSniperEnemyProperty2.m_iHeath = int.Parse(xmlElement4.GetAttribute("Health").Trim());
					iSniperEnemyProperty2.m_iDamage = int.Parse(xmlElement4.GetAttribute("Damage").Trim());
					iSniperEnemyProperty2.m_fMinNTime = float.Parse(xmlElement4.GetAttribute("MinNTime").Trim());
					iSniperEnemyProperty2.m_fMaxNTime = float.Parse(xmlElement4.GetAttribute("MaxNTime").Trim());
					iSniperEnemyProperty2.m_fNRatio = float.Parse(xmlElement4.GetAttribute("NRatio").Trim());
					iSniperEnemyProperty2.m_fMinWTime = float.Parse(xmlElement4.GetAttribute("MinWT").Trim());
					iSniperEnemyProperty2.m_fMaxWTime = float.Parse(xmlElement4.GetAttribute("MaxWT").Trim());
					iSniperEnemyProperty2.m_fWRatio = float.Parse(xmlElement4.GetAttribute("WRatio").Trim());
					iSniperEnemyProperty2.m_iAHead = int.Parse(xmlElement4.GetAttribute("AHead").Trim());
					iSniperEnemyProperty2.m_iABody = int.Parse(xmlElement4.GetAttribute("ABody").Trim());
					iSniperEnemyProperty2.m_iAArm = int.Parse(xmlElement4.GetAttribute("AArm").Trim());
					iSniperEnemyProperty2.m_iALeg = int.Parse(xmlElement4.GetAttribute("ALeg").Trim());
					iSniperEnemyProperty2.m_iASpecial = int.Parse(xmlElement4.GetAttribute("ASpecial").Trim());
					iSniperEnemyProperty2.m_iMachineHealth = int.Parse(xmlElement4.GetAttribute("MachineHealth").Trim());
					m_EnemyProperty.Add(iSniperEnemyProperty2.m_strName, iSniperEnemyProperty2);
				}
			}
			else
			{
				if (!("BuJiProperty" == childNode.Name))
				{
					continue;
				}
				{
					IEnumerator enumerator5 = childNode.ChildNodes.GetEnumerator();
					try
					{
						if (enumerator5.MoveNext())
						{
							XmlNode xmlNode5 = (XmlNode)enumerator5.Current;
							XmlElement xmlElement5 = (XmlElement)xmlNode5;
							m_iBuJiDisappear = int.Parse(xmlElement5.GetAttribute("BuJiDisappear").Trim());
							m_iMaxBuJiNum = int.Parse(xmlElement5.GetAttribute("MaxBuJiNum").Trim());
							m_iAddHeath = int.Parse(xmlElement5.GetAttribute("AddHealth").Trim());
							m_iAddTime = int.Parse(xmlElement5.GetAttribute("AddTime").Trim());
						}
					}
					finally
					{
						IDisposable disposable = enumerator5 as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}
	}
}
