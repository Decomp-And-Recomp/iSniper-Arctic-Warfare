using System;
using System.Collections;
using UnityEngine;

public class iSniperBuJiRefreshTrigger : MonoBehaviour
{
	internal class ArrayListCompare : IComparer
	{
		private System.Random r = new System.Random();

		public int Compare(object x, object y)
		{
			return r.Next(-1, 1);
		}
	}

	protected ArrayList m_BuJiPoints;

	protected iSniperStageCfg m_StageCfg;

	private iSniperGameScene m_GameScene;

	private float m_fRandomTime;

	private bool bFirstValue = true;

	private void Start()
	{
		m_GameScene = GameObject.Find("Main Camera").GetComponent<iSniperGame>().m_GameScene;
		m_BuJiPoints = new ArrayList();
		m_StageCfg = base.transform.parent.gameObject.GetComponent("iSniperStageCfg") as iSniperStageCfg;
		m_fRandomTime = UnityEngine.Random.Range(40f, 55f);
		int childCount = base.gameObject.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.gameObject.transform.GetChild(i).gameObject;
			iSniperEnemyPoint iSniperEnemyPoint2 = gameObject.GetComponent("iSniperEnemyPoint") as iSniperEnemyPoint;
			iSniperEnemyPoint2.name += i + 1;
			m_BuJiPoints.Add(iSniperEnemyPoint2);
		}
		StartCoroutine(StartRefresh());
	}

	public IEnumerator StartRefresh()
	{
		while (!IsThisRunning())
		{
			yield return 0;
		}
		while (IsThisRunning())
		{
			if (m_GameScene.BuJiNum() < m_StageCfg.m_iMaxBuJiNum && m_GameScene.m_State == iSniperGameScene.State.kGaming)
			{
				IComparer c = new ArrayListCompare();
				m_BuJiPoints.Sort(c);
				int iCount = m_StageCfg.m_iMaxBuJiNum - m_GameScene.BuJiNum();
				foreach (iSniperEnemyPoint point in m_BuJiPoints)
				{
					if (!point.m_bHaveEnemy)
					{
						iSniperNpc buji = new iSniperBuJi();
						point.m_bHaveEnemy = true;
						buji.m_PointScript = point;
						buji.m_GameScene = m_GameScene;
						string bujiID = m_StageCfg.name + "_" + point.name;
						buji.Initialize(bujiID, point.transform.position);
						m_GameScene.AddBuJi(buji);
						iCount--;
						if (iCount <= 0)
						{
							bFirstValue = false;
							break;
						}
					}
				}
				yield return 0;
			}
			else
			{
				yield return 0;
			}
		}
	}

	private bool IsThisRunning()
	{
		string text = "Stage" + iSniperGameApp.GetInstance().m_GameState.m_iStageIndex;
		return text == m_StageCfg.name;
	}
}
