using System;
using System.Collections;
using UnityEngine;

public class iSniperEnemyRefreshTrigger : MonoBehaviour
{
	internal class ArrayListCompare : IComparer
	{
		private System.Random r = new System.Random();

		public int Compare(object x, object y)
		{
			return r.Next(-1, 1);
		}
	}

	protected ArrayList m_EnemyPoints;

	protected iSniperStageCfg m_StageCfg;

	private iSniperGameScene m_GameScene;

	private iSniperGameState m_GameState;

	private void Start()
	{
		m_GameScene = GameObject.Find("Main Camera").GetComponent<iSniperGame>().m_GameScene;
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		m_EnemyPoints = new ArrayList();
		m_StageCfg = base.transform.parent.gameObject.GetComponent("iSniperStageCfg") as iSniperStageCfg;
		int childCount = base.gameObject.transform.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.gameObject.transform.GetChild(i).gameObject;
			iSniperEnemyPoint iSniperEnemyPoint2 = gameObject.GetComponent("iSniperEnemyPoint") as iSniperEnemyPoint;
			iSniperEnemyPoint2.name += i + 1;
			m_EnemyPoints.Add(iSniperEnemyPoint2);
		}
		StartCoroutine(StartRefresh());
	}

	public IEnumerator StartRefresh()
	{
		while (!IsThisRunning())
		{
			yield return 0;
		}
		if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
		{
			if (m_GameState.m_iArcCurScene == 4)
			{
				m_StageCfg.m_iKillEnemySum = 999;
			}
			else
			{
				m_StageCfg.m_iKillEnemySum += m_GameState.RecomputeAddKillEnemySum(m_GameState.m_iArcDaysNum);
				if (m_GameState.m_iArcCurScene == 2)
				{
					m_StageCfg.m_iKillEnemySum /= 2;
				}
				if ((m_GameState.m_iArcCurScene == 5 || m_GameState.m_iArcCurScene == 6) && m_GameState.m_iArcCurStage == 5)
				{
					m_StageCfg.m_iKillEnemySum /= 2;
					Debug.Log("m_StageCfg.m_iKillEnemySum = m_StageCfg.m_iKillEnemySum / 2;");
				}
				if (m_GameState.m_iArcCurScene == 6 && m_GameState.m_iArcCurStage == 4)
				{
					m_StageCfg.m_iKillEnemySum = (int)((float)m_StageCfg.m_iKillEnemySum / 1.5f);
					Debug.Log("m_StageCfg.m_iKillEnemySum = (int)(m_StageCfg.m_iKillEnemySum / 1.5f)");
				}
			}
		}
		while (m_GameState.m_iKillEnemyNum < m_StageCfg.m_iKillEnemySum)
		{
			int iMaxEnemyNum = m_StageCfg.m_iKillEnemySum - m_GameState.m_iKillEnemyNum;
			if (iMaxEnemyNum > m_StageCfg.m_iMaxEnemyNum)
			{
				iMaxEnemyNum = m_StageCfg.m_iMaxEnemyNum;
			}
			if (m_GameScene.EnemyNum() < iMaxEnemyNum)
			{
				IComparer c = new ArrayListCompare();
				m_EnemyPoints.Sort(c);
				bool bFirstValue = true;
				int iCount = iMaxEnemyNum - m_GameScene.EnemyNum();
				foreach (iSniperEnemyPoint point in m_EnemyPoints)
				{
					if (!point.m_bHaveEnemy && !m_GameScene.m_CameraScript.JudgeIsOutScreen(point.transform.position) && (point.m_fLastRefreshTime <= 0f || Time.time - point.m_fLastRefreshTime >= m_StageCfg.m_fRefreshEnemyTimeGap))
					{
						iSniperNpc enemy = null;
						switch (point.m_strType)
						{
						case "XunLuo":
							enemy = new iSniperXunLuo();
							break;
						case "JuJi":
							enemy = new iSniperJuJi();
							break;
						case "JingJie":
							enemy = new iSniperJingJie();
							break;
						case "YouJi":
							enemy = new iSniperYouJi();
							break;
						case "YinBao":
							enemy = new iSniperYinBao();
							break;
						case "SaoShe":
							enemy = new iSniperSaoShe();
							break;
						case "ChongFeng":
							enemy = new iSniperChongFeng();
							break;
						case "MoTo":
							enemy = new iSniperMoTo();
							break;
						case "ZhuangJia":
							enemy = new iSniperZhuangJia();
							break;
						case "YinHu":
							enemy = new iSniperYinHu();
							break;
						case "DuoCang":
							enemy = new iSniperDuoCang();
							break;
						}
						point.m_bHaveEnemy = true;
						enemy.m_PointScript = point;
						string enemyID = m_StageCfg.name + "_" + point.name;
						enemy.m_GameScene = m_GameScene;
						enemy.Initialize(enemyID, point.transform.position);
						m_GameScene.AddEnemy(enemy);
						iCount--;
						if (iCount <= 0)
						{
							break;
						}
					}
				}
			}
			yield return 0;
		}
	}

	private bool IsThisRunning()
	{
		string text = "Stage" + iSniperGameApp.GetInstance().m_GameState.m_iStageIndex;
		return text == m_StageCfg.name;
	}
}
