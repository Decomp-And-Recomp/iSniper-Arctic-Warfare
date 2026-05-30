using UnityEngine;

public class FlashColorScript : MonoBehaviour
{
	public float animationSpeed = 5.5f;

	private float maxBright = 1f;

	private float minBright;

	protected bool increasing = true;

	protected float deltaTime;

	private void Update()
	{
		Color color = base.GetComponent<Renderer>().material.GetColor("_TintColor");
		deltaTime += Time.deltaTime;
		if (deltaTime < 0.05f)
		{
			return;
		}
		if (increasing)
		{
			color.r += animationSpeed * deltaTime;
			color.g += animationSpeed * deltaTime;
			color.b += animationSpeed * deltaTime;
			if (color.r >= maxBright || color.g >= maxBright || color.b >= maxBright)
			{
				increasing = false;
			}
		}
		else
		{
			color.r -= animationSpeed * deltaTime;
			color.g -= animationSpeed * deltaTime;
			color.b -= animationSpeed * deltaTime;
			if (color.r <= minBright || color.g <= minBright || color.b <= minBright)
			{
				increasing = true;
			}
		}
		base.GetComponent<Renderer>().material.SetColor("_TintColor", color);
		deltaTime = 0f;
	}
}
