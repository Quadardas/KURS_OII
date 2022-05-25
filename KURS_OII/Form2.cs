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
    public partial class Form2 : Form
    {
        Form1 form1;
        List<RichTextBox> richTextBoxes;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            richTextBoxes = new List<RichTextBox>() { form1.richTextBox1, form1.richTextBox2, form1.richTextBox3, form1.richTextBox4, form1.richTextBox5 };

        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.load();
            richTextBox1.Text = form1.richTextBox1.Text;
            richTextBox2.Text = form1.richTextBox2.Text;
            richTextBox3.Text = form1.richTextBox3.Text;
            richTextBox4.Text = form1.richTextBox6.Text;
            richTextBox5.Text = form1.richTextBox5.Text;

           foreach(RichTextBox richTextBox in richTextBoxes)
            {
                richTextBox.Clear();
            }
        
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // File.WriteAllLines(form1.directoryname + )
            File.WriteAllLines(form1.directoryname + @"\name.txt", richTextBox1.Lines);
            File.WriteAllLines(form1.directoryname + @"\questions.txt", richTextBox2.Lines);
            File.WriteAllLines(form1.directoryname + @"\answer_name.txt", richTextBox3.Lines);
            File.WriteAllLines(form1.directoryname + @"\p.txt", richTextBox5.Lines);
            File.WriteAllLines(form1.directoryname + @"\yes_no_p.txt", richTextBox4.Lines);
        }
    }
}
