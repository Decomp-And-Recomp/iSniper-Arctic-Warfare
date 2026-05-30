using System;
using System.Collections;
using UnityEngine;

public class iSniperGameUIHelper : UIHelper
{
	private enum STATE
	{
		kBegin = 0,
		kTextShow = 1,
		kTalk = 2,
		kTextHide = 3,
		kEnd = 4,
		kIdle = 5
	}

	public enum BootCampsSTATE
	{
		kno = 0,
		kwelcome = 1,
		kShowAim = 2,
		kAim = 3,
		kZoom = 4,
		kShot = 5,
		kClose = 6,
		kEnded = 7
	}

	private enum NextScene
	{
		kMenu = 0,
		kRestart = 1,
		kResult = 2
	}

	private iSniperGunProperty m_GunProperty;

	private float m_UnloadTime = 10f;

	private bool m_needcompute;

	private bool m_bHolding;

	private float m_fStartAngle;

	private float m_fMaxAngle;

	private float m_fCurAngle;

	private float m_fMaxTime;

	private float m_fDuringTime;

	private float m_fMaxAlphaTime;

	private bool m_bIsShowHelpTip;

	private float m_fHelpTipTime = 1.5f;

	private float m_fHelpAimSwitch = 0.5f;

	private float m_fAlphaTime;

	public BootCampsSTATE m_enCampsState;

	public bool m_bIsAim;

	public bool m_bEnemyIsDie;

	private bool m_bStartDown;

	private float m_fBulletDownTime;

	private float m_fBulletSpeedH;

	private float m_fBulletSpeedV;

	private int m_iCurBulletIndex;

	public bool m_bIsSkip;

	public bool m_bPauseResume;

	private STATE m_enTalkState;

	private float m_fTalkStateTime;

	private int m_iTalkTextIndex;

	private float m_shottip_duringtime;

	private string m_title_shottip_animation;

	private bool m_bNeedReloadTime;

	private float m_fReloadTime;

	private int m_fMovAimImageIndex;

	private int m_fMoveAimSmallImageIndex;

	private int m_movetimes;

	private NextScene m_next_scene;

	public AudioSource m_MusicBk;

	private AudioSource m_SoundGun1;

	private AudioSource m_SoundGun2;

	private AudioSource m_SoundReload1;

	private AudioSource m_SoundReload2;

	private AudioSource m_SoundBreathOn;

	private iSniperGameScene m_GameScene;

	private AudioSource m_SoundBreathOff;

	private AudioSource m_SoundZoomIn;

	private AudioSource m_SoundZoomOut;

	private AudioSource m_SoundBulletoutH;

	private AudioSource m_SoundBulletoutL;

	private AudioSource m_SoundBulletUp;

	public AudioSource m_SoundHurt1;

	public AudioSource m_SoundHurt2;

	public AudioSource m_SoundScene;

	public AudioSource m_SoundHeadTitleShot;

	public AudioSource m_SoundFlex1;

	public AudioSource m_SoundFlex2;

	private AudioSource m_SoundShotReveb1;

	private AudioSource m_SoundShotReveb2;

	private AudioSource m_SoundBack;

	private AudioSource m_SoundClick;

