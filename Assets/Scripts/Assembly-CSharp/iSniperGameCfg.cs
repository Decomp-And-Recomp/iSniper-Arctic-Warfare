using System.Collections;
using UnityEngine;

public class iSniperGameCfg : MonoBehaviour
{
	public ArrayList m_ArrayStageCfgs;

	public ArrayList m_ArrayCameraCfgs;

	private void Awake()
	{
		m_ArrayStageCfgs = new ArrayList();
		m_ArrayCameraCfgs = new ArrayList();
		GameObject gameObject = GameObject.Find("Scene_DATA");
		int childCount = gameObject.transform.GetChildCount();
		for (int i = 1; i <= childCount; i++)
		{
			GameObject gameObject2 = GameObject.Find("Stage" + i);
			iSniperStageCfg value = gameObject2.GetComponent("iSniperStageCfg") as iSniperStageCfg;
			m_ArrayStageCfgs.Add(value);
			iSniperCameraCfg value2 = gameObject2.GetComponent("iSniperCameraCfg") as iSniperCameraCfg;
			m_ArrayCameraCfgs.Add(value2);
		}
	}

	public iSniperStageCfg GetStageCfg(int index)
	{
		return (iSniperStageCfg)m_ArrayStageCfgs[index - 1];
	}

	public iSniperCameraCfg GetCameraCfg(int index)
	{
		return (iSniperCameraCfg)m_ArrayCameraCfgs[index - 1];
	}
}
