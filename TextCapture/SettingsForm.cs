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
        public Settings settings { get; set; }
        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            this.settings = settings;
            this.okButton.Click += OkButton_Click;
            this.LettertextBox.Text = settings.Hotkey.ToString();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (this.LettertextBox.Text == "")
            {
                this.LettertextBox.Text = "C";
            }
            settings.Hotkey = (char)this.LettertextBox.Text[0];
            Close();
        }

    }
}
