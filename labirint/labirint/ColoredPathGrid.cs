using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labirint
{
    public class ColoredPathGrid : ColoredGrid
    {
        public ColoredPathGrid(int rows, int cols) : base(rows, cols) { }

        private Distances _path;
        private Cell _end;
        private int _maxDistance;

        public Distances Path
        {
            get => _path;
            set
            {
                _path = value;
                (_end, _maxDistance) = value.Max;
            }
        }
        public int PathLength => _maxDistance + 1;

        protected override bool DrawPath(Cell cell, Graphics g, int cellSize)
        {
            if (Path == null)
            {
                return false;
            }

            var thisDistance = Path[cell];
            if (thisDistance >= 0)
            {

                var x1 = cell.Column * cellSize;
                var y1 = cell.Row * cellSize;
                var x2 = (cell.Column + 1) * cellSize;
                var y2 = (cell.Row + 1) * cellSize;
                var cx = cell.Column * cellSize + cellSize / 2;
                var cy = cell.Row * cellSize + cellSize / 2;
                using (var pen = new Pen(Color.Red, 2))
                {
                    if (cell.Up != null && (Path[cell.Up] == thisDistance + 1 || Path[cell.Up] == thisDistance - 1 && thisDistance != 0))
                    {
                        g.DrawLine(pen, cx, cy, cx, y1);
                    }
                    if (cell.Down != null && (Path[cell.Down] == thisDistance + 1 || Path[cell.Down] == thisDistance - 1 && thisDistance != 0))
                    {
                        g.DrawLine(pen, cx, cy, cx, y2);
                    }
                    if (cell.Right != null && (Path[cell.Right] == thisDistance + 1 || Path[cell.Right] == thisDistance - 1 && thisDistance != 0))
                    {
                        g.DrawLine(pen, cx, cy, x2, cy);
                    }
                    if (cell.Left != null && (Path[cell.Left] == thisDistance + 1 || Path[cell.Left] == thisDistance - 1 && thisDistance != 0))
                    {
                        g.DrawLine(pen, cx, cy, x1, cy);
                    }

                    if (thisDistance == 0)
                    {
                        g.DrawRectangle(pen, cx - 2, cy - 2, 4, 4);

                    }
                    if (thisDistance == _maxDistance)
                    {
                        g.DrawLine(pen, cx - 4, cy - 4, cx + 4, cy + 4);
                        g.DrawLine(pen, cx + 4, cy - 4, cx - 4, cy + 4);
                    }
                }

                return true;
            }

            return false;
        }
    }
}