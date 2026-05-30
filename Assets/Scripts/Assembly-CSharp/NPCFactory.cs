using UnityEngine;

public class NPCFactory
{
	public static GameObject CreateEnemy(string strType, string strName, Vector3 position)
	{
		string text = strType;
		if ("YouJi" == strType || "ChongFeng" == strType)
		{
			text = "JingJie";
		}
		else if ("PinMin" == strType || "YinBao" == strType)
		{
			text = "PinMin" + Random.Range(1, 4);
		}
		string path = "iSniper3D/Prefabs/Npcs/" + text + "_Prefab";
		GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		gameObject.name = strName;
		MeshControllerScript meshControllerScript = gameObject.GetComponent("MeshControllerScript") as MeshControllerScript;
		int num = ((iSniperGameApp.GetInstance().m_GameState.m_bArcadeIsLock || iSniperGameApp.GetInstance().m_GameState.m_bStoryMode) ? iSniperGameApp.GetInstance().m_GameState.m_iPlayerCurrentScene : iSniperGameApp.GetInstance().m_GameState.m_iArcCurScene);
		bool flag = 3 <= num && num <= 6;
		switch (strType)
		{
		case "JuJi":
			meshControllerScript.SetTexture("Enemy", GetMaterial((!flag) ? "JuJi1" : "JuJi2"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("Agp"));
			break;
		case "JingJie":
		case "YouJi":
		case "ChongFeng":
			meshControllerScript.SetTexture("Enemy", GetMaterial((!flag) ? "JingJie1" : "JingJie2"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("Mp5"));
			break;
		case "XunLuo":
		case "MoTo":
			meshControllerScript.SetTexture("Enemy", GetMaterial((!flag) ? "XunLuo1" : "XunLuo2"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("Mp5"));
			break;
		case "SaoShe":
			meshControllerScript.SetTexture("Enemy", GetMaterial((!flag) ? "SaoShe1" : "SaoShe2"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("AK"));
			break;
		case "ZhuangJia":
			meshControllerScript.SetTexture("Enemy", GetMaterial("ZhuangJia"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("HeavyGun"));
			break;
		case "DuoCang":
			meshControllerScript.SetTexture("Enemy", GetMaterial("DuoCang"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("H97"));
			break;
		case "YinHu":
			meshControllerScript.SetTexture("Enemy", GetMaterial("YinHu"));
			meshControllerScript.SetTexture("Weapon", GetMaterial("Agp"));
			break;
		case "PinMin":
		{
			meshControllerScript.ShowPart("Trigger", false);
			int num3 = Random.Range(0, 1000);
			if ("PinMin1" == text)
			{
				num3 %= 2;
				meshControllerScript.SetTexture("PingMin01", GetMaterial("PinMin1_" + (num3 + 1)));
			}
			else if ("PinMin2" == text)
			{
				num3 %= 4;
				meshControllerScript.SetTexture("PingMin02", GetMaterial("PinMin2_" + (num3 + 1)));
			}
			else if ("PinMin3" == text)
			{
				num3 %= 2;
				meshControllerScript.SetTexture("PingMin03", GetMaterial("PinMin3_" + (num3 + 1)));
			}
			break;
		}
		case "YinBao":
		{
			int num2 = Random.Range(0, 1000);
			if ("PinMin1" == text)
			{
				num2 %= 2;
				meshControllerScript.SetTexture("PingMin01", GetMaterial("PinMin1_" + (num2 + 1)));
			}
			else if ("PinMin2" == text)
			{
				num2 %= 4;
				meshControllerScript.SetTexture("PingMin02", GetMaterial("PinMin2_" + (num2 + 1)));
			}
			else if ("PinMin3" == text)
			{
				num2 %= 2;
				meshControllerScript.SetTexture("PingMin03", GetMaterial("PinMin3_" + (num2 + 1)));
			}
			meshControllerScript.SetTexture("Trigger", GetMaterial("Trigger"));
			break;
		}
		}
		return gameObject;
	}

	private static Material GetMaterial(string name)
	{
		return Resources.Load<Material>("iSniper3D/Models/Npcs/" + name + "_M");
	}
}
