using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labirint
{
    public partial class Form1 : Form
    {
        int counter = 0;
        int lives = 3;
        Timer MyTimer;
        Point lastPoint = Point.Empty;
        bool isMouseDown = new Boolean();
        bool isKeyPressed = new Boolean();
        ColoredPathGrid labirint;
        int cellSize;
        int x = 0;
        int y = 0;
        int xEnd;
        int yEnd;
        int preyX = -1;
        int preyY = -1;
        Point startingPoint;
        Point endingPoint;
        Image img;
        bool hintOn = false;
        PictureBox pc2;

        public Form1()
        {
            InitializeComponent();
            pc2 = new PictureBox();
            pc2.BackColor = Color.Transparent;
            pc2.Parent = pictureBox1;
            pc2.Size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pc2.MouseDown += new MouseEventHandler(PictureBox1_MouseDown);
            pc2.MouseUp += new MouseEventHandler(PictureBox1_MouseUp);
            pc2.MouseMove += new MouseEventHandler(PictureBox1_MouseMove);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
            label3.Text = "Životi: 3";
            preyX = -1;
            preyY = -1;
            if (MyTimer != null)
            {
                MyTimer.Stop();
                MyTimer.Tick -= MyTimer_Tick;
            }
            pictureBox1.BorderStyle = BorderStyle.None;
            int level = Int32.Parse(comboBox1.SelectedItem.ToString());
            int gridSize = level * 5;
            labirint = new ColoredPathGrid(gridSize, gridSize);
            cellSize = (pictureBox1.Size.Width - 1) / gridSize;

            startingPoint = pictureBox1.Location;
            startingPoint.Offset(cellSize / 2, cellSize / 2);
            labirint.Distances = labirint[0, 0].Distances;
            xEnd = labirint._farthest.Column;
            yEnd = labirint._farthest.Row;
            endingPoint = new Point(labirint._farthest.Column * cellSize + cellSize / 2, labirint._farthest.Row * cellSize + cellSize / 2);

            x = 0;
            y = 0;
            isKeyPressed = false;

            if(radioButton1.Checked)
            {
                Random random = new Random();
                preyX = random.Next(0, gridSize);
                preyY = random.Next(0, gridSize);
                while ((preyX == 0 && preyY == 0) || (preyX == xEnd && preyY == yEnd))
                {
                    preyX = random.Next(0, gridSize);
                    preyY = random.Next(0, gridSize);
                }
            } 
           
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
            refreshPicture();

            Button hintButton = new Button();
            hintButton.Text = "&Hint";
            hintButton.Location = new Point(23, 420);
            hintButton.Click += new EventHandler(Hint_Click);
            Controls.Add(hintButton);
        }

        private void refreshPicture()
        {
            img = labirint.ToImg(cellSize, xEnd, yEnd, preyX, preyY, false);
            pictureBox1.Image = img;
            pc2.Image = labirint.drawPrey(preyX, preyY, cellSize);
            Cursor.Position = PointToScreen(startingPoint);
            lastPoint = Point.Empty;
            isMouseDown = false;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!hintOn)
            {
                Point st = new Point(startingPoint.X - pictureBox1.Location.X, startingPoint.Y - pictureBox1.Location.Y);
                if (!isKeyPressed && Location.X >= st.X - cellSize / 2 && e.Location.X <= st.X + cellSize / 2 && e.Location.Y >= st.Y - cellSize / 2 && e.Location.Y <= st.Y + cellSize / 2)
                {
                    lastPoint = e.Location;
                    isMouseDown = true;
                }
                else
                {
                    isMouseDown = false;
                    lastPoint = Point.Empty;
                    refreshPicture();
                }
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true && !hintOn)
            {
                if (lastPoint != null)
                {
                    using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                    {
                        foreach (Cell cell in labirint.Cells)
                        {
                            var x1 = cellSize * cell.Column;
                            var x2 = cellSize * (cell.Column + 1);
                            var y1 = cellSize * cell.Row;
                            var y2 = cellSize * (cell.Row + 1);

                            int x = e.Location.X;
                            int y = e.Location.Y;
                            if (cell.Up == null)
                            {
                                if (x >= x1 && x <= x2 && y >= y1 && y <= y1)
                                {
                                    MessageBox.Show("Zid!!");
                                    lives--;
                                    label3.Text = String.Concat("Životi: ", lives);
                                    if (lives == 0)
                                    {
                                        if (MyTimer != null)
                                        {
                                            MyTimer.Stop();
                                            MyTimer.Tick -= MyTimer_Tick;
                                        }
                                        MessageBox.Show("Game over!");
                                        Reset();
                                    }
                                    refreshPicture();
                                    return;
                                }

                            }

                            if (!cell.IsLinked(cell.Down))
                            {
                                if (x >= x1 && x <= x2 && y >= y2 && y <= y2)
                                {
                                    MessageBox.Show("Zid!!");
                                    lives--;
                                    label3.Text = String.Concat("Životi: ", lives);
                                    if (lives == 0)
                                    {
                                        if (MyTimer != null)
                                        {
                                            MyTimer.Stop();
                                            MyTimer.Tick -= MyTimer_Tick;
                                        }
                                        MessageBox.Show("Game over!");
                                        Reset();
                                    }
                                    refreshPicture();
                                    return;
                                }
                            }

                            if (cell.Left == null)
                            {
                                if (x >= x1 && x <= x1 && y >= y1 && y <= y2)
                                {
                                    MessageBox.Show("Zid!!");
                                    lives--;
                                    label3.Text = String.Concat("Životi: ", lives);
                                    if (lives == 0)
                                    {
                                        if (MyTimer != null)
                                        {
                                            MyTimer.Stop();
                                            MyTimer.Tick -= MyTimer_Tick;
                                        }
                                        MessageBox.Show("Game over!");
                                        Reset();
                                    }
                                    refreshPicture();
                                    return;
                                }
                            }

                            if (!cell.IsLinked(cell.Right))
                            {
                                if (x >= x2 && x <= x2 && y >= y1 && y <= y2)
                                {
                                    MessageBox.Show("Zid!!");
                                    lives--;
                                    label3.Text = String.Concat("Životi: ", lives);
                                    if (lives == 0)
                                    {
                                        if (MyTimer != null)
                                        {
                                            MyTimer.Stop();
                                            MyTimer.Tick -= MyTimer_Tick;
                                        }
                                        MessageBox.Show("Game over!");
                                        Reset();
                                    }
                                    refreshPicture();
                                    return;
                                }
                            }

                        }

                        g.DrawLine(new Pen(Color.Black, 1), lastPoint, e.Location);
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                    }

                    pictureBox1.Invalidate();
                    lastPoint = e.Location;
                    if (checkEnd(e))
                    {
                        if(MyTimer != null)
                        {
                            MyTimer.Stop();
                            MyTimer.Tick -= MyTimer_Tick;
                        }
                        MessageBox.Show("Pobjeda!!");
                        Reset();
                    }
                    checkPrey(e);
                }

            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private Boolean checkEnd(MouseEventArgs e)
        {
            Point st = new Point(endingPoint.X, endingPoint.Y);
            if (preyX == -1 && preyY == -1 && e.Location.X >= st.X - cellSize / 2 && e.Location.X <= st.X + cellSize / 2 && e.Location.Y >= st.Y - cellSize / 2 && e.Location.Y <= st.Y + cellSize / 2)
            {
                isMouseDown = false;
                lastPoint = Point.Empty;
                return true;
            }

            return false;
        }

        private void checkPrey(MouseEventArgs e)
        {

            Point st = new Point(preyX * cellSize + cellSize / 2, preyY * cellSize + cellSize / 2);
            if (e.Location.X >= st.X - cellSize / 2 && e.Location.X <= st.X + cellSize / 2 && e.Location.Y >= st.Y - cellSize / 2 && e.Location.Y <= st.Y + cellSize / 2)
            {
                preyX = -1;
                preyY = -1;
                pc2.Image = labirint.drawPrey(preyX, preyY, cellSize);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isMouseDown || hintOn) return;

            if (e.KeyCode == Keys.Up)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Up) || (y - 1) < 0)
                {
                    MessageBox.Show("Zid!!");
                    lives--;
                    label3.Text = String.Concat("Životi: ", lives);
                    if (lives == 0)
                    {
                        if (MyTimer != null)
                        {
                            MyTimer.Stop();
                            MyTimer.Tick -= MyTimer_Tick;
                        }
                        MessageBox.Show("Game over!");
                        Reset();
                    }
                    return;
                }

                y--;

                ProccessKeyDown();
            }

            if (e.KeyCode == Keys.Down)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Down) || (y + 1) > cellSize)
                {
                    MessageBox.Show("Zid!!");
                    lives--;
                    label3.Text = String.Concat("Životi: ", lives);
                    if (lives == 0)
                    {
                        if (MyTimer != null)
                        {
                            MyTimer.Stop();
                            MyTimer.Tick -= MyTimer_Tick;
                        }
                        MessageBox.Show("Game over!");
                        Reset();
                    }
                    return;
                }

                y++;

                ProccessKeyDown();
            }

            if (e.KeyCode == Keys.Left)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Left) || (x - 1) < 0)
                {
                    MessageBox.Show("Zid!!");
                    lives--;
                    label3.Text = String.Concat("Životi: ", lives);
                    if (lives == 0)
                    {
                        if (MyTimer != null)
                        {
                            MyTimer.Stop();
                            MyTimer.Tick -= MyTimer_Tick;
                        }
                        MessageBox.Show("Game over!");
                        Reset();
                    }
                    return;
                }

                x--;

                ProccessKeyDown();
            }

            if (e.KeyCode == Keys.Right)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Right) || (x + 1) > cellSize)
                {
                    MessageBox.Show("Zid!!");
                    lives--;
                    label3.Text = String.Concat("Životi: ", lives);
                    if (lives == 0)
                    {
                        if (MyTimer != null)
                        {
                            MyTimer.Stop();
                            MyTimer.Tick -= MyTimer_Tick;
                        }
                        MessageBox.Show("Game over!");
                        Reset();
                    }
                    return;
                }

                x++;

                ProccessKeyDown();
            }

        }
        private void ProccessKeyDown()
        {
            var start = labirint[0, 0];
            var distances = start.Distances;
            labirint.Path = distances.PathTo(labirint[y, x]);

            img = labirint.ToImg(cellSize, xEnd, yEnd, preyX, preyY, false);
            pictureBox1.Image = img;
            pc2.Image = labirint.drawPrey(preyX, preyY, cellSize);

            isKeyPressed = true;
            if (y == yEnd && x == xEnd && preyX == -1 && preyY == -1)
            {
                if(MyTimer != null)
                {
                    MyTimer.Stop();
                    MyTimer.Tick -= MyTimer_Tick;
                }
                MessageBox.Show("Pobjeda!!");
                x = 0;
                y = 0;
                isKeyPressed = false;
                Reset();
            }

            if(x == preyX && y == preyY)
            {
                preyX = -1;
                preyY = -1;
            }

        }

        private void Hint_Click(object sender, EventArgs e)
        {
            hintOn = true;
            var start = labirint[0, 0];
            var distances = start.Distances;
            labirint.Distances = labirint[0, 0].Distances;
            Image img = labirint.ToImg(cellSize, xEnd, yEnd, preyX, preyY, true);
            pictureBox1.Image = img;
            pc2.Image = labirint.drawPrey(preyX, preyY, cellSize);

            timer1.Start();
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

        private void Timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = img;
            pc2.Image = labirint.drawPrey(preyX, preyY, cellSize);
            if (isMouseDown)
                Cursor.Position = PointToScreen(new Point(lastPoint.X + pictureBox1.Location.X, lastPoint.Y + pictureBox1.Location.Y));
            timer1.Stop();
            hintOn = false;
        }

        public void Reset ()
        {
            counter = 30;
            label2.ForeColor = Color.Black;
            lives = 3;
            MyTimer = null;
            lastPoint = Point.Empty;
            isMouseDown = new Boolean();
            isKeyPressed = new Boolean();
            labirint = null;
            cellSize = 0;
            x = 0;
            y = 0;
            xEnd = 0;
            yEnd = 0;
            preyX = -1;
            preyY = -1;
            startingPoint = Point.Empty;
            endingPoint = Point.Empty;
            img = null;
            hintOn = false;
            pictureBox1.Controls.Clear();
            pc2 = new PictureBox();
            pc2.BackColor = Color.Transparent;
            pc2.Parent = pictureBox1;
            pc2.Size = new Size(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pc2.MouseDown += new MouseEventHandler(PictureBox1_MouseDown);
            pc2.MouseUp += new MouseEventHandler(PictureBox1_MouseUp);
            pc2.MouseMove += new MouseEventHandler(PictureBox1_MouseMove);
            pictureBox1.Image = null;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
