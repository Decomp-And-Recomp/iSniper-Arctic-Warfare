using UnityEngine;

public class iSniperPinMin : iSniperNpc
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
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			npc.m_CurrentPose = Pose.kStandby;
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				npc.CrossAnimation("PingMin_Zou01", true);
			}
			else
			{
				npc.CrossAnimation("PingMin_Zou02", true);
			}
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			while (npc.m_CameraScript.IsOutScreen(m_EndPosition))
			{
				m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			}
			iSniperPinMin2.m_NavAgent.speed = 1.2f;
			iSniperPinMin2.m_NavAgent.angularSpeed = 360f;
			iSniperPinMin2.m_NavAgent.destination = m_EndPosition;
			iSniperPinMin2.m_NavAgent.Resume();
			iSniperPinMin2.m_NavAgent.acceleration = 50f;
			m_fStartTime = 0f;
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			if (iSniperPinMin2.m_NavAgent.remainingDistance <= 0.15f && !iSniperPinMin2.m_NavAgent.pathPending)
			{
				iSniperPinMin iSniperPinMin3 = (iSniperPinMin)npc;
				npc.SetState(iSniperPinMin3.ZHANGWANGSTATE);
			}
		}
	}

	public class ZhangWangState : NpcState
	{
		private float m_fMaxZhangwangTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
			npc.m_bWarning = false;
			npc.m_CurrentPose = Pose.kStandby;
			npc.RandomCrossAnimation("PingMin_ZhangWang01");
			m_fMaxZhangwangTime = Random.Range(1f, 5f);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fMaxZhangwangTime -= deltaTime;
			if (m_fMaxZhangwangTime <= 0f)
			{
				iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
				npc.SetState(iSniperPinMin2.WALKSTATE);
			}
		}
	}

	public class DunXiaState : NpcState
	{
		private float m_fAnimationTime;

		private bool m_bFadou;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kSquat;
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				npc.PlayAnimation("PingMin_DunXia01", false);
				m_fAnimationTime = npc.AnimationLength("PingMin_DunXia01");
				m_bFadou = true;
			}
			else
			{
				npc.PlayAnimation("PingMin_DunXia02", false);
				m_fAnimationTime = npc.AnimationLength("PingMin_DunXia02");
				m_bFadou = false;
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
				if (m_bFadou)
				{
					npc.SetState(iSniperPinMin2.FADOUSTATE);
				}
				else
				{
					npc.SetState(iSniperPinMin2.TAOPAOSTATE);
				}
			}
		}
	}

	public class FaDouState : NpcState
	{
		private float m_fMaxFadouTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kSquat;
			npc.CrossAnimation("PingMin_FaDou01", true);
			m_fMaxFadouTime = Random.Range(2f, 4f);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fMaxFadouTime -= deltaTime;
			if (m_fMaxFadouTime <= 0f)
			{
				iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
				npc.SetState(iSniperPinMin2.DUNXIAZHANGWANGSTATE);
			}
		}
	}

	public class DunXiaZhangWang : NpcState
	{
		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
			npc.CrossAnimation("PingMin_DunXia03", true);
			npc.m_CurrentPose = Pose.kSquat;
			m_fAnimationTime = npc.AnimationLength("PingMin_DunXia03");
			npc.m_bWarning = false;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
				if (Utils.ProbabilityIsRandomHit(0.5f))
				{
					npc.SetState(iSniperPinMin2.WALKSTATE);
				}
				else
				{
					npc.SetState(iSniperPinMin2.ZHANGWANGSTATE);
				}
			}
		}
	}

	public class TaoPaoState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fRushTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
			npc.m_CurrentPose = Pose.kStandby;
			npc.CrossAnimation("PingMin_TaoPao01", true);
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			while (npc.m_CameraScript.IsOutScreen(m_EndPosition))
			{
				Debug.Log("NOTICE:m_EndPosition is OUT Screen!!!!!!!!!!!");
				m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			}
			m_fRushTime = Vector3.Distance(m_StartPosition, m_EndPosition) / 5f;
			iSniperPinMin2.m_NavAgent.speed = 2.75f;
			iSniperPinMin2.m_NavAgent.angularSpeed = 360f;
			iSniperPinMin2.m_NavAgent.destination = m_EndPosition;
			iSniperPinMin2.m_NavAgent.Resume();
			iSniperPinMin2.m_NavAgent.acceleration = 50f;
			m_fStartTime = 0f;
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			if (iSniperPinMin2.m_NavAgent.remainingDistance <= 0.15f && !iSniperPinMin2.m_NavAgent.pathPending)
			{
				iSniperPinMin iSniperPinMin3 = (iSniperPinMin)npc;
				npc.SetState(iSniperPinMin3.ZHANGWANGSTATE);
			}
		}
	}

	public class DamageState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
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
				iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
				if (Utils.ProbabilityIsRandomHit(0.5f))
				{
					npc.SetState(iSniperPinMin2.TAOPAOSTATE);
				}
				else
				{
					npc.SetState(iSniperPinMin2.DUNXIASTATE);
				}
			}
		}
	}

	public class DeadState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperPinMin iSniperPinMin2 = (iSniperPinMin)npc;
			iSniperPinMin2.m_NavAgent.Stop();
			if (npc.m_CurrentPose == Pose.kStandby)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					npc.PlayAnimation("PingMin_Death04", false);
					m_fAnimationTime = npc.AnimationLength("PingMin_Death04");
					break;
				case Part.kBody:
					npc.PlayAnimation("PingMin_Death02", false);
					m_fAnimationTime = npc.AnimationLength("PingMin_Death02");
					break;
				case Part.kArmL:
				case Part.kLegL:
					npc.PlayAnimation("PingMin_Death03", false);
					m_fAnimationTime = npc.AnimationLength("PingMin_Death03");
					break;
				case Part.kArmR:
				case Part.kLegR:
					npc.PlayAnimation("PingMin_Death03", false);
					m_fAnimationTime = npc.AnimationLength("PingMin_Death03");
					break;
				}
			}
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					npc.PlayAnimation("Squat_Death_Head01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Death_Head01");
					break;
				case Part.kBody:
					npc.CrossAnimation("Trigger_Death03", false);
					m_fAnimationTime = npc.AnimationLength("Trigger_Death03");
					break;
				case Part.kArmL:
				case Part.kLegL:
					npc.PlayAnimation("Squat_Death_LS01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Death_LS01");
					break;
				case Part.kArmR:
				case Part.kLegR:
					npc.PlayAnimation("Squat_Death_RS01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Death_RS01");
					break;
				}
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

	public DunXiaState DUNXIASTATE = new DunXiaState();

	public FaDouState FADOUSTATE = new FaDouState();

	public DunXiaZhangWang DUNXIAZHANGWANGSTATE = new DunXiaZhangWang();

	public TaoPaoState TAOPAOSTATE = new TaoPaoState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public UnityEngine.AI.NavMeshAgent m_NavAgent;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("PinMin", strName, position);
		m_NavAgent = m_Model.GetComponent("NavMeshAgent") as UnityEngine.AI.NavMeshAgent;
		if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			SetState(WALKSTATE);
		}
		else
		{
			SetState(ZHANGWANGSTATE);
		}
	}

	public override void OnListenGun()
	{
		if (!m_bWarning)
		{
			m_bWarning = true;
			SetState(DUNXIASTATE);
		}
	}

	public override void OnHit(Part part)
	{
		if (m_iCurrentHealth > 0)
		{
			Debug.Log("m_iCurrentHealth > 0 :" + m_iCurrentHealth);
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
