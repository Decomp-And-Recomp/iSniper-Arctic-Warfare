using UnityEngine;

public class iSniperChongFeng : iSniperNpc
{
	public class RushState : NpcState
	{
		private int m_iPathIndex;

		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fRushTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_NavAgent.Resume();
			npc.CrossAnimation("Rush01", true);
		}

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Rush01", true);
			m_iPathIndex = 0;
			RecomputePath(npc);
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_fFireTime -= deltaTime;
			if (iSniperChongFeng2.m_fFireTime <= 0f)
			{
				iSniperChongFeng2.STOPSTANDSTATE.m_LastState = this;
				npc.SetState(iSniperChongFeng2.STOPSTANDSTATE);
			}
			else if (iSniperChongFeng2.m_NavAgent.remainingDistance <= 0.15f && !iSniperChongFeng2.m_NavAgent.pathPending)
			{
				m_iPathIndex++;
				if (m_iPathIndex >= npc.m_PointScript.m_Path.Length)
				{
					npc.SetState(iSniperChongFeng2.STANDSTATE);
				}
				else
				{
					RecomputePath(npc);
				}
			}
		}

		private void RecomputePath(iSniperNpc npc)
		{
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.m_PointScript.m_Path[m_iPathIndex].transform.position;
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_NavAgent.SetDestination(m_EndPosition);
			iSniperChongFeng2.m_NavAgent.Resume();
			iSniperChongFeng2.m_NavAgent.speed = 2.75f;
			iSniperChongFeng2.m_NavAgent.angularSpeed = 360f;
			iSniperChongFeng2.m_NavAgent.acceleration = 15f;
			m_fStartTime = 0f;
		}
	}

	public class StopStandState : NpcState
	{
		public NpcState m_LastState;

		public Vector3 m_LastForward;

		private float m_fStateTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_NavAgent.Stop(true);
			iSniperChongFeng2.m_NavAgent.updateRotation = false;
			npc.LookAtCamera();
			npc.CrossAnimation("Aim_Readyfire01", true);
			m_fStateTime = 2f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStateTime -= deltaTime;
			if (m_fStateTime <= 0f)
			{
				iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
				iSniperChongFeng2.FIRESTATE.m_LastState = m_LastState;
				npc.SetState(iSniperChongFeng2.FIRESTATE);
			}
		}
	}

	public class FireState : NpcState
	{
		public NpcState m_LastState;

		public Vector3 m_LastForward;

		private float m_fAnimaitonTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.ResetFireTime();
			npc.PlayAnimation("Aim_Fire01", false);
			m_fAnimaitonTime = npc.AnimationLength("Aim_Fire01");
			npc.Fire();
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimaitonTime -= deltaTime;
			npc.LookAtCamera();
			if (m_fAnimaitonTime <= 0f)
			{
				iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
				iSniperChongFeng2.m_NavAgent.Resume();
				iSniperChongFeng2.m_NavAgent.updateRotation = true;
				npc.ContinueState(m_LastState);
			}
		}
	}

	public class StandState : NpcState
	{
		public override void Enter(iSniperNpc npc)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_NavAgent.updateRotation = false;
			npc.CrossAnimation("Aim_Readyfire01", true);
			npc.LookAtCamera();
			iSniperChongFeng2.m_bIsLastState = true;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_fFireTime -= deltaTime;
			if (iSniperChongFeng2.m_fFireTime <= 0f)
			{
				iSniperChongFeng2.FIRESTATE.m_LastState = this;
				npc.SetState(iSniperChongFeng2.FIRESTATE);
			}
		}
	}

	public class DamageState : NpcState
	{
		public NpcState m_LastState;

		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_NavAgent.Stop(true);
			switch (m_DamagePart)
			{
			case Part.kHead:
				npc.PlayAnimation("Aim_Damage_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Damage_Head01");
				break;
			case Part.kBody:
				npc.PlayAnimation("Aim_Damage_Body01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Damage_Body01");
				break;
			case Part.kArmL:
			case Part.kLegL:
				npc.PlayAnimation("Aim_Damage_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Damage_LS01");
				break;
			case Part.kArmR:
			case Part.kLegR:
				npc.PlayAnimation("Aim_Damage_RS01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Damage_RS01");
				break;
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
				npc.ContinueState(m_LastState);
			}
		}
	}

	public class DeadState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.ShowMark(false);
			npc.DestroyObserver();
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)npc;
			iSniperChongFeng2.m_NavAgent.Stop(true);
			switch (m_DamagePart)
			{
			case Part.kHead:
				npc.PlayAnimation("Aim_Death_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Death_Head01");
				break;
			case Part.kBody:
				npc.PlayAnimation("Aim_Death_Body01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Death_Body01");
				break;
			case Part.kArmL:
			case Part.kLegL:
				npc.PlayAnimation("Aim_Death_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Death_LS01");
				break;
			case Part.kArmR:
			case Part.kLegR:
				npc.PlayAnimation("Aim_Death_RS01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Death_RS01");
				break;
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

	public float m_fFireTime = -1f;

	public RushState RUSHSTATE = new RushState();

	public StopStandState STOPSTANDSTATE = new StopStandState();

	public FireState FIRESTATE = new FireState();

	public StandState STANDSTATE = new StandState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public UnityEngine.AI.NavMeshAgent m_NavAgent;

	public bool m_bIsLastState;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("ChongFeng", strName, position);
		m_NavAgent = GameObject.Find(strName).AddComponent<UnityEngine.AI.NavMeshAgent>();
		m_bIsLastState = false;
		ResetFireTime();
		SetState(RUSHSTATE);
	}

	public override void OnHit(Part part)
	{
		if (m_iCurrentHealth > 0)
		{
			DAMAGESTATE.m_DamagePart = part;
			DAMAGESTATE.m_LastState = m_State;
			SetState(DAMAGESTATE);
		}
		else
		{
			DEADSTATE.m_DamagePart = part;
			SetState(DEADSTATE);
		}
	}

	public override void OnListenGun()
	{
		base.OnListenGun();
	}

	public override bool IsDead()
	{
		if (base.IsDead())
		{
			return true;
		}
		return DEADSTATE == m_State;
	}

	public void ResetFireTime()
	{
		if (m_bWarning)
		{
			m_fFireTime = Random.Range(m_Property.m_fMinWTime, m_Property.m_fMaxWTime);
		}
		else
		{
			m_fFireTime = Random.Range(m_Property.m_fMinNTime, m_Property.m_fMaxNTime);
		}
	}
}
