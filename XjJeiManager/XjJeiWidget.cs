using Engine;
using Engine.Content;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Game
{
    public class XjJEIWidget : CanvasWidget
    {

        private StackPanelWidget stackPanelWidget;
        public SubsystemXjJeiBehavior xj;
        public Color daybg= new Color(0, 0, 0, 125);
        public Color nightbg = new Color(125,125,125,44);
        private TextBoxWidget boxWidget;
        private List<XjBitmapClickWidget> btns = new List<XjBitmapClickWidget>();
        private XjJeiManager xjJeiManager = new XjJeiManager();
        private List<XjBitmapClickWidget> nbtns = new List<XjBitmapClickWidget>();
        private List<BlockIconWidget> xjJeiBmpWidgets = new List<BlockIconWidget>();//合成谱的九宫格视图
        private LabelWidget pageLabel;
        private int showpos = 0;
        private CanvasWidget mainview = new CanvasWidget();
        private StackPanelWidget rcip_stack = new StackPanelWidget() { Direction = LayoutDirection.Vertical, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center, Margin = new Vector2(40, 0) };//工具视图
        private StackPanelWidget nine_stack = new StackPanelWidget() { Direction = LayoutDirection.Vertical, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Near };
        private int allnum = 0;//总blocksData数
        private int row = 4;//行数
        private int col = 9;//列数
        private int allPage = 0,nowPage = 0,allrcps = 0;
        private List<int> global_search_lists=new List<int>();
        private List<int> clicked_blocks_lists = new List<int>();
        private bool inSer = false;
        private bool inSetting = false;
        private bool isBack = false;//是否在执行返回上一级操作
        private int clickpos = 0;//表示每个方块的绝对坐标
        private List<CraftingRecipe> craftings=new List<CraftingRecipe>();
        private List<Color> colorTabels = new List<Color>() { 
        Color.Black,Color.Blue,Color.Cyan,Color.DarkBlue,Color.DarkCyan,Color.DarkGreen,
            Color.DarkMagenta,Color.DarkRed,Color.DarkYellow,Color.Gray,Color.Green,
            Color.LightBlue,Color.LightCyan,Color.LightGray,Color.LightGreen,Color.LightMagenta,
            Color.LightRed,Color.LightYellow,Color.Magenta,Color.Red,Color.White,Color.Yellow
        };
        private int selectColorPos = 0;
        private XjTimer xjTimer = new XjTimer(1100);
        private int timcnt = 0;
        private List<List<int>> rcplists=new List<List<int>>();
        public int viewwhich = 0;
        private TextBoxWidget inputbox = new TextBoxWidget() { Size = new Vector2(200, 48), Color = Color.Gray };//地点保存框
        public List<CraftingRecipe> craftingRecipe = new List<CraftingRecipe>();
        private XjBitmapWidget JeiHead = new XjBitmapWidget();
        public ComponentCraftingTable componentCraftingTable = null;
        public ComponentInventoryBase componentInventory = null;
        public ComponentFurnace componentFurnace = null;
        public SubsystemTerrain subsystemTerrain=null;
        public SubsystemGameInfo subsystemGameInfo=null;
        public SubsystemTimeOfDay subsystemTimeOfDay = null;
        private XjBitmapClickWidget bevelled_index,bevelled_next, bevelled_back,btn_bck, btn_nck,CellInfoSettings;
        private BevelledButtonWidget savePoint = new BevelledButtonWidget() { Text = "保存地点", HorizontalAlignment = WidgetAlignment.Far,Size=new Vector2(160,48)};
        private BevelledButtonWidget ColorSelect = new BevelledButtonWidget() { Text = "颜色", Size=new Vector2(88,48) ,Margin=new Vector2(10,0)};
        private BevelledButtonWidget DeleteSelectedPoint = new BevelledButtonWidget() { Text = "删除选中", Size = new Vector2(160, 48), Margin = new Vector2(10, 0) };
        private Subtexture subtexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Box_1"), new Vector2(0, 0), new Vector2(2, 2));
        private Texture2D headTexture = ContentManager.Get<Texture2D>("JEITextures/JEI_Head_1");
        private Texture2D pointsTexture = ContentManager.Get<Texture2D>("JEITextures/JEI_Points");
        private Texture2D prop_Health= ContentManager.Get<Texture2D>("JEITextures/JEI_Property_1");
        private Texture2D prop_LongAtk = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_2");
        private Texture2D prop_Defense = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_3");
        private Texture2D prop_NearAtk = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_4");
        private Texture2D prop_Light = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_5");
        private Texture2D prop_Food = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_6");
        private Texture2D prop_Burn = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_7");
        private Texture2D prop_MoveSpd = ContentManager.Get<Texture2D>("JEITextures/JEI_Property_8");
        private Subtexture fastTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Fast_1"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture jeiSettings = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Settings"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture fastmoveTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Fast_2"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture indexTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Index_1"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture switchonTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Switch_On"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture switchoffTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Switch_Off"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture leftTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Left"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture rightTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Right"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture upTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Up"), new Vector2(0, 0), new Vector2(1, 1));
        private Subtexture downTexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_Down"), new Vector2(0, 0), new Vector2(1, 1));
        private XjBitmapClickWidget fastCraft = new XjBitmapClickWidget() {Size=new Vector2(22,22),HorizontalAlignment=WidgetAlignment.Center,VerticalAlignment=WidgetAlignment.Far,Margin=new Vector2(10,0)};
        private XjBitmapClickWidget fastMove = new XjBitmapClickWidget() { Size = new Vector2(22, 22), HorizontalAlignment = WidgetAlignment.Near, VerticalAlignment = WidgetAlignment.Far, Margin = new Vector2(10, 0) };
        private CanvasWidget canvasWidget_head;
        private int connCraftPos = 0;
        private XjBitmapClickWidget xjBitmapClickWidget= new XjBitmapClickWidget();
        private XjBitmapClickWidget xjBitmapClickWidget_posi = new XjBitmapClickWidget();
        private XjBitmapClickWidget xjBitmapClickWidget_dura = new XjBitmapClickWidget();
        private XjBitmapClickWidget xjBitmapClickWidget_upmode = new XjBitmapClickWidget();
        private XjBitmapClickWidget jeiPoints = new XjBitmapClickWidget();
        private XjBitmapClickWidget connCraftBtn_next = new XjBitmapClickWidget() { VerticalAlignment=WidgetAlignment.Near};
        private XjBitmapClickWidget connCraftBtn_back = new XjBitmapClickWidget() { VerticalAlignment=WidgetAlignment.Far};
        private SliderWidget slidera = new SliderWidget() { MinValue = 0, MaxValue = 255, Size = new Vector2(400, 36), Granularity = 1 };
        private SliderWidget sliderb = new SliderWidget() { MinValue = 0, MaxValue = 255, Size = new Vector2(400, 36), Granularity = 1 };
        private SliderWidget sliderc = new SliderWidget() { MinValue = 0, MaxValue = 255, Size = new Vector2(400, 36), Granularity = 1 };
        private SliderWidget sliderd = new SliderWidget() { MinValue = 0, MaxValue = 512, Size = new Vector2(400, 36), Granularity = 1 };
        private SliderWidget slidere = new SliderWidget() { MinValue = 0, MaxValue = 512, Size = new Vector2(400, 36), Granularity = 1 };
        private RectangleWidget rectangleWidget_body, rectangleWidget_search;
        private FireWidget fireWidget  = new FireWidget() { Size = new Vector2(128, 64), ParticlesPerSecond =0 };
        private LabelWidget progess = new LabelWidget() {Size=new Vector2(64,64) };
        private ListPanelWidget listPanelWidget = new ListPanelWidget();
        private List<XjBitmapClickWidget> connCraftBtns = new List<XjBitmapClickWidget>();
        private List<int> connCraftBlocks = new List<int>();
        private StackPanelWidget stackVer = new StackPanelWidget() { Direction = LayoutDirection.Vertical,HorizontalAlignment=WidgetAlignment.Far,VerticalAlignment=WidgetAlignment.Center };
        private bool inProperty = false;
        public XjJEIWidget(SubsystemXjJeiBehavior xjx)
        {
            xj = xjx;
            xjJeiManager.init();
            XElement node = ContentManager.Get<XElement>("JEIWidgets/main");
            LoadContents(this,node);
            mainview = (CanvasWidget)Children.Find("mainview");
            stackPanelWidget = (StackPanelWidget)Children.Find("jeicontent");
            boxWidget = (TextBoxWidget)Children.Find("search");
            boxWidget.Color = Color.LightBlue;
            pageLabel = (LabelWidget)Children.Find("Page");
            JeiHead = (XjBitmapWidget)Children.Find("JeiHead");
            fastCraft.NormalSubtexture = fastTexture;
            fastMove.NormalSubtexture = fastmoveTexture;
            JeiHead.DesiredSize = new Vector2(660, 60);
            JeiHead.Texture = headTexture;
            bevelled_index = (XjBitmapClickWidget)Children.Find("JeiIndex");
            bevelled_index.NormalSubtexture = indexTexture;
            CellInfoSettings = (XjBitmapClickWidget)Children.Find("JEI_Settings");
            CellInfoSettings.Size = new Vector2(48,48);
            CellInfoSettings.NormalSubtexture =jeiSettings;
            jeiPoints = (XjBitmapClickWidget)Children.Find("JEI_Points");
            jeiPoints.Size = new Vector2(48, 48);
            jeiPoints.NormalSubtexture = new Subtexture(pointsTexture,Vector2.Zero,Vector2.One);
            ColorSelect.CenterColor = colorTabels[0];
            bevelled_next = (XjBitmapClickWidget)Children.Find("btn_next");
            bevelled_next.NormalSubtexture = rightTexture;
            connCraftBtn_back.Size = new Vector2(38);
            connCraftBtn_back.NormalSubtexture = upTexture;
            connCraftBtn_next.Size = new Vector2(38);
            connCraftBtn_next.NormalSubtexture = downTexture;
            bevelled_back = (XjBitmapClickWidget)Children.Find("btn_back");
            bevelled_back.NormalSubtexture = leftTexture;
            boxWidget.TextChanged += Textchange;            
            boxWidget.Color = Color.White;
            stackPanelWidget.Direction = LayoutDirection.Vertical;
            boxWidget.Text = "搜索物品";
            boxWidget.Enter += TextEnter;
            canvasWidget_head = Children.Find<CanvasWidget>("Canvas_head");
            rectangleWidget_body = Children.Find<RectangleWidget>("Body_bg");
            rectangleWidget_search = Children.Find<RectangleWidget>("Search_bg");
            subsystemTerrain = xj.player.Project.FindSubsystem<SubsystemTerrain>();
            subsystemGameInfo = xj.player.Project.FindSubsystem<SubsystemGameInfo>();
            subsystemTimeOfDay = xj.player.Project.FindSubsystem<SubsystemTimeOfDay>();
            listPanelWidget.Margin = new Vector2(0, 5);
            listPanelWidget.ItemWidgetFactory += makeListItem;
            listPanelWidget.ScrollPosition = 0f;
            listPanelWidget.SelectedIndex = 0;
            listPanelWidget.Direction = LayoutDirection.Vertical;

            if (SettingsManager.GuiSize == GuiSize.Smallest)
            {
                row = 5;
                mainview.Size = new Vector2(660,380);
            }
            else if (SettingsManager.GuiSize == GuiSize.Smaller) {
                row = 4;
                mainview.Size = new Vector2(660, 300);
            }
            else {
                row = 3;
                mainview.Size = new Vector2(660, 220);
            }
            xjBitmapClickWidget.Size = new Vector2(switchoffTexture.Texture.Width / 2, switchoffTexture.Texture.Height / 2);
            xjBitmapClickWidget_posi.Size = new Vector2(switchoffTexture.Texture.Width / 2, switchoffTexture.Texture.Height / 2);
            xjBitmapClickWidget_dura.Size = new Vector2(switchoffTexture.Texture.Width / 2, switchoffTexture.Texture.Height / 2);
            xjBitmapClickWidget_upmode.Size = new Vector2(switchoffTexture.Texture.Width / 2, switchoffTexture.Texture.Height / 2);
            initPageNum();
            setBGColor();
            updatePage();
            displayBlocks();
            xjTimer.setAction(mytimaction);
        }     
        private void clearWidgets(StackPanelWidget container)
        {
            while (container.Children.Count() > 0)
            {
                container.Children.Remove(container.Children[0]);
            }

        }        
        private void mytimaction()
        {
            if (xjJeiBmpWidgets.Count() != 0)
            {//让timcnt=1直接显示第二个
                bool finish = false;
                for (int i = 0; i < rcplists.Count(); i++)
                {
                    for (int k = timcnt; k < rcplists[i].Count(); k++)
                    {
                        if (timcnt < rcplists[i].Count())
                        {
                            xjJeiBmpWidgets[i].Value = xjJeiManager.values[rcplists[i][timcnt]];
                            nbtns[i].Name = $"jei_{rcplists[i][timcnt]}";
                            finish = true;
                            break;
                        }
                    }
                }
                timcnt += 1;
                if (finish == false) { timcnt = 0; }//这里需要设置0，循环
            }
        }
        private void displayBlocks()
        {//列出所有blocks
         //直接读取xjJeimanager的方块数据
            viewwhich = 1;
            clearWidgets(stackPanelWidget);
            btns.Clear();
            stackPanelWidget.HorizontalAlignment = WidgetAlignment.Near;
            stackPanelWidget.Direction = LayoutDirection.Vertical;
            try
            {
                int startpos = nowPage * row * col;
                int endpos = (nowPage + 1) * row * col;
                
                for (int i = startpos; i < endpos; i += col)
                {

                    StackPanelWidget stack = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(2, 1) };
                    if (i == startpos) { stack.Margin = new Vector2(2, 10); }
                    else
                    {
                        stack.Margin = new Vector2(2, 2);
                    }

                    for (int j = 0; j < col; j++)
                    {
                        int pp = i + j;
                        if (pp >= allnum) break;
                        XjBitmapClickWidget btn = new XjBitmapClickWidget() { mSize = new Vector2(64, 64), NormalSubtexture = subtexture };
                        BlockIconWidget blockIconWidget = new BlockIconWidget() { Value = xjJeiManager.values[pp], Size = new Vector2(58, 58), Margin = new Vector2(3, 3) };
                        btn.Name = $"jei_{pp}"; btn.Margin = new Vector2(4, 0);                        
                        btn.Children.Add(blockIconWidget);
                        stack.Children.Add(btn);
                        btns.Add(btn);
                    }
                    stackPanelWidget.Children.Add(stack);
                }
              
            }
            catch (Exception e)
            {
                displayError(e.ToString());

            }
        }
        private void loadWidget()
        {//返回绝对坐标集
         //返回搜索到的方块
         //读取lists数据??
            viewwhich = 2;
            clearWidgets(stackPanelWidget);
            btns.Clear();
            stackPanelWidget.HorizontalAlignment = WidgetAlignment.Near;
            stackPanelWidget.Direction = LayoutDirection.Vertical;
            try
            {
                int startpos = nowPage * row * col;
                int endpos = (nowPage + 1) * row * col;
                for (int i = startpos; i < endpos; i += col)
                {
                    StackPanelWidget stack = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
                    if (i == startpos) { stack.Margin = new Vector2(2, 10); }
                    else
                    {
                        stack.Margin = new Vector2(2, 2);
                    }
                    for (int j = 0; j < col; j++)
                    {
                        int pp = i + j;
                        if (pp >= global_search_lists.Count()) break;
                        XjBitmapClickWidget btn = new XjBitmapClickWidget() { mSize = new Vector2(64, 64), NormalSubtexture = subtexture };
                        BlockIconWidget blockIconWidget = new BlockIconWidget() { Value = xjJeiManager.values[global_search_lists[pp]], Size = new Vector2(58, 58), Margin = new Vector2(3, 3) };
                        btn.Name = $"jei_{global_search_lists[pp]}"; btn.Margin = new Vector2(4, 0);
                        btn.Children.Add(blockIconWidget);
                        stack.Children.Add(btn);
                        btns.Add(btn);
                    }
                    stackPanelWidget.Children.Add(stack);
                }

            }
            catch (Exception e)
            {
                displayError(e.ToString());

            }

        }
        private void setRecipeWidget(int id)//id是绝对坐标
        {//设置合成谱视图
         //传参 点击位置            
            if (row == 3) mainview.Size = new Vector2(660, 310);
            int lastpos = clicked_blocks_lists.Count() - 1;
            if (lastpos < 0) lastpos = 0;
            if (clicked_blocks_lists.Count() > 0)
            {
                //这次点击和上次点击不一样
                if (clicked_blocks_lists[lastpos] != id)
                {
                    if (!isBack)
                    {
                        //缓存上次点击的位置
                        clicked_blocks_lists.Add(clickpos);
                    }
                    else {//返回过来的，删除上一个
                        clickpos = id;
                        clicked_blocks_lists.RemoveAt(clicked_blocks_lists.Count() - 1);
                        isBack = false;
                    }
                    showpos = 0;
                }
            }
            else
            {
                clicked_blocks_lists.Add(clickpos);
            }
            craftingRecipe = craftings;//缓存上次的配方
            craftings = xjJeiManager.getRecipesByPos(clickpos);//获取新配方
            if (craftings.Count() != 0)
            {
                craftingRecipe = craftings;//令全局保存的配方等于新配方
                allrcps = craftings.Count();
                timcnt = 1;
                rcplists = xjJeiManager.decodeIngredian(craftings[showpos].Ingredients);
                xjTimer.setRun(true);
            }
            else
            {//没有配方显示属性       
                inProperty = true;
                showProperty(clickpos);
                craftings = craftingRecipe;
                return;
            }
            inProperty = false;
            //功能开始
            viewwhich = 3;
            clearWidgets(stackPanelWidget);
            clearWidgets(nine_stack);
            clearWidgets(rcip_stack);
            stackPanelWidget.HorizontalAlignment = WidgetAlignment.Near;
            stackPanelWidget.Direction = LayoutDirection.Vertical;
            xjJeiBmpWidgets.Clear();
            StackPanelWidget stkmain = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center };//主容器
            StackPanelWidget stkc = new StackPanelWidget() { Direction = LayoutDirection.Vertical };//结果
            LabelWidget labelc = new LabelWidget() { Text = $"{xjJeiManager.getNameByPos(clickpos)}({craftingRecipe[showpos].ResultCount})", Size = new Vector2(64, 64), WordWrap = true };
            LabelWidget desc = new LabelWidget() { Text = $"{craftingRecipe[showpos].Description}", FontScale = 0.6f, Size = new Vector2(50, mainview.Size.Y), WordWrap = true, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Near };
            nbtns.Clear();//清除按钮缓存
            int cek = getCraftWidth(craftingRecipe[showpos].Ingredients);//计算配方宽度
            changeIngreList(cek);//重新生成list
            changeIngre(craftingRecipe[showpos].Ingredients, cek);//获得去除无用成分的新配方
            setMutiRecipe(cek);
            StackPanelWidget stackPanel1 = new StackPanelWidget() { Direction = LayoutDirection.Vertical, VerticalAlignment = WidgetAlignment.Center, HorizontalAlignment = WidgetAlignment.Center };
            for (int i = 0; i < rcplists.Count(); i += cek)
            {
                StackPanelWidget stack = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center };
                for (int j = 0; j < cek; j++)
                {
                    int po = i + j;
                    List<int> yy = rcplists[po];
                    int pp = 0;
                    if (yy.Count() != 0) pp = yy[0];
                    else
                    {
                        foreach (int uu in yy) { if (uu != 0) pp = uu; break; }
                    }
                    XjBitmapClickWidget bmp = new XjBitmapClickWidget() { mSize = getBlockIconSize(cek), NormalSubtexture = subtexture };
                    BlockIconWidget xjJeiBmp = new BlockIconWidget() { Size = getBlockIconSize(cek)-new Vector2(2,2), Margin = new Vector2(2, 2), Value = xjJeiManager.values[pp] };                    
                    bmp.Name = $"jei_{pp}";
                    bmp.Children.Add(xjJeiBmp);
                    xjJeiBmpWidgets.Add(xjJeiBmp);//视图缓存
                    stack.Children.Add(bmp);
                    nbtns.Add(bmp);
                }
                stackPanel1.Children.Add(stack);

            }
            StackPanelWidget sss = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, VerticalAlignment = WidgetAlignment.Center, HorizontalAlignment = WidgetAlignment.Center };
            StackPanelWidget stackPanel = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, VerticalAlignment = WidgetAlignment.Center, HorizontalAlignment = WidgetAlignment.Center };

            if (subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative) {
                xj.player.ComponentMiner.Inventory.RemoveSlotItems(16, xj.player.ComponentMiner.Inventory.GetSlotCount(16));
                xj.player.ComponentMiner.Inventory.AddSlotItems(16, xjJeiManager.values[clickpos], 1);
                InventorySlotWidget inventorySlotWidget = new InventorySlotWidget() { Size=new Vector2(64,64)};
                inventorySlotWidget.AssignInventorySlot(xj.player.ComponentMiner.Inventory,16);
                stackPanel.Children.Add(inventorySlotWidget);
            }
            else
            {
                BlockIconWidget resblock = new BlockIconWidget() { Value = xjJeiManager.values[clickpos], Size = new Vector2(64, 64) };//结果视图
                stackPanel.Children.Add(resblock);
            }
            
            setLevel((int)craftingRecipe[showpos].RequiredHeatLevel);//设定合成工具视图
            stkc.Children.Add(stackPanel); //合成物品预览
            stkc.Children.Add(labelc);//合成物品名字
            stkc.Children.Add(setProperty(clickpos));//物品属性
            sss.Children.Add(stackPanel1); sss.Children.Add(fastCraft);
            nine_stack.Children.Add(sss);
            stkmain.Children.Add(nine_stack);//需要物品宫格视图
            stkmain.Children.Add(rcip_stack);//合成工具视图
            stkmain.Children.Add(stkc);//结果视图
            stkmain.Children.Add(desc);
            stkmain.Children.Add(stackVer);
            stackPanelWidget.Children.Add(stkmain);
            string vlp = xjJeiManager.getCraftIdByPos(clickpos);            
            connCraftBlocks = xjJeiManager.findConnCraftBlocks(vlp);
            makeConnCraftWidget();
        }
        private Vector2 getBlockIconSize(int cek) {

            switch (cek) {
                case 3:
                    return new Vector2(58,58);
                case 4:
                    return new Vector2(48, 48);
                case 5:
                    return new Vector2(38,38);
                case 6:
                    return new Vector2(28,28);
                default:
                    return new Vector2(58,58);            
            }
        }
        private Widget makeConnCraftWidget() {
            clearWidgets(stackVer);
            Vector2 mmmm = new Vector2();
            if (inProperty) mmmm = new Vector2(140, 20); else mmmm = new Vector2(0,20);
            StackPanelWidget blocklist = new StackPanelWidget() {Direction=LayoutDirection.Vertical, VerticalAlignment=WidgetAlignment.Center,Margin= mmmm};
            connCraftBtn_back.Margin = new Vector2(mmmm.X, 2) ;
            connCraftBtn_next.Margin =new Vector2(mmmm.X,2);
            stackVer.Children.Add(connCraftBtn_back);
            connCraftBtns.Clear();
            int endpos = 0;
            if (connCraftPos + 5 < connCraftBlocks.Count) endpos = connCraftPos+5;
            else endpos=connCraftBlocks.Count;
            if (connCraftBlocks.Count>0) for(int v=connCraftPos;v<endpos;v++) {
                XjBitmapClickWidget xjBitmapClick = new XjBitmapClickWidget() { NormalSubtexture = subtexture,mSize=new Vector2(38)};
                BlockIconWidget blockIconWidget = new BlockIconWidget() { Size = new Vector2(36), Value = connCraftBlocks[v], Margin = new Vector2(1,1) };
                xjBitmapClick.Children.Add(blockIconWidget);
                blocklist.Children.Add(xjBitmapClick);
                connCraftBtns.Add(xjBitmapClick);
            }
            stackVer.Children.Add(blocklist);
            stackVer.Children.Add(connCraftBtn_next);
            return stackVer;
        }
        private void setFastCrafting()
        {//设定快速合成界面
            int resultIndex = 0;
            int fuelIndex = 0;
            int startIndex = 0;
            int eachnum = 0;
            int maxcek = 0;
            int mode = 0;
            if (xjJeiManager.getNameByPos(xjJeiManager.getPosByValue(508)) == "机床")
            {
                mode = 1;//工业
            }
            else mode = 0;
            int cek = getCraftWidth(craftingRecipe[showpos].Ingredients);//获得配方宽度
            int level = (int)craftingRecipe[showpos].RequiredHeatLevel;
            if (level <= 0)
            {
                if (cek == 3)
                {
                    findComponentCraftingTable();maxcek = 3;
                    if (mode == 1)
                    {
                        if (componentCraftingTable == null)
                        {
                            findComponentJichuang(); maxcek = 4;
                            if (componentCraftingTable == null)
                            {
                                findComponentChejian(); maxcek = 5;
                            }
                        }
                    }
                    
                }
                else if (cek == 4&&mode==1) {
                    findComponentJichuang();maxcek = 4;
                    if (componentCraftingTable == null) { findComponentChejian(); maxcek = 5; }
                }
                else if (cek == 5&&mode==1) {
                    findComponentChejian();maxcek = 5;
                }
                if (componentCraftingTable == null)
                {
                    if (mode == 1)
                    {
                        switch (cek)
                        {
                            case 3: xj.toast("需要工作台在附近"); break;
                            case 4: xj.toast("需要机床在附近"); break;
                            case 5: xj.toast("需要车间在附近"); break;
                            default:
                                xj.toast("不能识别的配方"); return;
                        }                        
                    }
                    else {
                        switch (cek)
                        {
                            case 3: xj.toast("需要工作台在附近"); break;
                            default:
                                xj.toast("不能识别的配方"); return;
                        }

                    }
                    return;
                }
                resultIndex = componentCraftingTable.SlotsCount - 2;
                startIndex = 0;

            }
            else
            {
                eachnum = getFuranceWidth(craftingRecipe[showpos].Ingredients);
                findComponentFurnace(eachnum);
                if (componentFurnace == null)
                {
                    xj.toast("附近没有炉子用于合成");
                    return;
                }
                fuelIndex = componentFurnace.SlotsCount - 3;
                resultIndex = componentFurnace.SlotsCount - 2;
                startIndex = 0;
            }
            viewwhich = 4;
            clearWidgets(stackPanelWidget); clearWidgets(nine_stack); clearWidgets(rcip_stack);           
            stackPanelWidget.HorizontalAlignment = WidgetAlignment.Center; stackPanelWidget.VerticalAlignment = WidgetAlignment.Center;
            stackPanelWidget.Direction = LayoutDirection.Horizontal;
            changeIngreList(cek);//重新生成rcplist
            int num;
            StackPanelWidget stackPanel1;
            ArrowLineWidget arrowLine;
            InventorySlotWidget inventorySlot;            
            //工作台视图
            if (level <= 0)
            {
                num = 0;
                //拿取背包相应物品
                for (int i = 0; i < cek; i++)
                {
                    for (int j = 0; j < cek; j++)
                    {
                        for (int k = 0; k < xj.player.ComponentMiner.Inventory.SlotsCount; k++)
                        {
                            bool ko = false;
                            if (xj.player.ComponentMiner.Inventory.GetSlotCount(k) == 0) continue;
                            for (int m = 0; m < rcplists[num].Count(); m++)
                            {
                                if (Terrain.ExtractContents(xjJeiManager.getValueByPos(rcplists[num][m])) == 0) { ko = true; break; }
                                if (xj.player.ComponentMiner.Inventory.GetSlotValue(k) == xjJeiManager.getValueByPos(rcplists[num][m])&&(componentCraftingTable.GetSlotCount(num)+1<=componentCraftingTable.GetSlotCapacity(num, xjJeiManager.getValueByPos(rcplists[num][m]))))
                                {
                                    componentCraftingTable.AddSlotItems(startIndex + i * maxcek + j, xjJeiManager.getValueByPos(rcplists[num][m]), 1);
                                    xj.player.ComponentMiner.Inventory.RemoveSlotItems(k, 1);
                                    break;
                                }
                            }
                            if (ko) { break; }
                        }
                        ++num;
                    }                
                }
                num = startIndex;                
                for (int i = 0; i < maxcek; i++)
                {
                    StackPanelWidget stackPanel = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
                    for (int j = 0; j < maxcek; j++)
                    {
                        InventorySlotWidget inventorySlotWidget = new InventorySlotWidget() { Size = new Vector2(48, 48) };
                        inventorySlotWidget.AssignInventorySlot(componentCraftingTable, num++);
                        stackPanel.Children.Add(inventorySlotWidget);
                    }
                    nine_stack.Children.Add(stackPanel);
                }
                //合成结果视图
                stackPanel1 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center };
                arrowLine = new ArrowLineWidget() { Color = Color.White, Width = 8, ArrowWidth = 36 };
                inventorySlot = new InventorySlotWidget() { Size = new Vector2(48, 48), Margin = new Vector2(20, 0) };//结果
                inventorySlot.AssignInventorySlot(componentCraftingTable,resultIndex);
                stackPanel1.Children.Add(arrowLine);
                stackPanel1.Children.Add(inventorySlot);

            }
            else {//熔炉视图
                num = 0;
             
                //拿取背包相应物品
                for (int i = 0; i < (eachnum-3); i++)
                {
                        for (int k = 0; k < xj.player.ComponentMiner.Inventory.SlotsCount; k++)
                        {
                            bool ko = false;
                            for (int m = 0; m < rcplists[num].Count(); m++)
                            {
                                if (Terrain.ExtractContents(xjJeiManager.getValueByPos(rcplists[num][m])) == 0) { ko = true; break; }
                                if (xj.player.ComponentMiner.Inventory.GetSlotValue(k) == xjJeiManager.getValueByPos(rcplists[num][m])&& (componentFurnace.GetSlotCount(num) + 1 <= componentFurnace.GetSlotCapacity(num, xjJeiManager.getValueByPos(rcplists[num][m]))))
                                {
                                componentFurnace.AddSlotItems(i, xjJeiManager.getValueByPos(rcplists[num][m]), 1);
                                xj.player.ComponentMiner.Inventory.RemoveSlotItems(k, 1);
                                break;
                                }
                            }
                            if (ko) { break; }
                        }
                    }
                
                num = 0;
                int fmaxcek = 0;
                if (eachnum == 9)
                {//不是原版熔炉 
                    maxcek = 3; fmaxcek = 2;
                }
                else {
                    fmaxcek = 1;maxcek = 2;
                }
                    for (int j = 0; j < fmaxcek; j++)
                    {
                    StackPanelWidget stackPanel = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
                    for (int i = 0; i < maxcek; i++)
                    {
                        InventorySlotWidget inventorySlotWidget = new InventorySlotWidget() { Size = new Vector2(48, 48) };
                        inventorySlotWidget.AssignInventorySlot(componentFurnace, num++);
                        stackPanel.Children.Add(inventorySlotWidget);
                    }
                    nine_stack.Children.Add(stackPanel);
                }
                //燃料格
                StackPanelWidget stackPanel12 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
                InventorySlotWidget inventorySlotWidget1 = new InventorySlotWidget() { Size = new Vector2(48, 48),Margin=new Vector2(0,10) };
                inventorySlotWidget1.AssignInventorySlot(componentFurnace, fuelIndex);
                progess.Text = "Fuel";
                inventorySlotWidget1.Children.Add(progess);
                stackPanel12.Children.Add(inventorySlotWidget1);
                nine_stack.Children.Add(stackPanel12);
                nine_stack.Children.Add(fireWidget);
                //合成结果视图
                stackPanel1 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, HorizontalAlignment = WidgetAlignment.Center, VerticalAlignment = WidgetAlignment.Center };
                arrowLine = new ArrowLineWidget() { Color = Color.White, Width = 8, ArrowWidth = 36 };
                inventorySlot = new InventorySlotWidget() { Size = new Vector2(48, 48), Margin = new Vector2(20, 0) };//结果
                inventorySlot.AssignInventorySlot(componentFurnace,resultIndex);
                stackPanel1.Children.Add(arrowLine);
                stackPanel1.Children.Add(inventorySlot);
            }
            //背包视图      
            StackPanelWidget stack_nine_main = new StackPanelWidget() { Direction=LayoutDirection.Horizontal,VerticalAlignment=WidgetAlignment.Center};
            StackPanelWidget morebtn_stack = new StackPanelWidget() {Direction=LayoutDirection.Vertical,VerticalAlignment=WidgetAlignment.Far };
            CanvasWidget canvasWidget = new CanvasWidget() { Margin=new Vector2(0,7)};
            stack_nine_main.Children.Add(nine_stack);
            morebtn_stack.Children.Add(fastMove);
            morebtn_stack.Children.Add(canvasWidget);
            morebtn_stack.Children.Add(fastCraft);
            stack_nine_main.Children.Add(morebtn_stack);
            stackPanelWidget.Children.Add(stack_nine_main);
            stackPanelWidget.Children.Add(stackPanel1);
            stackPanelWidget.Children.Add(setBackpack());
        }
        private void backfunction()
        {//返回功能按钮
         //viewwhich码说明
         //1 displayallblocks
         //2 searchblocks
         //3 recipeview
         //4 craftview
         //btns 方块按钮缓存
         //nbtns 合成谱按钮缓存
            xjTimer.setRun(false);
            connCraftPos = 0;
            switch (viewwhich) {
                case 1:
                    xj.player.ComponentGui.ModalPanelWidget = null;
                    clicked_blocks_lists.Clear();
                    break;//界面消失
                case 2://在搜索中
                    clicked_blocks_lists.Clear();
                    boxWidget.Text = "";inSer = false;nowPage = 0;
                    displayBlocks();//在搜索中和不在搜索中的回退
                    break;
                case 3://在合成谱界面
                    if (row == 3) mainview.Size = new Vector2(660, 220);
                    if (clicked_blocks_lists.Count() > 1) {
                        isBack = true;
                        int cache = clicked_blocks_lists[clicked_blocks_lists.Count() - 2];
                        setRecipeWidget(cache);
                    }
                    else
                    {
                        if (inSer) {                            
                            loadWidget();
                        }
                        else
                        {
                            displayBlocks();
                        }
                        clicked_blocks_lists.Clear();

                    }
                    break;
                case 4://在工作台界面
                    setRecipeWidget(clickpos);
                    break;
                default:
                    if (row == 3) mainview.Size = new Vector2(660, 220);
                    displayBlocks();
                    break;
            }            
        }
        private void displayError(String str)
        {//打印ERROR
            clearWidgets(stackPanelWidget);
            LabelWidget label = new LabelWidget() { Text = str, WordWrap = true };
            stackPanelWidget.Children.Add(label);
        }
        private void initPageNum()
        {
            allnum = xjJeiManager.getCount();
            nowPage = 0;
            allPage = (int)Math.Ceiling((float)allnum / (float)(row * col));
        }
        private void Textchange(TextBoxWidget tx)
        {//搜索功能
            String input = tx.Text;
            if (input == "搜索物品") return;
            if (String.IsNullOrEmpty(input)) {
                xjTimer.setRun(false);
                nbtns.Clear();
                inSer = false;//没有处在搜索中
                initPageNum();//计算总分页数据
                updatePage();//刷新视图
                displayBlocks();
                return; 
            }
            global_search_lists.Clear();
            global_search_lists = xjJeiManager.findBlocksByName(input);//搜索到的物品集
            if (global_search_lists.Count() == 0) { xj.toast($"没有查询到包含【{input}】的物品！"); }
            else
            {
                inSer = true;//处于搜索中
                //计算搜索分页数据
                allPage = (int)Math.Ceiling((float)global_search_lists.Count() / (float)(row * col));
                nowPage = 0; allnum = global_search_lists.Count();
                updatePage();//刷新视图
                loadWidget();//加载搜索结果
            }
        }
        private void TextEnter(TextBoxWidget tx)
        {
            tx.Text = "";
        }
        private void updatePage()
        {
            pageLabel.Text = $"{nowPage + 1}/{allPage}[{allnum}]";
        }        
        private void setMutiRecipe(int cek)
        {//设置配方翻页视图
            StackPanelWidget panelWidget = new StackPanelWidget() { Direction = LayoutDirection.Horizontal ,Margin=new Vector2(27,16),HorizontalAlignment=WidgetAlignment.Center,VerticalAlignment=WidgetAlignment.Near};
            XjBitmapClickWidget btnback = new XjBitmapClickWidget();
            btnback.Name = "btnback";
            btnback.NormalSubtexture = leftTexture;
            btnback.Size = getBlockIconSize(cek);
            LabelWidget info = new LabelWidget() { Text = $"{showpos + 1}/{allrcps}" };
            XjBitmapClickWidget btnnext = new XjBitmapClickWidget();
            btnnext.Name = "btnnext";
            btnnext.NormalSubtexture = rightTexture;
            btnnext.Size = getBlockIconSize(cek);
            panelWidget.Children.Add(btnback); panelWidget.Children.Add(info); panelWidget.Children.Add(btnnext);
            nine_stack.Children.Add(panelWidget);
            btn_bck = btnback; btn_nck = btnnext;
        }
        private void setLevel(int level)
        {//设置合成加热等级视图
            BlockIconWidget nesblock = new BlockIconWidget() { Value = getTool(level), Size = new Vector2(64, 64) };//合成工具视图
            ArrowLineWidget arrowLine = new ArrowLineWidget() { Color = Color.White, Width = 8, ArrowWidth = 36 };
            rcip_stack.Children.Add(nesblock); rcip_stack.Children.Add(arrowLine);
        }
        private void showProperty(int pos) {//展示方块信息
            viewwhich = 3;//算setRecipe界面
            clearWidgets(stackPanelWidget); clearWidgets(nine_stack); clearWidgets(rcip_stack);
            StackPanelWidget stackPanel_main = new StackPanelWidget() {Direction=LayoutDirection.Horizontal,HorizontalAlignment=WidgetAlignment.Center,VerticalAlignment=WidgetAlignment.Center };
            StackPanelWidget stackPanel = new StackPanelWidget() { Direction=LayoutDirection.Vertical};
            if (subsystemGameInfo == null) { xj.toast("internal error!"); return; }
            if (subsystemGameInfo.WorldSettings.GameMode == GameMode.Creative)
            {

                InventorySlotWidget inventorySlot = new InventorySlotWidget() { Size = new Vector2(64, 64), VerticalAlignment = WidgetAlignment.Center, HorizontalAlignment = WidgetAlignment.Center };
                inventorySlot.AssignInventorySlot(xj.player.ComponentMiner.Inventory, 16);
                xj.player.ComponentMiner.Inventory.RemoveSlotItems(16, xj.player.ComponentMiner.Inventory.GetSlotCount(16));
                xj.player.ComponentMiner.Inventory.AddSlotItems(16, xjJeiManager.values[clickpos], 1);
                stackPanel.Children.Add(inventorySlot);
                LabelWidget labelWidget = new LabelWidget() { Text = $"{xjJeiManager.getNameByPos(pos)}", Size = new Vector2(256, 64), WordWrap = true };
                stackPanel.Children.Add(labelWidget);
                stackPanel.Children.Add(setProperty(pos));
                stackPanel_main.Children.Add(stackPanel);
                stackPanel_main.Children.Add(setBackpack());
            }
            else
            {
                BlockIconWidget blockIconWidget = new BlockIconWidget() { Value = xjJeiManager.getValueByPos(pos), Size = new Vector2(60, 60), Margin = new Vector2(2, 2), VerticalAlignment = WidgetAlignment.Center, HorizontalAlignment = WidgetAlignment.Center };
                stackPanel.Children.Add(blockIconWidget);
                LabelWidget labelWidget = new LabelWidget() { Text = xjJeiManager.getNameByPos(pos), Size = new Vector2(256, 64), WordWrap = true,HorizontalAlignment=WidgetAlignment.Center };
                stackPanel.Children.Add(labelWidget);
                stackPanel.Children.Add(setProperty(pos));
                stackPanel_main.Children.Add(stackPanel);
            }
            stackPanel_main.Children.Add(stackVer);
            string vlp = xjJeiManager.getCraftIdByPos(clickpos);
            connCraftBlocks = xjJeiManager.findConnCraftBlocks(vlp);
            makeConnCraftWidget();
            stackPanelWidget.Children.Add(stackPanel_main);
        }
        private StackPanelWidget setBackpack() {//背包界面
            int num = 6;
            StackPanelWidget bag_stack = new StackPanelWidget() { Direction = LayoutDirection.Vertical };
            LabelWidget labelWidget = new LabelWidget() { Text = "my backpack", WordWrap = true, Size = new Vector2(128, 48), HorizontalAlignment = WidgetAlignment.Center, Margin = new Vector2(0, 10) };
            bag_stack.Children.Add(labelWidget);
            for (int i = 0; i < 4; i++)
            {
                StackPanelWidget stackPanel = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
                for (int j = 0; j < 4; j++)
                {
                    InventorySlotWidget inventorySlotWidget = new InventorySlotWidget() { Size = new Vector2(48, 48) };
                    inventorySlotWidget.AssignInventorySlot(xj.player.ComponentMiner.Inventory, num++);
                    stackPanel.Children.Add(inventorySlotWidget);
                }
                bag_stack.Children.Add(stackPanel);                
            }
            return bag_stack;
        }
        private StackPanelWidget setProperty(int pos) {//属性视图 
            StackPanelWidget stackPanel = new StackPanelWidget() { Direction = LayoutDirection.Vertical, VerticalAlignment = WidgetAlignment.Center, HorizontalAlignment = WidgetAlignment.Near ,Margin=new Vector2(10,0)};
            Block block = xjJeiManager.blocks[pos];
            ClothingBlock clothingBlock = block as ClothingBlock;
            if (clothingBlock != null)
            {
                int clouthindex = Terrain.ExtractData(xjJeiManager.getValueByPos(pos));
                StackPanelWidget stackline = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget.Texture = prop_Health;//耐久
                LabelWidget labelWidget = new LabelWidget() { Text = $"{ClothingBlock.m_clothingData[ClothingBlock.GetClothingIndex(clouthindex)].Sturdiness} 耐久", FontScale = 0.7f };
                labelWidget.Size = new Vector2(XjJeiLibrary.caculateWidth(labelWidget,labelWidget.FontScale,labelWidget.Text,200),XjJeiLibrary.caculateHeight(labelWidget,labelWidget.m_maxLines,labelWidget.FontScale));
                stackline.Children.Add(xjBitmapWidget); stackline.Children.Add(labelWidget);

                StackPanelWidget stackline2 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget2 = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget2.Texture = prop_Defense;//防御
                LabelWidget labelWidget2 = new LabelWidget() { Text = $"{ClothingBlock.m_clothingData[ClothingBlock.GetClothingIndex(clouthindex)].ArmorProtection*100}%伤害减免", Size = new Vector2(32, 32), FontScale = 0.7f };
                stackline2.Children.Add(xjBitmapWidget2); stackline2.Children.Add(labelWidget2);
                labelWidget2.Size = new Vector2(XjJeiLibrary.caculateWidth(labelWidget2, labelWidget2.FontScale, labelWidget2.Text, 200), XjJeiLibrary.caculateHeight(labelWidget2, labelWidget2.m_maxLines, labelWidget2.FontScale));

                StackPanelWidget stackPanelWidget1 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(0, 3) };
                StackPanelWidget stackPanelWidget2 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(0, 3) };
                StackPanelWidget stackPanelWidget3 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(0, 3) };
                stackPanelWidget1.Children.Add(stackline); 
                stackPanelWidget1.Children.Add(stackline2); 
                stackPanel.Children.Add(stackPanelWidget1);
                stackPanel.Children.Add(stackPanelWidget2);
                stackPanel.Children.Add(stackPanelWidget3);
            }
            else {
                string dut;
                StackPanelWidget stackline = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget.Texture = prop_Health;//耐久
                if (block.Durability == -1)
                {
                    dut = "无限";
                }
                else {
                    dut = block.Durability.ToString();
                }
                LabelWidget labelWidget = new LabelWidget() { Text = dut, Size = new Vector2(32, 32), FontScale = 0.7f };
                stackline.Children.Add(xjBitmapWidget); stackline.Children.Add(labelWidget);

                StackPanelWidget stackline1 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget1 = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget1.Texture = prop_LongAtk;//投掷攻击力
                LabelWidget labelWidget1 = new LabelWidget() { Text = block.DefaultProjectilePower.ToString(), Size = new Vector2(32, 32), FontScale = 0.7f };
                stackline1.Children.Add(xjBitmapWidget1); stackline1.Children.Add(labelWidget1);

                StackPanelWidget stackline3 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget3 = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget3.Texture = prop_NearAtk;//近战攻击力
                LabelWidget labelWidget3 = new LabelWidget() { Text = block.DefaultMeleePower.ToString(), Size = new Vector2(32, 32), FontScale = 0.7f };
                stackline3.Children.Add(xjBitmapWidget3); stackline3.Children.Add(labelWidget3);

                StackPanelWidget stackline5 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget5 = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget5.Texture = prop_Food;//营养
                LabelWidget labelWidget5 = new LabelWidget() { Text = block.GetNutritionalValue(xjJeiManager.getValueByPos(pos)).ToString(), Size = new Vector2(32, 32), FontScale = 0.7f };
                stackline5.Children.Add(xjBitmapWidget5); stackline5.Children.Add(labelWidget5);

                StackPanelWidget stackline6 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(3, 0) };
                XjBitmapWidget xjBitmapWidget6 = new XjBitmapWidget() { DesiredSize = new Vector2(18, 18) };
                xjBitmapWidget6.Texture = prop_Burn;//燃烧时长
                LabelWidget labelWidget6 = new LabelWidget() { Text = block.FireDuration.ToString(), Size = new Vector2(32, 32), FontScale = 0.7f };
                stackline6.Children.Add(xjBitmapWidget6); stackline6.Children.Add(labelWidget6);


                StackPanelWidget stackPanelWidget1 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(0, 3) };
                StackPanelWidget stackPanelWidget2 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(0, 3) };
                StackPanelWidget stackPanelWidget3 = new StackPanelWidget() { Direction = LayoutDirection.Horizontal, Margin = new Vector2(0, 3) };
                stackPanelWidget1.Children.Add(stackline); stackPanelWidget1.Children.Add(stackline1);
                stackPanelWidget2.Children.Add(stackline5);
                stackPanelWidget3.Children.Add(stackline6);
                stackPanel.Children.Add(stackPanelWidget1);
                stackPanel.Children.Add(stackPanelWidget2);
                stackPanel.Children.Add(stackPanelWidget3);

            }
            return stackPanel;
        }
        private void findComponentChejian() {//找车间
            bool flag = false;
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    for (int k = -2; k < 3; k++)
                    {
                        if (subsystemTerrain.Terrain.GetCellContents((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k) == 501)
                        {
                            ComponentBlockEntity blockEntity = subsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k);
                            if (blockEntity != null)
                            {
                                componentCraftingTable = blockEntity.Entity.FindComponent<ComponentCraftingTable>(throwOnError: true);
                                if (componentCraftingTable != null)
                                {
                                    if (componentCraftingTable.SlotsCount == 27)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    else
                                    {
                                        componentCraftingTable = null;
                                    }
                                }
                            }
                        }
                    }
                    if (flag) break;
                }
                if (flag) break;
            }          
            if (!flag) { componentCraftingTable = null; }
        }
        private void findComponentJichuang()
        {//找车间
            bool flag = false;
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    for (int k = -2; k < 3; k++)
                    {
                        if (subsystemTerrain.Terrain.GetCellContents((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k) == 508)
                        {
                            ComponentBlockEntity blockEntity = subsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k);
                            if (blockEntity != null)
                            {
                                ComponentInventoryBase componentI = blockEntity.Entity.FindComponent<ComponentInventoryBase>(throwOnError: true);
                                if (componentI != null)
                                {
                                    if (componentI.SlotsCount == 18)
                                    {
                                        componentCraftingTable = (ComponentCraftingTable)componentI;
                                        flag = true;
                                        break;
                                    }
                                    else
                                    {
                                        componentCraftingTable = null;
                                    }
                                }
                            }
                        }
                    }
                    if (flag) break;
                }
                if (flag) break;
            }
            if (!flag) { componentCraftingTable = null; }
        }
        private void findComponentCraftingTable() {//寻找最近的工作台
            bool flag = false;
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    for (int k = -2; k < 3; k++)
                    {
                        if (subsystemTerrain.Terrain.GetCellContents((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k) == 27)
                        {
                            ComponentBlockEntity blockEntity = subsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>().GetBlockEntity((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k);
                            if (blockEntity != null)
                            {
                                componentCraftingTable = blockEntity.Entity.FindComponent<ComponentCraftingTable>(throwOnError: true);
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag) break;
                }
                if (flag) break;
            }     
            if (!flag) { componentCraftingTable = null; }
        }
        private void findComponentFurnace(int slotCnt)
        {//寻找最近的熔炉
            bool flag = false;
           for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    for (int k = -2; k < 3; k++)
                    {
                        SubsystemBlockEntities subsystemBlockEntities = subsystemTerrain.Project.FindSubsystem<SubsystemBlockEntities>();
                        if (subsystemBlockEntities!=null) {
                            ComponentBlockEntity blockEntity = subsystemBlockEntities.GetBlockEntity((int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.X + i, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Y + j, (int)xj.player.ComponentMiner.ComponentPlayer.ComponentBody.Position.Z + k);
                            if (blockEntity != null)
                            {
                                componentFurnace = blockEntity.Entity.FindComponent<ComponentFurnace>();
                                if (componentFurnace != null)
                                {
                                    if (componentFurnace.SlotsCount == slotCnt)
                                    {
                                        flag = true;
                                        break;
                                    }
                                    else
                                    {
                                        componentFurnace = null;
                                    }
                                }
                            }
                        }
                    }
                    if (flag) break;
                }
                if (flag) break;
            }           
            if (!flag) { componentFurnace = null; }
        }
        private int getCraftWidth(string[] ingre) {//计算配方大小
            if (ingre==null) return 0;
            int width = 0;int num = 0;
            int cek = (int)Math.Sqrt(ingre.Length);
            for (int i=0;i<cek;i++) {//每列
                bool has = false;
                for (int k = 0; k < cek; k++)
                {//每行
                    if (!string.IsNullOrEmpty(ingre[num++])) {//该行有最大位置
                        if (width <= k) width = k+1;
                        has= true;
                    }
                }
                if (has) {//该列最大位置
                    if (width <= i) width = i+1;
                }
            }
            if (width < 3) width = 3;
            return width;
        }
        private int getFuranceWidth(string[] ingre) {
            int num = 0;
            foreach (string s in ingre) {
                if (!string.IsNullOrEmpty(s)) ++num; else {
                    if (num <= 2) num = 5;
                    else { num = 9; }
                    return num;
                }
            }
            return ingre.Length;
        }
        private string[] changeIngre(string[] desc,int width_) { //改变配方大小，去除无用位置
            if (desc == null) return desc;
            List<string> output = new List<string>();
            int cek = (int)Math.Sqrt(desc.Length);
            if (cek == width_) return desc;
            for (int i=0;i<cek;i++) {
                if (i == width_) break;
                for (int k=0;k<width_;k++) {
                        output.Add(desc[i*cek+k]);                    
                }
            }
            if (output.Count() != (width_ * width_)) return desc;  else return output.ToArray();
        }
        private void changeIngreList(int width) {
            if (rcplists.Count() == 0) return;
            List<List<int>> newList = new List<List<int>>();
            int cek = (int)Math.Sqrt(rcplists.Count());
            if (cek == width) return;
            for (int i=0;i<cek;i++) {
                if (i == width) break;
                for (int j=0;j<width;j++) {
                        newList.Add(rcplists[i * cek + j]);                    
                }
            }
            rcplists = newList;
        }
        private int getTool(int level)
        {
            switch (level)
            {
                case 0:
                    return 27;
                default: return 64;
            }
        }
        private void setPointList() {
            if (inSetting) { displayBlocks(); inSetting = !inSetting; return; }
            clearWidgets(stackPanelWidget);
            StackPanelWidget linestack = new StackPanelWidget() {Direction=LayoutDirection.Horizontal };
            CanvasWidget inputback = new CanvasWidget() { Margin=new Vector2(10,4), Size = new Vector2(200, 48) };
            RectangleWidget inputbg = new RectangleWidget() { FillColor=Color.Black};
            inputback.Children.Add(inputbg);
            LabelWidget label = new LabelWidget() { Text = "注: 刷新距离需要再次点击位置图标",Color=Color.Red,FontScale=0.6f};
            label.Size = new Vector2(caculateLabelWidget(label.Text),label.Font.LineHeight);
            label.Margin = new Vector2(10,0);
            inputback.Children.Add(inputbox);
            int num = 0;
            listPanelWidget.ClearItems();
            foreach (SubsystemXjJeiBehavior.MarkFLag mm in SubsystemXjJeiBehavior.markFLags) {
                listPanelWidget.AddItem(num);
                ++num;
            }
            linestack.Children.Add(inputback);linestack.Children.Add(ColorSelect); linestack.Children.Add(savePoint);linestack.Children.Add(DeleteSelectedPoint);            
            stackPanelWidget.Children.Add(linestack);
            stackPanelWidget.Children.Add(label);
            stackPanelWidget.Children.Add(listPanelWidget);
        }
        private void onListItemClick(object obj) {
            int mm = (int)obj;int num = 0;
            if(SubsystemXjJeiBehavior.markFLags.Contains(SubsystemXjJeiBehavior.markFLags[mm])) SubsystemXjJeiBehavior.markFLags.Remove(SubsystemXjJeiBehavior.markFLags[mm]);
            listPanelWidget.ClearItems();
            foreach (SubsystemXjJeiBehavior.MarkFLag xmm in SubsystemXjJeiBehavior.markFLags)
            {
                listPanelWidget.AddItem(num);
                ++num;
            }
        }
        private float caculateLabelWidget(string s) {
            float f = 0f;LabelWidget label = new LabelWidget();
            foreach (char d in s) {
                f += label.Font.GetGlyph(d).Width;
            }
            return f;
        }
        private Widget makeListItem(object obj) {
            int m = (int)obj;
            SubsystemXjJeiBehavior.MarkFLag mm= SubsystemXjJeiBehavior.markFLags[m];
            StackPanelWidget line = new StackPanelWidget() { Direction = LayoutDirection.Vertical };
            StackPanelWidget stackPanel = new StackPanelWidget() { Direction=LayoutDirection.Horizontal};
            stackPanel.Margin = new Vector2(0,2);
            LabelWidget labelWidget = new LabelWidget();
            labelWidget.Text =$"[{m+1}]  "+ mm.name;
            labelWidget.Color = mm.color;
            labelWidget.Margin = new Vector2(10,0);
            labelWidget.Size = new Vector2(caculateLabelWidget(labelWidget.Text),labelWidget.Font.LineHeight);
            LabelWidget labelWidget_point = new LabelWidget();
            labelWidget_point.Text = $"({mm.point.X:0},{mm.point.Y:0},{mm.point.Z:0})";
            labelWidget_point.Margin = new Vector2(4, 0);
            labelWidget_point.Size = new Vector2(caculateLabelWidget(labelWidget_point.Text), labelWidget.Font.LineHeight);
            labelWidget_point.Color = mm.color;
            LabelWidget labelWidget_distance = new LabelWidget();
            float distance = Vector3.Distance(xj.player.ComponentBody.Position,mm.point);
            labelWidget_distance.Text = $"  {distance:0.00} 米";
            labelWidget_distance.Size = new Vector2(caculateLabelWidget(labelWidget_distance.Text), labelWidget.Font.GlyphHeight);
            labelWidget_distance.Margin=new Vector2(4,0);
            labelWidget_distance.Color = mm.color;
            CanvasWidget canvasline = new CanvasWidget() { Size=new Vector2(int.MaxValue,2)};
            RectangleWidget rectangle = new RectangleWidget() { FillColor=Color.Magenta,OutlineColor=Color.Magenta};
            canvasline.Children.Add(rectangle);
            canvasline.Margin = new Vector2(0,10);
            stackPanel.Children.Add(labelWidget);
            stackPanel.Children.Add(labelWidget_point); stackPanel.Children.Add(labelWidget_distance);
            line.Children.Add(stackPanel);
            line.Children.Add(canvasline);
            return line;
        }
        private void Settings() {
            if (inSetting){ displayBlocks(); inSetting = !inSetting; return; }
            clearWidgets(stackPanelWidget);

            if (SubsystemXjJeiBehavior.run)
            {
                xjBitmapClickWidget.NormalSubtexture = switchonTexture;
            }
            else
            {
                xjBitmapClickWidget.NormalSubtexture = switchoffTexture;
            }
            if (SubsystemXjJeiBehavior.posi)
            {
                xjBitmapClickWidget_posi.NormalSubtexture = switchonTexture;
            }
            else
            {
                xjBitmapClickWidget_posi.NormalSubtexture = switchoffTexture;
            }
            if (SubsystemXjJeiBehavior.dura)
            {
                xjBitmapClickWidget_dura.NormalSubtexture = switchonTexture;
            }
            else
            {
                xjBitmapClickWidget_dura.NormalSubtexture = switchoffTexture;
            }
            if (SubsystemXjJeiBehavior.upmode)
            {
                xjBitmapClickWidget_upmode.NormalSubtexture = switchonTexture;
            }
            else
            {
                xjBitmapClickWidget_upmode.NormalSubtexture = switchoffTexture;
            }
            StackPanelWidget stack_Main = new StackPanelWidget() { Direction=LayoutDirection.Vertical};
            ScrollPanelWidget scrollPanelWidget = new ScrollPanelWidget() { Direction=LayoutDirection.Vertical};

            StackPanelWidget sha = new StackPanelWidget() {Direction=LayoutDirection.Horizontal };
            StackPanelWidget shb = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget shc = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget shd = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget she = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget shf = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget shg = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget shh = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            StackPanelWidget shi = new StackPanelWidget() { Direction = LayoutDirection.Horizontal };
            LabelWidget labela = new LabelWidget() { Text = "矿物信息显示:"};
            LabelWidget labele = new LabelWidget() { Text = "装备耐久显示:" };
            LabelWidget labelf = new LabelWidget() { Text = "人物坐标显示:" };
            LabelWidget labelb = new LabelWidget() { Text = $"背景色R:" };
            LabelWidget labelc = new LabelWidget() { Text = $"背景色G:" };
            LabelWidget labeld = new LabelWidget() { Text = $"背景色B:" };
            LabelWidget labelg = new LabelWidget() { Text = "右下角模式:" };
            LabelWidget labelh = new LabelWidget() { Text = "右下角间距-水平:" };
            LabelWidget labeli = new LabelWidget() { Text = "右下角间距-垂直:" };
            slidera.Value = (SubsystemXjJeiBehavior.color>>16)&0xff;
            slidera.Value = (SubsystemXjJeiBehavior.color >> 8) & 0xff;
            slidera.Value = SubsystemXjJeiBehavior.color&0xff;
            sliderd.Value = SubsystemXjJeiBehavior.margin.X;
            slidere.Value = SubsystemXjJeiBehavior.margin.Y;
            sha.Children.Add(labela); sha.Children.Add(xjBitmapClickWidget);
            shg.Children.Add(labelg); shg.Children.Add(xjBitmapClickWidget_upmode);
            she.Children.Add(labele); she.Children.Add(xjBitmapClickWidget_dura);
            shf.Children.Add(labelf); shf.Children.Add(xjBitmapClickWidget_posi);
            shb.Children.Add(labelb); shb.Children.Add(slidera);
            shc.Children.Add(labelc); shc.Children.Add(sliderb);
            shd.Children.Add(labeld); shd.Children.Add(sliderc);
            shh.Children.Add(labelh); shh.Children.Add(sliderd);
            shi.Children.Add(labeli); shi.Children.Add(slidere);
            stack_Main.Children.Add(sha);
            stack_Main.Children.Add(she);
            stack_Main.Children.Add(shf);
            stack_Main.Children.Add(shg);
            stack_Main.Children.Add(shb);
            stack_Main.Children.Add(shc);
            stack_Main.Children.Add(shd);
            stack_Main.Children.Add(shh);
            stack_Main.Children.Add(shi);
            scrollPanelWidget.Children.Add(stack_Main);
            stackPanelWidget.Children.Add(scrollPanelWidget);
            inSetting = !inSetting;
        }
        public void setBGColor(){
            rectangleWidget_body.FillColor = new Color((SubsystemXjJeiBehavior.color >> 16) & 0xff, (SubsystemXjJeiBehavior.color >> 8) & 0xff, SubsystemXjJeiBehavior.color & 0xff, 125);
            rectangleWidget_search.FillColor = new Color((SubsystemXjJeiBehavior.color >> 16) & 0xff, (SubsystemXjJeiBehavior.color >> 8) & 0xff, SubsystemXjJeiBehavior.color & 0xff, 125);
            slidera.Text = $"{(SubsystemXjJeiBehavior.color >> 16) & 0xff}";
            sliderb.Text = $"{(SubsystemXjJeiBehavior.color >> 8) & 0xff}";
            sliderc.Text = $"{(SubsystemXjJeiBehavior.color) & 0xff}";
        }
        public override void Update()
        {
            if (xjBitmapClickWidget!=null&&xjBitmapClickWidget_dura!=null&&xjBitmapClickWidget_posi!=null) {
                if (xjBitmapClickWidget.IsClicked) {
                    SubsystemXjJeiBehavior.run = !SubsystemXjJeiBehavior.run;
                    if (SubsystemXjJeiBehavior.run)
                    {
                        xjBitmapClickWidget.NormalSubtexture = switchonTexture;
                    }
                    else
                    {
                        xjBitmapClickWidget.NormalSubtexture = switchoffTexture;
                    }
                }
                if (xjBitmapClickWidget_posi.IsClicked)
                {
                    SubsystemXjJeiBehavior.posi = !SubsystemXjJeiBehavior.posi;
                    if (SubsystemXjJeiBehavior.posi)
                    {
                        xjBitmapClickWidget_posi.NormalSubtexture = switchonTexture;
                    }
                    else
                    {
                        xjBitmapClickWidget_posi.NormalSubtexture = switchoffTexture;
                    }
                }
                if (jeiPoints.IsClicked) {
                    setPointList();              
                }
                if (ColorSelect.IsClicked) {
                    ++selectColorPos;
                    if (selectColorPos > (colorTabels.Count - 1)) selectColorPos = 0;
                    ColorSelect.CenterColor = colorTabels[selectColorPos];
                }
                if (DeleteSelectedPoint.IsClicked)
                {
                    if(listPanelWidget.SelectedIndex.HasValue)onListItemClick(listPanelWidget.SelectedIndex);

                }
                if (savePoint.IsClicked) {
                    if (!string.IsNullOrEmpty(inputbox.Text)) {
                        int num = 0;
                        SubsystemXjJeiBehavior.MarkFLag mm = new SubsystemXjJeiBehavior.MarkFLag();
                        mm.name = inputbox.Text;
                        mm.color = colorTabels[selectColorPos];
                        mm.point = xj.player.ComponentBody.Position;
                        if(!SubsystemXjJeiBehavior.markFLags.Contains(mm))SubsystemXjJeiBehavior.markFLags.Add(mm);
                        listPanelWidget.ClearItems();
                        foreach (SubsystemXjJeiBehavior.MarkFLag xmm in SubsystemXjJeiBehavior.markFLags)
                        {
                            listPanelWidget.AddItem(num);
                            ++num;
                        }

                    }
                }
                if (xjBitmapClickWidget_dura.IsClicked)
                {
                    SubsystemXjJeiBehavior.dura = !SubsystemXjJeiBehavior.dura;
                    if (SubsystemXjJeiBehavior.dura)
                    {
                        xjBitmapClickWidget_dura.NormalSubtexture = switchonTexture;
                    }
                    else
                    {
                        xjBitmapClickWidget_dura.NormalSubtexture = switchoffTexture;
                    }
                }
                if (xjBitmapClickWidget_upmode.IsClicked)
                {
                    SubsystemXjJeiBehavior.upmode = !SubsystemXjJeiBehavior.upmode;
                    if (SubsystemXjJeiBehavior.upmode)
                    {
                        xjBitmapClickWidget_upmode.NormalSubtexture = switchonTexture;
                    }
                    else
                    {
                        xjBitmapClickWidget_upmode.NormalSubtexture = switchoffTexture;
                    }
                }
                if (slidera.IsSliding) {
                    SubsystemXjJeiBehavior.color =((int)slidera.Value<<16)|((int)sliderb.Value<<8)|((int)sliderc.Value);
                    slidera.Text = $"{(int)slidera.Value}";
                    setBGColor();
                }
                if (sliderb.IsSliding) {
                    SubsystemXjJeiBehavior.color = ((int)slidera.Value << 16) | ((int)sliderb.Value << 8) | ((int)sliderc.Value);
                    slidera.Text = $"{(int)sliderb.Value}";
                    setBGColor();
                }
                if (sliderc.IsSliding) {
                    SubsystemXjJeiBehavior.color =((int)slidera.Value<<16)|((int)sliderb.Value<<8)|((int)sliderc.Value);
                    slidera.Text = $"{(int)sliderc.Value}";
                    setBGColor();
                }
                if (sliderd.IsSliding) {
                    SubsystemXjJeiBehavior.margin_changed = true;
                    SubsystemXjJeiBehavior.margin.X =(int)sliderd.Value;
                    sliderd.Text=$"{(int)sliderd.Value}";
                    CellInfoShow.jumpcanvas.Margin = new Vector2(SubsystemXjJeiBehavior.margin.X-20, CellInfoShow.jumpcanvas.Margin.Y);
                }
                if (slidere.IsSliding) {
                    SubsystemXjJeiBehavior.margin_changed = true;
                    SubsystemXjJeiBehavior.margin.Y = (int)slidere.Value;
                    slidere.Text=$"{(int)slidere.Value}";
                    CellInfoShow.jumpcanvas.Margin = new Vector2(CellInfoShow.jumpcanvas.Margin.X, SubsystemXjJeiBehavior.margin.Y);
                }
            }
            if (CellInfoSettings.IsClicked) {
                Settings();
            }
            if (componentFurnace != null && componentFurnace.SmeltingProgress != 0)
            {
                fireWidget.ParticlesPerSecond = 10f;
                progess.Text = $"{Math.Ceiling(componentFurnace.SmeltingProgress*100)}%";
            }
            else {
                fireWidget.ParticlesPerSecond = 0f;
                progess.Text = "Fuel";
            }
            if (viewwhich == 4 || viewwhich == 3) { canvasWidget_head.IsVisible = false; } else { canvasWidget_head.IsVisible = true; }
            if (fastCraft.IsClicked)
            { //点击了快速合成按钮
                setFastCrafting();
            }
            if (fastMove.IsClicked)
            {
                if (componentCraftingTable != null)
                {
                    XjJeiLibrary.moveInventory(componentCraftingTable, xj.player.ComponentMiner.Inventory,xj.player, subsystemGameInfo.WorldSettings.GameMode);
                }
                else if (componentFurnace != null)
                {
                    XjJeiLibrary.moveInventory(componentFurnace, xj.player.ComponentMiner.Inventory, xj.player, subsystemGameInfo.WorldSettings.GameMode);
                }
            }
            if (connCraftBtn_back.IsClicked) {
                if (connCraftPos - 1 >= 0) connCraftPos -= 1;
                makeConnCraftWidget();
            }
            if (connCraftBtn_next.IsClicked) {
                if (connCraftPos + 5 < connCraftBlocks.Count) connCraftPos += 1;
                makeConnCraftWidget();
            }
            for(int i=0;i<connCraftBtns.Count;i++) {
                if (connCraftBtns[i].IsClicked) {
                    int getclickpos = xjJeiManager.getPosByValue(connCraftBlocks[connCraftPos+i]);
                    clickpos = getclickpos;
                    setRecipeWidget(getclickpos);
                }         
            }
            for (int i = 0; i < btns.Count(); i++)
            {
                if (btns[i].IsClicked)
                {
                    string[] s = btns[i].Name.Split(new char[] { '_' }, StringSplitOptions.None);
                    int id = int.Parse(s[1]);
                    if (id != 0)
                    {
                        clickpos = id;
                        setRecipeWidget(clickpos);
                    }
                }
            }
                for (int i = 0; i < nbtns.Count(); i++)
                {
                    if (nbtns[i].IsClicked)
                    {//点击了配方里面的合成谱
                        string[] s = nbtns[i].Name.Split(new char[] { '_' }, StringSplitOptions.None);
                        int id = int.Parse(s[1]);
                    if (id != 0)
                    {
                        clickpos = id;
                        setRecipeWidget(clickpos);
                    }
                }
                }
                if (bevelled_index != null) if (bevelled_index.IsClicked)
                    {
                        timcnt = 0;                       
                        backfunction();
                    }
                if (bevelled_next != null) if (bevelled_next.IsClicked) { if ((nowPage + 1) >= allPage) nowPage = 0; else nowPage += 1; updatePage(); if (inSer) loadWidget(); else displayBlocks();};
                if (bevelled_back != null) if (bevelled_back.IsClicked) { if ((nowPage - 1) < 0) nowPage = allPage - 1; else nowPage -= 1; updatePage(); if (inSer) loadWidget(); else displayBlocks(); }
                if (btn_nck != null) if (btn_nck.IsClicked)
                    {
                        timcnt = 1;
                        if ((showpos + 1) >= allrcps) showpos = 0; else showpos += 1;
                        setRecipeWidget(clickpos);
                    }
                if (btn_bck != null) if (btn_bck.IsClicked)
                    {
                        timcnt = 1;
                        if ((showpos - 1) < 0) showpos = allrcps - 1; else showpos -= 1;
                        setRecipeWidget(clickpos);
                    }
                xjTimer.update();
            }
        }
    }

