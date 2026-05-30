using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;

public class iSniperGameState
{
	public enum Result
	{
		kSuccess = 0,
		kTimeup = 1,
		kBloodup = 2
	}

	public enum LastScene
	{
		kMenu = 0,
		kStoryStart = 1
	}

	internal class ArrayListCompare : IComparer
	{
		private System.Random r = new System.Random();

		public int Compare(object x, object y)
		{
			return r.Next(-1, 1);
		}
	}

	public bool m_bStoryMode;

	public bool m_bBootCampsMode;

	public bool m_bFirstMenu = true;

	public bool m_bArcBootCamp;

	public string m_strSceneName;

	public Result m_GameResult;

	public LastScene m_LastScene;

	public bool m_bTiltHolding;

	public int m_iPlayerCurrentHealth;

	public int m_iPlayerScore;

	public int m_iStageIndex;

	public int m_iKillEnemyNum;

	public int m_iFireNum;

	public int m_iHeadshotNum;

	public int m_iHitNum;

	public float m_fGameTime;

	public float m_fGameTimeBonus;

	public int m_iGameCash;

	public int m_iGameExperience;

	public float m_fCurrentGunFovDeltaPercent;

	public bool m_bStoryStart;

	public bool m_bIsEndedScene;

	public bool m_bLastArcLock;

	public int m_iArcCurScene;

	public int m_iArcCurStage;

	public float m_fNewVersion;

	public float m_fVersion;

	public bool m_bSoundOn;

	public bool m_bMusicOn;

	public bool m_bIsTiltControl;

	public bool m_bIsInvertYAixs;

	public float m_fCurrentSensitivty;

	public string m_strPlayerUserGunName;

	public int m_iPlayerCash;

	public int m_iPlayerLevel;

	public int m_iPlayer2NextLevelExperience;

	public int m_iPlayerCurrentScene;

	public bool m_bArcadeIsLock;

	public int m_iArcDaysNum;

	public int m_iArcLastScene;

	public int m_iArcLastStage;

	public bool m_bArcWinState;

	public bool m_bArcLockScene;

	public int m_iAchWorst;

	public int m_iAchElite;

	public int m_iAchGunsmith;

	public int m_iAchCollateral;

	public int m_iAchNewHand;

	public int m_iAchGod;

	public int m_iAchBest;

	public int m_iAchFever;

	public bool m_bLeaderBoard;

	public int m_iAchKillPinMinNum;

	public int m_iAchKillEnemyNum;

	public int m_iAchHeadShotNum;

	public int m_iLoginTimes;

	public bool m_bBootCampsOver;

	public int m_iRangge = 625432741;

	public void Initialize()
	{
		ResetGameData();
		m_bIsEndedScene = false;
		m_fVersion = 1f;
		m_fNewVersion = 1.06f;
		m_bSoundOn = true;
		m_bMusicOn = true;
		m_bIsTiltControl = false;
		m_bIsInvertYAixs = false;
		m_fCurrentSensitivty = 27.5f;
		m_iPlayerCash = 0;
		m_iPlayerLevel = 1;
		m_bArcadeIsLock = false;
		m_bArcWinState = false;
		m_bArcBootCamp = false;
		m_bStoryMode = true;
		m_bBootCampsMode = false;
		m_iArcLastScene = 1;
		m_iArcLastStage = 1;
		m_iArcDaysNum = 1;
		ResetData();
		LoadData();
		m_iPlayerCurrentHealth = GetPlayerHealth();
		if (m_iArcLastScene <= 0 || m_iArcCurScene > 8)
		{
			m_iArcLastScene = UnityEngine.Random.Range(1, 8);
		}
		if (m_iArcLastStage <= 0)
		{
			m_iArcLastStage = 1;
		}
	}

