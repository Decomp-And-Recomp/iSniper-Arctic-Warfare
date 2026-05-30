using System.Collections;
using System.Xml;
using UnityEngine;

public class iSniperStoryCfg : MonoBehaviour
{
	public class Item
	{
		public string m_strImage = string.Empty;

		public string m_strHead = string.Empty;

		public string m_strText = string.Empty;
	}

	public class Story
	{
		public ArrayList m_arrStart;

		public ArrayList m_arrEnd;

		public Story()
		{
			m_arrStart = new ArrayList();
			m_arrEnd = new ArrayList();
		}
	}

	public TextAsset m_XmlStory;

	public ArrayList m_Story;

	private void Awake()
	{
		m_Story = new ArrayList();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(m_XmlStory.text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (childNode.Name.IndexOf("Story") == -1)
			{
				continue;
			}
			Story story = new Story();
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				if ("Start" == childNode2.Name)
				{
					foreach (XmlNode childNode3 in childNode2.ChildNodes)
					{
						if (!("Item" == childNode3.Name))
						{
							continue;
						}
						Item item = new Item();
						XmlElement xmlElement = (XmlElement)childNode3;
						string text = xmlElement.GetAttribute("Image").Trim();
						if (text.Length > 0)
						{
							if (Utils.IsRetina())
							{
								text += "_HD";
							}
							item.m_strImage = "iSniper3D/UI/Story/Materials/" + text + "_M";
						}
						string text2 = xmlElement.GetAttribute("Head").Trim();
						if (text2.Length > 0)
						{
							if (Utils.IsRetina())
							{
								text2 += "_HD";
							}
							item.m_strHead = "iSniper3D/UI/Head/Materials/" + text2 + "_M";
						}
						item.m_strText = xmlElement.GetAttribute("Text").Trim();
						story.m_arrStart.Add(item);
					}
				}
				else
				{
					if (!("End" == childNode2.Name))
					{
						continue;
					}
					foreach (XmlNode childNode4 in childNode2.ChildNodes)
					{
						if (!("Item" == childNode4.Name))
						{
							continue;
						}
						Item item2 = new Item();
						XmlElement xmlElement2 = (XmlElement)childNode4;
						string text3 = xmlElement2.GetAttribute("Image").Trim();
						if (text3.Length > 0)
						{
							if (Utils.IsRetina())
							{
								text3 += "_HD";
							}
							item2.m_strImage = "iSniper3D/UI/Story/Materials/" + text3 + "_M";
						}
						string text4 = xmlElement2.GetAttribute("Head").Trim();
						if (text4.Length > 0)
						{
							if (Utils.IsRetina())
							{
								text4 += "_HD";
							}
							item2.m_strHead = "iSniper3D/UI/Head/Materials/" + text4 + "_M";
						}
						item2.m_strText = xmlElement2.GetAttribute("Text").Trim();
						story.m_arrEnd.Add(item2);
					}
				}
			}
			m_Story.Add(story);
		}
	}

	public ArrayList GetStory(int index, bool bStart)
	{
		if (bStart)
		{
			return ((Story)m_Story[index - 1]).m_arrStart;
		}
		return ((Story)m_Story[index - 1]).m_arrEnd;
	}
}
