using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Media;


namespace NLO_killer
{
    public partial class MainWindow : Window
    {
        Random rnd; // генератор случайных чисел

        int N = 10; // количество шаров
        double R = 50; // диаметр шаров

        Image[] Balls; // массив шаров

        ThicknessAnimation[] Animations; // анимация для каждого шара

        Ellipse S; // лопающийся НЛО
        DoubleAnimation[] CrackAnimationsW; // анимация для лопающегося шара
        DoubleAnimation[] CrackAnimationsH;

        // Список хранения путей картинок НЛО
        List <string> Images = new List<string>
        {
            "../../../Images/nlo1.png", "../../../Images/nlo2.png", "../../../Images/nlo3.png", "../../../Images/nlo4.png"
        };

        List<Ellipse> bullets = new List<Ellipse> { }; // Список снарядов
        ThicknessAnimation bulletAnime; // анимация для снаряда

        System.Windows.Threading.DispatcherTimer timerCrush; // Таймер отслеживания столкновений
        System.Windows.Threading.DispatcherTimer timerShoting; // Таймер интервальной стрельбы

        Gun gun; // объект Пушка
        int x1, y1; // Первые координаты линии пушки
        int gunAngleDec; // Наклон пушки в градусах
        double gunAngleRad; // Наклон пуски в радианах

        SoundPlayer shotSound = new SoundPlayer(@"shot.wav");

        MediaPlayer bomPlayer = new MediaPlayer();
        MediaPlayer themePlayer = new MediaPlayer();

        int score = 0; // очки игры

        // Инициализация
        public MainWindow()
        {
            InitializeComponent();

            rnd = new Random();
            //Balls = new Image[N]; // создаем массив НЛО

            //Animations = new ThicknessAnimation[N]; // создаем объекты анимации
            CrackAnimationsW = new DoubleAnimation[N];
            CrackAnimationsH = new DoubleAnimation[N];

            bulletAnime = new ThicknessAnimation(); // создаем объекты анимации
            
            // Создаем таймер для отслеживания столкновения НЛО и снаряда
            timerCrush = new System.Windows.Threading.DispatcherTimer();
            timerCrush.Tick += new EventHandler(timerTick);
            timerCrush.Interval = new TimeSpan(0, 0, 0, 0, 100);

            // Создаем таймер для интервальной стрельбы
            timerShoting = new System.Windows.Threading.DispatcherTimer();
            timerShoting.Tick += new EventHandler(timerTickShoting);
            timerShoting.Interval = new TimeSpan(0, 0, 0, 0, 200);


            gunAngleDec = 90; // предустановка угла наклона пушки 90 градусов
            gunAngleRad = gunAngleDec * (Math.PI) / 180;    // предустановка угла наклона пушки в радианах
            x1 = (int)g.Width / 2;  // предустановка первой координаты Х для пушки
            y1 = (int)g.Height - 30;  // предустановка первой координаты Y для пушки

            bomPlayer.Open(new Uri(Environment.CurrentDirectory + "\\bom.wav"));
            themePlayer.Open(new Uri(Environment.CurrentDirectory + "\\theme.mp3"));
            themePlayer.MediaEnded += themePlayerRepeat;
            themePlayer.Play();

        }

        // Бесконечное проигрывание музыкальной темы
        void themePlayerRepeat(object sender, EventArgs e)
        {
            themePlayer.Stop();
            themePlayer.Play();
        }

        // Таймер отслеживания столкновений
        void timerTick(object sender, EventArgs e)
        {
            if (Balls == null) return; // Если НЛО нет, то выходим
            //if (Balls[0] == null) return; // Если НЛО нет, то выходим

            for (int j = 0; j < Balls.Length; j++) // Цикл НЛО
            {
                for (int i = 0; i < bullets.Count; i++) // Цикл снарядов
                {
                    if (((Math.Abs((double)bullets[i].Margin.Left - Balls[j].Margin.Left) < 50) && (Math.Abs((double)bullets[i].Margin.Left - (Balls[j].Margin.Left + Balls[j].Width)) < 50)
                        && (Math.Abs((double)bullets[i].Margin.Top - Balls[j].Margin.Top) < 50) && (Math.Abs((double)bullets[i].Margin.Top - (Balls[j].Margin.Top + Balls[j].Height)) < 50)))
                    {
                        bomPlayer.Stop(); // звук лопающегося шарика
                        bomPlayer.Play();

                        bullets.RemoveAt(i); 
                        Crack(j); // лопаем НЛО

                        lblScore.Content = ++score; // увеличиваем очки игры и выводим на экран

                        // Конструкция для удаления элемента массива.
                        // Происходит временная конвертация в List, удаление и обратно в массив
                        List<Image> temp = Balls.ToList();
                        temp.RemoveAt(j);
                        Balls = temp.ToArray();
                        
                        return;
                    }
                }
            }  // Цикл НЛО

            // Плавно убираем надпись с номером уровня
            if (tbGameInfo.Opacity > 0.0) tbGameInfo.Opacity -= 0.1;
                else tbGameInfo.Opacity = 0.0;

        }

