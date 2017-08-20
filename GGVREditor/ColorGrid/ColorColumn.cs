using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Grid
{
    class ColorColumn : DataGridViewColumn
    {
        public ColorColumn() : base()
        { this.CellTemplate = new ColorCell(); }
        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set { base.CellTemplate = value; }
        }
    }
}