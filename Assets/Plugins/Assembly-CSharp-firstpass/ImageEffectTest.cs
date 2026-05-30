using UnityEngine;

[AddComponentMenu("Image Effects/Fuild")]
[ExecuteInEditMode]
public class ImageEffectTest : ImageEffectBase
{
	protected new void Start()
	{
		if (!SystemInfo.supportsRenderTextures)
		{
			base.enabled = false;
		}
		else
		{
			base.Start();
		}
	}

	protected new void OnDisable()
	{
		base.OnDisable();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, base.material);
	}
}
