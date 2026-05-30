using System;
using System.Collections;
using UnityEngine;

public class iSniperYinHu : iSniperNpc
{
	public class HideState : NpcState
	{
		private float m_fShowTime;

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			iSniperYinHu2.m_bHaveDamage = false;
			if (Utils.ProbabilityIsRandomHit(0.8f))
			{
				npc.LookAtCamera();
				npc.CrossAnimation((!iSniperYinHu2.IsRightTurn()) ? "YinHu_Idle01_L" : "YinHu_Idle01_R", true);
				if (npc.m_bWarning)
				{
					m_fShowTime = UnityEngine.Random.Range(npc.m_Property.m_fMinWTime, npc.m_Property.m_fMaxWTime);
				}
				else
				{
					m_fShowTime = UnityEngine.Random.Range(npc.m_Property.m_fMinNTime, npc.m_Property.m_fMaxNTime);
				}
			}
			else
			{
				npc.SetState(iSniperYinHu2.RUNSTATE);
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fShowTime -= deltaTime;
			if (m_fShowTime <= 0f)
			{
				iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
				npc.SetState(iSniperYinHu2.TURNSTATE);
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			string name = ((!iSniperYinHu2.IsRightTurn()) ? "YinHu_TurnL01" : "YinHu_TurnR01");
			float time = m_fAnimationLength - m_fAnimationTime;
			npc.ContinuePlayAnimation(name, time);
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			string name = ((!iSniperYinHu2.IsRightTurn()) ? "YinHu_TurnL01" : "YinHu_TurnR01");
			npc.PlayAnimation(name, false);
			m_fAnimationTime = npc.AnimationLength(name);
			m_fAnimationLength = m_fAnimationTime;
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetPosition();
			if (iSniperYinHu2.IsRightTurn())
			{
				m_EndPosition.x -= 0.3f;
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			float num = m_fAnimationTime / m_fAnimationLength;
			npc.m_Model.transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, 1f - num);
			if (m_fAnimationTime <= 0f)
			{
				iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
				npc.SetState(iSniperYinHu2.SHOWSTATE);
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			if (iSniperYinHu2.m_bHaveDamage)
			{
				npc.SetState(iSniperYinHu2.RETURNSTATE);
			}
			else if (iSniperYinHu2.IsRightTurn())
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			if (iSniperYinHu2.m_bHaveDamage)
			{
				npc.SetState(iSniperYinHu2.RETURNSTATE);
				return;
			}
			if (iSniperYinHu2.IsRightTurn())
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			if (!m_bHaveFire)
			{
				m_fFireTime -= deltaTime;
				if (m_fFireTime <= 0f)
				{
					if (iSniperYinHu2.IsRightTurn())
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
					npc.SetState(iSniperYinHu2.RETURNSTATE);
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			string name = ((!iSniperYinHu2.IsRightTurn()) ? "YinHu_TurnL02" : "YinHu_TurnR02");
			float time = npc.m_Model.GetComponent<Animation>()[name].length - m_fAnimationTime;
			npc.ContinuePlayAnimation(name, time);
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			string name = ((!iSniperYinHu2.IsRightTurn()) ? "YinHu_TurnL02" : "YinHu_TurnR02");
			npc.PlayAnimation(name, false);
			m_fAnimationTime = npc.AnimationLength(name);
			m_fAnimationLength = m_fAnimationTime;
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.GetPosition();
			if (iSniperYinHu2.IsRightTurn())
			{
				m_EndPosition.x += 0.3f;
			}
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fAnimationTime -= deltaTime;
			float num = m_fAnimationTime / m_fAnimationLength;
			npc.m_Model.transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, 1f - num);
			if (m_fAnimationTime <= 0f)
			{
				iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
				npc.SetState(iSniperYinHu2.HIDESTATE);
			}
		}
	}

	public class RunState : NpcState
	{
		private Vector3 m_StartPosition;

		private Vector3 m_EndPosition;

		private float m_fRunTime;

		private float m_fStartTime;

		public override void Contiune(iSniperNpc npc)
		{
			npc.CrossAnimation("YinHu_Rush01", true);
		}

		public override void Enter(iSniperNpc npc)
		{
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			npc.CrossAnimation("YinHu_Rush01", true);
			iSniperYinHu2.m_iPointIndex = iSniperYinHu2.RandomIndex();
			m_StartPosition = npc.GetPosition();
			m_EndPosition = npc.m_PointScript.m_Path[iSniperYinHu2.m_iPointIndex].transform.position;
			m_fRunTime = Vector3.Distance(m_StartPosition, m_EndPosition) / 5f;
			m_fStartTime = 0f;
			iSniperYinHu2.m_bIsRunState = true;
			npc.m_Model.transform.forward = (m_EndPosition - m_StartPosition).normalized;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			npc.m_Model.transform.position = Vector3.Lerp(m_StartPosition, m_EndPosition, m_fStartTime / m_fRunTime);
			if (m_fStartTime >= m_fRunTime)
			{
				iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
				npc.SetState(iSniperYinHu2.HIDESTATE);
				iSniperYinHu2.m_bIsRunState = false;
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			npc.m_bWarning = true;
			iSniperYinHu2.m_bHaveDamage = true;
			m_fAnimationTime = 0f;
			if (iSniperYinHu2.IsRightTurn())
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
			iSniperYinHu iSniperYinHu2 = (iSniperYinHu)npc;
			if (iSniperYinHu2.IsRightTurn())
			{
				npc.PlayAnimation("YinHu_Death_Body02", false);
				m_fAnimationTime = npc.AnimationLength("YinHu_Death_Body02");
			}
			else
			{
				npc.PlayAnimation("YinHu_Death_Body01", false);
				m_fAnimationTime = npc.AnimationLength("YinHu_Death_Body01");
			}
			if (iSniperYinHu2.m_bIsRunState)
			{
				npc.PlayAnimation("Aim_Death_Body01", false);
				m_fAnimationTime = npc.AnimationLength("Aim_Death_Body01");
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

	internal class ArrayListCompare : IComparer
	{
		private System.Random r = new System.Random();

		public int Compare(object x, object y)
		{
			return r.Next(-1, 1);
		}
	}

	public int m_iPointIndex;

	public bool m_bHaveDamage;

	public HideState HIDESTATE = new HideState();

	public TurnState TURNSTATE = new TurnState();

	public ShowState SHOWSTATE = new ShowState();

	public ReturnState RETURNSTATE = new ReturnState();

	public RunState RUNSTATE = new RunState();

	public DamageState DAMAGESTATE = new DamageState();

	public DeadState DEADSTATE = new DeadState();

	public bool m_bIsRunState;

	public override void Initialize(string strName, Vector3 position)
	{
		base.Initialize(strName, position);
		CreateModel("YinHu", strName, position);
		Transform mix = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].layer = 1;
		m_Model.GetComponent<Animation>()["YinHu_FireL01"].AddMixingTransform(mix);
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].wrapMode = WrapMode.Once;
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].blendMode = AnimationBlendMode.Blend;
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].layer = 1;
		m_Model.GetComponent<Animation>()["YinHu_FireR01"].AddMixingTransform(mix);
		int num = UnityEngine.Random.Range(0, 1000);
		m_iPointIndex = num % 6;
		m_bIsRunState = false;
		m_Model.transform.position = m_PointScript.m_Path[m_iPointIndex].transform.position;
		m_bHaveDamage = false;
		SetState(HIDESTATE);
	}

	public override void OnHit(Part part)
	{
		if (m_iCurrentHealth > 0)
		{
			if (RUNSTATE != m_State)
			{
				DAMAGESTATE.m_DamagePart = part;
				DAMAGESTATE.m_LastState = m_State;
				SetState(DAMAGESTATE);
			}
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
		return m_iPointIndex % 2 != 0;
	}

	public int RandomIndex()
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < 6; i++)
		{
			if (i != m_iPointIndex)
			{
				arrayList.Add(i);
			}
		}
		IComparer comparer = new ArrayListCompare();
		arrayList.Sort(comparer);
		arrayList.Sort(comparer);
		int num = UnityEngine.Random.Range(0, 1000);
		num %= 5;
		return (int)arrayList[num];
	}
}
