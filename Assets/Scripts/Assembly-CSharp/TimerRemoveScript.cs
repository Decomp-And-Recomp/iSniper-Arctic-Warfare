using UnityEngine;

public class TimerRemoveScript : MonoBehaviour
{
	public float life;

	protected float createdTime;

	private void Start()
	{
		createdTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - createdTime > life)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
