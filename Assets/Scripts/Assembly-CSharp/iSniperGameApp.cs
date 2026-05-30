public class iSniperGameApp
{
	private static iSniperGameApp s_Instance;

	public iSniperGameState m_GameState;

	public iSniperGunCenter m_GunCenter;

	public static iSniperGameApp GetInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = new iSniperGameApp();
			s_Instance.Initialize();
		}
		return s_Instance;
	}

	public void Initialize()
	{
		m_GameState = new iSniperGameState();
		m_GameState.Initialize();
		m_GunCenter = new iSniperGunCenter();
		m_GunCenter.Initialize();
	}

	public string GetKey()
	{
		return "0123456789Sniper";
	}
}
