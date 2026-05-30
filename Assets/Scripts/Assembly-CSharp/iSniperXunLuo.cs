using UnityEngine;

public class iSniperXunLuo : iSniperNpc
{
	public class WalkState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fWalkTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckime;

		public override void Enter(iSniperNpc npc)
		{
			if (Utils.ProbabilityIsRandomHit(0.5f))
			{
				npc.CrossAnimation("Standby_Watch_Forward01", true);
			}
			else
			{
				npc.CrossAnimation("Standby_Forward01", true);
			}
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetRandomPoint(npc.m_PointScript.m_fMinDistance, npc.m_PointScript.m_fMaxDistance);
			m_fWalkTime = Vector3.Distance(m_StartPosition, m_EndPosition) / 1f;
			m_bNeedRotate = true;
			m_StartForward = npc.m_Model.transform.forward;
			Vector3 endPosition = m_EndPosition;
			endPosition.y = m_StartPosition.y;
			m_EndForward = (endPosition - m_StartPosition).normalized;
			m_fRotateTime = Mathf.Abs(Vector3.Angle(m_StartForward, m_EndForward)) / 360f;
			if (m_fRotateTime < 0.005f)
			{
				m_bNeedRotate = false;
			}
			m_fStartTime = 0f;
			m_fCheckime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			m_fCheckime += deltaTime;
			if (m_bNeedRotate)
			{
				float num = m_fStartTime / m_fRotateTime;
				npc.m_Model.transform.forward = Vector3.Slerp(m_StartForward, m_EndForward, num);
				if (num >= 1f)
				{
					m_bNeedRotate = false;
				}
			}
			Vector3 position = Vector3.Lerp(m_StartPosition, m_EndPosition, m_fStartTime / m_fWalkTime);
			if (m_fCheckime >= 0f)
			{
				m_fCheckime = 0f;
				position.y = npc.CheckGround() + 0.01f;
				if (!m_bNeedRotate)
				{
					npc.m_bNeedBackPoint = npc.CheckObstacle();
				}
			}
			else
			{
				position.y = npc.m_Model.transform.position.y;
			}
			npc.m_Model.transform.position = position;
			if (m_fStartTime >= Mathf.Max(m_fWalkTime, m_fRotateTime) || npc.m_bNeedBackPoint)
			{
				iSniperXunLuo iSniperXunLuo2 = (iSniperXunLuo)npc;
				npc.SetState(iSniperXunLuo2.WATCHSTATE);
			}
		}
	}

	public class WatchState : NpcState
	{
		private float m_fMaxWatchTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Standby_Watch01", true);
			m_fMaxWatchTime = Random.Range(2f, 6f);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fMaxWatchTime -= deltaTime;
			if (m_fMaxWatchTime <= 0f)
			{
				iSniperXunLuo iSniperXunLuo2 = (iSniperXunLuo)npc;
				npc.SetState(iSniperXunLuo2.WALKSTATE);
			}
		}
	}

	public class WarningState : NpcState
	{
		private float m_fFireTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		public override void Enter(iSniperNpc npc)
		{
			if (npc.m_bWarning)
			{
				m_bNeedRotate = false;
				if (npc.m_CurrentPose == Pose.kAim)
				{
					npc.m_CurrentPose = Pose.kAim;
					npc.RandomCrossAnimation("Aim_Watch01");
				}
				else if (npc.m_CurrentPose == Pose.kSquat)
				{
					npc.m_CurrentPose = Pose.kSquat;
					npc.RandomCrossAnimation("Squat_Watch01");
				}
			}
			else
			{
				npc.m_bWarning = true;
				if (Utils.ProbabilityIsRandomHit(0.5f))
				{
					npc.m_CurrentPose = Pose.kAim;
					npc.RandomCrossAnimation("Aim_Watch01");
				}
				else
				{
					npc.m_CurrentPose = Pose.kSquat;
					npc.RandomCrossAnimation("Squat_Watch01");
				}
				m_bNeedRotate = true;
				m_StartForward = npc.m_Model.transform.forward;
				Vector3 position = npc.GetPosition();
				Vector3 cameraPos = npc.m_CameraScript.GetCameraPos();
				cameraPos.y = position.y;
				m_EndForward = (cameraPos - position).normalized;
				m_fRotateTime = Mathf.Abs(Vector3.Angle(m_StartForward, m_EndForward)) / 360f;
				if (m_fRotateTime < 0.005f)
				{
					m_bNeedRotate = false;
				}
				m_fStartTime = 0f;
			}
			m_fFireTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			if (m_bNeedRotate)
			{
				m_fStartTime += deltaTime;
				float num = m_fStartTime / m_fRotateTime;
				npc.m_Model.transform.forward = Vector3.Slerp(m_StartForward, m_EndForward, num);
				if (num >= 1f)
				{
					m_bNeedRotate = false;
				}
			}
			m_fFireTime -= deltaTime;
			if (m_fFireTime <= 0f)
			{
				npc.Fire();
				m_fFireTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
			}
		}
	}

	public class DamageState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
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
				iSniperXunLuo iSniperXunLuo2 = (iSniperXunLuo)npc;
				npc.SetState(iSniperXunLuo2.WARNINGSTATE);
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

	public WalkState WALKSTATE = new WalkState();

	public WatchState WATCHSTATE = new WatchState();

	public WarningState WARNINGSTATE = new WarningState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("XunLuo", strName, position);
		SetPosition(GetRandomPoint(0f, m_PointScript.m_fMinDistance));
		if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			SetState(WATCHSTATE);
		}
		else
		{
			SetState(WALKSTATE);
		}
		m_CurrentPose = Pose.kStandby;
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

	public override void OnListenGun()
	{
		if (!m_bWarning && !IsHideInObstacle())
		{
			SetState(WARNINGSTATE);
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
