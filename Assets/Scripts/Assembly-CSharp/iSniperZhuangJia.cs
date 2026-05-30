using UnityEngine;

public class iSniperZhuangJia : iSniperNpc
{
	public class WalkState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fWalkTime;

		private float m_fCheckTime;

		private float m_fStartTime;

		private float m_fTime;

		public override void Contiune(iSniperNpc npc)
		{
			npc.RandomCrossAnimation("Heavy01_Forward01");
		}

		public override void Enter(iSniperNpc npc)
		{
			npc.RandomCrossAnimation("Heavy01_Forward01");
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.m_PointScript.m_Path[0].transform.position;
			iSniperZhuangJia iSniperZhuangJia2 = (iSniperZhuangJia)npc;
			iSniperZhuangJia2.m_NavAgent.SetDestination(m_EndPosition);
			iSniperZhuangJia2.m_NavAgent.Resume();
			iSniperZhuangJia2.m_NavAgent.speed = 0.9f;
			iSniperZhuangJia2.m_NavAgent.angularSpeed = 360f;
			iSniperZhuangJia2.m_NavAgent.acceleration = 25f;
			m_fTime = 1f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperZhuangJia iSniperZhuangJia2 = (iSniperZhuangJia)npc;
			iSniperZhuangJia2.m_fFireTime -= deltaTime;
			m_fTime -= deltaTime;
			if (m_fTime <= 0f)
			{
				npc.FireEffect();
				m_fTime = 1f;
			}
			if (iSniperZhuangJia2.m_fFireTime <= 0f)
			{
				iSniperZhuangJia2.ResetFireTime();
				npc.PlayMixAnimation("Heavy01_Fire01");
				npc.Fire();
				m_fTime = 1f;
			}
			npc.LookAtCamera();
			if (iSniperZhuangJia2.m_NavAgent.remainingDistance < 0.15f && !iSniperZhuangJia2.m_NavAgent.pathPending)
			{
				npc.SetState(iSniperZhuangJia2.STOPSTATE);
			}
		}
	}

	public class StopState : NpcState
	{
		private float m_fTime;

		public override void Contiune(iSniperNpc npc)
		{
			npc.CrossAnimation("Heavy01_Readyfire01", true);
			npc.LookAtCamera();
		}

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Heavy01_Readyfire01", true);
			npc.LookAtCamera();
			m_fTime = 1f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperZhuangJia iSniperZhuangJia2 = (iSniperZhuangJia)npc;
			iSniperZhuangJia2.m_fFireTime -= deltaTime;
			npc.LookAtCamera();
			m_fTime -= deltaTime;
			if (m_fTime <= 0f)
			{
				npc.FireEffect();
				m_fTime = 1f;
			}
			if (iSniperZhuangJia2.m_fFireTime <= 0f)
			{
				iSniperZhuangJia2.ResetFireTime();
				npc.PlayMixAnimation("Heavy01_Fire01");
				npc.Fire();
				m_fTime = 1f;
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
			GameObject gameObject = Object.Instantiate(npc.g_EffectsConfig.m_BodyExplosionEffect, npc.m_HeadObj.transform.position, Quaternion.identity) as GameObject;
			gameObject.transform.up = npc.m_Model.transform.position.normalized;
			npc.m_GameScene.PlaySoundAtPos(npc.m_GameScene.m_SoundExpMoto, npc.m_Model.transform.position);
			iSniperZhuangJia iSniperZhuangJia2 = (iSniperZhuangJia)npc;
			iSniperZhuangJia2.m_NavAgent.Stop(true);
			switch (m_DamagePart)
			{
			case Part.kHead:
				npc.PlayAnimation("Heavy01_Death_Head01", false);
				m_fAnimationTime = npc.AnimationLength("Heavy01_Death_Head01");
				break;
			case Part.kBody:
				npc.PlayAnimation("Heavy01_Death_Body01", false);
				m_fAnimationTime = npc.AnimationLength("Heavy01_Death_Body01");
				break;
			case Part.kArmL:
			case Part.kArmR:
			case Part.kLegL:
			case Part.kLegR:
				npc.PlayAnimation("Heavy01_Death_LS01", false);
				m_fAnimationTime = npc.AnimationLength("Heavy01_Death_LS01");
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

	public WalkState WALKSTATE = new WalkState();

	public StopState STOPSTATE = new StopState();

	public DeadState DEADSTATE = new DeadState();

	public UnityEngine.AI.NavMeshAgent m_NavAgent;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("ZhuangJia", strName, position);
		m_NavAgent = GameObject.Find(strName).AddComponent<UnityEngine.AI.NavMeshAgent>();
		Transform mix = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		m_Model.GetComponent<Animation>()["Heavy01_Fire01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Heavy01_Fire01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Heavy01_Fire01"].layer = 1;
		m_Model.GetComponent<Animation>()["Heavy01_Fire01"].AddMixingTransform(mix);
		ResetFireTime();
		SetState(WALKSTATE);
	}

	public override void OnHit(Part part)
	{
		if (m_iCurrentHealth <= 0)
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
