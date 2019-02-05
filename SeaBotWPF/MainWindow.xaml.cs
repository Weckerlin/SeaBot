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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;

namespace SeaBotWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

      
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
       
            lbl_last_log.Visibility = Visibility.Collapsed;
            lbl_last_log.Visibility = Visibility.Visible;
            lbl_last_log.Content = "Lastest log: "+new Random().Next(int.MaxValue);
            //  <Border.Effect>

            
           
          
            var fade = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(2),
            };

            var radius = new DoubleAnimation()
            {
                From = 0,
                To = 100,
                Duration = TimeSpan.FromSeconds(5),
            };

          
        //    Storyboard.SetTarget(fade, (DropShadowEffect)StartBorder.Effect);
            Storyboard.SetTargetProperty(fade, new PropertyPath(DropShadowEffect.BlurRadiusProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);


            sb.Begin(this);

        }
      
    }
   
}
