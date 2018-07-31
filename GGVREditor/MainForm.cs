using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Resources;
using System.Collections;

namespace GGVREditor
{
    public partial class MainForm : Form
    {
        private EditSettings _settings;

        internal enum SelectedGrid
        {
            Undetermined, Fixed
        }

        private const string FILE_EXECUTABLE = @"GalGunVR\Binaries\Win64\GalGunVR-Win64-Shipping.exe";

        private const string FILE_GAL_DATA_UASSET = @"GalGunVR\Content\VRGG\DataTable\GalData\GalData.uasset";
        private const string FILE_GAL_VISUAL_DATA_UASSET = @"GalGunVR\Content\VRGG\DataTable\GalData\GalVisualDatas.uasset";
        private const string FILE_PLAYER_PARAMETERS_UASSET = @"GalGunVR\Content\VRGG\DataTable\Shooting\PlayerParameters.uasset";

        private const string FILE_GIRLS_HEIGHT_CURVE_UASSET = @"GalGunVR\Content\VRGG\AI\GAL\ChangeBodySize\GirlsHeightCurve.uasset";
        private const string FILE_GIRLS_BUST_CURVE_UASSET = @"GalGunVR\Content\VRGG\AI\GAL\ChangeBodySize\BustSizeCurve.uasset";
        private const string FILE_GIRLS_WAIST_CURVE_UASSET = @"GalGunVR\Content\VRGG\AI\GAL\ChangeBodySize\Lumber1SizeCurve.uasset";
        private const string FILE_GIRLS_HIP_CURVE_UASSET = @"GalGunVR\Content\VRGG\AI\GAL\ChangeBodySize\HipSizeCurve.uasset";


        private const string FILE_PAK_FILE = @"GalGunVR\Content\Paks\GalGunVR-WindowsNoEditor.pak";
        private const string FILE_SIG_FILE = @"GalGunVR\Content\Paks\GalGunVR-WindowsNoEditor.sig";

        private static readonly string[] unpackedFilesCheck = new string[]
        {
            FILE_GAL_VISUAL_DATA_UASSET, FILE_GIRLS_HEIGHT_CURVE_UASSET, FILE_PLAYER_PARAMETERS_UASSET, FILE_GAL_DATA_UASSET
        };

        private static readonly string[] packedFilesCheck = new string[]
        {
            FILE_PAK_FILE
        };

        private static readonly string[] packedFilesRequired = new string[]
        {
            FILE_GAL_VISUAL_DATA_UASSET, FILE_GIRLS_HEIGHT_CURVE_UASSET, FILE_PLAYER_PARAMETERS_UASSET, FILE_GAL_DATA_UASSET
        };

        private Dictionary<string, FileInPakLocation> _pakOffsets;

        private Control _selectedControl;

        private bool _edited;

        public static readonly string[] characterNames = new string[]
            {
                "Sayaka Nitta", "Megumi Tendoh", "Tsukasa Shiguchi", "Rion Harusame", "Otome Kurosawa", "Mai Aoshima", "Maki Sudoh", "Anita Bellman",
                "Yukina Jinbo", "Kanko Shishido", "Tsubomi Haruno", "Neneko Kosugi", "Saki Takada", "Maria Natsuki", "Tsuzumi Mursame", "Midori Hanba",
                "Ruriko Mikasa", "Urakara Nishibina", "Karen Sazanami", "Rikiko Tanaka", "Kazami Saijoh", "Makdoka Tsukimiya", "Suzume Asano",
                "Saori Fujiwara", "Yurina Gozu", "Mafuyu Yanagida", "Konomi Kujirai", "Yuri Tsurugi", "Ren Yoshikawa", "Shizuka Ninomiya", "Saori Fujino",
                "Michiyo Azuma", "Rena Kuribayashi", "Shinobu Kamizono", "Maya Kamizono", "Kurona", "Yuko Yureino", "Risu", "Chiru"
            };

        public static readonly Dictionary<byte, string> outfitValues = new Dictionary<byte, string>()
        {
            { 0x81, "Risu (0x81)" }, { 0x82, "Chiru (0x82)" }, { 0x83, "Yuko (0x83)" }, { 0x84, "Shinobu (0x84)" }, { 0x85, "Maya (0x85)" }, { 0x86, "1st grade (0x86)" },
            { 0x87, "2nd grade (0x87)" }, { 0x88, "3rd grade (0x88)" }, { 0x89, "Teacher (0x89)" }, { 0x8A, "Kurona (0x8A)" }, { 0x8B, "Unknown (0x8B)" }
        };
        public static readonly Dictionary<byte, string> accessoryValues = new Dictionary<byte, string>()
        {
            { 0x6E, "Ribbon (0x6E)" }, { 0x6F, "Tie (0x6F)" }
        };
        public static readonly Dictionary<byte, string> shoes = new Dictionary<byte, string>()
        {
            { 0x93, "Sneakers (0x93)" }, { 0x94, "Loafers (0x94)" }
        };
        public static readonly Dictionary<byte, string> socks = new Dictionary<byte, string>()
        {
            { 0x00, "Short White (0x00)" }, { 0x01, "Short Dark (0x01)" }, { 0x02, "Average White (0x00)" }, { 0x03, "Average Dark (0x03)" }, { 0x04, "Sports socks (0x04)" },
            { 0x05, "Kneehigh White with Black top (0x05)" }, { 0x06, "Kneehigh Black (0x06)" }, { 0x07, "Kneehigh White (0x07)" }, { 0x08, "Kneehigh Pink/White (0x08)" },
            { 0x09, "Kneehigh Purple/White (0x09)" }, { 0x0A, "Kneehigh Black/White (0x0A)" }, { 0x0B, "Leglong White (0x0B)" }, { 0x0C, "Leglong Dark (0x0C)" }
        };
        public static readonly Dictionary<byte, string> hairs = new Dictionary<byte, string>()
        {
            { 0x8E, "Bald (0x8E)" }, { 0x8F, "Short blue (0x8F)" }, { 0x90, "Short red curly (0x90)" }, { 0x91, "Pigtail brown (0x91)" }, { 0x92, "Short blue with ahoge (0x92)" },
            { 0x93, "Medium blue with pigtail (0x93)" }, { 0x94, "Dark grey with pigtail (0x94)" }, { 0x95, "Dark grey with short twintails (0x95)" }, { 0x96, "Medium dark covering right eye (0x96)" },
            { 0x97, "Long dark (0x97)" }, { 0x98, "Blue with twintails (0x98)" }, { 0x99, "Red with twintails (0x99)" }, { 0x9A, "Red with Moon accessory (0x9A)" },
            { 0x9B, "Medium light brown with clips (0x9B)" }, { 0x9C, "Long blue with clips (0x9C)" }, { 0x9D, "Medium pink (0x9D)" }, { 0x9E, "Medium red covering eyes (0x9E)" },
            { 0x9F, "Light Grey with twintails (0x9F)" }, { 0xA0, "Long brown with Tetris clips (0xA0)" }, { 0xA1, "Medium blue (0xA1)" }, { 0xA2, "Short yellow (0xA2)" }, { 0xA3, "Green with pigtail (0xA3)" },
            { 0xA4, "Blue with bunny clips (0xA4)" }, { 0xA5, "Pink with short twintails (0xA5)" }, { 0xA6, "Dark brown with ahoge (0xA6)" }, { 0xA7, "Medium yellow with ribbon (0xA7)" },
            { 0xA8, "Medium blonde (0xA8)" }, { 0xA9, "Medium yellow with pigtail (0xA9)" }, { 0xAA, "Green with right pigtail (0xAA)" }, { 0xAB, "Blue with curly twintails (0xAB)" },
            { 0xAC, "Long purple (0xAC)" }, { 0xAD, "Blue with clef (0xAD)" }, { 0xAE, "Grey with ribbon twintais (0xAE)" }, { 0xAF, "Brown with alice band (0xAF)" },
            { 0xB0, "Black with crosses (Yuko) (0xB0)" }, { 0xB1, "Purple with bells (Shinobu) (0xB1)" }, { 0xB2, "Long curly yellow (Risu) (0xB2)" }, { 0xB3, "Dark with crosses (Maya) (0xB3)" },
            { 0xB4, "Red with devil horns (Kurona) (0xB4)" }, { 0xB5, "Silver with devil clips (Chiru) (0xB5)" }
        };

