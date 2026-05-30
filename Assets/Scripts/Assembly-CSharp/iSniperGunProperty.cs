public class iSniperGunProperty
{
	public int m_iIndex;

	public string m_strName;

	public int m_iPrice;

	public int m_iHarmLevel;

	public int m_iHarmMaxUpdateNum;

	public int m_iHarmCurrentUpdateNum;

	public int m_iHarmCash;

	public int m_iSliencerLevel;

	public int m_iSliencerMaxUpdateNum;

	public int m_iSliencerCurrentUpdateNum;

	public int m_iSliencerCash;

	public int m_iFireLevel;

	public int m_iFireMaxUpdateNum;

	public int m_iFireCurrentUpdateNum;

	public int m_iFireCash;

	public int m_iClipLevel;

	public int m_iClipMaxUpdateNum;

	public int m_iClipCurrentUpdateNum;

	public int m_iClipCash;

	public int m_iReloadLevel;

	public int m_iReloadMaxUpdateNum;

	public int m_iReloadCurrentUpdateNum;

	public int m_iReloadCash;

	public int m_iZoomLevel;

	public int m_iZoomMaxUpdateNum;

	public int m_iZoomCurrentUpdateNum;

	public int m_iZoomCash;

	public string m_strSound;

	public int m_iState;

	public iSniperGunProperty()
	{
		m_strName = string.Empty;
		m_iPrice = 0;
		m_iState = -1;
		m_iHarmCurrentUpdateNum = 0;
		m_iSliencerCurrentUpdateNum = 0;
		m_iFireCurrentUpdateNum = 0;
		m_iClipCurrentUpdateNum = 0;
		m_iReloadCurrentUpdateNum = 0;
		m_iZoomCurrentUpdateNum = 0;
	}

	public bool IsLock()
	{
		return -1 == m_iState;
	}

	public bool IsBuy()
	{
		return 1 == m_iState;
	}

	public int GetMaxHarmStar()
	{
		return m_iHarmLevel + m_iHarmMaxUpdateNum;
	}

	public int GetCurrnentHarmStar()
	{
		return m_iHarmLevel + m_iHarmCurrentUpdateNum;
	}

	public int GetCurrentHarm()
	{
		int currnentHarmStar = GetCurrnentHarmStar();
		return 80 + (currnentHarmStar - 1) * 20;
	}

	public int GetMaxSliencerStar()
	{
		return m_iSliencerLevel + m_iSliencerMaxUpdateNum;
	}

	public int GetCurrentSliencerStar()
	{
		return m_iSliencerLevel + m_iSliencerCurrentUpdateNum;
	}

	public int GetCurrentSliencer()
	{
		int currentSliencerStar = GetCurrentSliencerStar();
		return 10 - (currentSliencerStar - 1);
	}

	public int GetMaxFireStar()
	{
		return m_iFireLevel + m_iFireMaxUpdateNum;
	}

	public int GetCurrentFireStar()
	{
		return m_iFireLevel + m_iFireCurrentUpdateNum;
	}

	public float GetCurrentFire()
	{
		return 2f - (float)(m_iFireLevel - 1) * 0.2f;
	}

	public int GetMaxClipStar()
	{
		return m_iClipLevel + m_iClipMaxUpdateNum;
	}

	public int GetCurrentClipStar()
	{
		return m_iClipLevel + m_iClipCurrentUpdateNum;
	}

	public int GetCurrentClip()
	{
		return 3 + (GetCurrentClipStar() - 1);
	}

	public int GetMaxReloadStar()
	{
		return m_iReloadLevel + m_iReloadMaxUpdateNum;
	}

	public int GetCurrentReloadStar()
	{
		return m_iReloadLevel + m_iReloadCurrentUpdateNum;
	}

	public float GetCurrentReload()
	{
		return 4f - (float)(GetCurrentReloadStar() - 1) * 0.4f;
	}

	public int GetMaxZoomStar()
	{
		return m_iZoomLevel + m_iZoomMaxUpdateNum;
	}

	public int GetCurrentZoomStar()
	{
		return m_iZoomLevel + m_iZoomCurrentUpdateNum;
	}

	public float GetCurrentZoom()
	{
		return 6f;
	}

	public float GetCurrentZoomDelta()
	{
		return (float)GetCurrentZoomStar() * 0.1f + 1f;
	}
}
