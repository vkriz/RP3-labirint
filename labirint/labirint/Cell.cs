using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labirint
{
    public class Cell
    {
        public int Row { get; }
        public int Column { get; }
        public Cell Up { get; set; }
        public Cell Down { get; set; }
        public Cell Left { get; set; }
        public Cell Right { get; set; }

        public Distances Distances
        {
            get
            {
                var distances = new Distances(this);
                var frontier = new HashSet<Cell>
                {
                    this
                };

                while (frontier.Any())
                {
                    var newFrontier = new HashSet<Cell>();

                    foreach (var cell in frontier)
                    {
                        foreach (var linked in cell.Links)
                        {
                            if (distances[linked] >= 0)
                            {
                                continue;
                            }
                            distances[linked] = distances[cell] + 1;
                            newFrontier.Add(linked);
                        }
                    }
                    frontier = newFrontier;
                }
                return distances;
            }
        }

        // neposredni susjedi
        public List<Cell> Neighbours
        {
            get { return new[] { Up, Down, Left, Right }.Where(c => c != null).ToList(); }
        }

        // polja koja su povezana s ovim poljem (moze se doci do njega nekim putem)
        private readonly Dictionary<Cell, bool> _links;
        public List<Cell> Links => _links.Keys.ToList();

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            _links = new Dictionary<Cell, bool>();
        }

        public void Link(Cell cell, bool bidirectional = true)
        {
            _links[cell] = true;
            if (bidirectional)
            {
                cell.Link(this, false);
            }
        }

        public void Unlink(Cell cell, bool bidirectional = true)
        {
            _links.Remove(cell);
            if (bidirectional)
            {
                cell.Unlink(this, false);
            }
        }

        public bool IsLinked(Cell cell)
        {
            if (cell == null)
            {
                return false;
            }

            return _links.ContainsKey(cell);
        }
    }
}