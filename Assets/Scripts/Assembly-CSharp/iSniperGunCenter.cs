using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;

public class iSniperGunCenter
{
	public Hashtable m_GunCfg;

	public ArrayList m_arrayGunCfg;

	public void Initialize()
	{
		m_GunCfg = new Hashtable();
		m_arrayGunCfg = new ArrayList();
		LoadStaticCfg();
		LoadUserGunCfg();
	}

	public iSniperGunProperty GetGunProperty(string gunName)
	{
		return (iSniperGunProperty)m_GunCfg[gunName];
	}

	public iSniperGunProperty GetGunProperty(int index)
	{
		return (iSniperGunProperty)m_arrayGunCfg[index];
	}

	public int GetGunIndex(string gunName)
	{
		return ((iSniperGunProperty)m_GunCfg[gunName]).m_iIndex;
	}

	public bool GetGunAllStar(int index)
	{
		iSniperGunProperty gunProperty = GetGunProperty(index);
		int maxHarmStar = gunProperty.GetMaxHarmStar();
		int currnentHarmStar = gunProperty.GetCurrnentHarmStar();
		if (maxHarmStar > currnentHarmStar)
		{
			return false;
		}
		int maxSliencerStar = gunProperty.GetMaxSliencerStar();
		int currentSliencerStar = gunProperty.GetCurrentSliencerStar();
		if (maxSliencerStar > currentSliencerStar)
		{
			return false;
		}
		int maxFireStar = gunProperty.GetMaxFireStar();
		int currentFireStar = gunProperty.GetCurrentFireStar();
		if (maxFireStar > currentFireStar)
		{
			return false;
		}
		int maxClipStar = gunProperty.GetMaxClipStar();
		int currentClipStar = gunProperty.GetCurrentClipStar();
		if (maxClipStar > currentClipStar)
		{
			return false;
		}
		int maxReloadStar = gunProperty.GetMaxReloadStar();
		int currentReloadStar = gunProperty.GetCurrentReloadStar();
		if (maxReloadStar > currentReloadStar)
		{
			return false;
		}
		int maxZoomStar = gunProperty.GetMaxZoomStar();
		int currentZoomStar = gunProperty.GetCurrentZoomStar();
		if (maxZoomStar > currentZoomStar)
		{
			return false;
		}
		return true;
	}

