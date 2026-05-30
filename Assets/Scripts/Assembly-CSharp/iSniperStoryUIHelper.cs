using System.Collections;
using UnityEngine;

public class iSniperStoryUIHelper : UIHelper
{
	private ArrayList m_story_list;

	private int m_story_index;

	private bool m_is_story_image1;

	private iSniperStoryCfg.Item m_Item;

	private AudioSource m_SoundClick;

	private AudioSource m_SoundMission;

	private iSniperGameState m_GameState;

	private bool m_bStoryMusic;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/StoryUI";
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		base.Start();
		iSniperStoryCfg component = GameObject.Find("StoryConfig_Prefab").GetComponent<iSniperStoryCfg>();
		if (iSniperGameApp.GetInstance().m_GameState.m_bStoryStart)
		{
			m_story_list = component.GetStory(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCurrentScene, true);
		}
		else if (m_GameState.m_bIsEndedScene)
		{
			m_story_list = component.GetStory(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCurrentScene, false);
		}
		else
		{
			m_story_list = component.GetStory(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCurrentScene - 1, false);
		}
		for (int i = 0; i < m_story_list.Count; i++)
		{
			iSniperStoryCfg.Item item = (iSniperStoryCfg.Item)m_story_list[i];
			if (item.m_strImage.Length > 0)
			{
				LoadMaterial(item.m_strImage);
			}
			if (item.m_strHead.Length > 0)
			{
				LoadMaterial(item.m_strHead);
			}
		}
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundMission = GetAudioSource("Main Camera/SoundMission");
		m_story_index = 0;
		m_Item = (iSniperStoryCfg.Item)m_story_list[m_story_index];
		((UIImage)m_control_table["storyimage1"]).SetTexture(LoadMaterial(m_Item.m_strImage));
		((UIImage)m_control_table["headimage"]).SetTexture(LoadMaterial(m_Item.m_strHead));
		((UIText)m_control_table["headtext"]).SetText(GetNpcHeadName(m_Item.m_strHead));
		((UIText)m_control_table["talktext"]).SetText(m_Item.m_strText);
		if (m_Item.m_strImage.IndexOf("GS00") != -1)
		{
			iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.Story);
		}
		if (m_Item.m_strImage.IndexOf("WZ") != -1 && m_Item.m_strImage.IndexOf("00") == -1)
		{
			iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
			PlaySound(m_SoundMission);
			m_bStoryMusic = true;
		}
		m_is_story_image1 = true;
		StartAnimation("flashimage");
		FadeIn();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("nextbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			if (m_bStoryMusic)
			{
				iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.Mission);
			}
			if (m_story_index == m_story_list.Count - 1)
			{
				FadeOut();
			}
			else if (m_story_index < m_story_list.Count - 1)
			{
				m_story_index++;
				m_Item = (iSniperStoryCfg.Item)m_story_list[m_story_index];
				if (m_Item.m_strImage.Length > 0)
				{
					((UIClickButton)m_control_table["nextbtn"]).Enable = false;
					SwitchStoryImage(m_Item.m_strImage);
				}
				else
				{
					StartAnimation("nextbtndelay");
					((UIImage)m_control_table["headimage"]).SetTexture(LoadMaterial(m_Item.m_strHead));
					((UIText)m_control_table["headtext"]).SetText(GetNpcHeadName(m_Item.m_strHead));
					((UIText)m_control_table["talktext"]).SetText(m_Item.m_strText);
				}
			}
		}
		if (GetControlId("skipbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			FadeOut();
		}
	}

	public IEnumerator TypeText(string str)
	{
		string text = string.Empty;
		((UIClickButton)m_control_table["nextbtn"]).Enable = false;
		char[] array = str.ToCharArray();
		foreach (char tmp in array)
		{
			text += tmp;
			((UIText)m_control_table["talktext"]).SetText(string.Empty);
			((UIText)m_control_table["talktext"]).SetText(text);
			yield return new WaitForSeconds(0.03f);
		}
		((UIClickButton)m_control_table["nextbtn"]).Enable = true;
	}

	public override void OnAnimationFinished(string name)
	{
		if ("fadein" == name)
		{
			UIImage uIImage = (UIImage)m_control_table["fadeimage"];
			uIImage.Visible = false;
			uIImage.Enable = false;
		}
		else if ("fadeout" == name)
		{
			if (iSniperGameApp.GetInstance().m_GameState.m_bStoryStart)
			{
				Application.LoadLevel("iSniper.StoryStart");
			}
			else if (m_GameState.m_bIsEndedScene)
			{
				m_GameState.m_bIsEndedScene = false;
				Application.LoadLevel("iSniper.Menu");
			}
			else
			{
				iSniperGameApp.GetInstance().m_GameState.m_bStoryStart = true;
				Application.LoadLevel("iSniper.Story");
			}
		}
		else if ("nextbtndelay" == name)
		{
			((UIClickButton)m_control_table["nextbtn"]).Enable = true;
		}
		else if ("storyimage2in" == name)
		{
			((UIClickButton)m_control_table["nextbtn"]).Enable = true;
			((UIImage)m_control_table["headimage"]).SetTexture(LoadMaterial(m_Item.m_strHead));
			((UIText)m_control_table["headtext"]).SetText(GetNpcHeadName(m_Item.m_strHead));
			((UIText)m_control_table["talktext"]).SetText(m_Item.m_strText);
		}
		else if ("storyimage2out" == name)
		{
			((UIImage)m_control_table["storyimage2"]).Visible = false;
		}
		else if ("storyimage1in" == name)
		{
			((UIClickButton)m_control_table["nextbtn"]).Enable = true;
			((UIImage)m_control_table["headimage"]).SetTexture(LoadMaterial(m_Item.m_strHead));
			((UIText)m_control_table["headtext"]).SetText(GetNpcHeadName(m_Item.m_strHead));
			((UIText)m_control_table["talktext"]).SetText(m_Item.m_strText);
		}
		else if ("storyimage1out" == name)
		{
			((UIImage)m_control_table["storyimage1"]).Visible = false;
		}
	}

	public void SwitchStoryImage(string name)
	{
		if (name.IndexOf("WZ") != -1 && name.IndexOf("00") == -1)
		{
			iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
			PlaySound(m_SoundMission);
			m_bStoryMusic = true;
		}
		if (m_is_story_image1)
		{
			((UIImage)m_control_table["storyimage1"]).SetAlpha(1f);
			StartAnimation("storyimage1out");
			((UIImage)m_control_table["storyimage2"]).SetAlpha(0f);
			((UIImage)m_control_table["storyimage2"]).Visible = true;
			((UIImage)m_control_table["storyimage2"]).SetTexture(LoadMaterial(name));
			StartAnimation("storyimage2in");
		}
		else
		{
			((UIImage)m_control_table["storyimage2"]).SetAlpha(1f);
			StartAnimation("storyimage2out");
			((UIImage)m_control_table["storyimage1"]).SetAlpha(0f);
			((UIImage)m_control_table["storyimage1"]).Visible = true;
			((UIImage)m_control_table["storyimage1"]).SetTexture(LoadMaterial(name));
			StartAnimation("storyimage1in");
		}
		m_is_story_image1 = !m_is_story_image1;
		((UIImage)m_control_table["headimage"]).SetTexture(null);
		((UIText)m_control_table["talktext"]).SetText(string.Empty);
		((UIText)m_control_table["headtext"]).SetText(string.Empty);
	}

	public void FadeIn()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIImage)m_control_table["fadeimage"]).SetAlpha(1f);
		StartAnimation("fadein");
	}

	public void FadeOut()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIImage)m_control_table["fadeimage"]).SetAlpha(0f);
		StartAnimation("fadeout");
	}

	public void ShowOkDialog(string content)
	{
		((UIImage)m_control_table["dialogblock"]).Enable = true;
		((UIImage)m_control_table["dialogblock"]).Visible = true;
		((UIImage)m_control_table["dialogokbk"]).Enable = true;
		((UIImage)m_control_table["dialogokbk"]).Visible = true;
		((UIText)m_control_table["dialogtext"]).Visible = true;
		((UIText)m_control_table["dialogtext"]).SetText(content);
		((UIClickButton)m_control_table["okbutton"]).Enable = true;
		((UIClickButton)m_control_table["okbutton"]).Visible = true;
	}

	public void HideOkDialog()
	{
		((UIImage)m_control_table["dialogblock"]).Enable = false;
		((UIImage)m_control_table["dialogblock"]).Visible = false;
		((UIImage)m_control_table["dialogokbk"]).Enable = false;
		((UIImage)m_control_table["dialogokbk"]).Visible = false;
		((UIText)m_control_table["dialogtext"]).Visible = false;
		((UIClickButton)m_control_table["okbutton"]).Enable = false;
		((UIClickButton)m_control_table["okbutton"]).Visible = false;
	}

	public string GetNpcHeadName(string strHead)
	{
		if (strHead.IndexOf("TX_1") != -1)
		{
			return "Glous";
		}
		if (strHead.IndexOf("TX_2") != -1)
		{
			return "Rebecca";
		}
		if (strHead.IndexOf("TX_3") != -1)
		{
			return "Adam";
		}
		return string.Empty;
	}

	private void PlaySound(AudioSource audio)
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && null != audio)
		{
			audio.Play();
		}
	}
}
