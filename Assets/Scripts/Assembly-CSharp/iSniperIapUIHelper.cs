using UnityEngine;

public class iSniperIapUIHelper : UIHelper
{
	public enum ProdcutID
	{
		NO0 = 0,
		NO1 = 1,
		NO2 = 2,
		NO3 = 3,
		NO4 = 4,
		NO5 = 5,
		NO6 = 6
	}

	private AudioSource m_SoundClick;

	private AudioSource m_SoundBack;

	public iSniperGameState m_GameState;

	private ProdcutID m_BuyProductID;

	private bool m_bIsBuying;

	private new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/IapUI";
		m_SoundClick = GetAudioSource("Main Camera/SoundClick");
		m_SoundBack = GetAudioSource("Main Camera/SoundBack");
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		m_BuyProductID = ProdcutID.NO0;
		m_bIsBuying = false;
		base.Start();
		ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
		FadeIn();
		FlurryPlugin.logEvent("EnterIAP");
		if (!iSniperAndroidIAP.m_bIsSupport)
		{
			iSniperAndroidIAP.Initialize();
		}
	}

	private void Update()
	{
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		bool flag = false;
		string sku = string.Empty;
		if (GetControlId("backbtn") == control.Id && command == 0)
		{
			PlaySound(m_SoundBack);
			FadeOut();
		}
		if (GetControlId("buy1btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			sku = "com.trinitigame.isniper3darcticwarfare.099cents";
			flag = true;
		}
		if (GetControlId("buy2btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			sku = "com.trinitigame.isniper3darcticwarfare.199cents";
			flag = true;
		}
		if (GetControlId("buy3btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			sku = "com.trinitigame.isniper3darcticwarfare.299cents";
			flag = true;
		}
		if (GetControlId("buy4btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			sku = "com.trinitigame.isniper3darcticwarfare.499cents";
			flag = true;
		}
		if (GetControlId("buy5btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			sku = "com.trinitigame.isniper3darcticwarfare.999cents";
			flag = true;
		}
		if (GetControlId("buy6btn") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			sku = "com.trinitigame.isniper3darcticwarfare.1999cents";
			flag = true;
		}
		if (GetControlId("okbutton") == control.Id && command == 0)
		{
			PlaySound(m_SoundClick);
			HideOkDialog();
		}
		if (flag)
		{
			iSniperAndroidIAP.BuyStartFunc = ShowConnect;
			iSniperAndroidIAP.CompleteFunc = SuccessEvent;
			iSniperAndroidIAP.FailureFunc = FailureEvent;
			iSniperAndroidIAP.BuyIap(sku);
		}
	}

	private void SuccessEvent()
	{
		HideConnect();
		ShowPlayerCash(iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash);
	}

	private void FailureEvent()
	{
		HideConnect();
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			ShowOkDialog("Connection timed out.");
		}
	}

	public void ShowPlayerCash(int num)
	{
		((UIText)m_control_table["cashnumber"]).SetText(num.ToString());
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
			Application.LoadLevel("iSniper.WeaponShop");
		}
	}

	public void ShowConnect()
	{
		((UIImage)m_control_table["iapblock"]).Enable = true;
		((UIImage)m_control_table["iapblock"]).Visible = true;
		((UIImage)m_control_table["connect"]).Enable = true;
		((UIImage)m_control_table["connect"]).Visible = true;
	}

	public void HideConnect()
	{
		((UIImage)m_control_table["iapblock"]).Enable = false;
		((UIImage)m_control_table["iapblock"]).Visible = false;
		((UIImage)m_control_table["connect"]).Enable = false;
		((UIImage)m_control_table["connect"]).Visible = false;
	}

	public bool IsConnectShow()
	{
		return ((UIImage)m_control_table["iapblock"]).Visible;
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

	public void PlaySound(AudioSource audio)
	{
		if (!(null == audio) && iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			audio.Play();
		}
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
}
