public class OpenClikPlugin
{
	private enum Status
	{
		kShowFull = 0,
		kShowTip = 1,
		kHide = 2
	}

	private static Status s_Status;

	public static void Initialize(string key)
	{
		s_Status = Status.kHide;
	}

	public static void AudriodInit(string appId, string appSignature)
	{
		ChartBoostAndroid.init(appId, appSignature);
		ChartBoostAndroid.onStart();
	}

	public static void Show(bool show_full)
	{
		ChartBoostAndroid.showInterstitial(null);
	}

	public static void Hide()
	{
		s_Status = Status.kHide;
	}
}
