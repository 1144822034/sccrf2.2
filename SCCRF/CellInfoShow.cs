using Engine;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TemplatesDatabase;
using Engine.Graphics;
using Engine.Media;
using Engine.Input;
namespace Game
{
    public class CellInfoShow : Component, IUpdateable, IDrawable
    {
        public ComponentPlayer componentPlayer;
        public SubsystemTerrain subsystemTerrain;
        public CellInfo infoDialog = new CellInfo();
        public MessageInfo messageInfo = new MessageInfo();
        public MoreInfo moreInfo = new MoreInfo();
        public bool disposed = false;
        public TerrainRaycastResult? terrainray;
        public bool showinfo = true;
        public Point3 point;
        public int cellValue = 0;
        public int dig = 0;
        public int heat = 0;
        public int grow = 0;
        public string disname;
        public int toollevel = 0;
        public Block block;
        public bool hasCreature = false;
        public float? Raymm;
        public PrimitivesRenderer3D primitivesRenderer = new PrimitivesRenderer3D();
        public FontBatch3D fontBatch;
        public bool markaddflag = false;
        public SubsystemModelsRenderer modelsRenderer;
        public int[] DrawOrders => new int[] { 100 };

        public UpdateOrder UpdateOrder => UpdateOrder.Input;

        public float boxsize = 0.05f;
        public FlatBatch3D flatBatch = new FlatBatch3D();
        public SubsystemPlantBlockBehavior subsystemPlantBlock;
        public SubsystemCreatureSpawn creatureSpawn;
        public SubsystemBodies subsystemBodies;
        public SubsystemXjJeiBehavior jeiBehavior;
        public ConvertPY convertPY = new ConvertPY();
        public static XjBitmapWidget jump = new XjBitmapWidget() { DesiredSize=new Vector2(128,128)};
        public static CanvasWidget jumpcanvas = new CanvasWidget() {Size=new Vector2(128,128), Margin = new Vector2(SubsystemXjJeiBehavior.margin.X-20, 40+SubsystemXjJeiBehavior.margin.Y+20), HorizontalAlignment = WidgetAlignment.Far, VerticalAlignment = WidgetAlignment.Far };
        public XjBitmapClickWidget msginfobtn = new XjBitmapClickWidget() { Size=new Vector2(64,64),HorizontalAlignment=WidgetAlignment.Far,Margin=new Vector2(0,3)};
        public static ClickableWidget touch = new ClickableWidget() { };
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {

            flatBatch = primitivesRenderer.FlatBatch();
            componentPlayer = base.Entity.FindComponent<ComponentPlayer>(throwOnError: true);
            subsystemTerrain = base.Entity.Project.FindSubsystem<SubsystemTerrain>(throwOnError: true);
            subsystemPlantBlock= base.Entity.Project.FindSubsystem<SubsystemPlantBlockBehavior>(throwOnError: true);
            creatureSpawn=Project.FindSubsystem<SubsystemCreatureSpawn>();
            modelsRenderer = base.Entity.Project.FindSubsystem<SubsystemModelsRenderer>();
            subsystemBodies = Project.FindSubsystem<SubsystemBodies>();
            jeiBehavior = base.Entity.Project.FindSubsystem<SubsystemXjJeiBehavior>();
            jump.Texture = TextureAtlasManager.GetSubtexture("JEITextures/JEI_Jump").Texture;
            jumpcanvas.Children.Add(touch);
            jumpcanvas.Children.Add(jump);
            jumpcanvas.DesiredSize = SubsystemXjJeiBehavior.jumpsize;
            jump.DesiredSize = SubsystemXjJeiBehavior.jumpsize;
            jumpcanvas.IsVisible = SubsystemXjJeiBehavior.showjump;
            messageInfo.setPlayer(componentPlayer);
            if (componentPlayer != null)
            {
                msginfobtn.NormalSubtexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_MSG"),Vector2.Zero,Vector2.One);
                msginfobtn.ClickedSubtexture = new Subtexture(ContentManager.Get<Texture2D>("JEITextures/JEI_MSG_P"), Vector2.Zero, Vector2.One);
                componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Add(infoDialog);
                componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Add(messageInfo);
                componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Add(moreInfo);
                componentPlayer.GuiWidget.Children.Find<StackPanelWidget>("RightControlsContainer").Children.Add(msginfobtn);
                componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Add(jumpcanvas);
            }

            int num = 0;
            try
            {
                List<string> namelist = new List<string>();
                Dictionary<string,List<string>> depancys =new Dictionary<string,List<string>>();
                List<uint> verList = new List<uint>();
                foreach (ModInfo modInfo in ModsManager.LoadedMods)
                {
                    if (!namelist.Contains(modInfo.Name)) {
                        if (!string.IsNullOrEmpty(modInfo.Dependency))
                        {
                            string[] mm = modInfo.Dependency.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> mlist = new List<string>();
                            if (mm.Length == 1)
                            {
                                mlist.Add(modInfo.Dependency);
                            }
                            else
                            {

                                foreach (string mmsa in mm)
                                {
                                    mlist.Add(mmsa);
                                }
                            }
                            depancys.Add(modInfo.Name, mlist);
                        }
                        namelist.Add(modInfo.Name);
                        verList.Add(modInfo.Version);
                    }
                }
                messageInfo.addMessage("[color=yellow]欢迎使用[/color][color=red]SCCRF合成书[/color][color=yellow]模组！[/color]");
                messageInfo.addMessage($"[color=green]<SCCRF>:[/color]使用一根[color=red]木棍[/color]开始你的[color=green]合成之旅[/color]");
                messageInfo.addMessage($"[color=green]<SCCRF>:[/color]电脑端按下[color=red]TAB[/color]快捷打开[color=green]消息框[/color]");
                messageInfo.addMessage($"[color=green]<SCCRF>:[/color]电脑端按下[color=red]B[/color]快捷打开[color=green]合成谱[/color]");
                messageInfo.addMessage($"[color=green]<SCCRF>:[/color]输入[color=red]/help[/color]使用[color=green]命令[/color]帮助");
                messageInfo.addMessage($"[color=green]并且您已安装([/color][color=red]{namelist.Count}[/color][color=green])个模组[/color]");
                foreach (string modInfo in namelist)
                {
                    messageInfo.addMessage($"[color=blue]{num+1}.[/color] {modInfo.Replace('[','{').Replace(']','}')} [color=yellow]{parseVersion(verList[num])}[/color]");
                    num++;
                }
                num = 0;
                foreach (KeyValuePair<string,List<string>> kkop in depancys) {
                    foreach (string huap in kkop.Value){
                        if (!namelist.Contains(huap)){
                            if (huap == "zh-cn" && namelist.Contains("NewBlocks_SC")) continue;
                            messageInfo.addMessage($"[color=yellow]<警告>[/color]:使用[color=blue]{kkop.Key}[/color]需要安装依赖MOD [color=green]{huap}[/color]");
                        }
                    }
                    ++num;
                }
                messageInfo.IsVisible = false;
            }
            catch (Exception e) {
                messageInfo.addMessage(e.ToString());
            }
            infoDialog.IsVisible=false;            
            base.Load(valuesDictionary, idToEntityMap);
        }
        public string parseVersion(uint ver) {         
            return $"V{ver / 100 % 10}.{ver / 10 % 10}.{ver % 10}";
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            base.Save(valuesDictionary, entityToIdMap);
         }

