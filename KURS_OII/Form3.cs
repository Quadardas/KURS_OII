using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KURS_OII
{
    public partial class Form3 : Form
    {
        public string name, b, c, d;
        Form1 form1;


        public Form3(Form1 form1, string b, string c, bool d)
        {
            InitializeComponent();
            this.name = name;
            this.form1 = form1;
            label1.Text = b;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            form1.dirname(textBox1.Text.ToString());
            Console.WriteLine("1");
            

        }
    }
}
