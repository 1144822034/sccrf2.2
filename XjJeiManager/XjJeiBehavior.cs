using Engine;
using Engine.Graphics;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplatesDatabase;

namespace Game
{
    public class SubsystemXjJeiBehavior : SubsystemBlockBehavior
    {
        public struct MarkFLag {
            public Vector3 point;
            public string name;
            public Color color;
        }
        public static List<MarkFLag> markFLags = new List<MarkFLag>();
        public override int[] HandledBlocks => new int[] { 1010 };
        public ComponentPlayer player;
        public XjJEIWidget xjJEIWidget;
        public List<Block> allblocks = new List<Block>();
        public static Terrain terrain;
        public static bool run = false;
        public static bool showjump = true;
        public static bool posi = false;
        public static bool dura = false;
        public static bool upmode = false;
        public static int color = 0;
        public static Vector2 jumpsize;
        public static Vector2 margin = Vector2.Zero;
        public static bool margin_changed = true;
        public SubsystemXjJeiBehavior() {           
        }
        public override bool OnUse(Ray3 ray3, ComponentMiner componentMiner)
        {
            try
            {
               
                player = componentMiner.ComponentPlayer;   
                if (xjJEIWidget == null) xjJEIWidget = new XjJEIWidget(this);
                player.ComponentGui.ModalPanelWidget = xjJEIWidget;
            }
            catch (Exception e)
            {
                toast(e.ToString());
            }

            return true;
        }
        public override void Dispose()
        {
            markFLags.Clear();
        }
        public void toast(string str)
        {
            if (player != null) player.ComponentGui.DisplaySmallMessage(str,Color.Red,false,false);
        }
        public override void Load(ValuesDictionary valuesDictionary)
        {
            base.Load(valuesDictionary);
            run = valuesDictionary.GetValue<bool>("CellInfoSwitch");
            color = valuesDictionary.GetValue<int>("CellInfoColor");
            posi = valuesDictionary.GetValue<bool>("CellInfoPosi");
            dura = valuesDictionary.GetValue<bool>("CellInfoDura");
            upmode = valuesDictionary.GetValue<bool>("CellInfoUpMode");
            margin.X = valuesDictionary.GetValue<int>("MarginX");
            margin.Y = valuesDictionary.GetValue<int>("MarginY");
            showjump = valuesDictionary.GetValue<bool>("ShowJump");
            jumpsize.X= (float)valuesDictionary.GetValue<int>("JumpSize");
            jumpsize.Y = (float)valuesDictionary.GetValue<int>("JumpSize");
            foreach (ValuesDictionary valuePairs in valuesDictionary.GetValue<ValuesDictionary>("MarkFlags").Values) {
                MarkFLag markFLag = new MarkFLag();
                markFLag.color = valuePairs.GetValue<Color>("color");
                markFLag.point = valuePairs.GetValue<Vector3>("point");
                markFLag.name = valuePairs.GetValue<string>("name");
                markFLags.Add(markFLag);
            }
        }
        public override void Save(ValuesDictionary valuesDictionary)
        {
            base.Save(valuesDictionary);
            valuesDictionary.SetValue("CellInfoSwitch", run);
            valuesDictionary.SetValue("CellInfoColor", color);
            valuesDictionary.SetValue("CellInfoPosi", posi);
            valuesDictionary.SetValue("CellInfoDura", dura);
            valuesDictionary.SetValue("CellInfoUpMode", upmode);
            valuesDictionary.SetValue("MarginX",(int)margin.X);
            valuesDictionary.SetValue("MarginY", (int)margin.Y);
            valuesDictionary.SetValue("JumpSize", (int)jumpsize.X);
            valuesDictionary.SetValue("ShowJump",showjump);
            ValuesDictionary keys = new ValuesDictionary();
            int y = 0;
            foreach (MarkFLag markFLag in markFLags)
            {
                ValuesDictionary valuePairs = new ValuesDictionary();
                valuePairs.SetValue("color", markFLag.color);
                valuePairs.SetValue("point", markFLag.point);
                valuePairs.SetValue("name", markFLag.name);
                keys.SetValue(y.ToString(), valuePairs);
                ++y;
            }
            valuesDictionary.SetValue("MarkFlags", keys);
        }
    }
}
