using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labirint
{
    public static class ColorExtensions
    {
        public static Color Scale(this Color color, float intensity)
        {
            Console.WriteLine("{0}", color.R);
            return Color.FromArgb(
                    (int)(color.R + (255 - color.R) * intensity),
                    (int)(color.G + (255 - color.G) * intensity),
                    (int)(color.B + (255 - color.B) * intensity)
                    );
        }

    }
}
