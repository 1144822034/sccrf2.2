using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Game
{
    class XjDataType
    {
        private XElement xinit;//初始化
        private Point3 point;
        private string blockname_;
        private XElement xdata_;
        public XjDataType(string blockname,Point3 p,XElement xdata=null){
            blockname_ = blockname;
            point = p;
            xdata_ = xdata;
            init();
        }
        private void init() {
            xinit = new XElement(blockname_);
            xinit.Add(new XElement("point",point));
            xinit.Add(new XElement("data",xdata_));
        }
        public XElement makedata(){
            return this.xinit;
        }
    }
}
