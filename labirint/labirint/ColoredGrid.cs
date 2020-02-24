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
        private Distances _distances;
        public Cell _farthest;
        private int _maximum;

        public ColoredGrid(int rows, int columns): base(rows, columns)
        {
            BackColor = Color.Green;
        }

        public Color BackColor { get; set; }

        public Distances Distances
        {
            get => _distances;
            set
            {
                _distances = value;
                (_farthest, _maximum) = value.Max;
            }
        }

        protected override Color? BackgroundColorFor(Cell cell)
        {
            if (Distances == null || Distances[cell] < 0)
            {
                return null;
            }
            var distance = Distances[cell];
            var intensity = (_maximum - distance) / (float)_maximum;

            return BackColor.Scale(intensity);
        }

}
}
