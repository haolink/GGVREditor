using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGVREditor
{
    public class FileInPakLocation
    {
        public string FileName { get; private set; }
        public long FileOffset { get; private set; }
        public int FileSize { get; private set; }
        public int ReadSize { get; private set; }
        public long HashOffset { get; private set; }
        public bool Encrypted { get; private set; }

        public FileInPakLocation(string fileName, long fileOffset, int fileSize, int readSize,long hashOffset, bool encrypted = false)
        {
            this.FileName = fileName;
            this.FileOffset = fileOffset;
            this.FileSize = fileSize;
            this.ReadSize = readSize;
            this.HashOffset = hashOffset;
            this.Encrypted = encrypted;
        }
    }
}
