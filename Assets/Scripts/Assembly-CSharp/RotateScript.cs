using UnityEngine;

public class RotateScript : MonoBehaviour
{
	private void Update()
	{
		base.transform.RotateAroundLocal(base.transform.forward, Time.deltaTime);
	}
}
