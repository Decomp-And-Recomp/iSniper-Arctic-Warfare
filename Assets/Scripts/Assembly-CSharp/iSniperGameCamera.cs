using System;
using UnityEngine;

public class iSniperGameCamera : MonoBehaviour
{
	private enum AimState
	{
		kNoAim = 0,
		kDynmaic = 1,
		kAiming = 2
	}

	public Camera m_Camera;

	public iSniperGameScene m_GameScene;

	public iSniperGameState m_GameState;

	public Camera m_JudegCam;

	private AimState m_AimState;

	private Vector3 m_StartGravity;

	private Vector2 m_AimPoint2D;

	private Vector3 m_AimPoint3D;

	private float m_fAimFov;

	private float m_fAimDuringTime;

	private Vector2 m_MoveSpeed;

	private bool m_bShake;

	private float m_fShakeTime;

	private float m_fStageSwitchTime;

	private float m_fRayTime;

	private bool m_bHeadShotCamera;

	private float m_fHeadShotTime;

	private float m_fHeadShotStartFov;

	private void Awake()
	{
		m_Camera = Camera.main;
		m_Camera.nearClipPlane = 0.5f;
		m_Camera.farClipPlane = 1000f;
		m_Camera.cullingMask = 31745;
		m_bHeadShotCamera = false;
		float fScaleFactor = UIManager.m_fScaleFactor;
		m_Camera.pixelRect = new Rect(((float)Screen.width - 960f * fScaleFactor) / 2f, ((float)Screen.height - 640f * fScaleFactor) / 2f, 960f * fScaleFactor, 640f * fScaleFactor);
		m_JudegCam = GameObject.Find("JudgeCamera").GetComponent<Camera>();
	}

	public void Initialize(iSniperGameScene scene)
	{
		m_GameScene = scene;
		m_GameState = iSniperGameApp.GetInstance().m_GameState;
		m_Camera = Camera.main;
		Restore();
		iSniperGunProperty useGunProperty = m_GameState.GetUseGunProperty();
		m_fAimFov = useGunProperty.GetCurrentZoom() + m_GameState.m_fCurrentGunFovDeltaPercent * useGunProperty.GetCurrentZoomDelta();
		m_bShake = false;
		m_fStageSwitchTime = 0f;
		m_fRayTime = 0.3f;
	}

