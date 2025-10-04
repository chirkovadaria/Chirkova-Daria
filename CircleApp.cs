using System;
using System.Drawing;
using System.Windows.Forms;

namespace CircleApp
{
    public partial class MainForm : Form
    {
        private int radius = 100; // Начальный радиус окружности
        private const int MIN_RADIUS = 1; // Минимальный радиус
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Настройка главного окна
            this.Text = "Программа с окружностью - Используйте клавиши влево/вправо";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true; // Позволяет форме получать события клавиш
            this.BackColor = Color.White;
            
            // Включение двойной буферизации для плавной анимации
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.UserPaint | 
                         ControlStyles.DoubleBuffer, true);
            
            // Обработчики событий
            this.Paint += MainForm_Paint;
            this.KeyDown += MainForm_KeyDown;
            this.Resize += MainForm_Resize;
            
            this.ResumeLayout(false);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            // Расчет центра окна
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            
            // Расчет координат прямоугольника, в который вписана окружность
            // Для окружности: левый верхний угол = (centerX - radius, centerY - radius)
            // размер = (2 * radius, 2 * radius)
            int x = centerX - radius;
            int y = centerY - radius;
            int diameter = 2 * radius;
            
            // Создание прямоугольника для окружности
            Rectangle circleRect = new Rectangle(x, y, diameter, diameter);
            
            // Рисование окружности синим цветом с толщиной линии 2
            using (Pen pen = new Pen(Color.Blue, 2))
            {
                g.DrawEllipse(pen, circleRect);
            }
            
            // Отображение текущего радиуса в левом верхнем углу
            string radiusText = $"Радиус: {radius}";
            using (Font font = new Font("Arial", 12))
            {
                g.DrawString(radiusText, font, Brushes.Black, new PointF(10, 10));
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            bool radiusChanged = false;
            
            // Максимальный радиус - половина ширины окна
            int maxRadius = this.ClientSize.Width / 2;
            
            switch (e.KeyCode)
            {
                case Keys.Right:
                    // Увеличение радиуса (клавиша вправо)
                    if (radius < maxRadius)
                    {
                        radius++;
                        radiusChanged = true;
                    }
                    break;
                    
                case Keys.Left:
                    // Уменьшение радиуса (клавиша влево)
                    if (radius > MIN_RADIUS)
                    {
                        radius--;
                        radiusChanged = true;
                    }
                    break;
            }
            
            // Перерисовка окна при изменении радиуса
            if (radiusChanged)
            {
                this.Invalidate(); // Запуск перерисовки
            }
        }
        
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // При изменении размера окна проверяем, не превышает ли радиус новый максимум
            int maxRadius = this.ClientSize.Width / 2;
            if (radius > maxRadius)
            {
                radius = maxRadius;
            }
            
            this.Invalidate(); // Перерисовка при изменении размера окна
        }
    }
    
    // Главный класс программы
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Создание и запуск главной формы
            using (MainForm mainForm = new MainForm())
            {
                Application.Run(mainForm);
            }
        }
    }
}