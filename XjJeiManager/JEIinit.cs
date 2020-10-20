using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    [PluginLoader("SCCRF", "SC合成书", 116u,"2.1.14","","","","","","","","zh-cn")]

    public class JEIinit
    {
       public static void Initialize() {
            CodeTable.init();
        }
    }
}