	private void Update()
	{
		if (m_GameScene == null || m_GameState == null)
		{
			return;
		}
		iSniperGameScene.State state = m_GameScene.m_State;
		int iStageIndex = m_GameState.m_iStageIndex;
		switch (state)
		{
		case iSniperGameScene.State.kGameReady:
			break;
		case iSniperGameScene.State.kGameStart:
			break;
		case iSniperGameScene.State.kGaming:
		{
			iSniperCameraCfg cameraCfg = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
			if (m_AimState != 0)
			{
				if (m_AimState == AimState.kDynmaic)
				{
					m_fAimDuringTime += Time.deltaTime;
					if (m_fAimDuringTime >= 0.2f)
					{
						m_fAimDuringTime = 0.2f;
						m_StartGravity = Input.acceleration;
						m_AimState = AimState.kAiming;
					}
					m_Camera.fieldOfView = cameraCfg.m_fFov + m_fAimDuringTime / 0.2f * (m_fAimFov - cameraCfg.m_fFov);
				}
				else if (m_AimState == AimState.kAiming)
				{
					if (m_GameState.m_bIsTiltControl)
					{
						ComputeMoveSpeed();
					}
					int num2 = ((!Utils.IsRetina()) ? 1 : 2);
					m_AimPoint2D.x += m_MoveSpeed.x * (float)num2 * Time.deltaTime;
					m_AimPoint2D.y += m_MoveSpeed.y * (float)num2 * Time.deltaTime;
					if (Utils.IsRetina())
					{
						if (m_AimPoint2D.x > ((float)Screen.width - 960f * UIManager.m_fScaleFactor) / 2f + 960f * UIManager.m_fScaleFactor)
						{
							m_AimPoint2D.x = ((float)Screen.width - 960f * UIManager.m_fScaleFactor) / 2f + 960f * UIManager.m_fScaleFactor;
						}
						else if (m_AimPoint2D.x < ((float)Screen.width - 960f * UIManager.m_fScaleFactor) / 2f)
						{
							m_AimPoint2D.x = ((float)Screen.width - 960f * UIManager.m_fScaleFactor) / 2f;
						}
						if (m_AimPoint2D.y > ((float)Screen.height - 640f * UIManager.m_fScaleFactor) / 2f + 640f * UIManager.m_fScaleFactor)
						{
							m_AimPoint2D.y = ((float)Screen.height - 640f * UIManager.m_fScaleFactor) / 2f + 640f * UIManager.m_fScaleFactor;
						}
						else if (m_AimPoint2D.y < ((float)Screen.height - 640f * UIManager.m_fScaleFactor) / 2f)
						{
							m_AimPoint2D.y = ((float)Screen.height - 640f * UIManager.m_fScaleFactor) / 2f;
						}
					}
					else
					{
						if (m_AimPoint2D.x > 480f)
						{
							m_AimPoint2D.x = 480f;
						}
						else if (m_AimPoint2D.x < 0f)
						{
							m_AimPoint2D.x = 0f;
						}
						if (m_AimPoint2D.y > 320f)
						{
							m_AimPoint2D.y = 320f;
						}
						else if (m_AimPoint2D.y < 0f)
						{
							m_AimPoint2D.y = 0f;
						}
					}
					float fieldOfView = m_Camera.fieldOfView;
					m_Camera.fieldOfView = cameraCfg.m_fFov;
					m_Camera.transform.LookAt(cameraCfg.m_Lookat);
					m_AimPoint3D = m_Camera.ScreenToWorldPoint(new Vector3(m_AimPoint2D.x, m_AimPoint2D.y, m_Camera.farClipPlane));
					m_Camera.fieldOfView = fieldOfView;
					m_Camera.transform.LookAt(m_AimPoint3D);
					m_fRayTime -= Time.deltaTime;
					if (m_fRayTime <= 0f && m_GameScene.m_UIHelper.IsAim())
					{
						m_fRayTime = 0.33f;
						Vector3 position = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
						Ray ray = m_Camera.ScreenPointToRay(position);
						int layerMask = 512;
						RaycastHit hitInfo;
						if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMask))
						{
							GameObject gameObject = hitInfo.collider.transform.root.gameObject;
							foreach (iSniperNpc value in m_GameScene.getEnemyMap().Values)
							{
								if (value.m_Model == gameObject)
								{
									m_GameScene.m_UIHelper.SetEnemyBlood(value.m_iCurrentHealth, value.m_Property.m_iHeath);
									if (gameObject.name.IndexOf("MoTo") != -1 && hitInfo.collider.name.IndexOf("Moto") != -1)
									{
										iSniperMoTo iSniperMoTo2 = (iSniperMoTo)value;
										m_GameScene.m_UIHelper.SetEnemyBlood(iSniperMoTo2.m_iMoToHealth, value.m_Property.m_iMachineHealth);
									}
								}
							}
							foreach (iSniperNpc value2 in m_GameScene.getPinMinMap().Values)
							{
								if (value2.m_Model == gameObject)
								{
									m_GameScene.m_UIHelper.SetEnemyBlood(value2.m_iCurrentHealth, value2.m_Property.m_iHeath);
								}
							}
							if (gameObject.name.IndexOf("Point") != -1 && m_GameState.m_bBootCampsMode && m_GameScene.m_UIHelper.m_enCampsState == iSniperGameUIHelper.BootCampsSTATE.kAim)
							{
								m_GameScene.m_UIHelper.m_bIsAim = true;
							}
						}
						else
						{
							m_GameScene.m_UIHelper.SetEnemyBlood(0, 0);
						}
					}
				}
			}
			if (m_bShake)
			{
				m_fShakeTime += Time.deltaTime;
				m_Camera.transform.position = cameraCfg.m_Position + UnityEngine.Random.onUnitSphere * 0.3f;
				if (m_fShakeTime > 0.3f)
				{
					m_Camera.transform.position = cameraCfg.m_Position;
					m_bShake = false;
				}
			}
			break;
		}
		case iSniperGameScene.State.kCameraMove:
		{
			iSniperCameraCfg cameraCfg2 = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex - 1);
			iSniperCameraCfg cameraCfg3 = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
			m_fStageSwitchTime += Time.deltaTime;
			float t = m_fStageSwitchTime / 4f;
			float num3 = Mathf.Sin(Mathf.Lerp(0f, (float)Math.PI / 2f, t));
			Vector3 position2 = Vector3.Lerp(cameraCfg2.m_Position, cameraCfg3.m_Position, num3);
			m_Camera.transform.position = position2;
			m_AimPoint3D = Vector3.Lerp(cameraCfg2.m_Lookat, cameraCfg3.m_Lookat, num3);
			m_Camera.transform.LookAt(m_AimPoint3D);
			float fieldOfView2 = Mathf.Lerp(cameraCfg2.m_fFov, cameraCfg3.m_fFov, num3);
			m_Camera.fieldOfView = fieldOfView2;
			if (num3 >= 1f)
			{
				m_GameScene.m_UIHelper.EnterStartState();
			}
			break;
		}
		case iSniperGameScene.State.kMovieDown:
		case iSniperGameScene.State.kGameOver:
			if (m_bHeadShotCamera && m_fHeadShotTime <= 0.25f)
			{
				m_fHeadShotTime += Time.deltaTime;
				float num = m_fHeadShotTime / 0.25f;
				m_Camera.fieldOfView = Mathf.Lerp(m_fHeadShotStartFov, m_fHeadShotStartFov * 0.2f, num);
				if (num >= 1f)
				{
					m_bHeadShotCamera = false;
				}
			}
			break;
		case iSniperGameScene.State.kStageEnd:
			break;
		}
	}

	public void Aim(Vector2 point)
	{
		m_AimPoint2D = point;
		m_StartGravity = Input.acceleration;
		m_AimPoint3D = m_Camera.ScreenToWorldPoint(new Vector3(m_AimPoint2D.x, m_AimPoint2D.y, m_Camera.farClipPlane));
		m_Camera.transform.LookAt(m_AimPoint3D);
		m_AimState = AimState.kDynmaic;
		m_fAimDuringTime = 0f;
	}

	public void Restore()
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperCameraCfg cameraCfg = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
		m_Camera.transform.position = cameraCfg.m_Position;
		m_AimPoint3D = cameraCfg.m_Lookat;
		m_Camera.transform.LookAt(cameraCfg.m_Lookat);
		m_Camera.fieldOfView = cameraCfg.m_fFov;
		m_AimState = AimState.kNoAim;
	}

	public void Shake()
	{
		m_bShake = true;
		m_fShakeTime = 0f;
	}

	private void ComputeMoveSpeed()
	{
		Vector3 vector = Input.acceleration - m_StartGravity;
		int num = 1;
		if (Screen.orientation == ScreenOrientation.LandscapeLeft)
		{
			num = 1;
		}
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
		{
			num = -1;
		}
		float num2 = 0.25f;
		if (m_StartGravity.z > num2 && !m_GameState.m_bIsInvertYAixs)
		{
			m_MoveSpeed.x = (float)(-num) * (vector.y / 0.2f) * m_GameState.m_fCurrentSensitivty;
			m_MoveSpeed.y = (float)(-num) * (vector.x / 0.2f) * m_GameState.m_fCurrentSensitivty;
		}
		if (m_StartGravity.z < 0f - num2 && !m_GameState.m_bIsInvertYAixs)
		{
			m_MoveSpeed.x = (float)(-num) * (vector.y / 0.2f) * m_GameState.m_fCurrentSensitivty;
			m_MoveSpeed.y = (float)num * (vector.x / 0.2f) * m_GameState.m_fCurrentSensitivty;
		}
		if (m_StartGravity.z <= num2 && m_StartGravity.z >= 0f - num2 && !m_GameState.m_bIsInvertYAixs)
		{
			m_MoveSpeed.x = (float)(-num) * (vector.y / 0.2f) * m_GameState.m_fCurrentSensitivty;
			m_MoveSpeed.y = (0f - vector.z / 0.2f) * m_GameState.m_fCurrentSensitivty;
		}
		if (m_StartGravity.z >= num2 && m_GameState.m_bIsInvertYAixs)
		{
			m_MoveSpeed.x = (float)(-num) * (vector.y / 0.2f) * m_GameState.m_fCurrentSensitivty;
			m_MoveSpeed.y = (float)num * (vector.x / 0.2f) * m_GameState.m_fCurrentSensitivty;
		}
		if (m_StartGravity.z < 0f - num2 && m_GameState.m_bIsInvertYAixs)
		{
			m_MoveSpeed.x = (float)(-num) * (vector.y / 0.2f) * m_GameState.m_fCurrentSensitivty;
			m_MoveSpeed.y = (float)(-num) * (vector.x / 0.2f) * m_GameState.m_fCurrentSensitivty;
		}
		if (m_StartGravity.z <= num2 && m_StartGravity.z >= 0f - num2 && m_GameState.m_bIsInvertYAixs)
		{
			m_MoveSpeed.x = (float)num * (vector.y / 0.2f) * m_GameState.m_fCurrentSensitivty;
			m_MoveSpeed.y = vector.z / 0.2f * m_GameState.m_fCurrentSensitivty;
		}
		if (m_GameState.m_bTiltHolding)
		{
			m_MoveSpeed.x *= 0.25f;
			m_MoveSpeed.y *= 0.25f;
		}
	}

	public void JoystickMoveSpeed(float x, float y)
	{
		int num = ((!Utils.IsRetina()) ? 1 : 2);
		float num2 = x / (float)num;
		float num3 = y / (float)num;
		m_MoveSpeed.x = num2 * m_GameState.m_fCurrentSensitivty * 0.1f;
		m_MoveSpeed.y = num3 * m_GameState.m_fCurrentSensitivty * 0.1f;
	}

	public void ResetMoveSpeed()
	{
		m_MoveSpeed.x = 0f;
		m_MoveSpeed.y = 0f;
	}

	public void AdjustFov()
	{
		iSniperGunProperty useGunProperty = m_GameState.GetUseGunProperty();
		m_fAimFov = useGunProperty.GetCurrentZoom() + useGunProperty.GetCurrentZoomDelta() * m_GameState.m_fCurrentGunFovDeltaPercent;
		m_Camera.fieldOfView = m_fAimFov;
	}

	public Vector3 GetCameraPos()
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperCameraCfg cameraCfg = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
		return cameraCfg.m_Position;
	}

	public Vector3 GetLookAtPos()
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperCameraCfg cameraCfg = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
		return cameraCfg.m_Lookat;
	}

	public void ReadyToCameraMove()
	{
		m_fStageSwitchTime = 0f;
	}

	public void HeadShotCamera()
	{
		m_bHeadShotCamera = true;
		m_fHeadShotTime = 0f;
		m_fHeadShotStartFov = m_Camera.fieldOfView;
	}

	public bool IsOutScreen(Vector3 point)
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperCameraCfg cameraCfg = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
		float fieldOfView = m_Camera.fieldOfView;
		m_Camera.fieldOfView = cameraCfg.m_fFov;
		m_Camera.transform.LookAt(cameraCfg.m_Lookat);
		Vector3 vector = m_Camera.WorldToScreenPoint(point);
		m_Camera.fieldOfView = fieldOfView;
		m_Camera.transform.LookAt(m_AimPoint3D);
		if (vector.x >= (float)Screen.width - 5f || vector.x <= 5f || vector.y >= (float)Screen.height - 5f || vector.y <= 5f)
		{
			return true;
		}
		return false;
	}

	public bool JudgeIsOutScreen(Vector3 point)
	{
		int iStageIndex = m_GameState.m_iStageIndex;
		iSniperCameraCfg cameraCfg = m_GameScene.m_SceneDataCfg.GetCameraCfg(iStageIndex);
		m_JudegCam.fieldOfView = cameraCfg.m_fFov;
		m_JudegCam.transform.LookAt(cameraCfg.m_Lookat);
		m_JudegCam.transform.position = cameraCfg.m_Position;
		iSniperNpc iSniperNpc2 = new iSniperNpc();
		Vector3 position = iSniperNpc2.CheckGround(point);
		position.y += 0.01f;
		Vector3 vector = m_JudegCam.WorldToScreenPoint(position);
		if (vector.x >= (float)Screen.width - 5f || vector.x <= 5f || vector.y >= (float)Screen.height - 5f || vector.y <= 5f)
		{
			return true;
		}
		return false;
	}
}
