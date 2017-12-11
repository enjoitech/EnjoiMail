using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnjoiMail
{
    public partial class FormAdd : System.Windows.Forms.Form
    {
        public FormAdd()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxUrl_Enter(object sender, EventArgs e)
        {
            if (this.textBoxUrl.ForeColor == Color.LightGray)
            {
                this.textBoxUrl.Text = "";
                this.textBoxUrl.ForeColor = Color.Black;
            }
        }

        private void textBoxUrl_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxUrl.Text))
            {
                this.textBoxUrl.ForeColor = Color.LightGray;
                this.textBoxUrl.Text = @"https://gmail.com or http(s)://mail.your-domain-for-g-suite.com";
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // check name and URL
            if (String.IsNullOrEmpty(this.textBoxName.Text))
            {
                MessageBox.Show(@"Please enter a name", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (String.IsNullOrEmpty(this.textBoxUrl.Text))
            {
                MessageBox.Show(@"Please enter an GMail URL", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(this.textBoxUrl.Text, "^https?://", RegexOptions.IgnoreCase))
            {
                MessageBox.Show(@"Please enter a valid URL", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public Model.Site GetSite()
        {
            return new Model.Site
            {
                Name = this.textBoxName.Text,
                Url = this.textBoxUrl.Text
            };
        }
    }
}
