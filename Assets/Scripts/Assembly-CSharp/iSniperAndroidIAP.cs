using UnityEngine;

public class iSniperAndroidIAP : MonoBehaviour
{
	public enum Platform
	{
		kGooglePlay = 0,
		kAmazon = 1
	}

	public delegate void CallbackFunc();

	public static bool m_bIsSupport = false;

	public static Platform m_platform = Platform.kGooglePlay;

	public static readonly string m_strKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvfxRqpA+fjKm64VbNaXM6offkWUUsgCRzZlJFJrjZD5MTcX2p2/nfyOYiNDAh9qrS6hoS7MfIvPYirc38oerql/die8eIsW5JtBkeVt2te9+ZCc2BjmOr2b3g+xirbE1bkReJP5JDARHColJA7lQ/6o4J8rvv9L1rGcYynrWeSdTegeBDRkuMPQjgNArMXzkw7hITPdLXhQtBgnn62tV7zvguxKMuYoqzmXpyMsSyyAFVGDQAvI7ITKXvRR+0LL2ybjmP0+0kwLu7NL+nshBm8msjHbCqclsiOwcEkaMFk/Jgqg8B2MLeL7Ff2PJIJA023FnMfPgzNJIde0hj20j6wIDAQAB";

	public static readonly string[] m_GooglePlayId = new string[6] { "com.trinitigame.isniper3darcticwarfare.099cents", "com.trinitigame.isniper3darcticwarfare.199cents", "com.trinitigame.isniper3darcticwarfare.299cents", "com.trinitigame.isniper3darcticwarfare.499cents", "com.trinitigame.isniper3darcticwarfare.999cents", "com.trinitigame.isniper3darcticwarfare.1999cents" };

	public static readonly int[] m_GooglePlayReward = new int[6] { 10000, 21000, 32000, 54000, 110000, 224000 };

	public static readonly string[] m_AmazonId = new string[6] { "com.trinitigame.isniper3darcticwarfare.099cents", "com.trinitigame.isniper3darcticwarfare.199cents", "com.trinitigame.isniper3darcticwarfare.299cents", "com.trinitigame.isniper3darcticwarfare.499cents", "com.trinitigame.isniper3darcticwarfare.999cents", "com.trinitigame.isniper3darcticwarfare.1999cents" };

	public static readonly int[] m_AmazonReward = new int[6] { 10000, 21000, 32000, 54000, 110000, 224000 };

	public static CallbackFunc BuyStartFunc = null;

	public static CallbackFunc CompleteFunc = null;

	public static CallbackFunc FailureFunc = null;

	public static string curBuySku = string.Empty;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		if (m_platform == Platform.kAmazon)
		{
			base.gameObject.AddComponent<AmazonIAPManager>();
		}
		else if (m_platform == Platform.kGooglePlay)
		{
			base.gameObject.AddComponent<GoogleIABManager>();
		}
		string empty = string.Empty;
		if (m_platform == Platform.kAmazon)
		{
			empty = "iSniper3D/IapPrefab/AmazonIAPEventListener";
			GameObject gameObject = Instantiate(Resources.Load<GameObject>(empty));
		}
		else if (m_platform == Platform.kGooglePlay)
		{
			empty = "iSniper3D/IapPrefab/IABAndroidEventListener";
			GameObject gameObject2 = Instantiate(Resources.Load<GameObject>(empty));
		}
	}

	private void Start()
	{
		Initialize();
	}

	public static void Initialize()
	{
		if (m_platform == Platform.kAmazon)
		{
			AmazonIAP.initiateItemDataRequest(m_AmazonId);
		}
		else if (m_platform == Platform.kGooglePlay)
		{
			GoogleIAB.init(m_strKey);
		}
	}

	public static void BuyIap(string sku)
	{
		Debug.Log("BuyIap sku :" + sku + " support " + m_bIsSupport);
		if (!m_bIsSupport)
		{
			Initialize();
			if (FailureFunc != null)
			{
				FailureFunc();
			}
			return;
		}
		if (m_platform == Platform.kAmazon)
		{
			AmazonIAP.initiatePurchaseRequest(sku);
		}
		else if (m_platform == Platform.kGooglePlay)
		{
			GoogleIAB.purchaseProduct(sku);
		}
		curBuySku = sku;
		if (BuyStartFunc != null)
		{
			BuyStartFunc();
		}
	}

	public static void SuccessEvent(string sku)
	{
		if (m_platform == Platform.kAmazon)
		{
			int num = 0;
			for (num = 0; num < m_AmazonId.Length && !(m_AmazonId[num] == sku); num++)
			{
			}
			iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash += m_AmazonReward[num];
		}
		else if (m_platform == Platform.kGooglePlay)
		{
			int num2 = 0;
			for (num2 = 0; num2 < m_GooglePlayId.Length && !(m_GooglePlayId[num2] == sku); num2++)
			{
			}
			iSniperGameApp.GetInstance().m_GameState.m_iPlayerCash += m_GooglePlayReward[num2];
		}
		iSniperGameApp.GetInstance().m_GameState.SaveData();
	}
}
