using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace GGVREditor
{
    public partial class UnpackProgress : Form
    {
        public string PakFile { get; set; }
        public string SigFile { get; set; }

        public string UnpackFolder { get; set; }

        private bool _completed;

        private byte[] _decryptionKey;
        private byte[] _decryptionIV;

        public UnpackProgress()
        {
            InitializeComponent();

            this._completed = false;
            this._decryptionKey = Encoding.ASCII.GetBytes("k14z0ZLR8a7jNm49uyBzxXYY9LpTHceh");
            this._decryptionIV  = Encoding.ASCII.GetBytes("LSNiC3jAkzBsffPuy8YsTa72RLD9KWIn");
        }

        private void UnpackProgress_Shown(object sender, EventArgs e)
        {
            this.lblUnpackFile.Text = "Reading file header";
            
            if(File.Exists(this.PakFile + ".bak") || File.Exists(this.SigFile + ".bak"))
            {
                if(MessageBox.Show("PAK file has been unpacked before, a backup file exists. Is it okay to overwrite this backup file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    this._completed = true;
                    Close();
                }                
            }
            
            StartUnpackThreaded();            
        }

        private void StartUnpackThreaded()
        {
            Thread t = new Thread(RunUnpack);
            t.Start();
        }

        private void RunUnpack()
        {
            while (File.Exists(this.PakFile + ".bak"))
            {
                File.Delete(this.PakFile + ".bak");
                Thread.Sleep(10);
            }

            while (File.Exists(this.SigFile + ".bak"))
            {
                File.Delete(this.SigFile + ".bak");
                Thread.Sleep(10);
            }

            File.Move(this.PakFile, this.PakFile + ".bak");
            File.Move(this.SigFile, this.SigFile + ".bak");

            List<FileInPakLocation> files = new List<FileInPakLocation>();

            FileStream fs = new FileStream(this.PakFile + ".bak", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);

            if (fs.Length < 100)
            {
                FinishThreadSafe();
                fs.Close();
                Close();
            }

            br.BaseStream.Seek(-0x2C, SeekOrigin.End);
            uint magic = br.ReadUInt32();
            if (magic != 0x5A6F12E1)
            {
                FinishThreadSafe();
                fs.Close();
                Close();
            }
            br.ReadUInt32();
            long offset = br.ReadInt64();
            long size = br.ReadInt64();

            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            ReadName(br);

            int fileCount = br.ReadInt32();

            for (int i = 0; i < fileCount; i++)
            {
                string fileName = ReadName(br);
                long pos = br.BaseStream.Position;

                long fOffset = br.ReadInt64();
                long fZSize = br.ReadInt64();
                long fSize = br.ReadInt64();
                int compression = br.ReadInt32();

                long hashLocation = br.BaseStream.Position;
                br.BaseStream.Seek(20, SeekOrigin.Current);

                byte enc = br.ReadByte();
                int chunkSize = br.ReadInt32();

                bool encrypted = (enc != 0);
                int eSize = (int)fSize;

                if(encrypted)
                {
                    if (eSize % 16 > 0)
                    {
                        eSize = ((eSize / 16) + 1) * 16;
                    }
                }

                long bPos = br.BaseStream.Position;
                fOffset += (bPos - pos);

                files.Add(new FileInPakLocation(fileName, fOffset, (int)fSize, eSize, hashLocation, (enc != 0)));
            }

            int fCount = files.Count;
            for(int i = 0; i < fCount; i++)
            {
                FileInPakLocation fipl = files[i];
                this.ShowProgressThreadSafe(i, fCount);
                UnpackFile(fipl, fs);                
            }

            br = null;
            fs.Close();            

            FinishThreadSafe();
        }

        private string ReadName(BinaryReader br)
        {
            int length = br.ReadInt32();
            if (length >= 0) //Ascii
            {
                byte[] buffer = new byte[length];
                br.BaseStream.Read(buffer, 0, length);
                return Encoding.ASCII.GetString(buffer).Trim(new char[] { '\0', '\r', '\n' });
            }
            else
            {
                length = (-2) * length;
                byte[] buffer = new byte[length];
                br.BaseStream.Read(buffer, 0, length);
                return Encoding.Unicode.GetString(buffer).Trim(new char[] { '\0', '\r', '\n' });
            }
        }

        private void ShowProgressThreadSafe(int fileNo, int fileCount)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ShowProgress(fileNo, fileCount);
                });
            }
            else
            {
                ShowProgress(fileNo, fileCount);
            }
        }

        private void ShowProgress(int fileNo, int fileCount)
        {
            int v = (int)Math.Round((float)fileNo / (float)fileCount * 100.0f);

            this.lblUnpackFile.Text = "Unpacking file " + (fileNo + 1).ToString() + " of " + fileCount.ToString();
            this.pbUnpackingProgress.Value = v;
        }


        private void FinishThreadSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Finish();
                });
            }
            else
            {
                Finish();
            }
        }

        private void Finish()
        {
            this._completed = true;
            Close();
        }

        private void UnpackProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (!this._completed);
        }

        private void UnpackFile(FileInPakLocation fipl, FileStream fs)
        {
            string outputFileName = this.UnpackFolder + Path.DirectorySeparatorChar + fipl.FileName.Replace('/', '\\');

            FileInfo file = new FileInfo(outputFileName);
            file.Directory.Create();
            string fullName = file.FullName;

            byte[] buffer = new byte[fipl.ReadSize];
            fs.Seek(fipl.FileOffset, SeekOrigin.Begin);
            fs.Read(buffer, 0, fipl.ReadSize);

            if(fipl.Encrypted)
            {
                buffer = Decrypt(buffer, fipl.FileSize);
            }

            FileStream ofs = new FileStream(fullName, FileMode.Create, FileAccess.Write, FileShare.None);
            ofs.Write(buffer, 0, fipl.FileSize);
            ofs.Close();

            buffer = null;
        }

        public byte[] Decrypt(byte[] cipher, int fileSize)
        {
            //init AES 128
            RijndaelManaged aes128 = new RijndaelManaged();
            aes128.Mode = CipherMode.ECB;
            aes128.Padding = PaddingMode.Zeros;

            int length = cipher.Length;
            

            ICryptoTransform decryptor = aes128.CreateDecryptor(this._decryptionKey, null);
            MemoryStream ms = new MemoryStream(cipher);
            CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

            byte[] plain = new byte[length];
            int decryptcount = cs.Read(plain, 0, length);

            ms.Close();
            cs.Close();

            byte[] result = new byte[fileSize];
            Buffer.BlockCopy(plain, 0, result, 0, fileSize);

            //return plaintext in String
            return result;
        }
    }
}
