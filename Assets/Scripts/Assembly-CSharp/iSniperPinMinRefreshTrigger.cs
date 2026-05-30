using System;
using System.Collections;
using UnityEngine;

public class iSniperPinMinRefreshTrigger : MonoBehaviour
{
	internal class ArrayListCompare : IComparer
	{
		private System.Random r = new System.Random();

		public int Compare(object x, object y)
		{
			return r.Next(-1, 1);
		}
	}

	protected ArrayList m_PinMinPoints;

	protected iSniperStageCfg m_StageCfg;

	private iSniperGameScene m_GameScene;

	private void Start()
	{
		m_GameScene = GameObject.Find("Main Camera").GetComponent<iSniperGame>().m_GameScene;
		m_PinMinPoints = new ArrayList();
		m_StageCfg = base.transform.parent.gameObject.GetComponent("iSniperStageCfg") as iSniperStageCfg;
		int childCount = base.gameObject.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.gameObject.transform.GetChild(i).gameObject;
			iSniperEnemyPoint iSniperEnemyPoint2 = gameObject.GetComponent("iSniperEnemyPoint") as iSniperEnemyPoint;
			iSniperEnemyPoint2.name += i + 1;
			m_PinMinPoints.Add(iSniperEnemyPoint2);
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
			IComparer c = new ArrayListCompare();
			m_PinMinPoints.Sort(c);
			if (m_GameScene.PinMinNum() < m_StageCfg.m_iMaxPinMinNum)
			{
				float fMinWaitTime = 0f;
				bool bFirstValue = true;
				int iCount = m_StageCfg.m_iMaxPinMinNum - m_GameScene.PinMinNum();
				foreach (iSniperEnemyPoint point in m_PinMinPoints)
				{
					if (point.m_bHaveEnemy)
					{
						continue;
					}
					if (point.m_fLastRefreshTime <= 0f || Time.time - point.m_fLastRefreshTime >= m_StageCfg.m_fRefreshPinMinTimeGap)
					{
						iSniperNpc pinmin = new iSniperPinMin();
						point.m_bHaveEnemy = true;
						pinmin.m_PointScript = point;
						pinmin.m_GameScene = m_GameScene;
						string pinminID = m_StageCfg.name + "_" + point.name;
						pinmin.Initialize(pinminID, point.transform.position);
						m_GameScene.AddPinMin(pinmin);
						iCount--;
						if (iCount <= 0)
						{
							break;
						}
					}
					else if (point.m_fLastRefreshTime > 0f)
					{
						float fWaitTime = m_StageCfg.m_fRefreshPinMinTimeGap - (Time.time - point.m_fLastRefreshTime);
						if (bFirstValue)
						{
							fMinWaitTime = fWaitTime;
							bFirstValue = false;
						}
						else if (fWaitTime < fMinWaitTime)
						{
							fMinWaitTime = fWaitTime;
						}
					}
				}
				if (iCount > 0 && fMinWaitTime > 0f)
				{
					yield return new WaitForSeconds(fMinWaitTime);
				}
				else
				{
					yield return 0;
				}
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
