using UnityEngine;

public class iSniperJuJi : iSniperNpc
{
	public class NormalState : NpcState
	{
		private float m_fFireTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Grovel_Readyfire01", true);
			if (npc.m_bWarning)
			{
				m_fFireTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
			}
			else
			{
				m_fFireTime = Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fFireTime -= deltaTime;
			if (m_fFireTime <= 0f)
			{
				iSniperJuJi iSniperJuJi2 = (iSniperJuJi)npc;
				npc.SetState(iSniperJuJi2.FIRESTATE);
			}
		}
	}

	public class FireState : NpcState
	{
		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.PlayAnimation("Grovel_Fire01", false);
			m_fAnimationTime = npc.AnimationLength("Grovel_Fire01");
			npc.Fire();
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperJuJi iSniperJuJi2 = (iSniperJuJi)npc;
				npc.SetState(iSniperJuJi2.NORMALSTATE);
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
			switch (m_DamagePart)
			{
			case Part.kHead:
				npc.PlayAnimation("Grovel_Damage_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Grovel_Damage_Head01");
				break;
			case Part.kBody:
				npc.PlayAnimation("Grovel_Damage_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Grovel_Damage_Head01");
				break;
			case Part.kArmL:
			case Part.kLegL:
				npc.PlayAnimation("Grovel_Damage_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Grovel_Damage_LS01");
				break;
			case Part.kArmR:
			case Part.kLegR:
				npc.PlayAnimation("Grovel_Damage_RS01", false);
				m_fAnimationTime = npc.AnimationLength("Grovel_Damage_RS01");
				break;
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				iSniperJuJi iSniperJuJi2 = (iSniperJuJi)npc;
				npc.SetState(iSniperJuJi2.NORMALSTATE);
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
			switch (m_DamagePart)
			{
			case Part.kHead:
			case Part.kBody:
				npc.PlayAnimation("Grovel_Death_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Grovel_Death_Head01");
				break;
			case Part.kArmL:
			case Part.kArmR:
			case Part.kLegL:
			case Part.kLegR:
				npc.PlayAnimation("Grovel_Death_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Grovel_Death_LS01");
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

	public NormalState NORMALSTATE = new NormalState();

	public FireState FIRESTATE = new FireState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("JuJi", strName, position);
		LookAtCamera();
		m_CurrentPose = Pose.kGrovel;
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

	public override bool IsDead()
	{
		if (base.IsDead())
		{
			return true;
		}
		return DEADSTATE == m_State;
	}
}
