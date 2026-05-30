using UnityEngine;

public class iSniperBuJi : iSniperNpc
{
	public class NormalState : NpcState
	{
		private float m_fDisappearTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperGameState gameState = iSniperGameApp.GetInstance().m_GameState;
			int iStageIndex = gameState.m_iStageIndex;
			iSniperStageCfg stageCfg = npc.m_GameScene.m_SceneDataCfg.GetStageCfg(iStageIndex);
			m_fDisappearTime = stageCfg.m_iBuJiDisappear;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			if (m_fDisappearTime != -1f)
			{
				m_fDisappearTime -= deltaTime;
				if (m_fDisappearTime <= 0f)
				{
					Object.Destroy(npc.m_Model);
				}
			}
		}
	}

	public class DeadState : NpcState
	{
		public override void Enter(iSniperNpc npc)
		{
			npc.ShowMark(false);
			npc.DestroyObserver();
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			npc.SetState(npc.DESTROYSTATE);
		}
	}

	public DeadState DEADSTATE = new DeadState();

	public NormalState NORMALSTATE = new NormalState();

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		if (Utils.ProbabilityIsRandomHit(0.4f))
		{
			if (GameObject.Find("Scene4_Prefab") != null && !m_GameState.m_bStoryMode)
			{
				CreateOtherModel("BuJi_HP", strName, position);
			}
			else
			{
				CreateOtherModel("BuJi_Time", strName, position);
			}
		}
		else if (GameObject.Find("Scene2_Prefab") != null)
		{
			CreateOtherModel("BuJi_Time", strName, position);
		}
		else
		{
			CreateOtherModel("BuJi_HP", strName, position);
		}
		SetState(NORMALSTATE);
	}

	public override void OnHit(Part part)
	{
		SetState(DEADSTATE);
	}
}
