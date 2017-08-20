using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using System.Drawing;

namespace GGVREditor
{
    public class EditSettings
    {
        public bool UsePAKFile { get; set; }
        public string GameDirectory { get; set; }

        private IniFile _iniFile;

        private CultureInfo _invariant;

        public EditSettings()
        {
            this._iniFile = new IniFile();
            this.UsePAKFile = this._iniFile.ReadBool("UsePAK", "Main", true);
            this.GameDirectory = this._iniFile.Read("GameDirectory", "Main");

            this._invariant = CultureInfo.InvariantCulture;
        }

        public void SaveSettings()
        {
            this._iniFile.WriteBool("UsePAK", this.UsePAKFile, "Main");
            this._iniFile.Write("GameDirectory", this.GameDirectory, "Main");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueId"></param>
        /// <param name="readValue"></param>
        /// <returns></returns>
        public T GetDefaultValue<T>(string valueId, T readValue) where T : IComparable
        {
            T res = readValue;

            string value = this._iniFile.Read(valueId, "Defaults", "p");

            bool parsable = false;
            if (typeof(T) == typeof(float))
            {
                float resFloat = (float)(object)res;
                if (parsable = float.TryParse(value, NumberStyles.Number, this._invariant, out resFloat))
                {
                    res = (T)(object)resFloat;
                }
            }
            if (typeof(T) == typeof(byte))
            {
                byte resByte = (byte)(object)res;
                if (parsable = byte.TryParse(value, NumberStyles.Integer, this._invariant, out resByte))
                {
                    res = (T)(object)resByte;
                }
            }
            if (typeof(T) == typeof(ColorComparable))
            {
                string[] parts = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if(parsable = (parts.Length == 4))
                {
                    int r, g, b, a;
                    if(int.TryParse(parts[0], out r) && int.TryParse(parts[1], out g) && int.TryParse(parts[2], out b) && int.TryParse(parts[3], out a) 
                        && r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255 && a >= 0 && a <= 255)
                    {
                        parsable = true;

                        ColorComparable cc = new ColorComparable();
                        cc.Color = Color.FromArgb(a, r, g, b);

                        res = (T)(object)cc;
                    }
                }
            }
        
            if (!parsable)
            {
                this.SetDefaultValue(valueId, readValue);
            }                        
            
            return res;
        }

        private void SetDefaultValue<T>(string valueId, T writeValue) where T : IComparable
        {
            if (typeof(T) == typeof(float))
            {
                float wF = (float)(object)writeValue;
                this._iniFile.Write(valueId, wF.ToString(this._invariant), "Defaults");
            }
            if (typeof(T) == typeof(byte))
            {
                float wB = (byte)(object)writeValue;
                this._iniFile.Write(valueId, wB.ToString(), "Defaults");
            }
            if(typeof(T) == typeof(ColorComparable) && writeValue != null)
            {
                ColorComparable cc = (ColorComparable)(object)writeValue;
                if(cc.Color == null)
                {
                    return;
                }
                Color c = cc.Color;
                this._iniFile.Write(valueId, c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + "," + c.A.ToString(), "Defaults");
            }
        }
    }
}