        public static readonly Dictionary<byte, string> faces = new Dictionary<byte, string>()
        {
            { 0x71, "Face 1 (0x71)" }, { 0x72, "Face 2 (0x72)" },  { 0x73, "Face 3 (0x73)" },  { 0x74, "Face 4 (0x74)" },  { 0x75, "Face 5 (0x75)" },  { 0x76, "Face 6 (0x76)" },  { 0x77, "Face 7 (0x77)" },  { 0x78, "Face 8 (0x78)" },
            { 0x79, "Face 9 (0x79)" }, { 0x7A, "Face 10 (0x7A)" }, { 0x7B, "Face 11 (0x7B)" }, { 0x7C, "Face 12 (0x7C)" }, { 0x7D, "Face 13 (0x7D)" }, { 0x7E, "Face 14 (0x7E)" }, { 0x7F, "Face 15 (0x7F)" }
        };

        public static readonly Dictionary<byte, string> skins = new Dictionary<byte, string>()
        {
            { 0xE8, "Skin 1 (0xE8)" }, { 0xE9, "Skin 2 (0xE9)" },  { 0xEA, "Skin 3 (0xEA)" },  { 0xEB, "Black broken (0xEB)" }
        };

        public static readonly Dictionary<byte, string> grades = new Dictionary<byte, string>()
        {
            { 0x60, "Not at school (0x60)" }, { 0x61, "1st year (0x61)" }, { 0x62, "2nd year (0x62)" },  { 0x63, "3rd year (0x63)" },  { 0x64, "Teacher 1st year (0x64)" },  { 0x65, "Teacher 2nd year (0x65)" },  { 0x66, "Teacher 3rd year (0x66)" }
        };

        public static readonly Dictionary<byte, string> classes = new Dictionary<byte, string>()
        {
            { 0x59, "- (0x59)" }, { 0x5A, "A (0x5A)" }, { 0x5B, "B (0x5B)" }, { 0x5C, "C (0x5C)" }, { 0x5D, "D (0x5D)" }, { 0x5E, "E (0x5E)" }, { 0x5F, "F (0x5F)" }
        };

        public static readonly Dictionary<byte, string> posts = new Dictionary<byte, string>()
        {
            { 0x68, "None (0x68)" }, { 0x69, "Student (0x69)" }, { 0x6A, "Moral guard (0x6A)" }, { 0x6B, "Student officer (0x6B)" }, { 0x6C, "Teacher (0x6C)" }
        };

        public static readonly Dictionary<byte, string> bloodTypes = new Dictionary<byte, string>()
        {
            { 0x34, "A (0x34)" }, { 0x35, "B (0x35)" }, { 0x36, "AB (0x36)" }, { 0x37, "0 (0x37)" }, { 0x38, "- (0x38)" }
        };

        public static readonly Dictionary<byte, string> weakPoints = new Dictionary<byte, string>()
        {
            { 0x54, "Head (0x54)" }, { 0x55, "Breasts (0x55)" }, { 0x56, "Hip (0x56)" }, { 0x57, "Legs (0x57)" }, { 0x58, "None (0x58)" }
        };

        public static readonly Dictionary<byte, string> personalities = new Dictionary<byte, string>()
        {
            { 0x3A, "Cute (0x3A)" }, { 0x3B, "Vital (0x3B)" }, { 0x3C, "Hentai (0x3C)" },  { 0x3D, "Perfect (0x3D)" },  { 0x3E, "Rural Morals Officer (0x3E)" },  { 0x3F, "Shy Morals Officer (0x3F)" },  { 0x40, "Serious Morals officer (0x40)" },
            { 0x41, "Serious Teacher (0x41)" }, { 0x42, "Cool Teacher (0x42)" }, { 0x43, "Hentai Teacher (0x43)" },  { 0x44, "Shinobu (0x44)" },  { 0x45, "Boyish (0x45)" },  { 0x46, "Maya (0x46)" },  { 0x47, "Kurona (0x47)" },
            { 0x48, "Yuko (0x48)" }, { 0x49, "Shy Teacher (0x49)" }, { 0x4A, "Risu (0x4A)" },  { 0x4B, "Ladylike (0x4B)" },  { 0x4C, "Chiru (0x4C)" },  { 0x4D, "Cool (0x4D)" },  { 0x4E, "Shy (0x4E)" },
            { 0x4F, "Aggressive (0x4F)" }, { 0x50, "Mysterious (0x50)" }, { 0x51, "Tsundere (0x51)" },  { 0x52, "Sadistic (0x52)" }
        };



        private BaseEditFields _girlHeightFields;
        private BaseEditFields _girlBustFields;
        private BaseEditFields _girlWaistFields;
        private BaseEditFields _girlHipFields;
        private BaseEditFields _playerParameters;

        private GGVRGirl[] _girls;

        public MainForm()
        {
            InitializeComponent();

            this._edited = false;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            this._girlHeightFields = new BaseEditFields();
            this._girlHeightFields.MarkEdited = this.MarkAsEdited;

            this._girlBustFields = new BaseEditFields();
            this._girlBustFields.MarkEdited = this.MarkAsEdited;

            this._girlWaistFields = new BaseEditFields();
            this._girlWaistFields.MarkEdited = this.MarkAsEdited;

            this._girlHipFields = new BaseEditFields();
            this._girlHipFields.MarkEdited = this.MarkAsEdited;

            this._playerParameters = new BaseEditFields();
            this._playerParameters.MarkEdited = this.MarkAsEdited;

            PopulateComboboxColumn<byte, string>(clmOOutfit, outfitValues);
            PopulateComboboxColumn<byte, string>(clmOAccessory, accessoryValues);
            PopulateComboboxColumn<byte, string>(clmOSocks, socks);
            PopulateComboboxColumn<byte, string>(clmOShoes, shoes);
            PopulateComboboxColumn<byte, string>(clmAHair, hairs);
            PopulateComboboxColumn<byte, string>(clmAFace, faces);
            PopulateComboboxColumn<byte, string>(clmASkin, skins);

            PopulateComboboxColumn<byte, string>(clmYear, grades);
            PopulateComboboxColumn<byte, string>(clmClass, classes);
            PopulateComboboxColumn<byte, string>(clmPost, posts);
            PopulateComboboxColumn<byte, string>(clmBloodType, bloodTypes);

            PopulateComboboxColumn<byte, string>(clmWeakPoint, weakPoints);
            PopulateComboboxColumn<byte, string>(clmNPersonality, personalities);
            PopulateComboboxColumn<byte, string>(clmPPersonality, personalities);
        }

