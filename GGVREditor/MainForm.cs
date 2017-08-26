﻿using System;
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

        private const string FILE_GAL_VISUAL_DATA_UASSET = @"GalGunVR\Content\VRGG\DataTable\GalData\GalVisualDatas.uasset";
        private const string FILE_GIRLS_HEIGHT_CURVE_UASSET = @"GalGunVR\Content\VRGG\AI\GAL\ChangeBodySize\GirlsHeightCurve.uasset";
        private const string FILE_PLAYER_PARAMETERS_UASSET = @"GalGunVR\Content\VRGG\DataTable\Shooting\PlayerParameters.uasset";

        private const string FILE_PAK_FILE = @"GalGunVR\Content\Paks\GalGunVR-WindowsNoEditor.pak";
        private const string FILE_SIG_FILE = @"GalGunVR\Content\Paks\GalGunVR-WindowsNoEditor.sig";

        private static readonly string[] unpackedFilesCheck = new string[]
        {
            FILE_GAL_VISUAL_DATA_UASSET, FILE_GIRLS_HEIGHT_CURVE_UASSET, FILE_PLAYER_PARAMETERS_UASSET
        };

        private static readonly string[] packedFilesCheck = new string[]
        {
            FILE_PAK_FILE
        };

        private static readonly string[] packedFilesRequired = new string[]
        {
            FILE_GAL_VISUAL_DATA_UASSET, FILE_GIRLS_HEIGHT_CURVE_UASSET, FILE_PLAYER_PARAMETERS_UASSET
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
            { 0x87, "2nd grade (0x87)" }, { 0x88, "3rd grade (0x88)" }, { 0x89, "Teacher (0x89)" }, { 0x8A, "Kurona (0x8A)" }
        };
        public static readonly Dictionary<byte, string> accessoryValues = new Dictionary<byte, string>()
        {
            { 0x6E, "Ribbon (0x6E)" }, { 0x6F, "Tie (0x6F)" }
        };
        public static readonly Dictionary<byte, string> shoes = new Dictionary<byte, string>()
        {
            { 0x93, "Sneakers (0x93)" }, { 0x94, "Formal (0x94)" }
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

        private BaseEditFields _girlHeightFields;
        private BaseEditFields _playerParameters;

        private GGVRGirl[] _girls;

        public MainForm()
        {
            InitializeComponent();

            this._edited = false;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            this._girlHeightFields = new BaseEditFields();
            this._girlHeightFields.MarkEdited = this.MarkAsEdited;

            this._playerParameters = new BaseEditFields();
            this._playerParameters.MarkEdited = this.MarkAsEdited;

            PopulateComboboxColumn<byte, string>(clmOOutfit, outfitValues);
            PopulateComboboxColumn<byte, string>(clmOAccessory, accessoryValues);
            PopulateComboboxColumn<byte, string>(clmOSocks, socks);
            PopulateComboboxColumn<byte, string>(clmOShoes, shoes);
            PopulateComboboxColumn<byte, string>(clmAHair, hairs);
            PopulateComboboxColumn<byte, string>(clmAFace, faces);
            PopulateComboboxColumn<byte, string>(clmASkin, skins);
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

        private void LoadValues()
        {
            //TODO: Handle PAK file
            FileStream fs = null;
            BinaryReader br = null;
            byte[] buffer = null;
            FileInPakLocation fipl = null;
            long baseOffset = 0;

            if(this._settings.UsePAKFile)
            {
                fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(fs);
            }

            if (!this._settings.UsePAKFile)
            {
                fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_GAL_VISUAL_DATA_UASSET, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(fs);
                buffer = new byte[(int)fs.Length];
                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)fs.Length);
            }
            else
            {
                fipl = this._pakOffsets[FILE_GAL_VISUAL_DATA_UASSET];
                baseOffset = fipl.FileOffset;
                buffer = new byte[fipl.FileSize];
                fs.Seek(baseOffset, SeekOrigin.Begin);
                fs.Read(buffer, 0, fipl.FileSize);
            }
            
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

            this._girls = girls.ToArray();

            if(this._girls.Length > 0)
            {
                cbCharSwap1.SelectedIndex = 0;
                cbCharSwap2.SelectedIndex = 0;
            }

            if (!this._settings.UsePAKFile)
            {
                br = null;
                fs.Close();

                fs = new FileStream(this._settings.GameDirectory + @"\GalGunVR\Content\VRGG\AI\GAL\ChangeBodySize\GirlsHeightCurve.uasset", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(fs);
                buffer = new byte[(int)fs.Length];
                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)fs.Length);
            }
            else
            {
                fipl = this._pakOffsets[FILE_GIRLS_HEIGHT_CURVE_UASSET];
                baseOffset = fipl.FileOffset;
                buffer = new byte[fipl.FileSize];
                fs.Seek(baseOffset, SeekOrigin.Begin);
                fs.Read(buffer, 0, fipl.FileSize);
            }

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
            }

            if (!this._settings.UsePAKFile)
            {
                br = null;
                fs.Close();

                fs = new FileStream(this._settings.GameDirectory + @"\GalGunVR\Content\VRGG\DataTable\Shooting\PlayerParameters.uasset", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(fs);
                buffer = new byte[(int)fs.Length];
                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)fs.Length);
            }
            else
            {
                fipl = this._pakOffsets[FILE_PLAYER_PARAMETERS_UASSET];
                baseOffset = fipl.FileOffset;
                buffer = new byte[fipl.FileSize];
                fs.Seek(baseOffset, SeekOrigin.Begin);
                fs.Read(buffer, 0, fipl.FileSize);
            }

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
                FillToDataGrid(dgvDataFixed, girl.ID, name, girl.Height, values);

                values = new GGVRBaseDataType[] { girl.Outfit, girl.Accessory, girl.Socks, girl.Shoes };
                FillToDataGrid(dgvOutfit, girl.ID, name, girl.Height, values);

                values = new GGVRBaseDataType[] { girl.Hair, girl.Face, girl.Skin, girl.EyeColor, girl.EyeBrowColor };
                FillToDataGrid(dgvAppearance, girl.ID, name, girl.Height, values);
            }
        }

        private void FillToDataGrid(DataGridView dgv, int girldId, string name, GGVRBaseDataType heightField, GGVRBaseDataType[] values)
        {
            bool found = false;

            List<object> objectValues = new List<object>();
            objectValues.Add(String.Format("{0:00}", girldId));
            objectValues.Add(name);
            objectValues.Add((heightField.Address - 0x0112).ToString("X"));
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

            if(this._settings.UsePAKFile)
            {
                try
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PAK_FILE, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    bw = new BinaryWriter(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open the PAK file for writing. Make sure that Gal*Gun VR isn't currently running!");
                    return false;
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
                    return false;
                }
            }
            

            foreach (GGVRGirl girl in this._girls)
            {
                girl.WriteAll(bw);
            }

            if (!this._settings.UsePAKFile)
            {
                bw = null;
                fs.Close();
                fs = null;

                try
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_GIRLS_HEIGHT_CURVE_UASSET, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    bw = new BinaryWriter(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open the Girl Height asset file for writing. Make sure that Gal*Gun VR isn't currently in a loading sequence (in the main menu the file can be safely edited)!");
                    return false;
                }
        }

            this._girlHeightFields.WriteAll(bw);
            
            if (!this._settings.UsePAKFile)
            {
                bw = null;
                fs.Close();
                fs = null;

                try
                {
                    fs = new FileStream(this._settings.GameDirectory + Path.DirectorySeparatorChar + FILE_PLAYER_PARAMETERS_UASSET, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    bw = new BinaryWriter(fs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open the Player parameter asset file for writing. Make sure that Gal*Gun VR isn't currently in a loading sequence (in the main menu the file can be safely edited)!");
                    return false;
                }
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

            bw = null;
            fs.Close();
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

            this.ReloadData();
            this.EnableEdited(true);
        }
    }
}
