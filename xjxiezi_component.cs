using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesDatabase;

namespace Game
{
    public class xjxiezi_component : Component
    {
        public String str = null;
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            this.str = valuesDictionary.GetValue("xjblock",defaultValue:"none");
            base.Load(valuesDictionary, idToEntityMap);
        }
        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue("xjblock",this.str);
            base.Save(valuesDictionary, entityToIdMap);
        }


    }
}