        public override void Dispose()
        {
            infoDialog.Dispose();
            messageInfo.Dispose();
            moreInfo.Dispose();
            componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Remove(infoDialog);
            componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Remove(messageInfo);
            componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Remove(moreInfo);
            jumpcanvas.Children.Remove(touch);
            jumpcanvas.Children.Remove(jump);
            componentPlayer.GuiWidget.Children.Find<CanvasWidget>("ControlsContainer").Children.Remove(jumpcanvas);
            componentPlayer.GuiWidget.Children.Find<StackPanelWidget>("RightControlsContainer").Children.Remove(msginfobtn);            
            infoDialog = null;
            messageInfo = null;
            moreInfo = null;
            msginfobtn = null;
            componentPlayer = null;
            markaddflag = true;
        }
        public void Draw(Camera camera, int drawOrder)
        {
            foreach (SubsystemXjJeiBehavior.MarkFLag markFLag in SubsystemXjJeiBehavior.markFLags)
            {               
                float f = Vector3.Distance(camera.ViewPosition, markFLag.point);
                Vector3 vector = Vector3.Transform(markFLag.point, camera.ViewMatrix);
                flatBatch.QueueLine(markFLag.point,new Vector3(markFLag.point.X,256, markFLag.point.Z), markFLag.color);
                Vector3 right = Vector3.TransformNormal(0.02f * Vector3.Cross(camera.ViewDirection, camera.ViewUp), camera.ViewMatrix);
                Vector3 down = Vector3.TransformNormal(-0.02f * Vector3.UnitY, camera.ViewMatrix);
                modelsRenderer.PrimitivesRenderer.FontBatch(ContentManager.Get<BitmapFont>("Fonts/Pericles"), 1, DepthStencilState.DepthWrite, null, null, SamplerState.PointWrap).QueueText(markFLag.name + $"({f:0.00})米",vector,right,down,markFLag.color, TextAnchor.Top);
                primitivesRenderer.Flush(camera.ViewProjectionMatrix);
            }
        }
        public void Update(float dt)
        {
            if (componentPlayer == null) {  return; }
            if (msginfobtn.IsClicked|| componentPlayer.ViewWidget.Input.IsKeyDownOnce(Key.Tab)) { //消息点击
                if (messageInfo.IsVisible)
                {
                    messageInfo.alwaysShow = false;
                    messageInfo.IsVisible = messageInfo.alwaysShow;
                }
                else {
                    messageInfo.alwaysShow = true;
                    messageInfo.IsVisible = messageInfo.alwaysShow;
                }
            }
            if (componentPlayer.ViewWidget.Input.IsKeyDownOnce(Key.B)) {
                if (jeiBehavior.xjJEIWidget == null) {
                    jeiBehavior.player = componentPlayer;
                    jeiBehavior.xjJEIWidget = new XjJEIWidget(jeiBehavior); 
                }
                if (componentPlayer.ComponentGui.ModalPanelWidget == null) componentPlayer.ComponentGui.ModalPanelWidget = jeiBehavior.xjJEIWidget;
                else componentPlayer.ComponentGui.ModalPanelWidget = null;
            }
            if (componentPlayer.ComponentHealth.DeathTime.HasValue) {
                foreach (SubsystemXjJeiBehavior.MarkFLag mark in SubsystemXjJeiBehavior.markFLags) {
                    if (mark.point == componentPlayer.ComponentBody.Position) { markaddflag = true; break; }
                    }
                if (!markaddflag)
                {
                    SubsystemXjJeiBehavior.MarkFLag markFLag = new SubsystemXjJeiBehavior.MarkFLag();
                    markFLag.point = componentPlayer.ComponentBody.Position;
                    markFLag.name = "死亡地点";
                    markFLag.color = Color.Red;
                    SubsystemXjJeiBehavior.markFLags.Add(markFLag);
                    markaddflag = true;
                }
            }
            if (touch.IsPressed)
            {
                componentPlayer.ComponentLocomotion.JumpOrder = 1f;
                jump.Texture = TextureAtlasManager.GetSubtexture("JEITextures/JEI_Jump_P").Texture;

            }
            else {
                jump.Texture = TextureAtlasManager.GetSubtexture("JEITextures/JEI_Jump").Texture;
            }
            if (messageInfo.IsVisible) {
                if (messageInfo.inputpos > 0)
                {
                    if (componentPlayer.ViewWidget.Input.IsKeyDownOnce(Key.UpArrow))
                    {
                        if (messageInfo.inputpos - 1 < 0)messageInfo.inputpos = messageInfo.inputcache.Count - 1; else messageInfo.inputpos -= 1;
                        messageInfo.inputtext.Text = messageInfo.inputcache[messageInfo.inputpos];
                    }
                    else if (componentPlayer.ViewWidget.Input.IsKeyDownOnce(Key.DownArrow))
                    {
                        if (messageInfo.inputpos + 1 > messageInfo.inputcache.Count - 1) messageInfo.inputpos = 0; else messageInfo.inputpos += 1;
                        messageInfo.inputtext.Text = messageInfo.inputcache[messageInfo.inputpos];
                    }
                }
            }           
            block = BlocksManager.Blocks[Terrain.ExtractContents(componentPlayer.ComponentMiner.ActiveBlockValue)];
            if (block.Durability > 0)
            {
                disname = block.GetDisplayName(null, componentPlayer.ComponentMiner.ActiveBlockValue);
                if (XjJeiManager.is_NewSC && ModsManager.customer_Strings.ContainsKey(disname))
                {
                    disname = ModsManager.customer_Strings[disname];
                }
                int durty = 0;
                int damage = block.GetDamage(componentPlayer.ComponentMiner.ActiveBlockValue);
                durty=(block.Durability - damage);
                moreInfo.setInfo(disname,durty,block.GetMeleePower(componentPlayer.ComponentMiner.ActiveBlockValue));
                moreInfo.bitmapWidget.Texture = moreInfo.texture;
            }
            else {
                moreInfo.naijiu.Text = "";
                moreInfo.bitmapWidget.Texture = null;
                moreInfo.labelWidget.Text = "";
            }
                moreInfo.setPosi(componentPlayer.ComponentBody.Position);
                moreInfo.setShow(SubsystemXjJeiBehavior.posi, SubsystemXjJeiBehavior.dura);
            if (!SubsystemXjJeiBehavior.run) return;
            if (componentPlayer.ComponentGui.ModalPanelWidget!=null) {infoDialog.IsVisible = false;return; }
            //检测是否有方块
            Ray3 ray = new Ray3(componentPlayer.GameWidget.ActiveCamera.ViewPosition, componentPlayer.GameWidget.ActiveCamera.ViewDirection);
            terrainray = componentPlayer.ComponentMiner.Raycast<TerrainRaycastResult>(ray, RaycastMode.Interaction, true, true, false);
            foreach (KeyValuePair<ComponentCreature, bool> creature in creatureSpawn.m_creatures)
            {
                if (creature.Key == null) { hasCreature = false; continue; }
                Vector3 creaturePosition = creature.Key.ComponentCreatureModel.EyePosition;
                Vector3 start = ray.Position;
                Vector3 direction = Vector3.Normalize(ray.Direction);
                Vector3 end = ray.Position + direction * 15f;
                //检测是否有生物
                if (creature.Key.ComponentBody.Position == componentPlayer.ComponentBody.Position) { hasCreature = false; continue; }
                Raymm = ray.Intersection(creature.Key.ComponentBody.BoundingBox);
                if (Raymm.HasValue)
                {
                    if (Raymm.Value <= 10f) {
                        infoDialog.setCreatureinfo(creature.Key);
                        hasCreature = true;
                        break;
                    }
                }
                else
                {
                    hasCreature = false;
                }
            }
            if (terrainray.HasValue && !hasCreature)
            {
                point = terrainray.Value.CellFace.Point;
                float disb = terrainray.Value.Distance;
                 if (Raymm.HasValue&&Raymm.Value <= disb) {
                    infoDialog.IsVisible = true;
                }else{
                    cellValue = terrainray.Value.Value;
                    if (Terrain.ExtractContents(componentPlayer.ComponentMiner.ActiveBlockValue) != 0)
                    {
                        //装备的工具等级
                        block = BlocksManager.Blocks[Terrain.ExtractContents(componentPlayer.ComponentMiner.ActiveBlockValue)];
                        toollevel = block.ToolLevel;
                        //hack为劈砍
                        //shovel为铲子
                        //quarry为镐子
                        //none为手
                    }
                    else
                    {
                        toollevel = 0;
                    }
                    block = BlocksManager.Blocks[Terrain.ExtractContents(cellValue)];
                    disname = block.GetDisplayName(null, cellValue);
                    if (XjJeiManager.is_NewSC&& ModsManager.customer_Strings.ContainsKey(disname)){
                        disname = ModsManager.customer_Strings[disname];
                    };
                    if (subsystemPlantBlock.HandledBlocks.Contains(Terrain.ExtractContents(cellValue)))
                    {
                        grow = Terrain.ExtractData(cellValue) & 7;
                        if (Terrain.ExtractContents(cellValue) == 174) grow = (int)((((float)grow) / 7f) * 100);//黑麦
                        else grow = (int)(((7f - (float)grow) / 7f) * 100);//南瓜
                        infoDialog.setPlantInfo(disname, cellValue, grow);
                    }
                    else
                    {//不是作物
                        grow = 0;
                        dig = block.RequiredToolLevel;
                        infoDialog.setBlockInfo($"{disname}", cellValue, dig, block.DigMethod, toollevel);
                    }
                    infoDialog.IsVisible = true;
                }



            }
            else {
                if(!hasCreature)infoDialog.IsVisible = false;
                else infoDialog.IsVisible = true;
            }
            infoDialog.setBottomWidget(moreInfo);
            infoDialog.setUpmode(SubsystemXjJeiBehavior.upmode);

        }
    }
}
