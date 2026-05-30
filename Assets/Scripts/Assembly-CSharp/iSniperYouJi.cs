using UnityEngine;

public class iSniperYouJi : iSniperNpc
{
	public class WatchState : NpcState
	{
		private float m_fFireTime;

		public override void Enter(iSniperNpc npc)
		{
			if (npc.m_StartPose == Pose.kAim)
			{
				npc.m_CurrentPose = Pose.kAim;
				npc.RandomCrossAnimation("Aim_Watch01");
			}
			else if (npc.m_StartPose == Pose.kSquat)
			{
				npc.m_CurrentPose = Pose.kSquat;
				npc.RandomCrossAnimation("Squat_Watch01");
			}
			m_fFireTime = Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fFireTime -= deltaTime;
			if (npc.IsHideInObstacle() || npc.IsOutScreen())
			{
				iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
				npc.SetState(iSniperYouJi2.WARNINGSTATE);
				iSniperYouJi2.WARNINGSTATE.SetRushTime(true);
			}
			if (m_fFireTime <= 0f)
			{
				npc.Fire();
				m_fFireTime = Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
			}
		}
	}

	public class WarngingState : NpcState
	{
		private float m_fFireTime;

		private bool m_bTimeRush;

		private float m_fRushTimer;

		public void SetRushTime(bool bRush)
		{
			if (bRush)
			{
				if (m_bTimeRush)
				{
					if (m_fRushTimer <= 0f)
					{
						m_fRushTimer = 1f;
					}
				}
				else
				{
					m_fRushTimer = 1f;
				}
			}
			else
			{
				m_fRushTimer = -1f;
			}
			m_bTimeRush = bRush;
		}

		public override void Enter(iSniperNpc npc)
		{
			if (npc.m_StartPose == Pose.kAim)
			{
				npc.m_CurrentPose = Pose.kAim;
				npc.CrossAnimation("Aim_Readyfire01", true);
			}
			else if (npc.m_StartPose == Pose.kSquat)
			{
				npc.m_CurrentPose = Pose.kSquat;
				npc.CrossAnimation("Squat_Readyfire01", true);
			}
			m_fFireTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fFireTime -= deltaTime;
			if (m_fFireTime <= 0f)
			{
				iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
				npc.SetState(iSniperYouJi2.FIRESTATE);
			}
			if (npc.IsHideInObstacle() || npc.IsOutScreen())
			{
				m_bTimeRush = true;
			}
			if (!m_bTimeRush)
			{
				return;
			}
			m_fRushTimer -= deltaTime;
			if (m_fRushTimer <= 0f)
			{
				m_bTimeRush = false;
				iSniperYouJi iSniperYouJi3 = (iSniperYouJi)npc;
				iSniperYouJi3.RUSHSTATE.m_StartPosition = npc.GetPosition();
				iSniperYouJi3.RUSHSTATE.m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
				Vector3 point = npc.CheckGround(iSniperYouJi3.RUSHSTATE.m_EndPosition);
				point.y -= 0.5f;
				while (npc.m_CameraScript.IsOutScreen(point))
				{
					iSniperYouJi3.RUSHSTATE.m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
					point = npc.CheckGround(iSniperYouJi3.RUSHSTATE.m_EndPosition);
					point.y -= 0.5f;
				}
				float num = Vector3.Distance(iSniperYouJi3.RUSHSTATE.m_StartPosition, iSniperYouJi3.RUSHSTATE.m_EndPosition);
				if (num >= 2.4f)
				{
					npc.SetState(iSniperYouJi3.RUSHSTATE);
				}
			}
		}
	}

	public class FireState : NpcState
	{
		private float m_fAnimaitonTime;

