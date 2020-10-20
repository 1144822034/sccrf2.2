using Engine;
using System.Collections.Generic;
using Engine.Graphics;

namespace Game
{
    public class MessageInfo : CanvasWidget
    {
        public List<string> msgList = new List<string>();
        public StackPanelWidget stackmain = new StackPanelWidget() { Direction=LayoutDirection.Vertical, Margin = new Vector2(20, 200) };
        public CanvasWidget canvasmain = new CanvasWidget() { Size = new Vector2(400, 168) ,Margin=new Vector2(2,1)};
        public CanvasWidget canvasline = new CanvasWidget() { Size = new Vector2(400, 32) ,Margin= new Vector2(2, 2) };
        public RectangleWidget rectangleWidget = new RectangleWidget() { FillColor=new Color(0,0,0,125),OutlineColor=Color.White};
        public RectangleWidget rectangleWidget2 = new RectangleWidget() { FillColor = new Color(0, 0, 0, 125), OutlineColor = Color.White };
        public ScrollPanelWidget scroll = new ScrollPanelWidget() { Direction=LayoutDirection.Vertical};
        public StackPanelWidget stack = new StackPanelWidget() { Direction=LayoutDirection.Vertical};
        public double lastShowTime = 0;
        public bool needShow = false;
        public bool alwaysShow = false;
        public int tran = 125;
        public bool startHidden = false;
        public StackPanelWidget stackline = new StackPanelWidget() { Direction=LayoutDirection.Horizontal};
        public LabelWidget inputa = new LabelWidget() { Text="/>:",Size=new Vector2(32,32),FontScale=0.7f};
        public LabelWidget sendstr = new LabelWidget() { Text = "发送", Size = new Vector2(64, 32) };
        public TextBoxWidget inputtext = new TextBoxWidget() { Size=new Vector2(250,30),FontScale=0.6f,MaximumLength=64};
        public CanvasWidget canvasline_input = new CanvasWidget() { Size = new Vector2(250, 30), Margin = new Vector2(20, 0) };
        public StackPanelWidget inputline = new StackPanelWidget() { Direction=LayoutDirection.Vertical} ;
        public RectangleWidget inputbg = new RectangleWidget() { FillColor=new Color(125,125,125,125)};
        public XjBitmapClickWidget sendtext = new XjBitmapClickWidget() { mSize=new Vector2(32,30),Margin=new Vector2(10,1)};
       

        public ComponentPlayer player;
        public List<string> inputcache = new List<string>();
        public int inputpos;
        public MessageInfo() {
            inputa.Size = new Vector2(XjJeiLibrary.caculateWidth(inputa,inputa.FontScale,inputa.Text,this.Size.X), inputa.Size.Y);
            sendtext.NormalSubtexture =new Subtexture( ContentManager.Get<Texture2D>("JEITextures/JEI_SMSG"),Vector2.Zero,Vector2.One);
            sendtext.ClickedSubtexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_SMSG_P"), Vector2.Zero, Vector2.One);
            this.HorizontalAlignment = WidgetAlignment.Near;
            this.VerticalAlignment = WidgetAlignment.Near;
            this.Size = new Vector2(400, 200);
            this.Children.Add(stackmain);
            stackmain.Children.Add(canvasmain);
            canvasmain.Children.Add(rectangleWidget2);
            canvasmain.Children.Add(scroll);
            scroll.Children.Add(stack);
            scroll.ScrollPosition = 0f;
            stackmain.Children.Add(canvasline);
            canvasline.Children.Add(rectangleWidget);
            canvasline.Children.Add(stackline);
            stackline.Children.Add(inputa);
            stackline.Children.Add(canvasline_input);
            stackline.Children.Add(sendtext);
            canvasline_input.Children.Add(inputbg);
            canvasline_input.Children.Add(inputline);
            inputline.Children.Add(inputtext);
        }
        public void setPlayer(ComponentPlayer p)
        {
            this.player = p;
        }
        public void addMessage(string msg) {

            stack.Children.Add(makeItem(msg));
            lastShowTime = Time.RealTime;
            needShow = true;
        }
        public override void Dispose() {
            this.IsDrawRequired = false;
            while (Children.Count > 0) { Children.Remove(Children[0]); }
        }

        public Color colorTable(string col) {
            switch (col) {
                case "red":
                    return Color.Red;
                case "blue":
                    return Color.Blue;
                case "green":
                    return Color.Green;
                case "lightgreen":
                    return Color.LightGreen;
                case "black":
                    return Color.Black;
                case "white":
                    return Color.White;
                case "yellow":
                    return Color.Yellow;
                default: return Color.Transparent;
            }
        }
        public Widget makeItem(object obj) {
            string msg = (string)obj;
            return matchColor(msg);
        }