	public iSniperGunProperty GetUseGunProperty()
	{
		return iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_strPlayerUserGunName);
	}

	public float GetGameTime()
	{
		return 30 + (m_iPlayerLevel - 1) * 5;
	}

	public int GetPlayerHealth()
	{
		return 100 + (m_iPlayerLevel - 1) * 10;
	}

	public int GetPlayerExperience()
	{
		int num = 0;
		for (int i = 0; i < m_iPlayerLevel; i++)
		{
			num += i;
		}
		return num * 50 + (m_iPlayerLevel - 1) * 200 + m_iPlayer2NextLevelExperience;
	}

	public int GetNeedExperience2NextLevel()
	{
		if (m_iPlayerLevel >= 50)
		{
			m_iPlayer2NextLevelExperience = 0;
			return 0;
		}
		return 200 + m_iPlayerLevel * 50 - m_iPlayer2NextLevelExperience;
	}

	public int RecomputeLevel()
	{
		if (m_iPlayerLevel >= 50)
		{
			m_iPlayerLevel = 50;
			m_iPlayer2NextLevelExperience = 0;
			return m_iPlayerLevel;
		}
		while (GetNeedExperience2NextLevel() <= 0)
		{
			m_iPlayer2NextLevelExperience -= 200 + m_iPlayerLevel * 50;
			m_iPlayerLevel++;
			if (50 <= m_iPlayerLevel)
			{
				m_iPlayerLevel = 50;
				m_iPlayer2NextLevelExperience = 0;
				break;
			}
		}
		return m_iPlayerLevel;
	}

	public int RecomputeScene(int iDays, bool bLastWin)
	{
		Debug.Log("Enter RecomputeScene!");
		if (iDays % 10 == 0)
		{
			m_iArcCurScene = 8;
			m_iArcLastScene = m_iArcCurScene;
			SaveData();
			return m_iArcCurScene;
		}
		if (!bLastWin || m_bArcLockScene)
		{
			m_iArcCurScene = m_iArcLastScene;
			m_iArcLastScene = m_iArcCurScene;
			SaveData();
			return m_iArcCurScene;
		}
		if (iDays < 1)
		{
			iDays = 1;
		}
		switch ((iDays - 1) / 5)
		{
		case 0:
			m_iArcCurScene = 1;
			break;
		case 1:
			if (Utils.ProbabilityIsRandomHit(0.4f))
			{
				m_iArcCurScene = 1;
			}
			else
			{
				m_iArcCurScene = 2;
			}
			break;
		case 2:
			if (Utils.ProbabilityIsRandomHit(0.6f))
			{
				m_iArcCurScene = 3;
			}
			else
			{
				m_iArcCurScene = UnityEngine.Random.Range(1, 3);
			}
			break;
		case 3:
			if (Utils.ProbabilityIsRandomHit(0.55f))
			{
				m_iArcCurScene = 4;
			}
			else
			{
				m_iArcCurScene = UnityEngine.Random.Range(1, 4);
			}
			break;
		case 4:
			if (Utils.ProbabilityIsRandomHit(0.6f))
			{
				m_iArcCurScene = 5;
			}
			else
			{
				m_iArcCurScene = UnityEngine.Random.Range(1, 5);
			}
			break;
		case 5:
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				m_iArcCurScene = 6;
			}
			else
			{
				m_iArcCurScene = UnityEngine.Random.Range(1, 6);
			}
			break;
		default:
		{
			string text = m_iRangge.ToString();
			Debug.Log(text);
			ArrayList arrayList = new ArrayList();
			char[] array = text.ToCharArray();
			foreach (char c in array)
			{
				arrayList.Add(c);
			}
			IComparer comparer = new ArrayListCompare();
			arrayList.Sort(comparer);
			text = string.Empty;
			for (int j = 0; j < arrayList.Count; j++)
			{
				text += arrayList[j];
			}
			Debug.Log(text);
			int length = text.Length;
			Debug.Log(length);
			m_iArcCurScene = int.Parse(text[length - 1].ToString().Trim());
			text = text.Remove(length - 1);
			Debug.Log(m_iArcCurScene);
			Debug.Log(text);
			if (text.Length <= 0)
			{
				m_iRangge = 512437426;
			}
			else
			{
				m_iRangge = int.Parse(text.Trim());
			}
			break;
		}
		}
		m_iArcLastScene = m_iArcCurScene;
		SaveData();
		return m_iArcCurScene;
	}

	public int RecomputeStage(int iLastStage, bool bLastWin)
	{
		Debug.Log("Enter RecomputeStage!");
		m_bArcWinState = false;
		if (m_iArcCurScene == 8)
		{
			m_iArcCurStage = 1;
			Debug.Log("@1@" + m_iArcCurStage);
			m_iArcLastStage = m_iArcCurStage;
			SaveData();
			return m_iArcCurStage;
		}
		if (!bLastWin)
		{
			m_iArcCurStage = m_iArcLastStage;
			m_iArcLastStage = m_iArcCurStage;
			Debug.Log("@2@" + m_iArcCurStage);
			SaveData();
			return m_iArcCurStage;
		}
		GameObject gameObject = GameObject.Find("Scene_DATA");
		int childCount = gameObject.transform.GetChildCount();
		for (m_iArcCurStage = UnityEngine.Random.Range(1, childCount + 1); m_iArcCurStage == m_iArcLastStage; m_iArcCurStage = UnityEngine.Random.Range(1, childCount + 1))
		{
		}
		Debug.Log("@3@" + m_iArcCurStage);
		m_iArcLastStage = m_iArcCurStage;
		SaveData();
		return m_iArcCurStage;
	}

	public int RecomputeAddKillEnemySum(int iDays)
	{
		int num = 0;
		if (iDays % 10 == 0)
		{
			if (iDays >= 100)
			{
				iDays = 100;
			}
			return iDays / 20;
		}
		if (iDays >= 100)
		{
			iDays = 100;
		}
		return (iDays - 1) / 3;
	}

	public void ResetGameData()
	{
		m_bTiltHolding = false;
		m_iPlayerScore = 0;
		m_iStageIndex = 1;
		m_iKillEnemyNum = 0;
		m_iFireNum = 0;
		m_iHeadshotNum = 0;
		m_iHitNum = 0;
		m_fGameTime = 0f;
		m_fGameTimeBonus = 0f;
		m_iGameCash = 0;
		m_iGameExperience = 0;
		m_bStoryStart = true;
		m_fCurrentGunFovDeltaPercent = 0f;
		m_iPlayerCurrentHealth = GetPlayerHealth();
	}

	public bool IsHaveSaveData()
	{
		return 1 != m_iPlayerCurrentScene;
	}

	public void ResetData()
	{
		m_strPlayerUserGunName = "M21";
		m_iPlayerCurrentScene = 1;
	}

	private void LoadData()
	{
		string content = string.Empty;
		Utils.FileGetString("gamedata.xml", ref content);
		if (content.Length <= 0)
		{
			return;
		}
		content = XXTEAUtils.Decrypt(content, iSniperGameApp.GetInstance().GetKey());
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		XmlElement xmlElement = (XmlElement)documentElement;
		m_fVersion = float.Parse(xmlElement.GetAttribute("Version").Trim());
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			XmlElement xmlElement2 = (XmlElement)childNode;
			if ("Option" == childNode.Name)
			{
				m_bIsTiltControl = bool.Parse(xmlElement2.GetAttribute("TiltCtrl").Trim());
				m_bIsInvertYAixs = bool.Parse(xmlElement2.GetAttribute("InvertYAixs").Trim());
				m_fCurrentSensitivty = float.Parse(xmlElement2.GetAttribute("Sensitivity").Trim());
				m_bSoundOn = bool.Parse(xmlElement2.GetAttribute("Sound").Trim());
				m_bMusicOn = bool.Parse(xmlElement2.GetAttribute("Music").Trim());
			}
			else if ("Player" == childNode.Name)
			{
				m_strPlayerUserGunName = xmlElement2.GetAttribute("Gun").Trim();
				m_iPlayerCash = int.Parse(xmlElement2.GetAttribute("Cash").Trim());
				m_iPlayerLevel = int.Parse(xmlElement2.GetAttribute("Level").Trim());
				m_iPlayer2NextLevelExperience = int.Parse(xmlElement2.GetAttribute("ToNextLevelExperience").Trim());
				m_iPlayerCurrentScene = int.Parse(xmlElement2.GetAttribute("PlayScene").Trim());
			}
			else if ("Arcade" == childNode.Name)
			{
				m_bArcadeIsLock = bool.Parse(xmlElement2.GetAttribute("ArcLock").Trim());
				m_iArcDaysNum = int.Parse(xmlElement2.GetAttribute("DayNum").Trim());
				m_iArcLastScene = int.Parse(xmlElement2.GetAttribute("LastScene").Trim());
				m_iArcLastStage = int.Parse(xmlElement2.GetAttribute("LastStage").Trim());
				m_bArcWinState = bool.Parse(xmlElement2.GetAttribute("WinState").Trim());
				if (xmlElement2.HasAttribute("LockScene"))
				{
					m_bArcLockScene = bool.Parse(xmlElement2.GetAttribute("LockScene").Trim());
				}
			}
			else if ("Achieve" == childNode.Name)
			{
				m_iAchWorst = int.Parse(xmlElement2.GetAttribute("Worst").Trim());
				m_iAchElite = int.Parse(xmlElement2.GetAttribute("Elite").Trim());
				m_iAchGunsmith = int.Parse(xmlElement2.GetAttribute("Gunsmith").Trim());
				m_iAchCollateral = int.Parse(xmlElement2.GetAttribute("Collateral").Trim());
				m_iAchNewHand = int.Parse(xmlElement2.GetAttribute("NewHand").Trim());
				m_iAchGod = int.Parse(xmlElement2.GetAttribute("God").Trim());
				m_iAchBest = int.Parse(xmlElement2.GetAttribute("Best").Trim());
				m_iAchFever = int.Parse(xmlElement2.GetAttribute("Fever").Trim());
				m_bLeaderBoard = bool.Parse(xmlElement2.GetAttribute("LeaderBoard").Trim());
			}
			else if ("AchieveInfo" == childNode.Name)
			{
				m_iAchKillPinMinNum = int.Parse(xmlElement2.GetAttribute("AchKillPinMin").Trim());
				m_iAchKillEnemyNum = int.Parse(xmlElement2.GetAttribute("AchKillEnemy").Trim());
				m_iAchHeadShotNum = int.Parse(xmlElement2.GetAttribute("AchHeadShot").Trim());
			}
			else if ("LoginInfo" == childNode.Name)
			{
				m_iLoginTimes = int.Parse(xmlElement2.GetAttribute("LoginTimes").Trim());
				m_bBootCampsOver = bool.Parse(xmlElement2.GetAttribute("BootCampsOver").Trim());
				if (xmlElement2.HasAttribute("Probability"))
				{
					m_iRangge = int.Parse(xmlElement2.GetAttribute("Probability").Trim());
				}
			}
		}
		if (m_fNewVersion != m_fVersion)
		{
			m_fVersion = m_fNewVersion;
			m_iLoginTimes = 0;
			SaveData();
		}
	}

	public void SaveData()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "no");
		xmlDocument.AppendChild(newChild);
		XmlElement xmlElement = xmlDocument.CreateElement("Game");
		xmlElement.SetAttribute("Version", m_fVersion.ToString());
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("Option");
		xmlElement2.SetAttribute("TiltCtrl", m_bIsTiltControl.ToString());
		xmlElement2.SetAttribute("InvertYAixs", m_bIsInvertYAixs.ToString());
		xmlElement2.SetAttribute("Sensitivity", m_fCurrentSensitivty.ToString());
		xmlElement2.SetAttribute("Sound", m_bSoundOn.ToString());
		xmlElement2.SetAttribute("Music", m_bMusicOn.ToString());
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement3 = xmlDocument.CreateElement("Player");
		xmlElement3.SetAttribute("Gun", m_strPlayerUserGunName);
		xmlElement3.SetAttribute("Cash", m_iPlayerCash.ToString());
		xmlElement3.SetAttribute("Level", m_iPlayerLevel.ToString());
		xmlElement3.SetAttribute("ToNextLevelExperience", m_iPlayer2NextLevelExperience.ToString());
		xmlElement3.SetAttribute("PlayScene", m_iPlayerCurrentScene.ToString());
		xmlElement.AppendChild(xmlElement3);
		XmlElement xmlElement4 = xmlDocument.CreateElement("Arcade");
		xmlElement4.SetAttribute("ArcLock", m_bArcadeIsLock.ToString());
		xmlElement4.SetAttribute("DayNum", m_iArcDaysNum.ToString());
		xmlElement4.SetAttribute("LastScene", m_iArcLastScene.ToString());
		xmlElement4.SetAttribute("LastStage", m_iArcLastStage.ToString());
		xmlElement4.SetAttribute("WinState", m_bArcWinState.ToString());
		xmlElement4.SetAttribute("LockScene", m_bArcLockScene.ToString());
		xmlElement.AppendChild(xmlElement4);
		XmlElement xmlElement5 = xmlDocument.CreateElement("AchieveInfo");
		xmlElement5.SetAttribute("AchKillPinMin", m_iAchKillPinMinNum.ToString());
		xmlElement5.SetAttribute("AchKillEnemy", m_iAchKillEnemyNum.ToString());
		xmlElement5.SetAttribute("AchHeadShot", m_iAchHeadShotNum.ToString());
		xmlElement.AppendChild(xmlElement5);
		XmlElement xmlElement6 = xmlDocument.CreateElement("Achieve");
		xmlElement6.SetAttribute("Worst", m_iAchWorst.ToString());
		xmlElement6.SetAttribute("Elite", m_iAchElite.ToString());
		xmlElement6.SetAttribute("Gunsmith", m_iAchGunsmith.ToString());
		xmlElement6.SetAttribute("Collateral", m_iAchCollateral.ToString());
		xmlElement6.SetAttribute("NewHand", m_iAchNewHand.ToString());
		xmlElement6.SetAttribute("God", m_iAchGod.ToString());
		xmlElement6.SetAttribute("Best", m_iAchBest.ToString());
		xmlElement6.SetAttribute("Fever", m_iAchFever.ToString());
		xmlElement6.SetAttribute("LeaderBoard", m_bLeaderBoard.ToString());
		xmlElement.AppendChild(xmlElement6);
		XmlElement xmlElement7 = xmlDocument.CreateElement("LoginInfo");
		xmlElement7.SetAttribute("LoginTimes", m_iLoginTimes.ToString());
		xmlElement7.SetAttribute("BootCampsOver", m_bBootCampsOver.ToString());
		xmlElement7.SetAttribute("Probability", m_iRangge.ToString());
		xmlElement.AppendChild(xmlElement7);
		StringWriter stringWriter = new StringWriter();
		xmlDocument.Save(stringWriter);
		string content = XXTEAUtils.Encrypt(stringWriter.ToString(), iSniperGameApp.GetInstance().GetKey());
		Utils.FileSaveString("gamedata.xml", content);
	}
}
