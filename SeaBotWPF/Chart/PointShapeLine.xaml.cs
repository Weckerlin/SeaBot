// SeaBotWPF
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace SeaBotWPF.Chart
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Media;

    using LiveCharts;
    using LiveCharts.Wpf;
    using LiveCharts.Wpf.Charts.Base;

    #endregion

    /// <summary>
    ///     Логика взаимодействия для PointShapeLine.xaml
    /// </summary>
    public partial class PointShapeLine : UserControl
    {
        public PointShapeLine()
        {
            this.InitializeComponent();
            Chart.Colors = new List<Color>
                               {
                                   this.ConvertStringToColor("#0E474F"),
                                   this.ConvertStringToColor("#4C9F84"),
                                   this.ConvertStringToColor("#E4DA9C"),
                                   this.ConvertStringToColor("#164367"),
                                   this.ConvertStringToColor("#66A7B2")
                               };
            this.SeriesCollection = new SeriesCollection
                                        {
                                            new LineSeries
                                                {
                                                    Title = "Series 1",
                                                    Values = new ChartValues<double>
                                                                 {
                                                                     4,
                                                                     6,
                                                                     5,
                                                                     2,
                                                                     4
                                                                 },
                                                    PointForeground =
                                                        (SolidColorBrush)new BrushConverter().ConvertFrom("#0E474F")
                                                },
                                            new LineSeries
                                                {
                                                    Title = "Series 2",
                                                    Values = new ChartValues<double>
                                                                 {
                                                                     6,
                                                                     7,
                                                                     3,
                                                                     4,
                                                                     6
                                                                 },
                                                    PointForeground =
                                                        (SolidColorBrush)new BrushConverter().ConvertFrom("#4C9F84")
                                                },
                                            new LineSeries
                                                {
                                                    Title = "Series 3",
                                                    Values = new ChartValues<double>
                                                                 {
                                                                     4,
                                                                     2,
                                                                     7,
                                                                     2,
                                                                     7
                                                                 },
                                                    PointForeground =
                                                        (SolidColorBrush)new BrushConverter().ConvertFrom("#E4DA9C"),
                                                    Fill = null
                                                },
                                            new LineSeries
                                                {
                                                    Title = "Series 4",
                                                    Values = new ChartValues<double>
                                                                 {
                                                                     1,
                                                                     5,
                                                                     3,
                                                                     5,
                                                                     6
                                                                 },
                                                    PointForeground =
                                                        (SolidColorBrush)new BrushConverter().ConvertFrom("#164367"),
                                                    Fill = null
                                                },
                                            new LineSeries
                                                {
                                                    Title = "Series 5",
                                                    Values = new ChartValues<double>
                                                                 {
                                                                     5,
                                                                     3,
                                                                     3,
                                                                     5,
                                                                     2
                                                                 },
                                                    PointForeground =
                                                        (SolidColorBrush)new BrushConverter().ConvertFrom("#66A7B2"),
                                                    Fill = null
                                                }
                                        };

            this.Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            this.YFormatter = value => value.ToString();

            this.DataContext = this;
        }

        public string[] Labels { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public Color ConvertStringToColor(string hex)
        {
            // remove the # at the front
            hex = hex.Replace("#", string.Empty);

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            var start = 0;

            // handle ARGB strings (8 characters int)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                start = 2;
            }

            // convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}