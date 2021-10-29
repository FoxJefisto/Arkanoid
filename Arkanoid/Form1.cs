using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arkanoid
{
    public partial class Form1 : Form
    {
        const int mapWidth = 40;
        const int mapHeight = 30;
        const int step = 20;
        public int[,] map = new int[mapHeight, mapWidth];
        Platform platform;
        Ball ball;
        Blocks blocks;
        public Image image = new Bitmap("D:\\VisualProjects\\C#\\Games\\Arkanoid\\Arkanoid\\images\\GameDetails.png");
        bool play;
        public Form1()
        {
            InitializeComponent();

            timer1.Tick += new EventHandler(update);
            this.KeyDown += new KeyEventHandler(inputCheck);

            Init();
        }

        private void inputCheck(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    platform.MoveRight();
                    if (!play)
                    ball.MoveWithPlatform();
                    break;
                case Keys.Left:
                    platform.MoveLeft();
                    if (!play)
                    ball.MoveWithPlatform();
                    break;
                case Keys.Space:
                    ball.dx = 1;
                    ball.dy = -1;
                    play = true;
                    break;
            }
        }

        private void update(object sender, EventArgs e)
        {
            ball.Move();
            Invalidate();
        }

        public void Init()
        {
            BackColor = Color.FromArgb(70,92,141);
            Width = mapWidth * step + 15;
            Height = mapHeight * step + 40;
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = 0;
                }
            }
            platform = new Platform(map);
            blocks = new Blocks(map);
            ball = new Ball(map, platform);
            
            
            timer1.Interval = 50;
            timer1.Start();
        }

        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (map[i, j] == 9)
                    {
                        g.DrawImage(image, new Rectangle(new Point((j-1)*step, i*step), new Size(step * 3, 15)), 426, 398, 104, 22, GraphicsUnit.Pixel);
                    }
                    if (map[i,j] == 8)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step, step)), 195, 413, 23, 23, GraphicsUnit.Pixel);
                    }
                    if (map[i,j] == 1)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step * 2, step)), blocks.imageX, blocks.imageY, blocks.width, blocks.height, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 2)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step * 2, step)), blocks.imageX, blocks.imageY + 52 * (map[i,j] - 1), blocks.width, blocks.height, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 3)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step * 2, step)), blocks.imageX, blocks.imageY + 52 * (map[i, j] - 1), blocks.width, blocks.height, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 4)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step * 2, step)), blocks.imageX, blocks.imageY + 52 * (map[i, j] - 1), blocks.width, blocks.height, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 5)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step * 2, step)), blocks.imageX, blocks.imageY + 52 * (map[i, j] - 1), blocks.width, blocks.height, GraphicsUnit.Pixel);
                    }
                    if (map[i, j] == 6)
                    {
                        g.DrawImage(image, new Rectangle(new Point(j * step, i * step), new Size(step * 2, step)), blocks.imageX, blocks.imageY + 52 * (map[i, j] - 1), blocks.width, blocks.height, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawMap(e.Graphics);
        }
    }

    class Platform
    {
        
        public int x;
        public int y;
        private int[,] map;

        public Platform(int[,] map)
        {
            this.map = map;
            x = map.GetLength(1)/2;
            y = map.GetLength(0)-5;
            map[y, x] = 99;
            map[y, x + 1] = 9;
            map[y, x + 2] = 99;
        }

        public void MoveRight()
        {
            if (x + 1 < map.GetLength(1) - 2)
            {
                map[y, x] = 0;
                map[y, x + 1] = 99;
                map[y, x + 2] = 9;
                x++;
                map[y, x + 2] = 99;
            }
        }

        public void MoveLeft()
        {
            if (x > 0)
            {
                map[y, x] = 9;
                map[y, x + 1] = 99;
                map[y, x + 2] = 0;
                x--;
                map[y, x] = 99;
            }
        }
    }

    class Ball
    {
        public int x;
        public int y;
        public int dx;
        public int dy;
        public int[,] map;
        public Platform platform;
        public Ball(int[,] map, Platform platform)
        {
            this.platform = platform;
            this.map = map;
            y = platform.y - 1;
            x = platform.x + 1;
            map[y, x] = 8;
            dx = 0;
            dy = 0;
        }

        public void MoveWithPlatform()
        {
            map[y, x] = 0;
            y = platform.y - 1;
            x = platform.x + 1;
        }

        public void Move()
        {
            if (x + dx >= map.GetLength(1) || x + dx < 0)
            {
                dx *= -1;
            }
            if (y + dy < 0 || y + dy >= map.GetLength(0))
            {
                dy *= -1;
            }
            if(map[y+dy,x] != 0)
            {
                if(map[y+dy, x] >= 10 && map[y+dy, x] <= 60)
                {
                    map[y + dy, x] = 0;
                    map[y + dy, x - 1] = 0;
                }
                else if(map[y+dy, x] >= 1 && map[y+dy,x] <= 6)
                {
                    map[y + dy, x] = 0;
                    map[y + dy, x + 1] = 0;
                }
                dy *= -1;
            }
            map[y, x] = 0;
            x += dx;
            y += dy;
            map[y, x] = 8;
        }
    }

    class Blocks
    {
        public int[,] map;
        public int imageX = 486;
        public int imageY = 49;
        public int width = 63;
        public int height = 31;

        public Blocks(int[,] map)
        {
            this.map = map;
            Random r = new Random();
            for(int i = 0; i < map.GetLength(0) / 3; i++)
            {
                for(int j = 0; j < map.GetLength(1) - 1; j+=2)
                {
                    map[i, j] = r.Next(1,7);
                    map[i, j + 1] = map[i,j] * 10;
                }
            }
        }
    }
}
