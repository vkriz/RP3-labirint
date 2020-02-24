using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labirint
{
    public partial class Form1 : Form
    {
        int counter = 0;
        Timer MyTimer;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MyTimer != null)
            {
                MyTimer.Stop();
                MyTimer.Tick -= MyTimer_Tick;
            }
            pictureBox1.BorderStyle = BorderStyle.None;
            int level = Int32.Parse(comboBox1.SelectedItem.ToString());
            int gridSize = level * 5;
            ColoredGrid labirint = new ColoredGrid(gridSize, gridSize);
            Image img = labirint.ToImg((pictureBox1.Size.Width - 1)/ gridSize, gridSize - 1, gridSize - 1, radioButton1.Checked);

            pictureBox1.Image = img;

            if(radioButton3.Checked)
            {
                counter = level * 30;
                if (counter >= 60)
                {
                    int mins = (int)(counter / 60);
                    int secs = counter - mins * 60;
                    String text = String.Concat("0", mins.ToString());
                    text = String.Concat(text, ":");
                    text = String.Concat(text, secs);
                    label2.Text = text;
                }
                else
                {
                    String text = String.Concat("00:", counter);
                    label2.Text = text;
                }
                MyTimer = new Timer();
                MyTimer.Interval = (1000);
                MyTimer.Tick += new EventHandler(MyTimer_Tick);
                MyTimer.Start();
            }
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            Timer MyTimer = (Timer)sender;
            if (!MyTimer.Enabled || counter < 0) return;
            if (counter == 0)
            {
                MyTimer.Stop();
                MyTimer.Tick -= MyTimer_Tick;
                string message = "Vrijeme je isteklo!";
                string caption = "Igra je gotova";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
            if(counter > 0) counter--;
            if(counter < 10)
            {
                label2.ForeColor = Color.Red;
            }
            if(counter >= 60)
            {
                int mins = (int)(counter / 60);
                int secs = counter - mins * 60;
                String text = String.Concat("0", mins.ToString());
                text = String.Concat(text, ":");
                if(secs >= 10)
                {
                    text = String.Concat(text, secs);
                }
                else
                {
                    text = String.Concat(text, "0");
                    text = String.Concat(text, secs);
                }
                label2.Text = text;
            }
            else
            {
                String text = "00:";
                if (counter >= 10)
                {
                    text = String.Concat(text, counter);
                }
                else
                {
                    text = String.Concat(text, "0");
                    text = String.Concat(text, counter);
                }
                label2.Text = text;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
