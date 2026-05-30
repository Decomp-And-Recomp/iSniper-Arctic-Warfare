using UnityEngine;

public class iSniperLoadingUIHelper : UIHelper
{
	public new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/LoadingUI";
		base.Start();
		int num = 1;
		if (Utils.IsRetina())
		{
			num = 2;
		}
		((UIImage)m_control_table["loading"]).Visible = true;
		FadeIn();
	}

	public void Update()
	{
	}

	public override void OnHandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public override void OnAnimationFinished(string name)
	{
		if ("fadein" == name)
		{
			((UIImage)m_control_table["fadeimage"]).Visible = false;
			((UIImage)m_control_table["loading"]).Visible = true;
			Application.LoadLevel(iSniperGameApp.GetInstance().m_GameState.m_strSceneName);
			iSniperGameApp.GetInstance().m_GameState.m_strSceneName = null;
		}
	}

	public void FadeIn()
	{
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIImage)m_control_table["fadeimage"]).SetAlpha(1f);
		StartAnimation("fadein");
	}
}
