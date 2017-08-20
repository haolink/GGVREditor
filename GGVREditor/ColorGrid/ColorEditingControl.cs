using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Grid
{
    class ColorEditingControl : Button, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        public Color Value;
        public ColorEditingControl()
        {
            this.Click += new EventHandler(ColorEditingControl_Click);
        }
        void ColorEditingControl_Click(object sender, EventArgs e)
        {
            ColorDialog diag = new ColorDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                if (this.Value != diag.Color)
                {
                    valueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                }
            }
        }
        public object EditingControlFormattedValue
        {
            get { return this.Value; }
            set
            {
                if (value != null)
                { this.Value = (Color)value; }
            }
        }
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        { return EditingControlFormattedValue; }
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        { this.Font = dataGridViewCellStyle.Font; }
        public int EditingControlRowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }
        public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }
        public void PrepareEditingControlForEdit(bool selectAll) { }
        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }
        public DataGridView EditingControlDataGridView
        {
            get { return dataGridView; }
            set { dataGridView = value; }
        }
        public bool EditingControlValueChanged
        {
            get { return valueChanged; }
            set { valueChanged = value; }
        }
        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }
    }
}