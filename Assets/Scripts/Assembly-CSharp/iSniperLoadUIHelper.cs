using UnityEngine;

public class iSniperLoadUIHelper : UIHelper
{
	private enum State
	{
		kFadeIn = 0,
		kProgress = 1,
		kFadeOut = 2
	}

	private float m_width;

	private State m_state;

	private float m_fSpeed;

	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/LoadUI";
		OpenClikPlugin.Show(true);
		base.Start();
		int num = 1;
		if (Utils.IsRetina())
		{
			num = 2;
		}
		((UIImage)m_control_table["progressor"]).SetClip(new Rect(106 * num, 46 * num, m_width * (float)num, 10 * num));
		FadeIn();
		m_state = State.kFadeIn;
		m_fSpeed = 100f;
		iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
		if (gameState.m_iLoginTimes < 3)
		{
			gameState.m_iLoginTimes++;
			gameState.SaveData();
		}
		FlurryPlugin.logEvent("IAPCrack", MiscPlugin.CheckOSIsIAPCrack().ToString(), "bIsIapCrack");
		FlurryPlugin.logEvent("OSCrack", MiscPlugin.CheckOSIsJailbreak().ToString(), "bOSIsbreak");
		FlurryPlugin.logEvent("MACAddress", MiscPlugin.GetMacAddr(), "MacAddr");
	}

	public void Update()
	{
		switch (m_state)
		{
		case State.kFadeIn:
			break;
		case State.kProgress:
		{
			m_width += Time.deltaTime * m_fSpeed;
			int num = 1;
			if (Utils.IsRetina())
			{
				num = 2;
			}
			((UIImage)m_control_table["progressor"]).SetClip(new Rect(106 * num, 46 * num, m_width * (float)num, 10 * num));
			if (m_width > 273f)
			{
				m_state = State.kFadeOut;
				FadeOut();
			}
			break;
		}
		case State.kFadeOut:
			break;
		}
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public override void OnAnimationFinished(string name)
	{
		if ("fadein" == name)
		{
			UIImage uIImage = (UIImage)m_control_table["fadeimage"];
			uIImage.Visible = false;
			uIImage.Enable = false;
			m_state = State.kProgress;
		}
		else if ("fadeout" == name)
		{
			Application.LoadLevel("iSniper.Menu");
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
}
