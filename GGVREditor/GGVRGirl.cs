using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.IO;

namespace GGVREditor
{
    public class GGVRGirl
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public GGVRDataType<float> Height { get; set; }
        public GGVRDataType<float> HeadSizeRatio { get; set; }
        public GGVRDataType<float> Bust { get; set; }
        public GGVRDataType<float> Waist { get; set; }
        public GGVRDataType<float> Hip { get; set; }

        public GGVRDataType<byte> Outfit { get; set; }
        public GGVRDataType<byte> Accessory { get; set; }
        public GGVRDataType<byte> Socks { get; set; }
        public GGVRDataType<byte> Shoes { get; set; }

        public GGVRDataType<ColorComparable> EyeColor { get; set; }
        public GGVRDataType<ColorComparable> EyeBrowColor { get; set; }

        private GGVRBaseDataType[] GetFields()
        {
            List<GGVRBaseDataType> fieldList = new List<GGVRBaseDataType>();

            fieldList.Add(this.Height);
            fieldList.Add(this.HeadSizeRatio);
            fieldList.Add(this.Bust);
            fieldList.Add(this.Waist);
            fieldList.Add(this.Hip);

            fieldList.Add(this.Outfit);
            fieldList.Add(this.Accessory);
            fieldList.Add(this.Socks);
            fieldList.Add(this.Shoes);

            return fieldList.ToArray();
        }

        public void WriteAll(BinaryWriter bw)
        {
            foreach (GGVRBaseDataType field in GetFields())
            {
                if (field != null && field.Enabled)
                {
                    field.WriteToFile(bw);
                }
            }
        }
    }
}
