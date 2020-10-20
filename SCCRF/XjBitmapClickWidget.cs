// Game.BitmapButtonWidget
using Engine;
using Engine.Media;
using Game;
using System.Xml.Linq;

namespace Game
{
	public class XjBitmapClickWidget : ButtonWidget
	{
		public XjBitmapWidget m_rectangleWidget;

		public RectangleWidget m_imageWidget;

		public LabelWidget m_labelWidget;

		public ClickableWidget m_clickableWidget;

		public Subtexture NormalSubtexture_;

		public Subtexture ClickedSubtexture_;
		

		public Color Color_;

		public override bool IsClicked => m_clickableWidget.IsClicked;

		public override bool IsChecked
		{
			get
			{
				return m_clickableWidget.IsChecked;
			}
			set
			{
				m_clickableWidget.IsChecked = value;
			}
		}

		public override bool IsAutoCheckingEnabled
		{
			get
			{
				return m_clickableWidget.IsAutoCheckingEnabled;
			}
			set
			{
				m_clickableWidget.IsAutoCheckingEnabled = value;
			}
		}

		public override string Text
		{
			get
			{
				return m_labelWidget.Text;
			}
			set
			{
				m_labelWidget.Text = value;
			}
		}

		public override BitmapFont Font
		{
			get
			{
				return m_labelWidget.Font;
			}
			set
			{
				m_labelWidget.Font = value;
			}
		}

		public Subtexture NormalSubtexture
		{
			get
			{
				return NormalSubtexture_;
			}
			set
			{
				NormalSubtexture_ = value;
			}
		}

		public Subtexture ClickedSubtexture
		{
			get
			{
				return ClickedSubtexture_;
			}
			set
			{
				ClickedSubtexture_ = value;
			}
		}

		public override Color Color
		{
			get
			{
				return Color_;
			}
			set
			{
				Color_ = value;
			}
		}
		public  Vector2 mSize
		{
			get
			{
				return m_rectangleWidget.DesiredSize;
			}
			set
			{
				Size = value;
				m_rectangleWidget.DesiredSize = value;
			}
		}
		public XjBitmapClickWidget()
		{
			Color = Color.White;
			XElement node = ContentManager.Get<XElement>("JEIWidgets/xjbitmapclickwidget");
			LoadContents(this,node);
			m_rectangleWidget = Children.Find<XjBitmapWidget>("Button.Xjwidget");
			m_imageWidget = Children.Find<RectangleWidget>("Button.Image");
			m_labelWidget = Children.Find<LabelWidget>("Button.Label");
			m_clickableWidget = Children.Find<ClickableWidget>("Button.Clickable");
		}

		public override void MeasureOverride(Vector2 parentAvailableSize)
		{
			bool flag =IsEnabled;
			m_labelWidget.Color = (flag ? Color : new Color(112, 112, 112));
			m_imageWidget.FillColor = (flag ? Color : new Color(112, 112, 112));
			if (m_clickableWidget.IsPressed || IsChecked)
			{
				if (ClickedSubtexture != null)
				{
					m_rectangleWidget.Texture = ClickedSubtexture.Texture;
				}
				else
				{
					m_rectangleWidget.Texture = NormalSubtexture.Texture;
				}

			}
			else
			{
				m_rectangleWidget.Texture = NormalSubtexture.Texture;

			}
			base.MeasureOverride(parentAvailableSize);
		}
	
	}
}