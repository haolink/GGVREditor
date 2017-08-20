using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace GGVREditor
{
    public abstract class GGVRBaseDataType
    {
        protected long _address;

        public long Address
        {
            get { return _address; }
        }

        protected bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
        }

        public abstract Type ValueType { get; } 

        public abstract void WriteToFile(BinaryWriter bw);

        public abstract bool SetNewValueFromString(string value);
        
        public abstract override string ToString();

        public abstract IComparable GetValue();        
        public abstract void AssignValue(IComparable newValue);

        public abstract void RestoreOriginal();
    }
}
