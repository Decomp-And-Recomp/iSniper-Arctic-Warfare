using UnityEngine;

public class iSniperSaoShe : iSniperNpc
{
	public class LeftRunState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fRunTime;

		private float m_fCheckTime;

		private float m_fStartTime;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Resume();
			if (iSniperSaoShe2.m_bReverseLR)
			{
				npc.CrossAnimation("Stand_RightRun01", true);
			}
			else
			{
				npc.CrossAnimation("Stand_LeftRun01", true);
			}
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			if (iSniperSaoShe2.m_bReverseLR)
			{
				npc.CrossAnimation("Stand_RightRun01", true);
			}
			else
			{
				npc.CrossAnimation("Stand_LeftRun01", true);
			}
			m_StartPosition = npc.GetPosition();
			m_EndPosition = iSniperSaoShe2.GetRandomLeftPoint();
			iSniperSaoShe2.m_NavAgent.destination = m_EndPosition;
			iSniperSaoShe2.m_NavAgent.Resume();
			iSniperSaoShe2.m_NavAgent.speed = 2.5f;
			iSniperSaoShe2.m_NavAgent.updateRotation = false;
			iSniperSaoShe2.m_NavAgent.acceleration = 14f;
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_fFireTime -= deltaTime;
			if (iSniperSaoShe2.m_fFireTime <= 0f)
			{
				iSniperSaoShe2.STOPSTANDSTATE.m_LastState = this;
				npc.SetState(iSniperSaoShe2.STOPSTANDSTATE);
				return;
			}
			npc.LookAtCamera();
			if (iSniperSaoShe2.m_NavAgent.remainingDistance <= 0.15f && !iSniperSaoShe2.m_NavAgent.pathPending)
			{
				npc.SetState(iSniperSaoShe2.RIGHTRUNSTATE);
			}
		}
	}

	public class RightRunState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fRunTime;

		private float m_fCheckTime;

		private float m_fStartTime;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Resume();
			if (iSniperSaoShe2.m_bReverseLR)
			{
				npc.CrossAnimation("Stand_LeftRun01", true);
			}
			else
			{
				npc.CrossAnimation("Stand_RightRun01", true);
			}
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			if (iSniperSaoShe2.m_bReverseLR)
			{
				npc.CrossAnimation("Stand_LeftRun01", true);
			}
			else
			{
				npc.CrossAnimation("Stand_RightRun01", true);
			}
			m_StartPosition = npc.GetPosition();
			m_EndPosition = iSniperSaoShe2.GetRandomRightPoint();
			iSniperSaoShe2.m_NavAgent.destination = m_EndPosition;
			iSniperSaoShe2.m_NavAgent.Resume();
			iSniperSaoShe2.m_NavAgent.speed = 2.5f;
			iSniperSaoShe2.m_NavAgent.updateRotation = false;
			iSniperSaoShe2.m_NavAgent.acceleration = 14f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_fFireTime -= deltaTime;
			if (iSniperSaoShe2.m_fFireTime <= 0f)
			{
				iSniperSaoShe2.STOPSTANDSTATE.m_LastState = this;
				npc.SetState(iSniperSaoShe2.STOPSTANDSTATE);
				return;
			}
			npc.LookAtCamera();
			if (iSniperSaoShe2.m_NavAgent.remainingDistance <= 0.15f && !iSniperSaoShe2.m_NavAgent.pathPending)
			{
				npc.SetState(iSniperSaoShe2.LEFTRUNSTATE);
			}
		}
	}

	public class StopStandState : NpcState
	{
		public NpcState m_LastState;

		private Vector3 m_LastForward;

		private float m_fStateTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Stop(true);
			m_LastForward = npc.m_Model.transform.forward;
			npc.LookAtCamera();
			npc.CrossAnimation("Stand_Readyfire01", true);
			m_fStateTime = 2f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStateTime -= deltaTime;
			if (m_fStateTime <= 0f)
			{
				iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
				iSniperSaoShe2.FIRESTATE.m_LastState = m_LastState;
				npc.SetState(iSniperSaoShe2.FIRESTATE);
			}
		}
	}

	public class FireState : NpcState
	{
		public NpcState m_LastState;

		public Vector3 m_LastForward;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.ResetFireTime();
			npc.LookAtCamera();
			npc.PlayAnimation("Stand_Fire01", false);
			m_fAnimationTime = npc.AnimationLength("Stand_Fire01");
			npc.Fire();
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				npc.ContinueState(m_LastState);
			}
		}
	}

	public class RandomRushState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fRushTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Resume();
			npc.CrossAnimation("Stand_Rush01", true);
		}

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Stand_Rush01", true);
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.m_PointScript.m_Path[npc.m_PointScript.m_Path.Length - 1].transform.position;
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Resume();
			iSniperSaoShe2.m_NavAgent.destination = m_EndPosition;
			iSniperSaoShe2.m_NavAgent.speed = 2.5f;
			iSniperSaoShe2.m_NavAgent.acceleration = 14f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_fFireTime -= deltaTime;
			if (iSniperSaoShe2.m_fFireTime <= 0f)
			{
				iSniperSaoShe2.STOPSTANDSTATE.m_LastState = this;
				npc.SetState(iSniperSaoShe2.STOPSTANDSTATE);
			}
			else if (iSniperSaoShe2.m_NavAgent.remainingDistance <= 0.15f && !iSniperSaoShe2.m_NavAgent.pathPending)
			{
				npc.SetState(iSniperSaoShe2.STANDSTATE);
			}
		}
	}

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
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Resume();
			npc.CrossAnimation("Stand_Rush01", true);
		}

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Stand_Rush01", true);
			m_iPathIndex = 0;
			RecomputePath(npc);
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_fFireTime -= deltaTime;
			if (iSniperSaoShe2.m_fFireTime <= 0f)
			{
				iSniperSaoShe2.STOPSTANDSTATE.m_LastState = this;
				npc.SetState(iSniperSaoShe2.STOPSTANDSTATE);
				return;
			}
			m_fStartTime += deltaTime;
			m_fCheckTime += deltaTime;
			if (m_bNeedRotate)
			{
				float num = m_fStartTime / m_fRotateTime;
				npc.m_Model.transform.forward = Vector3.Lerp(m_StartForward, m_EndForward, num);
				if (num >= 1f)
				{
					m_bNeedRotate = false;
				}
			}
			Vector3 position = Vector3.Lerp(m_StartPosition, m_EndPosition, m_fStartTime / m_fRushTime);
			if (m_fCheckTime >= 0f)
			{
				m_fCheckTime = 0f;
				position.y = npc.CheckGround() + 0.01f;
			}
			else
			{
				position.y = npc.m_Model.transform.position.y;
			}
			npc.m_Model.transform.position = position;
			if (m_fStartTime >= m_fRushTime)
			{
				m_iPathIndex++;
				if (m_iPathIndex >= npc.m_PointScript.m_Path.Length)
				{
					npc.SetState(iSniperSaoShe2.STANDSTATE);
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
			Vector3 startPosition = m_StartPosition;
			Vector3 vector = npc.CheckGround(m_EndPosition);
			m_fRushTime = Vector3.Distance(startPosition, vector) / 2.5f;
			m_bNeedRotate = true;
			m_StartForward = npc.m_Model.transform.forward;
			startPosition.y = 0f;
			vector.y = 0f;
			m_EndForward = (vector - startPosition).normalized;
			m_fRotateTime = Mathf.Abs(Vector3.Angle(m_StartForward, m_EndForward)) / 360f;
			if (m_fRotateTime < 0.005f)
			{
				m_bNeedRotate = false;
			}
			m_fStartTime = 0f;
		}
	}

	public class StandState : NpcState
	{
		public override void Contiune(iSniperNpc npc)
		{
			npc.CrossAnimation("Stand_Readyfire01", true);
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.updateRotation = false;
			npc.CrossAnimation("Stand_Readyfire01", true);
			npc.LookAtCamera();
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_fFireTime -= deltaTime;
			if (iSniperSaoShe2.m_fFireTime <= 0f)
			{
				iSniperSaoShe2.FIRESTATE.m_LastState = this;
				npc.SetState(iSniperSaoShe2.FIRESTATE);
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
			m_fAnimationTime = 0f;
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Stop(true);
			switch (m_DamagePart)
			{
			case Part.kHead:
				npc.PlayAnimation("Stand_Damage_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Damage_Head01");
				break;
			case Part.kBody:
				npc.PlayAnimation("Stand_Damage_Body01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Damage_Body01");
				break;
			case Part.kArmL:
			case Part.kLegL:
				npc.PlayAnimation("Stand_Damage_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Damage_LS01");
				break;
			case Part.kArmR:
			case Part.kLegR:
				npc.PlayAnimation("Stand_Damage_RS01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Damage_RS01");
				break;
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (!(m_fAnimationTime <= 0f))
			{
				return;
			}
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			if (npc.m_bWarning)
			{
				npc.ContinueState(m_LastState);
				return;
			}
			npc.m_bWarning = true;
			if (npc.m_PointScript.m_bCanMove2Player)
			{
				npc.SetState(iSniperSaoShe2.RANDOMRUSHSTATE);
			}
			else
			{
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
			iSniperSaoShe iSniperSaoShe2 = (iSniperSaoShe)npc;
			iSniperSaoShe2.m_NavAgent.Stop(true);
			switch (m_DamagePart)
			{
			case Part.kHead:
				npc.PlayAnimation("Stand_Death_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Death_Head01");
				break;
			case Part.kBody:
				npc.PlayAnimation("Stand_Death_Body01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Death_Body01");
				break;
			case Part.kArmL:
			case Part.kLegL:
				npc.PlayAnimation("Stand_Death_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Death_LS01");
				break;
			case Part.kArmR:
			case Part.kLegR:
				npc.PlayAnimation("Stand_Death_RS01", false);
				m_fAnimationTime = npc.AnimationLength("Stand_Death_RS01");
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

	public bool m_bReverseLR;

	public LeftRunState LEFTRUNSTATE = new LeftRunState();

	public RightRunState RIGHTRUNSTATE = new RightRunState();

	public StopStandState STOPSTANDSTATE = new StopStandState();

	public FireState FIRESTATE = new FireState();

	public RandomRushState RANDOMRUSHSTATE = new RandomRushState();

	public RushState RUSHSTATE = new RushState();

	public StandState STANDSTATE = new StandState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public UnityEngine.AI.NavMeshAgent m_NavAgent;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("SaoShe", strName, position);
		m_NavAgent = GameObject.Find(strName).AddComponent<UnityEngine.AI.NavMeshAgent>();
		m_bReverseLR = m_CameraScript.GetCameraPos().z < position.z;
		m_CurrentPose = Pose.kStand;
		m_fFireTime = Random.Range(m_Property.m_fMinNTime, m_Property.m_fMaxNTime);
		if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			SetState(LEFTRUNSTATE);
		}
		else
		{
			SetState(RIGHTRUNSTATE);
		}
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
		if (!m_bWarning)
		{
			base.OnListenGun();
			if (m_PointScript.m_bCanMove2Player)
			{
				SetState(RANDOMRUSHSTATE);
			}
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

	public Vector3 GetRandomLeftPoint()
	{
		Vector3 position = m_PointScript.m_Range[0].transform.position;
		Vector3 position2 = m_PointScript.m_Range[1].transform.position;
		return GetRandomPointBetweenTwoPoint(position, position2);
	}

	public Vector3 GetRandomRightPoint()
	{
		Vector3 position = m_PointScript.m_Range[2].transform.position;
		Vector3 position2 = m_PointScript.m_Range[3].transform.position;
		return GetRandomPointBetweenTwoPoint(position, position2);
	}
}