        private bool CheckFiles(string directory, string[] checkedFiles)
        {            
            if(!Directory.Exists(directory))
            {
                return false;
            }

            foreach(string fileCheck in checkedFiles)
            {
                if(!File.Exists(directory + Path.DirectorySeparatorChar + fileCheck))
                {
                    return false;
                }
            }
            return true;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            object picture = Properties.Resources.ResourceManager.GetObject("Shinobu");
            if(picture != null && picture is Bitmap)
            {
                pictureBox1.Image = (Bitmap)picture;
            }            


            LoadSettings();
        }

        private void LoadSettings()
        {
            EditSettings settings = new EditSettings();
            settings.UsePAKFile = false;

            this._settings = settings;

            string dir = settings.GameDirectory;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = dir;
            fbd.Description = "Please select the Gal*Gun VR install folder!";

            bool hasPak = false;
            bool hasUnpacked = false;

            do
            {
                hasPak = this.CheckFiles(dir, packedFilesCheck);
                hasUnpacked = this.CheckFiles(dir, unpackedFilesCheck);

                if (!hasPak && !hasUnpacked)
                {
                    if(fbd.ShowDialog() != DialogResult.OK)
                    {
                        break;
                    }
                    dir = fbd.SelectedPath;
                }
            }
            while (!hasPak && !hasUnpacked);

            if (!hasPak && !hasUnpacked)
            {
                Close();
                return;
            }

            settings.GameDirectory = dir;

            if(!hasPak)
            {
                this.lblPAKFileInfo.Text = "\r\nThe PAK file is already unpacked.\r\n\r\nThis feature is disabled.";
                this.btnUnpackPAK.Enabled = false;
            }
            else
            {
                this.lblPAKFileInfo.Text = "\r\nThe game comes with a big file called GalGunVR-WindowsNoEditor.pak . You can have this file unpacked. This editor will happily function even if all files are unpacked from it.\r\n\r\n" +
                    "Using an unpacked file allows you to edit several settings while the game is running - the game will also load faster so you even have a bit of an advantage.\r\n" +
                    "Unpacking requires 2 GB of disk space.\r\n\r\nDuring the unpacking the game mustn't be running!";
                this.btnUnpackPAK.Enabled = true;
            }

            if (hasPak && hasUnpacked)
            {
                PackedOrUnpacked pou = new PackedOrUnpacked();
                pou.ShowDialog();

                settings.UsePAKFile = (pou.Result == PackedOrUnpacked.POUResult.PAK);
            }
            else if (hasPak)
            {
                settings.UsePAKFile = true;
            }
            else
            {
                settings.UsePAKFile = false;
            }
            settings.SaveSettings();

            if (settings.UsePAKFile)
            {
                this.GetPAKFileMainOffsets();
            }

            this.EnableEdited(false);

            this.ReloadData();
        }

        private void GetPAKFileMainOffsets()
        {
            this._pakOffsets = new Dictionary<string, FileInPakLocation>();

            FileStream fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);

            if(fs.Length < 100)
            {
                MessageBox.Show("Unable to parse PAK file! Invalid length!");
                fs.Close();
                Close();
            }

            br.BaseStream.Seek(-0x2C, SeekOrigin.End);
            uint magic = br.ReadUInt32();
            if(magic != 0x5A6F12E1)
            {
                MessageBox.Show("Unable to parse PAK file! Magic bytes incorrect!");
                fs.Close();
                Close();
            }
            br.ReadUInt32();
            long offset = br.ReadInt64();
            long size = br.ReadInt64();

            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            ReadName(br);

            int fileCount = br.ReadInt32();

            for(int i = 0; i < fileCount; i++)
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

                long bPos = br.BaseStream.Position;
                fOffset += (bPos - pos);

                foreach(string fn in packedFilesRequired)
                {
                    string fnSwapped = fn.Replace(Path.DirectorySeparatorChar, '/');
                    if(fnSwapped == fileName)
                    {
                        this._pakOffsets.Add(fn, new FileInPakLocation(fileName, fOffset, (int)fSize, (int)fSize, hashLocation, enc == 1));
                    }
                }
            }

            br = null;
            fs.Close(); 

