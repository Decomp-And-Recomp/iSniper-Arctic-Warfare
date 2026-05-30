using System.Collections;
using UnityEngine;

public class iSniperGameScene
{
	public enum State
	{
		kGameReady = 0,
		kGameStart = 1,
		kGaming = 2,
		kMovieDown = 3,
		kStageEnd = 4,
		kCameraMove = 5,
		kGameOver = 6,
		kBootCamps = 7
	}

	public EffectsConfig g_EffectsConfig;

	public iSniperGameCamera m_CameraScript;

	public iSniperGameUIHelper m_UIHelper;

	public iSniperGameCfg m_SceneDataCfg;

	public iSniperGameState m_GameState;

	public State m_State;

	public bool m_bIsTest;

	public bool m_bIsTouchMove;

	public bool m_bReadyShowAim;

	public bool m_bTouch;

	public int m_iTouchMoveID;

	public Vector2 m_Center;

	public float m_Direction;

	public float m_Distance;

	private Hashtable m_EnemyMap;

	private float m_fLastCreateObserverTime;

	private Hashtable m_PinMinMap;

	private Hashtable m_BuJiMap;

	private AudioSource m_SoundHitGround1;

	private AudioSource m_SoundHitGround2;

	private AudioSource m_SoundHitIron1;

	private AudioSource m_SoundHitIron2;

	public AudioSource m_SoundExp;

	public AudioSource m_SoundExpMoto;

	public AudioSource m_SoundExpBody;

	public AudioSource m_SoundAddTime;

	public AudioSource m_SoundAddHp;

	public GameObject m_LaserGunEffect;

	public bool m_bPlayGunEffect;

	public float m_fGunEffectTime;

	public Vector3 m_GunEffectEndPos;

	public Vector3 m_GunEffectStartPos;

