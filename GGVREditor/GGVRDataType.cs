using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Globalization;

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
        }

        public override string ToString() {
            if(!this.Enabled)
            {
                return "-";
            }

            if (typeof(T) == typeof(float))
            {
                float v = (float)(object)this._value;
                return v.ToString("G5", CultureInfo.InvariantCulture);
            }
            if (typeof(T) == typeof(int))
            {
                int v = (int)(object)this._value;
                return v.ToString();
            }
            if(typeof(T) == typeof(byte))
            {
                byte b = (byte)(object)this._value;
                return b.ToString("X").ToUpperInvariant();
            }

            return _value.ToString();
        }

        public override bool SetNewValueFromString(string value)
        {
            bool changedValue = false;

            if (typeof(T) == typeof(float))
            {
                float v;
                float current = (float)(object)this._value;
                if (float.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                {
                    if (Math.Abs(current / v - 1) >= 0.001)
                    {
                        this._value = (T)(object)v;
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
                        this._value = (T)(object)v;
                        changedValue = true;
                    }
                }
            }
            if (typeof(T) == typeof(byte))
            {
                byte b;
                byte current = (byte)(object)this._value;
                if(Byte.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b)) {
                    if(b != current)
                    {
                        this._value = (T)(object)b;
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
