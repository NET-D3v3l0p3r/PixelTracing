using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PixelTracing
{
    public class VertexExtractor
    {

        public bool this[int x, int y]
        {
            get { return PixelData[x + y * Width]; }
            set { PixelData[x + y * Width] = value; }
        }
        public bool[] PixelData { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }


        public VertexExtractor(int w, int h)
        {
            Width = w;
            Height = h;

            PixelData = new bool[Width * Height];
        }

        public static unsafe VertexExtractor GetFromThinnedBinaryImage(string path)
        {
            VertexExtractor v = null;
            Bitmap image = new Bitmap(path);
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            v = new VertexExtractor(data.Width, data.Height);

            byte[] pixels = new byte[data.Width * data.Height * 4];
            Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
            image.UnlockBits(data);


            for (int i = 0; i < pixels.Length; i += 4)
                v.PixelData[i / 4] = pixels[i] == 255;


            return v;
        }

        public FieldValue[] GetField()
        {
            FieldValue[] field = new FieldValue[Width * Height];

            for (int i = 0; i < PixelData.Length; i++)
            {
                if (!PixelData[i])
                    continue;

                int x = i % Width;
                int y = i / Width;
                bool[] n = GetNeighbours(x, y);


                field[i] = new FieldValue();
                for (int j = 0; j < field[i].Directions.Length; j++)
                {
                    if (!n[j])
                        continue;
                    field[i].Directions[j] = (Direction)j + 1;
                }
            }
            return field;
        }



        public IEnumerable<Segment> GetSegments()
        {

        }

        public enum Direction
        {
            Idle = 0,

            TopLeft = 1,
            TopMid = 2,
            TopRight = 3,

            MidLeft = 4,
            MidRight = 5,

            BotLeft = 6,
            BotMid = 7,
            BotRight = 8,
        }

        public bool[] GetNeighbours(int x, int y)
        {

            bool[] neighbours = new bool[8];

            if (x - 1 >= 0 && y - 1 >= 0)
                neighbours[0] = this[x - 1, y - 1];
            if (y - 1 >= 0)
                neighbours[1] = this[x - 0, y - 1];
            if (x + 1 < Width && y - 1 >= 0)
                neighbours[2] = this[x + 1, y - 1];


            if (x - 1 >= 0)
                neighbours[3] = this[x - 1, y];
            if (x + 1 < Width)
                neighbours[4] = this[x + 1, y];



            if (x - 1 >= 0 && y + 1 < Height)
                neighbours[5] = this[x - 1, y + 1];
            if (y + 1 < Height)
                neighbours[6] = this[x - 0, y + 1];
            if (x + 1 < Width && y + 1 < Height)
                neighbours[7] = this[x + 1, y + 1];

            return neighbours;
        }
    }
}