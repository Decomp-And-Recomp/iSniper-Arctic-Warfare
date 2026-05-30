using System.Collections;
using UnityEngine;

public class iSniperMenuUIHelper : UIHelper
{
	private enum NextScene
	{
		kWeapon = 0,
		kOption = 1,
		kStory = 2,
		kText = 3,
		kStoryStart = 4,
		kBootCamps = 5
	}

	private enum DLGSTATE
	{
		kReview = 0,
		kNewGame = 1
	}

	private DLGSTATE m_DlgState;

	private NextScene m_next_scene;

	private AudioSource m_SoundClick;

	private AudioSource m_SoundBack;

	private iSniperGameCenter m_GameCenter;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/MenuUI";
		base.Start();
		StartAnimation("flashimage");
		FadeIn();
		iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
		m_GameCenter = GameObject.Find("Main Camera").GetComponent<iSniperGameCenter>();
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundBack = GetAudioSource("Main Camera/SoundBack");
		if (gameState.m_iLoginTimes == 3)
		{
			ShowYesNoDialog("Having fun? Leave a review in the app store!");
			m_DlgState = DLGSTATE.kReview;
			gameState.m_iLoginTimes++;
			gameState.SaveData();
		}
		FlurryPlugin.logEvent("EnterMenuUI");
	}

	public void Update()
	{
		if (m_next_scene != NextScene.kText || Input.GetMouseButtonDown(0))
		{
		}
		m_GameCenter.SentInfo2GameCenter();
		m_GameCenter.CheckIsSuccess();
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
		if (GetControlId("storymodebtn") == control.Id && command == 0)
		{
			PlayClickSound();
			((UIClickButton)m_control_table["storymodebtn"]).Enable = false;
			((UIClickButton)m_control_table["storymodebtn"]).Visible = false;
			((UIImage)m_control_table["storymodeimage"]).Visible = false;
			((UIImage)m_control_table["newgameimage"]).Visible = true;
			((UIClickButton)m_control_table["newgamebtn"]).Enable = true;
			((UIClickButton)m_control_table["newgamebtn"]).Visible = true;
			((UIImage)m_control_table["continueimage"]).Visible = true;
			if (!gameState.IsHaveSaveData())
			{
				((UIImage)m_control_table["continueimage"]).SetColor(Color.gray);
			}
			((UIClickButton)m_control_table["continuebtn"]).Enable = gameState.IsHaveSaveData();
			((UIClickButton)m_control_table["continuebtn"]).Visible = gameState.IsHaveSaveData();
		}
		if (GetControlId("newgamebtn") == control.Id && command == 0)
		{
			PlayClickSound();
			if (gameState.IsHaveSaveData())
			{
				ShowYesNoDialog("Would you like to restart the game?");
				m_DlgState = DLGSTATE.kNewGame;
			}
			else
			{
				gameState.m_iPlayerCurrentScene = 1;
				gameState.m_bStoryMode = true;
				if (gameState.m_bBootCampsOver)
				{
					m_next_scene = NextScene.kText;
				}
				else
				{
					m_next_scene = NextScene.kBootCamps;
				}
				FadeOut();
			}
		}
		if (GetControlId("continuebtn") == control.Id && command == 0)
		{
			PlayClickSound();
			gameState.m_bStoryMode = true;
			m_next_scene = NextScene.kStory;
			FadeOut();
		}
		if (GetControlId("arcademodebtn") == control.Id && command == 0)
		{
			PlayClickSound();
			if (!gameState.m_bArcadeIsLock)
			{
				if (gameState.m_iArcDaysNum == 1)
				{
					gameState.RecomputeScene(gameState.m_iArcDaysNum, gameState.m_bArcWinState);
					gameState.m_bArcLockScene = true;
					gameState.SaveData();
					gameState.m_bArcBootCamp = true;
					if (gameState.m_bBootCampsOver)
					{
						gameState.m_bStoryMode = false;
						Debug.Log(iSniperGameApp.GetInstance().m_GameState.m_bStoryMode);
						m_next_scene = NextScene.kText;
					}
					else
					{
						m_next_scene = NextScene.kBootCamps;
					}
					FadeOut();
				}
				else
				{
					gameState.m_bStoryMode = false;
					Debug.Log(gameState.m_bArcWinState);
					gameState.RecomputeScene(gameState.m_iArcDaysNum, gameState.m_bArcWinState);
					gameState.m_bArcLockScene = true;
					gameState.SaveData();
					m_next_scene = NextScene.kStoryStart;
					FadeOut();
				}
			}
			else
			{
				ShowOkDialog("Complete Story Mode to unlock Infinite Mode!");
			}
		}
		if (GetControlId("achievementbtn") == control.Id && command == 0)
		{
			PlayClickSound();
			Application.OpenURL("http://www.trinitigame.com/support?game=isaw&version=1.0.8");
		}
		if (GetControlId("leaderboardbtn") == control.Id && command == 0)
		{
			PlayClickSound();
			if (iSniperAndroidIAP.m_platform == iSniperAndroidIAP.Platform.kAmazon)
			{
				Application.OpenURL("http://www.amazon.com/gp/mas/dl/android?p=com.trinitigame.android.isniper3d2");
			}
			else
			{
				Application.OpenURL("market://details?id=com.trinitigame.android.isniper3d2");
			}
		}
		if (GetControlId("optionbtn") == control.Id && command == 0)
		{
			PlayClickSound();
			m_next_scene = NextScene.kOption;
			FadeOut();
		}
		if (GetControlId("weaponbtn") == control.Id && command == 0)
		{
			PlayClickSound();
			m_next_scene = NextScene.kWeapon;
			FadeOut();
		}
		if (GetControlId("yesbutton") == control.Id && command == 0)
		{
			PlayClickSound();
			if (m_DlgState == DLGSTATE.kNewGame)
			{
				HideYesNoDialog();
				gameState.m_iPlayerCurrentScene = 1;
				gameState.m_bStoryMode = true;
				if (gameState.m_bBootCampsOver)
				{
					m_next_scene = NextScene.kText;
				}
				else
				{
					m_next_scene = NextScene.kBootCamps;
				}
				FlurryPlugin.logEvent("RestartNewGame");
				gameState.SaveData();
				FadeOut();
			}
			if (m_DlgState == DLGSTATE.kReview)
			{
				if (iSniperAndroidIAP.m_platform == iSniperAndroidIAP.Platform.kAmazon)
				{
					Application.OpenURL("http://www.amazon.com/gp/mas/dl/android?p=com.trinitigame.android.isniper3d2");
				}
				else
				{
					Application.OpenURL("market://details?id=com.trinitigame.android.isniper3d2");
				}
				HideYesNoDialog();
			}
		}
		if (GetControlId("nobutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			HideYesNoDialog();
		}
		if (GetControlId("okbutton") == control.Id && command == 0)
		{
			PlayClickSound();
			HideOkDialog();
		}
	}

	public override void OnAnimationFinished(string name)
	{
		if ("fadein" == name)
		{
			UIImage uIImage = (UIImage)m_control_table["fadeimage"];
			uIImage.Visible = false;
			uIImage.Enable = false;
			iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
			if (gameState.m_bLastArcLock && !gameState.m_bArcadeIsLock)
			{
				ShowOkDialog("Story mode complete, Infinite Mode unlocked!");
				gameState.m_bLastArcLock = gameState.m_bArcadeIsLock;
			}
		}
		else if ("fadeout" == name)
		{
			OpenClikPlugin.Hide();
			switch (m_next_scene)
			{
			case NextScene.kWeapon:
				iSniperGameApp.GetInstance().m_GameState.m_LastScene = iSniperGameState.LastScene.kMenu;
				Application.LoadLevel("iSniper.WeaponShop");
				break;
			case NextScene.kOption:
				Application.LoadLevel("iSniper.Option");
				break;
			case NextScene.kStory:
				iSniperGameApp.GetInstance().m_GameState.m_bStoryStart = true;
				Application.LoadLevel("iSniper.Story");
				break;
			case NextScene.kText:
				Application.LoadLevel("iSniper.ShowText");
				break;
			case NextScene.kStoryStart:
				iSniperGameApp.GetInstance().m_GameState.m_bStoryStart = true;
				Application.LoadLevel("iSniper.StoryStart");
				break;
			case NextScene.kBootCamps:
				iSniperGameApp.GetInstance().m_GameState.m_bBootCampsMode = true;
				iSniperGameApp.GetInstance().m_GameState.m_iKillEnemyNum = 0;
				Application.LoadLevel("iSniper.Loading");
				iSniperGameApp.GetInstance().m_GameState.m_strSceneName = "iSniper.BootCamps";
				break;
			}
		}
	}

	public IEnumerator TypeText(string str)
	{
		iSniperMusicMgr.Instance().PlayMusic(iSniperMusicMgr.MusicType.No);
		string text = string.Empty;
		char[] array = str.ToCharArray();
		foreach (char tmp in array)
		{
			text += tmp;
			((UIText)m_control_table["newgametext"]).SetText(string.Empty);
			((UIText)m_control_table["newgametext1"]).SetText(string.Empty);
			((UIText)m_control_table["newgametext"]).SetText(text);
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds(0.3f);
		StartCoroutine("TypeTextName", "Ethan Embry");
	}

	public IEnumerator TypeTextName(string str)
	{
		string text = string.Empty;
		char[] array = str.ToCharArray();
		foreach (char tmp in array)
		{
			text += tmp;
			((UIText)m_control_table["newgametext1"]).SetText(string.Empty);
			((UIText)m_control_table["newgametext1"]).SetText(text);
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds(1f);
		m_next_scene = NextScene.kStory;
		TypeTextFadeOut();
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

	public void TypeTextFadeOut()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		StartAnimation("fadeout");
	}

	public void ShowYesNoDialog(string content)
	{
		((UIImage)m_control_table["dialogblock"]).Enable = true;
		((UIImage)m_control_table["dialogblock"]).Visible = true;
		((UIImage)m_control_table["dialogyesnobk"]).Enable = true;
		((UIImage)m_control_table["dialogyesnobk"]).Visible = true;
		((UIText)m_control_table["dialogtext"]).Visible = true;
		((UIText)m_control_table["dialogtext"]).SetText(content);
		((UIClickButton)m_control_table["yesbutton"]).Enable = true;
		((UIClickButton)m_control_table["nobutton"]).Enable = true;
		((UIClickButton)m_control_table["yesbutton"]).Visible = true;
		((UIClickButton)m_control_table["nobutton"]).Visible = true;
	}

	public void HideYesNoDialog()
	{
		((UIImage)m_control_table["dialogblock"]).Enable = false;
		((UIImage)m_control_table["dialogblock"]).Visible = false;
		((UIImage)m_control_table["dialogyesnobk"]).Enable = false;
		((UIImage)m_control_table["dialogyesnobk"]).Visible = false;
		((UIText)m_control_table["dialogtext"]).Visible = false;
		((UIClickButton)m_control_table["yesbutton"]).Enable = false;
		((UIClickButton)m_control_table["nobutton"]).Enable = false;
		((UIClickButton)m_control_table["yesbutton"]).Visible = false;
		((UIClickButton)m_control_table["nobutton"]).Visible = false;
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

	private void PlayClickSound()
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && null != m_SoundClick)
		{
			m_SoundClick.Play();
		}
	}

	private void PlaySound(AudioSource audio)
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && null != audio)
		{
			audio.Play();
		}
	}
}
