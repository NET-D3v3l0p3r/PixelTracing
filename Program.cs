using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            VertexExtractor v = VertexExtractor.GetFromThinnedBinaryImage(@"C:\users\3r0rxx\desktop\tests.png");

            Bitmap bmp = new Bitmap(v.Width * 25, v.Height * 25);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Gray);

            var field = v.GetField();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < field.Length; i++)
            {
                if (field[i] == null)
                    continue;


                int x = i % v.Width;
                int y = i / v.Width;


                for (int j = 0; j < field[i].Directions.Length; j++)
                {
                    if (field[i].Directions[j] == VertexExtractor.Direction.Idle)
                        continue;

                    string s = field[i].Directions[j] + "";
                    var t = new string(s.ToList().FindAll(p => Char.IsUpper(p)).ToArray());

                    sb.Append(t[1] + Environment.NewLine);
                }

                g.FillRectangle(Brushes.Black, new RectangleF(x * 25, y * 25, 25, 25));
                g.DrawString(sb.ToString(), new Font("Arial", 8), Brushes.Red, x * 25, y * 25);

                sb.Clear();
            }



            bmp.Save(@"C:\users\3r0rxx\desktop\processed.png", System.Drawing.Imaging.ImageFormat.Png);


            Console.WriteLine("Done.");

        }
    }
}
