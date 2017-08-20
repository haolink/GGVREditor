using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace GGVREditor
{
    public class IniFile
    {
        private string _path;

        public string Path
        {
            get { return _path; }
        }

        private static string _EXE = Assembly.GetExecutingAssembly().GetName().Name;

        private const int BUFFER_SIZE = 0x8000;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, string RetVal, int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            _path = new FileInfo(IniPath ?? _EXE + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null, string DefaultValue = "")
        {
            var RetVal = new string(' ', BUFFER_SIZE);
            GetPrivateProfileString(Section ?? _EXE, Key, DefaultValue, RetVal, BUFFER_SIZE, _path);
            return RetVal.Substring(0, RetVal.IndexOf('\0'));
        }

        public string[] ReadKeys(string Section = null)
        {
            var RetVal = new string(' ', BUFFER_SIZE);
            GetPrivateProfileString(Section ?? _EXE, null, "", RetVal, BUFFER_SIZE, _path);
            return RetVal.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] ReadSections()
        {
            var RetVal = new string(' ', BUFFER_SIZE);
            GetPrivateProfileString(null, null, "", RetVal, BUFFER_SIZE, _path);
            return RetVal.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? _EXE, Key, Value, _path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? _EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? _EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        public void WriteBool(string Key, bool Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? _EXE, Key, (Value ? "1" : "0"), _path);
        }

        public bool ReadBool(string Key, string Section = null, bool Default = false)
        {
            return (Read(Key, Section, Default ? "1":"0").Trim() == "1");
        }

    }
}
