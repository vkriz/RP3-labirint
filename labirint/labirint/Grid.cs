using System;
using System.Collections.Generic;
using System.Drawing;

namespace labirint
{
    public class Grid
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Size => Rows * Columns;
        private List<List<Cell>> _grid;

        private enum DrawMode
        {
            Background, Walls
        }

        protected virtual Color? BackgroundColorFor(Cell cell)
        {
            return null;
        }

        public virtual Cell this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= Rows)
                {
                    return null;
                }
                if(column < 0 || column >= Columns)
                {
                    return null;
                }
                return _grid[row][column];
            }
        }

        public Cell RandomCell()
        {
            var rand = new Random();
            var row = rand.Next(Rows);
            var column = rand.Next(Columns);
            var randomCell = this[row, column];

            if(randomCell == null)
            {
                throw new InvalidOperationException("Random cell is null.");
            }
            return randomCell;
        }

        public IEnumerable<List<Cell>> Row
        {
            get
            {
                foreach(var row in _grid)
                {
                    yield return row;
                }
            }
        }

        public IEnumerable<Cell> Cells
        {
            get
            {
                foreach(var row in Row)
                {
                    foreach(var cell in row)
                    {
                        yield return cell;
                    }
                }
            }
        }

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid()
        {
            _grid = new List<List<Cell>>();
            for(var r = 0; r < Rows; ++r)
            {
                var row = new List<Cell>();
                for(var c = 0; c < Columns; ++c)
                {
                    row.Add(new Cell(r, c));
                }
                _grid.Add(row);
            }
        }

        private void ConfigureCells()
        {
            foreach(var cell in Cells)
            {
                var row = cell.Row;
                var column = cell.Column;

                cell.Up = this[row - 1, column];
                cell.Down = this[row + 1, column];
                cell.Left = this[row, column - 1];
                cell.Right = this[row, column + 1];
            }
            var rand = new Random();
            var randAlgorithm = rand.Next(2);
            if(randAlgorithm == 0)
            {
                BinaryTree();
            }
            else
            {
                SideWinder();
            }
        }

        public void BinaryTree()
        {
            var rand = new Random();
            foreach (var cell in Cells)
            {
                var neighbours = new List<Cell>();
                if (cell.Up != null) neighbours.Add(cell.Up);
                if (cell.Right != null) neighbours.Add(cell.Right);

                if (neighbours.Count == 0)
                {
                    continue;
                }
                var randNum = rand.Next(neighbours.Count);
                var neighbour = neighbours[randNum];
                if(neighbour != null)
                {
                    cell.Link(neighbour);
                }
            }
        }

        public void SideWinder ()
        {
            var rand = new Random();
            foreach(var row in Row)
            {
                var run = new List<Cell>();
                foreach(var cell in row)
                {
                    run.Add(cell);

                    var atRightEdge = cell.Right == null;
                    var atUpEdge = cell.Up == null;
                    var shouldCloseOut = atRightEdge || (!atUpEdge && rand.Next(2) == 0);
                    if (shouldCloseOut)
                    {
                        var member = run[rand.Next(run.Count)];
                        if(member.Up != null)
                        {
                            member.Link(member.Up);
                        }
                        run.Clear();
                    }
                    else
                    {
                        cell.Link(cell.Right);
                    }
                }
            }
        }

        public Image ToImg(int cellSize)
        {
            var width = cellSize * Columns;
            var height = cellSize * Rows;

            var img = new Bitmap(width + 1, height + 1);
            using(var g = Graphics.FromImage(img))
            {
                g.Clear(Color.White);
                foreach (var mode in new[] { DrawMode.Background, DrawMode.Walls })
                {
                    foreach (var cell in Cells)
                    {
                        var x1 = cellSize * cell.Column;
                        var x2 = cellSize * (cell.Column + 1);
                        var y1 = cellSize * cell.Row;
                        var y2 = cellSize * (cell.Row + 1);

                        if(mode == DrawMode.Background)
                        {
                            var color = BackgroundColorFor(cell);
                            if(color != null)
                            {
                                Console.WriteLine("trebao bi");
                                g.FillRectangle(new SolidBrush(color.GetValueOrDefault()), x1, y1, cellSize, cellSize);
                            }
                        } else if (mode == DrawMode.Walls)
                        {
                            if (cell.Up == null)
                            {
                                g.DrawLine(Pens.Black, x1, y1, x2, y1);
                            }

                            if (!cell.IsLinked(cell.Down))
                            {
                                g.DrawLine(Pens.Black, x1, y2, x2, y2);
                                Console.WriteLine("{0} {1} {2} {3}", x1, y2, x2, y2);
                            }

                            if (cell.Left == null)
                            {
                                g.DrawLine(Pens.Black, x1, y1, x1, y2);
                            }

                            if (!cell.IsLinked(cell.Right))
                            {
                                g.DrawLine(Pens.Black, x2, y1, x2, y2);
                            }
                        }
                    }
                }
            }
            return img;
        }
    }
}
