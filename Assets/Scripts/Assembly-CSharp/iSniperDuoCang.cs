using UnityEngine;

public class iSniperDuoCang : iSniperNpc
{
	public class HideState : NpcState
	{
		private float m_fShowTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			iSniperDuoCang2.m_bHaveDamage = false;
			npc.LookAtCamera();
			npc.CrossAnimation((!iSniperDuoCang2.IsRightTurn()) ? "YinHu_Idle01_L" : "YinHu_Idle01_R", true);
			if (npc.m_bWarning)
			{
				m_fShowTime = Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
			}
			else
			{
				m_fShowTime = Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fShowTime -= deltaTime;
			if (m_fShowTime <= 0f)
			{
				iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
				npc.SetState(iSniperDuoCang2.TURNSTATE);
			}
		}
	}

	public class TurnState : NpcState
	{
		private float m_fAnimationTime;

		private float m_fAnimationLength;

		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			string name = ((!iSniperDuoCang2.IsRightTurn()) ? "YinHu_TurnL01" : "YinHu_TurnR01");
			float time = m_fAnimationLength - m_fAnimationTime;
			npc.ContinuePlayAnimation(name, time);
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			string name = ((!iSniperDuoCang2.IsRightTurn()) ? "YinHu_TurnL01" : "YinHu_TurnR01");
			npc.PlayAnimation(name, false);
			m_fAnimationTime = npc.AnimationLength(name);
			m_fAnimationLength = m_fAnimationTime;
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetPosition();
			if (iSniperDuoCang2.IsRightTurn())
			{
				if (iSniperDuoCang2.m_bReverseLR)
				{
					m_EndPosition.x += 0.3f;
				}
				else
				{
					m_EndPosition.x -= 0.3f;
				}
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			float num = m_fAnimationTime / m_fAnimationLength;
			npc.m_Model.transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, 1f - num);
			if (m_fAnimationTime <= 0f)
			{
				iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
				npc.SetState(iSniperDuoCang2.SHOWSTATE);
			}
		}
	}

	public class ShowState : NpcState
	{
		private float m_fFireTime;

		private bool m_bHaveFire;

		private float m_fReturnTime;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			if (iSniperDuoCang2.m_bHaveDamage)
			{
				npc.SetState(iSniperDuoCang2.RETURNSTATE);
			}
			else if (iSniperDuoCang2.IsRightTurn())
			{
				npc.CrossAnimation("YinHu_ReadyFireR01", true);
			}
			else
			{
				npc.CrossAnimation("YinHu_ReadyFireL01", true);
			}
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			if (iSniperDuoCang2.m_bHaveDamage)
			{
				npc.SetState(iSniperDuoCang2.RETURNSTATE);
				return;
			}
			if (iSniperDuoCang2.IsRightTurn())
			{
				npc.CrossAnimation("YinHu_ReadyFireR01", true);
			}
			else
			{
				npc.CrossAnimation("YinHu_ReadyFireL01", true);
			}
			m_fFireTime = 1f;
			m_bHaveFire = false;
			m_fReturnTime = 1f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			if (!m_bHaveFire)
			{
				m_fFireTime -= deltaTime;
				if (m_fFireTime <= 0f)
				{
					if (iSniperDuoCang2.IsRightTurn())
					{
						npc.PlayMixAnimation("YinHu_FireR01");
					}
					else
					{
						npc.PlayMixAnimation("YinHu_FireL01");
					}
					npc.Fire();
					m_bHaveFire = true;
				}
			}
			if (m_bHaveFire)
			{
				m_fReturnTime -= deltaTime;
				if (m_fReturnTime <= 0f)
				{
					npc.SetState(iSniperDuoCang2.RETURNSTATE);
				}
			}
		}
	}

	public class ReturnState : NpcState
	{
		private float m_fAnimationTime;

		private float m_fAnimationLength;

		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		public override void Contiune(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			string name = ((!iSniperDuoCang2.IsRightTurn()) ? "YinHu_TurnL02" : "YinHu_TurnR02");
			float time = npc.m_Model.GetComponent<Animation>()[name].length - m_fAnimationTime;
			npc.ContinuePlayAnimation(name, time);
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			string name = ((!iSniperDuoCang2.IsRightTurn()) ? "YinHu_TurnL02" : "YinHu_TurnR02");
			npc.PlayAnimation(name, false);
			m_fAnimationTime = npc.AnimationLength(name);
			m_fAnimationLength = m_fAnimationTime;
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetPosition();
			if (iSniperDuoCang2.IsRightTurn())
			{
				if (iSniperDuoCang2.m_bReverseLR)
				{
					m_EndPosition.x -= 0.3f;
				}
				else
				{
					m_EndPosition.x += 0.3f;
				}
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			float num = m_fAnimationTime / m_fAnimationLength;
			npc.m_Model.transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, 1f - num);
			if (m_fAnimationTime <= 0f)
			{
				iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
				npc.SetState(iSniperDuoCang2.HIDESTATE);
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
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			npc.m_bWarning = true;
			iSniperDuoCang2.m_bHaveDamage = true;
			m_fAnimationTime = 0f;
			if (iSniperDuoCang2.IsRightTurn())
			{
				npc.PlayAnimation("YinHu_Damage_Body02", false);
				m_fAnimationTime = npc.AnimationLength("YinHu_Damage_Body02");
			}
			else
			{
				npc.PlayAnimation("YinHu_Damage_Body01", false);
				m_fAnimationTime = npc.AnimationLength("YinHu_Damage_Body01");
			}
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

	public class DeadState : NpcState
	{
		public Part m_DamagePart;

		private float m_fAnimationTime;

		public override void Enter(iSniperNpc npc)
		{
			npc.ShowMark(false);
			npc.DestroyObserver();
			iSniperDuoCang iSniperDuoCang2 = (iSniperDuoCang)npc;
			if (iSniperDuoCang2.IsRightTurn())
			{
				npc.PlayAnimation("YinHu_Death_Body02", false);
				m_fAnimationTime = npc.AnimationLength("YinHu_Death_Body02");
			}
			else
			{
				npc.PlayAnimation("YinHu_Death_Body01", false);
				m_fAnimationTime = npc.AnimationLength("YinHu_Death_Body01");
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

	public bool m_bHaveDamage;

	public bool m_bReverseLR;

	public HideState HIDESTATE = new HideState();

	public TurnState TURNSTATE = new TurnState();

	public ShowState SHOWSTATE = new ShowState();

	public ReturnState RETURNSTATE = new ReturnState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("DuoCang", strName, position);
		m_bReverseLR = m_CameraScript.GetCameraPos().z < position.z;
		Transform mix = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].layer = 1;
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].layer = 1;
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].AddMixingTransform(mix);
		m_bHaveDamage = false;
		SetState(HIDESTATE);
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

	public bool IsRightTurn()
	{
		return m_PointScript.m_bCanMove2Player;
	}
}