		public override void Enter(iSniperNpc npc)
		{
			m_fAnimaitonTime = 0f;
			if (npc.m_CurrentPose == Pose.kAim)
			{
				npc.PlayAnimation("Aim_Fire01", false);
				m_fAnimaitonTime = npc.AnimationLength("Aim_Fire01");
			}
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				npc.PlayAnimation("Squat_Fire01", false);
				m_fAnimaitonTime = npc.AnimationLength("Squat_Fire01");
			}
			npc.Fire();
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimaitonTime -= deltaTime;
			if (m_fAnimaitonTime <= 0f)
			{
				iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
				npc.SetState(iSniperYouJi2.WARNINGSTATE);
			}
		}
	}

	public class RushState : NpcState
	{
		public Vector3 m_StartPosition;

		public Vector3 m_EndPosition;

		private float m_fRushTime;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		private int m_iState;

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Rush01", true);
			npc.m_CurrentPose = Pose.kStandby;
			m_iState = 1;
			iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
			iSniperYouJi2.m_NavAgent.enabled = true;
			iSniperYouJi2.m_NavAgent.Resume();
			iSniperYouJi2.m_NavAgent.SetDestination(m_EndPosition);
			iSniperYouJi2.m_NavAgent.Resume();
			iSniperYouJi2.m_NavAgent.speed = 4f;
			iSniperYouJi2.m_NavAgent.acceleration = 15f;
			iSniperYouJi2.m_NavAgent.angularSpeed = 300f;
			iSniperYouJi2.m_NavAgent.updateRotation = true;
			m_fStartTime = 0f;
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			switch (m_iState)
			{
			case 1:
			{
				iSniperYouJi iSniperYouJi3 = (iSniperYouJi)npc;
				if (!(iSniperYouJi3.m_NavAgent.remainingDistance <= 0.15f) || iSniperYouJi3.m_NavAgent.pathPending)
				{
					break;
				}
				m_fStartTime = 0f;
				iSniperYouJi3.m_NavAgent.Stop();
				iSniperYouJi3.m_NavAgent.updateRotation = false;
				npc.RandomCrossAnimation("Aim_Watch01");
				m_StartForward = npc.m_Model.transform.forward;
				Vector3 position = npc.GetPosition();
				Vector3 cameraPos = npc.m_CameraScript.GetCameraPos();
				cameraPos.y = position.y;
				m_EndForward = (cameraPos - position).normalized;
				m_fRotateTime = Mathf.Abs(Vector3.Angle(m_StartForward, m_EndForward)) / 300f;
				if (m_fRotateTime < 0.005f)
				{
					if (npc.m_bWarning)
					{
						iSniperYouJi3.WARNINGSTATE.SetRushTime(false);
						npc.SetState(iSniperYouJi3.WARNINGSTATE);
					}
					else
					{
						npc.SetState(iSniperYouJi3.WATCHSTATE);
					}
				}
				else
				{
					m_iState = 2;
				}
				break;
			}
			case 2:
			{
				m_fStartTime += deltaTime;
				float num = m_fStartTime / m_fRotateTime;
				npc.m_Model.transform.forward = Vector3.Lerp(m_StartForward, m_EndForward, num);
				if (num >= 1f)
				{
					iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
					if (npc.m_bWarning)
					{
						iSniperYouJi2.WARNINGSTATE.SetRushTime(false);
						npc.SetState(iSniperYouJi2.WARNINGSTATE);
					}
					else
					{
						npc.SetState(iSniperYouJi2.WATCHSTATE);
					}
				}
				break;
			}
			}
		}
	}

	public class DamageState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.m_bWarning = true;
			m_fAnimationTime = 0f;
			iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
			iSniperYouJi2.m_NavAgent.Stop(true);
			npc.LookAtCamera();
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
			else if (npc.m_CurrentPose == Pose.kAim)
			{
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
				iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
				iSniperYouJi2.WARNINGSTATE.SetRushTime(true);
				npc.SetState(iSniperYouJi2.WARNINGSTATE);
			}
		}
	}

	public class DeadState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYouJi iSniperYouJi2 = (iSniperYouJi)npc;
			iSniperYouJi2.m_NavAgent.Stop(true);
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
			else if (npc.m_CurrentPose == Pose.kAim)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					if (Utils.ProbabilityIsRandomHit(0.5f))
					{
						npc.PlayAnimation("Aim_Death_Head01", false);
						m_fAnimationTime = npc.AnimationLength("Aim_Death_Head01");
					}
					else
					{
						npc.PlayAnimation("Aim_Death_Head02", false);
						m_fAnimationTime = npc.AnimationLength("Aim_Death_Head02");
					}
					break;
				case Part.kBody:
				{
					int num = Random.Range(1, 4);
					npc.PlayAnimation("Aim_Death_Body0" + num, false);
					m_fAnimationTime = npc.AnimationLength("Aim_Death_Body0" + num);
					break;
				}
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
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				switch (m_DamagePart)
				{
				case Part.kHead:
					npc.PlayAnimation("Squat_Death_Head01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Death_Head01");
					break;
				case Part.kBody:
					npc.PlayAnimation("Squat_Death_Body01", false);
					m_fAnimationTime = npc.AnimationLength("Squat_Death_Body01");
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

	public WatchState WATCHSTATE = new WatchState();

	public RushState RUSHSTATE = new RushState();

	public WarngingState WARNINGSTATE = new WarngingState();

	public FireState FIRESTATE = new FireState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public UnityEngine.AI.NavMeshAgent m_NavAgent;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("YouJi", strName, position);
		LookAtCamera();
		m_NavAgent = GameObject.Find(strName).AddComponent<UnityEngine.AI.NavMeshAgent>();
		if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			m_StartPose = Pose.kAim;
		}
		else
		{
			m_StartPose = Pose.kSquat;
		}
		SetPosition(GetRandomPoint(m_PointScript.m_fMinDistance, m_PointScript.m_fMaxDistance));
		RUSHSTATE.m_StartPosition = GetPosition();
		RUSHSTATE.m_EndPosition = GetRandomPoint(m_PointScript.m_fMinDistance, m_PointScript.m_fMaxDistance);
		float num = Vector3.Distance(RUSHSTATE.m_StartPosition, RUSHSTATE.m_EndPosition);
		if (num >= 2.4f)
		{
			SetState(RUSHSTATE);
		}
		else
		{
			SetState(WATCHSTATE);
		}
	}

	public override void OnListenGun()
	{
		base.OnListenGun();
		if (WATCHSTATE == m_State)
		{
			WARNINGSTATE.SetRushTime(Utils.ProbabilityIsRandomHit(0.5f));
			SetState(WARNINGSTATE);
		}
		else
		{
			WARNINGSTATE.SetRushTime(true);
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
