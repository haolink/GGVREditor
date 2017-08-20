using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace GGVREditor
{
    public class ColorComparable : IComparable
    {
        public Color Color { get; set; }

        public int CompareTo(object obj)
        {
            if(obj is ColorComparable)
            {
                ColorComparable cc = (ColorComparable)obj;
                if(this.Color == null && cc.Color == null)
                {
                    return 0;
                }
                if(this.Color == null)
                {
                    return -1;
                }
                if(cc.Color == null)
                {
                    return 1;
                }

                int ret = this.Color.GetHue().CompareTo(cc.Color.GetHue());
                if(ret != 0)
                {
                    return ret;
                }
                ret = this.Color.GetSaturation().CompareTo(cc.Color.GetSaturation());
                if (ret != 0)
                {
                    return ret;
                }
                return this.Color.GetBrightness().CompareTo(cc.Color.GetBrightness());
            }
            else
            {
                return 0;
            }
        }
    }
}
