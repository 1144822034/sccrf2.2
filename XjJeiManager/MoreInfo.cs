using Engine;
using Engine.Graphics;

namespace Game
{
    public class MoreInfo:CanvasWidget
    {
        public StackPanelWidget stackPanelWidget = new StackPanelWidget() { Direction=LayoutDirection.Vertical};
        public StackPanelWidget shuxing = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
        public XjBitmapWidget bitmapWidget = new XjBitmapWidget() { DesiredSize=new Vector2(16,16)};
        public LabelWidget labelWidget = new LabelWidget() { Text="",Size=new Vector2(16,16),FontScale=0.5f,Margin=new Vector2(6,0)};
        public LabelWidget position = new LabelWidget() { FontScale=0.5f};
        public LabelWidget naijiu = new LabelWidget() { FontScale=0.5f};
        public Texture2D texture = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_4");
        public MoreInfo() {
            this.HorizontalAlignment = WidgetAlignment.Far;
            this.VerticalAlignment = WidgetAlignment.Far;
            this.Children.Add(stackPanelWidget);
            shuxing.Children.Add(bitmapWidget);
            shuxing.Children.Add(labelWidget);
            stackPanelWidget.Children.Add(shuxing);
            stackPanelWidget.Children.Add(naijiu);
            stackPanelWidget.Children.Add(position);
        }
        public override void Dispose()
        {
            this.IsDrawRequired = false;
            while (Children.Count > 0) { Children.Remove(Children[0]); }
        }
        public void setShow(bool a,bool b)
        {
            position.IsVisible = a;
            naijiu.IsVisible = b;
            labelWidget.IsVisible = b;
        }

        public void setPosi(Vector3 position_) {
            float w;
            position.Text = $"({(int)position_.X},{(int)position_.Y},{(int)position_.Z})";
            position.Size = new Vector2(XjJeiLibrary.caculateWidth(position, 0.5f, position.Text,this.Size.X), 16);
            if (XjJeiLibrary.caculateWidth(naijiu, 0.5f, naijiu.Text, this.Size.X) >= XjJeiLibrary.caculateWidth(position, 0.5f, position.Text, this.Size.X))
            {
                w = XjJeiLibrary.caculateWidth(naijiu, 0.5f, naijiu.Text, this.Size.X);
            }
            else
            {
                w = XjJeiLibrary.caculateWidth(position, 0.5f, position.Text, this.Size.X);
            }
            this.Size = new Vector2(w, shuxing.ActualSize.Y + naijiu.Size.Y);
            this.Margin = SubsystemXjJeiBehavior.margin;
        }
        public void setInfo(string name,int naijiu_,float attack) {
            labelWidget.Text = $"{attack.ToString():0.00}";
            naijiu.Text =$"{name} 耐久："+ naijiu_.ToString();
            naijiu.Size = new Vector2(XjJeiLibrary.caculateWidth(naijiu, 0.5f, naijiu.Text, this.Size.X), 16);
            Size = new Vector2(naijiu.Size.X+shuxing.ActualSize.X, shuxing.ActualSize.Y+naijiu.Size.Y);
            Margin = SubsystemXjJeiBehavior.margin;
        }
    }
}
