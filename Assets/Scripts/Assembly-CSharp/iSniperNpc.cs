using System;
using UnityEngine;

public class iSniperNpc
{
	public class NpcState
	{
		public virtual void Contiune(iSniperNpc npc)
		{
		}

		public virtual void Enter(iSniperNpc npc)
		{
		}

		public virtual void Loop(iSniperNpc npc, float deltaTime)
		{
		}

		public virtual void Exit(iSniperNpc npc)
		{
		}
	}

	public enum Part
	{
		kHead = 0,
		kBody = 1,
		kArmL = 2,
		kArmR = 3,
		kLegL = 4,
		kLegR = 5
	}

	public enum Pose
	{
		kGrovel = 0,
		kStand = 1,
		kStandby = 2,
		kSquat = 3,
		kAim = 4
	}

	public class DelayBloodState : NpcState
	{
		private float m_fStartTime;

		public override void Enter(iSniperNpc npc)
		{
			m_fStartTime = 0f;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			m_fStartTime += deltaTime;
			if (m_fStartTime >= 0.25f)
			{
				if (npc.m_Model.name.IndexOf("ZhuangJia") != -1)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(npc.g_EffectsConfig.m_AttackIronEffect, npc.m_BloodObj.transform.position, Quaternion.identity) as GameObject;
				}
				else
				{
					npc.m_BloodMain.GetComponent<ParticleEmitter>().Emit();
					npc.m_BloodDrop.GetComponent<ParticleEmitter>().Emit();
					npc.m_BloodMain1.GetComponent<ParticleEmitter>().Emit();
					npc.m_BloodDrop1.GetComponent<ParticleEmitter>().Emit();
				}
				npc.PlaySound(npc.m_SoundLastHeadShot);
				npc.OnHit(Part.kHead);
			}
		}
	}

	public class DestroyState : NpcState
	{
		private enum Status
		{
			kWait = 0,
			kDowning = 1
		}

		private float m_fStartDownTime;

		private float m_fAlpha;

		private Status m_Status;

		public override void Enter(iSniperNpc npc)
		{
			m_fStartDownTime = 0f;
			m_Status = Status.kWait;
		}

		public override void Loop(iSniperNpc npc, float deltaTime)
		{
			if (npc.m_bCanDestroy)
			{
				return;
			}
			MeshControllerScript meshControllerScript = npc.m_Model.GetComponent("MeshControllerScript") as MeshControllerScript;
			switch (m_Status)
			{
			case Status.kWait:
				m_fStartDownTime += deltaTime;
				if (m_fStartDownTime > 1f)
				{
					npc.ShowShadow(false);
					m_Status = Status.kDowning;
					meshControllerScript.SetShader(Resources.Load<Shader>("iSniper3D/Shaders/ModelTransparentShow"));
					m_fAlpha = 1f;
					meshControllerScript.SetMaterialColor("_TintColor", new Color(1f, 1f, 1f, m_fAlpha));
				}
				break;
			case Status.kDowning:
			{
				float num = deltaTime * 0.5f;
				m_fAlpha -= num;
				if (m_fAlpha <= 0f)
				{
					m_fAlpha = 0f;
					npc.m_bCanDestroy = true;
				}
				meshControllerScript.SetMaterialColor("clrBase", new Color(1f, 1f, 1f, m_fAlpha));
				break;
			}
			}
		}
	}

	public GameObject m_Model;

	public GameObject m_HeadObj;

	public GameObject m_WaistObj;

	public GameObject m_FootObj;

	public GameObject m_MarkObj;

	public GameObject m_ShadowObj;

	public GameObject m_BloodObj;

	public GameObject m_BloodMain;

	public GameObject m_BloodDrop;

	public GameObject m_BloodMain1;

	public GameObject m_BloodDrop1;

	public GameObject m_Observer;

	public iSniperEnemyPoint m_PointScript;

	public iSniperEnemyProperty m_Property;

	public iSniperGameCamera m_CameraScript;

	public iSniperGameUIHelper m_UIHelper;

	public iSniperGameScene m_GameScene;

	public iSniperGameState m_GameState;

	public iSniperGameCfg m_StagesCfg;

	public EffectsConfig g_EffectsConfig;

	public AudioSource m_SoundHurt1;

	public AudioSource m_SoundHurt2;

	public AudioSource m_SoundHeadshot1;

	public AudioSource m_SoundHeadshot2;

	public AudioSource m_SoundLastHeadShot;

	public AudioSource m_SoundShot1;

	public AudioSource m_SoundShot2;

	public AudioSource m_SoundMoTo1;

	public AudioSource m_SoundMoTo2;

	private Vector3 m_BornPoint;

	public int m_iCurrentHealth;

	public int m_iCurrentDamage;

	public int m_iCurrentAHead;

	public int m_iCurrentABody;

	public int m_iCurrentAArm;

	public int m_iCurrentALeg;

	public bool m_bWarning;

	public bool m_bCanDestroy;

	public Pose m_StartPose;

	public Pose m_CurrentPose;

	public bool m_bNeedReturnBornPoint;

	public bool m_bNeedBackPoint;

	public string m_strTypeName;

	private bool m_bCreateObserver;

	private bool m_bMarkShowing;

	private float m_fCreateTime;

	private float m_fCheckShadowNormalRate;

	public NpcState m_State;

	public DestroyState DESTROYSTATE = new DestroyState();

	public DelayBloodState DELAYBLOODSTATE = new DelayBloodState();

	public virtual void Initialize(string strName, Vector3 position)
	{
		m_fCreateTime = 0f;
		m_bCreateObserver = false;
		m_bWarning = false;
		m_bCanDestroy = false;
		m_BornPoint = position;
		m_bNeedReturnBornPoint = false;
		m_bNeedBackPoint = false;
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		m_CameraScript = m_GameScene.m_CameraScript;
		m_UIHelper = m_GameScene.m_UIHelper;
		m_StagesCfg = m_GameScene.m_SceneDataCfg;
		m_fCheckShadowNormalRate = 0f;
	}

	public void CreateModel(string strType, string strName, Vector3 position)
	{
		m_Model = NPCFactory.CreateEnemy(strType, strName, position);
		m_ShadowObj = m_Model.transform.Find("Shadow_Prefab").gameObject;
		m_MarkObj = m_Model.transform.Find("Mark_Prefab").gameObject;
		m_HeadObj = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head").gameObject;
		m_WaistObj = m_Model.transform.Find("Bip01").gameObject;
		m_FootObj = m_Model.transform.Find("Bip01/Bip01 Footsteps").gameObject;
		m_MarkObj.GetComponent<FollowObjectScript>().m_FollowObject = m_HeadObj;
		m_ShadowObj.GetComponent<FollowObjectXZScript>().m_FollowObjectY = m_Model;
		m_ShadowObj.GetComponent<FollowObjectXZScript>().m_FollowObjectXZ = m_WaistObj;
		m_ShadowObj.GetComponent<FollowObjectXZScript>().m_Offset = new Vector3(0f, 0.05f, 0f);
		if (m_GameState.m_iArcCurScene == 6 || m_GameState.m_iPlayerCurrentScene == 6)
		{
			m_ShadowObj.GetComponent<iSniperShadow>().m_Size = new Vector2(0.65f, 0.65f);
		}
		else
		{
			m_ShadowObj.GetComponent<iSniperShadow>().m_Size = new Vector2(0.8f, 0.8f);
		}
		m_strTypeName = strType;
		position = CheckGround(position);
		position.y += 0.01f;
		m_Model.transform.position = position;
		iSniperNpcShell iSniperNpcShell2 = m_Model.GetComponent("iSniperNpcShell") as iSniperNpcShell;
		iSniperNpcShell2.m_iSniperNpcRef = this;
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperStageCfg stageCfg = m_StagesCfg.GetStageCfg(iStageIndex);
		string text = m_PointScript.m_strType;
		if ("YinHu" == text)
		{
			text += m_PointScript.m_iIndex;
		}
		if (stageCfg.m_EnemyProperty.Contains(text))
		{
			m_Property = (iSniperEnemyProperty)stageCfg.m_EnemyProperty[text];
			if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
			{
				int num = m_GameState.m_iArcDaysNum - 1;
				if (num >= 100)
				{
					num = 99;
				}
				m_iCurrentDamage = m_Property.m_iDamage + Mathf.RoundToInt(0.5f * (float)num);
				if (text != "PinMin")
				{
					int num2 = m_GameState.m_iArcDaysNum - 1;
					if (num2 >= 55)
					{
						num2 = 54;
					}
					m_iCurrentAHead = m_Property.m_iAHead + num2 / 5 * 10;
					m_iCurrentABody = m_Property.m_iABody + num2 / 5 * 10;
					m_iCurrentAArm = m_Property.m_iAArm + num2 / 5 * 10;
					m_iCurrentALeg = m_Property.m_iALeg + num2 / 5 * 10;
				}
				else
				{
					m_iCurrentDamage = m_Property.m_iDamage;
					m_iCurrentAHead = m_Property.m_iAHead;
					m_iCurrentABody = m_Property.m_iABody;
					m_iCurrentAArm = m_Property.m_iAArm;
					m_iCurrentALeg = m_Property.m_iALeg;
				}
			}
			else
			{
				m_iCurrentDamage = m_Property.m_iDamage;
				m_iCurrentAHead = m_Property.m_iAHead;
				m_iCurrentABody = m_Property.m_iABody;
				m_iCurrentAArm = m_Property.m_iAArm;
				m_iCurrentALeg = m_Property.m_iALeg;
			}
			m_iCurrentHealth = m_Property.m_iHeath;
		}
		GameObject gameObject = GameObject.Find("Effects_Prefab");
		g_EffectsConfig = gameObject.GetComponent("EffectsConfig") as EffectsConfig;
		m_BloodObj = UnityEngine.Object.Instantiate(g_EffectsConfig.m_BloodEffect) as GameObject;
		m_BloodMain = m_BloodObj.transform.GetChild(0).gameObject;
		m_BloodDrop = m_BloodObj.transform.GetChild(1).gameObject;
		m_BloodMain1 = m_BloodObj.transform.GetChild(2).gameObject;
		m_BloodDrop1 = m_BloodObj.transform.GetChild(3).gameObject;
		m_BloodMain.GetComponent<ParticleEmitter>().emit = false;
		m_BloodDrop.GetComponent<ParticleEmitter>().emit = false;
		m_BloodMain1.GetComponent<ParticleEmitter>().emit = false;
		m_BloodDrop1.GetComponent<ParticleEmitter>().emit = false;
		RefreshCreateTime();
		m_SoundHurt1 = m_Model.transform.Find("Sound/Hurt1Obj").GetComponent<AudioSource>();
		m_SoundHurt2 = m_Model.transform.Find("Sound/Hurt2Obj").GetComponent<AudioSource>();
		m_SoundHeadshot1 = m_Model.transform.Find("Sound/HeadshotObj1").GetComponent<AudioSource>();
		m_SoundHeadshot2 = m_Model.transform.Find("Sound/HeadshotObj2").GetComponent<AudioSource>();
		if (text != "PinMin" && text != "YinBao")
		{
			m_SoundShot1 = m_Model.transform.Find("Sound/SoundShot1").GetComponent<AudioSource>();
			m_SoundShot2 = m_Model.transform.Find("Sound/SoundShot2").GetComponent<AudioSource>();
		}
		if (text == "MoTo")
		{
			m_SoundMoTo1 = m_Model.transform.Find("Sound/MotoSound1").GetComponent<AudioSource>();
			m_SoundMoTo2 = m_Model.transform.Find("Sound/MotoSound2").GetComponent<AudioSource>();
		}
		m_SoundLastHeadShot = m_Model.transform.Find("Sound/HeadShot_LastShot").GetComponent<AudioSource>();
	}

	public void CreateOtherModel(string strType, string strName, Vector3 position)
	{
		m_Model = NPCFactory.CreateEnemy(strType, strName, position);
		m_strTypeName = strType;
		position = CheckGround(position);
		position.y += 0.8f;
		m_Model.transform.position = position;
	}

	public void RefreshCreateTime()
	{
		m_fCreateTime = Time.time;
	}

	public void Destroy()
	{
		m_PointScript.Reset();
		if (null != m_Model)
		{
			UnityEngine.Object.Destroy(m_Model);
			m_Model = null;
		}
		if (null != m_BloodObj)
		{
			UnityEngine.Object.Destroy(m_BloodObj);
			m_BloodObj = null;
		}
		DestroyObserver();
	}

	public Vector3 GetBornPosition()
	{
		return m_BornPoint;
	}

	public Vector3 GetRandomPoint(float fMinRadius, float fMaxRadius)
	{
		float f = UnityEngine.Random.Range(-(float)Math.PI, (float)Math.PI);
		float num = UnityEngine.Random.Range(0f, fMaxRadius - fMinRadius);
		num += fMinRadius;
		Vector3 zero = Vector3.zero;
		zero.x = m_BornPoint.x + num * Mathf.Cos(f);
		zero.z = m_BornPoint.z + num * Mathf.Sin(f);
		zero.y = 10000f;
		RaycastHit hitInfo;
		if (Physics.Raycast(zero, Vector3.down, out hitInfo, float.PositiveInfinity, 5120))
		{
			zero.y = hitInfo.point.y;
		}
		else
		{
			zero.y = m_Model.transform.position.y;
		}
		return zero;
	}

	public Vector3 GetRandomPoint(Vector3 direction, float fMinDistance, float fMaxDistance, bool bBornPoint)
	{
		Ray ray = new Ray((!bBornPoint) ? m_Model.transform.position : GetBornPosition(), direction);
		float distance = UnityEngine.Random.Range(fMinDistance, fMaxDistance);
		return ray.GetPoint(distance);
	}

	public Vector3 GetRandomPointBetweenTwoPoint(Vector3 point1, Vector3 point2)
	{
		Vector3 position = Vector3.Lerp(point1, point2, UnityEngine.Random.Range(0f, 1f));
		return CheckGround(position);
	}

	public Vector3 GetNearPlayerPoint()
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperStageCfg stageCfg = m_StagesCfg.GetStageCfg(iStageIndex);
		float maxDistanceDelta = UnityEngine.Random.Range(stageCfg.m_fMinCameraDistance, stageCfg.m_fMaxCameraDistance);
		return Vector3.MoveTowards(m_CameraScript.GetCameraPos(), GetPosition(), maxDistanceDelta);
	}

	public float CheckGround()
	{
		Vector3 position = m_HeadObj.transform.position;
		RaycastHit hitInfo;
		Physics.Raycast(position, Vector3.down, out hitInfo, float.PositiveInfinity, 5120);
		return hitInfo.point.y;
	}

	public Vector3 CheckGround(Vector3 position)
	{
		Vector3 result = position;
		position.y = 1000f;
		RaycastHit hitInfo;
		Physics.Raycast(position, Vector3.down, out hitInfo, float.PositiveInfinity, 5120);
		result.y = hitInfo.point.y;
		return result;
	}

	public Vector3 CheckGroundNormal()
	{
		Vector3 position = m_HeadObj.transform.position;
		RaycastHit hitInfo;
		Physics.Raycast(position, Vector3.down, out hitInfo, float.PositiveInfinity, 5120);
		return hitInfo.normal;
	}

	public bool CheckObstacle()
	{
		int layerMask = 64000;
		Vector3 point = m_Model.transform.position + new Vector3(0f, 0.1f, 0f);
		Vector3 point2 = m_Model.transform.position + new Vector3(0f, 1.8f, 0f);
		RaycastHit hitInfo;
		bool result = Physics.CapsuleCast(point, point2, 0.1f, m_Model.transform.forward, out hitInfo, 0.3f, layerMask);
		if (null != hitInfo.collider)
		{
			Debug.Log("obstacle name = " + hitInfo.collider.name);
		}
		return result;
	}

	public bool CheckObstacleNoEnemy()
	{
		int layerMask = 63488;
		Vector3 point = m_Model.transform.position + new Vector3(0f, 0.1f, 0f);
		Vector3 point2 = m_Model.transform.position + new Vector3(0f, 1.8f, 0f);
		RaycastHit hitInfo;
		bool result = Physics.CapsuleCast(point, point2, 0.1f, m_Model.transform.forward, out hitInfo, 0.3f, layerMask);
		if (null != hitInfo.collider)
		{
		}
		return result;
	}

	public bool IsHideInObstacle()
	{
		Vector3 cameraPos = m_CameraScript.GetCameraPos();
		Vector3 position = m_HeadObj.transform.position;
		Vector3 vector = position - cameraPos;
		int layerMask = 7168;
		RaycastHit hitInfo;
		if (Physics.Raycast(cameraPos, vector.normalized, out hitInfo, vector.magnitude, layerMask))
		{
			return true;
		}
		return false;
	}

	public bool IsHideInObstacle(Vector3 position)
	{
		Vector3 cameraPos = m_CameraScript.GetCameraPos();
		Vector3 vector = position - cameraPos;
		int layerMask = 7168;
		RaycastHit hitInfo;
		if (Physics.Raycast(cameraPos, vector.normalized, out hitInfo, vector.magnitude, layerMask))
		{
			return true;
		}
		return false;
	}

	public bool IsOutScreen()
	{
		if (m_GameScene.m_State != iSniperGameScene.State.kGaming)
		{
			return false;
		}
		return m_CameraScript.IsOutScreen(GetPosition());
	}

	public void SetPosition(Vector3 pos)
	{
		m_Model.transform.position = pos;
	}

	public Vector3 GetPosition()
	{
		return m_Model.transform.position;
	}

	public void ShowMark(bool bVisible)
	{
		if (bVisible)
		{
			if (!IsDead() && m_bMarkShowing)
			{
				m_MarkObj.GetComponent<Renderer>().enabled = bVisible;
			}
		}
		else
		{
			m_MarkObj.GetComponent<Renderer>().enabled = false;
		}
	}

	public void SetMarkShow()
	{
		if (!IsDead())
		{
			m_bMarkShowing = true;
			m_MarkObj.GetComponent<Renderer>().enabled = !m_UIHelper.IsAim();
		}
	}

	public void ShowShadow(bool bVisible)
	{
		if (!(null == m_ShadowObj))
		{
			m_ShadowObj.GetComponent<Renderer>().enabled = bVisible;
		}
	}

	public void DestroyObserver()
	{
		if (!(null == m_Observer))
		{
			UnityEngine.Object.Destroy(m_Observer);
			m_Observer = null;
		}
	}

	public bool CreateObserver()
	{
		if (IsDead())
		{
			return false;
		}
		if (m_bCreateObserver)
		{
			return false;
		}
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperStageCfg stageCfg = m_StagesCfg.GetStageCfg(iStageIndex);
		if (Time.time - m_fCreateTime < stageCfg.m_fMaxObserverTime)
		{
			return false;
		}
		m_bCreateObserver = true;
		Vector3 vector = Vector3.MoveTowards(m_CameraScript.GetCameraPos(), m_CameraScript.GetLookAtPos(), 25f);
		vector.x += UnityEngine.Random.Range(-5f, 5f);
		vector.y += UnityEngine.Random.Range(-5f, 5f);
		m_Observer = UnityEngine.Object.Instantiate(g_EffectsConfig.m_ObserverEffect, vector, Quaternion.identity) as GameObject;
		TranslateRotateScript translateRotateScript = m_Observer.GetComponent("TranslateRotateScript") as TranslateRotateScript;
		translateRotateScript.m_BeginPosition = vector;
		translateRotateScript.m_MarkObject = m_MarkObj;
		translateRotateScript.m_NpcObject = m_Model;
		translateRotateScript.m_fTimeout = 0f;
		return true;
	}

	public void LookAtCamera()
	{
		Vector3 position = m_Model.transform.position;
		Vector3 cameraPos = m_CameraScript.GetCameraPos();
		Vector3 forward = cameraPos - position;
		forward.y = 0f;
		m_Model.transform.forward = forward;
	}

	public void FireEffect()
	{
		Transform transform = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Bone/FirePoint");
		GameObject gameObject = UnityEngine.Object.Instantiate(g_EffectsConfig.m_GunFireEffect, transform.position, Quaternion.identity) as GameObject;
		FollowObjectScript component = gameObject.GetComponent<FollowObjectScript>();
		component.m_FollowObject = transform.gameObject;
		if (Utils.ProbabilityIsRandomHit(0.2f))
		{
			m_GameScene.EnemyFire();
			PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundShot2 : m_SoundShot1);
		}
	}

	public void Fire()
	{
		Transform transform = m_Model.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Bone/FirePoint");
		GameObject gameObject = UnityEngine.Object.Instantiate(g_EffectsConfig.m_GunFireEffect, transform.position, Quaternion.identity) as GameObject;
		FollowObjectScript component = gameObject.GetComponent<FollowObjectScript>();
		component.m_FollowObject = transform.gameObject;
		PlaySound((!Utils.ProbabilityIsRandomHit(0.6f)) ? m_SoundShot2 : m_SoundShot1);
		if (m_GameScene.m_State != iSniperGameScene.State.kGaming)
		{
			return;
		}
		bool flag = ((!m_bWarning) ? Utils.ProbabilityIsRandomHit(m_Property.m_fNRatio) : Utils.ProbabilityIsRandomHit(m_Property.m_fWRatio));
		if (m_Model.name.IndexOf("ChongFeng") != -1 && null != GameObject.Find("Scene4_Prefab") && !m_GameState.m_bStoryMode)
		{
			iSniperChongFeng iSniperChongFeng2 = (iSniperChongFeng)this;
			if (iSniperChongFeng2.m_bIsLastState)
			{
				flag = Utils.ProbabilityIsRandomHit(m_Property.m_fWRatio * 4f);
			}
		}
		if (flag)
		{
			m_CameraScript.Shake();
			m_UIHelper.ShowShotByGun();
			PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_UIHelper.m_SoundHurt2 : m_UIHelper.m_SoundHurt1);
			if (m_GameScene.PlayerLoseBlood(m_iCurrentDamage))
			{
				m_GameState.m_GameResult = iSniperGameState.Result.kBloodup;
				m_GameScene.EnterGameOverState();
			}
		}
		else if (Utils.ProbabilityIsRandomHit(0.5f))
		{
			m_GameScene.EnemyFire();
		}
	}

	public void PlayAnimation(string name, bool loop)
	{
		m_Model.GetComponent<Animation>()[name].wrapMode = ((!loop) ? WrapMode.ClampForever : WrapMode.Loop);
		m_Model.GetComponent<Animation>()[name].time = 0f;
		m_Model.GetComponent<Animation>().Play(name);
	}

	public void CrossAnimation(string name, bool loop)
	{
		m_Model.GetComponent<Animation>()[name].wrapMode = ((!loop) ? WrapMode.ClampForever : WrapMode.Loop);
		m_Model.GetComponent<Animation>()[name].time = 0f;
		m_Model.GetComponent<Animation>().CrossFade(name);
	}

	public void PlayMixAnimation(string name)
	{
		m_Model.GetComponent<Animation>().Play(name);
	}

	public void ContinuePlayAnimation(string name, float time)
	{
		m_Model.GetComponent<Animation>()[name].wrapMode = WrapMode.ClampForever;
		m_Model.GetComponent<Animation>()[name].time = time;
		m_Model.GetComponent<Animation>().Play(name);
	}

	public void RandomPlayAnimation(string name)
	{
		m_Model.GetComponent<Animation>()[name].time = UnityEngine.Random.Range(0f, 1f) * m_Model.GetComponent<Animation>()[name].length;
		m_Model.GetComponent<Animation>()[name].wrapMode = WrapMode.Loop;
		m_Model.GetComponent<Animation>().Play(name);
	}

	public void RandomCrossAnimation(string name)
	{
		m_Model.GetComponent<Animation>()[name].time = UnityEngine.Random.Range(0f, 1f) * m_Model.GetComponent<Animation>()[name].length;
		m_Model.GetComponent<Animation>()[name].wrapMode = WrapMode.Loop;
		m_Model.GetComponent<Animation>().CrossFade(name);
	}

	public float AnimationLength(string name)
	{
		return m_Model.GetComponent<Animation>()[name].length;
	}

	public void SetState(NpcState state)
	{
		if (m_State != state)
		{
			if (m_State != null)
			{
				m_State.Exit(this);
			}
			m_State = state;
			if (m_State != null)
			{
				m_State.Enter(this);
			}
		}
	}

	public void ContinueState(NpcState state)
	{
		if (m_State != null)
		{
			m_State.Exit(this);
		}
		m_State = state;
		m_State.Contiune(this);
	}

	public void DoLogic(float deltaTime)
	{
		if (null != m_ShadowObj)
		{
			m_fCheckShadowNormalRate += deltaTime;
			if (m_fCheckShadowNormalRate >= 0f)
			{
				m_ShadowObj.transform.up = CheckGroundNormal();
				m_fCheckShadowNormalRate = 0f;
			}
		}
		if (m_State != null)
		{
			m_State.Loop(this, deltaTime);
		}
	}

	public virtual void OnHit(Part part)
	{
	}

	public virtual void EnterDeadState()
	{
		m_iCurrentHealth = 0;
		Part part = (Part)UnityEngine.Random.Range(0, 6);
		OnHit(part);
	}

	public virtual bool IsDead()
	{
		return DELAYBLOODSTATE == m_State || DESTROYSTATE == m_State;
	}

	public void EmitBlood(string strName)
	{
		if (strName.IndexOf("ZhuangJia") != -1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(g_EffectsConfig.m_AttackIronEffect, m_BloodObj.transform.position, Quaternion.identity) as GameObject;
			return;
		}
		m_BloodMain.GetComponent<ParticleEmitter>().Emit();
		m_BloodDrop.GetComponent<ParticleEmitter>().Emit();
		m_BloodMain1.GetComponent<ParticleEmitter>().Emit();
		m_BloodDrop1.GetComponent<ParticleEmitter>().Emit();
	}

	public bool JudgeGun(Ray ray, RaycastHit hit)
	{
		bool flag = false;
		GameObject gameObject = hit.collider.transform.root.gameObject;
		if (m_Model == gameObject)
		{
			flag = true;
			bool flag2 = false;
			iSniperGunProperty useGunProperty = m_GameState.GetUseGunProperty();
			m_BloodObj.transform.position = hit.point;
			if (IsHitHead(hit.collider.name))
			{
				int num = useGunProperty.GetCurrentHarm() - m_iCurrentAHead;
				if (num < 0)
				{
					num = 0;
				}
				m_iCurrentHealth -= num;
				if (m_iCurrentHealth <= 0)
				{
					if (m_Property.m_iScore >= 0)
					{
						m_GameState.m_iHeadshotNum++;
						if (m_GameState.m_iAchHeadShotNum < 50)
						{
							m_GameState.m_iAchHeadShotNum++;
							m_GameState.SaveData();
						}
						if (m_GameState.m_iAchHeadShotNum == 50)
						{
							m_GameState.m_iAchGod = 1;
							m_GameState.SaveData();
						}
						if (m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode || m_GameState.m_iArcCurScene != 4)
						{
							m_GameState.m_fGameTime += 6f;
							m_GameState.m_fGameTimeBonus += 6f;
						}
						m_UIHelper.ShowTitleShotTip("head");
						int iStageIndex = m_GameState.m_iStageIndex;
						iSniperStageCfg stageCfg = m_StagesCfg.GetStageCfg(iStageIndex);
						if (m_GameState.m_iKillEnemyNum == stageCfg.m_iKillEnemySum - 1)
						{
							SetState(DELAYBLOODSTATE);
							if (m_UIHelper.IsAim())
							{
								m_UIHelper.ShowAimUI();
							}
							m_UIHelper.ShowHeadShotMask(true);
							m_UIHelper.ShowTitleShotTip("head");
							m_CameraScript.HeadShotCamera();
						}
						else
						{
							EmitBlood(m_Model.name);
							PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHeadshot2 : m_SoundHeadshot1);
							OnHit(Part.kHead);
						}
					}
					else
					{
						m_GameState.m_fGameTime -= 5f;
						m_GameState.m_fGameTimeBonus -= 5f;
						m_UIHelper.ShowTitleShotTip("friend");
						EmitBlood(m_Model.name);
						PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
						OnHit(Part.kHead);
					}
				}
				else
				{
					PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
					EmitBlood(m_Model.name);
					OnHit(Part.kHead);
				}
			}
			else if (IsHitBody(hit.collider.name))
			{
				EmitBlood(m_Model.name);
				int num2 = useGunProperty.GetCurrentHarm() - m_iCurrentABody;
				if (num2 < 0)
				{
					num2 = 0;
				}
				m_iCurrentHealth -= num2;
				PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
				OnHit(Part.kBody);
				if (m_iCurrentHealth <= 0)
				{
					if (m_Property.m_iScore >= 0)
					{
						if (m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode || m_GameState.m_iArcCurScene != 4)
						{
							m_GameState.m_fGameTime += 4f;
							m_GameState.m_fGameTimeBonus += 4f;
						}
						m_UIHelper.ShowTitleShotTip("body");
					}
					else
					{
						m_GameState.m_fGameTime -= 5f;
						m_GameState.m_fGameTimeBonus -= 5f;
						m_UIHelper.ShowTitleShotTip("friend");
					}
				}
			}
			else if (IsHitArmL(hit.collider.name))
			{
				EmitBlood(m_Model.name);
				int num3 = useGunProperty.GetCurrentHarm() - m_iCurrentAArm;
				if (num3 < 0)
				{
					num3 = 0;
				}
				m_iCurrentHealth -= num3;
				PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
				OnHit(Part.kArmL);
				if (m_iCurrentHealth <= 0)
				{
					if (m_Property.m_iScore >= 0)
					{
						if (m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode || m_GameState.m_iArcCurScene != 4)
						{
							m_GameState.m_fGameTime += 2f;
							m_GameState.m_fGameTimeBonus += 2f;
						}
						m_UIHelper.ShowTitleShotTip("limb");
					}
					else
					{
						m_GameState.m_fGameTime -= 5f;
						m_GameState.m_fGameTimeBonus -= 5f;
						m_UIHelper.ShowTitleShotTip("friend");
					}
				}
			}
			else if (IsHitArmR(hit.collider.name))
			{
				EmitBlood(m_Model.name);
				int num4 = useGunProperty.GetCurrentHarm() - m_iCurrentAArm;
				if (num4 < 0)
				{
					num4 = 0;
				}
				m_iCurrentHealth -= num4;
				PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
				OnHit(Part.kArmR);
				if (m_iCurrentHealth <= 0)
				{
					if (m_Property.m_iScore >= 0)
					{
						if (m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode || m_GameState.m_iArcCurScene != 4)
						{
							m_GameState.m_fGameTime += 2f;
							m_GameState.m_fGameTimeBonus += 2f;
						}
						m_UIHelper.ShowTitleShotTip("limb");
					}
					else
					{
						m_GameState.m_fGameTime -= 5f;
						m_GameState.m_fGameTimeBonus -= 5f;
						m_UIHelper.ShowTitleShotTip("friend");
					}
				}
			}
			else if (IsHitLegL(hit.collider.name))
			{
				EmitBlood(m_Model.name);
				int num5 = useGunProperty.GetCurrentHarm() - m_iCurrentALeg;
				if (num5 < 0)
				{
					num5 = 0;
				}
				m_iCurrentHealth -= num5;
				PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
				OnHit(Part.kLegL);
				if (m_iCurrentHealth <= 0)
				{
					if (m_Property.m_iScore >= 0)
					{
						if (m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode || m_GameState.m_iArcCurScene != 4)
						{
							m_GameState.m_fGameTime += 2f;
							m_GameState.m_fGameTimeBonus += 2f;
						}
						m_UIHelper.ShowTitleShotTip("limb");
					}
					else
					{
						m_GameState.m_fGameTime -= 5f;
						m_GameState.m_fGameTimeBonus -= 5f;
						m_UIHelper.ShowTitleShotTip("friend");
					}
				}
			}
			else if (IsHitLegR(hit.collider.name))
			{
				EmitBlood(m_Model.name);
				int num6 = useGunProperty.GetCurrentHarm() - m_iCurrentALeg;
				if (num6 < 0)
				{
					num6 = 0;
				}
				m_iCurrentHealth -= num6;
				PlaySound((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHurt2 : m_SoundHurt1);
				OnHit(Part.kLegR);
				if (m_iCurrentHealth <= 0)
				{
					if (m_Property.m_iScore >= 0)
					{
						if (m_GameState.m_bArcadeIsLock || m_GameState.m_bStoryMode || m_GameState.m_iArcCurScene != 4)
						{
							m_GameState.m_fGameTime += 2f;
							m_GameState.m_fGameTimeBonus += 2f;
						}
						m_UIHelper.ShowTitleShotTip("limb");
					}
					else
					{
						m_GameState.m_fGameTime -= 5f;
						m_GameState.m_fGameTimeBonus -= 5f;
						m_UIHelper.ShowTitleShotTip("friend");
					}
				}
			}
			else
			{
				OnHitMachine(hit);
				flag2 = true;
			}
			if (m_iCurrentHealth <= 0)
			{
				m_iCurrentHealth = 0;
				m_GameState.m_iPlayerScore += m_Property.m_iScore;
			}
			if (!flag2)
			{
				m_UIHelper.SetEnemyBlood(m_iCurrentHealth, m_Property.m_iHeath);
			}
			m_UIHelper.SetPlayerScore(m_GameState.m_iPlayerScore);
		}
		if (!flag)
		{
			JudgeListenGun(ray, hit);
		}
		return flag;
	}

	public virtual void OnHitMachine(RaycastHit hit)
	{
	}

	public bool IsHitHead(string name)
	{
		return "Bip01 Head" == name;
	}

	public bool IsHitBody(string name)
	{
		return "Bip01 Spine1" == name;
	}

	public bool IsHitArmL(string name)
	{
		return "Bip01 L UpperArm" == name || "Bip01 L Forearm" == name;
	}

	public bool IsHitArmR(string name)
	{
		return "Bip01 R UpperArm" == name || "Bip01 R Forearm" == name;
	}

	public bool IsHitLegL(string name)
	{
		return "Bip01 L Thigh" == name || "Bip01 L Foot" == name;
	}

	public bool IsHitLegR(string name)
	{
		return "Bip01 R Thigh" == name || "Bip01 R Foot" == name;
	}

	public void JudgeListenGun(Ray ray, RaycastHit hit)
	{
		iSniperGunProperty useGunProperty = iSniperGameApp.GetInstance().m_GameState.GetUseGunProperty();
		float num = ComputePoint2RayDistance(ray, GetPosition());
		float num2 = Vector3.Distance(hit.point, GetPosition());
		if (num <= (float)useGunProperty.GetCurrentSliencer() / 1.5f || num2 <= (float)useGunProperty.GetCurrentSliencer() / 1.5f)
		{
			OnListenGun();
		}
	}

	public virtual void OnListenGun()
	{
		m_bWarning = true;
	}

	public float ComputePoint2RayDistance(Ray ray, Vector3 point)
	{
		Vector3 vector = point - ray.origin;
		Vector3 normalized = vector.normalized;
		float f = Mathf.Acos(Vector3.Dot(normalized, ray.direction));
		return vector.magnitude * Mathf.Sin(f);
	}

	public void PlaySound(AudioSource audio)
	{
		if (!(null == audio) && iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			audio.Play();
		}
	}
}
