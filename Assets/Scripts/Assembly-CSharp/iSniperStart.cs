using UnityEngine;

public class iSniperStart : MonoBehaviour
{
	private int m_counter;

	private float m_fTime;

	private void Start()
	{
		Application.targetFrameRate = 90;
		m_counter = 0;
		if (iSniperAndroidIAP.m_platform == iSniperAndroidIAP.Platform.kGooglePlay)
		{
			OpenClikPlugin.AudriodInit("51d0f7fd16ba47ad3c000000", "8cf5f6dc941b8ae4b490e8325befd62ff61559dd");
		}
		else if (iSniperAndroidIAP.m_platform == iSniperAndroidIAP.Platform.kAmazon)
		{
			OpenClikPlugin.AudriodInit("51d1100c17ba47a022000001", "924ae9a34968b52695c6cc9dbacc41bfbf004e73");
		}
		FlurryPlugin.StartSession("EZ179G71J3YCDZHUAYWB");
		FlurryPlugin.logEvent("LauchGame", "NewStart", "LauchType");
		TouchScreenKeyboard.hideInput = true;
	}

	private void Update()
	{
		m_counter++;
		if (m_counter == 10)
		{
			Application.LoadLevel("iSniper.Load");
		}
	}
}