        public StackPanelWidget matchColor(string str) {
            StackPanelWidget stackPanel = new StackPanelWidget() { Direction=LayoutDirection.Horizontal};
            string[] ll = str.Split(new string[] {"[color=","]","[/color]" },System.StringSplitOptions.None);
            if (ll[0] == str)
            {
                LabelWidget labelWidget = new LabelWidget() { Text = str, FontScale = 0.6f, WordWrap =true};
                labelWidget.Margin = new Vector2(2, 0);
                labelWidget.Size = new Vector2(XjJeiLibrary.caculateWidth(labelWidget, labelWidget.FontScale, str,canvasmain.Size.X),XjJeiLibrary.caculateHeight(labelWidget, 1, labelWidget.FontScale));
                stackPanel.Children.Add(labelWidget);
            }
            else {
                bool isColor = false;Color cc=Color.White;
                foreach (string ain in ll) {
                    if (!isColor) {                         
                         cc = colorTable(ain);
                        if (cc != Color.Transparent)
                        {
                            isColor = true;

                        }
                        else {
                            LabelWidget labelWidget = new LabelWidget() { Text = ain, FontScale = 0.6f, WordWrap =true };
                            float wid = XjJeiLibrary.caculateWidth(labelWidget, labelWidget.FontScale, ain, canvasmain.Size.X);
                            labelWidget.Size = new Vector2(XjJeiLibrary.caculateWidth(labelWidget, labelWidget.FontScale, ain, canvasmain.Size.X),XjJeiLibrary.caculateHeight(labelWidget,1,labelWidget.FontScale));
                            labelWidget.Margin = new Vector2(2, 0);
                            stackPanel.Children.Add(labelWidget);
                        }
                    }
                    else {
                        LabelWidget labelWidget = new LabelWidget() { Text = ain, FontScale = 0.6f,  Color = cc,WordWrap=true };
                        labelWidget.Size = new Vector2(XjJeiLibrary.caculateWidth(labelWidget, labelWidget.FontScale, ain, canvasmain.Size.X),XjJeiLibrary.caculateHeight(labelWidget, 1, labelWidget.FontScale));
                        labelWidget.Margin = new Vector2(2, 0);
                        stackPanel.Children.Add(labelWidget);
                        isColor = false;
                    }
                }
            
            }

            
            return stackPanel;
        }

        public override void Update()
        {
            if (sendtext.IsClicked)
            {
                if (!string.IsNullOrEmpty(inputtext.Text)) {
                    List<string> parsedata = XjJeiLibrary.parseCommand(inputtext.Text);                    
                    if (parsedata.Count > 0) {
                        string cmd = parsedata[0];
                        switch (cmd) {
                            case "hiddenjump":
                                SubsystemXjJeiBehavior.showjump = false;
                                CellInfoShow.jumpcanvas.IsVisible = false;
                                addMessage("关闭成功");
                                break;
                            case "help":
                                addMessage("[color=green]/hiddenjump[/color] 隐藏跳跃按钮");
                                addMessage("[color=green]/showjump[/color] 显示跳跃按钮");
                                addMessage("[color=green]/changeweather[/color] 改变天气");
                                addMessage("[color=green]/jumpsize[/color] [color=red]10[/color] 改变跳跃按钮大小");
                                addMessage("[color=green]/help[/color] 查看帮助");
                                break;
                            case "jumpsize":
                                if (parsedata.Count == 2)
                                {
                                    int spa = int.Parse(parsedata[1]);
                                    SubsystemXjJeiBehavior.jumpsize = new Vector2(spa,spa);
                                    CellInfoShow.jump.DesiredSize = SubsystemXjJeiBehavior.jumpsize;
                                    CellInfoShow.jumpcanvas.Size = SubsystemXjJeiBehavior.jumpsize;
                                    addMessage("设置成功");
                                }
                                else {
                                    addMessage("参数错误");
                                }
                                break;
                            case "showjump":
                                SubsystemXjJeiBehavior.showjump = true;
                                CellInfoShow.jumpcanvas.IsVisible = true;
                                addMessage("开启成功");
                                break;
                            case "changeweather":
                                addMessage("此功能尚未开发");break;
                                if (parsedata.Count > 1)
                                {
                                    addMessage("更改天气成功");
                                }
                                else {
                                    addMessage("可使用参数'sun' 'rain' 'snow'");
                                }
                                break;
                            default: addMessage($"不能识别的指令'[color=red]{inputtext.Text}[/color]'");break;
                        }
                    }
                    else {
                        addMessage("暂不支持发送聊天消息");
                    }
                    inputcache.Add(inputtext.Text);
                    inputpos = inputcache.Count;
                    scroll.ScrollPosition = scroll.CalculateScrollAreaLength();
                    inputtext.Text = "";
                }
            }
            if (!alwaysShow)
            {
                if (needShow)
                {//需要显示
                    if (Time.RealTime - lastShowTime < 6)
                    {
                        this.m_isVisible = true;
                    }
                    else
                    {
                        this.m_isVisible = false;
                        needShow = false;
                    }
                }
                else
                {
                    this.m_isVisible = false;
                }
            }
            if (stack.Children.Count > 400) stack.Children.Clear();

        }
    }
    }

