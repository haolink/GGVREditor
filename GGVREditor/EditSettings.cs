using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

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
            string value = this._iniFile.Read(valueId, "Defaults", "p");
            T res = readValue;

            bool parsable = false;
            if(typeof(T) == typeof(float))
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
        }
    }
}
