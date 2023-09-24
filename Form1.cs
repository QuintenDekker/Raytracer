using System.Numerics;

namespace Raytracer
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        const int width = 300;
        const int height = 300;
        public Form1()
        {
            InitializeComponent();

            Paint += Form1_Paint;
            bitmap = new Bitmap(width,height);
            for (int i  = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bitmap.SetPixel(i, j, Color.Black);
                }
            }
            Invalidate();
            World world = new World(bitmap, this);
        }

        

        private void DrawPixel(int x, int y, Color c)
        {
            bitmap.SetPixel(x, y, c);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}