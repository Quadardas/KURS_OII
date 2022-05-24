
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace KURS_OII
{
    public partial class Form1 : Form
    {
        string directoryname = null;
        int q_number = -1;
        double[,] yes = null;
        double[,] no = null;
        string[] text = null;
        int text_number = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void �����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // new Form2().Show();
        }
        public void dirname(string name)
        {
            directoryname = name;
            //MessageBox.Show(directoryname);
            if (directoryname != null)//���� ��� ����� �� ������
            {
                try
                {
                    
                    richTextBox1.Text = File.ReadAllText(directoryname + @"\name.txt");
                    richTextBox2.Text = File.ReadAllText(directoryname + @"\questions.txt");
                    richTextBox3.Text = File.ReadAllText(directoryname + @"\answer_name.txt");
                    richTextBox5.Text = File.ReadAllText(directoryname + @"\p.txt");
                    richTextBox6.Text = File.ReadAllText(directoryname + @"\yes_no_p.txt");
                }
                catch
                {
                    MessageBox.Show("�� ������� ��������� ����");
                    return;
                }
            }
            else
            {
                MessageBox.Show("�� ������� ��������� ����");
                return;
            }
        }
        private void �������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directoryname = null;   //��� �����, � ������� ����� ��
            Form3 f3 = new Form3(this, "������� �������� �����, � ������� �������� ����� ���� ������", "���������", true); //�������� �������� � �������� ������ � �����
            f3.ShowDialog();    //������� �����
            send_question();
            text = new string[richTextBox2.Lines.Count() * richTextBox3.Lines.Count()];
            read_yes_no_table(ref yes, ref no, richTextBox2.Lines.Count(), richTextBox3.Lines.Count());//�������� ����������� P(E|H) � P(E|��H)
        }
        private int read_yes_no_table(ref double[,] yes, ref double[,] no, int question, int answer) //P(E|H) � P(E|��H)
        {
            string[] p = null;
            p = read_string(p, richTextBox6);  //��������� ������ ������������

            yes = new double[answer, question]; //P(E|H)
            no = new double[answer, question];  //P(E|��H)
            int space = -1; //��������� �������
            for (int i = 0; i < p.Length; i++)
            {
                for (int j = 0; j < question; j++)
                {
                    space = p[i].IndexOf(' ');    //����� ������������� ����� �������
                    p[i] = p[i].Substring(space + 1);   //������ ����� ������� �� ������� ������
                    if (divide(i, j, ref p, ref yes) == -1) return -1;    //�������� ����������� ������ �� � �������
                    if (divide(i, j, ref p, ref no) == -1) return -1; //�������� ����������� ������ ��� � �������
                }
            }
            return 0;
        }

        private string[] read_string(string[] mas, RichTextBox text)    //��������� ��� ������ �� rich
        {
            int length = text.Lines.Count();    //������ ������� ����� ������ � ����            
            mas = new string[length];   //���������������� ������ �����
            for (int i = 0; i < length; i++)    //��������� ������ ������ richbox � ���������� � ������
                try
                {
                    mas[i] = text.Lines[i];
                }
                catch
                {
                    MessageBox.Show("������ � ���� ������");
                    return null;
                }
            return mas;
        }

        private int divide(int i, int j, ref string[] answers, ref double[,] table) //�������� ������� � ������������� 
        {
            int space = answers[i].IndexOf(' ');    //����� ������������� ����������� ������ 
            try
            {
                string p = "";
                if (space != -1) p = answers[i].Remove(space);  //���� �� ����� ������, �� �������� ���� �� ������ �� �������, �.�. �����������
                else p = answers[i];    //���� ����� ������, ��������� �� ��� ����
                table[i, j] = Convert.ToDouble(p);  //�������� ������ � ����� � ���������� � ������� �������
                answers[i] = answers[i].Substring(space + 1);   //������ ����������� ������ �� �������� ������
            }
            catch
            {
                MessageBox.Show("������ � ���������� ������������");
                return -1;
            }
            return 0;
        }

        private void send_question() //���������� ������ � rich � ������� ��������
        {
            q_number++; //���������� �������
            if (q_number >= richTextBox2.Lines.Count()) //���� �������� � �� �� ��������                
                //q_number = 0;  //�������� �������
                return;
            richTextBox4.Text = richTextBox2.Lines[q_number];   //���������� ��������� ������            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (q_number <= richTextBox2.Lines.Count() - 1)
            {
                text[text_number] = richTextBox4.Text + "\n ����� ������������: " + Convert.ToDouble(trackBar1.Value) + "\n\n"; //���������� ����� ������� � �����

                double phe = 0, pneh = 0, pq = 0;
                string[] phq = new string[richTextBox5.Lines.Count()];
                for (int i = 0; i < richTextBox3.Lines.Count(); i++)    //�������� ��� ������� �������� ������
                {
                    text[text_number] += "������� " + richTextBox3.Lines[i] + "\n";
                    phe = Phe(i);//����������� P(H|E)
                    text[text_number] += "P(H|E)=" + Convert.ToString(phe) + "\n";//������� � �����������
                    pneh = Pneh(i);//����������� P(��H|E)
                    text[text_number] += "P(��H|E)=" + Convert.ToString(pneh) + "\n";//������� � �����������
                    pq = Pq(Convert.ToDouble(trackBar1.Value), -3, 3);//����������� P(Q)
                    phq[i] = Convert.ToString(phe * pq + pneh * (1 - pq));//������������� ��������� �����������
                    text[text_number] += "P(H|Q)=" + phq[i] + "\n\n";//������� � �����������           
                }
                richTextBox5.Lines = phq;//������������ ��������� �����������
                send_question();    //���������� ��������� ������     
                text_number++;
            }
            else
            {
                MessageBox.Show("������������ ���������");
                return;
            }
        }

        private double Phe(int i)
        {
            double ph = Convert.ToDouble(richTextBox5.Lines[i]);    //��������� �����������
            double peh = yes[i, q_number];  //�����������, ��� ���������� E,���� ����� � (��������)
            double peneh = no[i, q_number]; //�����������, ��� ���������� E,���� ������� � (��������)
            double phe = (peh * ph) / ((peh * ph) + (peneh * (1 - ph)));    //�������������, �����������, ��� ���������� � ���� ����� � (�����-���)
            label1.Text = phe.ToString();
            return phe;

           
        }

        private double Pneh(int i)
        {
            double ph = Convert.ToDouble(richTextBox5.Lines[i]);    //��������� �����������
            double peh = yes[i, q_number];  //�����������, ��� ���������� E,���� ����� � (��������)
            double peneh = no[i, q_number]; //�����������, ��� ���������� E,���� ������� � (��������)
            double phe = ((1 - peh) * ph) / (((1 - peh) * ph) + ((1 - peneh) * (1 - ph)));  //�������������, �����������, ��� �� ���������� � ���� ����� � (��������)
            return phe;
        }

        private double Pq(double q, int a1, int a2)
        {
            double pq = (q - a1) / (a2 - a1);
            return pq;
        }

        private void button2_Click(object sender, EventArgs e)  //�������������
        {
            richTextBox5.LoadFile(directoryname + @"\p.txt");   //�������� ����������� �� ��������������
            q_number = -1;
            send_question();    //��������� ������ ������
            text = null;    //� ����������� ������ ���
            text_number = 0;
            text = new string[richTextBox2.Lines.Count() * richTextBox3.Lines.Count()];
        }

        private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // Form4 f = new Form4(text);
           // f.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.White;
            richTextBox2.BackColor = Color.White;
            richTextBox3.BackColor = Color.White;
            richTextBox4.BackColor = Color.White;
            richTextBox5.BackColor = Color.White;
        }
        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (q_number <= richTextBox2.Lines.Count() - 1)
            {
                MessageBox.Show("������������ ��� �� ��������");
            }
            else
            {
                string file = "";
                Form3 f3 = new Form3(this, "������� ��� ������������ �����", "���������", false); //�������� �������� � �������� ������ � �����
                f3.ShowDialog();    //������� �����
                file = f3.name + ".txt";
                System.IO.File.WriteAllLines(@file, text);
            }
        }
        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // new Form6().Show();
        }

    }
}
