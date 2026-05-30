using UnityEngine;

public class iSniperShadow : MonoBehaviour
{
	public Material m_Material;

	private MeshFilter m_MeshFilter;

	private MeshRenderer m_MeshRenderer;

	public Vector2 m_Size = new Vector2(10f, 10f);

	private GameObject m_RootObj;

	private void Start()
	{
		m_MeshFilter = (MeshFilter)base.gameObject.AddComponent(typeof(MeshFilter));
		m_MeshRenderer = (MeshRenderer)base.gameObject.AddComponent(typeof(MeshRenderer));
		m_MeshRenderer.castShadows = false;
		m_MeshRenderer.receiveShadows = false;
		m_MeshRenderer.GetComponent<Renderer>().material = m_Material;
		m_RootObj = base.transform.root.gameObject;
	}

	private void Update()
	{
		m_MeshFilter.mesh.Clear();
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		Color[] array3 = new Color[4];
		int[] array4 = new int[6];
		array[0] = -m_RootObj.transform.right * m_Size.x + m_RootObj.transform.forward * m_Size.y;
		array[1] = -m_RootObj.transform.right * m_Size.x - m_RootObj.transform.forward * m_Size.y;
		array[2] = m_RootObj.transform.right * m_Size.x - m_RootObj.transform.forward * m_Size.y;
		array[3] = m_RootObj.transform.right * m_Size.x + m_RootObj.transform.forward * m_Size.y;
		array2[0] = new Vector2(0f, 0f);
		array2[1] = new Vector2(0f, 1f);
		array2[2] = new Vector2(1f, 1f);
		array2[3] = new Vector2(1f, 0f);
		Color color = new Color(1f, 1f, 1f, 1f);
		array3[0] = color;
		array3[1] = color;
		array3[2] = color;
		array3[3] = color;
		array4[0] = 0;
		array4[1] = 1;
		array4[2] = 3;
		array4[3] = 3;
		array4[4] = 1;
		array4[5] = 2;
		m_MeshFilter.mesh.vertices = array;
		m_MeshFilter.mesh.uv = array2;
		m_MeshFilter.mesh.colors = array3;
		m_MeshFilter.mesh.triangles = array4;
	}
}
