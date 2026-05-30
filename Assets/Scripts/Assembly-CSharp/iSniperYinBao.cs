using UnityEngine;

public class iSniperYinBao : iSniperNpc
{
	public class WalkState : NpcState
	{
		private Vector3 m_StartPosition;

		public Vector3 m_EndPosition;

		private float m_fWalkTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kStandby;
			npc.CrossAnimation("Trigger_Forward", true);
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			while (npc.m_CameraScript.IsOutScreen(m_EndPosition))
			{
				m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			}
			iSniperYinBao2.m_NavAgent.angularSpeed = 360f;
			iSniperYinBao2.m_NavAgent.speed = 1.2f;
			iSniperYinBao2.m_NavAgent.destination = m_EndPosition;
			iSniperYinBao2.m_NavAgent.Resume();
			iSniperYinBao2.m_NavAgent.acceleration = 50f;
			m_fStartTime = 0f;
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			if (iSniperYinBao2.m_NavAgent.remainingDistance <= 0.15f && !iSniperYinBao2.m_NavAgent.pathPending)
			{
				iSniperYinBao iSniperYinBao3 = (iSniperYinBao)npc;
				if (Utils.ProbabilityIsRandomHit(0.5f))
				{
					npc.SetState(iSniperYinBao3.ZHANGWANGSTATE);
				}
				else
				{
					npc.SetState(iSniperYinBao3.ZHANGWANGWALKSTATE);
				}
			}
		}
	}

	public class ZhangWangWalkState : NpcState
	{
		private Vector3 m_StartPosition;

		public Vector3 m_EndPosition;

		private float m_fWalkTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kStandby;
			npc.CrossAnimation("Trigger_Forward02", true);
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			while (npc.m_CameraScript.IsOutScreen(m_EndPosition))
			{
				m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			}
			iSniperYinBao2.m_NavAgent.angularSpeed = 460f;
			iSniperYinBao2.m_NavAgent.speed = 1.2f;
			iSniperYinBao2.m_NavAgent.destination = m_EndPosition;
			iSniperYinBao2.m_NavAgent.Resume();
			iSniperYinBao2.m_NavAgent.acceleration = 50f;
			m_fStartTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			if (iSniperYinBao2.m_NavAgent.remainingDistance <= 0.15f && !iSniperYinBao2.m_NavAgent.pathPending)
			{
				npc.SetState(iSniperYinBao2.WALKSTATE);
			}
		}
	}

	public class ZhangWangState : NpcState
	{
		private float m_fMaxZhangwangTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kStandby;
			npc.RandomCrossAnimation("PingMin_ZhangWang01");
			m_fMaxZhangwangTime = Random.Range(1f, 5f);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fMaxZhangwangTime -= deltaTime;
			if (m_fMaxZhangwangTime <= 0f)
			{
				iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
				npc.SetState(iSniperYinBao2.WALKSTATE);
			}
		}
	}

	public class DunXiaState : NpcState
	{
		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kSquat;
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				npc.PlayAnimation("PingMin_DunXia02", false);
				m_fAnimationTime = npc.AnimationLength("PingMin_DunXia02");
			}
			else
			{
				npc.PlayAnimation("PingMin_DunXia02", false);
				m_fAnimationTime = npc.AnimationLength("PingMin_DunXia02");
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
				npc.SetState(iSniperYinBao2.THREATENSTATE);
			}
		}
	}

	public class ThreatenState : NpcState
	{
		public float m_fYinBaoTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kStandby;
			npc.CrossAnimation("Trigger_zhangwang01", true);
			m_fYinBaoTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fYinBaoTime -= deltaTime;
			if (m_fYinBaoTime <= 0f)
			{
				iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
				npc.SetState(iSniperYinBao2.YINBAOSTATE);
			}
		}
	}

	public class YinBaoState : NpcState
	{
		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kStandby;
			npc.PlayAnimation("Trigger_Start", false);
			m_fAnimationTime = npc.AnimationLength("Trigger_Start");
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				Vector3 position = npc.m_WaistObj.transform.position;
				GameObject gameObject = Object.Instantiate(npc.g_EffectsConfig.m_BodyExplosionEffect, position, Quaternion.identity) as GameObject;
				gameObject.AddComponent<iSniperExplosionCheck>();
				npc.m_GameState.m_fGameTime -= 10f;
				npc.m_GameState.m_fGameTimeBonus -= 10f;
				npc.m_GameScene.m_UIHelper.ShowTitleShotTip("exp");
				npc.m_GameScene.PlaySoundAtPos(npc.m_GameScene.m_SoundExpBody, npc.m_Model.transform.position);
				MeshControllerScript meshControllerScript = npc.m_Model.GetComponent("MeshControllerScript") as MeshControllerScript;
				meshControllerScript.ShowPart("Trigger", false);
				npc.EnterDeadState();
			}
		}
	}

	public class DamageState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			m_fAnimationTime = 0f;
			if (npc.m_CurrentPose == Pose.kStandby)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					npc.PlayAnimation("Standby_Damage_Head01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Damage_Head01");
					break;
				case Part.kBody:
					npc.PlayAnimation("Standby_Damage_Body01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Damage_Body01");
					break;
				case Part.kArmL:
				case Part.kLegL:
					npc.PlayAnimation("Standby_Damage_LS01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Damage_LS01");
					break;
				case Part.kArmR:
				case Part.kLegR:
					npc.PlayAnimation("Standby_Damage_RS01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Damage_RS01");
					break;
				}
			}
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					npc.PlayAnimation("Squat_Damage_Head01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Damage_Head01");
					break;
				case Part.kBody:
					npc.PlayAnimation("Squat_Damage_Body01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Damage_Body01");
					break;
				case Part.kArmL:
				case Part.kLegL:
					npc.PlayAnimation("Squat_Damage_LS01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Damage_LS01");
					break;
				case Part.kArmR:
				case Part.kLegR:
					npc.PlayAnimation("Squat_Damage_RS01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Damage_RS01");
					break;
				}
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
				npc.SetState(iSniperYinBao2.YINBAOSTATE);
			}
		}
	}

	public class DeadState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinBao iSniperYinBao2 = (iSniperYinBao)npc;
			iSniperYinBao2.m_NavAgent.Stop();
			npc.ShowMark(false);
			npc.DestroyObserver();
			if (npc.m_CurrentPose == Pose.kStandby)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					npc.PlayAnimation("Standby_Death_Head01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Death_Head01");
					break;
				case Part.kBody:
					npc.PlayAnimation("Standby_Death_Body01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Death_Body01");
					break;
				case Part.kArmL:
				case Part.kLegL:
					npc.PlayAnimation("Standby_Death_LS01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Death_LS01");
					break;
				case Part.kArmR:
				case Part.kLegR:
					npc.PlayAnimation("Standby_Death_RS01", false);
					m_fAnimationTime = npc.AnimationLength("Standby_Death_RS01");
					break;
				}
			}
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				npc.CrossAnimation("Trigger_Death03", false);
				m_fAnimationTime = npc.AnimationLength("Trigger_Death03");
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				npc.SetState(npc.DESTROYSTATE);
			}
		}
	}

	public WalkState WALKSTATE = new WalkState();

	public ZhangWangState ZHANGWANGSTATE = new ZhangWangState();

	public ZhangWangWalkState ZHANGWANGWALKSTATE = new ZhangWangWalkState();

	public ThreatenState THREATENSTATE = new ThreatenState();

	public YinBaoState YINBAOSTATE = new YinBaoState();

	public DunXiaState DUNXIASTATE = new DunXiaState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public UnityEngine.AI.NavMeshAgent m_NavAgent;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("YinBao", strName, position);
		m_NavAgent = m_Model.GetComponent("NavMeshAgent") as UnityEngine.AI.NavMeshAgent;
		m_CurrentPose = Pose.kStandby;
		SetState(WALKSTATE);
	}

	public override void OnListenGun()
	{
		if (!m_bWarning)
		{
			base.OnListenGun();
			SetState(DUNXIASTATE);
		}
	}

	public override void OnHit(Part part)
	{
		if (m_iCurrentHealth > 0)
		{
			DAMAGESTATE.m_DamagePart = part;
			SetState(DAMAGESTATE);
		}
		else
		{
			DEADSTATE.m_DamagePart = part;
			SetState(DEADSTATE);
		}
	}

	public override bool IsDead()
	{
		if (base.IsDead())
		{
			return true;
		}
		return DEADSTATE == m_State;
	}
}
