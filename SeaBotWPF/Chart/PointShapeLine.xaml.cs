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
using LiveCharts;
using LiveCharts.Wpf;

namespace SeaBotWPF.Chart
{
    /// <summary>
    /// Логика взаимодействия для PointShapeLine.xaml
    /// </summary>
    public partial class PointShapeLine : UserControl
    {
        public PointShapeLine()
        {
            InitializeComponent();
            LiveCharts.Wpf.Charts.Base.Chart.Colors = new List<System.Windows.Media.Color>
            {
                ConvertStringToColor("#0E474F"),
                ConvertStringToColor("#4C9F84"),
                ConvertStringToColor("#E4DA9C"),
                ConvertStringToColor("#164367"),
                ConvertStringToColor("#66A7B2"),
               
            };
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                    , 
                 
                    PointForeground =
                    (SolidColorBrush)(new BrushConverter().ConvertFrom("#0E474F"))
        },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                PointForeground = 
                    (SolidColorBrush)(new BrushConverter().ConvertFrom("#4C9F84"))
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },


                    PointForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E4DA9C")),Fill=null
                },
                new LineSeries
                {
                Title = "Series 4",
                Values = new ChartValues<double> { 1,5,3,5,6 },


                PointForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#164367")),Fill=null
            },
                new LineSeries
                {
                    Title = "Series 5",
                    Values = new ChartValues<double> { 5,3,3,5,2 },


                    PointForeground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#66A7B2")),Fill=null
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString();
            
       

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public System.Windows.Media.Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }
    }
    
}