	public void Initialize()
	{
		GameObject gameObject = GameObject.Find("Main Camera");
		m_CameraScript = gameObject.GetComponent<iSniperGameCamera>();
		m_UIHelper = gameObject.GetComponent<iSniperGameUIHelper>();
		m_SceneDataCfg = gameObject.GetComponent<iSniperGameCfg>();
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
		{
			m_GameState.m_iStageIndex = m_GameState.RecomputeStage(m_GameState.m_iArcLastStage, m_GameState.m_bArcWinState);
		}
		m_bReadyShowAim = false;
		GameObject gameObject2 = GameObject.Find("Effects_Prefab");
		g_EffectsConfig = gameObject2.GetComponent("EffectsConfig") as EffectsConfig;
		m_CameraScript.Initialize(this);
		m_EnemyMap = new Hashtable();
		m_fLastCreateObserverTime = 0f;
		m_PinMinMap = new Hashtable();
		m_BuJiMap = new Hashtable();
		if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode)
		{
			if (m_GameState.m_iArcCurScene != 4)
			{
				m_GameState.m_fGameTime = m_GameState.GetGameTime();
			}
			else
			{
				m_GameState.m_fGameTime = 149f;
			}
		}
		else
		{
			m_GameState.m_fGameTime = 149f;
		}
		m_SoundHitGround1 = GetAudioSource("SceneSound_Prefab/SoundHitGround1");
		m_SoundHitGround2 = GetAudioSource("SceneSound_Prefab/SoundHitGround2");
		m_SoundHitIron1 = GetAudioSource("SceneSound_Prefab/SoundHitIron1");
		m_SoundHitIron2 = GetAudioSource("SceneSound_Prefab/SoundHitIron2");
		m_SoundExp = GetAudioSource("SceneSound_Prefab/SoundExp");
		m_SoundExpMoto = GetAudioSource("SceneSound_Prefab/SoundExpMoto");
		m_SoundExpBody = GetAudioSource("SceneSound_Prefab/SoundExpFlesh");
		m_SoundAddHp = GetAudioSource("SceneSound_Prefab/SoundAddHp");
		m_SoundAddTime = GetAudioSource("SceneSound_Prefab/SoundAddTime");
		EnterGameReadyState();
	}

	public void Destroy()
	{
		ClearEnemy();
		ClearPinMin();
		ClearBuJi();
	}

	private void HandlePCControls(iSniperGameCamera cameraScript)
	{
		if (m_UIHelper.IsAim())
		{
			cameraScript.JoystickMoveSpeed(Input.GetAxisRaw("Mouse X") * 100f, Input.GetAxisRaw("Mouse Y") * 100f);
			if (Input.GetMouseButtonDown(0) && !((UIImage)m_UIHelper.m_control_table["firebtnmask"]).Enable)
			{
				m_UIHelper.OnHandleEvent(new UIControl { Id = m_UIHelper.GetControlId("firebtn") }, 1, 0f, 0f);
			}
			if (Input.GetMouseButtonDown(1))
			{
				m_UIHelper.HideAim();
				m_CameraScript.Restore();
				foreach (iSniperNpc value7 in m_EnemyMap.Values)
				{
					value7.ShowMark(true);
				}
			}
			float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
			if (scroll != 0f)
			{
				m_UIHelper.OnHandleEvent(new UIControl { Id = m_UIHelper.GetControlId("aimanglemove") }, 1, 0f, scroll * 100f);
				if (m_GameState.m_bBootCampsMode && m_UIHelper.m_enCampsState == iSniperGameUIHelper.BootCampsSTATE.kZoom)
				{
					m_UIHelper.OnHandleEvent(new UIControl { Id = m_UIHelper.GetControlId("aimanglemove") }, 2, 0f, 0f);
				}
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(1))
			{	
				m_UIHelper.ShowAim();
				m_CameraScript.Aim(Input.mousePosition);
				foreach (iSniperNpc value6 in m_EnemyMap.Values)
				{
					value6.ShowMark(false);
				}
				if (m_GameState.m_bBootCampsMode && m_UIHelper.m_enCampsState == iSniperGameUIHelper.BootCampsSTATE.kShowAim)
				{
					m_UIHelper.OnHandleEvent(new UIControl { Id = m_UIHelper.GetControlId("tipbtn") }, 0, 0f, 0f);
				}
			}
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			m_UIHelper.m_UIManagerRef.HandleInput(array[i]);
		}
	}

	private void HandleMobileControls(iSniperGameCamera cameraScript)
	{
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		for (int i = 0; i < array.Length; i++)
		{
			UITouchInner touch = array[i];
			if (m_UIHelper.IsAim() && touch.phase == TouchPhase.Moved && !iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl && m_bTouch && touch.fingerId == m_iTouchMoveID && touch.deltaPosition.magnitude > 1f)
			{
				float x = touch.deltaPosition.x;
				float y = touch.deltaPosition.y;
				Vector2 vector = touch.position - m_Center;
				if ((Screen.width > 960 && Screen.height > 640) || (Screen.width > 640 && Screen.height > 960))
				{
					vector *= 2.5f;
					x += Mathf.Abs(vector.x) / 70f * x;
					y += Mathf.Abs(vector.y) / 70f * y;
				}
				else
				{
					x += Mathf.Abs(vector.x) / 120f * x;
					y += Mathf.Abs(vector.y) / 120f * y;
				}
				cameraScript.JoystickMoveSpeed(x, y);
			}
			if (m_UIHelper.m_UIManagerRef.HandleInput(touch))
			{
				m_bTouch = false;
				continue;
			}
			if (touch.phase == TouchPhase.Began && iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				if (m_UIHelper.ShowAimUI())
				{
					m_CameraScript.Aim(touch.position);
					foreach (iSniperNpc value4 in m_EnemyMap.Values)
					{
						value4.ShowMark(false);
					}
				}
				else
				{
					m_CameraScript.Restore();
					foreach (iSniperNpc value5 in m_EnemyMap.Values)
					{
						value5.ShowMark(true);
					}
				}
			}
			if (iSniperGameApp.GetInstance().m_GameState.m_bIsTiltControl)
			{
				continue;
			}
			if (touch.phase == TouchPhase.Began && !m_UIHelper.IsAim())
			{
				m_bReadyShowAim = true;
			}
			if (touch.phase == TouchPhase.Began && m_UIHelper.IsAim())
			{
				m_bIsTouchMove = false;
				m_Center = touch.position;
			}
			if (touch.phase == TouchPhase.Moved && m_UIHelper.IsAim() && touch.deltaPosition.magnitude > 1f)
			{
				m_bIsTouchMove = true;
				m_bTouch = true;
				m_iTouchMoveID = touch.fingerId;
			}
			if (touch.phase == TouchPhase.Ended && m_UIHelper.IsAim())
			{
				m_Center = Vector2.zero;
			}
			if (touch.phase == TouchPhase.Ended && !m_UIHelper.IsAim() && m_bReadyShowAim)
			{
				m_UIHelper.ShowAim();
				m_CameraScript.Aim(touch.position);
				m_bReadyShowAim = false;
				foreach (iSniperNpc value6 in m_EnemyMap.Values)
				{
					value6.ShowMark(false);
				}
			}
			else if (m_UIHelper.IsAim() && touch.phase == TouchPhase.Ended && !m_bIsTouchMove)
			{
				m_UIHelper.HideAim();
				m_CameraScript.Restore();
				foreach (iSniperNpc value7 in m_EnemyMap.Values)
				{
					value7.ShowMark(true);
				}
			}
			if (m_UIHelper.IsAim() && touch.phase == TouchPhase.Ended && m_bIsTouchMove)
			{
				cameraScript.ResetMoveSpeed();
			}
		}
	}

	public void DoLogic(float deltaTime)
	{
		ArrayList arrayList = new ArrayList();
		foreach (iSniperNpc value in m_EnemyMap.Values)
		{
			value.DoLogic(deltaTime);
			if (value.m_bCanDestroy)
			{
				arrayList.Add(value);
			}
		}
		foreach (iSniperNpc item in arrayList)
		{
			RemoveEnemy(item);
			item.Destroy();
		}
		arrayList.Clear();
		ArrayList arrayList2 = new ArrayList();
		foreach (iSniperNpc value2 in m_PinMinMap.Values)
		{
			value2.DoLogic(deltaTime);
			if (value2.m_bCanDestroy)
			{
				arrayList2.Add(value2);
			}
		}
		foreach (iSniperNpc item2 in arrayList2)
		{
			RemovePinMin(item2);
			item2.Destroy();
		}
		arrayList2.Clear();
		ArrayList arrayList3 = new ArrayList();
		foreach (iSniperNpc value3 in m_BuJiMap.Values)
		{
			value3.DoLogic(deltaTime);
			if (value3.m_bCanDestroy)
			{
				arrayList3.Add(value3);
			}
		}
		foreach (iSniperNpc item3 in arrayList3)
		{
			RemoveBuJi(item3);
			item3.Destroy();
		}
		arrayList3.Clear();
		if (m_State != State.kGaming)
		{
			return;
		}
		iSniperGameCamera cameraScript = m_CameraScript;
		cameraScript.ResetMoveSpeed();
		if (Application.isMobilePlatform)
		{
			HandleMobileControls(cameraScript);
		}
		else
		{
			HandlePCControls(cameraScript);
		}
		if (!m_GameState.m_bBootCampsMode)
		{
			m_GameState.m_fGameTime -= deltaTime;
			if (m_GameState.m_fGameTime < 0f)
			{
				m_GameState.m_fGameTime = 0f;
			}
			m_UIHelper.SetGameTime(m_GameState.m_fGameTime);
			if (m_GameState.m_fGameTime == 0f)
			{
				if (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode && m_GameState.m_iArcCurScene == 4)
				{
					m_GameState.m_GameResult = iSniperGameState.Result.kSuccess;
				}
				else
				{
					m_GameState.m_GameResult = iSniperGameState.Result.kTimeup;
				}
				EnterGameOverState();
			}
		}
		else
		{
			m_UIHelper.SetGameTime(0f);
		}
		if (m_UIHelper.IsAim())
		{
			return;
		}
		m_fLastCreateObserverTime -= deltaTime;
		if (!(m_fLastCreateObserverTime < 0f))
		{
			return;
		}
		foreach (iSniperNpc value8 in m_EnemyMap.Values)
		{
			if (value8.CreateObserver())
			{
				m_fLastCreateObserverTime = 1f;
				break;
			}
		}
	}

	public void EnterGameReadyState()
	{
		m_State = State.kGameReady;
	}

	public void EnterGameStartState()
	{
		m_UIHelper.SetPlayerBlood(m_GameState.m_iPlayerCurrentHealth);
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperStageCfg stageCfg = m_SceneDataCfg.GetStageCfg(iStageIndex);
		m_UIHelper.SetEnemyData(m_GameState.m_iKillEnemyNum, stageCfg.m_iKillEnemySum);
		if (m_GameState.m_bBootCampsMode)
		{
			m_UIHelper.SetGameTime(0f);
		}
		else
		{
			m_UIHelper.SetGameTime(m_GameState.m_fGameTime);
		}
		m_UIHelper.SetEnemyBlood(0, 0);
		m_UIHelper.SetPlayerScore(m_GameState.m_iPlayerScore);
		m_State = State.kGameStart;
	}

	public void EnterGamingState()
	{
		foreach (iSniperNpc value in m_EnemyMap.Values)
		{
			value.RefreshCreateTime();
		}
		m_State = State.kGaming;
	}

	public void EnterMovieDownState()
	{
		m_UIHelper.SetEnemyBlood(0, 0);
		m_State = State.kMovieDown;
		m_UIHelper.EnterMovieDownState();
	}

	public void EnterStageEndState()
	{
		m_State = State.kStageEnd;
	}

	public void EnterCameraMoveState()
	{
		ClearPinMin();
		ClearBuJi();
		ClearEnemy();
		m_State = State.kCameraMove;
		m_CameraScript.ReadyToCameraMove();
		m_GameState.m_iStageIndex++;
		m_GameState.m_iKillEnemyNum = 0;
	}

	public void EnterGameOverState()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		m_State = State.kGameOver;
		m_UIHelper.DelaySwitchToResult();
	}

	public void CheckStageEndAction()
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		if (iStageIndex >= m_SceneDataCfg.m_ArrayStageCfgs.Count || (!m_GameState.m_bArcadeIsLock && !m_GameState.m_bStoryMode))
		{
			m_GameState.m_GameResult = iSniperGameState.Result.kSuccess;
			EnterGameOverState();
		}
		else
		{
			EnterCameraMoveState();
		}
	}

	public Hashtable getEnemyMap()
	{
		return m_EnemyMap;
	}

	public Hashtable getPinMinMap()
	{
		return m_PinMinMap;
	}

	public void AddEnemy(iSniperNpc enemy)
	{
		m_EnemyMap.Add(enemy.m_Model.name, enemy);
	}

	public void RemoveEnemy(iSniperNpc enemy)
	{
		m_EnemyMap.Remove(enemy.m_Model.name);
	}

	public void ClearEnemy()
	{
		foreach (iSniperNpc value in m_EnemyMap.Values)
		{
			value.Destroy();
		}
		m_EnemyMap.Clear();
	}

	public int EnemyNum()
	{
		return m_EnemyMap.Count;
	}

	public void AddPinMin(iSniperNpc pinmin)
	{
		m_PinMinMap.Add(pinmin.m_Model.name, pinmin);
	}

	public void RemovePinMin(iSniperNpc pinmin)
	{
		m_PinMinMap.Remove(pinmin.m_Model.name);
	}

	public void ClearPinMin()
	{
		foreach (iSniperNpc value in m_PinMinMap.Values)
		{
			value.Destroy();
		}
		m_PinMinMap.Clear();
	}

	public int PinMinNum()
	{
		return m_PinMinMap.Count;
	}

	public void AddBuJi(iSniperNpc buji)
	{
		m_BuJiMap.Add(buji.m_Model.name, buji);
	}

	public void RemoveBuJi(iSniperNpc buji)
	{
		m_BuJiMap.Remove(buji.m_Model.name);
	}

	public void ClearBuJi()
	{
		foreach (iSniperNpc value in m_BuJiMap.Values)
		{
			value.Destroy();
		}
		m_BuJiMap.Clear();
	}

	public int BuJiNum()
	{
		return m_BuJiMap.Count;
	}

	public void LaserGunEffect(Vector3 EndPos)
	{
	}

	public void Explosion(Vector3 point)
	{
		foreach (iSniperNpc value in m_EnemyMap.Values)
		{
			if (value.IsDead())
			{
				continue;
			}
			float num = Vector3.Distance(value.m_Model.transform.position, point);
			if (num > 8f)
			{
				if (num <= 13.333333f)
				{
					value.OnListenGun();
				}
				continue;
			}
			if ("MoTo" == value.m_strTypeName)
			{
				iSniperMoTo iSniperMoTo2 = (iSniperMoTo)value;
				iSniperMoTo2.ResetCurMoToHealth(120);
				value.m_iCurrentHealth -= 30;
				if (value.m_iCurrentHealth <= 0 || iSniperMoTo2.GetCurMotoHealth() <= 0)
				{
					value.EnterDeadState();
					m_GameState.m_iKillEnemyNum++;
					m_GameState.m_iPlayerScore += value.m_Property.m_iScore;
				}
			}
			if ("YinBao" == value.m_strTypeName)
			{
				value.EnterDeadState();
				continue;
			}
			value.EnterDeadState();
			m_GameState.m_iKillEnemyNum++;
			m_GameState.m_iPlayerScore += value.m_Property.m_iScore;
		}
		foreach (iSniperNpc value2 in m_PinMinMap.Values)
		{
			if (value2.IsDead())
			{
				continue;
			}
			float num2 = Vector3.Distance(value2.m_Model.transform.position, point);
			if (num2 > 8f)
			{
				if (num2 <= 20f)
				{
					value2.OnListenGun();
				}
			}
			else
			{
				value2.EnterDeadState();
				m_GameState.m_iPlayerScore += value2.m_Property.m_iScore;
			}
		}
		if (m_GameState.m_iPlayerScore < 0)
		{
			m_GameState.m_iPlayerScore = 0;
		}
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperStageCfg stageCfg = m_SceneDataCfg.GetStageCfg(iStageIndex);
		m_UIHelper.SetEnemyData(m_GameState.m_iKillEnemyNum, stageCfg.m_iKillEnemySum);
		m_UIHelper.SetPlayerScore(m_GameState.m_iPlayerScore);
		if (m_GameState.m_iKillEnemyNum == stageCfg.m_iKillEnemySum)
		{
			EnterMovieDownState();
		}
	}

	public void RocketGunExplosion(Vector3 point)
	{
		foreach (iSniperNpc value in m_EnemyMap.Values)
		{
			if (value.IsDead())
			{
				continue;
			}
			float num = Vector3.Distance(value.m_Model.transform.position, point);
			if (num > 5.3333335f)
			{
				if (num <= 10f)
				{
					value.OnListenGun();
				}
				continue;
			}
			iSniperGunProperty useGunProperty = m_GameState.GetUseGunProperty();
			int currentHarm = useGunProperty.GetCurrentHarm();
			currentHarm = ((!(num < 3f)) ? (currentHarm - 120) : (currentHarm - 60));
			if (currentHarm < 0)
			{
				currentHarm = 0;
			}
			value.m_iCurrentHealth -= currentHarm;
			if (value.m_iCurrentHealth <= 0)
			{
				value.EnterDeadState();
				m_GameState.m_iKillEnemyNum++;
				m_GameState.m_iPlayerScore += value.m_Property.m_iScore;
			}
		}
		foreach (iSniperNpc value2 in m_PinMinMap.Values)
		{
			if (value2.IsDead())
			{
				continue;
			}
			float num2 = Vector3.Distance(value2.m_Model.transform.position, point);
			if (num2 > 4f)
			{
				if (num2 <= 6.6666665f)
				{
					value2.OnListenGun();
				}
			}
			else
			{
				value2.EnterDeadState();
				m_GameState.m_iPlayerScore += value2.m_Property.m_iScore;
				m_GameState.m_fGameTime -= 5f;
				m_GameState.m_fGameTimeBonus -= 5f;
			}
		}
		if (m_GameState.m_iPlayerScore < 0)
		{
			m_GameState.m_iPlayerScore = 0;
		}
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperStageCfg stageCfg = m_SceneDataCfg.GetStageCfg(iStageIndex);
		m_UIHelper.SetEnemyData(m_GameState.m_iKillEnemyNum, stageCfg.m_iKillEnemySum);
		m_UIHelper.SetPlayerScore(m_GameState.m_iPlayerScore);
		if (m_GameState.m_iKillEnemyNum == stageCfg.m_iKillEnemySum)
		{
			EnterMovieDownState();
		}
	}

	public void EnemyFire()
	{
		Vector3 position = GameObject.Find("Main Camera/EffectArea").transform.position;
		position.x += Random.Range(-2.5f, 2.5f);
		position.z += Random.Range(-2.5f, 4.5f);
		position.y = 1000f;
		int layerMask = 7680;
		RaycastHit hitInfo;
		Physics.Raycast(position, Vector3.down, out hitInfo, float.PositiveInfinity, layerMask);
		if (null == hitInfo.collider)
		{
			return;
		}
		Vector3 point = hitInfo.point;
		if (!m_CameraScript.IsOutScreen(point))
		{
			if (hitInfo.collider.name.IndexOf("gold") != -1)
			{
				GameObject gameObject = Object.Instantiate(g_EffectsConfig.m_AttackIronEffect, point, Quaternion.identity) as GameObject;
				gameObject.transform.up = hitInfo.normal;
				PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitIron2 : m_SoundHitIron1, point);
			}
			else if (hitInfo.collider.name.IndexOf("grd") != -1 || hitInfo.collider.name.IndexOf("gro") != -1 || hitInfo.collider.name.IndexOf("house") != -1)
			{
				GameObject gameObject2 = Object.Instantiate(g_EffectsConfig.m_AttackSpaceEffect, point, Quaternion.identity) as GameObject;
				gameObject2.transform.up = hitInfo.normal;
				PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
			}
			else if (hitInfo.collider.name.IndexOf("snow") != -1)
			{
				GameObject gameObject3 = Object.Instantiate(g_EffectsConfig.m_AttackSnowEffect, point, Quaternion.identity) as GameObject;
				PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
			}
			else if (hitInfo.collider.name.IndexOf("dirt") != -1)
			{
				GameObject gameObject4 = Object.Instantiate(g_EffectsConfig.m_DustEffect, point, Quaternion.identity) as GameObject;
				gameObject4.transform.up = hitInfo.normal;
				PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
			}
			else if (hitInfo.collider.name.IndexOf("tree_sg") != -1 || hitInfo.collider.name.IndexOf("wood") != -1)
			{
				GameObject gameObject5 = Object.Instantiate(g_EffectsConfig.m_AttackWoodEffect, point, Quaternion.identity) as GameObject;
				gameObject5.transform.up = hitInfo.normal;
				PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
			}
		}
	}

	public void Fire()
	{
		m_GameState.m_iFireNum++;
		if (m_bIsTest)
		{
			m_GameState.m_iKillEnemyNum++;
			int iStageIndex = m_GameState.m_iStageIndex;
			iSniperStageCfg stageCfg = m_SceneDataCfg.GetStageCfg(iStageIndex);
			m_UIHelper.SetEnemyData(m_GameState.m_iKillEnemyNum, stageCfg.m_iKillEnemySum);
			if (m_GameState.m_iKillEnemyNum == stageCfg.m_iKillEnemySum)
			{
				EnterMovieDownState();
			}
			m_bIsTest = false;
			return;
		}
		Vector3 position = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
		Ray ray = m_CameraScript.m_Camera.ScreenPointToRay(position);
		int layerMask = 7680;
		RaycastHit hitInfo;
		Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMask);
		if (null == hitInfo.collider)
		{
			return;
		}
		bool flag = false;
		foreach (iSniperNpc value in m_EnemyMap.Values)
		{
			if (value.IsDead() || !value.JudgeGun(ray, hitInfo))
			{
				continue;
			}
			flag = true;
			if (!value.IsDead())
			{
				continue;
			}
			foreach (iSniperNpc value2 in m_EnemyMap.Values)
			{
				if (!value2.IsDead() && Vector3.Distance(value.GetPosition(), value2.GetPosition()) <= 6f)
				{
					value2.OnListenGun();
				}
			}
			foreach (iSniperNpc value3 in m_PinMinMap.Values)
			{
				if (!value3.IsDead() && Vector3.Distance(value.GetPosition(), value3.GetPosition()) <= 6f)
				{
					value3.OnListenGun();
				}
			}
			if (m_GameState.m_bBootCampsMode)
			{
				m_UIHelper.m_bEnemyIsDie = true;
			}
			m_GameState.m_iKillEnemyNum++;
			if (m_GameState.m_iAchKillEnemyNum < 100)
			{
				m_GameState.m_iAchKillEnemyNum++;
				m_GameState.SaveData();
			}
			if (m_GameState.m_iAchKillEnemyNum == 100)
			{
				m_GameState.m_iAchNewHand = 1;
				m_GameState.SaveData();
			}
			int iStageIndex2 = m_GameState.m_iStageIndex;
			iSniperStageCfg stageCfg2 = m_SceneDataCfg.GetStageCfg(iStageIndex2);
			m_UIHelper.SetEnemyData(m_GameState.m_iKillEnemyNum, stageCfg2.m_iKillEnemySum);
			if (m_GameState.m_iKillEnemyNum == stageCfg2.m_iKillEnemySum)
			{
				EnterMovieDownState();
			}
		}
		foreach (iSniperNpc value4 in m_PinMinMap.Values)
		{
			if (value4.IsDead() || !value4.JudgeGun(ray, hitInfo))
			{
				continue;
			}
			flag = true;
			if (!value4.IsDead())
			{
				continue;
			}
			foreach (iSniperNpc value5 in m_EnemyMap.Values)
			{
				if (!value5.IsDead() && Vector3.Distance(value4.GetPosition(), value5.GetPosition()) <= 6f)
				{
					value5.OnListenGun();
				}
			}
			foreach (iSniperNpc value6 in m_PinMinMap.Values)
			{
				if (!value6.IsDead() && Vector3.Distance(value4.GetPosition(), value6.GetPosition()) <= 6f)
				{
					value6.OnListenGun();
				}
			}
			if (m_GameState.m_iAchKillPinMinNum < 5)
			{
				m_GameState.m_iAchKillPinMinNum++;
				m_GameState.SaveData();
			}
			if (m_GameState.m_iAchKillPinMinNum == 5)
			{
				m_GameState.m_iAchCollateral = 1;
				m_GameState.SaveData();
			}
		}
		iSniperGunProperty useGunProperty = m_GameState.GetUseGunProperty();
		if (useGunProperty.m_strName == "RPG")
		{
			GameObject gameObject = Object.Instantiate(g_EffectsConfig.m_CanExplosionEffect, hitInfo.point, Quaternion.identity) as GameObject;
			gameObject.transform.up = hitInfo.normal;
			RocketGunExplosion(hitInfo.point);
			PlaySoundAtPos(m_SoundExp, hitInfo.point);
		}
		if (hitInfo.collider.name.IndexOf("BuJi_HP") != -1)
		{
			int iStageIndex3 = m_GameState.m_iStageIndex;
			iSniperStageCfg stageCfg3 = m_SceneDataCfg.GetStageCfg(iStageIndex3);
			m_GameState.m_iPlayerCurrentHealth += stageCfg3.m_iAddHeath * m_GameState.GetPlayerHealth() / 100;
			if (m_GameState.m_iPlayerCurrentHealth > m_GameState.GetPlayerHealth())
			{
				m_GameState.m_iPlayerCurrentHealth = m_GameState.GetPlayerHealth();
			}
			PlaySoundAtPos(m_SoundAddHp, GameObject.Find("Main Camera").transform.position);
			m_UIHelper.SetPlayerBlood(m_GameState.m_iPlayerCurrentHealth);
			Object.Destroy(hitInfo.collider.transform.root.gameObject);
			GameObject gameObject2 = Object.Instantiate(g_EffectsConfig.m_AddHPEffect, hitInfo.point, Quaternion.identity) as GameObject;
			m_UIHelper.ShowTitleShotTip("health");
			flag = true;
		}
		if (hitInfo.collider.name.IndexOf("BuJi_Time") != -1)
		{
			int iStageIndex4 = m_GameState.m_iStageIndex;
			iSniperStageCfg stageCfg4 = m_SceneDataCfg.GetStageCfg(iStageIndex4);
			m_GameState.m_fGameTime += stageCfg4.m_iAddTime;
			m_GameState.m_fGameTimeBonus += stageCfg4.m_iAddTime;
			PlaySoundAtPos(m_SoundAddTime, GameObject.Find("Main Camera").transform.position);
			m_UIHelper.SetGameTime(m_GameState.m_fGameTime);
			Object.Destroy(hitInfo.collider.transform.root.gameObject);
			GameObject gameObject3 = Object.Instantiate(g_EffectsConfig.m_AddTimeEffect, hitInfo.point, Quaternion.identity) as GameObject;
			m_UIHelper.ShowTitleShotTip("time");
			flag = true;
		}
		if (!flag)
		{
			if (!(null == hitInfo.collider))
			{
				Vector3 point = hitInfo.point;
				if (hitInfo.collider.name.IndexOf("gold") != -1)
				{
					GameObject gameObject4 = Object.Instantiate(g_EffectsConfig.m_AttackIronEffect, point, Quaternion.identity) as GameObject;
					gameObject4.transform.up = hitInfo.normal;
					PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitIron2 : m_SoundHitIron1, point);
				}
				else if (hitInfo.collider.name.IndexOf("grd") != -1 || hitInfo.collider.name.IndexOf("gro") != -1 || hitInfo.collider.name.IndexOf("house") != -1)
				{
					GameObject gameObject5 = Object.Instantiate(g_EffectsConfig.m_AttackSpaceEffect, point, Quaternion.identity) as GameObject;
					gameObject5.transform.up = hitInfo.normal;
					PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
				}
				else if (hitInfo.collider.name.IndexOf("snow") != -1)
				{
					GameObject gameObject6 = Object.Instantiate(g_EffectsConfig.m_AttackSnowEffect, point, Quaternion.identity) as GameObject;
					PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
				}
				else if (hitInfo.collider.name.IndexOf("dirt") != -1)
				{
					GameObject gameObject7 = Object.Instantiate(g_EffectsConfig.m_DustEffect, point, Quaternion.identity) as GameObject;
					gameObject7.transform.up = hitInfo.normal;
					PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
				}
				else if (hitInfo.collider.name.IndexOf("tree_sg") != -1 || hitInfo.collider.name.IndexOf("wood") != -1)
				{
					GameObject gameObject8 = Object.Instantiate(g_EffectsConfig.m_AttackWoodEffect, point, Quaternion.identity) as GameObject;
					gameObject8.transform.up = hitInfo.normal;
					PlaySoundAtPos((!Utils.ProbabilityIsRandomHit(0.5f)) ? m_SoundHitGround2 : m_SoundHitGround1, point);
				}
				else if (hitInfo.collider.name.IndexOf("exp") != -1)
				{
					Vector3 position2 = hitInfo.collider.gameObject.transform.position;
					GameObject gameObject9 = Object.Instantiate(g_EffectsConfig.m_ExtinguisherEffect, position2, Quaternion.identity) as GameObject;
					gameObject9.transform.up = hitInfo.normal;
					gameObject9.AddComponent<iSniperExplosionCheck>();
					PlaySoundAtPos(m_SoundExpMoto, point);
					Object.Destroy(hitInfo.collider.gameObject);
				}
				else if (hitInfo.collider.name.IndexOf("oil") != -1)
				{
					Vector3 position3 = hitInfo.collider.gameObject.transform.position;
					GameObject gameObject10 = Object.Instantiate(g_EffectsConfig.m_CanExplosionEffect, position3, Quaternion.identity) as GameObject;
					gameObject10.transform.up = hitInfo.normal;
					gameObject10.AddComponent<iSniperExplosionCheck>();
					PlaySoundAtPos(m_SoundExpMoto, point);
					Object.Destroy(hitInfo.collider.gameObject);
				}
			}
		}
		else
		{
			m_GameState.m_iHitNum++;
		}
	}

	public bool PlayerLoseBlood(int value)
	{
		bool result = false;
		m_GameState.m_iPlayerCurrentHealth -= value;
		if (m_GameState.m_iPlayerCurrentHealth <= 0)
		{
			m_GameState.m_iPlayerCurrentHealth = 0;
			result = true;
		}
		m_UIHelper.SetPlayerBlood(m_GameState.m_iPlayerCurrentHealth);
		return result;
	}

	public AudioSource GetAudioSource(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject != null)
		{
			return gameObject.GetComponent<AudioSource>();
		}
		return null;
	}

	public void PlaySoundAtPos(AudioSource audio, Vector3 pos)
	{
		if (!(null == audio) && iSniperGameApp.GetInstance().m_GameState.m_bSoundOn)
		{
			audio.transform.parent.position = pos;
			audio.Play();
		}
	}
}
