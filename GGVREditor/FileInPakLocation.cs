using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGVREditor
{
    public class FileInPakLocation
    {
        public long FileOffset { get; private set; }
        public int FileSize { get; private set; }
        public long HashOffset { get; private set; }

        public FileInPakLocation(long fileOffset, int fileSize, long hashOffset)
        {
            this.FileOffset = fileOffset;
            this.FileSize = fileSize;
            this.HashOffset = hashOffset;
        }
    }
}
