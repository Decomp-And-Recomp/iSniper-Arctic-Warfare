using UnityEngine;

public class iSniperWeaponShopUIHelper : UIHelper
{
	private enum DLGSTATE
	{
		kBuyGun = 0,
		kBuyCash = 1,
		kUpdateDamage = 2,
		kUpdateSliencer = 3,
		kUpdateFirerate = 4,
		kUpdateClip = 5,
		kUpdateReload = 6,
		kUpdateZoom = 7
	}

	private const float QUATERTIME = 0.6f;

	private const float VIBRATIONTIME = 0.6f;

	private const int WEAPONSIZE = 410;

	private const int WEAPONNUM = 10;

	private bool m_bVibrating;

	private Vector2 m_weapon_pos;

	private DampedVibration m_Vibration;

	private float[] m_VibrationPoints;

	private int m_VibrationIndex;

	private float m_Amplitude;

	private float m_fVibrationTime;

	private int m_iHDSize = 1;

	private float m_move_begintime;

	private float m_move_delta;

	private Rect[] m_LeftMatRect;

	private Rect[] m_RightMatRect;

	private int m_MatIndex;

	private float m_ShiftTime;

	private int m_show_gun_index;

	private DLGSTATE m_DlgState;

	private AudioSource m_SoundClick;

	private AudioSource m_SoundWPMove;

	private AudioSource m_SoundEquip;

	private AudioSource m_SoundBack;

