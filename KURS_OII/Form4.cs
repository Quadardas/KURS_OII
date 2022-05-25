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
    public partial class Form4 : Form
    {
        string[] text;
        
        public Form4(string [] text)
        {
            InitializeComponent();
            this.text = text;
            for(int i = 0; i < text.Length; i++)
            {
                richTextBox1.Text += text[i] + '\n';
            }
            
        }
    }
}
