using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Engine;

namespace Game
{
    public class XjJeiLibrary
    {
        public XjJeiLibrary() { }

        public static float caculateWidth(LabelWidget labelWidget, float scale, string str, float maxwidth)
        {
            float width = 0;
            foreach (char c in str)
            {
                width += (labelWidget.Font.GetGlyph(c).Width + labelWidget.Font.GetGlyph(c).Offset.X) * scale;
                if (width > maxwidth) width = maxwidth;
            }
            return width;
        }
        public static float caculateHeight(LabelWidget sendtext, int linecnt, float scale)
        {

            return (sendtext.Font.LineHeight + sendtext.Font.GetGlyph('0').Offset.Y) * scale * linecnt;
        }
        public static void moveInventoryByCraft(IInventory srcInventory, IInventory toInventory, Dictionary<int, int> items, ComponentGui gui)
        {
            //items格式 id 数量
            Dictionary<int, int> fromList = new Dictionary<int, int>();
            Dictionary<int, int> toList = new Dictionary<int, int>();

            for (int i = 0; i < srcInventory.SlotsCount; i++)
            {
                if (srcInventory.GetSlotCount(i) != 0)
                {
                    if (fromList.ContainsKey(srcInventory.GetSlotValue(i))) fromList[srcInventory.GetSlotValue(i)] = srcInventory.GetSlotCount(i) + fromList[srcInventory.GetSlotValue(i)];
                    else fromList.Add(srcInventory.GetSlotValue(i), srcInventory.GetSlotCount(i));
                }
            }
            for (int i = 0; i < toInventory.SlotsCount; i++)
            {
                if (srcInventory.GetSlotCount(i) != 0)
                {
                    if (toList.ContainsKey(toInventory.GetSlotValue(i))) toList[srcInventory.GetSlotValue(i)] = toInventory.GetSlotCount(i) + toList[toInventory.GetSlotValue(i)];
                    else toList.Add(toInventory.GetSlotValue(i), toInventory.GetSlotCount(i));
                }
            }
            int useSlotCount = 0; int ja = 0; int allcnt;
            //判断能否全部转移
            foreach (int id in fromList.Keys)
            {//id,cnt
                if (toList.ContainsKey(id))
                {
                    allcnt = toList[id] + fromList[id];
                    useSlotCount += (int)Math.Ceiling((double)allcnt / (double)toInventory.GetSlotCapacity(ja, id));
                }
                else
                {
                    allcnt = fromList[id];
                    useSlotCount += (int)Math.Ceiling((double)allcnt / (double)toInventory.GetSlotCapacity(ja, id));
                }
                ++ja;
            }
            if (useSlotCount > toInventory.SlotsCount)
            { //不能全部转移
                gui.DisplaySmallMessage("背包空间不足，不能转移物品", Color.Red, false, false);
            }
            else
            { //能全部转移
                //先移除全部
                for (int i = 0; i < srcInventory.SlotsCount; i++)
                {
                    if (srcInventory.GetSlotCount(i) == 0) continue;
                    srcInventory.RemoveSlotItems(i, srcInventory.GetSlotCount(i));
                }
                for (int i = 0; i < toInventory.SlotsCount; i++)
                {
                    if (toInventory.GetSlotCount(i) == 0) continue;
                    toInventory.RemoveSlotItems(i, toInventory.GetSlotCount(i));
                }
                int uu = 0;
                for (int i = 0; i < toInventory.SlotsCount; i++)
                {
                    for (int j = 0; j < (int)Math.Ceiling((double)toList[toList.Keys.ToArray()[i]] / (double)toInventory.GetSlotCapacity(ja, toList.Keys.ToArray()[i])); j++)//计算转移次数
                    {
                        int cap = toInventory.GetSlotCapacity(uu, toList.Keys.ToArray()[i]);
                        if (toList.Values.ToArray()[i] - cap < 0) cap = toList.Values.ToArray()[i]; else toList[toList.Keys.ToArray()[i]] = toList[toList.Keys.ToArray()[i]] - cap;
                        toInventory.AddSlotItems(uu, toList.Keys.ToArray()[i], cap);
                    }
                    ++uu;
                }
                gui.DisplaySmallMessage("转移成功!",Color.Red, false, false);
            }
        }
        public static List<string> parseCommand(string str) {
            List<string> param = new List<string>();
            if (str.StartsWith("/")) {
                if (str.Contains(" ")) {
                    string[] cmds = str.Split(new char[] { ' '},StringSplitOptions.None);
                    for (int i=0;i<cmds.Length;i++) {
                        if (i == 0) {
                            string hu = cmds[i].Split(new char[] { '/'},StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                            param.Add(hu);
                        } else {
                            param.Add(cmds[i]);
                        }
                    }
                
                } else {
                    string cmd = str.Split(new char[] { '/'},StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    param.Add(cmd);
                }                      
            }
            return param;        
        }
        public static void moveInventory(IInventory a, IInventory b,ComponentPlayer player, GameMode mode)
        {
            bool remo = false;
            for (int i = 0; i < a.SlotsCount; i++)
            {//遍历目标物品
                if (a.GetSlotCount(i) > 0)
                {//该格有物品
                    for (int k = 0; k < b.SlotsCount; k++)
                    {//遍历背包
                        if ((b.GetSlotCount(k) == 0 || b.GetSlotValue(k) == a.GetSlotValue(i)))
                        {
                            //背包该格为空或者物品相同
                            //能够转移的数量
                            int needmovecnt = b.GetSlotCapacity(i, a.GetSlotValue(i)) - b.GetSlotCount(k);
                            //总共的转移数量
                            int allmovecnt = a.GetSlotCount(i);
                            if (needmovecnt >= allmovecnt) needmovecnt = allmovecnt;
                            if (needmovecnt == 0) { i -= 1; continue; }
                            else
                            {
                                if (mode != GameMode.Creative) b.AddSlotItems(k, a.GetSlotValue(i), needmovecnt);
                                a.RemoveSlotItems(i, needmovecnt);
                            }
                        }
                    }
                }
            }
            if (remo) player.ComponentGui.DisplaySmallMessage("一键转移成功", Color.Red, false, false);
            else { player.ComponentGui.DisplaySmallMessage("没有需要转移的物品呢", Color.Red, false, false); }
        }
    }
}