	private AudioSource m_SoundUpdate;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/WeaponShopUI";
		base.Start();
		StartAnimation("buttondynmaicbk");
		if (Utils.IsRetina())
		{
			m_iHDSize = 2;
		}
		m_weapon_pos = ((UIImage)m_control_table["weapon1"]).GetPosition();
		m_VibrationPoints = new float[10];
		for (int i = 1; i <= 10; i++)
		{
			m_VibrationPoints[i - 1] = m_weapon_pos.x - (float)((i - 1) * (410 * m_iHDSize));
		}
		string strPlayerUserGunName = iSniperGameApp.GetInstance().m_GameState.m_strPlayerUserGunName;
		m_VibrationIndex = iSniperGameApp.GetInstance().m_GunCenter.GetGunIndex(strPlayerUserGunName);
		m_weapon_pos.x = m_VibrationPoints[m_VibrationIndex];
		for (int j = 1; j <= 10; j++)
		{
			string key = "weapon" + j;
			Vector2 weapon_pos = m_weapon_pos;
			weapon_pos.x += (j - 1) * (410 * m_iHDSize);
			((UIImage)m_control_table[key]).SetPosition(weapon_pos);
		}
		m_Vibration = new DampedVibration();
		m_bVibrating = false;
		m_MatIndex = 0;
		m_ShiftTime = 0f;
		m_LeftMatRect = new Rect[3];
		m_LeftMatRect[0] = new Rect(14 * m_iHDSize, 385 * m_iHDSize, 30 * m_iHDSize, 11 * m_iHDSize);
		m_LeftMatRect[1] = new Rect(14 * m_iHDSize, 405 * m_iHDSize, 30 * m_iHDSize, 11 * m_iHDSize);
		m_LeftMatRect[2] = new Rect(14 * m_iHDSize, 425 * m_iHDSize, 30 * m_iHDSize, 11 * m_iHDSize);
		m_RightMatRect = new Rect[3];
		m_RightMatRect[0] = new Rect(87 * m_iHDSize, 385 * m_iHDSize, 30 * m_iHDSize, 11 * m_iHDSize);
		m_RightMatRect[1] = new Rect(87 * m_iHDSize, 405 * m_iHDSize, 30 * m_iHDSize, 11 * m_iHDSize);
		m_RightMatRect[2] = new Rect(87 * m_iHDSize, 425 * m_iHDSize, 30 * m_iHDSize, 11 * m_iHDSize);
		OnSelectWeapon(m_VibrationIndex);
		ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
		FadeIn();
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundWPMove = GetAudioSource("Main Camera/SoundWPMove");
		m_SoundEquip = GetAudioSource("Main Camera/SoundEquip");
		m_SoundBack = GetAudioSource("Main Camera/SoundBack");
		m_SoundUpdate = GetAudioSource("Main Camera/SoundUpdate");
		FlurryPlugin.logEvent("EnterWeaponShop");
	}

	public void Update()
	{
		if (m_bVibrating)
		{
			m_fVibrationTime += Time.deltaTime;
			if (m_fVibrationTime >= 0.6f)
			{
				m_Amplitude = 0f;
				m_fVibrationTime = 0.6f;
				m_bVibrating = false;
				OnSelectWeapon(m_VibrationIndex);
			}
			float num = m_Vibration.CalculateDistance(m_fVibrationTime);
			for (int i = 1; i <= 10; i++)
			{
				string key = "weapon" + i;
				Vector2 weapon_pos = m_weapon_pos;
				weapon_pos.x += num + (float)((i - 1) * (410 * m_iHDSize));
				((UIImage)m_control_table[key]).SetPosition(weapon_pos);
			}
		}
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
		if (GetControlId("weaponmove") == control.Id)
		{
			switch (command)
			{
			case 0:
				m_move_begintime = Time.time;
				m_move_delta = 0f;
				m_bVibrating = false;
				ShowEquipMark(false);
				ShowBuyButton(false);
				ShowEquipBtn(false);
				ShowLockMark(false);
				HideAllGunStar();
				break;
			case 1:
			{
				m_weapon_pos.x += wparam;
				m_move_delta += wparam;
				for (int j = 1; j <= 10; j++)
				{
					string key = "weapon" + j;
					Vector2 position = ((UIImage)m_control_table[key]).GetPosition();
					position.x += wparam;
					((UIImage)m_control_table[key]).SetPosition(position);
				}
				m_bVibrating = false;
				break;
			}
			case 2:
			{
				PlaySound(m_SoundWPMove);
				int i = 0;
				float num = Time.time - m_move_begintime;
				float num2 = ((!(num > 0f)) ? 0f : (m_move_delta / num));
				if (Mathf.Abs(num2) > 10f)
				{
					i = m_VibrationIndex;
					i = ((!(num2 > 0f)) ? (i + 1) : (i - 1));
					if (i < 0)
					{
						i = 0;
					}
					else if (i > m_VibrationPoints.Length - 1)
					{
						i = m_VibrationPoints.Length - 1;
					}
				}
				else if (m_weapon_pos.x >= m_VibrationPoints[0])
				{
					i = 0;
				}
				else if (m_weapon_pos.x <= m_VibrationPoints[m_VibrationPoints.Length - 1])
				{
					i = m_VibrationPoints.Length - 1;
				}
				else
				{
					for (; i < m_VibrationPoints.Length && !(Mathf.Abs(m_weapon_pos.x - m_VibrationPoints[i]) <= (float)(410 * m_iHDSize / 2)); i++)
					{
					}
				}
				m_VibrationIndex = i;
				m_Amplitude = m_weapon_pos.x - m_VibrationPoints[i];
				m_weapon_pos.x = m_VibrationPoints[i];
				m_Vibration.SetParameter(m_Amplitude, 3f, 2.6179938f, 0f);
				m_fVibrationTime = 0f;
				m_bVibrating = true;
				break;
			}
			}
		}
		if (GetControlId("leftmarkbutton") == control.Id && command == 0)
		{
			if (m_VibrationIndex == 0)
			{
				return;
			}
			PlaySound(m_SoundWPMove);
			m_VibrationIndex--;
			m_weapon_pos.x += 410 * m_iHDSize / 2;
			m_Amplitude = m_weapon_pos.x - m_VibrationPoints[m_VibrationIndex];
			m_weapon_pos.x = m_VibrationPoints[m_VibrationIndex];
			m_Vibration.SetParameter(m_Amplitude, 3f, 2.6179938f, 0f);
			m_fVibrationTime = 0f;
			m_bVibrating = true;
			((UIClickButton)m_control_table["leftmarkbutton"]).Enable = false;
			((UIClickButton)m_control_table["rightmarkbutton"]).Enable = false;
			ShowEquipMark(false);
			ShowBuyButton(false);
			ShowEquipBtn(false);
			HideAllGunStar();
		}
		if (GetControlId("rightmarkbutton") == control.Id && command == 0)
		{
			if (m_VibrationIndex == m_VibrationPoints.Length - 1)
			{
				return;
			}
			PlaySound(m_SoundWPMove);
			m_VibrationIndex++;
			m_weapon_pos.x -= 410 * m_iHDSize / 2;
			m_Amplitude = m_weapon_pos.x - m_VibrationPoints[m_VibrationIndex];
			m_weapon_pos.x = m_VibrationPoints[m_VibrationIndex];
			m_Vibration.SetParameter(m_Amplitude, 3f, 2.6179938f, 0f);
			m_fVibrationTime = 0f;
			m_bVibrating = true;
			((UIClickButton)m_control_table["leftmarkbutton"]).Enable = false;
			((UIClickButton)m_control_table["rightmarkbutton"]).Enable = false;
			ShowEquipMark(false);
			ShowBuyButton(false);
			ShowEquipBtn(false);
			HideAllGunStar();
		}
		if (GetControlId("buybutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty.m_iPrice)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kBuyGun;
				ShowYesNoDialog("Would you like to buy " + gunProperty.m_strName + "\nfor " + gunProperty.m_iPrice + "?");
			}
		}
		if (GetControlId("equipbutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundEquip);
			iSniperGunProperty gunProperty2 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			iSniperGameApp.GetInstance().m_GameState.m_strPlayerUserGunName = gunProperty2.m_strName;
			FlurryPlugin.logEvent("PlayerCurrentUserGun", iSniperGameApp.GetInstance().m_GameState.m_strPlayerUserGunName, "GunName");
			ShowEquipMark(true);
			ShowEquipBtn(false);
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("backbutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			FadeOut();
		}
		if (GetControlId("damageupdatebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty3 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty3.m_iHarmCash)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kUpdateDamage;
				ShowYesNoDialog("Would you like to upgrade\n" + gunProperty3.m_strName + " for " + gunProperty3.m_iHarmCash + "?");
			}
		}
		if (GetControlId("sliencerupdatebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty4 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty4.m_iSliencerCash)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kUpdateSliencer;
				ShowYesNoDialog("Would you like to upgrade\n" + gunProperty4.m_strName + " for " + gunProperty4.m_iSliencerCash + "?");
			}
		}
		if (GetControlId("firerateupdatebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty5 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty5.m_iFireCash)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kUpdateFirerate;
				ShowYesNoDialog("Would you like to upgrade\n" + gunProperty5.m_strName + " for " + gunProperty5.m_iFireCash + "?");
			}
		}
		if (GetControlId("clipupdatebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty6 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty6.m_iClipCash)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kUpdateClip;
				ShowYesNoDialog("Would you like to upgrade\n" + gunProperty6.m_strName + " for " + gunProperty6.m_iClipCash + "?");
			}
		}
		if (GetControlId("reloadupdatebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty7 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty7.m_iReloadCash)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kUpdateReload;
				ShowYesNoDialog("Would you like to upgrade\n" + gunProperty7.m_strName + " for " + gunProperty7.m_iReloadCash + "?");
			}
		}
		if (GetControlId("zoomupdatebtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			iSniperGunProperty gunProperty8 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
			if (iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash < gunProperty8.m_iZoomCash)
			{
				m_DlgState = DLGSTATE.kBuyCash;
				ShowYesNoDialog("You don't have enough credits! Would you like to buy some more?");
			}
			else
			{
				m_DlgState = DLGSTATE.kUpdateZoom;
				ShowYesNoDialog("Would you like to upgrade\n" + gunProperty8.m_strName + " for " + gunProperty8.m_iZoomCash + "?");
			}
		}
		if (GetControlId("getcashbutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			IAPFadeOut();
		}
		if (GetControlId("yesbutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			if (m_DlgState == DLGSTATE.kBuyGun)
			{
				iSniperGunProperty gunProperty9 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty9.m_iState = 1;
				iSniperGameApp.GetInstance().m_GameState.m_strPlayerUserGunName = gunProperty9.m_strName;
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty9.m_iPrice;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				ShowEquipMark(true);
				ShowBuyButton(false);
				OnSelectWeapon(m_show_gun_index);
				FlurryPlugin.logEvent("BuyGun", gunProperty9.m_strName, "gunName");
				FlurryPlugin.logEvent("ExpendMoney", gunProperty9.m_iPrice.ToString(), "Expend_BuyGun");
			}
			else if (m_DlgState == DLGSTATE.kBuyCash)
			{
				PlaySound(m_SoundClick);
				IAPFadeOut();
			}
			else if (m_DlgState == DLGSTATE.kUpdateDamage)
			{
				iSniperGunProperty gunProperty10 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty10.m_iHarmCurrentUpdateNum++;
				ShowUpGunStar();
				OnSelectWeapon(m_show_gun_index);
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty10.m_iHarmCash;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				PlaySound(m_SoundUpdate);
				FlurryPlugin.logEvent("UpdateDamage", gunProperty10.GetCurrnentHarmStar().ToString(), gunProperty10.m_strName);
				FlurryPlugin.logEvent("ExpendMoney", gunProperty10.m_iHarmCash.ToString(), "Expend_UpWeapon");
			}
			else if (m_DlgState == DLGSTATE.kUpdateSliencer)
			{
				iSniperGunProperty gunProperty11 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty11.m_iSliencerCurrentUpdateNum++;
				OnSelectWeapon(m_show_gun_index);
				ShowUpGunStar();
				PlaySound(m_SoundUpdate);
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty11.m_iSliencerCash;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				FlurryPlugin.logEvent("UpdateSliencer", gunProperty11.GetCurrentSliencerStar().ToString(), gunProperty11.m_strName);
				FlurryPlugin.logEvent("ExpendMoney", gunProperty11.m_iSliencerCash.ToString(), "Expend_UpWeapon");
			}
			else if (m_DlgState == DLGSTATE.kUpdateFirerate)
			{
				iSniperGunProperty gunProperty12 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty12.m_iFireCurrentUpdateNum++;
				OnSelectWeapon(m_show_gun_index);
				ShowUpGunStar();
				PlaySound(m_SoundUpdate);
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty12.m_iFireCash;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				FlurryPlugin.logEvent("UpdateFirerate", gunProperty12.GetCurrentFireStar().ToString(), gunProperty12.m_strName);
				FlurryPlugin.logEvent("ExpendMoney", gunProperty12.m_iFireCash.ToString(), "Expend_UpWeapon");
			}
			else if (m_DlgState == DLGSTATE.kUpdateClip)
			{
				iSniperGunProperty gunProperty13 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty13.m_iClipCurrentUpdateNum++;
				OnSelectWeapon(m_show_gun_index);
				ShowUpGunStar();
				PlaySound(m_SoundUpdate);
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty13.m_iClipCash;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				FlurryPlugin.logEvent("UpdateClip", gunProperty13.GetCurrentClipStar().ToString(), gunProperty13.m_strName);
				FlurryPlugin.logEvent("ExpendMoney", gunProperty13.m_iClipCash.ToString(), "Expend_UpWeapon");
			}
			else if (m_DlgState == DLGSTATE.kUpdateReload)
			{
				iSniperGunProperty gunProperty14 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty14.m_iReloadCurrentUpdateNum++;
				OnSelectWeapon(m_show_gun_index);
				ShowUpGunStar();
				PlaySound(m_SoundUpdate);
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty14.m_iReloadCash;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				FlurryPlugin.logEvent("UpdateReload", gunProperty14.GetCurrentReloadStar().ToString(), gunProperty14.m_strName);
				FlurryPlugin.logEvent("ExpendMoney", gunProperty14.m_iReloadCash.ToString(), "Expend_UpWeapon");
			}
			else if (m_DlgState == DLGSTATE.kUpdateZoom)
			{
				iSniperGunProperty gunProperty15 = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_show_gun_index);
				gunProperty15.m_iZoomCurrentUpdateNum++;
				OnSelectWeapon(m_show_gun_index);
				ShowUpGunStar();
				PlaySound(m_SoundUpdate);
				iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash -= gunProperty15.m_iZoomCash;
				ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
				FlurryPlugin.logEvent("UpdateZoom", gunProperty15.GetCurrentZoomStar().ToString(), gunProperty15.m_strName);
				FlurryPlugin.logEvent("ExpendMoney", gunProperty15.m_iZoomCash.ToString(), "Expend_UpWeapon");
			}
			HideYesNoDialog();
			if (iSniperGameApp.GetInstance().m_GameState.m_iAchFever == 0)
			{
				bool flag = true;
				for (int k = 0; k < 10; k++)
				{
					if (!iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(k).IsBuy())
					{
						flag = false;
						break;
					}
					if (!iSniperGameApp.GetInstance().m_GunCenter.GetGunAllStar(k))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					iSniperGameApp.GetInstance().m_GameState.m_iAchFever = 1;
					Debug.Log("m_iAchFever");
				}
			}
			if (iSniperGameApp.GetInstance().m_GameState.m_iAchGunsmith == 0)
			{
				for (int l = 0; l < 8; l++)
				{
					if (iSniperGameApp.GetInstance().m_GunCenter.GetGunAllStar(l))
					{
						iSniperGameApp.GetInstance().m_GameState.m_iAchGunsmith = 1;
						Debug.Log("m_iAchGunsmith");
						break;
					}
				}
			}
			iSniperGameApp.GetInstance().m_GunCenter.SaveUserGunCfg();
			iSniperGameApp.GetInstance().m_GameState.SaveData();
		}
		if (GetControlId("nobutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			HideYesNoDialog();
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
			switch (iSniperGameApp.GetInstance().m_GameState.m_LastScene)
			{
			case iSniperGameState.LastScene.kMenu:
				Application.LoadLevel("iSniper.Menu");
				break;
			case iSniperGameState.LastScene.kStoryStart:
				Application.LoadLevel("iSniper.StoryStart");
				break;
			}
		}
		else if ("iapfadeout" == name)
		{
			Application.LoadLevel("iSniper.IAP");
		}
		else if ("statup" == name)
		{
			UIImage uIImage2 = (UIImage)m_control_table["starup"];
			uIImage2.Visible = false;
			uIImage2.SetScale(1f);
		}
	}

	public void ShowPlayerCash(int num)
	{
		((UIText)m_control_table["cashnumber"]).SetText(num.ToString());
	}

	public void ShowWeaponLeftMark(bool val)
	{
		((UIImage)m_control_table["leftMark"]).Visible = val;
		((UIClickButton)m_control_table["leftmarkbutton"]).Enable = val;
	}

	public void ShowWeaponRightMark(bool val)
	{
		((UIImage)m_control_table["rightMark"]).Visible = val;
		((UIClickButton)m_control_table["rightmarkbutton"]).Enable = val;
	}

	public void ShowEquipMark(bool val)
	{
		((UIImage)m_control_table["equiptext"]).Visible = val;
		((UIImage)m_control_table["equipmark"]).Visible = val;
	}

	public void ShowLockMark(bool val)
	{
		((UIImage)m_control_table["locktext"]).Visible = val;
	}

	public void ShowEquipBtn(bool val)
	{
		((UIClickButton)m_control_table["equipbutton"]).Visible = val;
		((UIClickButton)m_control_table["equipbutton"]).Enable = val;
	}

	public void ShowBuyButton(bool val)
	{
		((UIClickButton)m_control_table["buybutton"]).Visible = val;
		((UIClickButton)m_control_table["buybutton"]).Enable = val;
	}

	public void OnSelectWeapon(int index)
	{
		m_show_gun_index = index;
		switch (index)
		{
		case 0:
			ShowWeaponLeftMark(false);
			ShowWeaponRightMark(true);
			break;
		case 9:
			ShowWeaponLeftMark(true);
			ShowWeaponRightMark(false);
			break;
		default:
			ShowWeaponLeftMark(true);
			ShowWeaponRightMark(true);
			break;
		}
		iSniperGunProperty gunProperty = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(index);
		string strPlayerUserGunName = iSniperGameApp.GetInstance().m_GameState.m_strPlayerUserGunName;
		int gunIndex = iSniperGameApp.GetInstance().m_GunCenter.GetGunIndex(strPlayerUserGunName);
		if (index == gunIndex)
		{
			ShowEquipMark(true);
			ShowBuyButton(false);
			ShowEquipBtn(false);
			ShowLockMark(false);
		}
		else if (gunProperty.IsBuy())
		{
			ShowEquipMark(false);
			ShowBuyButton(false);
			ShowEquipBtn(true);
			ShowLockMark(false);
		}
		else if (gunProperty.IsLock())
		{
			ShowEquipMark(false);
			ShowBuyButton(false);
			ShowEquipBtn(false);
			ShowLockMark(true);
		}
		else
		{
			ShowEquipMark(false);
			ShowBuyButton(true);
			ShowEquipBtn(false);
			ShowLockMark(false);
		}
		ShowGunStar(index);
	}

	public void ShowUpGunStar()
	{
		iSniperGunProperty gunProperty = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(m_VibrationIndex);
		UIImage uIImage = (UIImage)m_control_table["starup"];
		uIImage.Visible = true;
		string key = null;
		if (m_DlgState == DLGSTATE.kUpdateDamage)
		{
			key = "damagestarbk" + gunProperty.GetCurrnentHarmStar();
		}
		else if (m_DlgState == DLGSTATE.kUpdateSliencer)
		{
			key = "sliencerstarbk" + gunProperty.GetCurrentSliencerStar();
		}
		else if (m_DlgState == DLGSTATE.kUpdateClip)
		{
			key = "clipstarbk" + gunProperty.GetCurrentClipStar();
		}
		else if (m_DlgState == DLGSTATE.kUpdateFirerate)
		{
			key = "fireratestarbk" + gunProperty.GetCurrentFireStar();
		}
		else if (m_DlgState == DLGSTATE.kUpdateZoom)
		{
			key = "zoomstarbk" + gunProperty.GetCurrentZoomStar();
		}
		else if (m_DlgState == DLGSTATE.kUpdateReload)
		{
			key = "reloadstarbk" + gunProperty.GetCurrentReloadStar();
		}
		UIImage uIImage2 = (UIImage)m_control_table[key];
		Debug.Log(uIImage2.GetPosition());
		Debug.Log(uIImage2.Rect);
		float x = uIImage2.GetPosition().x - (uIImage.Rect.width - uIImage2.Rect.width) / 8f;
		float y = uIImage2.GetPosition().y + (uIImage.Rect.height - uIImage2.Rect.height) / 8f;
		Vector2 vector = new Vector2(x, y);
		Debug.Log(vector);
		uIImage.SetPosition(vector);
		StartAnimation("statup");
	}

	public void ShowGunStar(int index)
	{
		iSniperGunProperty gunProperty = iSniperGameApp.GetInstance().m_GunCenter.GetGunProperty(index);
		((UIText)m_control_table["weaponname"]).SetText(gunProperty.m_strName);
		((UIText)m_control_table["weaponpricetext"]).SetText(gunProperty.m_iPrice.ToString());
		int maxHarmStar = gunProperty.GetMaxHarmStar();
		int currnentHarmStar = gunProperty.GetCurrnentHarmStar();
		int maxSliencerStar = gunProperty.GetMaxSliencerStar();
		int currentSliencerStar = gunProperty.GetCurrentSliencerStar();
		int maxFireStar = gunProperty.GetMaxFireStar();
		int currentFireStar = gunProperty.GetCurrentFireStar();
		int maxClipStar = gunProperty.GetMaxClipStar();
		int currentClipStar = gunProperty.GetCurrentClipStar();
		int maxReloadStar = gunProperty.GetMaxReloadStar();
		int currentReloadStar = gunProperty.GetCurrentReloadStar();
		int maxZoomStar = gunProperty.GetMaxZoomStar();
		int currentZoomStar = gunProperty.GetCurrentZoomStar();
		if (gunProperty.IsBuy() && currnentHarmStar < maxHarmStar)
		{
			((UIClickButton)m_control_table["damageupdatebtn"]).Enable = true;
			((UIClickButton)m_control_table["damageupdatebtn"]).Visible = true;
			((UIText)m_control_table["damageupdatecashtext"]).SetText(gunProperty.m_iHarmCash.ToString());
		}
		else
		{
			((UIClickButton)m_control_table["damageupdatebtn"]).Enable = false;
			((UIClickButton)m_control_table["damageupdatebtn"]).Visible = false;
			((UIText)m_control_table["damageupdatecashtext"]).SetText(string.Empty);
		}
		if (gunProperty.IsBuy() && currentSliencerStar < maxSliencerStar)
		{
			((UIClickButton)m_control_table["sliencerupdatebtn"]).Enable = true;
			((UIClickButton)m_control_table["sliencerupdatebtn"]).Visible = true;
			((UIText)m_control_table["sliencerupdatecashtext"]).SetText(gunProperty.m_iSliencerCash.ToString());
		}
		else
		{
			((UIClickButton)m_control_table["sliencerupdatebtn"]).Enable = false;
			((UIClickButton)m_control_table["sliencerupdatebtn"]).Visible = false;
			((UIText)m_control_table["sliencerupdatecashtext"]).SetText(string.Empty);
		}
		if (gunProperty.IsBuy() && currentFireStar < maxFireStar)
		{
			((UIClickButton)m_control_table["firerateupdatebtn"]).Enable = true;
			((UIClickButton)m_control_table["firerateupdatebtn"]).Visible = true;
			((UIText)m_control_table["firerateupdatecashtext"]).SetText(gunProperty.m_iFireCash.ToString());
		}
		else
		{
			((UIClickButton)m_control_table["firerateupdatebtn"]).Enable = false;
			((UIClickButton)m_control_table["firerateupdatebtn"]).Visible = false;
			((UIText)m_control_table["firerateupdatecashtext"]).SetText(string.Empty);
		}
		if (gunProperty.IsBuy() && currentClipStar < maxClipStar)
		{
			((UIClickButton)m_control_table["clipupdatebtn"]).Enable = true;
			((UIClickButton)m_control_table["clipupdatebtn"]).Visible = true;
			((UIText)m_control_table["clipupdatecashtext"]).SetText(gunProperty.m_iClipCash.ToString());
		}
		else
		{
			((UIClickButton)m_control_table["clipupdatebtn"]).Enable = false;
			((UIClickButton)m_control_table["clipupdatebtn"]).Visible = false;
			((UIText)m_control_table["clipupdatecashtext"]).SetText(string.Empty);
		}
		if (gunProperty.IsBuy() && currentReloadStar < maxReloadStar)
		{
			((UIClickButton)m_control_table["reloadupdatebtn"]).Enable = true;
			((UIClickButton)m_control_table["reloadupdatebtn"]).Visible = true;
			((UIText)m_control_table["reloadupdatecashtext"]).SetText(gunProperty.m_iReloadCash.ToString());
		}
		else
		{
			((UIClickButton)m_control_table["reloadupdatebtn"]).Enable = false;
			((UIClickButton)m_control_table["reloadupdatebtn"]).Visible = false;
			((UIText)m_control_table["reloadupdatecashtext"]).SetText(string.Empty);
		}
		if (gunProperty.IsBuy() && currentZoomStar < maxZoomStar)
		{
			((UIClickButton)m_control_table["zoomupdatebtn"]).Enable = true;
			((UIClickButton)m_control_table["zoomupdatebtn"]).Visible = true;
			((UIText)m_control_table["zoomupdatecashtext"]).SetText(gunProperty.m_iZoomCash.ToString());
		}
		else
		{
			((UIClickButton)m_control_table["zoomupdatebtn"]).Enable = false;
			((UIClickButton)m_control_table["zoomupdatebtn"]).Visible = false;
			((UIText)m_control_table["zoomupdatecashtext"]).SetText(string.Empty);
		}
		for (int i = 1; i <= 10; i++)
		{
			string key = "damagestarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= maxHarmStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "damagestar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= currnentHarmStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "sliencerstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= maxSliencerStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "sliencerstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= currentSliencerStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "fireratestarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= maxFireStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "fireratestar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= currentFireStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "clipstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= maxClipStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "clipstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= currentClipStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "reloadstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= maxReloadStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "reloadstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= currentReloadStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "zoomstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= maxZoomStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
			key = "zoomstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			if (i <= currentZoomStar)
			{
				((UIImage)m_control_table[key]).Visible = true;
			}
		}
	}

	public void HideAllGunStar()
	{
		for (int i = 1; i <= 10; i++)
		{
			string key = "damagestar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "damagestarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "sliencerstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "sliencerstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "fireratestar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "fireratestarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "clipstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "clipstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "reloadstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "reloadstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "zoomstar" + i;
			((UIImage)m_control_table[key]).Visible = false;
			key = "zoomstarbk" + i;
			((UIImage)m_control_table[key]).Visible = false;
		}
		((UIClickButton)m_control_table["damageupdatebtn"]).Enable = false;
		((UIClickButton)m_control_table["damageupdatebtn"]).Visible = false;
		((UIClickButton)m_control_table["sliencerupdatebtn"]).Enable = false;
		((UIClickButton)m_control_table["sliencerupdatebtn"]).Visible = false;
		((UIClickButton)m_control_table["firerateupdatebtn"]).Enable = false;
		((UIClickButton)m_control_table["firerateupdatebtn"]).Visible = false;
		((UIClickButton)m_control_table["clipupdatebtn"]).Enable = false;
		((UIClickButton)m_control_table["clipupdatebtn"]).Visible = false;
		((UIClickButton)m_control_table["reloadupdatebtn"]).Enable = false;
		((UIClickButton)m_control_table["reloadupdatebtn"]).Visible = false;
		((UIClickButton)m_control_table["zoomupdatebtn"]).Enable = false;
		((UIClickButton)m_control_table["zoomupdatebtn"]).Visible = false;
		((UIText)m_control_table["damageupdatecashtext"]).SetText(string.Empty);
		((UIText)m_control_table["sliencerupdatecashtext"]).SetText(string.Empty);
		((UIText)m_control_table["firerateupdatecashtext"]).SetText(string.Empty);
		((UIText)m_control_table["clipupdatecashtext"]).SetText(string.Empty);
		((UIText)m_control_table["reloadupdatecashtext"]).SetText(string.Empty);
		((UIText)m_control_table["zoomupdatecashtext"]).SetText(string.Empty);
		((UIText)m_control_table["weaponpricetext"]).SetText(string.Empty);
		((UIText)m_control_table["weaponname"]).SetText(string.Empty);
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

	public void IAPFadeOut()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIImage)m_control_table["fadeimage"]).SetAlpha(0f);
		StartAnimation("iapfadeout");
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
	}

	public void HideOkDialog()
	{
		((UIImage)m_control_table["dialogblock"]).Enable = false;
		((UIImage)m_control_table["dialogblock"]).Visible = false;
		((UIImage)m_control_table["dialogokbk"]).Enable = false;
		((UIImage)m_control_table["dialogokbk"]).Visible = false;
		((UIText)m_control_table["dialogtext"]).Visible = false;
	}

	public void PlaySound(AudioSource audio)
	{
		if (iSniperGameApp.GetInstance().m_GameState.m_bSoundOn && null != audio)
		{
			audio.Play();
		}
	}
}
