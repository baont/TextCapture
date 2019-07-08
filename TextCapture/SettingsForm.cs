using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextCapture
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.okButton.Click += OkButton_Click;
            this.LettertextBox.Text = Properties.Settings.Default[CustomApplicationContext.HOT_KEY].ToString();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (this.LettertextBox.Text == "")
            {
                this.LettertextBox.Text = "C";
            }
            Properties.Settings.Default[CustomApplicationContext.HOT_KEY] = (char)this.LettertextBox.Text.ToUpper()[0];
            Properties.Settings.Default.Save();
            Close();
        }

    }
}
