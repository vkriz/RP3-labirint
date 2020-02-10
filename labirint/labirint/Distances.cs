using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labirint
{
    public class Distances
    {
        private Cell Root { get; }
        private readonly Dictionary<Cell, int> _cells;
        public List<Cell> Cells => _cells.Keys.ToList();

        public Distances(Cell root)
        {
            Root = root;
            _cells = new Dictionary<Cell, int> { { Root, 0 } };
        }

        public (Cell maxCell, int maxDistance) Max
        {
            get
            {
                var maxDistance = 0;
                var maxCell = Root;
                foreach(var cell in _cells)
                {
                    if(cell.Value > maxDistance)
                    {
                        maxDistance = cell.Value;
                        maxCell = cell.Key;
                    }
                }
                return (maxCell, maxDistance);
            }
        }

        public int this[Cell cell]
        {
            get
            {
                if (_cells.ContainsKey(cell))
                {
                    return _cells[cell];
                }
                return -1;
            }
            set => _cells[cell] = value;
        }
    }
}
