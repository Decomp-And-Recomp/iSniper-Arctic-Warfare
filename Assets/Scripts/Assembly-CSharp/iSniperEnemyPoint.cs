using UnityEngine;

public class iSniperEnemyPoint : MonoBehaviour
{
	protected Color JingJieColor = Color.green;

	protected Color JuJiColor = Color.blue;

	protected Color YouJiColor = Color.red;

	protected Color XunLuoColor = Color.yellow;

	protected Color YinBaoColor = Color.magenta;

	protected Color PinMinColor = Color.white;

	protected Color ZhuangJiaColor = Color.cyan;

	protected Color SaoSheColor = Color.black;

	protected Color ChongFengColor = new Color(0.7f, 0.3f, 0.5f);

	protected Color MoToColor = Color.magenta;

	protected Color YinHuColor = Color.red;

	protected Color DuoCangColor = Color.magenta;

	protected Color BuJiColor = Color.grey;

	public string m_strType = string.Empty;

	public int m_iIndex;

	public float m_fLastRefreshTime;

	public bool m_bHaveEnemy;

	public float m_fMinDistance;

	public float m_fMaxDistance = 5f;

	public bool m_bCanMove2Player = true;

	public GameObject[] m_Path;

	public GameObject[] m_Range;

	public void Reset()
	{
		m_fLastRefreshTime = Time.time;
		m_bHaveEnemy = false;
	}
}
