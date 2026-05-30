using UnityEngine;

public class UIControl
{
	protected UIContainer m_Parent;

	protected int m_Id;

	protected Rect m_Rect;

	protected bool m_Visible;

	protected bool m_Enable;

	protected bool m_Clip;

	protected Rect m_ClipRect;

	public int Id
	{
		get
		{
			return m_Id;
		}
		set
		{
			m_Id = value;
		}
	}

	public Rect Rect
	{
		get
		{
			return m_Rect;
		}
		set
		{
			m_Rect = value;
		}
	}

	public bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			m_Visible = value;
		}
	}

	public bool Enable
	{
		get
		{
			return m_Enable;
		}
		set
		{
			m_Enable = value;
		}
	}

	public UIControl()
	{
		m_Parent = null;
		m_Id = 0;
		m_Rect = new Rect(0f, 0f, 0f, 0f);
		m_Visible = true;
		m_Enable = true;
	}

	public void SetParent(UIContainer parent)
	{
		m_Parent = parent;
	}

	public void SetClip(Rect clip_rect)
	{
		m_Clip = true;
		m_ClipRect = clip_rect;
	}

	public void ClearClip()
	{
		m_Clip = false;
	}

	public virtual bool PtInRect(Vector2 pt)
	{
		float fScaleFactor = UIManager.m_fScaleFactor;
		float num = 960f * fScaleFactor;
		float num2 = 640f * fScaleFactor;
		float num3 = 0f;
		float num4 = 0f;
		if ((float)Screen.width > num)
		{
			num3 = ((float)Screen.width - num) / 2f;
		}
		if ((float)Screen.height > num2)
		{
			num4 = ((float)Screen.height - num2) / 2f;
		}
		Vector2 vector = pt;
		vector.x -= num3;
		vector.y -= num4;
		pt = vector;
		if (pt.x >= m_Rect.xMin * fScaleFactor && pt.x < m_Rect.xMax * fScaleFactor && pt.y >= m_Rect.yMin * fScaleFactor && pt.y < m_Rect.yMax * fScaleFactor)
		{
			if (m_Clip)
			{
				return pt.x >= m_ClipRect.xMin && pt.x < m_ClipRect.xMax && pt.y >= m_ClipRect.yMin && pt.y < m_ClipRect.yMax;
			}
			return true;
		}
		return false;
	}

	public virtual void Draw()
	{
	}

	public virtual void Update()
	{
	}

	public virtual bool HandleInput(UITouchInner touch)
	{
		return false;
	}
}
