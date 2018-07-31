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

        public GGVRDataType<byte> BaseID { get; set; }

        public GGVRDataType<float> Height { get; set; }
        public GGVRDataType<float> HeadSizeRatio { get; set; }
        public GGVRDataType<float> Bust { get; set; }
        public GGVRDataType<float> Waist { get; set; }
        public GGVRDataType<float> Hip { get; set; }

        public GGVRDataType<byte> Outfit { get; set; }
        public GGVRDataType<byte> Accessory { get; set; }
        public GGVRDataType<byte> Socks { get; set; }
        public GGVRDataType<byte> Shoes { get; set; }

        public GGVRDataType<byte> Hair { get; set; }
        public GGVRDataType<byte> Face { get; set; }
        public GGVRDataType<byte> Skin { get; set; }
        public GGVRDataType<ColorComparable> EyeColor { get; set; }
        public GGVRDataType<ColorComparable> EyeBrowColor { get; set; }

        public GGVRDataType<byte> Year { get; set; }
        public GGVRDataType<byte> Class { get; set; }
        public GGVRDataType<byte> Post { get; set; }
        public GGVRDataType<byte> BloodType { get; set; }
        public GGVRDataType<byte> BirthMonth { get; set; }
        public GGVRDataType<byte> BirthDay { get; set; }
        public GGVRDataType<byte> WeakPoint { get; set; }

        public GGVRDataType<byte> PersonalityNormal { get; set; }
        public GGVRDataType<float> SpeedNormal { get; set; }
        public GGVRDataType<int> HPNormal { get; set; }
        public GGVRDataType<int> AttackNormal1Strength { get; set; }
        public GGVRDataType<int> AttackNormal2Strength { get; set; }
        public GGVRDataType<int> AttackNormal3Strength { get; set; }

        public GGVRDataType<byte> PersonalityPossessed { get; set; }
        public GGVRDataType<float> SpeedPossessed { get; set; }
        public GGVRDataType<int> HPPossessed { get; set; }
        public GGVRDataType<int> AttackPossessed1Strength { get; set; }
        public GGVRDataType<int> AttackPossessed2Strength { get; set; }
        public GGVRDataType<int> AttackPossessed3Strength { get; set; }

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

            fieldList.Add(this.Hair);
            fieldList.Add(this.Face);
            fieldList.Add(this.Skin);
            fieldList.Add(this.EyeColor);
            fieldList.Add(this.EyeBrowColor);            

            return fieldList.ToArray();
        }

        private GGVRBaseDataType[] GetFieldsGalData()
        {
            List<GGVRBaseDataType> fieldList = new List<GGVRBaseDataType>();

            fieldList.Add(this.Year);
            fieldList.Add(this.Class);
            fieldList.Add(this.Post);
            fieldList.Add(this.BloodType);
            fieldList.Add(this.BirthMonth);
            fieldList.Add(this.BirthDay);
            fieldList.Add(this.WeakPoint);

            fieldList.Add(this.PersonalityNormal);
            fieldList.Add(this.SpeedNormal);
            fieldList.Add(this.HPNormal);
            fieldList.Add(this.AttackNormal1Strength);
            fieldList.Add(this.AttackNormal2Strength);
            fieldList.Add(this.AttackNormal3Strength);

            fieldList.Add(this.PersonalityPossessed);
            fieldList.Add(this.SpeedPossessed);
            fieldList.Add(this.HPPossessed);
            fieldList.Add(this.AttackPossessed1Strength);
            fieldList.Add(this.AttackPossessed2Strength);
            fieldList.Add(this.AttackPossessed3Strength);

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

        public void WriteAllGalData(BinaryWriter bw)
        {
            foreach (GGVRBaseDataType field in GetFieldsGalData())
            {
                if (field != null && field.Enabled)
                {
                    field.WriteToFile(bw);
                }
            }
        }

        public void RestoreAll()
        {
            foreach (GGVRBaseDataType field in GetFields())
            {
                field.RestoreOriginal();
            }
            foreach (GGVRBaseDataType field in GetFieldsGalData())
            {
                field.RestoreOriginal();
            }
        }

        public void SwapWith(GGVRGirl girl2)
        {
            GGVRBaseDataType[] fields1 = this.GetFields();
            GGVRBaseDataType[] fields2 = girl2.GetFields();
            for(int i = 0; i < Math.Min(fields1.Length, fields2.Length); i++)
            {
                GGVRBaseDataType f1 = fields1[i];
                GGVRBaseDataType f2 = fields2[i];
                IComparable v1 = f1.GetValue();
                f1.AssignValue(f2.GetValue());
                f2.AssignValue(v1);
            }

            fields1 = this.GetFieldsGalData();
            fields2 = girl2.GetFieldsGalData();
            for (int i = 0; i < Math.Min(fields1.Length, fields2.Length); i++)
            {
                GGVRBaseDataType f1 = fields1[i];
                GGVRBaseDataType f2 = fields2[i];
                IComparable v1 = f1.GetValue();
                f1.AssignValue(f2.GetValue());
                f2.AssignValue(v1);
            }
        }
    }
}
