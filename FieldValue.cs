using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PixelTracing.VertexExtractor;

namespace PixelTracing
{
    public class FieldValue
    {
        public Direction[] Directions { get; private set; }
        public FieldValue()
        {
            Directions = new Direction[8];
        }
    }
}
