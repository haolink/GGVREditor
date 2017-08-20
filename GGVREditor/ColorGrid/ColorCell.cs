using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Grid
{
    public class ColorCell : DataGridViewCell
    {
        public ColorCell() : base() { }
        public new Color Value = Color.White;
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            ColorEditingControl ctl = DataGridView.EditingControl as ColorEditingControl;
            ctl.Value = (Color)this.Value;
        }
        public override Type EditType
        {
            get { return typeof(ColorEditingControl); }
        }
        public override Type ValueType
        {
            get { return typeof(Color); }
        }
        public override object DefaultNewRowValue
        {
            get { return Color.White; }
        }
        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);
            this.InitializeEditingControl(e.RowIndex, this.DefaultNewRowValue, this.Style);
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (this.Selected)
            {
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.SelectionBackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }
            }
            else
            {
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }
            }
            // Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            { PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle); }
        }
    }
}