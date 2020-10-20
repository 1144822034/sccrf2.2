using Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Game
{
    public class XjJeiManager
    {
        public List<int> values = new List<int>();
        public List<Block> blocks = new List<Block>();
        public List<string> names = new List<string>();
        public List<string> craftids = new List<string>();
        public static bool is_NewSC=false;
        public Dictionary<string, string> newblockcrafts = new Dictionary<string, string>();
        public XjJeiManager()
        {
            init();
        }
        public int getCount()
        {
            return values.Count();
        }
        public int getPosByValue(int value)
        {
            int k = 0;
            foreach (int d in values)
            {
                if (d == value) return k;
                ++k;
            }
            return 0;
        }
        public bool checkInput_Allen(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > 127) return false;
            }
            return true;
        }
        public List<int> findConnCraftBlocks(string craftid)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < CraftingRecipesManager.Recipes.Count; i++)
            {
                for (int j = 0; j < CraftingRecipesManager.Recipes[i].Ingredients.Length; j++)
                {
                    if (CraftingRecipesManager.Recipes[i].Ingredients[j] == craftid) if (!list.Contains(CraftingRecipesManager.Recipes[i].ResultValue))
                        {
                            list.Add(CraftingRecipesManager.Recipes[i].ResultValue);
                            break;
                        }
                }
            }
            return list;
        }
        public List<int> findBlocksByName(String named)
        {//返回绝对位置
            List<int> ids = new List<int>(); int k = 0;
            string name = "";
            foreach (string tm in names)
            {
                if (tm.Contains(named)) ids.Add(k);
                if (checkInput_Allen(named))
                {//全英文，快捷搜索
                    name = named.ToLower();
                    bool found = true;
                    string mpp = ConvertPY.getPinyin(tm);
                    string removed = "";
                    string[] mppa = mpp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string mao in mppa) { removed += mao; }
                    if ((removed.Contains(name)) && !ids.Contains(k)) { ids.Add(k); }//全部检查
                    for (int y = 0; y < mppa.Length; y++)
                    {
                        if (y >= name.Length - 1) break;
                        if (mppa[y][0] != name[y]) { found = false; break; }
                    }
                    if (found && !ids.Contains(k)) ids.Add(k);//部分检查
                }
                ++k;
            }
            return ids;
        }
        public string getNameByPos(int pos)
        {
            return names[pos];
        }
        public int getValueByPos(int pos)
        {
            return values[pos];
        }
        public string getCraftIdByPos(int pos)
        {
            return craftids[pos];
        }
        public string getCateByPos(int pos)
        {
            int value = values[pos];
            return blocks[pos].GetCategory(value);
        }
        public List<CraftingRecipe> getRecipesByPos(int value)
        {
            string cate = blocks[0].GetCategory(values[value]);
            if (cate == "Painted" || cate.Contains("染色"))
            {
                value = getValueByPos(Terrain.ReplaceData(values[value], 0));
            }
            List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();
            foreach (CraftingRecipe crafting in CraftingRecipesManager.Recipes)
            {
                    if (crafting.ResultValue == values[value]) craftingRecipes.Add(crafting);
                }
            return craftingRecipes;
        }
        public int getPosByCraftid(string craftid)
        {
            int k = 0;
            foreach (string mp in craftids)
            {
                if (mp == craftid) return k;
                ++k;
            }
            return 0;
        }
        public List<List<int>> decodeIngredian(string[] ingredians)
        {//把配方解析成Block的value
            List<List<int>> result = new List<List<int>>();
            foreach (string needname in ingredians)
            {
                List<int> values = new List<int>();
                if (string.IsNullOrEmpty(needname))
                {
                    result.Add(values);
                    continue;
                }
                if (needname.Contains(":"))
                {
                    string[] split = needname.Split(':');
                    Block[] array = BlocksManager.FindBlocksByCraftingId(split[0]);
                    if (array != null)
                    {
                        int dd = int.Parse(split[1], CultureInfo.InvariantCulture);
                        int tt = Terrain.MakeBlockValue(array[0].BlockIndex, 0, dd);
                        values.Add(getPosByValue(tt));
                    }
                }
                else
                {
                    int k = 0;
                    foreach (string element in craftids)
                    {

                        if (element == needname)
                        {                          
                             values.Add(k);
                        }
                        ++k;
                    }
                }

                result.Add(values);
            }
            return result;
        }
        public void init()
        {
            bool isNewBlock = false;
            foreach (ModInfo modInfo in ModsManager.LoadedMods)
            {
                if (modInfo.Name == "NewBlocks_SC") { isNewBlock = true;is_NewSC = true; }
            }
            if (isNewBlock)
            {
                XElement item = ModsManager.CombineXml(new XElement("NewBlocks"), ModsManager.GetEntries(".xjdb"));
               
                foreach (XElement element in item.Elements())
                {
                    foreach (XElement element1 in element.Elements())
                    {
                        if (!newblockcrafts.ContainsKey(element1.Attribute("DisplayName").Value))
                        {                           
                            newblockcrafts.Add(element1.Attribute("DisplayName").Value, element1.Attribute("CraftingId").Value);
                        }
                    }
                }
            }


                int k = 0;
                values.Add(0);
                blocks.Add(BlocksManager.Blocks[0]);
                names.Add("空气");
                craftids.Add("空气");
                foreach (Block block in BlocksManager.Blocks)
                {
                    foreach (int cv in block.GetCreativeValues())
                    {//添加所有方块
                        if (cv == 0) continue;
                        if (values.Contains(cv)) continue;
                        string ll = block.GetDisplayName(null, cv);
                    if (XjJeiManager.is_NewSC && ModsManager.customer_Strings.ContainsKey(ll))
                    {
                        ll = ModsManager.customer_Strings[ll];
                    }
                    if (string.IsNullOrEmpty(ll)) continue;
                        if (block.GetCategory(cv) == "Painted") continue;
                    if (isNewBlock)
                    {
                        if (newblockcrafts.TryGetValue(ll, out string namm))
                        {
                            craftids.Add(namm);
                        }
                        else craftids.Add(block.CraftingId);
                    }
                    else {
                         craftids.Add(block.CraftingId);
                    }
                        values.Add(cv);
                        blocks.Add(block);
                        names.Add(ll);
                        k += 1;
                    }
                }

            }

        }
    }

