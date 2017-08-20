using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace GGVREditor
{
    public partial class MainForm : Form
    {
        private EditSettings _settings;

        internal enum SelectedGrid
        {
            Undetermined, Fixed
        }

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
            { 0x81, "Risu" }, { 0x82, "Chiru" }, { 0x83, "Yuko" }, { 0x84, "Shinobu" }, { 0x85, "Maya" }, { 0x86, "1st grade" }, { 0x87, "2nd grade" }, { 0x88, "3rd grade" }, { 0x89, "Teacher" }, { 0x8A, "Kurona" }
        };
        public static readonly Dictionary<byte, string> accessoryValues = new Dictionary<byte, string>()
        {
            { 0x6E, "Ribbon" }, { 0x6F, "Tie" }
        };

        private GGVRGirl[] _girls;

        public MainForm()
        {
            InitializeComponent();

            PopulateComboboxColumn<byte, string>(clmOOutfit, outfitValues);
            PopulateComboboxColumn<byte, string>(clmOAccessory, accessoryValues);
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.LoadValues();

            this.FillGrid();
        }

        private void LoadValues()
        {
            //TODO: Handle PAK file

            FileStream fs = new FileStream(this._settings.GameDirectory + @"\GalGunVR\Content\VRGG\DataTable\GalData\GalVisualDatas.uasset", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(buffer, 0, (int)fs.Length);
            
            byte initial = 0x3B;

            byte[] needle;
            int[] offsets = new int[characterNames.Length];

            for (int i = 0; i < characterNames.Length; i++)
            {
                byte searchByte = (byte)(initial + i);
                needle = new byte[]
                {
                    searchByte, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x9C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };

                List<long> locations = buffer.IndexesOf(needle);

                if (locations.Count >= 0)
                {
                    offsets[i] = (int)locations[0];
                } else
                {
                    offsets[i] = -1;
                }
            }

            List<GGVRGirl> girls = new List<GGVRGirl>();

            for(int i = 0; i < offsets.Length; i++)
            {
                if(offsets[i] > 0)
                {
                    GGVRGirl g = new GGVRGirl();

                    string gPrep = "G" + (i + 1).ToString();

                    g.ID = i + 1;
                    g.Name = characterNames[i];

                    g.Height = this.LoadValueAndOriginal<float>(br, offsets[i] + 0x112, gPrep, "Height");
                    g.HeadSizeRatio = this.LoadValueAndOriginal<float>(br, offsets[i] + 0x12F, gPrep, "HeadSizeRatio");
                    g.Bust = this.LoadValueAndOriginal<float>(br, offsets[i] + 0x14C, gPrep, "Bust");
                    g.Waist = this.LoadValueAndOriginal<float>(br, offsets[i] + 0x169, gPrep, "Waist");
                    g.Hip = this.LoadValueAndOriginal<float>(br, offsets[i] + 0x186, gPrep, "Hip");

                    g.Outfit = this.LoadValueAndOriginal<byte>(br, offsets[i] + 0x1DC, gPrep, "Outfit");
                    g.Accessory = this.LoadValueAndOriginal<byte>(br, offsets[i] + 0x2D2, gPrep, "Accessory");
                    g.Socks = this.LoadValueAndOriginal<byte>(br, offsets[i] + 0x31C, gPrep, "Socks");
                    g.Shoes = this.LoadValueAndOriginal<byte>(br, offsets[i] + 0x35E, gPrep, "Shoes");                    

                    girls.Add(g);
                }
            }

            fs.Close();

            this._girls = girls.ToArray();
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
            SaveFile();

            MessageBox.Show("File saved.");
        }

        public void SaveFile()
        {
            //Todo: PAK file
            FileStream fsGVD = new FileStream(this._settings.GameDirectory + @"\GalGunVR\Content\VRGG\DataTable\GalData\GalVisualDatas.uasset", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            BinaryWriter bwGVD = new BinaryWriter(fsGVD);

            foreach (GGVRGirl girl in this._girls)
            {
                girl.WriteAll(bwGVD);
            }

            bwGVD = null;
            fsGVD.Close();
            fsGVD = null;

            EnableEdited(false);
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

            string inputValue = cell.Value.ToString();
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
    }
}
