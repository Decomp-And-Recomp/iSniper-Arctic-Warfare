using System.Collections;
using UnityEngine;

public class MeshControllerScript : MonoBehaviour
{
	protected Hashtable m_SkinRenderMap;

	private void Awake()
	{
		m_SkinRenderMap = new Hashtable();
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)componentsInChildren[i];
			skinnedMeshRenderer.updateWhenOffscreen = true;
			m_SkinRenderMap.Add(skinnedMeshRenderer.name, skinnedMeshRenderer);
		}
	}

	public void SetTexture(string name, Material material)
	{
		((SkinnedMeshRenderer)m_SkinRenderMap[name]).material = material;
	}

	public void SetShader(Shader shader)
	{
		foreach (SkinnedMeshRenderer value in m_SkinRenderMap.Values)
		{
			value.material.shader = shader;
		}
	}

	public void SetMaterialColor(string propName, Color clr)
	{
		foreach (SkinnedMeshRenderer value in m_SkinRenderMap.Values)
		{
			value.material.SetColor(propName, clr);
		}
	}

	public void ShowPart(string name, bool bVisible)
	{
		((SkinnedMeshRenderer)m_SkinRenderMap[name]).gameObject.active = bVisible;
	}

	public void HideObject()
	{
		foreach (SkinnedMeshRenderer value in m_SkinRenderMap.Values)
		{
			value.gameObject.active = false;
		}
	}
}