            if (this._pakOffsets.Count != packedFilesRequired.Length)
            {
                MessageBox.Show("Wasn't able to find all required files in the PAK file.");
                Close();
            }
        }

        private string ReadName(BinaryReader br)
        {
            int length = br.ReadInt32();
            if(length >= 0) //Ascii
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

        private void ReloadData()
        {
            this.LoadValues();

            this.FillGrid();
        }

        private void MarkAsEdited(object sender, EventArgs e)
        {
            EnableEdited(true);
        }

        private void PopulateComboboxColumn<T, U>(DataGridViewComboBoxColumn dgvcbc, Dictionary<T, U> dictionary)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Keys");
            dataTable.Columns.Add("Values");
            foreach (KeyValuePair<T, U> kvp in dictionary)
            {
                dataTable.Rows.Add(kvp.Key, kvp.Value);
            }
            dgvcbc.DataSource = dataTable;
            dgvcbc.DisplayMember = "Values";
            dgvcbc.ValueMember = "Keys";
            dgvcbc.ValueType = typeof(U);            
        }

        public MainForm(EditSettings settings) : this()
        {
            this._settings = settings;            
        }

        private void LoadAssetFile(string file, out int fileLength, ref FileStream fs, ref BinaryReader br, out long baseOffset, ref byte[] buffer)
        {
            if (this._settings.UsePAKFile)
            {
                if (fs == null)
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    br = new BinaryReader(fs);
                }

                FileInPakLocation fipl = this._pakOffsets[file];
                baseOffset = fipl.FileOffset;
                fileLength = fipl.FileSize;

                fs.Seek(baseOffset, SeekOrigin.Begin);
            }
            else
            {
                if (fs != null)
                {
                    br.Close();
                    br = null;
                    fs = null;
                }

                fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(fs);
                fileLength = (int)fs.Length;
                fs.Seek(0, SeekOrigin.Begin);                
                baseOffset = 0;
            }

            buffer = new byte[fileLength];
            fs.Read(buffer, 0, fileLength);
        }

        private void LoadAssetFileForWrite(string file, ref FileStream fs, ref BinaryWriter bw)
        {
            if (this._settings.UsePAKFile)
            {
                if (fs == null)
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    bw = new BinaryWriter(fs);
                }

                FileInPakLocation fipl = this._pakOffsets[file];
                long baseOffset = fipl.FileOffset;

                fs.Seek(baseOffset, SeekOrigin.Begin);
            }
            else
            {
                if (fs != null)
                {
                    bw.Close();
                    bw = null;
                    fs = null;
                }

                fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                bw = new BinaryWriter(fs);
                
                fs.Seek(0, SeekOrigin.Begin);                
            }
        }

        private void LoadValues()
        {
            //TODO: Handle PAK file
            FileStream fs = null;
            BinaryReader br = null;
            byte[] buffer = null;
            long baseOffset = 0;
            int fileLength = 0;

            LoadAssetFile(FILE_GAL_VISUAL_DATA_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);
            
            
            byte initial = 0x3B;
            List<long> locations;

            byte[] needle;
            int[] offsets = new int[characterNames.Length];

            for (int i = 0; i < characterNames.Length; i++)
            {
                byte searchByte = (byte)(initial + i);
                needle = new byte[]
                {
                    searchByte, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x9C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };

                locations = buffer.IndexesOf(needle);

                if (locations.Count >= 0)
                {
                    offsets[i] = (int)locations[0];
                }
                else
                {
                    offsets[i] = -1;
                }
            }

            List<GGVRGirl> girls = new List<GGVRGirl>();

            cbCharSwap1.Items.Clear();
            cbCharSwap2.Items.Clear();

            for (int i = 0; i < offsets.Length; i++)
            {
                if(offsets[i] > 0)
                {
                    GGVRGirl g = new GGVRGirl();

                    string gPrep = "G" + (i + 1).ToString();

                    g.ID = i + 1;
                    g.Name = characterNames[i];

                    g.BaseID = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i], gPrep, "BaseID");

                    g.Height = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x112, gPrep, "Height");

                    g.HeadSizeRatio = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x12F, gPrep, "HeadSizeRatio");
                    g.Bust = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x14C, gPrep, "Bust");
                    g.Waist = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x169, gPrep, "Waist");
                    g.Hip = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x186, gPrep, "Hip");

                    g.Outfit = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x1DC, gPrep, "Outfit");
                    g.Accessory = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x2D2, gPrep, "Accessory");
                    g.Socks = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x31C, gPrep, "Socks");
                    g.Shoes = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x35E, gPrep, "Shoes");

                    g.Hair = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x21, gPrep, "Hair");
                    g.Face = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x46, gPrep, "Face");
                    g.Skin = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0xF1, gPrep, "Skin");
                    g.EyeColor = this.LoadValueAndOriginal<ColorComparable>(br, baseOffset + offsets[i] + 0x7F, gPrep, "EyeColor");
                    g.EyeBrowColor = this.LoadValueAndOriginal<ColorComparable>(br, baseOffset + offsets[i] + 0xC0, gPrep, "EyeBrowColor");

                    cbCharSwap1.Items.Add(g.Name + " (0x" + g.BaseID.ToString() + ")");
                    cbCharSwap2.Items.Add(g.Name + " (0x" + g.BaseID.ToString() + ")");

                    girls.Add(g);
                }
            }

            LoadAssetFile(FILE_GAL_DATA_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);
            
            initial = 0x04;

            for (int i = 0; i < characterNames.Length; i++)
            {
                byte searchByte = (byte)(initial + i);
                needle = new byte[]
                {
                    searchByte, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x82, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x83, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x88, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };

                locations = buffer.IndexesOf(needle);

                if (locations.Count >= 0)
                {
                    offsets[i] = (int)locations[0];
                }
                else
                {
                    offsets[i] = -1;
                }
            }

            for (int i = 0; i < offsets.Length; i++)
            {
                if (i >= girls.Count)
                {
                    break;
                }

                if (offsets[i] > 0)
                {
                    GGVRGirl g = girls[i];
                    string gPrep = "G" + (i + 1).ToString();

                    g.Year = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x05A, gPrep, "Grade");
                    g.Class = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x83, gPrep, "Class");
                    g.Post = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0xAC, gPrep, "Post");
                    g.BloodType = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x20C, gPrep, "BloodType");


                    g.BirthMonth = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x235, gPrep, "BirthMonth");
                    g.BirthDay = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x257, gPrep, "BirthDay");
                    g.BirthMonth.AssignValueRange(0, 12);
                    g.BirthDay.AssignValueRange(0, 31);

                    g.WeakPoint = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x446, gPrep, "WeakPoint");

                    g.PersonalityNormal = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x13F, gPrep, "NPersonality");
                    g.SpeedNormal = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x160, gPrep, "NSPeed");
                    g.HPNormal = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x2DB, gPrep, "NHP");
                    g.AttackNormal1Strength = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x2F8, gPrep, "NAttack1");
                    g.AttackNormal2Strength = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x315, gPrep, "NAttack2");
                    g.AttackNormal3Strength = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x332, gPrep, "NAttack3");

                    g.PersonalityPossessed = this.LoadValueAndOriginal<byte>(br, baseOffset + offsets[i] + 0x1BE, gPrep, "PPersonality");
                    g.SpeedPossessed = this.LoadValueAndOriginal<float>(br, baseOffset + offsets[i] + 0x1DF, gPrep, "PSPeed");
                    g.HPPossessed = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x3A5, gPrep, "PHP");
                    g.AttackPossessed1Strength = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x3C2, gPrep, "PAttack1");
                    g.AttackPossessed2Strength = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x3DF, gPrep, "PAttack2");
                    g.AttackPossessed3Strength = this.LoadValueAndOriginal<int>(br, baseOffset + offsets[i] + 0x3FC, gPrep, "PAttack3");
                }
            }

            this._girls = girls.ToArray();

            if(this._girls.Length > 0)
            {
                cbCharSwap1.SelectedIndex = 0;
                cbCharSwap2.SelectedIndex = 0;
            }

            LoadAssetFile(FILE_GIRLS_HEIGHT_CURVE_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);            

            needle = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            locations = buffer.IndexesOf(needle);

            if(locations.Count == 2)
            {
                this._girlHeightFields.AddRelation(txtMinCM, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x2C, "GirlHeights", "MinCM"));
                this._girlHeightFields.AddRelation(txtMinScale, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x30, "GirlHeights", "MinScale"));
                this._girlHeightFields.AddRelation(txtNormCM, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x47, "GirlHeights", "NormCM"));
                this._girlHeightFields.AddRelation(txtNormScale, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x4B, "GirlHeights", "NormScale"));
                this._girlHeightFields.AddRelation(txtMaxCM, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x62, "GirlHeights", "MaxCM"));
                this._girlHeightFields.AddRelation(txtMaxScale, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x66, "GirlHeights", "MaxScale"));

                this._girlHeightFields.AddRelation(txtMinHSRV1, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x10C, "GirlHeights", "MinHSRV1"));
                this._girlHeightFields.AddRelation(txtMinHSRV2, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x110, "GirlHeights", "MinHSRV2"));
                this._girlHeightFields.AddRelation(txtNormHSRV1, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x127, "GirlHeights", "NormHSRV1"));
                this._girlHeightFields.AddRelation(txtNormHSRV2, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x12B, "GirlHeights", "NormHSRV2"));
                this._girlHeightFields.AddRelation(txtMaxHSRV1, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x142, "GirlHeights", "MaxHSRV1"));
                this._girlHeightFields.AddRelation(txtMaxHSRV2, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x146, "GirlHeights", "MaxHSRV1"));
            }

            LoadAssetFile(FILE_GIRLS_BUST_CURVE_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);
            needle = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            locations = buffer.IndexesOf(needle);

            if (locations.Count == 3)
            {
                this._girlBustFields.AddRelation(txtMinBustCM, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x2C, "GirlBusts", "MinCM"));
                this._girlBustFields.AddRelation(txtMinBustScale, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x30, "GirlBusts", "MinScale"));
                this._girlBustFields.AddRelation(txtNormBustCM, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x47, "GirlBusts", "NormCM"));
                this._girlBustFields.AddRelation(txtNormBustScale, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x4B, "GirlBusts", "NormScale"));
                this._girlBustFields.AddRelation(txtMaxBustCM, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x62, "GirlBusts", "MaxCM"));
                this._girlBustFields.AddRelation(txtMaxBustScale, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x66, "GirlBusts", "MaxScale"));

                this._girlBustFields.AddRelation(txtMinBustCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x2C, "GirlBusts", "MinCMY"));
                this._girlBustFields.AddRelation(txtMinBustScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x30, "GirlBusts", "MinScaleY"));
                this._girlBustFields.AddRelation(txtNormBustCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x47, "GirlBusts", "NormCMY"));
                this._girlBustFields.AddRelation(txtNormBustScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x4B, "GirlBusts", "NormScaleY"));
                this._girlBustFields.AddRelation(txtMaxBustCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x62, "GirlBusts", "MaxCMY"));
                this._girlBustFields.AddRelation(txtMaxBustScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x66, "GirlBusts", "MaxScaleY"));

                this._girlBustFields.AddRelation(txtMinBustCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x2C, "GirlBusts", "MinCMZ"));
                this._girlBustFields.AddRelation(txtMinBustScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x30, "GirlBusts", "MinScaleZ"));
                this._girlBustFields.AddRelation(txtNormBustCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x47, "GirlBusts", "NormCMZ"));
                this._girlBustFields.AddRelation(txtNormBustScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x4B, "GirlBusts", "NormScaleZ"));
                this._girlBustFields.AddRelation(txtMaxBustCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x62, "GirlBusts", "MaxCMZ"));
                this._girlBustFields.AddRelation(txtMaxBustScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x66, "GirlBusts", "MaxScaleZ"));
            }

            LoadAssetFile(FILE_GIRLS_WAIST_CURVE_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);
            needle = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            locations = buffer.IndexesOf(needle);

            if (locations.Count == 2)
            {
                this._girlWaistFields.AddRelation(txtMinWaistCMX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x2C, "GirlWaists", "MinCM"));
                this._girlWaistFields.AddRelation(txtMinWaistScaleX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x30, "GirlWaists", "MinScale"));
                this._girlWaistFields.AddRelation(txtNormWaistCMX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x47, "GirlWaists", "NormCM"));
                this._girlWaistFields.AddRelation(txtNormWaistScaleX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x4B, "GirlWaists", "NormScale"));
                this._girlWaistFields.AddRelation(txtMaxWaistCMX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x62, "GirlWaists", "MaxCM"));
                this._girlWaistFields.AddRelation(txtMaxWaistScaleX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x66, "GirlWaists", "MaxScale"));

                this._girlWaistFields.AddRelation(txtMinWaistCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x10C, "GirlWaists", "MinCMY"));
                this._girlWaistFields.AddRelation(txtMinWaistScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x110, "GirlWaists", "MinScaleY"));
                this._girlWaistFields.AddRelation(txtMaxWaistCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x127, "GirlWaists", "MaxCMY"));
                this._girlWaistFields.AddRelation(txtMaxWaistScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x12B, "GirlWaists", "MaxScaleY"));

                this._girlWaistFields.AddRelation(txtMinWaistCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x2C, "GirlWaists", "MinCMZ"));
                this._girlWaistFields.AddRelation(txtMinWaistScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x30, "GirlWaists", "MinScaleZ"));
                this._girlWaistFields.AddRelation(txtNormWaistCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x47, "GirlWaists", "NormCMZ"));
                this._girlWaistFields.AddRelation(txtNormWaistScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x4B, "GirlWaists", "NormScaleZ"));
                this._girlWaistFields.AddRelation(txtMaxWaistCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x62, "GirlWaists", "MaxCMZ"));
                this._girlWaistFields.AddRelation(txtMaxWaistScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x66, "GirlWaists", "MaxScaleZ"));
            }

            LoadAssetFile(FILE_GIRLS_HIP_CURVE_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);
            needle = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            locations = buffer.IndexesOf(needle);

            if (locations.Count == 3)
            {
                this._girlHipFields.AddRelation(txtMinHipCMX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x2C, "GirlHips", "MinCM"));
                this._girlHipFields.AddRelation(txtMinHipScaleX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x30, "GirlHips", "MinScale"));
                this._girlHipFields.AddRelation(txtNormHipCMX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x47, "GirlHips", "NormCM"));
                this._girlHipFields.AddRelation(txtNormHipScaleX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x4B, "GirlHips", "NormScale"));
                this._girlHipFields.AddRelation(txtMaxHipCMX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x62, "GirlHips", "MaxCM"));
                this._girlHipFields.AddRelation(txtMaxHipScaleX, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x66, "GirlHips", "MaxScale"));

                this._girlHipFields.AddRelation(txtMinHipCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x2C, "GirlHips", "MinCMY"));
                this._girlHipFields.AddRelation(txtMinHipScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x30, "GirlHips", "MinScaleY"));
                this._girlHipFields.AddRelation(txtNormHipCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x47, "GirlHips", "NormCMY"));
                this._girlHipFields.AddRelation(txtNormHipScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x4B, "GirlHips", "NormScaleY"));
                this._girlHipFields.AddRelation(txtMaxHipCMY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x62, "GirlHips", "MaxCMY"));
                this._girlHipFields.AddRelation(txtMaxHipScaleY, this.LoadValueAndOriginal<float>(br, baseOffset + locations[1] + 0x66, "GirlHips", "MaxScaleY"));

                this._girlHipFields.AddRelation(txtMinHipCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x2C, "GirlHips", "MinCMZ"));
                this._girlHipFields.AddRelation(txtMinHipScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x30, "GirlHips", "MinScaleZ"));
                this._girlHipFields.AddRelation(txtNormHipCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x47, "GirlHips", "NormCMZ"));
                this._girlHipFields.AddRelation(txtNormHipScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x4B, "GirlHips", "NormScaleZ"));
                this._girlHipFields.AddRelation(txtMaxHipCMZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x62, "GirlHips", "MaxCMZ"));
                this._girlHipFields.AddRelation(txtMaxHipScaleZ, this.LoadValueAndOriginal<float>(br, baseOffset + locations[2] + 0x66, "GirlHips", "MaxScaleZ"));
            }




            LoadAssetFile(FILE_PLAYER_PARAMETERS_UASSET, out fileLength, ref fs, ref br, out baseOffset, ref buffer);

            needle = new byte[] { 0x00, 0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            locations = buffer.IndexesOf(needle);

            if (locations.Count == 1)
            {
                this._playerParameters.AddRelation(txtInitialHealth, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x1C, "PlayerParams", "Health"));
                this._playerParameters.AddRelation(txtDepletionRate, this.LoadValueAndOriginal<float>(br, baseOffset + locations[0] + 0x73, "PlayerParams", "Depletion"));                
            }

            br = null;
            fs.Close();            
        }

        private GGVRDataType<T> LoadValueAndOriginal<T>(BinaryReader br, long address, string prepend, string name) where T : IComparable
        {
            GGVRDataType<T> res = new GGVRDataType<T>(br, address);
            res.AssignOriginalValue(this._settings.GetDefaultValue<T>(prepend + name, res.Value));
            return res;
        }

        private void FillGrid()
        {
            List<string> displayNames = new List<string>();
            
            for(int i = 0; i < this._girls.Length; i++)
            {
                GGVRGirl girl = this._girls[i];

                string name = girl.Name;                

                GGVRBaseDataType[] values;

                values = new GGVRBaseDataType[] { girl.Height, girl.HeadSizeRatio, girl.Bust, girl.Waist, girl.Hip };
                FillToDataGrid(dgvDataFixed, girl.ID, name, girl.BaseID.Address, values);

                values = new GGVRBaseDataType[] { girl.Outfit, girl.Accessory, girl.Socks, girl.Shoes };
                FillToDataGrid(dgvOutfit, girl.ID, name, girl.BaseID.Address, values);

                values = new GGVRBaseDataType[] { girl.Hair, girl.Face, girl.Skin, girl.EyeColor, girl.EyeBrowColor };
                FillToDataGrid(dgvAppearance, girl.ID, name, girl.BaseID.Address, values);

                values = new GGVRBaseDataType[] { girl.WeakPoint, girl.PersonalityNormal, girl.SpeedNormal, girl.HPNormal, girl.PersonalityPossessed, girl.SpeedPossessed, girl.HPPossessed };
                FillToDataGrid(dgvShootingParameters, girl.ID, name, girl.Year.Address - 0x5A, values);

                values = new GGVRBaseDataType[] { girl.Year, girl.Class, girl.Post, girl.BloodType, girl.BirthMonth, girl.BirthDay };
                FillToDataGrid(dgvCosmetic, girl.ID, name, girl.Year.Address - 0x5A, values);
            }
        }

        private void FillToDataGrid(DataGridView dgv, int girldId, string name, long baseAddress, GGVRBaseDataType[] values)
        {
            bool found = false;

            List<object> objectValues = new List<object>();
            objectValues.Add(String.Format("{0:00}", girldId));
            objectValues.Add(name);
            objectValues.Add(baseAddress.ToString("X").ToUpperInvariant());
            for (int i = 0; i < values.Length; i++)
            {
                DataGridViewColumn clmn = dgv.Columns[i + 3];
                if(clmn is DataGridViewTextBoxColumn)
                {
                    objectValues.Add(values[i].ToString());
                }
                else
                {
                    objectValues.Add(values[i].GetValue());
                }
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Tag != null && row.Tag is int)
                {
                    int index = (int)row.Tag;
                    if (index == girldId)
                    {
                        FillRow(row, objectValues.ToArray());
                        
                        found = true;
                        row.Cells[0].ReadOnly = true;
                        row.Cells[1].ReadOnly = true;
                        row.Cells[2].ReadOnly = true;
                        for (int i = 0; i < values.Length; i++)
                        {
                            row.Cells[i + 3].Tag = values[i];
                        }
                    }
                }
            }

            if (!found)
            {
                int rowNumber = dgv.Rows.Add();
                DataGridViewRow row = dgv.Rows[rowNumber];                

                FillRow(row, objectValues.ToArray());

                row.Tag = girldId;
                row.Cells[0].ReadOnly = true;
                row.Cells[1].ReadOnly = true;
                row.Cells[2].ReadOnly = true;
                for (int i = 0; i < values.Length; i++)
                {
                    row.Cells[i + 3].Tag = values[i];
                }
            }
        }

        private void FillRow(DataGridViewRow row, object[] objectValues)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                DataGridViewCell dgc = row.Cells[i];
                AssignCell(dgc, objectValues[i]);
            }
        }

        private void AssignCell(DataGridViewCell dgc, object value)
        {
            if (dgc is DataGridViewComboBoxCell)
            {
                DataGridViewComboBoxCell dgvcbc = (DataGridViewComboBoxCell)dgc;
                object o = null;

                foreach (DataRowView drw in dgvcbc.Items)
                {
                    object oTmp = drw.Row[0];
                    object oCmp = oTmp;
                    if (oCmp is string)
                    {
                        if (value is byte)
                        {
                            byte iTmp = 0;
                            if (byte.TryParse((string)oCmp, out iTmp))
                            {
                                oCmp = iTmp;
                            }
                        }
                        else if (value is int)
                        {
                            int iTmp = 0;
                            if (int.TryParse((string)oCmp, out iTmp))
                            {
                                oCmp = iTmp;
                            }
                        }
                    }
                    if (oCmp.Equals(value))
                    {
                        o = oTmp;
                        break;
                    }
                }
                /*string v = ((dgvcbc.OwningColumn as DataGridViewComboBoxColumn).Items[0] as DataRowView).Row[1].ToString();
                object o = (object)((dgvcbc.OwningColumn as DataGridViewComboBoxColumn).Items[0] as DataRowView).Row[0];*/

                dgvcbc.Value = o;
            }
            else
            {
                dgc.Value = value;
            }
        }

        private void AssignCellFromDataType(DataGridViewCell cell, GGVRBaseDataType dt)
        {
            object v = dt.ToString();
            if(cell is DataGridViewComboBoxCell)
            {
                v = dt.GetValue();
            }
            //cell.Value = dt.ToString();
            AssignCell(cell, v);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Focus();
            if(SaveFile())
            {
                MessageBox.Show("Changes saved.");
            }            
        }

        public bool SaveFile()
        {
            FileStream fs = null;
            BinaryWriter bw = null;

            try
            {
                LoadAssetFileForWrite(FILE_GAL_VISUAL_DATA_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }


            foreach (GGVRGirl girl in this._girls)
            {
                girl.WriteAll(bw);
            }

            try
            {
                LoadAssetFileForWrite(FILE_GAL_DATA_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }


            foreach (GGVRGirl girl in this._girls)
            {
                girl.WriteAllGalData(bw);
            }

            try
            {
                LoadAssetFileForWrite(FILE_GIRLS_HEIGHT_CURVE_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }

            this._girlHeightFields.WriteAll(bw);

            try
            {
                LoadAssetFileForWrite(FILE_GIRLS_BUST_CURVE_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }

            this._girlBustFields.WriteAll(bw);


            try
            {
                LoadAssetFileForWrite(FILE_GIRLS_WAIST_CURVE_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }

            this._girlWaistFields.WriteAll(bw);

            try
            {
                LoadAssetFileForWrite(FILE_GIRLS_HIP_CURVE_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }

            this._girlHipFields.WriteAll(bw);

            try
            {
                LoadAssetFileForWrite(FILE_PLAYER_PARAMETERS_UASSET, ref fs, ref bw);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the output file for writing. Make sure that Gal*Gun VR isn't currently running! Exception code: " + ex.Message);
                return false;
            }


            this._playerParameters.WriteAll(bw);

            /*if(this._settings.UsePAKFile) //Calculate hashes
            {
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                foreach(KeyValuePair<string, FileInPakLocation> kvp in this._pakOffsets)
                {
                    FileInPakLocation fipl = kvp.Value;
                    fs.Seek(fipl.FileOffset, SeekOrigin.Begin);
                    byte[] buffer = new byte[fipl.FileSize];
                    fs.Read(buffer, 0, fipl.FileSize);
                    byte[] hash = sha1.ComputeHash(buffer);
                    fs.Seek(fipl.HashOffset, SeekOrigin.Begin);
                    fs.Write(hash, 0, 20);
                }
                sha1 = null;
            }*/

            bw = null;
            fs.Close();
            fs = null;

            EnableEdited(false);

            return true;
        }


        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.CurrentCell == null)
            {
                return;
            }
            if (e.ColumnIndex < 2)
            {
                dgv.EndEdit();
                return;
            }

            object tag = dgv.CurrentCell.Tag;
            if (!(tag is GGVRBaseDataType))
            {
                dgv.EndEdit();
                return;
            }
            GGVRBaseDataType cct = (GGVRBaseDataType)tag;
            if (!cct.Enabled)
            {
                dgv.EndEdit();
                return;
            }

            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgv.CurrentCell = dgv.CurrentCell;
            dgv.BeginEdit(true);
        }

        private void dgvAppearance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.CurrentCell == null)
            {
                return;
            }

            object tag = dgv.CurrentCell.Tag;
            if (!(tag is GGVRBaseDataType))
            {
                dgv.EndEdit();
                return;
            }
            GGVRBaseDataType cct = (GGVRBaseDataType)tag;
            if (!cct.Enabled)
            {
                dgv.EndEdit();
                return;
            }

            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && cct.ValueType == typeof(ColorComparable))
            {
                ColorComparable cc = (ColorComparable)cct.GetValue();
                dlgColor.Color = cc.Color;
                if(dlgColor.ShowDialog() != DialogResult.Cancel)
                {
                    cc.Color = dlgColor.Color;
                    EnableEdited(true);
                    dgv.CurrentCell.Value = cct.ToString();
                }
            }
        }

        private void dgvData_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

            object tag = cell.Tag;
            if (!(tag is GGVRBaseDataType))
            {
                return;
            }

            GGVRBaseDataType dt = (GGVRBaseDataType)tag;

            string inputValue = "";
            if (cell.Value != null)
            {
                inputValue = cell.Value.ToString();
            }
            
            if (dgv.EditingControl != null)
            {
                if(dgv.EditingControl is ComboBox)
                {
                    inputValue = ((cell as DataGridViewComboBoxCell).Items[(dgv.EditingControl as ComboBox).SelectedIndex] as DataRowView).Row[0].ToString();
                    if (dt.ValueType == typeof(byte))
                    {
                        try
                        {
                            inputValue = byte.Parse(inputValue).ToString("X").ToUpperInvariant();
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                else
                {
                    inputValue = dgv.EditingControl.Text.ToString();
                }                
            }            

            bool edited = dt.SetNewValueFromString(inputValue);

            if (edited)
            {
                EnableEdited(true);
            }

            AssignCellFromDataType(cell, dt);            
        }

        public void EnableEdited(bool edited)
        {
            _edited = edited;
            btnSave.Enabled = edited;

            if (edited)
            {
                DataGridView[] dgvs = new DataGridView[] { dgvDataFixed };

                foreach (DataGridView dgv in dgvs)
                {
                    if (dgv.SortedColumn != null)
                    {
                        DataGridViewColumn col = dgv.SortedColumn;

                        if (col.Index >= 2)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                            col.SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                    }
                }
            }
        }

        private void dgvData_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (!(sender is DataGridView))
            {
                return;
            }

            e.Handled = true;

            DataGridView dgv = (DataGridView)sender;

            if (dgv.EditingControl != null)
            {
                dgv.CancelEdit();
            }

            int columnIndex = e.Column.Index;

            if (columnIndex == 0) //Sort by ID
            {
                e.SortResult = (int.Parse((string)e.CellValue1)).CompareTo(int.Parse((string)e.CellValue2));
                return;
            }
            if (columnIndex == 1) //Sort by Name
            {
                e.SortResult = ((string)(e.CellValue1)).CompareTo((string)(e.CellValue2));
                return;
            }
            if (columnIndex == 2) //Sort by address
            {
                e.SortResult = (int.Parse((string)e.CellValue1, System.Globalization.NumberStyles.HexNumber)).CompareTo(int.Parse((string)e.CellValue2, System.Globalization.NumberStyles.HexNumber));               
                return;
            }

            bool sortAddress = false;
            if (columnIndex == 1)
            {
                sortAddress = true;
            }

            DataGridViewCell cell1 = dgv.Rows[e.RowIndex1].Cells[columnIndex];
            DataGridViewCell cell2 = dgv.Rows[e.RowIndex2].Cells[columnIndex];

            object tag1 = cell1.Tag;
            object tag2 = cell2.Tag;
            if (!(tag1 is GGVRBaseDataType) || !(tag2 is GGVRBaseDataType))
            {
                e.SortResult = 0;
                return;
            }

            GGVRBaseDataType girl1Type = (GGVRBaseDataType)tag1;
            GGVRBaseDataType girl2Type = (GGVRBaseDataType)tag2;

            if (sortAddress)
            {
                e.SortResult = girl1Type.Address.CompareTo(girl2Type.Address);
            }
            else
            {
                e.SortResult = girl1Type.GetValue().CompareTo(girl2Type.GetValue());
            }
        }

        private void dgvData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                _selectedControl = e.Control;
                e.Control.ContextMenuStrip = cellContextMenu;
            }
            else
            {
                _selectedControl = null;
            }
        }

        private void tsRestoreOriginal_Click(object sender, EventArgs e)
        {
            if (_selectedControl is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl c = (DataGridViewTextBoxEditingControl)_selectedControl;

                DataGridView dgv = c.EditingControlDataGridView;

                DataGridViewCell cell = c.EditingControlDataGridView.CurrentCell;
                object tag = cell.Tag;

                if (!(cell.Tag is GGVRBaseDataType))
                {
                    return;
                }

                GGVRBaseDataType dt = (GGVRBaseDataType)tag;
                if (!dt.Enabled)
                {
                    return;
                }

                dt.RestoreOriginal();
                string nv = dt.ToString();
                cell.Value = nv;

                if (dgv.EditingControl != null)
                {
                    dgv.EditingControl.Text = nv;
                }

                this.EnableEdited(true);
            }
        }

        private void tsCopy_Click(object sender, EventArgs e)
        {
            if (_selectedControl is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl c = (DataGridViewTextBoxEditingControl)_selectedControl;
                Clipboard.SetText(c.Text);
            }
        }

        private void tsPaste_Click(object sender, EventArgs e)
        {
            if (_selectedControl is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl c = (DataGridViewTextBoxEditingControl)_selectedControl;
                c.Text = Clipboard.GetText();
            }
        }


        private void tsShowFieldAddress_Click(object sender, EventArgs e)
        {
            if (_selectedControl is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl c = (DataGridViewTextBoxEditingControl)_selectedControl;

                DataGridView dgv = c.EditingControlDataGridView;

                DataGridViewCell cell = c.EditingControlDataGridView.CurrentCell;
                object tag = cell.Tag;

                if (!(cell.Tag is GGVRBaseDataType))
                {
                    return;
                }

                GGVRBaseDataType dt = (GGVRBaseDataType)tag;
                if (!dt.Enabled)
                {
                    return;
                }

                string address = dt.Address.ToString("X");
                Clipboard.SetText(address);
                MessageBox.Show("Address: " + address + "\r\n\r\n" + "Copied to clipboard.");
            }
        }

        private void tsMainShowAddresses_Click(object sender, EventArgs e)
        {
            clmAddress.Visible = tsMainShowAddresses.Checked;
            clmOAddress.Visible = tsMainShowAddresses.Checked;
            clmAAddress.Visible = tsMainShowAddresses.Checked;
            clmSPGirlAddress.Visible = tsMainShowAddresses.Checked;
            clmCDGirlAddress.Visible = tsMainShowAddresses.Checked;
        }

        private void dgvAppearance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

            object tag = cell.Tag;
            if (!(tag is GGVRBaseDataType))
            {
                return;
            }
            GGVRBaseDataType cct = (GGVRBaseDataType)tag;

            if ((cell is DataGridViewButtonCell) && e.RowIndex >= 0 && cct.ValueType == typeof(ColorComparable))
            {
                ColorComparable cc = (ColorComparable)cct.GetValue();
                Color c = cc.Color;

                e.CellStyle.BackColor = cc.Color;

                if(c.R + c.B + c.G < 375)
                {
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Black;
                }                
            }
        }

        private void btnMaxScales_Click(object sender, EventArgs e)
        {
            this._girlHeightFields.SetTextBoxValue(this.txtMinCM, (0.0f).ToString());
            this._girlHeightFields.SetTextBoxValue(this.txtMinScale, (0.0f).ToString());
            this._girlHeightFields.SetTextBoxValue(this.txtNormCM, (156.0f).ToString());
            this._girlHeightFields.SetTextBoxValue(this.txtNormScale, (1.0f).ToString());
            this._girlHeightFields.SetTextBoxValue(this.txtMaxCM, (1560.0f).ToString());
            this._girlHeightFields.SetTextBoxValue(this.txtMaxScale, (10.0f).ToString());
        }

        private void btnMaximiseBust_Click(object sender, EventArgs e)
        {
            this._girlBustFields.SetTextBoxValue(this.txtMinBustCM, (0.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMinBustScale, (0.3f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtNormBustCM, (83.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtNormBustScale, (1.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMaxBustCM, (668.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMaxBustScale, (10.0f).ToString());            

            this._girlBustFields.SetTextBoxValue(this.txtMinBustCMY, (0.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMinBustScaleY, (0.3f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtNormBustCMY, (83.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtNormBustScaleY, (1.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMaxBustCMY, (668.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMaxBustScaleY, (5.5f).ToString());

            this._girlBustFields.SetTextBoxValue(this.txtMinBustCMZ, (38.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMinBustScaleZ, (0.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtNormBustCMZ, (83.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtNormBustScaleZ, (1.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMaxBustCMZ, (668.0f).ToString());
            this._girlBustFields.SetTextBoxValue(this.txtMaxBustScaleZ, (3.25f).ToString());
        }

        private void btnGodMode_Click(object sender, EventArgs e)
        {
            this._playerParameters.SetTextBoxValue(this.txtInitialHealth, "1000000000");
            this._playerParameters.SetTextBoxValue(this.txtDepletionRate, (0.0f).ToString());
        }

        private void tsiTBRestoreOriginal_Click(object sender, EventArgs e)
        {
            if(txtBoxContextMenu.SourceControl is TextBox)
            {
                TextBox tb = (TextBox)txtBoxContextMenu.SourceControl;
                object tag = tb.Tag;

                if(!(tag is GGVRBaseDataType))
                {
                    return;
                }
                GGVRBaseDataType ddt = (GGVRBaseDataType)tag;

                ddt.RestoreOriginal();
                tb.Text = ddt.ToString();                
            }
        }

        private void tsiTBCopy_Click(object sender, EventArgs e)
        {
            if (txtBoxContextMenu.SourceControl is TextBox)
            {
                TextBox tb = (TextBox)txtBoxContextMenu.SourceControl;
                object tag = tb.Tag;

                if (!(tag is GGVRBaseDataType))
                {
                    return;
                }
                Clipboard.SetText(tb.Text);
            }
        }

        private void tsiTBPaste_Click(object sender, EventArgs e)
        {
            if (txtBoxContextMenu.SourceControl is TextBox)
            {
                TextBox tb = (TextBox)txtBoxContextMenu.SourceControl;
                object tag = tb.Tag;

                if (!(tag is GGVRBaseDataType))
                {
                    return;
                }
                tb.Text = Clipboard.GetText();
                tb.Focus();
            }
        }

        private void tsiTBShowAddress_Click(object sender, EventArgs e)
        {
            if (txtBoxContextMenu.SourceControl is TextBox)
            {
                TextBox tb = (TextBox)txtBoxContextMenu.SourceControl;
                object tag = tb.Tag;

                if (!(tag is GGVRBaseDataType))
                {
                    return;
                }
                GGVRBaseDataType ddt = (GGVRBaseDataType)tag;

                string address = ddt.Address.ToString("X");
                Clipboard.SetText(address);
                MessageBox.Show("Address: " + address + "\r\n\r\n" + "Copied to clipboard.");
            }
            
        }

        private void btnUnpackPAK_Click(object sender, EventArgs e)
        {
            if(this._edited)
            {
                if (MessageBox.Show("There are unsaved changes. If you continue they're lost. Is this okay?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if(File.Exists(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE))
            {
                if(MessageBox.Show("This process cannot be cancelled. Gal*Gun mustn't be running. Are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                UnpackProgress up = new UnpackProgress();
                up.PakFile = this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE;
                up.SigFile = this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_SIG_FILE;
                up.UnpackFolder = this._settings.GameDirectory;

                up.ShowDialog();

                up = null;

                LoadSettings();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._edited)
            {
                if (MessageBox.Show("There are unsaved changes. If you continue they're lost. Is this okay?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnSwapParameters_Click(object sender, EventArgs e)
        {
            if (cbCharSwap1.SelectedIndex == cbCharSwap2.SelectedIndex)
            {
                MessageBox.Show("Please choose two different characters.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GGVRGirl girl1 = this._girls[cbCharSwap1.SelectedIndex];
            GGVRGirl girl2 = this._girls[cbCharSwap2.SelectedIndex];

            girl1.SwapWith(girl2);

            this.EnableEdited(true);

            MessageBox.Show("Character parameters were swapped!");

            this.FillGrid();
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            if(cbCharSwap1.SelectedIndex == cbCharSwap2.SelectedIndex)
            {
                MessageBox.Show("Please choose two different characters.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;                
            }

            if (this._edited)
            {
                if (MessageBox.Show("There are unsaved changes. If you continue they're lost. Is this okay?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            GGVRGirl girl1 = this._girls[cbCharSwap1.SelectedIndex];
            GGVRGirl girl2 = this._girls[cbCharSwap2.SelectedIndex];
            byte girlIndex1 = girl1.BaseID.Value;
            byte girlIndex2 = girl2.BaseID.Value;

            FileStream fs = null;
            BinaryWriter bw = null;

            if (this._settings.UsePAKFile)
            {
                try
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    bw = new BinaryWriter(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open the PAK file for writing. Make sure that Gal*Gun VR isn't currently running!");
                    return;
                }
            }
            else
            {
                try
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_GAL_VISUAL_DATA_UASSET, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    bw = new BinaryWriter(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open the Visual Data asset file for writing. Make sure that Gal*Gun VR isn't currently in a loading sequence (in the main menu the file can be safely edited)!");
                    return;
                }
            }

            girl1.BaseID.Value = girlIndex2;
            girl2.BaseID.Value = girlIndex1;

            girl1.BaseID.WriteToFile(bw);
            girl2.BaseID.WriteToFile(bw);

            bw.Close();
            bw = null;
            fs = null;

            MessageBox.Show("Characters were successfully swapped!");

            LoadSettings();
        }

        private void tsMainRestoreAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you wish to restore all values to their respective original values?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            foreach (GGVRGirl g in this._girls)
            {
                g.RestoreAll();
            }
            this._playerParameters.RestoreAll();
            this._girlHeightFields.RestoreAll();
            this._girlBustFields.RestoreAll();
            this._girlWaistFields.RestoreAll();
            this._girlHipFields.RestoreAll();

            this.FillGrid();
            this.EnableEdited(true);
        }        
    }
}
