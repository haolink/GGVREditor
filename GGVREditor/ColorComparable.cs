using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.IO;

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

        public static ColorComparable FromStream(BinaryReader br)
        {
            ColorComparable cc = new ColorComparable();
            int r, g, b, a;
            r = (int)Math.Round(br.ReadSingle() * 255.0f);
            b = (int)Math.Round(br.ReadSingle() * 255.0f);
            g = (int)Math.Round(br.ReadSingle() * 255.0f);
            a = (int)Math.Round(br.ReadSingle() * 255.0f);

            cc.Color = Color.FromArgb(a, r, b, g);

            return cc;
        }

        public void ToStream(BinaryWriter bw)
        {
            float r, g, b, a;
            r = (float)(this.Color.R / 255.0f);
            g = (float)(this.Color.G / 255.0f);
            b = (float)(this.Color.B / 255.0f);
            a = (float)(this.Color.A / 255.0f);

            bw.Write(r);
            bw.Write(g);
            bw.Write(b);
            bw.Write(a);
        }

        public override string ToString()
        {
            return String.Format("#{0:X02}{1:X02}{2:X02}{3:X02}", this.Color.R, this.Color.B, this.Color.G, this.Color.A);
        }
    }
}
