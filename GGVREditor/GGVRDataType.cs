using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Globalization;
using System.Drawing;

namespace GGVREditor
{
    public class GGVRDataType<T> : GGVRBaseDataType where T : IComparable
    {
        protected T _value;
        protected T _originalValue;

        public T Value
        {
            get { return _value; }
            set {
                if (_enabled)
                {
                    _value = value;
                }
            }
        }

        public T OriginalValue
        {
            get
            {
                return _originalValue;
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(T);
            }
        }

        public bool HasValueRange { get; set; }

        public T MinimumValue { get; set; }
        public T MaximumValue { get; set; }

        public void AssignValueRange(T minimal, T maximum)
        {
            this.HasValueRange = true;
            this.MinimumValue = minimal;
            this.MaximumValue = maximum;
        }

        public void RemoveValueRange()
        {
            this.HasValueRange = false;
        }

        public bool Available
        {
            get { return _enabled; }
        }        


        public GGVRDataType(BinaryReader br, long? offset = null)
        {
            if(offset != null)
            {
                br.BaseStream.Seek(offset.GetValueOrDefault(), SeekOrigin.Begin);
            }

            this._address = br.BaseStream.Position;
            this._enabled = true;

            this.HasValueRange = false;

            if (typeof(T) == typeof(float))
            {
                _value = (T)(object)(br.ReadSingle());
            }
            if (typeof(T) == typeof(int))
            {
                _value = (T)(object)(br.ReadInt32());
            }
            if (typeof(T) == typeof(byte))
            {
                _value = (T)(object)(br.ReadByte());
            }
            if (typeof(T) == typeof(ColorComparable))
            {                
                _value = (T)(object)(ColorComparable.FromStream(br));
            }
        }

        public GGVRDataType(bool enabled = false)
        {
            if(enabled)
            {
                throw new IOException("Cannot create an enabled set without address!");
            }
            
            this._enabled = false;
            this._address = -1;
            if(typeof(T) == typeof(int) || typeof(T) == typeof(byte))
            {
                _value = (T)(object)0;
            }
            if (typeof(T) == typeof(float))
            {
                _value = (T)(object)(0.0f);
            }
            if(typeof(T) == typeof(ColorComparable))
            {
                _value = (T)(object)new ColorComparable();
            }
        }

        public override void WriteToFile(BinaryWriter bw) {
            if(!this.Enabled || this.Address < 0)
            {
                return;
            }

            bw.BaseStream.Seek(this._address, SeekOrigin.Begin);

            if (typeof(T) == typeof(float))
            {
                float w = (float)(object)this._value;
                bw.Write(w);
            }
            if (typeof(T) == typeof(int))
            {
                int w = (int)(object)this._value;
                bw.Write(w);
            }
            if (typeof(T) == typeof(byte))
            {
                byte w = (byte)(object)this._value;
                bw.Write(w);
            }
            if (typeof(T) == typeof(ColorComparable))
            {
                ColorComparable cc = (ColorComparable)(object)this._value;
                cc.ToStream(bw);
            }
        }

        public override string ToString() {
            if(!this.Enabled)
            {
                return "-";
            }

            if (typeof(T) == typeof(float))
            {
                float v = (float)(object)this._value;
                return String.Format(CultureInfo.InvariantCulture, "{0:###############0.##}", v);
            }
            if (typeof(T) == typeof(int))
            {
                int v = (int)(object)this._value;
                return v.ToString();
            }
            if(typeof(T) == typeof(byte))
            {
                byte b = (byte)(object)this._value;
                return b.ToString();
            }
            if (typeof(T) == typeof(ColorComparable))
            {
                ColorComparable cc = (ColorComparable)(object)this._value;
                return cc.ToString();
            }

            return _value.ToString();
        }

        private T NormaliseValue(T value)
        {
            if(!this.HasValueRange)
            {
                return value;
            }

            if(value.CompareTo(this.MinimumValue) < 0)
            {
                return this.MinimumValue;
            }

            if(value.CompareTo(this.MaximumValue) > 0)
            {
                return this.MaximumValue;
            }

            return value;
        }

        public override bool SetNewValueFromString(string value)
        {
            bool changedValue = false;
            T newValue;

            if (typeof(T) == typeof(float))
            {
                float v;
                float current = (float)(object)this._value;
                if (float.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                {
                    if (Math.Abs(current / v - 1) >= 0.001)
                    {
                        newValue = (T)(object)v;                    

                        this._value = this.NormaliseValue(newValue);
                        changedValue = true;
                    }
                }
            }
            if (typeof(T) == typeof(int))
            {
                int v;
                int current = (int)(object)this._value;
                if (int.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                {
                    if (v != current)
                    {
                        newValue = (T)(object)v;

                        this._value = this.NormaliseValue(newValue);
                        changedValue = true;
                    }
                }
            }
            if (typeof(T) == typeof(byte))
            {
                byte b;
                byte current = (byte)(object)this._value;
                if(Byte.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out b)) {
                    if(b != current)
                    {
                        newValue = (T)(object)b;

                        this._value = this.NormaliseValue(newValue);
                        changedValue = true;
                    }
                }
            }

            return changedValue;
        }

        public void AssignOriginalValue(T originalValue)
        {
            this._originalValue = originalValue;
        }

        public override void RestoreOriginal()
        {
            this._value = this._originalValue;
        }

        public override IComparable GetValue()
        {
            return _value;
        }

        public override void AssignValue(IComparable newValue)
        {
            if (newValue is T)
            {
                this._value = (T)newValue;
            }
        }
    }
}
