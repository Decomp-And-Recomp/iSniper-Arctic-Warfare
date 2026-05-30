using UnityEngine;

public class iSniperJingJie : iSniperNpc
{
	public class NormalState : NpcState
	{
		private float m_fFireTime;

		public override void Enter(iSniperNpc npc)
		{
			if (npc.m_CurrentPose == Pose.kAim)
			{
				npc.RandomCrossAnimation("Aim_Watch01");
			}
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				npc.RandomCrossAnimation("Squat_Watch01");
			}
			m_fFireTime = Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fFireTime -= deltaTime;
			if (m_fFireTime <= 0f)
			{
				npc.Fire();
				m_fFireTime = Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
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
			if (!npc.m_bWarning)
			{
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
			else
			{
				m_bNeedRotate = false;
			}
			npc.m_bWarning = true;
			if (npc.m_CurrentPose == Pose.kAim)
			{
				npc.CrossAnimation("Aim_Readyfire01", true);
			}
			else if (npc.m_CurrentPose == Pose.kSquat)
			{
				npc.CrossAnimation("Squat_Readyfire01", true);
			}
			m_fFireTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			if (m_bNeedRotate)
			{
				m_fStartTime += deltaTime;
				float num = m_fStartTime / m_fRotateTime;
				npc.m_Model.transform.forward = Vector3.Lerp(m_StartForward, m_EndForward, num);
				if (num >= 1f)
				{
					m_bNeedRotate = false;
				}
			}
			m_fFireTime -= deltaTime;
			if (m_fFireTime <= 0f)
			{
				iSniperJingJie iSniperJingJie2 = (iSniperJingJie)npc;
				npc.SetState(iSniperJingJie2.FIRESTATE);
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
				iSniperJingJie iSniperJingJie2 = (iSniperJingJie)npc;
				npc.SetState(iSniperJingJie2.WARNINGSTATE);
			}
		}
	}

	public class DamageState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			if (npc.m_CurrentPose == Pose.kAim)
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
				iSniperJingJie iSniperJingJie2 = (iSniperJingJie)npc;
				npc.SetState(iSniperJingJie2.WARNINGSTATE);
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
			if (npc.m_CurrentPose == Pose.kAim)
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

	public NormalState NORMALSTATE = new NormalState();

	public WarningState WARNINGSTATE = new WarningState();

	public FireState FIRESTATE = new FireState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("JingJie", strName, position);
		LookAtCamera();
		if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			m_CurrentPose = Pose.kAim;
		}
		else
		{
			m_CurrentPose = Pose.kSquat;
		}
		SetState(NORMALSTATE);
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
		if (!m_bWarning)
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