	private void LoadStaticCfg()
	{
		XmlDocument xmlDocument = new XmlDocument();
		GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("iSniper3D/Config/GunConfig")) as GameObject;
		iSniperGunCfg iSniperGunCfg2 = gameObject.GetComponent("iSniperGunCfg") as iSniperGunCfg;
		xmlDocument.LoadXml(iSniperGunCfg2.m_XmlFile.text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Item" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				iSniperGunProperty iSniperGunProperty2 = new iSniperGunProperty();
				iSniperGunProperty2.m_strName = xmlElement.GetAttribute("Name").Trim();
				iSniperGunProperty2.m_iPrice = int.Parse(xmlElement.GetAttribute("Price").Trim());
				iSniperGunProperty2.m_iHarmLevel = int.Parse(xmlElement.GetAttribute("HL").Trim());
				iSniperGunProperty2.m_iHarmMaxUpdateNum = int.Parse(xmlElement.GetAttribute("HU").Trim());
				iSniperGunProperty2.m_iHarmCash = int.Parse(xmlElement.GetAttribute("HC").Trim());
				iSniperGunProperty2.m_iSliencerLevel = int.Parse(xmlElement.GetAttribute("SL").Trim());
				iSniperGunProperty2.m_iSliencerMaxUpdateNum = int.Parse(xmlElement.GetAttribute("SU").Trim());
				iSniperGunProperty2.m_iSliencerCash = int.Parse(xmlElement.GetAttribute("SC").Trim());
				iSniperGunProperty2.m_iFireLevel = int.Parse(xmlElement.GetAttribute("FL").Trim());
				iSniperGunProperty2.m_iFireMaxUpdateNum = int.Parse(xmlElement.GetAttribute("FU").Trim());
				iSniperGunProperty2.m_iFireCash = int.Parse(xmlElement.GetAttribute("FC").Trim());
				iSniperGunProperty2.m_iClipLevel = int.Parse(xmlElement.GetAttribute("CL").Trim());
				iSniperGunProperty2.m_iClipMaxUpdateNum = int.Parse(xmlElement.GetAttribute("CU").Trim());
				iSniperGunProperty2.m_iClipCash = int.Parse(xmlElement.GetAttribute("CC").Trim());
				iSniperGunProperty2.m_iReloadLevel = int.Parse(xmlElement.GetAttribute("RL").Trim());
				iSniperGunProperty2.m_iReloadMaxUpdateNum = int.Parse(xmlElement.GetAttribute("RU").Trim());
				iSniperGunProperty2.m_iReloadCash = int.Parse(xmlElement.GetAttribute("RC").Trim());
				iSniperGunProperty2.m_iZoomLevel = int.Parse(xmlElement.GetAttribute("ZL").Trim());
				iSniperGunProperty2.m_iZoomMaxUpdateNum = int.Parse(xmlElement.GetAttribute("ZU").Trim());
				iSniperGunProperty2.m_iZoomCash = int.Parse(xmlElement.GetAttribute("ZC").Trim());
				iSniperGunProperty2.m_iState = int.Parse(xmlElement.GetAttribute("Buy").Trim());
				iSniperGunProperty2.m_strSound = xmlElement.GetAttribute("Sound").Trim();
				iSniperGunProperty2.m_iIndex = m_arrayGunCfg.Count;
				m_GunCfg.Add(iSniperGunProperty2.m_strName, iSniperGunProperty2);
				m_arrayGunCfg.Add(iSniperGunProperty2);
			}
		}
	}

	private void LoadUserGunCfg()
	{
		string content = string.Empty;
		Utils.FileGetString("guncfg_user.xml", ref content);
		if (content.Length <= 0)
		{
			return;
		}
		content = XXTEAUtils.Decrypt(content, iSniperGameApp.GetInstance().GetKey());
		if (content.Length <= 0)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(content);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Item" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				iSniperGunProperty gunProperty = GetGunProperty(xmlElement.GetAttribute("Name").Trim());
				gunProperty.m_iState = int.Parse(xmlElement.GetAttribute("Buy").Trim());
				gunProperty.m_iHarmCurrentUpdateNum = int.Parse(xmlElement.GetAttribute("HU").Trim());
				gunProperty.m_iSliencerCurrentUpdateNum = int.Parse(xmlElement.GetAttribute("SU").Trim());
				gunProperty.m_iFireCurrentUpdateNum = int.Parse(xmlElement.GetAttribute("FU").Trim());
				gunProperty.m_iClipCurrentUpdateNum = int.Parse(xmlElement.GetAttribute("CU").Trim());
				gunProperty.m_iReloadCurrentUpdateNum = int.Parse(xmlElement.GetAttribute("RU").Trim());
				gunProperty.m_iZoomCurrentUpdateNum = int.Parse(xmlElement.GetAttribute("ZU").Trim());
			}
		}
	}

	public void SaveUserGunCfg()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
		xmlDocument.AppendChild(newChild);
		XmlElement xmlElement = xmlDocument.CreateElement("Gun");
		xmlDocument.AppendChild(xmlElement);
		for (int i = 0; i < m_arrayGunCfg.Count; i++)
		{
			iSniperGunProperty iSniperGunProperty2 = (iSniperGunProperty)m_arrayGunCfg[i];
			XmlElement xmlElement2 = xmlDocument.CreateElement("Item");
			xmlElement2.SetAttribute("Name", iSniperGunProperty2.m_strName);
			xmlElement2.SetAttribute("Buy", iSniperGunProperty2.m_iState.ToString());
			xmlElement2.SetAttribute("HU", iSniperGunProperty2.m_iHarmCurrentUpdateNum.ToString());
			xmlElement2.SetAttribute("SU", iSniperGunProperty2.m_iSliencerCurrentUpdateNum.ToString());
			xmlElement2.SetAttribute("FU", iSniperGunProperty2.m_iFireCurrentUpdateNum.ToString());
			xmlElement2.SetAttribute("CU", iSniperGunProperty2.m_iClipCurrentUpdateNum.ToString());
			xmlElement2.SetAttribute("RU", iSniperGunProperty2.m_iReloadCurrentUpdateNum.ToString());
			xmlElement2.SetAttribute("ZU", iSniperGunProperty2.m_iZoomCurrentUpdateNum.ToString());
			xmlElement.AppendChild(xmlElement2);
		}
		StringWriter stringWriter = new StringWriter();
		xmlDocument.Save(stringWriter);
		string content = XXTEAUtils.Encrypt(stringWriter.ToString(), iSniperGameApp.GetInstance().GetKey());
		Utils.FileSaveString("guncfg_user.xml", content);
	}
}
