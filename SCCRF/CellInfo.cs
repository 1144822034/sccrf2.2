using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class CellInfo:CanvasWidget
    {
        public LabelWidget title = new LabelWidget() { FontScale = 0.6f ,Margin=new Vector2(0,2)};
        public StackPanelWidget stackmain = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
        public StackPanelWidget stackLine1 = new StackPanelWidget() { Direction = LayoutDirection.Vertical , Margin = new Vector2(0, 2) };
        public LabelWidget digValue = new LabelWidget() {  Size = new Vector2(16, 16), FontScale = 0.5f };
        public LabelWidget growValue = new LabelWidget() {  Size = new Vector2(16, 16), FontScale = 0.5f};
        public LabelWidget heatValue = new LabelWidget() {  Size = new Vector2(16, 16), FontScale = 0.5f};
        public LabelWidget nowdig = new LabelWidget() {  Size = new Vector2(16, 16), FontScale = 0.5f };
        public LabelWidget toolValue = new LabelWidget() { Size = new Vector2(16, 16), FontScale = 0.5f };
        public LabelWidget nowtool = new LabelWidget() { Size = new Vector2(16, 16), FontScale = 0.5f };
        public LabelWidget now = new LabelWidget() { Size = new Vector2(16, 16), FontScale = 0.5f };
        public LabelWidget creatureInfo = new LabelWidget() { Size = new Vector2(16, 16), FontScale = 0.6f };
        public LabelWidget creatureHealth = new LabelWidget() { Size = new Vector2(16, 16), FontScale = 0.6f};
        public RectangleWidget rectangleWidget = new RectangleWidget() { FillColor = new Color(0,0,0,127), OutlineColor = Color.White };
        public BlockIconWidget iconWidget = new BlockIconWidget() { Size = new Vector2(40,40),Value=0 };
        public bool upMode = false;
        public ValueBarWidget HealthBarWidget = new ValueBarWidget() {VerticalAlignment=WidgetAlignment.Center, HalfBars = true, BarBlending = false, UnlitBarColor = new Color(64, 64, 64, 255), BarSize = new Vector2(16, 16), TextureLinearFilter = true, BarsCount = 10,LitBarColor=Color.Red};
        public CanvasWidget widget=null;
        public void setBottomWidget(CanvasWidget widget_) {
            if(widget==null)widget = widget_;
        }
        public CellInfo()
        {
            HealthBarWidget.BarSubtexture = TextureAtlasManager.GetSubtexture("Textures/Atlas/HealthBar");
            Size = Vector2.One;
            Children.Add(rectangleWidget);
            Children.Add(stackmain);
        }
        public override void Dispose()
        {
            this.IsDrawRequired = false;
            while (Children.Count > 0) { Children.Remove(Children[0]); }
        }

        public void setUpmode(bool mode) {
            upMode = mode;
            if (mode) {
                VerticalAlignment = WidgetAlignment.Far;
                HorizontalAlignment = WidgetAlignment.Far;
                rectangleWidget.FillColor = Color.Transparent;
                rectangleWidget.OutlineColor = Color.Transparent;
            }
            else {
                VerticalAlignment = WidgetAlignment.Near;
                HorizontalAlignment = WidgetAlignment.Center;
                rectangleWidget.FillColor = new Color(0, 0, 0, 127);
                rectangleWidget.OutlineColor = Color.White;
            }
        }
        public void setCreatureinfo(ComponentCreature componentCreature) {
            if (componentCreature == null) return;
            clearWidgets();
            StackPanelWidget stackLine_healthinfo = new StackPanelWidget() { Direction = LayoutDirection.Vertical, Margin = new Vector2(2, 0) };
            creatureInfo.Text = $"{componentCreature.DisplayName}";
            if (componentCreature.ComponentHealth.Health < 0.2f) { creatureHealth.Color = Color.Red; }
            else if (componentCreature.ComponentHealth.Health < 0.5f) { creatureHealth.Color = Color.Yellow; }
            else { creatureHealth.Color = Color.Green; }
            creatureInfo.Size = new Vector2(XjJeiLibrary.caculateWidth(creatureInfo,creatureInfo.FontScale,creatureInfo.Text,this.Size.X),creatureInfo.Size.Y);
            creatureHealth.Text = $"{componentCreature.ComponentHealth.AttackResilience*componentCreature.ComponentHealth.Health:0.00}";
            stackLine_healthinfo.Children.Add(creatureInfo);
            stackLine_healthinfo.Children.Add(creatureHealth);
            if (!upMode)
            {
                Margin = Vector2.Zero;
                stackmain.Children.Add(stackLine_healthinfo);
                stackmain.Children.Add(HealthBarWidget);                
            }
            else {//右下角模式                
                stackmain.Children.Add(stackLine_healthinfo);
                stackmain.Children.Add(HealthBarWidget);
                Margin = new Vector2(0, widget.DesiredSize.Y+SubsystemXjJeiBehavior.margin.Y);
            }
            HealthBarWidget.Value = componentCreature.ComponentHealth.Health;
            this.Size = new Vector2(HealthBarWidget.ActualSize.X+creatureInfo.ActualSize.X+4, creatureHealth.Size.Y+creatureInfo.Size.Y+4);
        }
        public string getGrowInfo(int grow) {
            switch (grow) {
                case 14: return "发芽期";
                case 42: return "生长期";
                case 71: return "成熟期";
                case 100: return "已成熟";
                default:return "生长中";
            }
        }
        public void setBlockInfo(string txt,int value,int dig,BlockDigMethod digmethod,int nowdig)
        {
            clearWidgets();
            if (!upMode)
            {
                title.Text = txt;
                iconWidget.Value = value;
                if (nowdig < dig) digValue.Color = Color.Red; else digValue.Color = Color.White;
                digValue.Text = $"{dig}";
                switch (digmethod)
                {
                    case BlockDigMethod.None:
                        toolValue.Text = "无";
                        break;

                    case BlockDigMethod.Hack:
                        toolValue.Text = $"斧";
                        break;
                    case BlockDigMethod.Shovel:
                        toolValue.Text = $"铲";
                        break;

                    case BlockDigMethod.Quarry:
                        toolValue.Text = $"镐";
                        break;
                }
                if (toolValue.Text != "无") title.Text = txt + $"({toolValue.Text})" + "\nID:" + Terrain.ExtractContents(value)+" Data:"+Terrain.ExtractData(value);
                else title.Text = txt+"\nID:"+Terrain.ExtractContents(value);
                toolValue.FontScale = 0.5f;
                iconWidget.Scale = 1f;
                iconWidget.Value = value;
                stackmain.HorizontalAlignment = WidgetAlignment.Center;
                title.HorizontalAlignment = WidgetAlignment.Far;
                toolValue.Size= new Vector2(XjJeiLibrary.caculateWidth(toolValue, toolValue.FontScale, toolValue.Text, this.Size.X), XjJeiLibrary.caculateHeight(toolValue,toolValue.m_lines.Count, toolValue.FontScale));
                title.Size = new Vector2(XjJeiLibrary.caculateWidth(title, title.FontScale, title.Text, this.Size.X), XjJeiLibrary.caculateHeight(title, title.m_lines.Count, title.FontScale));
                stackmain.Children.Add(iconWidget);
                stackmain.Children.Add(title);
                Margin = Vector2.Zero;
                float yya;
                if (iconWidget.Size.Y >= title.Size.Y) { yya = iconWidget.Size.Y; }
                else yya = title.Size.Y;

                Size = new Vector2(title.Size.X + 40 + iconWidget.Size.X, yya);
            }
            else {
                title.Text = txt;
                title.Size = new Vector2(XjJeiLibrary.caculateWidth(title, title.FontScale, title.Text, this.Size.X), XjJeiLibrary.caculateHeight(title, title.m_lines.Count, title.FontScale));
                stackmain.Children.Add(title);
                Margin = new Vector2(widget.Size.X+ SubsystemXjJeiBehavior.margin.X, SubsystemXjJeiBehavior.margin.Y + widget.Size.Y);
                Size = title.Size;
            }
        }
        public void setPlantInfo(string txt,int value, int grow)
        {
            clearWidgets();
            if (!upMode)
            {
                title.Text = txt + $"({getGrowInfo(grow)})" + "\nID:" + Terrain.ExtractContents(value) + " Data:" + Terrain.ExtractData(value);
                iconWidget.Value = value;
                iconWidget.Scale = 0.7f;
                title.Size = new Vector2(XjJeiLibrary.caculateWidth(title, title.FontScale, title.Text, this.Size.X), XjJeiLibrary.caculateHeight(title, title.m_lines.Count, title.FontScale));
                stackmain.Direction = LayoutDirection.Horizontal;
                stackmain.HorizontalAlignment = WidgetAlignment.Center;
                stackmain.Children.Add(iconWidget);
                stackmain.Children.Add(title);
                Margin = Vector2.Zero;
                float yya;
                if (iconWidget.Size.Y >= title.Size.Y) { yya = iconWidget.Size.Y; }
                else yya = title.Size.Y;
                Size = new Vector2(title.Size.X+40+iconWidget.Size.X,yya);
            }
            else {
                title.Text = txt + $"({getGrowInfo(grow)})";
                title.Size = new Vector2(XjJeiLibrary.caculateWidth(title,title.FontScale,title.Text, this.Size.X), XjJeiLibrary.caculateHeight(title, title.m_lines.Count, title.FontScale));
                title.HorizontalAlignment = WidgetAlignment.Near;               
                stackmain.HorizontalAlignment = WidgetAlignment.Near;
                stackmain.Children.Add(title);
                Margin = new Vector2(widget.Size.X + SubsystemXjJeiBehavior.margin.X, SubsystemXjJeiBehavior.margin.Y+ widget.DesiredSize.Y);
                Size = title.Size;
            }
        }
        public void clearWidgets() {
            while (stackmain.Children.Count>0) { stackmain.Children.Remove(stackmain.Children[0]); }        
        }
    }
}
