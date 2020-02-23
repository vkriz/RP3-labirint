using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace labirint
{
    public partial class Form1 : Form
    {

        Point lastPoint = Point.Empty;
        bool isMouseDown = new Boolean();
        bool isKeyPressed = new Boolean();
        ColoredPathGrid labirint;
        int cellSize;
        int x = 0;
        int y = 0;
        Point startingPoint;
        Point endingPoint;
        List<Cell> visited = new List<Cell>();
        Image img;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.None;
            int gridSize = Int32.Parse(textBox1.Text);
            labirint = new ColoredPathGrid(gridSize, gridSize);

            cellSize = (pictureBox1.Size.Width - 1) / gridSize;

            startingPoint = pictureBox1.Location;
            startingPoint.Offset(cellSize / 2, cellSize / 2);
            endingPoint = pictureBox1.Location;
            endingPoint.Offset(cellSize / 2, pictureBox1.Height - cellSize / 2);
            x = 0;
            y = 0;
            isKeyPressed = false;
            refreshPicture();
        }

        private void refreshPicture()
        {
            img = labirint.ToImg(cellSize, false);
            pictureBox1.Image = img;
            Cursor.Position = PointToScreen(startingPoint);
            visited.Clear();
            lastPoint = Point.Empty;
            isMouseDown = false;

        }


        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Point st  = new Point(startingPoint.X - pictureBox1.Location.X, startingPoint.Y - pictureBox1.Location.Y);
            if (!isKeyPressed && Location.X >= st.X - cellSize/2 && e.Location.X <= st.X + cellSize / 2 && e.Location.Y >= st.Y - cellSize / 2 && e.Location.Y <= st.Y + cellSize / 2)
            {
                lastPoint = e.Location;
                isMouseDown = true;
            }
            else
            {
                refreshPicture();
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true)
            {
                if (lastPoint != null)
                {
                    using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                    {
                        foreach(Cell cell in labirint.Cells)
                        {
                            var x1 = cellSize * cell.Column;
                            var x2 = cellSize * (cell.Column + 1);
                            var y1 = cellSize * cell.Row;
                            var y2 = cellSize * (cell.Row + 1);

                            int x = e.Location.X;
                            int y = e.Location.Y;
                            if (cell.Up == null)
                            {
                                //g.DrawLine(Pens.Black, x1, y1, x2, y1);
                                if(x >= x1 && x <= x2 && y >= y1 && y <= y1)
                                {
                                    MessageBox.Show("Zid!!");
                                    refreshPicture();
                                    return;
                                }
                                 
                            }

                            if (!cell.IsLinked(cell.Down))
                            {
                                if (x >= x1 && x <= x2 && y >= y2 && y <= y2)
                                {
                                    MessageBox.Show("Zid!!");
                                    refreshPicture();
                                    return;
                                }
                            }

                            if (cell.Left == null)
                            {
                                if (x >= x1 && x <= x1 && y >= y1 && y <= y2)
                                {
                                    MessageBox.Show("Zid!!");
                                    refreshPicture();
                                    return;
                                }
                            }

                            if (!cell.IsLinked(cell.Right))
                            {
                                if (x >= x2 && x <= x2 && y >= y1 && y <= y2)
                                {
                                    MessageBox.Show("Zid!!");
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
                        MessageBox.Show("Pobjeda!!");
                    }
                }

            }

        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            lastPoint = Point.Empty;

            if (checkEnd(e))
            {
                MessageBox.Show("Pobjeda!!");
            }
            refreshPicture();
        }

        private Boolean checkEnd(MouseEventArgs e)
        {
            Point st = new Point(endingPoint.X - pictureBox1.Location.X, endingPoint.Y - pictureBox1.Location.Y);
            if (e.Location.X >= st.X - cellSize / 2 && e.Location.X <= st.X + cellSize / 2 && e.Location.Y >= st.Y - cellSize / 2 && e.Location.Y <= st.Y + cellSize / 2)
            {
                isMouseDown = false;
                lastPoint = Point.Empty;
                return true;
            }

            return false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isMouseDown) return;

            if (e.KeyCode == Keys.Up)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Up) || (y - 1) < 0)
                {
                    MessageBox.Show("Zid!!");
                    return;
                }
                if (visited.Contains(labirint[y-1, x]))
                    return;

                y--;

                ProccessKeyDown();
            }

            if (e.KeyCode == Keys.Down)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Down) || (y + 1) > cellSize)
                {
                    MessageBox.Show("Zid!!");
                    return;
                }
                if (visited.Contains(labirint[y + 1, x]))
                    return;

                y++;

                ProccessKeyDown();
            }

            if (e.KeyCode == Keys.Left)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Left) || (x - 1) < 0)
                {
                    MessageBox.Show("Zid!!");
                    return;
                }
                if (visited.Contains(labirint[y, x-1]))
                    return;

                x--;

                ProccessKeyDown();
            }

            if (e.KeyCode == Keys.Right)
            {
                if (!labirint[y, x].IsLinked(labirint[y, x].Right) || (x+1) > cellSize)
                {
                    MessageBox.Show("Zid!!");
                    return;
                }
                if (visited.Contains(labirint[y, x+1]))
                    return;

                x++;

                ProccessKeyDown();
            }

        }
        private void ProccessKeyDown()
        {
            var start = labirint[0, 0];
            var distances = start.Distances;
            labirint.Path = distances.PathTo(labirint[y, x]);

            Image img = labirint.ToImg(cellSize, false);
            pictureBox1.Image = img;
            isKeyPressed = true;
            visited.Add(labirint[y, x]);
            if (y == (labirint.Rows - 1) && x == 0)
            {
                MessageBox.Show("Pobjeda");
                x = 0;
                y = 0;
                isKeyPressed = false;
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("tuuu");
            if(isKeyPressed)
            {
                var start = labirint[y, x];
                var distances = start.Distances;
                labirint.Path = distances.PathTo(labirint[labirint.Rows - 1, 0]);

                Image img = labirint.ToImg(cellSize, true);
                pictureBox1.Image = img;

                timer1.Start();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var start = labirint[0,0];
            var distances = start.Distances;
            labirint.Path = distances.PathTo(labirint[y, x]);

            Image img = labirint.ToImg(cellSize, false);
            pictureBox1.Image = img;
            timer1.Stop();
        }
    }
}
