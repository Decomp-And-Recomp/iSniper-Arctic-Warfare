using System.Collections;
using UnityEngine;

public class iSniperShowTextUIHelper : UIHelper
{
	private float m_fTime = 2f;

	private string m_strText;

	private string m_strName;

	private new void Start()
	{
		m_font_path = "iSniper3D/Fonts/Materials/";
		m_ui_material_path = "iSniper3D/UI/Materials/";
		m_ui_cfgxml = "iSniper3D/UI/ShowTextUI";
		base.Start();
		((UIImage)m_control_table["fadeimage"]).Visible = true;
		((UIImage)m_control_table["fadeimage"]).Enable = true;
		((UIText)m_control_table["newgametext"]).Visible = true;
		((UIText)m_control_table["newgametext1"]).Visible = true;
		if (iSniperGameApp.GetInstance().m_GameState.m_bStoryMode)
		{
			m_strText = "  Sometimes the truth isn't what you saw.";
			m_strName = " Ethan Embry";
		}
		else
		{
			m_strText = "               Vengeance eats the soul.";
			m_strName = "Glous Miller";
		}
		StartCoroutine("TypeText", m_strText);
	}

	private void Update()
	{
		m_fTime -= Time.deltaTime;
		if (Input.GetMouseButtonDown(0) && m_fTime <= 0f)
		{
			StopCoroutine("TypeText");
			StopCoroutine("TypeTextName");
			((UIText)m_control_table["newgametext"]).SetText(m_strText);
			((UIText)m_control_table["newgametext1"]).SetText(m_strName);
			StartCoroutine("LoadScene");
		}
	}

	public IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(1.5f);
		if (iSniperGameApp.GetInstance().m_GameState.m_bStoryMode)
		{
			iSniperGameApp.GetInstance().m_GameState.m_bStoryStart = true;
			Application.LoadLevel("iSniper.Story");
		}
		else
		{
			iSniperGameApp.GetInstance().m_GameState.m_bStoryStart = true;
			Application.LoadLevel("iSniper.StoryStart");
		}
	}

	public IEnumerator TypeText(string str)
	{
		yield return new WaitForSeconds(1.5f);
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
		StartCoroutine("TypeTextName", m_strName);
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
		yield return new WaitForSeconds(1.5f);
		StartCoroutine("LoadScene");
	}
}