        // Таймер интервальных выстрелов
        void timerTickShoting(object sender, EventArgs e)
        {
            CreateBullet();
            shotSound.Play(); // звук выстрела

        }

            // Создание НЛО
            void CreateUFO(int n, Brush br)
        {
            g.Children.Remove(Balls[n]); // удаляем НЛО, если он уже был создан
            

            Balls[n] = new Image(); // создаем объект НЛО
            // В рандоме верхнюю границу нужно делать на 1 больше (чтобы выпадал 3, граница должна быть 4)
            Balls[n].Source = new BitmapImage(new Uri(Images[rnd.Next(0, 4)], UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }; // рандомно выбираем картинку
            Balls[n].Width = R; // ширина
            Balls[n].Height = R; // высота

            double x = 0; // горизонтальная координата

            // вертикальная координата - случайное значение
            double y = rnd.NextDouble() * (g.Height - 2 * R);
            
            Balls[n].Margin = new Thickness(x, y, 0, 0);    // задаем положение НЛО

            int v = rnd.Next(5, 20); // выбираем случайную скорость

            Animations[n] = new ThicknessAnimation
            {
                From = new Thickness(0, y, 0, 0), // начальное положение
                To = new Thickness(g.Width - R, y, 0, 0), // конечное положение
                Duration = TimeSpan.FromSeconds(v), // время все анимации

                AutoReverse = true, // после окончания - реверс
                RepeatBehavior = new RepeatBehavior(10) // повторять 10 раз
            };
            
            Balls[n].BeginAnimation(Ellipse.MarginProperty, Animations[n]); // добавить анимацию

            g.Children.Add(Balls[n]); // размещаем НЛО на холсте
        }

        // Создание и отрисовка снаряда
        void CreateBullet()
        {
            
            Ellipse bul = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            bullets.Add(bul);

            bul.Margin = new Thickness(x1, y1, 0, 0); // Назначаем координаты снаряда относительно координат конца пушки


            int xxx = (int)(y1 / Math.Tan(gun.angle));
            int x = x1 - xxx;
            int y = 0;

            // снаряд падает вверх в пределах окна, от 0 до конца экрана
            if (((x1 - xxx) > 0 && gunAngleDec <= 90) || ((x1 - xxx) < 0 && gunAngleDec > 90))
            {
                x = x1 - xxx; y = 0;
            }
            else
            // снаряд падает за пределы левого края экрана
            // пересчитывается y, чтобы снаряд не улетал дальше левого края, а исчезал при достижении края
            if ((x1 - xxx) < 0 && gunAngleDec <= 90)
            {
                x = 0; y = (int)(Math.Abs(x1 - xxx) * Math.Tan(gun.angle));
            }
            else
            // снаряд падает за пределя правого края экрана
            // пересчитывается y, чтобы снаряд не улетал дальше правого края, а исчезал при достижении края
            if ((x1 - xxx) > g.Width && gunAngleDec > 90)
            {
                x = (int)g.Width; y = (int)(Math.Abs(x1 + Math.Abs(xxx) - g.Width) * -1 * Math.Tan(gun.angle));
            }

            bulletAnime = new ThicknessAnimation();
            bulletAnime.From = new Thickness(x1 - 5, y1 - 5, 0, 0); // начальное положение
            bulletAnime.To = new Thickness(x, y, 0, 0); // конечное положение
            bulletAnime.Duration = TimeSpan.FromSeconds(1); // время все анимации
            bulletAnime.RepeatBehavior = new RepeatBehavior(1); // повторять 10 раз
            bulletAnime.Completed += BulletAnimeOff;

            bul.BeginAnimation(Ellipse.MarginProperty, bulletAnime);    // добавить анимацию

            g.Children.Add(bul); // размещаем шар на холсте

            if (!timerCrush.IsEnabled) timerCrush.Start(); // Запуск таймера отслеживания столкновений
        }

        // Эффект лопающегося НЛО при попадании снаряда
        void Crack(int n)
        {
            g.Children.Remove(S); // удаляем шар, если он уже есть

            S = new Ellipse(); // создаем лопающийся шар
            S.Stroke = Brushes.Gray; // цвет
            S.StrokeThickness = 3; // толщина линии
            S.StrokeDashArray = new DoubleCollection(new double[] { 4, 2 }); // текстура линии
            S.Margin = Balls[n].Margin; // сохраняем положение
            S.Width = 1; // в начале диаметр минимальный
            S.Height = 1;

            g.Children.Remove(Balls[n]); // удаляем исходный шар
            g.Children.Add(S); // создаем лопающийся шар

            // создаем анимацию
            CrackAnimationsW[n] = new DoubleAnimation();
            CrackAnimationsW[n].From = R; // начальный радиус
            CrackAnimationsW[n].To = R + 10; // конечный
            CrackAnimationsW[n].Duration = TimeSpan.FromMilliseconds(100); // скорость лопания
            CrackAnimationsW[n].Completed += CrackAnimation_Completed; // что делать после анимации

            CrackAnimationsH[n] = new DoubleAnimation();
            CrackAnimationsH[n].From = R;
            CrackAnimationsH[n].To = R + 10;
            CrackAnimationsH[n].Duration = TimeSpan.FromMilliseconds(100);

            // запустить анимацию
            S.BeginAnimation(Ellipse.WidthProperty, CrackAnimationsW[n]);
            S.BeginAnimation(Ellipse.HeightProperty, CrackAnimationsH[n]);
        }

        // НЕ ИСПОЛЬЗУЕТСЯ эта функция будет вызвана автоматически, когда закончится анимация
        void CrackAnimation_Completed(object sender, EventArgs e)
        {
            g.Children.Remove(S); // удалить лопающийся шар
        }

        // Отрисовка пушки
        void DrawGun()
        {
            g.Children.Remove(g.Children.OfType<Line>().FirstOrDefault()); // Удаляем линию - пушку

            gun = new Gun(x1, y1, (int)g.Width / 2, (int)g.Height); // Создаем новый объект линию - пушку
            gun.angle = gunAngleDec;    // Назначаем новой пушке новый угол из значения глобальной переменной

            gun.X1 = x1 = (int)(g.Width / 2 - Math.Cos(gun.angleRad) * 30); // Высчитываем угол наклон пушки по новому углу
            gun.Y1 = y1 = (int)(g.Height - Math.Sin(gun.angleRad) * 30);    // Высчитываем угол наклон пушки по новому углу

            g.Children.Add(gun.rect); // размещаем пушку на холсте
        }

        // Функция, вызываемая после завершения анимации снаряда
        void BulletAnimeOff(object sender, EventArgs e)
        {
            g.Children.Remove(g.Children.OfType<Ellipse>().FirstOrDefault()); // Удаляем снаряд
        }


        // *****************************************************************************************
        // ************* События мыши, кнопок, окна ************************************************
        // *****************************************************************************************

        // Измерение положения пушки относительно положения курсора мышки
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(g).X; // Получаем координату мышки Х
            double y = e.GetPosition(g).Y; // Получаем координату мышки Y

            double X = (g.Width / 2 - x);   // нижняя точка пушки
            double Y = (g.Height - y);      // нижняя точка пушки

            double rad = Math.Atan2(Y , X);
            gunAngleDec = (int)(rad * 180 / Math.PI);   // Обновляем глобальную переменную угла наклона

            if (gunAngleDec <= 0) gunAngleDec = 0;
            if (gunAngleDec >= 180) gunAngleDec = 180;

            lblangle.Content = gunAngleDec.ToString();
            DrawGun();  // Отрисовка пушки
        }

        // Выстрел при нажатии кнопки мыши
        void Shot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateBullet();
            shotSound.Play(); // звук выстрела
            timerShoting.Start();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            timerShoting.Stop();
        }

        // Старт Игры
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            lblScore.Content = score = 0; // обнуление счета

            tbGameInfo.Opacity = 1;

            // ++++++++++++++++++++++++++++++++++++++++
            // +++++++++++ Создание НЛО
            // ++++++++++++++++++++++++++++++++++++++++
            Balls = new Image[N]; // создаем массив НЛО
            Animations = new ThicknessAnimation[N]; // создаем объекты анимации
            g.Children.Clear();

            for (int n = 0; n < N; n++)
                CreateUFO(n, Brushes.Blue); // в цикле создаем и отрисовываем НЛО
            
            // ++++++++++++++++++++++++++++++++++++++++
            // +++++++++++ Создание пушки
            // ++++++++++++++++++++++++++++++++++++++++
            DrawGun(); // Отрисовка Пушки
        }
       
    }
}
