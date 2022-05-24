
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
        private void редакторБазЗнанийToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // new Form2().Show();
        }
        public void dirname(string name)
        {
            directoryname = name;
            //MessageBox.Show(directoryname);
            if (directoryname != null)//если имя папки не пустое
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
                    MessageBox.Show("Не удалось загрузить файл");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не удалось загрузить файл");
                return;
            }
        }
        private void загрузитьБазуЗнанийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directoryname = null;   //имя папки, в которой лежит БЗ
            Form3 f3 = new Form3(this, "Введите название папки, в которой хранятся файлы базы знаний", "Загрузить", true); //описание действия и название кнопки в форме
            f3.ShowDialog();    //вызвали форму
            send_question();
            text = new string[richTextBox2.Lines.Count() * richTextBox3.Lines.Count()];
            read_yes_no_table(ref yes, ref no, richTextBox2.Lines.Count(), richTextBox3.Lines.Count());//получаем вероятности P(E|H) и P(E|неH)
        }
        private int read_yes_no_table(ref double[,] yes, ref double[,] no, int question, int answer) //P(E|H) и P(E|неH)
        {
            string[] p = null;
            p = read_string(p, richTextBox6);  //считываем строки вероятностей

            yes = new double[answer, question]; //P(E|H)
            no = new double[answer, question];  //P(E|неH)
            int space = -1; //положение пробела
            for (int i = 0; i < p.Length; i++)
            {
                for (int j = 0; j < question; j++)
                {
                    space = p[i].IndexOf(' ');    //здесь заканчивается номер вопроса
                    p[i] = p[i].Substring(space + 1);   //убрали номер вопроса из входной строки
                    if (divide(i, j, ref p, ref yes) == -1) return -1;    //записали вероятность ответа да в таблицу
                    if (divide(i, j, ref p, ref no) == -1) return -1; //записали вероятность ответа нет в таблицу
                }
            }
            return 0;
        }

        private string[] read_string(string[] mas, RichTextBox text)    //считываем все записи из rich
        {
            int length = text.Lines.Count();    //узнали сколько строк текста в окне            
            mas = new string[length];   //инициализировали массив строк
            for (int i = 0; i < length; i++)    //считываем каждую строку richbox и записываем в массив
                try
                {
                    mas[i] = text.Lines[i];
                }
                catch
                {
                    MessageBox.Show("Ошибка в базе знаний");
                    return null;
                }
            return mas;
        }

        private int divide(int i, int j, ref string[] answers, ref double[,] table) //выделяем таблицу с вероятностями 
        {
            int space = answers[i].IndexOf(' ');    //здесь заканчивается вероятность ответа 
            try
            {
                string p = "";
                if (space != -1) p = answers[i].Remove(space);  //если не конец строки, то вырезаем чать от начала до пробела, т.е. вероятность
                else p = answers[i];    //если конец строки, оставляем всё как есть
                table[i, j] = Convert.ToDouble(p);  //переводи строку в число и записываем в таблицу ответов
                answers[i] = answers[i].Substring(space + 1);   //убрали вероятность ответа из исходной строки
            }
            catch
            {
                MessageBox.Show("Ошибка в заполнении вероятностей");
                return -1;
            }
            return 0;
        }

        private void send_question() //отправляем вопрос в rich с текущим вопросом
        {
            q_number++; //наращиваем счетчик
            if (q_number >= richTextBox2.Lines.Count()) //если вопросов в БЗ не осталось                
                //q_number = 0;  //обнуляем счетчик
                return;
            richTextBox4.Text = richTextBox2.Lines[q_number];   //перемещаем очередной вопрос            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (q_number <= richTextBox2.Lines.Count() - 1)
            {
                text[text_number] = richTextBox4.Text + "\n Ответ пользователя: " + Convert.ToDouble(trackBar1.Value) + "\n\n"; //записываем текст вопроса и ответ

                double phe = 0, pneh = 0, pq = 0;
                string[] phq = new string[richTextBox5.Lines.Count()];
                for (int i = 0; i < richTextBox3.Lines.Count(); i++)    //пересчет для каждого варианта ответа
                {
                    text[text_number] += "Вариант " + richTextBox3.Lines[i] + "\n";
                    phe = Phe(i);//прощитываем P(H|E)
                    text[text_number] += "P(H|E)=" + Convert.ToString(phe) + "\n";//выводим в трассировку
                    pneh = Pneh(i);//прощитываем P(неH|E)
                    text[text_number] += "P(неH|E)=" + Convert.ToString(pneh) + "\n";//выводим в трассировку
                    pq = Pq(Convert.ToDouble(trackBar1.Value), -3, 3);//высчитываем P(Q)
                    phq[i] = Convert.ToString(phe * pq + pneh * (1 - pq));//пересчитываем априорную вероятность
                    text[text_number] += "P(H|Q)=" + phq[i] + "\n\n";//выводим в трассировку           
                }
                richTextBox5.Lines = phq;//переписываем априорную вероятность
                send_question();    //отправляем следующий вопрос     
                text_number++;
            }
            else
            {
                MessageBox.Show("Консультация закончена");
                return;
            }
        }

        private double Phe(int i)
        {
            double ph = Convert.ToDouble(richTextBox5.Lines[i]);    //априорная вероятность
            double peh = yes[i, q_number];  //вероятность, что произойдет E,если верно Н (условная)
            double peneh = no[i, q_number]; //вероятность, что произойдет E,если неверно Н (условная)
            double phe = (peh * ph) / ((peh * ph) + (peneh * (1 - ph)));    //апостериорная, вероятность, что произойдет Н если верно Е (услов-ная)
            label1.Text = phe.ToString();
            return phe;

           
        }

        private double Pneh(int i)
        {
            double ph = Convert.ToDouble(richTextBox5.Lines[i]);    //априорная вероятность
            double peh = yes[i, q_number];  //вероятность, что произойдет E,если верно Н (условная)
            double peneh = no[i, q_number]; //вероятность, что произойдет E,если неверно Н (условная)
            double phe = ((1 - peh) * ph) / (((1 - peh) * ph) + ((1 - peneh) * (1 - ph)));  //апостериорная, вероятность, что не произойдет Н если верно Е (условная)
            return phe;
        }

        private double Pq(double q, int a1, int a2)
        {
            double pq = (q - a1) / (a2 - a1);
            return pq;
        }

        private void button2_Click(object sender, EventArgs e)  //перезапустить
        {
            richTextBox5.LoadFile(directoryname + @"\p.txt");   //изменяем вероятности на первоначальные
            q_number = -1;
            send_question();    //отправили первый вопрос
            text = null;    //в трассировке ничего нет
            text_number = 0;
            text = new string[richTextBox2.Lines.Count() * richTextBox3.Lines.Count()];
        }

        private void трассировкаToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (q_number <= richTextBox2.Lines.Count() - 1)
            {
                MessageBox.Show("Консультация еще не окончена");
            }
            else
            {
                string file = "";
                Form3 f3 = new Form3(this, "Введите имя сохраняемого файла", "Сохранить", false); //описание действия и название кнопки в форме
                f3.ShowDialog();    //вызвали форму
                file = f3.name + ".txt";
                System.IO.File.WriteAllLines(@file, text);
            }
        }
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // new Form6().Show();
        }

    }
}
