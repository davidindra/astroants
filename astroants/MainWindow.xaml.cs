using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace astroants
{
    public partial class MainWindow : Window
    {
        Thread t = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void sliderGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            amountGreen.Content = sliderGreen.Value;
        }

        private void sliderBlue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            amountBlue.Content = sliderBlue.Value;
        }

        private void sliderSteps_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            amountSteps.Content = sliderSteps.Value;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            List<string> ants = new List<string>();

            for (int i = 0; i < (int)sliderGreen.Value; i++) { ants.Add("green"); }
            for (int i = 0; i < (int)sliderBlue.Value; i++) { ants.Add("blue"); }

            t = new Thread(new ParameterizedThreadStart(movesThread));
            t.Start(new object[2] { ants, (int)sliderSteps.Value });
        }

        private void movesThread(object parameters)
        {
            disableControls();

            List<string> ants = (List<string>)((Array)parameters).GetValue(0);
            int moves = (int)((Array)parameters).GetValue(1);

            this.Dispatcher.Invoke(() =>
            {
                progress.Value = 0;
                renderAnts(ants);
            });

            for (int i = 1; i <= moves; i++)
            {
                Thread.Sleep(800);
                ants = calculateMove(ants);
                this.Dispatcher.Invoke(() =>
                {
                    progress.Value = moves == i ? 100 : 100 / moves * i;
                    renderAnts(ants);
                });
            }

            enableControls();
        }

        private void disableControls()
        {
            this.Dispatcher.Invoke(() => {
                start.IsEnabled = false;
                sliderGreen.IsEnabled = false;
                sliderBlue.IsEnabled = false;
                sliderSteps.IsEnabled = false;
            });
        }

        private void enableControls()
        {
            this.Dispatcher.Invoke(() => {
                start.IsEnabled = true;
                sliderGreen.IsEnabled = true;
                sliderBlue.IsEnabled = true;
                sliderSteps.IsEnabled = true;
            });
        }

        private List<string> calculateMove(List<string> ants)
        {
            List<string> result = new List<string>();

            bool skipNext = false;
            for (int i = 0; i < ants.Count(); i++)
            {
                if (skipNext)
                {
                    skipNext = false;
                    continue;
                }

                if (ants.Count() > (i+1) && ants[i] == "green" && ants[i + 1] == "blue")
                {
                    result.Add("blue");
                    result.Add("green");
                    skipNext = true;
                }
                else
                {
                    result.Add(ants[i]);
                }
            }

            return result;
        }

        private System.Windows.Controls.Image getAnt(string type, int offsetLeft = 0, int offsetTop = 0, int size = 50)
        {
            Bitmap bmp = type == "green" ? Properties.Resources.antGreen : Properties.Resources.antBlue;

            System.Windows.Controls.Image image = new System.Windows.Controls.Image
            {
                Width = size,
                Height = size,
                Margin = new Thickness(offsetLeft, offsetTop, 0, 0),
                Source = 
                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()
                    )
            };

            return image;
        }

        private void renderAnts(List<string> ants)
        {
            int maxSizeY = (int)(canvas.Height - canvas.Height / 5);
            int maxSizeX = (int)(canvas.Width - 10 * ants.Count()) / ants.Count();
            int size = Math.Min(maxSizeY, maxSizeX);

            canvas.Children.Clear();

            int iterator = 0;
            foreach (string ant in ants)
            {
                canvas.Children.Add(getAnt(ant, 5 + iterator * (size + 10), (int)((canvas.Height - size) / 2), size));

                iterator++;
            }
        }
    }
}
