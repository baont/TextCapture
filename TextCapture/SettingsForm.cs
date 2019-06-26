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
            for (char ch = 'A'; ch <= 'Z'; ch++)
            {
                this.letterCombo.Items.Add(ch);
            }
            this.Shown += SettingsForm_Shown;
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            this.letterCombo.SelectedItem = settings.Hotkey;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            settings.Hotkey = (char)letterCombo.SelectedItem;
            Close();
        }
    }
}
