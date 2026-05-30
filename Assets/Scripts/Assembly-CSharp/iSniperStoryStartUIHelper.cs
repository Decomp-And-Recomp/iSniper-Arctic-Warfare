using System.Collections;
using UnityEngine;

public class iSniperStoryStartUIHelper : UIHelper
{
	private enum NextScene
	{
		kMenu = 0,
		kWeapon = 1,
		kScene = 2
	}

	private NextScene m_next_scene;

	private iSniperGameState m_GameState;

	private AudioSource m_SoundClick;

	private AudioSource m_SoundStart;

	private AudioSource m_SoundBack;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/StoryStartUI";
		base.Start();
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
		{
			SetMissionIndex(m_GameState.m_iArcDaysNum);
		}
		else
		{
			SetMissionIndex(m_GameState.m_iPlayerCurrentScene);
		}
		StartAnimation("flashimage");
		StartAnimation("flashimage3");
		FadeIn();
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundStart = GetAudioSource("Main Camera/SoundStart");
		m_SoundBack = GetAudioSource("Main Camera/SoundBack");
		ShowInfo();
		FlurryPlugin.logEvent("StoryStartUI", m_GameState.m_bStoryMode.ToString(), "bIsStoryMode");
	}

	public void ShowInfo()
	{
		((UIText)m_control_table["leveltext"]).SetText(m_GameState.m_iPlayerLevel.ToString());
		((UIText)m_control_table["healthtext"]).SetText(m_GameState.GetPlayerHealth().ToString());
		((UIText)m_control_table["experiencetext"]).SetText(m_GameState.GetPlayerExperience().ToString());
		((UIText)m_control_table["nextleveltext"]).SetText(m_GameState.GetNeedExperience2NextLevel().ToString());
		((UIText)m_control_table["weapontext"]).SetText(m_GameState.m_strPlayerUserGunName);
		string text = null;
		if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
		{
			int iArcLastScene = m_GameState.m_iArcLastScene;
			text = "SceneDes_Prefab/Scene" + m_GameState.m_iArcCurScene;
			Debug.Log("Arcade Mode! CurrentScene:" + text);
		}
		else
		{
			text = "SceneDes_Prefab/Scene" + m_GameState.m_iPlayerCurrentScene;
		}
		iSniperSceneCfg component = GameObject.Find(text).GetComponent<iSniperSceneCfg>();
		((UIText)m_control_table["timetext"]).SetText(component.m_strTime);
		((UIText)m_control_table["addresstext"]).SetText(component.m_strAddress);
		((UIText)m_control_table["descripttext"]).SetText(component.m_strDescribe);
		string text2 = "iSniper3D/UI/StoryStart/Materials/" + component.m_strScene + "_M";
		((UIImage)m_control_table["cjImage"]).SetAlpha(1f);
		((UIImage)m_control_table["cjImage"]).SetTexture(LoadMaterial(text2));
		string text3 = "TX_1";
		if (Utils.IsRetina())
		{
			text3 += "_HD";
		}
		string text4 = "iSniper3D/UI/Head/Materials/" + text3 + "_M";
		((UIImage)m_control_table["headimage"]).SetTexture(LoadMaterial(text4));
		((UIText)m_control_table["headtext"]).SetText(GetNpcHeadName(text3));
	}

	public IEnumerator TypeText(string str)
	{
		string text = string.Empty;
		char[] array = str.ToCharArray();
		foreach (char tmp in array)
		{
			text += tmp;
			((UIText)m_control_table["descripttext"]).SetText(string.Empty);
			((UIText)m_control_table["descripttext"]).SetText(text);
			yield return new WaitForSeconds(0.03f);
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("startbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundStart);
			m_next_scene = NextScene.kScene;
			FadeOut();
		}
		if (GetControlId("weaponshopbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			m_next_scene = NextScene.kWeapon;
			FadeOut();
		}
		if (GetControlId("backbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			m_next_scene = NextScene.kMenu;
			FadeOut();
		}
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
			switch (m_next_scene)
			{
			case NextScene.kScene:
			{
				iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
				string text = null;
				text = ((m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode) ? ("iSniper.Scene" + m_GameState.m_iPlayerCurrentScene) : ("iSniper.Scene" + m_GameState.m_iArcCurScene));
				m_GameState.m_strSceneName = text;
				Application.LoadLevel("iSniper.Loading");
				break;
			}
			case NextScene.kWeapon:
				iSniperGameApp.GetInstance().m_GameState.m_LastScene = iSniperGameState.LastScene.kStoryStart;
				Application.LoadLevel("iSniper.WeaponShop");
				break;
			case NextScene.kMenu:
				Application.LoadLevel("iSniper.Menu");
				break;
			}
		}
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

	public void SetMissionIndex(int index)
	{
		((UIText)m_control_table["missiontext"]).SetText(index + " ");
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
