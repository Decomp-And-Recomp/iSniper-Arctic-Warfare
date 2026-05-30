using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInput()
    {
        GameObject input = new GameObject("PC Input");
        input.AddComponent<PCInput>();

        DontDestroyOnLoad(input);
    }

	private static Touch[] fakeTouches = new Touch[0];

	public static Touch[] touches
	{
		get
		{
			if (Application.isMobilePlatform) return Input.touches;
											  return fakeTouches;
		}
	}

	public static int touchCount
	{
		get
		{
			if (Application.isMobilePlatform) return Input.touchCount;
											  return fakeTouches.Length;
		}
	}

	public static Touch GetTouch(int index)
	{
		if (Application.isMobilePlatform) return Input.GetTouch(index);
										  return touches[index];
	}

	private static Touch touch = new Touch() { fingerId = 0, phase = TouchPhase.Ended };

	private void Update()
	{
		Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		bool ended = touch.phase == TouchPhase.Ended;

		if (Input.GetMouseButtonDown(0))
		{
			touch.phase = TouchPhase.Began;
		}
		else if (Input.GetMouseButton(0))
		{
			touch.phase = touch.position == mousePosition ? TouchPhase.Stationary : TouchPhase.Moved;
		}

		if (Input.GetMouseButtonUp(0))
		{
			touch.phase = TouchPhase.Ended;
		}

		if (ended && touch.phase == TouchPhase.Ended)
		{
			fakeTouches = new Touch[0];
		}
		else
		{
			touch.deltaPosition = mousePosition - touch.position;

			if (touch.phase == TouchPhase.Stationary) touch.deltaTime += Time.deltaTime;
			else 									  touch.deltaTime  = Time.deltaTime;

			touch.position = mousePosition;

			fakeTouches = new Touch[1] { touch };
		}
	}
}
