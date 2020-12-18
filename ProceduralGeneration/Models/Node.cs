using System.Drawing;

namespace ProceduralGeneration
{
    class Node
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public float Elevation { get; set; }
        public float Moisture { get; set; }
        public Color Biome => GetColor();

        public Node(int posX, int posY, float elevation, float moisture, int width = 1, int height = 1)
        {
            this.Location = new Point(posX, posY);
            this.Size = new Size(width, height);
            this.Elevation = elevation;
            this.Moisture = moisture;
        }

        private Color GetColor()
        {
            if (Elevation < 5)
            {
                return Color.FromArgb(68, 69, 117);
            }
            if (Elevation < 10)
            {
                return Color.FromArgb(160, 144, 128);

            }
            if (Elevation < 20)
            {
                return Color.FromArgb(136, 170, 84);
            }
            if (Elevation < 40)
            {
                return Color.FromArgb(85, 153, 68);
            }
            if (Elevation < 60)
            {
                return Color.FromArgb(52, 119, 84);
            }
            if (Elevation < 80)
            {
                return Color.FromArgb(136, 153, 119);
            }
            else
            {
                return Color.FromArgb(137, 137, 137);
            }
        }
    }
}
