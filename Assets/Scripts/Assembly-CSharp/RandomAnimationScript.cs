using UnityEngine;

public class RandomAnimationScript : MonoBehaviour
{
	public string m_animationName = "Blow01";

	private void Start()
	{
		base.GetComponent<Animation>()[m_animationName].time = Random.Range(0f, 1f) * base.GetComponent<Animation>()["Blow01"].length;
		base.GetComponent<Animation>()[m_animationName].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play(m_animationName);
	}
}
