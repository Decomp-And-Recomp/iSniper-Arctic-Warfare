using UnityEngine;

public class UIPressButton : UIButtonBase
{
	public enum Command
	{
		Click = 0,
		Pressing = 1
	}

	protected int m_FingerId;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			SetSpritePosition(0, position);
			SetSpritePosition(1, position);
			SetSpritePosition(2, position);
		}
	}

	public UIPressButton()
	{
		m_FingerId = -1;
	}

	public void Reset()
	{
		m_State = State.Normal;
		m_FingerId = -1;
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_State = State.Pressed;
				m_FingerId = touch.fingerId;
				m_Parent.SendEvent(this, 1, 0f, 0f);
				return true;
			}
			return false;
		}
		if (touch.fingerId == m_FingerId)
		{
			if (touch.phase == TouchPhase.Moved)
			{
				m_State = State.Pressed;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				m_State = State.Normal;
				m_FingerId = -1;
				m_Parent.SendEvent(this, 0, 0f, 0f);
			}
			return true;
		}
		return false;
	}
}