	public new void Start()
	{
		Resources.UnloadUnusedAssets();
		m_GameScene = GameObject.Find("Main Camera").GetComponent<iSniperGame>().m_GameScene;
		m_GunProperty = iSniperGameApp.GetInstance().m_GameState.GetUseGunProperty();
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/GameUI";
		base.Start();
		m_needcompute = false;
		m_bHolding = false;
		m_fStartAngle = 0.48332196f;
		m_fMaxAngle = -0.48332196f;
		m_fCurAngle = 0f;
		m_fMaxTime = 5f;
		m_fDuringTime = 0f;
		m_fMaxAlphaTime = 1f;
		m_bStartDown = false;
		m_fBulletDownTime = 0f;
		m_fBulletSpeedH = 0f;
		m_fBulletSpeedV = 0f;
		m_iCurBulletIndex = m_GunProperty.GetCurrentClip();
		FadeIn();
		m_fMovAimImageIndex = 0;
		m_fMoveAimSmallImageIndex = 0;
		m_movetimes = 1;
		CheckAudioObjs();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			((UIImage)m_control_table["aimcdmask"]).Visible = true;
			m_needcompute = true;
			m_bHolding = true;
			((UIImage)m_control_table["holdbtnmask"]).Visible = true;
			ShowHoldCycle();
			iSniperGameApp.GetInstance().m_GameState.m_bTiltHolding = true;
		}
		if (Input.GetKeyUp(KeyCode.C))
		{
			((UIImage)m_control_table["holdbtnmask"]).Enable = true;
			m_bHolding = false;
			HideHoldCycle();
			iSniperGameApp.GetInstance().m_GameState.m_bTiltHolding = false;
		}
		iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
		iSniperGameScene.State state = m_GameScene.m_State;
		if (Input.GetMouseButtonDown(0) && m_enTalkState != STATE.kIdle)
		{
			m_fTalkStateTime = 1f;
			m_enTalkState = STATE.kEnd;
		}
		switch (state)
		{
		case iSniperGameScene.State.kGameReady:
			break;
		case iSniperGameScene.State.kGameStart:
			if (m_enTalkState == STATE.kBegin)
			{
				m_fTalkStateTime += Time.deltaTime;
				if (m_fTalkStateTime >= 1f)
				{
					m_enTalkState = STATE.kTextShow;
					m_fTalkStateTime = 0f;
				}
				int iStageIndex3 = gameState.m_iStageIndex;
				iSniperStageCfg stageCfg3 = m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex3);
				if (stageCfg3.m_bMusicOn)
				{
					if ((bool)m_SoundScene && m_SoundScene.GetComponent<AudioSource>().isPlaying)
					{
						m_SoundScene.Stop();
					}
					if ((bool)m_MusicBk && !m_MusicBk.isPlaying)
					{
						PlayBkMusic();
					}
					break;
				}
				if ((bool)m_MusicBk && m_MusicBk.isPlaying)
				{
					m_MusicBk.Stop();
				}
				if ((bool)m_SoundScene && !m_SoundScene.isPlaying)
				{
					m_SoundScene.loop = true;
					PlayBkSound(m_SoundScene);
				}
			}
			else if (m_enTalkState == STATE.kTextShow)
			{
				int iStageIndex4 = gameState.m_iStageIndex;
				iSniperStageCfg stageCfg4 = m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex4);
				string text2 = (string)stageCfg4.m_MovieStartTexts[m_iTalkTextIndex];
				((UIText)m_control_table["startTalkText"]).SetText(text2);
				((UIText)m_control_table["startTalkText"]).SetAlpha(0f);
				((UIText)m_control_table["startTalkText"]).Visible = true;
				StartAnimation("starttalktextshow");
				m_enTalkState = STATE.kIdle;
			}
			else if (m_enTalkState == STATE.kTalk)
			{
				int iStageIndex5 = gameState.m_iStageIndex;
				iSniperStageCfg stageCfg5 = m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex5);
				m_fTalkStateTime += Time.deltaTime;
				float num2 = (float)((string)stageCfg5.m_MovieStartTexts[m_iTalkTextIndex]).Length * 0.05f;
				if (m_fTalkStateTime >= num2)
				{
					m_enTalkState = STATE.kTextHide;
					m_fTalkStateTime = 0f;
				}
			}
			else if (m_enTalkState == STATE.kTextHide)
			{
				StartAnimation("starttalktexthide");
				m_enTalkState = STATE.kIdle;
			}
			else if (m_enTalkState == STATE.kEnd)
			{
				m_fTalkStateTime += Time.deltaTime;
				if (m_fTalkStateTime >= 1f)
				{
					((UIText)m_control_table["startTalkText"]).Visible = false;
					EndMovieEffect();
					m_enTalkState = STATE.kIdle;
				}
			}
			break;
		case iSniperGameScene.State.kGaming:
			if (m_needcompute)
			{
				ComputeHoldCDAnimation();
			}
			if (m_bStartDown)
			{
				ComputeBulletDownAnimation();
			}
			if (m_bNeedReloadTime)
			{
				m_fReloadTime -= Time.deltaTime;
				if (m_fReloadTime <= 0f)
				{
					m_fReloadTime = 0f;
					m_bNeedReloadTime = false;
					ReloadBullets();
				}
			}
			if (gameState.m_bBootCampsMode)
			{
				UIText uIText = (UIText)m_control_table["tipText"];
				uIText.Enable = true;
				uIText.Visible = true;
				switch (m_enCampsState)
				{
				case BootCampsSTATE.kno:
					((UIImage)m_control_table["hideimage"]).Enable = true;
					((UIImage)m_control_table["hideimage"]).Visible = true;
					((UIImage)m_control_table["hideimage"]).SetAlpha(0f);
					m_fHelpTipTime -= Time.deltaTime;
					if (m_fHelpTipTime <= 0f)
					{
						m_enCampsState = BootCampsSTATE.kwelcome;
						m_fHelpTipTime = 0.5f;
					}
					break;
				case BootCampsSTATE.kwelcome:
					uIText.SetText(string.Empty);
					((UIImage)m_control_table["textimage"]).Visible = true;
					m_fHelpTipTime -= Time.deltaTime;
					if (m_fHelpTipTime <= 0f)
					{
						m_enCampsState = BootCampsSTATE.kShowAim;
						((UIImage)m_control_table["touchimage"]).Visible = false;
						((UIImage)m_control_table["touchimage1"]).Visible = true;
						StartAnimation("helpflashimage");
						((UIClickButton)m_control_table["tipbtn"]).Enable = true;
						((UIClickButton)m_control_table["tipbtn"]).Visible = true;
						m_fHelpTipTime = 1.5f;
					}
					break;
				case BootCampsSTATE.kShowAim:
					StartAnimation("starttiptextshow");
					uIText.SetText(Application.isMobilePlatform ? "Tap to enter Sniper View." : "Right click to enter Sniper View.");
					SwitchAimLockImg();
					break;
				case BootCampsSTATE.kAim:
					StartAnimation("starttiptextshow");
					if (Application.isMobilePlatform)
					{
						if (m_GameScene.m_GameState.m_bIsTiltControl)
						{
							uIText.SetText("Tilt to aim. \n(You can also change the Control Mode \n to Drag in Options).");
						}
						else
						{
							uIText.SetText("Drag to aim. \n(You can also switch the Control Mode \n to Tilt in Options).");
						}		
					}
					else
					{
						uIText.SetText("Move mouse to aim.");
					}
					((UIImage)m_control_table["zoomimage"]).Visible = false;
					if (m_bIsAim)
					{
						((UIImage)m_control_table["fireimage"]).Visible = true;
						uIText.SetText(Application.isMobilePlatform ? "Tap to fire." : "Left click to fire.");
						m_enCampsState = BootCampsSTATE.kShot;
					}
					break;
				case BootCampsSTATE.kZoom:
					StartAnimation("starttiptextshow");
					uIText.SetText(Application.isMobilePlatform ? "Swipe up and down to zoom in and out." : "Scroll up and down to zoom in and out.");
					if (IsAim())
					{
						((UIImage)m_control_table["zoomimage"]).Visible = true;
					}
					else
					{
						((UIImage)m_control_table["zoomimage"]).Visible = false;
					}
					break;
				case BootCampsSTATE.kShot:
					if (IsAim())
					{
						((UIImage)m_control_table["fireimage"]).Visible = true;
					}
					else
					{
						((UIImage)m_control_table["fireimage"]).Visible = false;
					}
					if (m_bEnemyIsDie)
					{
						((UIImage)m_control_table["fireimage"]).Visible = false;
						m_enCampsState = BootCampsSTATE.kClose;
					}
					break;
				case BootCampsSTATE.kClose:
					StartAnimation("starttiptextshow");
					uIText.SetText(Application.isMobilePlatform ? "Tap the screen to exit Sniper View." : "Right click to exit Sniper View.");
					if (!IsAim())
					{
						m_enCampsState = BootCampsSTATE.kEnded;
						m_fHelpTipTime = 4f;
					}
					break;
				case BootCampsSTATE.kEnded:
					uIText.SetText("Eliminate the remaining enemies. \n Headshots do more damage.");
					m_fHelpTipTime -= Time.deltaTime;
					if (m_fHelpTipTime <= 0f)
					{
						uIText.Visible = false;
						uIText.SetText(string.Empty);
						((UIImage)m_control_table["textimage"]).Visible = false;
					}
					break;
				}
			}
			if (m_title_shottip_animation != null && m_title_shottip_animation.Length > 0)
			{
				m_shottip_duringtime += Time.deltaTime;
				if (m_shottip_duringtime >= 2f)
				{
					ExitTitleShotTip();
					m_title_shottip_animation = string.Empty;
				}
			}
			break;
		case iSniperGameScene.State.kStageEnd:
			if (m_enTalkState == STATE.kBegin)
			{
				m_fTalkStateTime += Time.deltaTime;
				if (m_fTalkStateTime >= 1f)
				{
					m_enTalkState = STATE.kTextShow;
					m_fTalkStateTime = 0f;
				}
			}
			else if (m_enTalkState == STATE.kTextShow)
			{
				int iStageIndex = gameState.m_iStageIndex;
				iSniperStageCfg stageCfg = m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex);
				string text = (string)stageCfg.m_MovieEndTexts[m_iTalkTextIndex];
				Debug.Log("StageEnd + " + text);
				((UIText)m_control_table["startTalkText"]).SetText(text);
				((UIText)m_control_table["startTalkText"]).SetAlpha(0f);
				((UIText)m_control_table["startTalkText"]).Visible = true;
				StartAnimation("starttalktextshow");
				m_enTalkState = STATE.kIdle;
			}
			else if (m_enTalkState == STATE.kTalk)
			{
				int iStageIndex2 = gameState.m_iStageIndex;
				iSniperStageCfg stageCfg2 = m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex2);
				m_fTalkStateTime += Time.deltaTime;
				float num = (float)((string)stageCfg2.m_MovieEndTexts[m_iTalkTextIndex]).Length * 0.05f;
				if (m_fTalkStateTime >= num)
				{
					m_enTalkState = STATE.kTextHide;
					m_fTalkStateTime = 0f;
				}
			}
			else if (m_enTalkState == STATE.kTextHide)
			{
				StartAnimation("starttalktexthide");
				m_enTalkState = STATE.kIdle;
			}
			else if (m_enTalkState == STATE.kEnd)
			{
				m_fTalkStateTime += Time.deltaTime;
				if (m_fTalkStateTime >= 1f)
				{
					m_enTalkState = STATE.kIdle;
					((UIText)m_control_table["startTalkText"]).Visible = false;
					m_GameScene.CheckStageEndAction();
				}
			}
			break;
		case iSniperGameScene.State.kMovieDown:
			break;
		}
	}

	public void EnterStartState()
	{
		((UIImage)m_control_table["startMovieTop"]).Visible = true;
		((UIImage)m_control_table["startMovieBottom"]).Visible = true;
		m_enTalkState = STATE.kBegin;
		m_fTalkStateTime = 0f;
		m_iTalkTextIndex = 0;
		m_GameScene.EnterGameStartState();
	}

	public void EnterEndState()
	{
		((UIImage)m_control_table["startMovieTop"]).Visible = true;
		((UIImage)m_control_table["startMovieBottom"]).Visible = true;
		m_enTalkState = STATE.kBegin;
		m_fTalkStateTime = 0f;
		m_iTalkTextIndex = 0;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		m_GameScene.EnterStageEndState();
	}

	public void EnterMovieDownState()
	{
		StartCoroutine(StartMovieEffect());
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("holdbtn") == control.Id && command == 1)
		{
			((UIImage)m_control_table["aimcdmask"]).Visible = true;
			m_needcompute = true;
			m_bHolding = true;
			((UIImage)m_control_table["holdbtnmask"]).Visible = true;
			ShowHoldCycle();
			PlaySound(m_SoundBreathOn);
			iSniperGameApp.GetInstance().m_GameState.m_bTiltHolding = true;
		}
		if (GetControlId("holdbtn") == control.Id && command == 0)
		{
			((UIImage)m_control_table["holdbtnmask"]).Enable = true;
			m_bHolding = false;
			HideHoldCycle();
			PlaySound(m_SoundBreathOff);
			iSniperGameApp.GetInstance().m_GameState.m_bTiltHolding = false;
		}
		if (GetControlId("joystickbtn") == control.Id)
		{
			iSniperGameCamera cameraScript = m_GameScene.m_CameraScript;
			switch (command)
			{
			case 0:
				cameraScript.ResetMoveSpeed();
				break;
			case 1:
			{
				if (lparam <= 0f)
				{
					cameraScript.ResetMoveSpeed();
					break;
				}
				float x = lparam * Mathf.Cos(wparam);
				float y = lparam * Mathf.Sin(wparam);
				cameraScript.JoystickMoveSpeed(x, y);
				break;
			}
			case 2:
				cameraScript.ResetMoveSpeed();
				break;
			}
		}
		if (GetControlId("firebtn") == control.Id && command == 1)
		{
			if (m_GameScene.m_GameState.m_bBootCampsMode && m_enCampsState != BootCampsSTATE.kShot && m_enCampsState != BootCampsSTATE.kEnded)
			{
				return;
			}
			m_bStartDown = true;
			int num = ((!Utils.IsRetina()) ? 1 : 2);
			m_fBulletSpeedH = UnityEngine.Random.Range(-20 * num, -5 * num);
			m_fBulletSpeedV = UnityEngine.Random.Range(25 * num, 30 * num);
			((UIImage)m_control_table["bulletshell"]).Visible = true;
			((UIImage)m_control_table["bulletactive"]).Rect = new Rect(390 * num, 45 * num, 58 * num, 13 * num);
			((UIImage)m_control_table["firebtnmask"]).Visible = true;
			((UIImage)m_control_table["firebtnmask"]).Enable = true;
			m_iCurBulletIndex--;
			if (m_iCurBulletIndex <= 1)
			{
				((UIImage)m_control_table["bullet1"]).Visible = false;
			}
			if (m_iCurBulletIndex <= 2)
			{
				((UIImage)m_control_table["bullet2"]).Visible = false;
			}
			if (m_iCurBulletIndex <= 3)
			{
				((UIImage)m_control_table["bullet3"]).Visible = false;
			}
			if (m_iCurBulletIndex <= 4)
			{
				((UIImage)m_control_table["bullet4"]).Visible = false;
			}
			if (m_iCurBulletIndex <= 5)
			{
				((UIImage)m_control_table["bullet5"]).Visible = false;
			}
			if (m_iCurBulletIndex == 0)
			{
				m_fReloadTime = m_GunProperty.GetCurrentReload() - m_GunProperty.GetCurrentFire();
				if (m_fReloadTime <= 0f)
				{
					ReloadBullets();
				}
				else
				{
					m_bNeedReloadTime = true;
				}
			}
			else
			{
				StartAnimation("bulletactiveload" + m_GunProperty.GetCurrentFireStar());
			}
			m_GameScene.Fire();
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				PlaySound(m_SoundGun1);
			}
			else
			{
				PlaySound(m_SoundGun2);
			}
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				PlaySound(m_SoundBulletoutL);
			}
			else
			{
				PlaySound(m_SoundBulletoutH);
			}
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				PlaySoundAtPos(m_SoundShotReveb1, GameObject.Find("Main Camera").transform.position);
			}
			else
			{
				PlaySoundAtPos(m_SoundShotReveb2, GameObject.Find("Main Camera").transform.position);
			}
		}
		if (GetControlId("tipbtn") == control.Id && command == 0)
		{
			((UIClickButton)m_control_table["tipbtn"]).Enable = false;
			((UIClickButton)m_control_table["tipbtn"]).Visible = false;
			((UIImage)m_control_table["hideimage"]).Enable = false;
			((UIImage)m_control_table["hideimage"]).Visible = false;
			((UIImage)m_control_table["hideimage"]).SetAlpha(255f);
			((UIImage)m_control_table["touchimage"]).Visible = false;
			((UIImage)m_control_table["touchimage1"]).Visible = false;
			m_enCampsState = BootCampsSTATE.kZoom;
			if (m_GameScene.m_GameState.m_bIsTiltControl)
			{
				ShowAimUI();
			}
			else
			{
				ShowAim();
			}
			Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3 vector = camera.WorldToScreenPoint(GameObject.Find("Aim").transform.position);
			m_GameScene.m_CameraScript.Aim(vector);
			m_enCampsState = BootCampsSTATE.kZoom;
		}
		if (GetControlId("pausebtn") == control.Id && command == 0)
		{
			if (m_GameScene.m_GameState.m_bBootCampsMode)
			{
				return;
			}
			if ((bool)m_SoundScene && m_GameScene.m_GameState.m_bSoundOn)
			{
				m_SoundScene.volume = 0f;
			}
			if ((bool)m_MusicBk && m_GameScene.m_GameState.m_bMusicOn)
			{
				m_MusicBk.volume = 0f;
			}
			PlaySound(m_SoundClick);
			ShowPauseUI(true);
			Time.timeScale = 0f;
			FlurryPlugin.logEvent("PAUSED");
		}
		if (GetControlId("pauseresumebtn") == control.Id && command == 0)
		{
			ShowPauseUI(false);
			if ((bool)m_SoundScene && m_GameScene.m_GameState.m_bSoundOn)
			{
				m_SoundScene.volume = 1f;
			}
			if ((bool)m_MusicBk && m_GameScene.m_GameState.m_bMusicOn)
			{
				m_MusicBk.volume = 1f;
			}
			m_bPauseResume = true;
			PlaySound(m_SoundClick);
			((UIText)m_control_table["tipText"]).Visible = true;
			if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				ShowAimUI();
				ShowAimUI();
				m_bPauseResume = false;
			}
			else
			{
				if (IsAim())
				{
					ShowAim();
				}
				m_bPauseResume = false;
			}
			TurnMusic();
			PlayBkSound(m_SoundScene);
			Time.timeScale = 1f;
		}
		if (GetControlId("pauserestartbtn") == control.Id && command == 0)
		{
			m_next_scene = NextScene.kRestart;
			PlaySound(m_SoundClick);
			Time.timeScale = 1f;
			FadeOut();
		}
		if (GetControlId("pauseoptionbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			ShowOptionUI(true);
			FlurryPlugin.logEvent("PAUSED_OPTIONS");
		}
		if (GetControlId("pausemenubtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			m_next_scene = NextScene.kMenu;
			Time.timeScale = 1f;
			FadeOut();
		}
		if (GetControlId("pausebackbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			ShowOptionUI(false);
		}
		if (GetControlId("pausecontrolbtn") == control.Id)
		{
			PlaySound(m_SoundClick);
			switch (command)
			{
			case 0:
				iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl = false;
				break;
			case 1:
				iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl = true;
				break;
			}
			if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				((UIImage)m_control_table["pauseinvert1"]).Visible = false;
				((UIImage)m_control_table["pauseinvert2"]).Visible = true;
				((UIPushButton)m_control_table["pauseinvertbtn"]).Enable = true;
			}
			else
			{
				((UIImage)m_control_table["pauseinvert1"]).Visible = true;
				((UIImage)m_control_table["pauseinvert2"]).Visible = false;
				((UIPushButton)m_control_table["pauseinvertbtn"]).Enable = false;
			}
			iSniperGameApp.GetInstance().m_GameState.SaveData();
			FlurryPlugin.logEvent("Operation", iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl.ToString(), "bIsTiltControl");
		}
		if (GetControlId("pauseinvertbtn") == control.Id)
		{
			PlaySound(m_SoundClick);
			switch (command)
			{
			case 0:
				iSniperGameApp.GetInstance().m_GameState.m_bIsInvertYAixs = false;
				break;
			case 1:
				iSniperGameApp.GetInstance().m_GameState.m_bIsInvertYAixs = true;
				break;
			}
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("pausesenstivitymove") == control.Id && command == 1)
		{
			UIImage uIImage = (UIImage)m_control_table["pausesenstivitymark"];
			Vector2 position = uIImage.GetPosition();
			position.x += wparam;
			int num2 = ((!Utils.IsRetina()) ? 1 : 2);
			if (position.x < (float)(138 * num2))
			{
				position.x = 138 * num2;
			}
			else if (position.x > (float)(216 * num2))
			{
				position.x = 216 * num2;
			}
			uIImage.SetPosition(position);
			float num3 = (position.x - (float)(138 * num2)) / (float)(216 * num2 - 138 * num2);
			float num4 = num3 * 45f;
			iSniperGameApp.GetInstance().m_GameState.m_fCurrentSensitivty = 5f + num4;
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("pauseresetbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			int num5 = ((!Utils.IsRetina()) ? 1 : 2);
			UIImage uIImage2 = (UIImage)m_control_table["pausesenstivitymark"];
			Vector2 position2 = uIImage2.GetPosition();
			position2.x = (float)num5 * 354f / 2f;
			uIImage2.SetPosition(position2);
			iSniperGameApp.GetInstance().m_GameState.m_fCurrentSensitivty = 27.5f;
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("pausesoundbtn") == control.Id)
		{
			PlaySound(m_SoundClick);
			switch (command)
			{
			case 0:
				iSniperGameApp.GetInstance().m_GameState.m_bSoundOn = false;
				break;
			case 1:
				iSniperGameApp.GetInstance().m_GameState.m_bSoundOn = true;
				break;
			}
			iSniperGameApp.GetInstance().m_GameState.SaveData();
			FlurryPlugin.logEvent("SoundOn", iSniperGameApp.GetInstance().m_GameState.m_bSoundOn.ToString(), "bSoundOn");
		}
		if (GetControlId("pausemusicbtn") == control.Id)
		{
			PlaySound(m_SoundClick);
			switch (command)
			{
			case 0:
				iSniperGameApp.GetInstance().m_GameState.m_bMusicOn = false;
				break;
			case 1:
				iSniperGameApp.GetInstance().m_GameState.m_bMusicOn = true;
				break;
			}
			iSniperGameApp.GetInstance().m_GameState.SaveData();
			FlurryPlugin.logEvent("MusicOn", iSniperGameApp.GetInstance().m_GameState.m_bMusicOn.ToString(), "bMusicOn");
		}
		if (GetControlId("aimanglemove") != control.Id || (m_GameScene.m_GameState.m_bBootCampsMode && m_enCampsState != BootCampsSTATE.kZoom && m_enCampsState != BootCampsSTATE.kEnded))
		{
			return;
		}
		switch (command)
		{
		case 0:
			((UIImage)m_control_table["aimmoveimagebk1"]).Visible = true;
			break;
		case 1:
		{
			Vector2 position3 = ((UIImage)m_control_table["aimangleimage"]).GetPosition();
			position3.y += lparam;
			bool flag = true;
			int num6 = ((!Utils.IsRetina()) ? 1 : 2);
			if (position3.y > (float)(197 * num6))
			{
				position3.y = 197 * num6;
				flag = false;
			}
			else if (position3.y < (float)(121 * num6))
			{
				position3.y = 121 * num6;
				flag = false;
			}
			((UIImage)m_control_table["aimmoveimage0"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage1"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage2"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage0"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage1"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage2"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage3"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage4"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage5"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage6"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage7"]).Visible = false;
			string key = "aimmoveimage" + m_fMovAimImageIndex;
			((UIImage)m_control_table[key]).Visible = true;
			key = "aimmovesmallimage" + m_fMoveAimSmallImageIndex;
			((UIImage)m_control_table[key]).Visible = true;
			if (flag)
			{
				if (m_movetimes % 5 == 0)
				{
					if (lparam > 0f)
					{
						m_fMovAimImageIndex--;
						m_fMoveAimSmallImageIndex--;
						if (!m_SoundFlex1.GetComponent<AudioSource>().isPlaying && !m_SoundFlex2.GetComponent<AudioSource>().isPlaying)
						{
							PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundFlex2 : m_SoundFlex1);
						}
						if (m_fMovAimImageIndex < 0)
						{
							m_fMovAimImageIndex = 2;
						}
						if (m_fMoveAimSmallImageIndex < 0)
						{
							m_fMoveAimSmallImageIndex = 7;
						}
					}
					else
					{
						m_fMovAimImageIndex++;
						m_fMoveAimSmallImageIndex++;
						if (!m_SoundFlex1.GetComponent<AudioSource>().isPlaying && !m_SoundFlex2.GetComponent<AudioSource>().isPlaying)
						{
							PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundFlex2 : m_SoundFlex1);
						}
						if (m_fMovAimImageIndex > 2)
						{
							m_fMovAimImageIndex = 0;
						}
						if (m_fMoveAimSmallImageIndex > 7)
						{
							m_fMoveAimSmallImageIndex = 0;
						}
					}
				}
				m_movetimes++;
			}
			((UIImage)m_control_table["aimangleimage"]).SetPosition(position3);
			iSniperGameApp.GetInstance().m_GameState.m_fCurrentGunFovDeltaPercent = ((float)(158 * num6) - position3.y) / (39f * (float)num6);
			iSniperGameCamera cameraScript2 = m_GameScene.m_CameraScript;
			cameraScript2.AdjustFov();
			break;
		}
		case 2:
			((UIImage)m_control_table["aimmoveimagebk1"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage0"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage1"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage2"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage0"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage1"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage2"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage3"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage4"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage5"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage6"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage7"]).Visible = false;
			if (m_GameScene.m_GameState.m_bBootCampsMode && m_enCampsState != BootCampsSTATE.kEnded)
			{
				m_enCampsState = BootCampsSTATE.kAim;
			}
			break;
		}
	}

	public override void OnAnimationFinished(string name)
	{
		if (name.IndexOf("bulletactiveload") != -1)
		{
			((UIImage)m_control_table["firebtnmask"]).Visible = false;
			((UIImage)m_control_table["firebtnmask"]).Enable = false;
		}
		else
		{
			if ("startmovieup" == name)
			{
				return;
			}
			if ("startmoviedown" == name)
			{
				EnterEndState();
			}
			else if ("endmovieup" == name)
			{
				((UIImage)m_control_table["startMovieTop"]).Visible = false;
			}
			else if ("endmoviedown" == name)
			{
				((UIImage)m_control_table["startMovieBottom"]).Visible = false;
				m_GameScene.EnterGamingState();
			}
			else if ("starttalktextshow" == name)
			{
				m_enTalkState = STATE.kTalk;
				m_fTalkStateTime = 0f;
			}
			else if ("starttalktexthide" == name)
			{
				m_iTalkTextIndex++;
				iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
				int iStageIndex = gameState.m_iStageIndex;
				iSniperStageCfg stageCfg = m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex);
				if (m_iTalkTextIndex < stageCfg.m_MovieStartTexts.Count)
				{
					m_enTalkState = STATE.kTextShow;
					m_fTalkStateTime = 0f;
				}
				else
				{
					m_enTalkState = STATE.kEnd;
					m_fTalkStateTime = 0f;
				}
			}
			else
			{
				if (name.IndexOf("tipshow_") != -1)
				{
					return;
				}
				if ("aimshotbygun" == name)
				{
					((UIImage)m_control_table["aimshotbygun"]).Visible = false;
				}
				else if ("shotbygun" == name)
				{
					((UIImage)m_control_table["shotbygun"]).Visible = false;
				}
				else if ("fadein" == name)
				{
					((UIImage)m_control_table["fadeimage"]).Visible = false;
					((UIImage)m_control_table["fadeimage"]).Enable = false;
					EnterStartState();
				}
				else if ("fadeout" == name)
				{
					switch (m_next_scene)
					{
					case NextScene.kMenu:
						Application.LoadLevel("iSniper.Menu");
						break;
					case NextScene.kRestart:
						Application.LoadLevel("iSniper.StoryStart");
						break;
					case NextScene.kResult:
					{
						iSniperGameState gameState2 = iSniperGameApp.GetInstance().m_GameState;
						if (gameState2.m_bBootCampsMode)
						{
							if (gameState2.m_bArcBootCamp)
							{
								gameState2.m_bStoryMode = false;
							}
							gameState2.m_bBootCampsMode = false;
							gameState2.m_bBootCampsOver = true;
							gameState2.SaveData();
							Application.LoadLevel("iSniper.ShowText");
						}
						else
						{
							Application.LoadLevel("iSniper.Result");
						}
						break;
					}
					}
				}
				else if (!("holdlightcycleshow" == name) && "holdlightcyclehide" == name)
				{
					((UIImage)m_control_table["holdlightcycle"]).Visible = false;
				}
			}
		}
	}

	public bool IsAim()
	{
		return ((UIImage)m_control_table["aimbk"]).Visible;
	}

	public bool ShowAimUI()
	{
		bool flag = IsAim();
		flag = !flag;
		if (flag)
		{
			if (!m_bPauseResume)
			{
				PlaySound(m_SoundZoomIn);
			}
			((UIImage)m_control_table["aimbk"]).Visible = true;
			((UIClickButton)m_control_table["firebtn"]).Visible = true;
			((UIClickButton)m_control_table["firebtn"]).Enable = true;
			((UIImage)m_control_table["firebk"]).Visible = true;
			((UIImage)m_control_table["bulletactive"]).Visible = true;
			if (m_iCurBulletIndex > 1)
			{
				((UIImage)m_control_table["bullet1"]).Visible = true;
			}
			if (m_iCurBulletIndex > 2)
			{
				((UIImage)m_control_table["bullet2"]).Visible = true;
			}
			if (m_iCurBulletIndex > 3)
			{
				((UIImage)m_control_table["bullet3"]).Visible = true;
			}
			if (m_iCurBulletIndex > 4)
			{
				((UIImage)m_control_table["bullet4"]).Visible = true;
			}
			if (m_iCurBulletIndex > 5)
			{
				((UIImage)m_control_table["bullet5"]).Visible = true;
			}
			((UIImage)m_control_table["bulletmask"]).Visible = true;
			if (((UIImage)m_control_table["firebtnmask"]).Enable)
			{
				((UIImage)m_control_table["firebtnmask"]).Visible = true;
			}
			if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				((UIImage)m_control_table["aimcdbk"]).Visible = true;
				((UIImage)m_control_table["holdbk"]).Visible = true;
				((UIPressButton)m_control_table["holdbtn"]).Visible = true;
				((UIPressButton)m_control_table["holdbtn"]).Enable = true;
				if (m_fDuringTime > 0f)
				{
					((UIImage)m_control_table["aimcdmask"]).Visible = true;
					((UIImage)m_control_table["holdbtnmask"]).Enable = true;
					((UIImage)m_control_table["holdbtnmask"]).Visible = true;
				}
				((UIImage)m_control_table["aimangleimage"]).Visible = true;
				((UIImage)m_control_table["aimmoveimagebk"]).Visible = true;
				((UINewMove)m_control_table["aimanglemove"]).Enable = true;
			}
			else
			{
				int num = ((!Utils.IsRetina()) ? 1 : 2);
				((UIJoystickButton)m_control_table["joystickbtn"]).MinDistance = 0f * (float)num;
				((UIJoystickButton)m_control_table["joystickbtn"]).MaxDistance = 30f * (float)num;
				((UIJoystickButton)m_control_table["joystickbtn"]).Visible = true;
				((UIJoystickButton)m_control_table["joystickbtn"]).Enable = true;
			}
		}
		else
		{
			if (!m_bPauseResume)
			{
				PlaySound(m_SoundZoomOut);
			}
			((UIImage)m_control_table["aimshotbygun"]).Visible = false;
			((UIImage)m_control_table["aimbk"]).Visible = false;
			((UIImage)m_control_table["holdbk"]).Visible = false;
			((UIImage)m_control_table["firebk"]).Visible = false;
			((UIImage)m_control_table["aimcdbk"]).Visible = false;
			((UIImage)m_control_table["aimcdmask"]).Visible = false;
			((UIPressButton)m_control_table["holdbtn"]).Visible = false;
			((UIClickButton)m_control_table["firebtn"]).Visible = false;
			((UIPressButton)m_control_table["holdbtn"]).Enable = false;
			((UIClickButton)m_control_table["firebtn"]).Enable = false;
			((UIImage)m_control_table["holdbtnmask"]).Enable = false;
			((UIImage)m_control_table["holdbtnmask"]).Visible = false;
			((UIImage)m_control_table["holdlightcycle"]).Visible = false;
			((UIJoystickButton)m_control_table["joystickbtn"]).Visible = false;
			((UIJoystickButton)m_control_table["joystickbtn"]).Enable = false;
			((UIImage)m_control_table["firebtnmask"]).Visible = false;
			((UIImage)m_control_table["bulletactive"]).Visible = false;
			((UIImage)m_control_table["bullet1"]).Visible = false;
			((UIImage)m_control_table["bullet2"]).Visible = false;
			((UIImage)m_control_table["bullet3"]).Visible = false;
			((UIImage)m_control_table["bullet4"]).Visible = false;
			((UIImage)m_control_table["bullet5"]).Visible = false;
			((UIImage)m_control_table["bulletmask"]).Visible = false;
			((UIImage)m_control_table["bulletshell"]).Visible = false;
			((UIImage)m_control_table["aimangleimage"]).Visible = false;
			((UIImage)m_control_table["aimmoveimagebk"]).Visible = false;
			((UIImage)m_control_table["aimmoveimagebk1"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage0"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage1"]).Visible = false;
			((UIImage)m_control_table["aimmoveimage2"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage0"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage1"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage2"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage3"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage4"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage5"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage6"]).Visible = false;
			((UIImage)m_control_table["aimmovesmallimage7"]).Visible = false;
			((UINewMove)m_control_table["aimanglemove"]).Enable = false;
			SetEnemyBlood(0, 0);
			HideTitleShotTip();
		}
		return flag;
	}

	public void ShowAim()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		if (!m_bPauseResume)
		{
			PlaySound(m_SoundZoomIn);
			m_bPauseResume = false;
		}
		((UIImage)m_control_table["aimbk"]).Visible = true;
		((UIImage)m_control_table["firebk"]).Visible = true;
		((UIClickButton)m_control_table["firebtn"]).Visible = true;
		((UIClickButton)m_control_table["firebtn"]).Enable = true;
		((UIImage)m_control_table["bulletactive"]).Visible = true;
		if (m_iCurBulletIndex > 1)
		{
			((UIImage)m_control_table["bullet1"]).Visible = true;
		}
		if (m_iCurBulletIndex > 2)
		{
			((UIImage)m_control_table["bullet2"]).Visible = true;
		}
		if (m_iCurBulletIndex > 3)
		{
			((UIImage)m_control_table["bullet3"]).Visible = true;
		}
		if (m_iCurBulletIndex > 4)
		{
			((UIImage)m_control_table["bullet4"]).Visible = true;
		}
		if (m_iCurBulletIndex > 5)
		{
			((UIImage)m_control_table["bullet5"]).Visible = true;
		}
		((UIImage)m_control_table["bulletmask"]).Visible = true;
		if (((UIImage)m_control_table["firebtnmask"]).Enable)
		{
			((UIImage)m_control_table["firebtnmask"]).Visible = true;
		}
		((UIImage)m_control_table["aimcdbk"]).Visible = true;
		((UIImage)m_control_table["holdbk"]).Visible = false;
		((UIPressButton)m_control_table["holdbtn"]).Visible = false;
		((UIPressButton)m_control_table["holdbtn"]).Enable = false;
		((UIImage)m_control_table["holdbtnmask"]).Enable = false;
		((UIImage)m_control_table["holdbtnmask"]).Visible = false;
		((UIImage)m_control_table["holdlightcycle"]).Visible = false;
		((UIImage)m_control_table["aimcdbk"]).Visible = false;
		((UIImage)m_control_table["aimcdmask"]).Visible = false;
		if (m_fDuringTime > 0f)
		{
			((UIImage)m_control_table["aimcdmask"]).Visible = true;
		}
		((UIImage)m_control_table["aimangleimage"]).Visible = true;
		((UIImage)m_control_table["aimmoveimagebk"]).Visible = true;
		((UINewMove)m_control_table["aimanglemove"]).Enable = true;
	}

	public void HideAim()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PlaySound(m_SoundZoomOut);
		((UIImage)m_control_table["aimshotbygun"]).Visible = false;
		((UIImage)m_control_table["aimbk"]).Visible = false;
		((UIImage)m_control_table["firebk"]).Visible = false;
		((UIImage)m_control_table["aimcdbk"]).Visible = false;
		((UIImage)m_control_table["aimcdmask"]).Visible = false;
		((UIPressButton)m_control_table["holdbtn"]).Visible = false;
		((UIClickButton)m_control_table["firebtn"]).Visible = false;
		((UIPressButton)m_control_table["holdbtn"]).Enable = false;
		((UIClickButton)m_control_table["firebtn"]).Enable = false;
		((UIImage)m_control_table["holdbtnmask"]).Enable = false;
		((UIImage)m_control_table["holdbtnmask"]).Visible = false;
		((UIImage)m_control_table["holdlightcycle"]).Visible = false;
		((UIJoystickButton)m_control_table["joystickbtn"]).Visible = false;
		((UIJoystickButton)m_control_table["joystickbtn"]).Enable = false;
		((UIImage)m_control_table["firebtnmask"]).Visible = false;
		((UIImage)m_control_table["bulletactive"]).Visible = false;
		((UIImage)m_control_table["bullet1"]).Visible = false;
		((UIImage)m_control_table["bullet2"]).Visible = false;
		((UIImage)m_control_table["bullet3"]).Visible = false;
		((UIImage)m_control_table["bullet4"]).Visible = false;
		((UIImage)m_control_table["bullet5"]).Visible = false;
		((UIImage)m_control_table["bulletmask"]).Visible = false;
		((UIImage)m_control_table["bulletshell"]).Visible = false;
		((UIImage)m_control_table["aimangleimage"]).Visible = false;
		((UIImage)m_control_table["aimmoveimagebk"]).Visible = false;
		((UIImage)m_control_table["aimmoveimagebk1"]).Visible = false;
		((UIImage)m_control_table["aimmoveimage0"]).Visible = false;
		((UIImage)m_control_table["aimmoveimage1"]).Visible = false;
		((UIImage)m_control_table["aimmoveimage2"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage0"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage1"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage2"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage3"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage4"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage5"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage6"]).Visible = false;
		((UIImage)m_control_table["aimmovesmallimage7"]).Visible = false;
		((UINewMove)m_control_table["aimanglemove"]).Enable = false;
		SetEnemyBlood(0, 0);
		HideTitleShotTip();
	}

	public void ComputeHoldCDAnimation()
	{
		float deltaTime = Time.deltaTime;
		if (m_bHolding)
		{
			if (iSniperGameApp.GetInstance().m_GameState.m_bTiltHolding)
			{
				m_fDuringTime += deltaTime;
				if (m_fDuringTime > m_fMaxTime)
				{
					m_fDuringTime = m_fMaxTime;
					iSniperGameApp.GetInstance().m_GameState.m_bTiltHolding = false;
				}
			}
		}
		else
		{
			m_fDuringTime -= deltaTime;
			if (m_fDuringTime < 0f)
			{
				((UIImage)m_control_table["holdbtnmask"]).Enable = false;
				((UIImage)m_control_table["holdbtnmask"]).Visible = false;
				m_fDuringTime = 0f;
				((UIImage)m_control_table["aimcdmask"]).Visible = false;
				m_needcompute = false;
			}
		}
		m_fCurAngle = m_fStartAngle + m_fDuringTime / m_fMaxTime * (m_fMaxAngle - m_fStartAngle);
		Vector2 position = new Vector2(35f + Mathf.Cos(m_fCurAngle) * 75f, 38f + Mathf.Sin(m_fCurAngle) * 75f);
		if (Utils.IsRetina())
		{
			position = new Vector2(68f + Mathf.Cos(m_fCurAngle) * 150f, 76f + Mathf.Sin(m_fCurAngle) * 150f);
		}
		((UIImage)m_control_table["aimcdmask"]).SetPosition(position);
		((UIImage)m_control_table["aimcdmask"]).SetAlpha(m_fDuringTime / m_fMaxAlphaTime);
		Rect rect = ((UIImage)m_control_table["aimcdbk"]).Rect;
		((UIImage)m_control_table["aimcdbk"]).SetClip(new Rect(rect.x, rect.y, rect.width, (1f - m_fDuringTime / m_fMaxTime) * rect.height));
	}

	public void ComputeBulletDownAnimation()
	{
		m_fBulletDownTime += Time.deltaTime;
		Vector2 position;
		if (Utils.IsRetina())
		{
			float num = m_fBulletDownTime * 10f;
			float num2 = m_fBulletSpeedH * num;
			float num3 = m_fBulletSpeedV * num - 18f * num * num;
			position = new Vector2(738f + num2, 104f + num3);
		}
		else
		{
			float num4 = m_fBulletDownTime * 10f;
			float num5 = m_fBulletSpeedH * num4;
			float num6 = m_fBulletSpeedV * num4 - 9f * num4 * num4;
			position = new Vector2(369f + num5, 52f + num6);
		}
		((UIImage)m_control_table["bulletshell"]).SetPosition(position);
		((UIImage)m_control_table["bulletshell"]).SetRotation((float)Math.PI * 2f * m_fBulletDownTime / 0.3f);
		if (position.y < -10f)
		{
			m_bStartDown = false;
			if (Utils.IsRetina())
			{
				((UIImage)m_control_table["bulletshell"]).SetPosition(new Vector2(738f, 104f));
			}
			else
			{
				((UIImage)m_control_table["bulletshell"]).SetPosition(new Vector2(369f, 52f));
			}
			((UIImage)m_control_table["bulletshell"]).SetRotation(0f);
			((UIImage)m_control_table["bulletshell"]).Visible = false;
			m_fBulletDownTime = 0f;
		}
	}

	public void SetGameTime(float time)
	{
		string text = string.Empty + time.ToString("f2");
		((UIText)m_control_table["gametimetext"]).SetText(text);
	}

	public void SetEnemyData(int current, int sum)
	{
		string text = current + "/" + sum;
		((UIText)m_control_table["enemydatatext"]).SetText(text);
	}

	public void SetPlayerBlood(int current)
	{
		int playerHealth = iSniperGameApp.GetInstance().m_GameState.GetPlayerHealth();
		if (Utils.IsRetina())
		{
			((UIImage)m_control_table["playerbloodimage"]).SetClip(new Rect(8f, 620f, 168f * ((float)current * 1f / (float)playerHealth), 20f));
		}
		else
		{
			((UIImage)m_control_table["playerbloodimage"]).SetClip(new Rect(4f, 310f, 84f * ((float)current * 1f / (float)playerHealth), 10f));
		}
		((UIText)m_control_table["playerbloodtext"]).SetText(current.ToString());
	}

	public void SetEnemyBlood(int current, int health)
	{
		if (current == 0 && health == 0)
		{
			((UIText)m_control_table["enemybloodtext"]).SetText(string.Empty);
		}
		else
		{
			((UIText)m_control_table["enemybloodtext"]).SetText(current + "/" + health);
		}
	}

	public void SetPlayerScore(int score)
	{
		((UIText)m_control_table["playerscoretext"]).SetText(score.ToString("D6"));
	}

	public void ShowShotByGun()
	{
		if (IsAim())
		{
			StopAnimation("aimshotbygun");
			((UIImage)m_control_table["aimshotbygun"]).Visible = true;
			((UIImage)m_control_table["aimshotbygun"]).SetAlpha(0f);
			StartAnimation("aimshotbygun");
		}
		else
		{
			StopAnimation("shotbygun");
			((UIImage)m_control_table["shotbygun"]).Visible = true;
			((UIImage)m_control_table["shotbygun"]).SetAlpha(0f);
			StartAnimation("shotbygun");
		}
	}

	public void ShowTitleShotTip(string title)
	{
		if (m_title_shottip_animation != null && m_title_shottip_animation.Length > 0)
		{
			StopAnimation(m_title_shottip_animation);
		}
		HideTitleShotTip();
		Vector2 position = new Vector2(240f, 210f);
		Vector2 position2 = new Vector2(240f, 190f);
		Vector2 position3 = new Vector2(240f, 170f);
		if (Utils.IsRetina())
		{
			position = new Vector2(480f, 420f);
			position2 = new Vector2(480f, 380f);
			position3 = new Vector2(480f, 340f);
		}
		float scale = 4f;
		if ("head" == title)
		{
			iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
			if (!gameState.m_bArcadeIsLock && !gameState.m_bStoryMode && gameState.m_iArcCurScene == 4)
			{
				SetTipImageProperty("headshottip", position, scale);
				SetTipImageProperty("enemyeliminated", position2, scale);
			}
			else
			{
				SetTipImageProperty("headshottip", position, scale);
				SetTipImageProperty("enemyeliminated", position2, scale);
				SetTipImageProperty("+6", position3, scale);
			}
			PlaySound(m_SoundHeadTitleShot);
			m_title_shottip_animation = "tipshow_headshottip";
		}
		else if ("normal" == title)
		{
			iSniperGameState gameState2 = iSniperGameApp.GetInstance().m_GameState;
			if (!gameState2.m_bArcadeIsLock && !gameState2.m_bStoryMode && gameState2.m_iArcCurScene == 4)
			{
				SetTipImageProperty("normalhittip", position, scale);
				SetTipImageProperty("targedestroy", position2, scale);
			}
			else
			{
				SetTipImageProperty("normalhittip", position, scale);
				SetTipImageProperty("targedestroy", position2, scale);
				SetTipImageProperty("+5", position3, scale);
			}
			m_title_shottip_animation = "tipshow_normalhittip";
		}
		else if ("cratical" == title)
		{
			iSniperGameState gameState3 = iSniperGameApp.GetInstance().m_GameState;
			if (!gameState3.m_bArcadeIsLock && !gameState3.m_bStoryMode && gameState3.m_iArcCurScene == 4)
			{
				SetTipImageProperty("craticalhittip", position, scale);
				SetTipImageProperty("targedestroy", position2, scale);
			}
			else
			{
				SetTipImageProperty("craticalhittip", position, scale);
				SetTipImageProperty("targedestroy", position2, scale);
				SetTipImageProperty("+10", position3, scale);
			}
			m_title_shottip_animation = "tipshow_craticalhittip";
		}
		else if ("body" == title)
		{
			iSniperGameState gameState4 = iSniperGameApp.GetInstance().m_GameState;
			if (!gameState4.m_bArcadeIsLock && !gameState4.m_bStoryMode && gameState4.m_iArcCurScene == 4)
			{
				SetTipImageProperty("bodyshottip", position, scale);
				SetTipImageProperty("enemyeliminated", position2, scale);
			}
			else
			{
				SetTipImageProperty("bodyshottip", position, scale);
				SetTipImageProperty("enemyeliminated", position2, scale);
				SetTipImageProperty("+4", position3, scale);
			}
			m_title_shottip_animation = "tipshow_bodyshottip";
		}
		else if ("lucky" == title)
		{
			SetTipImageProperty("luckyshottip", position, scale);
			m_title_shottip_animation = "tipshow_luckyshottip";
		}
		else if ("limb" == title)
		{
			iSniperGameState gameState5 = iSniperGameApp.GetInstance().m_GameState;
			if (!gameState5.m_bArcadeIsLock && !gameState5.m_bStoryMode && gameState5.m_iArcCurScene == 4)
			{
				SetTipImageProperty("limbshottip", position, scale);
				SetTipImageProperty("enemycrippled", position2, scale);
			}
			else
			{
				SetTipImageProperty("limbshottip", position, scale);
				SetTipImageProperty("enemycrippled", position2, scale);
				SetTipImageProperty("+2", position3, scale);
			}
			m_title_shottip_animation = "tipshow_limbshottip";
		}
		else if ("friend" == title)
		{
			SetTipImageProperty("friendlyfiretip", position, scale);
			SetTipImageProperty("hostageinjured", position2, scale);
			SetTipImageProperty("-5", position3, scale);
			m_title_shottip_animation = "tipshow_friendlyfiretip";
		}
		else if ("health" == title)
		{
			SetTipImageProperty("healthbonus", position, scale);
			SetTipImageProperty("+10%hp", position3, scale);
			m_title_shottip_animation = "tipshow_healthbonus";
		}
		else if ("time" == title)
		{
			SetTipImageProperty("timebonus", position, scale);
			SetTipImageProperty("+10", position3, scale);
			m_title_shottip_animation = "tipshow_timebonus";
		}
		else if ("exp" == title)
		{
			SetTipImageProperty("friendlyfiretip", position, scale);
			SetTipImageProperty("-10", position3, scale);
			m_title_shottip_animation = "tipshow_friendlyfiretip";
		}
		else
		{
			Debug.Log("no find is show xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx " + title);
		}
		m_shottip_duringtime = 0f;
		StartAnimation(m_title_shottip_animation);
	}

	public void ExitTitleShotTip()
	{
		string[] array = m_title_shottip_animation.Split('_');
		StartAnimation("tipexit_" + array[1]);
	}

	public void HideTitleShotTip()
	{
		((UIImage)m_control_table["headshottip"]).Visible = false;
		((UIImage)m_control_table["normalhittip"]).Visible = false;
		((UIImage)m_control_table["craticalhittip"]).Visible = false;
		((UIImage)m_control_table["bodyshottip"]).Visible = false;
		((UIImage)m_control_table["luckyshottip"]).Visible = false;
		((UIImage)m_control_table["limbshottip"]).Visible = false;
		((UIImage)m_control_table["friendlyfiretip"]).Visible = false;
		((UIImage)m_control_table["healthpointbonus"]).Visible = false;
		((UIImage)m_control_table["enemyeliminated"]).Visible = false;
		((UIImage)m_control_table["timebonus"]).Visible = false;
		((UIImage)m_control_table["healthbonus"]).Visible = false;
		((UIImage)m_control_table["enemycrippled"]).Visible = false;
		((UIImage)m_control_table["scorebonus"]).Visible = false;
		((UIImage)m_control_table["targedestroy"]).Visible = false;
		((UIImage)m_control_table["hostageinjured"]).Visible = false;
		((UIImage)m_control_table["+20p"]).Visible = false;
		((UIImage)m_control_table["+500"]).Visible = false;
		((UIImage)m_control_table["+20"]).Visible = false;
		((UIImage)m_control_table["+10%hp"]).Visible = false;
		((UIImage)m_control_table["+6"]).Visible = false;
		((UIImage)m_control_table["+4"]).Visible = false;
		((UIImage)m_control_table["+2"]).Visible = false;
		((UIImage)m_control_table["+10"]).Visible = false;
		((UIImage)m_control_table["+5"]).Visible = false;
		((UIImage)m_control_table["-5"]).Visible = false;
		((UIImage)m_control_table["-10"]).Visible = false;
	}

	public void SetTipImageProperty(string name, Vector2 position, float scale)
	{
		((UIImage)m_control_table[name]).SetPosition(position);
		((UIImage)m_control_table[name]).SetScale(scale);
		((UIImage)m_control_table[name]).SetAlpha(0f);
		((UIImage)m_control_table[name]).Visible = true;
	}

	public IEnumerator StartMovieEffect()
	{
		yield return new WaitForSeconds(1f);
		if (IsAim())
		{
			ShowAimUI();
		}
		iSniperGameCamera cameraScript = m_GameScene.m_CameraScript;
		cameraScript.Restore();
		ShowHeadShotMask(false);
		HideTitleShotTip();
		yield return new WaitForSeconds(1f);
		if (Utils.IsRetina())
		{
			((UIImage)m_control_table["startMovieTop"]).Rect = new Rect(0f, 640f, 960f, 100f);
			((UIImage)m_control_table["startMovieBottom"]).Rect = new Rect(0f, -100f, 960f, 100f);
		}
		else
		{
			((UIImage)m_control_table["startMovieTop"]).Rect = new Rect(0f, 320f, 480f, 50f);
			((UIImage)m_control_table["startMovieBottom"]).Rect = new Rect(0f, -50f, 480f, 50f);
		}
		((UIImage)m_control_table["startMovieTop"]).Visible = true;
		((UIImage)m_control_table["startMovieBottom"]).Visible = true;
		StartAnimation("startmovieup");
		StartAnimation("startmoviedown");
	}

	public void EndMovieEffect()
	{
		if (Utils.IsRetina())
		{
			((UIImage)m_control_table["startMovieTop"]).Rect = new Rect(0f, 540f, 960f, 100f);
			((UIImage)m_control_table["startMovieBottom"]).Rect = new Rect(0f, 0f, 960f, 100f);
		}
		else
		{
			((UIImage)m_control_table["startMovieTop"]).Rect = new Rect(0f, 270f, 480f, 50f);
			((UIImage)m_control_table["startMovieBottom"]).Rect = new Rect(0f, 0f, 480f, 50f);
		}
		((UIImage)m_control_table["startMovieTop"]).Visible = true;
		((UIImage)m_control_table["startMovieBottom"]).Visible = true;
		StartAnimation("endmovieup");
		StartAnimation("endmoviedown");
	}

	public void DelaySwitchToResult()
	{
		StartCoroutine(SwitchToResult());
	}

	public IEnumerator SwitchToResult()
	{
		yield return new WaitForSeconds(1f);
		m_next_scene = NextScene.kResult;
		FadeOut();
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

	public void ShowHoldCycle()
	{
		((UIImage)m_control_table["holdlightcycle"]).Visible = true;
		((UIImage)m_control_table["holdlightcycle"]).SetAlpha(0f);
		StartAnimation("holdlightcycleshow");
	}

	public void HideHoldCycle()
	{
		((UIImage)m_control_table["holdlightcycle"]).Visible = true;
		((UIImage)m_control_table["holdlightcycle"]).SetAlpha(1f);
		StopAnimation("holdlightcycleshow");
		StartAnimation("holdlightcyclehide");
	}

	public void ReloadBullets()
	{
		if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			PlaySound(m_SoundReload1);
		}
		else
		{
			PlaySound(m_SoundReload2);
		}
		m_iCurBulletIndex = m_GunProperty.GetCurrentClip();
		if (m_iCurBulletIndex > 1)
		{
			((UIImage)m_control_table["bullet1"]).Visible = IsAim();
		}
		if (m_iCurBulletIndex > 2)
		{
			((UIImage)m_control_table["bullet2"]).Visible = IsAim();
		}
		if (m_iCurBulletIndex > 3)
		{
			((UIImage)m_control_table["bullet3"]).Visible = IsAim();
		}
		if (m_iCurBulletIndex > 4)
		{
			((UIImage)m_control_table["bullet4"]).Visible = IsAim();
		}
		if (m_iCurBulletIndex > 5)
		{
			((UIImage)m_control_table["bullet5"]).Visible = IsAim();
		}
		if (Utils.IsRetina())
		{
			((UIImage)m_control_table["bullet1"]).Rect = new Rect(764f, 70f, 116f, 26f);
			((UIImage)m_control_table["bullet2"]).Rect = new Rect(764f, 54f, 116f, 26f);
			((UIImage)m_control_table["bullet3"]).Rect = new Rect(764f, 38f, 116f, 26f);
			((UIImage)m_control_table["bullet4"]).Rect = new Rect(764f, 22f, 116f, 26f);
			((UIImage)m_control_table["bullet5"]).Rect = new Rect(764f, 6f, 116f, 26f);
		}
		else
		{
			((UIImage)m_control_table["bullet1"]).Rect = new Rect(382f, 35f, 58f, 13f);
			((UIImage)m_control_table["bullet2"]).Rect = new Rect(382f, 27f, 58f, 13f);
			((UIImage)m_control_table["bullet3"]).Rect = new Rect(382f, 19f, 58f, 13f);
			((UIImage)m_control_table["bullet4"]).Rect = new Rect(382f, 11f, 58f, 13f);
			((UIImage)m_control_table["bullet5"]).Rect = new Rect(382f, 3f, 58f, 13f);
		}
		StartAnimation("bulletload" + m_GunProperty.GetCurrentFireStar());
		StartAnimation("bulletactiveload" + m_GunProperty.GetCurrentFireStar());
	}

	public void ShowPauseUI(bool bVisible)
	{
		((UIImage)m_control_table["pausebk"]).Enable = bVisible;
		((UIImage)m_control_table["pausebk"]).Visible = bVisible;
		((UIImage)m_control_table["pausetextbk"]).Visible = bVisible;
		((UIImage)m_control_table["pausetext"]).Visible = bVisible;
		((UIImage)m_control_table["pauseflashimage1"]).Visible = bVisible;
		((UIImage)m_control_table["pauseflashimage2"]).Visible = bVisible;
		if (bVisible)
		{
			((UIImage)m_control_table["pauseflashimage1"]).SetAlpha(1f);
			((UIImage)m_control_table["pauseflashimage2"]).SetAlpha(1f);
			StartAnimation("pauseflashimage");
		}
		else
		{
			StopAnimation("pauseflashimage");
		}
		((UIImage)m_control_table["pauseresumebk"]).Visible = bVisible;
		((UIImage)m_control_table["pauserestartbk"]).Visible = bVisible;
		((UIImage)m_control_table["pauseoptionbk"]).Visible = bVisible;
		((UIImage)m_control_table["pausemenubk"]).Visible = bVisible;
		((UIClickButton)m_control_table["pauseresumebtn"]).Enable = bVisible;
		((UIClickButton)m_control_table["pauseresumebtn"]).Visible = bVisible;
		((UIClickButton)m_control_table["pauserestartbtn"]).Enable = bVisible;
		((UIClickButton)m_control_table["pauserestartbtn"]).Visible = bVisible;
		((UIClickButton)m_control_table["pauseoptionbtn"]).Enable = bVisible;
		((UIClickButton)m_control_table["pauseoptionbtn"]).Visible = bVisible;
		((UIClickButton)m_control_table["pausemenubtn"]).Enable = bVisible;
		((UIClickButton)m_control_table["pausemenubtn"]).Visible = bVisible;
	}

	public void ShowOptionUI(bool bVisible)
	{
		((UIImage)m_control_table["pauseresumebk"]).Visible = !bVisible;
		((UIImage)m_control_table["pauserestartbk"]).Visible = !bVisible;
		((UIImage)m_control_table["pauseoptionbk"]).Visible = !bVisible;
		((UIImage)m_control_table["pausemenubk"]).Visible = !bVisible;
		((UIClickButton)m_control_table["pauseresumebtn"]).Enable = !bVisible;
		((UIClickButton)m_control_table["pauseresumebtn"]).Visible = !bVisible;
		((UIClickButton)m_control_table["pauserestartbtn"]).Enable = !bVisible;
		((UIClickButton)m_control_table["pauserestartbtn"]).Visible = !bVisible;
		((UIClickButton)m_control_table["pauseoptionbtn"]).Enable = !bVisible;
		((UIClickButton)m_control_table["pauseoptionbtn"]).Visible = !bVisible;
		((UIClickButton)m_control_table["pausemenubtn"]).Enable = !bVisible;
		((UIClickButton)m_control_table["pausemenubtn"]).Visible = !bVisible;
		((UIImage)m_control_table["pausecontrolbk"]).Visible = bVisible;
		((UIImage)m_control_table["pauseinvertbk"]).Visible = bVisible;
		((UIImage)m_control_table["pausesentivitybk"]).Visible = bVisible;
		((UIImage)m_control_table["pausesoundbk"]).Visible = bVisible;
		((UIImage)m_control_table["pausemusicbk"]).Visible = bVisible;
		((UIImage)m_control_table["pausebackbk"]).Visible = bVisible;
		((UIClickButton)m_control_table["pausebackbtn"]).Enable = bVisible;
		((UIClickButton)m_control_table["pausebackbtn"]).Visible = bVisible;
		((UIPushButton)m_control_table["pausecontrolbtn"]).Enable = bVisible;
		((UIPushButton)m_control_table["pausecontrolbtn"]).Visible = bVisible;
		((UIPushButton)m_control_table["pauseinvertbtn"]).Enable = bVisible;
		((UIPushButton)m_control_table["pauseinvertbtn"]).Visible = bVisible;
		((UIPushButton)m_control_table["pausesoundbtn"]).Enable = bVisible;
		((UIPushButton)m_control_table["pausesoundbtn"]).Visible = bVisible;
		((UIPushButton)m_control_table["pausemusicbtn"]).Enable = bVisible;
		((UIPushButton)m_control_table["pausemusicbtn"]).Visible = bVisible;
		((UIImage)m_control_table["pausesenstivitymark"]).Visible = bVisible;
		((UIMove)m_control_table["pausesenstivitymove"]).Enable = bVisible;
		((UIClickButton)m_control_table["pauseresetbtn"]).Enable = bVisible;
		((UIImage)m_control_table["pauseinvert1"]).Visible = false;
		((UIImage)m_control_table["pauseinvert2"]).Visible = false;
		if (bVisible)
		{
			((UIPushButton)m_control_table["pausecontrolbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl);
			((UIPushButton)m_control_table["pauseinvertbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bIsInvertYAixs);
			((UIPushButton)m_control_table["pausesoundbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bSoundOn);
			((UIPushButton)m_control_table["pausemusicbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bMusicOn);
			if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				((UIPushButton)m_control_table["pauseinvertbtn"]).Enable = true;
				((UIImage)m_control_table["pauseinvert1"]).Visible = false;
				((UIImage)m_control_table["pauseinvert2"]).Visible = true;
			}
			else
			{
				((UIPushButton)m_control_table["pauseinvertbtn"]).Enable = false;
				((UIImage)m_control_table["pauseinvert1"]).Visible = true;
				((UIImage)m_control_table["pauseinvert2"]).Visible = false;
			}
			float num = iSniperGameApp.GetInstance().m_GameState.m_fCurrentSensitivty - 5f;
			float num2 = 45f;
			float num3 = num / num2;
			int num4 = ((!Utils.IsRetina()) ? 1 : 2);
			Vector2 position = ((UIImage)m_control_table["pausesenstivitymark"]).GetPosition();
			position.x = 138f + num3 * 78f;
			position.x *= num4;
			((UIImage)m_control_table["pausesenstivitymark"]).SetPosition(position);
		}
	}

	public void ShowHeadShotMask(bool bVisible)
	{
		((UIImage)m_control_table["headshotmask"]).Visible = bVisible;
	}

	public void PlayBkMusic()
	{
		if (null != m_MusicBk)
		{
			m_MusicBk.loop = true;
			if (iSniperGameApp.GetInstance().m_GameState.m_bMusicOn)
			{
				m_MusicBk.volume = 1f;
			}
			else
			{
				m_MusicBk.volume = 0f;
			}
			m_MusicBk.Play();
		}
	}

	public void CheckAudioObjs()
	{
		m_MusicBk = ((!Utils.ProbabilityIsRandomHit(0.5f)) ? GetAudioSource("Main Camera/Sounds/MusicObj2") : GetAudioSource("Main Camera/Sounds/MusicObj1"));
		m_SoundScene = GetAudioSource("Main Camera/Sounds/SceneMusic");
		iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
		int iStageIndex = gameState.m_iStageIndex;
		if (m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex).m_bMusicOn)
		{
			PlayBkMusic();
		}
		else
		{
			m_SoundScene.loop = true;
			PlayBkSound(m_SoundScene);
		}
		string path = "iSniper3D/Prefabs/GunSound/" + m_GunProperty.m_strSound + "_Prefab";
		GameObject gameObject = Instantiate(Resources.Load<GameObject>(path));
		gameObject.transform.parent = Camera.main.transform;
		gameObject.transform.position = Camera.main.transform.position;
		gameObject.name = m_GunProperty.m_strSound + "_Prefab";
		m_SoundGun1 = GetAudioSource("Main Camera/" + gameObject.name + "/Shot/Shot1");
		m_SoundGun2 = GetAudioSource("Main Camera/" + gameObject.name + "/Shot/Shot1");
		m_SoundReload1 = GetAudioSource("Main Camera/" + gameObject.name + "/ReLoad/ReLoad1");
		m_SoundReload2 = GetAudioSource("Main Camera/" + gameObject.name + "/ReLoad/ReLoad2");
		m_SoundBulletoutL = GetAudioSource("Main Camera/" + gameObject.name + "/Hit/Hit1");
		m_SoundBulletoutH = GetAudioSource("Main Camera/" + gameObject.name + "/Hit/Hit2");
		m_SoundBreathOn = GetAudioSource("Main Camera/Sounds/SoundBreathOn");
		m_SoundBreathOff = GetAudioSource("Main Camera/Sounds/SoundBreathOff");
		m_SoundZoomIn = GetAudioSource("Main Camera/Sounds/SoundZoomIn");
		m_SoundZoomOut = GetAudioSource("Main Camera/Sounds/SoundZoomOut");
		m_SoundBulletUp = GetAudioSource("Main Camera/Sounds/SountBulletUp");
		m_SoundHurt1 = GetAudioSource("Main Camera/Sounds/SoundHurt1");
		m_SoundHurt2 = GetAudioSource("Main Camera/Sounds/SoundHurt2");
		m_SoundFlex1 = GetAudioSource("Main Camera/Sounds/SoundFlex1");
		m_SoundFlex2 = GetAudioSource("Main Camera/Sounds/SoundFlex2");
		m_SoundShotReveb1 = GetAudioSource("SceneSound_Prefab/Reveb01");
		m_SoundShotReveb2 = GetAudioSource("SceneSound_Prefab/Reveb02");
		m_SoundHeadTitleShot = GetAudioSource("SceneSound_Prefab/SoundHeadTitleShot");
		m_SoundBack = GetAudioSource("SceneSound_Prefab/SoundBack");
		m_SoundClick = GetAudioSource("SceneSound_Prefab/SoundClick");
	}

	public void TurnMusic()
	{
		if (!(null == m_MusicBk))
		{
			if (iSniperGameApp.GetInstance().m_GameState.m_bMusicOn)
			{
				m_MusicBk.volume = 1f;
			}
			else
			{
				m_MusicBk.volume = 0f;
			}
		}
	}

	public void PlaySound(AudioSource audio)
	{
		if (!(null == audio) && iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			audio.Play();
		}
	}

	public void PlayBkSound(AudioSource audio)
	{
		if (!(null == audio) && iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && !audio.isPlaying)
		{
			audio.Play();
		}
	}

	public void PlaySoundAtPos(AudioSource audio, Vector3 pos)
	{
		if (!(null == audio) && iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			audio.transform.parent.position = pos;
			audio.Play();
		}
	}

	public void SwitchAimLockImg()
	{
		m_fHelpAimSwitch -= Time.deltaTime;
		if (m_fHelpAimSwitch <= 0f)
		{
			((UIImage)m_control_table["touchimage"]).Visible = ((UIImage)m_control_table["touchimage1"]).Visible;
			((UIImage)m_control_table["touchimage1"]).Visible = !((UIImage)m_control_table["touchimage1"]).Visible;
			m_fHelpAimSwitch = 0.5f;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause || m_GameScene == null)
		{
			return;
		}
		iSniperGameScene.State state = m_GameScene.m_State;
		if (state == iSniperGameScene.State.kGaming && !m_GameScene.m_GameState.m_bBootCampsMode)
		{
			if ((bool)m_SoundScene && m_GameScene.m_GameState.m_bSoundOn)
			{
				m_SoundScene.volume = 0f;
			}
			if ((bool)m_MusicBk && m_GameScene.m_GameState.m_bMusicOn)
			{
				m_MusicBk.volume = 0f;
			}
			PlaySound(m_SoundClick);
			ShowPauseUI(true);
			((UIText)m_control_table["tipText"]).Visible = false;
			Time.timeScale = 0f;
			Debug.Log("NO Pause!");
		}
	}
}
