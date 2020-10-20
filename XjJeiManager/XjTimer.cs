using System;
namespace Game
{
   public class XjTimer
    {
        public int delay=0;
        public Action function;
        public bool run = false;
        public double lastupdatetime = 0;
        public XjTimer(int de) {
            delay = de;
        }
        public void setAction(Action action) {
            function = action;
        }
        public void setRun(bool s) {
            run = s;
        }
        public void setDelay(int d) {
            delay = d;
        }
        public void update() {
            double x = (Engine.Time.RealTime - lastupdatetime) * 1000;
            if ((x>delay)&&run) {//计算毫秒
                function?.Invoke();
                lastupdatetime = Engine.Time.RealTime;
            }
        }

    }
}
