using UnityEngine;

public class iSniperOptionUIHelper : UIHelper
{
	private int m_help_index;

	private Rect[] m_LeftMatRect;

	private Rect[] m_RightMatRect;

	private int m_MatIndex;

	private float m_ShiftTime;

	private AudioSource m_SoundClick;

	private AudioSource m_SoundBack;

	private Vector2 m_fCreatlist_pos;

	private float m_move_delta;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/OptionUI";
		base.Start();
		m_help_index = 0;
		m_MatIndex = 0;
		m_ShiftTime = 0f;
		int num = 1;
		if (Utils.IsRetina())
		{
			num = 2;
		}
		m_LeftMatRect = new Rect[3];
		m_LeftMatRect[0] = new Rect(14 * num, 385 * num, 30 * num, 11 * num);
		m_LeftMatRect[1] = new Rect(14 * num, 405 * num, 30 * num, 11 * num);
		m_LeftMatRect[2] = new Rect(14 * num, 425 * num, 30 * num, 11 * num);
		m_RightMatRect = new Rect[3];
		m_RightMatRect[0] = new Rect(87 * num, 385 * num, 30 * num, 11 * num);
		m_RightMatRect[1] = new Rect(87 * num, 405 * num, 30 * num, 11 * num);
		m_RightMatRect[2] = new Rect(87 * num, 425 * num, 30 * num, 11 * num);
		StartAnimation("flashimage");
		((UISelectButton)m_control_table["controlbtn"]).Set(true);
		HideAllPanel();
		SlideControlPanel();
		FadeIn();
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundBack = GetAudioSource("Main Camera/SoundBack");
		FlurryPlugin.logEvent("EnterOptionUI");
	}

	public void Update()
	{
		m_ShiftTime += Time.deltaTime;
		if (m_ShiftTime >= 0.2f)
		{
			m_ShiftTime = 0f;
			m_MatIndex++;
			m_MatIndex %= 3;
		}
		((UIImage)m_control_table["leftMark"]).SetTexture(m_LeftMatRect[m_MatIndex]);
		((UIImage)m_control_table["rightMark"]).SetTexture(m_RightMatRect[m_MatIndex]);
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (GetControlId("creditlistmove") == control.Id)
		{
			switch (command)
			{
			case 1:
			{
				m_fCreatlist_pos.y += lparam;
				m_move_delta += lparam;
				UIImage uIImage2 = (UIImage)m_control_table["creditlist"];
				if (Utils.IsRetina())
				{
					uIImage2.Rect = new Rect(uIImage2.Rect.x, uIImage2.Rect.y + lparam, uIImage2.Rect.width, uIImage2.Rect.height);
					uIImage2.SetClip(new Rect(0f, 160f, 600f, 400f));
				}
				else
				{
					uIImage2.Rect = new Rect(uIImage2.Rect.x, uIImage2.Rect.y + lparam, uIImage2.Rect.width, uIImage2.Rect.height);
					uIImage2.SetClip(new Rect(0f, 80f, 300f, 200f));
				}
				break;
			}
			case 2:
			{
				UIImage uIImage = (UIImage)m_control_table["creditlist"];
				uIImage.Rect = new Rect(uIImage.Rect.x, uIImage.Rect.y + lparam, uIImage.Rect.width, uIImage.Rect.height);
				if (Utils.IsRetina())
				{
					if (uIImage.Rect.y < 70f)
					{
						uIImage.Rect = new Rect(uIImage.Rect.x, 70f, uIImage.Rect.width, uIImage.Rect.height);
					}
					if (uIImage.Rect.y > 170f)
					{
						uIImage.Rect = new Rect(uIImage.Rect.x, 170f, uIImage.Rect.width, uIImage.Rect.height);
					}
				}
				else
				{
					if (uIImage.Rect.y < 35f)
					{
						uIImage.Rect = new Rect(uIImage.Rect.x, 35f, uIImage.Rect.width, uIImage.Rect.height);
					}
					if (uIImage.Rect.y > 85f)
					{
						uIImage.Rect = new Rect(uIImage.Rect.x, 85f, uIImage.Rect.width, uIImage.Rect.height);
					}
				}
				break;
			}
			}
		}
		if (GetControlId("controlbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			((UISelectButton)m_control_table["soundbtn"]).Set(false);
			((UISelectButton)m_control_table["helpbtn"]).Set(false);
			((UISelectButton)m_control_table["creditsbtn"]).Set(false);
			((UISelectButton)m_control_table["extrabtn"]).Set(false);
			HideAllPanel();
			SlideControlPanel();
		}
		if (GetControlId("soundbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			((UISelectButton)m_control_table["controlbtn"]).Set(false);
			((UISelectButton)m_control_table["helpbtn"]).Set(false);
			((UISelectButton)m_control_table["creditsbtn"]).Set(false);
			((UISelectButton)m_control_table["extrabtn"]).Set(false);
			HideAllPanel();
			SlideSoundPanel();
		}
		if (GetControlId("helpbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			((UISelectButton)m_control_table["controlbtn"]).Set(false);
			((UISelectButton)m_control_table["soundbtn"]).Set(false);
			((UISelectButton)m_control_table["creditsbtn"]).Set(false);
			((UISelectButton)m_control_table["extrabtn"]).Set(false);
			HideAllPanel();
			SlideHelpPanel();
		}
		if (GetControlId("creditsbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			((UISelectButton)m_control_table["controlbtn"]).Set(false);
			((UISelectButton)m_control_table["soundbtn"]).Set(false);
			((UISelectButton)m_control_table["helpbtn"]).Set(false);
			((UISelectButton)m_control_table["extrabtn"]).Set(false);
			HideAllPanel();
			SlideCreditsPanel();
		}
		if (GetControlId("extrabtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			((UISelectButton)m_control_table["controlbtn"]).Set(false);
			((UISelectButton)m_control_table["soundbtn"]).Set(false);
			((UISelectButton)m_control_table["helpbtn"]).Set(false);
			((UISelectButton)m_control_table["creditsbtn"]).Set(false);
			HideAllPanel();
			SlideExtraPanel();
		}
		if (GetControlId("tiltjoystickbtn") == control.Id)
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
				((UIImage)m_control_table["InvertY"]).Visible = false;
				((UIImage)m_control_table["InvertY1"]).Visible = true;
				((UIPushButton)m_control_table["invertaxisbtn"]).Enable = true;
			}
			else
			{
				((UIImage)m_control_table["InvertY"]).Visible = true;
				((UIImage)m_control_table["InvertY1"]).Visible = false;
				((UIPushButton)m_control_table["invertaxisbtn"]).Enable = false;
			}
			iSniperGameApp.GetInstance().m_GameState.SaveData();
			FlurryPlugin.logEvent("Operation", iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl.ToString(), "bIsTiltControl");
		}
		if (GetControlId("invertaxisbtn") == control.Id)
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
		if (GetControlId("sensitivitymove") == control.Id && command == 1)
		{
			UIImage uIImage3 = (UIImage)m_control_table["sensitivitymark"];
			Vector2 position = uIImage3.GetPosition();
			position.x += wparam;
			int num = ((!Utils.IsRetina()) ? 1 : 2);
			if (position.x < (float)(56 * num))
			{
				position.x = 56 * num;
			}
			else if (position.x > (float)(138 * num))
			{
				position.x = 138 * num;
			}
			uIImage3.SetPosition(position);
			float num2 = (position.x - (float)(56 * num)) / (float)(138 * num - 56 * num);
			float num3 = num2 * 45f;
			iSniperGameApp.GetInstance().m_GameState.m_fCurrentSensitivty = 5f + num3;
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("resetbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			int num4 = ((!Utils.IsRetina()) ? 1 : 2);
			UIImage uIImage4 = (UIImage)m_control_table["sensitivitymark"];
			Vector2 position2 = uIImage4.GetPosition();
			position2.x = (float)num4 * 194f / 2f;
			uIImage4.SetPosition(position2);
			iSniperGameApp.GetInstance().m_GameState.m_fCurrentSensitivty = 27.5f;
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("soundswitchbtn") == control.Id)
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
		if (GetControlId("musicswitchbtn") == control.Id)
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
			iSniperMusicMgr.Instance().TurnMusic();
			FlurryPlugin.logEvent("MusicOn", iSniperGameApp.GetInstance().m_GameState.m_bMusicOn.ToString(), "bMusicOn");
		}
		if (GetControlId("prehelpbtn") == control.Id && command == 0)
		{
			m_help_index--;
			if (m_help_index <= 0)
			{
				m_help_index = 0;
				((UIImage)m_control_table["leftMark"]).Visible = false;
				((UIImage)m_control_table["rightMark"]).Visible = true;
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = false;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = true;
			}
			else
			{
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = true;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = true;
				((UIImage)m_control_table["leftMark"]).Visible = true;
				((UIImage)m_control_table["rightMark"]).Visible = true;
			}
			if (Utils.IsRetina())
			{
				((UIImage)m_control_table["helppanelbk1"]).SetTexture(new Rect(0f, (m_help_index + 1) * 256 * 2, 750f, 512f));
			}
			else
			{
				((UIImage)m_control_table["helppanelbk1"]).SetTexture(new Rect(0f, (m_help_index + 1) * 256, 375f, 256f));
			}
			PlaySound(m_SoundClick);
		}
		if (GetControlId("nexthelpbtn") == control.Id && command == 0)
		{
			m_help_index++;
			if (m_help_index >= 1)
			{
				m_help_index = 1;
				((UIImage)m_control_table["leftMark"]).Visible = true;
				((UIImage)m_control_table["rightMark"]).Visible = false;
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = true;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = false;
			}
			else
			{
				((UIImage)m_control_table["leftMark"]).Visible = true;
				((UIImage)m_control_table["rightMark"]).Visible = true;
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = true;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = true;
			}
			if (Utils.IsRetina())
			{
				((UIImage)m_control_table["helppanelbk1"]).SetTexture(new Rect(0f, (m_help_index + 1) * 256 * 2, 750f, 512f));
			}
			else
			{
				((UIImage)m_control_table["helppanelbk1"]).SetTexture(new Rect(0f, (m_help_index + 1) * 256, 375f, 256f));
			}
			PlaySound(m_SoundClick);
		}
		if (GetControlId("videobtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			Application.OpenURL("http://www.youtube.com/watch?v=eg_-esvG7Aw");
			Debug.Log("videobtn btn");
		}
		if (GetControlId("buyisniper1btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			return;
		}
		if (GetControlId("buyisniper2btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			return;
		}
		if (GetControlId("reviewbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			Application.OpenURL("http://www.trinitigame.com/support?game=isaw&version=1.0.8");
			Debug.Log("reviewbtn btn");
		}
		if (GetControlId("supportbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			if (iSniperAndroidIAP.m_platform == iSniperAndroidIAP.Platform.kAmazon)
			{
				Application.OpenURL("http://www.amazon.com/gp/mas/dl/android?p=com.trinitigame.android.isniper3d2");
			}
			else
			{
				Application.OpenURL("market://details?id=com.trinitigame.android.isniper3d2");
			}
			Debug.Log("supportbtn btn");
		}
		if (GetControlId("websitebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			Application.OpenURL("http://www.trinitigame.com/forum/viewforum.php?f=101");
			Debug.Log("websitebtn btn");
		}
		if (GetControlId("backbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			FadeOut();
		}
	}

	private void HideAllPanel()
	{
		StopAnimation("controlpanelslide");
		StopAnimation("soundpanelslide");
		StopAnimation("helppanelslide");
		StopAnimation("creditspanelslide");
		StopAnimation("historypanelslide");
		StopAnimation("extrapanelslide");
		HideControlPanel();
		HideSoundPanel();
		HideHelpPanel();
		HideCreditsPanel();
		HideHistoryPanel();
		HideExtraPanel();
	}

	private void HideControlPanel()
	{
		((UIImage)m_control_table["controlpanelbk"]).Visible = false;
		((UIPushButton)m_control_table["invertaxisbtn"]).Visible = false;
		((UIPushButton)m_control_table["invertaxisbtn"]).Enable = false;
		((UIPushButton)m_control_table["tiltjoystickbtn"]).Visible = false;
		((UIPushButton)m_control_table["tiltjoystickbtn"]).Enable = false;
		((UIImage)m_control_table["sensitivitymark"]).Visible = false;
		((UIMove)m_control_table["sensitivitymove"]).Enable = false;
		((UIClickButton)m_control_table["resetbtn"]).Enable = false;
		((UIImage)m_control_table["InvertY"]).Visible = false;
		((UIImage)m_control_table["InvertY1"]).Visible = false;
	}

	private void SlideControlPanel()
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		((UIImage)m_control_table["controlpanelbk"]).SetPosition(new Vector2(-152.5f * (float)num, 205f * (float)num));
		((UIPushButton)m_control_table["invertaxisbtn"]).Rect = new Rect(-255 * num, 195 * num, 100 * num, 20 * num);
		((UIPushButton)m_control_table["tiltjoystickbtn"]).Rect = new Rect(-255 * num, 228 * num, 100 * num, 20 * num);
		((UIImage)m_control_table["InvertY"]).Rect = new Rect(-124 * num, 193 * num, 100 * num, 20 * num);
		((UIImage)m_control_table["InvertY1"]).Rect = new Rect(-124 * num, 193 * num, 100 * num, 20 * num);
		float num2 = iSniperGameApp.GetInstance().m_GameState.m_fCurrentSensitivty - 5f;
		float num3 = 45f;
		float num4 = num2 / num3;
		float num5 = 56f + num4 * 82f - 305f;
		((UIImage)m_control_table["sensitivitymark"]).SetPosition(new Vector2(num5 * (float)num, 173.5f * (float)num));
		((UIImage)m_control_table["controlpanelbk"]).Visible = true;
		((UIPushButton)m_control_table["invertaxisbtn"]).Visible = true;
		((UIPushButton)m_control_table["invertaxisbtn"]).Enable = true;
		((UIPushButton)m_control_table["invertaxisbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bIsInvertYAixs);
		((UIPushButton)m_control_table["tiltjoystickbtn"]).Visible = true;
		((UIPushButton)m_control_table["tiltjoystickbtn"]).Enable = true;
		((UIPushButton)m_control_table["tiltjoystickbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl);
		((UIImage)m_control_table["sensitivitymark"]).Visible = true;
		((UIMove)m_control_table["sensitivitymove"]).Enable = false;
		((UIClickButton)m_control_table["resetbtn"]).Enable = false;
		if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
		{
			((UIImage)m_control_table["InvertY"]).Visible = false;
			((UIImage)m_control_table["InvertY1"]).Visible = true;
			((UIPushButton)m_control_table["invertaxisbtn"]).Enable = true;
		}
		else
		{
			((UIImage)m_control_table["InvertY"]).Visible = true;
			((UIImage)m_control_table["InvertY1"]).Visible = false;
			((UIPushButton)m_control_table["invertaxisbtn"]).Enable = false;
		}
		StartAnimation("controlpanelslide");
	}

	private void HideSoundPanel()
	{
		((UIImage)m_control_table["soundpanelbk"]).Visible = false;
		((UIPushButton)m_control_table["soundswitchbtn"]).Visible = false;
		((UIPushButton)m_control_table["soundswitchbtn"]).Enable = false;
		((UIPushButton)m_control_table["musicswitchbtn"]).Visible = false;
		((UIPushButton)m_control_table["musicswitchbtn"]).Enable = false;
	}

	private void SlideSoundPanel()
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		((UIImage)m_control_table["soundpanelbk"]).SetPosition(new Vector2(-152.5f * (float)num, 217.5f * (float)num));
		((UIPushButton)m_control_table["soundswitchbtn"]).Rect = new Rect(-255 * num, 228 * num, 100 * num, 20 * num);
		((UIPushButton)m_control_table["musicswitchbtn"]).Rect = new Rect(-255 * num, 195 * num, 100 * num, 20 * num);
		((UIImage)m_control_table["soundpanelbk"]).Visible = true;
		((UIPushButton)m_control_table["soundswitchbtn"]).Visible = true;
		((UIPushButton)m_control_table["soundswitchbtn"]).Enable = true;
		((UIPushButton)m_control_table["soundswitchbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bSoundOn);
		((UIPushButton)m_control_table["musicswitchbtn"]).Visible = true;
		((UIPushButton)m_control_table["musicswitchbtn"]).Enable = true;
		((UIPushButton)m_control_table["musicswitchbtn"]).Set(!iSniperGameApp.GetInstance().m_GameState.m_bMusicOn);
		StartAnimation("soundpanelslide");
	}

	private void HideHelpPanel()
	{
		((UIImage)m_control_table["helppanelbk1"]).Visible = false;
		((UIClickButton)m_control_table["prehelpbtn"]).Enable = false;
		((UIClickButton)m_control_table["nexthelpbtn"]).Enable = false;
		((UIImage)m_control_table["leftMark"]).Visible = false;
		((UIImage)m_control_table["rightMark"]).Visible = false;
	}

	private void SlideHelpPanel()
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		((UIImage)m_control_table["helppanelbk1"]).SetPosition(new Vector2(-187.5f * (float)num, 173 * num));
		((UIImage)m_control_table["helppanelbk1"]).Visible = true;
		((UIClickButton)m_control_table["prehelpbtn"]).Enable = false;
		((UIClickButton)m_control_table["nexthelpbtn"]).Enable = false;
		StartAnimation("helppanelslide");
	}

	private void HideCreditsPanel()
	{
		((UIImage)m_control_table["creditspanelbk"]).Visible = false;
		((UIImage)m_control_table["creditlist"]).Visible = false;
		((UINewMove)m_control_table["creditlistmove"]).Enable = false;
	}

	private void SlideCreditsPanel()
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		((UIImage)m_control_table["creditspanelbk"]).SetPosition(new Vector2(-152.5f * (float)num, 178.5f * (float)num));
		((UIImage)m_control_table["creditlist"]).SetPosition(new Vector2(-135.5f * (float)num, 175.5f * (float)num));
		((UINewMove)m_control_table["creditlistmove"]).Enable = true;
		((UIImage)m_control_table["creditspanelbk"]).Visible = true;
		((UIImage)m_control_table["creditlist"]).Visible = true;
		UIImage uIImage = (UIImage)m_control_table["creditlist"];
		if (Utils.IsRetina())
		{
			uIImage.Rect = new Rect(uIImage.Rect.x, 70f, uIImage.Rect.width, uIImage.Rect.height);
			uIImage.SetClip(new Rect(0f, 160f, 600f, 400f));
		}
		else
		{
			uIImage.Rect = new Rect(uIImage.Rect.x, 35f, uIImage.Rect.width, uIImage.Rect.height);
			uIImage.SetClip(new Rect(0f, 80f, 300f, 200f));
		}
		StartAnimation("creditspanelslide");
	}

	private void HideHistoryPanel()
	{
		((UIImage)m_control_table["historypanelbk1"]).Visible = false;
		((UIImage)m_control_table["historypanelbk2"]).Visible = false;
		((UIClickButton)m_control_table["buyisniper1btn"]).Enable = false;
		((UIClickButton)m_control_table["buyisniper2btn"]).Enable = false;
		((UIClickButton)m_control_table["videobtn"]).Enable = false;
	}

	private void SlideHistoryPanel()
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		((UIImage)m_control_table["historypanelbk1"]).SetPosition(new Vector2(-261.5f * (float)num, 193.5f * (float)num));
		((UIImage)m_control_table["historypanelbk2"]).SetPosition(new Vector2(-84f * (float)num, 188 * num));
		((UIImage)m_control_table["historypanelbk1"]).Visible = true;
		((UIImage)m_control_table["historypanelbk2"]).Visible = true;
		((UIClickButton)m_control_table["buyisniper1btn"]).Enable = false;
		((UIClickButton)m_control_table["buyisniper2btn"]).Enable = false;
		((UIClickButton)m_control_table["videobtn"]).Enable = false;
		StartAnimation("historypanelslide");
	}

	private void HideExtraPanel()
	{
		((UIImage)m_control_table["extrapanelbk"]).Visible = false;
		((UIClickButton)m_control_table["reviewbtn"]).Enable = false;
		((UIClickButton)m_control_table["supportbtn"]).Enable = false;
		((UIClickButton)m_control_table["websitebtn"]).Enable = false;
	}

	private void SlideExtraPanel()
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		((UIImage)m_control_table["extrapanelbk"]).SetPosition(new Vector2(-183f * (float)num, 194f * (float)num));
		((UIImage)m_control_table["extrapanelbk"]).Visible = true;
		((UIClickButton)m_control_table["reviewbtn"]).Enable = false;
		((UIClickButton)m_control_table["supportbtn"]).Enable = false;
		((UIClickButton)m_control_table["websitebtn"]).Enable = false;
		StartAnimation("extrapanelslide");
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
			Application.LoadLevel("iSniper.Menu");
		}
		else if ("controlpanelslide" == name)
		{
			((UIPushButton)m_control_table["invertaxisbtn"]).Enable = true;
			((UIPushButton)m_control_table["tiltjoystickbtn"]).Enable = true;
			((UIMove)m_control_table["sensitivitymove"]).Enable = true;
			((UIClickButton)m_control_table["resetbtn"]).Enable = true;
			if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				((UIImage)m_control_table["InvertY"]).Visible = false;
				((UIImage)m_control_table["InvertY1"]).Visible = true;
				((UIPushButton)m_control_table["invertaxisbtn"]).Enable = true;
			}
			else
			{
				((UIImage)m_control_table["InvertY"]).Visible = true;
				((UIImage)m_control_table["InvertY1"]).Visible = false;
				((UIPushButton)m_control_table["invertaxisbtn"]).Enable = false;
			}
		}
		else if ("soundpanelslide" == name)
		{
			((UIPushButton)m_control_table["soundswitchbtn"]).Enable = true;
			((UIPushButton)m_control_table["musicswitchbtn"]).Enable = true;
		}
		else if ("helppanelslide" == name)
		{
			((UIClickButton)m_control_table["prehelpbtn"]).Enable = false;
			((UIClickButton)m_control_table["nexthelpbtn"]).Enable = false;
			if (m_help_index == 0)
			{
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = false;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = true;
				((UIImage)m_control_table["leftMark"]).Visible = false;
				((UIImage)m_control_table["rightMark"]).Visible = true;
			}
			else if (m_help_index == 1)
			{
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = true;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = false;
				((UIImage)m_control_table["leftMark"]).Visible = true;
				((UIImage)m_control_table["rightMark"]).Visible = false;
			}
			else
			{
				((UIClickButton)m_control_table["prehelpbtn"]).Enable = true;
				((UIClickButton)m_control_table["nexthelpbtn"]).Enable = true;
				((UIImage)m_control_table["leftMark"]).Visible = true;
				((UIImage)m_control_table["rightMark"]).Visible = true;
			}
		}
		else if ("historypanelslide" == name)
		{
			((UIClickButton)m_control_table["buyisniper1btn"]).Enable = true;
			((UIClickButton)m_control_table["buyisniper2btn"]).Enable = true;
			((UIClickButton)m_control_table["videobtn"]).Enable = true;
		}
		else if ("extrapanelslide" == name)
		{
			((UIClickButton)m_control_table["reviewbtn"]).Enable = true;
			((UIClickButton)m_control_table["supportbtn"]).Enable = true;
			((UIClickButton)m_control_table["websitebtn"]).Enable = true;
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

	private void PlaySound(AudioSource audio)
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && null != audio)
		{
			audio.Play();
		}
	}
}
