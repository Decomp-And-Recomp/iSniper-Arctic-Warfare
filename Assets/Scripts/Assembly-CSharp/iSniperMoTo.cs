using System;
using UnityEngine;

public class iSniperMoTo : iSniperNpc
{
	public class First03State : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fMoToTime;

		private float m_fAnimationTime;

		public bool m_bRight;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			Vector3 position = npc.m_PointScript.m_Range[0].transform.position;
			Vector3 position2 = npc.m_PointScript.m_Range[3].transform.position;
			position.y = 0f;
			position2.y = 0f;
			npc.m_Model.transform.forward = (position2 - position).normalized;
			if (position.x < position2.x)
			{
				npc.CrossAnimation("Moto_TurnRightFire01", false);
				m_fAnimationTime = npc.AnimationLength("Moto_TurnRightFire01");
				m_bRight = true;
			}
			else
			{
				npc.CrossAnimation("Moto_TurnLeftFire01", false);
				m_fAnimationTime = npc.AnimationLength("Moto_TurnLeftFire01");
				m_bRight = false;
			}
			m_StartPosition = npc.CheckGround(position);
			m_EndPosition = npc.CheckGround(position2);
			m_fMoToTime = Vector3.Distance(m_StartPosition, m_EndPosition) / 4.5f;
			m_fCheckTime = 0f;
			m_fStartTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			iSniperMoTo2.m_fFireTime -= deltaTime;
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				if (m_bRight)
				{
					npc.CrossAnimation("Moto_Right_ReadyFire01", true);
				}
				else
				{
					npc.CrossAnimation("Moto_Left_ReadyFire01", true);
				}
			}
			if (iSniperMoTo2.m_fFireTime <= 0f)
			{
				iSniperMoTo2.ResetFireTime();
				if (m_bRight)
				{
					npc.PlayMixAnimation("Moto_Right_Fire01");
				}
				else
				{
					npc.PlayMixAnimation("Moto_Left_Fire01");
				}
				npc.Fire();
			}
			m_fStartTime += deltaTime;
			m_fCheckTime += deltaTime;
			Vector3 position = Vector3.Lerp(m_StartPosition, m_EndPosition, m_fStartTime / m_fMoToTime);
			if (m_fCheckTime >= iSniperMoTo2.m_fCheckRatio)
			{
				m_fCheckTime = 0f;
				position.y = npc.CheckGround() + 0.01f;
			}
			else
			{
				position.y = npc.m_Model.transform.position.y;
			}
			npc.m_Model.transform.position = position;
			if (m_fStartTime >= m_fMoToTime)
			{
				iSniperMoTo2.SECOND32STATE.m_bRight = m_bRight;
				npc.SetState(iSniperMoTo2.SECOND32STATE);
			}
		}
	}

	public class Second32State : NpcState
	{
		public bool m_bRight;

		private float m_fRadius;

		private Vector3 m_Center;

		private float m_fTurnTime;

		private float m_fStartTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			npc.CrossAnimation("Moto_TurnIdle01", false);
			Vector3 position = npc.m_PointScript.m_Range[3].transform.position;
			Vector3 position2 = npc.m_PointScript.m_Range[2].transform.position;
			position.y = 0f;
			position2.y = 0f;
			m_Center = (position + position2) / 2f;
			m_fRadius = Vector3.Distance(position, position2) / 2f;
			m_fTurnTime = (float)Math.PI * m_fRadius / 4f;
			m_fStartTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			float f = Mathf.Lerp((float)Math.PI, 0f, m_fStartTime / m_fTurnTime);
			float num = Mathf.Sin(f) * m_fRadius;
			float z = Mathf.Cos(f) * m_fRadius;
			if (!m_bRight)
			{
				num = 0f - num;
			}
			Vector3 vector = m_Center + new Vector3(num, 0f, z);
			Vector3 position = npc.GetPosition();
			position.y = 0f;
			npc.m_Model.transform.forward = (vector - position).normalized;
			npc.m_Model.transform.position = npc.CheckGround(vector);
			if (m_fStartTime >= m_fTurnTime)
			{
				iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
				npc.SetState(iSniperMoTo2.THIRD21STATE);
			}
		}
	}

	public class Third21State : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fMoToTime;

		private float m_fAnimationTime;

		public bool m_bRight;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			Vector3 position = npc.m_PointScript.m_Range[2].transform.position;
			Vector3 position2 = npc.m_PointScript.m_Range[1].transform.position;
			position.y = 0f;
			position2.y = 0f;
			npc.m_Model.transform.forward = (position2 - position).normalized;
			if (position.x < position2.x)
			{
				npc.CrossAnimation("Moto_TurnRightFire01", false);
				m_fAnimationTime = npc.AnimationLength("Moto_TurnRightFire01");
				m_bRight = true;
			}
			else
			{
				npc.CrossAnimation("Moto_TurnLeftFire01", false);
				m_fAnimationTime = npc.AnimationLength("Moto_TurnLeftFire01");
				m_bRight = false;
			}
			m_StartPosition = npc.CheckGround(position);
			m_EndPosition = npc.CheckGround(position2);
			m_fMoToTime = Vector3.Distance(m_StartPosition, m_EndPosition) / 4.5f;
			m_fCheckTime = 0f;
			m_fStartTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			iSniperMoTo2.m_fFireTime -= deltaTime;
			m_fAnimationTime -= deltaTime;
			if (m_fAnimationTime <= 0f)
			{
				if (m_bRight)
				{
					npc.CrossAnimation("Moto_Right_ReadyFire01", true);
				}
				else
				{
					npc.CrossAnimation("Moto_Left_ReadyFire01", true);
				}
			}
			if (iSniperMoTo2.m_fFireTime <= 0f)
			{
				iSniperMoTo2.ResetFireTime();
				if (m_bRight)
				{
					npc.PlayMixAnimation("Moto_Right_Fire01");
				}
				else
				{
					npc.PlayMixAnimation("Moto_Left_Fire01");
				}
				npc.Fire();
			}
			m_fStartTime += deltaTime;
			m_fCheckTime += deltaTime;
			Vector3 position = Vector3.Lerp(m_StartPosition, m_EndPosition, m_fStartTime / m_fMoToTime);
			if (m_fCheckTime >= iSniperMoTo2.m_fCheckRatio)
			{
				m_fCheckTime = 0f;
				position.y = npc.CheckGround() + 0.01f;
			}
			else
			{
				position.y = npc.m_Model.transform.position.y;
			}
			npc.m_Model.transform.position = position;
			if (m_fStartTime >= m_fMoToTime)
			{
				iSniperMoTo2.FORTH10STATE.m_bRight = m_bRight;
				npc.SetState(iSniperMoTo2.FORTH10STATE);
			}
		}
	}

	public class Forth10State : NpcState
	{
		public bool m_bRight;

		private float m_fRadius;

		private Vector3 m_Center;

		private float m_fTurnTime;

		private float m_fStartTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			npc.CrossAnimation("Moto_TurnIdle01", false);
			Vector3 position = npc.m_PointScript.m_Range[1].transform.position;
			Vector3 position2 = npc.m_PointScript.m_Range[0].transform.position;
			position.y = 0f;
			position2.y = 0f;
			m_Center = (position + position2) / 2f;
			m_fRadius = Vector3.Distance(position, position2) / 2f;
			m_fTurnTime = (float)Math.PI * m_fRadius / 4f;
			m_fStartTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			float f = Mathf.Lerp(0f, (float)Math.PI, m_fStartTime / m_fTurnTime);
			float num = Mathf.Sin(f) * m_fRadius;
			float z = Mathf.Cos(f) * m_fRadius;
			if (!m_bRight)
			{
				num = 0f - num;
			}
			Vector3 vector = m_Center + new Vector3(num, 0f, z);
			Vector3 position = npc.GetPosition();
			position.y = 0f;
			npc.m_Model.transform.forward = (vector - position).normalized;
			npc.m_Model.transform.position = npc.CheckGround(vector);
			if (m_fStartTime >= m_fTurnTime)
			{
				iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
				npc.SetState(iSniperMoTo2.FIRST03STATE);
			}
		}
	}

	public class RushState : NpcState
	{
		private int m_iPathIndex;

		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fMoToTime;

		private bool m_bNeedRotate;

		private Vector3 m_StartForward;

		private Vector3 m_EndForward;

		private float m_fRotateTime;

		private float m_fStartTime;

		private float m_fCheckTime;

		public override void Contiune(iSniperNpc npc)
		{
			npc.CrossAnimation("Moto_TurnReadyFire01", true);
		}

		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Moto_TurnReadyFire01", true);
			m_iPathIndex = 0;
			RecomputePath(npc);
			m_fCheckTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
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
			Vector3 position = Vector3.Lerp(m_StartPosition, m_EndPosition, m_fStartTime / m_fMoToTime);
			if (m_fCheckTime >= iSniperMoTo2.m_fCheckRatio)
			{
				m_fCheckTime = 0f;
				position.y = npc.CheckGround() + 0.01f;
			}
			else
			{
				position.y = npc.m_Model.transform.position.y;
			}
			npc.m_Model.transform.position = position;
			if (m_fStartTime >= m_fMoToTime)
			{
				m_iPathIndex++;
				if (m_iPathIndex >= npc.m_PointScript.m_Path.Length)
				{
					npc.SetState(iSniperMoTo2.STOPSTATE);
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
			m_fMoToTime = Vector3.Distance(startPosition, vector) / 4.5f;
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

	public class StopState : NpcState
	{
		public override void Enter(iSniperNpc npc)
		{
			npc.CrossAnimation("Moto_ReadyFire01", true);
			npc.LookAtCamera();
			npc.m_SoundMoTo1.Stop();
			npc.m_SoundMoTo2.Stop();
			string name = npc.m_Model.name + "/xuediche/xuediche_01";
			ParticleSystem component = GameObject.Find(name).GetComponent<ParticleSystem>();
			component.Stop(true);
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperMoTo iSniperMoTo2 = (iSniperMoTo)npc;
			iSniperMoTo2.m_fFireTime -= deltaTime;
			if (iSniperMoTo2.m_fFireTime <= 0f)
			{
				iSniperMoTo2.ResetFireTime();
				npc.PlayMixAnimation("Moto_Fire01");
				npc.Fire();
			}
		}
	}

	public class DeadState : NpcState
	{
		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.ShowMark(false);
			npc.DestroyObserver();
			npc.m_SoundMoTo1.Stop();
			npc.m_SoundMoTo2.Stop();
			npc.PlayAnimation("Moto_Death01", false);
			m_fAnimationTime = npc.AnimationLength("Moto_Death01");
			string name = npc.m_Model.name + "/xuediche/xuediche_01";
			ParticleSystem component = GameObject.Find(name).GetComponent<ParticleSystem>();
			component.Stop(true);
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

	public int m_iMoToHealth;

	private GameObject m_MoToObj;

	private float m_fCheckRatio = 0.01f;

	public First03State FIRST03STATE = new First03State();

	public Second32State SECOND32STATE = new Second32State();

	public Third21State THIRD21STATE = new Third21State();

	public Forth10State FORTH10STATE = new Forth10State();

	public RushState RUSHSTATE = new RushState();

	public StopState STOPSTATE = new StopState();

	public DeadState DEADSTATE = new DeadState();

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("MoTo", strName, position);
		m_ShadowObj.GetComponent<iSniperShadow>().m_Size = new Vector2(1f, 2f);
		m_iMoToHealth = m_Property.m_iMachineHealth;
		m_MoToObj = m_Model.transform.Find("Bip01/MoToCar_Prefab").gameObject;
		Transform mix = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		m_Model.GetComponent<Animation>()["Moto_Right_Fire01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Right_Fire01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Right_Fire01"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Right_Fire01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["Moto_Right_Damage01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Right_Damage01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Right_Damage01"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Right_Damage01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["Moto_Left_Fire01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Left_Fire01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Left_Fire01"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Left_Fire01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["Moto_Left_Damage01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Left_Damage01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Left_Damage01"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Left_Damage01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["Moto_Fire01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Fire01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Fire01"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Fire01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["Moto_Damage01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Damage01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Damage01"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Damage01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["Moto_Damage02"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["Moto_Damage02"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["Moto_Damage02"].layer = 1;
		m_Model.GetComponent<Animation>()["Moto_Damage02"].AddMixingTransform(mix);
		ResetFireTime();
		SetState(FIRST03STATE);
	}

	public override void OnHit(Part part)
	{
		if (m_iCurrentHealth > 0)
		{
			if (FIRST03STATE == m_State)
			{
				if (FIRST03STATE.m_bRight)
				{
					PlayMixAnimation("Moto_Right_Damage01");
				}
				else
				{
					PlayMixAnimation("Moto_Left_Damage01");
				}
			}
			else if (THIRD21STATE == m_State)
			{
				if (FIRST03STATE.m_bRight)
				{
					PlayMixAnimation("Moto_Right_Damage01");
				}
				else
				{
					PlayMixAnimation("Moto_Left_Damage01");
				}
			}
			else if (STOPSTATE == m_State)
			{
				PlayMixAnimation("Moto_Damage02");
			}
			else
			{
				PlayMixAnimation("Moto_Damage01");
			}
			if (!m_bWarning)
			{
				m_bWarning = true;
				if (m_PointScript.m_bCanMove2Player)
				{
					SetState(RUSHSTATE);
				}
			}
		}
		else
		{
			SetState(DEADSTATE);
		}
	}

	public override void OnHitMachine(RaycastHit hit)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(g_EffectsConfig.m_AttackIronEffect, hit.point, Quaternion.identity) as GameObject;
		iSniperGunProperty useGunProperty = m_GameState.GetUseGunProperty();
		if ("Mount" == hit.collider.name)
		{
			int num = useGunProperty.GetCurrentHarm() - (m_Property.m_iAHead + 20);
			if (num < 0)
			{
				num = 0;
			}
			m_iMoToHealth -= num;
		}
		else if ("Moto" == hit.collider.name)
		{
			int num2 = useGunProperty.GetCurrentHarm() - (m_Property.m_iABody + 40);
			if (num2 < 0)
			{
				num2 = 0;
			}
			m_iMoToHealth -= num2;
		}
		if (m_iMoToHealth <= 0)
		{
			Vector3 position = m_MoToObj.transform.Find("Mount").position;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(g_EffectsConfig.m_CarExplosionEffect, position, Quaternion.identity) as GameObject;
			gameObject2.AddComponent<iSniperExplosionCheck>();
			m_iMoToHealth = 0;
			m_iCurrentHealth = 0;
			if ("Mount" == hit.collider.name)
			{
				m_UIHelper.ShowTitleShotTip("cratical");
			}
			else
			{
				m_UIHelper.ShowTitleShotTip("normal");
			}
			SetState(DEADSTATE);
		}
		else if (!m_bWarning && m_PointScript.m_bCanMove2Player)
		{
			SetState(RUSHSTATE);
		}
		m_bWarning = true;
		m_UIHelper.SetEnemyBlood(m_iMoToHealth, m_Property.m_iMachineHealth);
	}

	public override void OnListenGun()
	{
		if (!m_bWarning)
		{
			base.OnListenGun();
			if (m_PointScript.m_bCanMove2Player)
			{
				SetState(RUSHSTATE);
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
			m_fFireTime = UnityEngine.Random.Range(m_Property.m_fMinWTime, m_Property.m_fMaxWTime);
		}
		else
		{
			m_fFireTime = UnityEngine.Random.Range(m_Property.m_fMinNTime, m_Property.m_fMaxNTime);
		}
	}

	public void ResetCurMoToHealth(int iHarm)
	{
		m_iMoToHealth -= iHarm - m_Property.m_iASpecial;
	}

	public int GetCurMotoHealth()
	{
		return m_iMoToHealth;
	}
}
