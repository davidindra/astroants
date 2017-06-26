using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace astroants
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            canvas.Children.Clear();
            canvas.Children.Add(getAnt(true, 0));
            canvas.Children.Add(getAnt(false, 100));
        }

        private System.Windows.Controls.Image getAnt(bool green = true, int offset = 0)
        {
            Bitmap bmp = green ? Properties.Resources.antGreen : Properties.Resources.antBlue;

            System.Windows.Controls.Image image = new System.Windows.Controls.Image
            {
                Width = bmp.Width,
                Height = bmp.Height,
                Margin = new Thickness(offset, 0, 0, 0),
                //Name = "ant",
                Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
            };

            return image;
        }
    }
}
