using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labirint
{
    public class ColoredGrid : Grid
    {

        public ColoredGrid(int rows, int columns): base(rows, columns)
        {
            BackColor = Color.Green;
        }

        public Color BackColor { get; set; }

    }
}
