using UnityEngine;

public class iSniperResultUIHelper : UIHelper
{
	private enum NextScene
	{
		kMenu = 0,
		kStoryStart = 1
	}

	private enum Item
	{
		kScore = 0,
		kAccuracy = 1,
		kHealth = 2,
		kTime = 3,
		kCashBonus = 4,
		kCashTotal = 5,
		kExperBonus = 6,
		kExperTotal = 7,
		kNone = 8
	}

	private const float ANIMATIONTIME = 0.3f;

	private NextScene m_next_scene;

	private float m_animation_time;

	private Item m_animation_item;

	private bool m_bPlayerLevelUp;

	private int m_iLevelUpRollTimes = 2;

	private float m_accurcay_degree;

	private iSniperGameState m_GameState;

	private AudioSource m_SoundClick;

	private AudioSource m_SoundCountLoop;

	private AudioSource m_SoundCountEnd;

	private AudioSource m_SoundBack;

	private AudioSource m_SoundSuccess;

	private iSniperGameCenter m_GameCenter;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		m_GameCenter = GameObject.Find("Main Camera").GetComponent<iSniperGameCenter>();
		m_GameState.m_bLastArcLock = m_GameState.m_bArcadeIsLock;
		iSniperGameState.Result gameResult = m_GameState.m_GameResult;
		if (gameResult == iSniperGameState.Result.kSuccess)
		{
			m_ui_cfgxml = "iSniper3D/UI/ResultSuccessUI";
			if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
			{
				m_GameState.m_bArcWinState = true;
				m_GameState.m_bArcLockScene = false;
				m_GameState.m_iArcLastScene = m_GameState.m_iArcCurScene;
				m_GameState.m_iArcLastStage = m_GameState.m_iArcCurStage;
				m_GameState.m_iArcDaysNum++;
				m_GameState.m_bLeaderBoard = true;
				if (m_GameState.m_iArcDaysNum >= 31 && m_GameState.m_iAchBest == 0)
				{
					m_GameState.m_iAchBest = 1;
				}
			}
			if (m_GameState.m_bStoryMode)
			{
				if (m_GameState.m_iPlayerCurrentScene < 8)
				{
					m_GameState.m_iPlayerCurrentScene++;
				}
				else
				{
					m_GameState.m_bIsEndedScene = true;
					m_GameState.m_bArcadeIsLock = false;
					if (m_GameState.m_iAchElite == 0)
					{
						m_GameState.m_iAchElite = 1;
					}
				}
			}
			m_SoundSuccess = GetAudioSource("Main Camera/SoundSuccess");
			PlayMusic(m_SoundSuccess);
			FlurryPlugin.logEvent("EnterResultSuccess", gameResult.ToString(), "SuccessType");
		}
		else
		{
			m_ui_cfgxml = "iSniper3D/UI/ResultFailedUI";
			if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
			{
				m_GameState.m_bArcWinState = false;
			}
			if (m_GameState.m_bStoryMode && m_GameState.m_iPlayerCurrentScene == 1 && m_GameState.m_iAchWorst == 0)
			{
				m_GameState.m_iAchWorst = 1;
			}
			FlurryPlugin.logEvent("EnterResultFailed", gameResult.ToString(), "FailedType");
		}
		base.Start();
		if (gameResult == iSniperGameState.Result.kBloodup)
		{
			((UIImage)m_control_table["bloodupbk"]).Visible = true;
		}
		m_animation_item = Item.kNone;
		m_accurcay_degree = 0f;
		if (m_GameState.m_iFireNum != 0)
		{
			m_accurcay_degree = (float)m_GameState.m_iHitNum * 100f / (float)m_GameState.m_iFireNum;
		}
		m_GameState.m_iGameExperience = (int)((float)(m_GameState.m_iPlayerScore / 50) * (m_accurcay_degree / 100f) + (float)(m_GameState.m_iHeadshotNum * 5));
		m_GameState.m_iPlayer2NextLevelExperience += m_GameState.m_iGameExperience;
		int iPlayerLevel = m_GameState.m_iPlayerLevel;
		((UIText)m_control_table["leveltext"]).SetText(iPlayerLevel.ToString());
		m_bPlayerLevelUp = m_GameState.RecomputeLevel() == ++iPlayerLevel;
		m_GameState.m_iGameCash = (int)((float)(m_GameState.m_iPlayerScore / 5) * (m_accurcay_degree / 100f) + (float)m_GameState.m_iPlayerCurrentHealth + m_GameState.m_fGameTimeBonus);
		if (gameResult != 0)
		{
			m_GameState.m_iGameCash /= 2;
		}
		m_GameState.m_iPlayerCash += m_GameState.m_iGameCash;
		m_GameState.SaveData();
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundCountLoop = GetAudioSource("Main Camera/CountLoop");
		m_SoundCountEnd = GetAudioSource("Main Camera/CountEnd");
		m_SoundBack = GetAudioSource("Main Camera/SoundBack");
		StartAnimation("flashimage");
		FadeIn();
		if ((Screen.width <= 960 || Screen.height <= 640) && Screen.width > 640 && Screen.height <= 960)
		{
		}
	}

	public void Update()
	{
		m_GameCenter.SentInfo2GameCenter();
		m_GameCenter.CheckIsSuccess();
		if (m_animation_item == Item.kNone)
		{
			return;
		}
		m_animation_time += Time.deltaTime;
		float t = m_animation_time / 0.3f;
		PlaySound(m_SoundCountLoop);
		switch (m_animation_item)
		{
		case Item.kScore:
		{
			int num8 = (int)Mathf.Lerp(0f, m_GameState.m_iPlayerScore, t);
			((UIText)m_control_table["scoretext"]).SetText(num8.ToString());
			break;
		}
		case Item.kAccuracy:
		{
			float num7 = Mathf.Lerp(0f, m_accurcay_degree, t);
			((UIText)m_control_table["accuracytext"]).SetText(num7.ToString("f0") + "%");
			break;
		}
		case Item.kHealth:
		{
			int num6 = (int)Mathf.Lerp(0f, m_GameState.m_iPlayerCurrentHealth, t);
			((UIText)m_control_table["healthtext"]).SetText(num6.ToString());
			break;
		}
		case Item.kTime:
		{
			float num5 = Mathf.Lerp(0f, m_GameState.m_fGameTime, t);
			((UIText)m_control_table["timetext"]).SetText(num5.ToString("f0") + "s");
			break;
		}
		case Item.kCashBonus:
		{
			int num4 = (int)Mathf.Lerp(0f, m_GameState.m_iGameCash, t);
			((UIText)m_control_table["cashbonustext"]).SetText(num4.ToString());
			break;
		}
		case Item.kCashTotal:
		{
			int num3 = (int)Mathf.Lerp(0f, m_GameState.m_iPlayerCash, t);
			((UIText)m_control_table["cashtotaltext"]).SetText(num3.ToString());
			break;
		}
		case Item.kExperBonus:
		{
			int num2 = (int)Mathf.Lerp(0f, m_GameState.m_iGameExperience, t);
			((UIText)m_control_table["experiencebonustext"]).SetText(num2.ToString());
			break;
		}
		case Item.kExperTotal:
		{
			int num = (int)Mathf.Lerp(0f, m_GameState.GetPlayerExperience(), t);
			((UIText)m_control_table["experiencetotaltext"]).SetText(num.ToString());
			break;
		}
		}
		if (!(m_animation_time >= 0.3f))
		{
			return;
		}
		m_animation_time = 0f;
		m_animation_item++;
		if (m_animation_item == Item.kNone)
		{
			if (null != m_SoundCountLoop)
			{
				m_SoundCountLoop.GetComponent<AudioSource>().Stop();
			}
			PlaySound(m_SoundCountEnd);
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("mainmenubtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			iSniperGameState.Result gameResult = m_GameState.m_GameResult;
			m_next_scene = NextScene.kMenu;
			FadeOut();
		}
		if (GetControlId("continuebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			m_next_scene = NextScene.kStoryStart;
			if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
			{
				m_GameState.m_bStoryMode = false;
				Debug.Log(iSniperGameApp.GetInstance().m_GameState.m_bArcWinState);
				m_GameState.RecomputeScene(m_GameState.m_iArcDaysNum, m_GameState.m_bArcWinState);
				m_GameState.m_bArcLockScene = true;
				m_GameState.SaveData();
			}
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
			StartAnimation("scoreimage");
		}
		else if ("fadeout" == name)
		{
			switch (m_next_scene)
			{
			case NextScene.kMenu:
				Application.LoadLevel("iSniper.Menu");
				break;
			case NextScene.kStoryStart:
				if (m_GameState.m_GameResult == iSniperGameState.Result.kSuccess)
				{
					if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
					{
						Application.LoadLevel("iSniper.StoryStart");
						break;
					}
					m_GameState.m_bStoryStart = false;
					Application.LoadLevel("iSniper.Story");
				}
				else
				{
					Application.LoadLevel("iSniper.StoryStart");
				}
				break;
			}
		}
		else if ("scoreimage" == name)
		{
			m_animation_item = Item.kScore;
			m_animation_time = 0f;
			((UIImage)m_control_table["scoreimage"]).Visible = false;
			StartAnimation("accuracyimage");
		}
		else if ("accuracyimage" == name)
		{
			((UIImage)m_control_table["accuracyimage"]).Visible = false;
			StartAnimation("healthimage");
		}
		else if ("healthimage" == name)
		{
			((UIImage)m_control_table["healthimage"]).Visible = false;
			StartAnimation("timeimage");
		}
		else if ("timeimage" == name)
		{
			((UIImage)m_control_table["timeimage"]).Visible = false;
			StartAnimation("cashbonusimage");
		}
		else if ("cashbonusimage" == name)
		{
			((UIImage)m_control_table["cashbonusimage"]).Visible = false;
			StartAnimation("cashtotalimage");
		}
		else if ("cashtotalimage" == name)
		{
			((UIImage)m_control_table["cashtotalimage"]).Visible = false;
			StartAnimation("experiencebonusimage");
		}
		else if ("experiencebonusimage" == name)
		{
			((UIImage)m_control_table["experiencebonusimage"]).Visible = false;
			StartAnimation("experiencetotalimage");
		}
		else if ("experiencetotalimage" == name)
		{
			((UIImage)m_control_table["experiencetotalimage"]).Visible = false;
			((UIImage)m_control_table["buttondisableimage"]).Visible = false;
			((UIImage)m_control_table["buttondisableimage"]).Enable = false;
			if (m_bPlayerLevelUp)
			{
				((UIImage)m_control_table["levelEffect"]).Visible = true;
				((UIImage)m_control_table["levelUpPonit"]).Visible = true;
				((UIText)m_control_table["leveltext"]).SetText("UP");
				StartAnimation("levelUpPonit");
				m_bPlayerLevelUp = false;
			}
		}
		else if ("levelUpPonit" == name)
		{
			StartAnimation("levelUpPonit1");
		}
		else if ("levelUpPonit1" == name)
		{
			StartAnimation("levelUpPonit2");
		}
		else if ("levelUpPonit2" == name)
		{
			StartAnimation("levelUpPonit3");
		}
		else if ("levelUpPonit3" == name)
		{
			if (m_iLevelUpRollTimes <= 0)
			{
				((UIImage)m_control_table["levelEffect"]).Visible = false;
				((UIImage)m_control_table["levelUpPonit"]).Visible = false;
				((UIText)m_control_table["leveltext"]).SetText(m_GameState.m_iPlayerLevel.ToString());
				m_iLevelUpRollTimes = 2;
			}
			else
			{
				StartAnimation("levelUpPonit");
				m_iLevelUpRollTimes--;
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

	private void PlaySound(AudioSource audio)
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && null != audio)
		{
			audio.Play();
		}
	}

	private void PlayMusic(AudioSource audio)
	{
		if (null != audio)
		{
			if (iSniperGameApp.GetInstance().m_GameState.m_bMusicOn)
			{
				audio.volume = 1f;
			}
			else
			{
				audio.volume = 0f;
			}
			audio.Play();
		}
	}
}
