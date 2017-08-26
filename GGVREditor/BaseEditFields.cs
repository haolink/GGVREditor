using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;

namespace GGVREditor
{
    public class BaseEditFields
    {
        public List<TextFieldDataSet> EditFields { get; private set; }

        public EventHandler MarkEdited;

        public BaseEditFields()
        {
            this.EditFields = new List<TextFieldDataSet>();
        }

        public void AddRelation(TextBox textBox, GGVRBaseDataType dataSet)
        {
            TextFieldDataSet tfdsOld = this.DataTypeOfTextEdit(textBox);

            if (tfdsOld != null)
            {
                this.EditFields.Remove(tfdsOld);
                textBox.Leave -= this.CheckIfEdited;
            }

            TextFieldDataSet tfds = new TextFieldDataSet(textBox, dataSet);
            this.EditFields.Add(tfds);

            textBox.Text = dataSet.ToString();
            textBox.Tag = dataSet;

            textBox.Leave += this.CheckIfEdited;
        }

        private TextFieldDataSet DataTypeOfTextEdit(TextBox tb)
        {
            foreach(TextFieldDataSet tfds in this.EditFields)
            {
                if(tfds.TextBox == tb)
                {
                    return tfds;
                }
            }

            return null;
        }

        private void CheckIfEdited(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = (TextBox)sender;

                this.SetTextBoxValue(tb, tb.Text);
            }
        }        

        public void SetTextBoxValue(TextBox tb, string text)
        {
            TextFieldDataSet tfds = this.DataTypeOfTextEdit(tb);

            if (tfds == null)
            {
                return;
            }

            bool edited = tfds.DataSet.SetNewValueFromString(text);

            if (edited)
            {
                if (this.MarkEdited != null)
                {
                    this.MarkEdited(this, EventArgs.Empty);
                }
            }

            tb.Text = tfds.DataSet.ToString();
        }

        public void WriteAll(BinaryWriter bw)
        {
            foreach(TextFieldDataSet ds in this.EditFields)
            {
                ds.DataSet.WriteToFile(bw);
            }
        }

        public void RestoreAll()
        {
            foreach (TextFieldDataSet ds in this.EditFields)
            {
                ds.DataSet.RestoreOriginal();
            }
        }
    }

    public class TextFieldDataSet
    {
        public TextBox TextBox { get; private set; }
        public GGVRBaseDataType DataSet { get; private set; }

        public TextFieldDataSet(TextBox textBox, GGVRBaseDataType dataSet)
        {
            this.TextBox = textBox;
            this.DataSet = dataSet;
        }
    }
}
