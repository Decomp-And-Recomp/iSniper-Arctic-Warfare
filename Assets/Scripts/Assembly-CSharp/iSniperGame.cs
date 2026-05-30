using UnityEngine;

public class iSniperGame : MonoBehaviour
{
	public iSniperGameScene m_GameScene;

	private void Awake()
	{
		m_GameScene = new iSniperGameScene();
		m_GameScene.Initialize();
	}

	private void Update()
	{
		m_GameScene.DoLogic(Time.deltaTime);
	}
}
