// // SeaBotWPF
// // Copyright (C) 2018 - 2019 Weespin
// // 
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.
// // 
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace SeaBotWPF
{
    #region

    using System;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Effects;

    #endregion

    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.lbl_last_log.Visibility = Visibility.Collapsed;
            this.lbl_last_log.Visibility = Visibility.Visible;
            this.lbl_last_log.Content = "Lastest log: " + new Random().Next(int.MaxValue);

            // <Border.Effect>
            var fade = new DoubleAnimation { From = 0, To = 360, Duration = TimeSpan.FromSeconds(2) };

            var radius = new DoubleAnimation { From = 0, To = 100, Duration = TimeSpan.FromSeconds(5) };

            // Storyboard.SetTarget(fade, (DropShadowEffect)StartBorder.Effect);
            Storyboard.SetTargetProperty(fade, new PropertyPath(DropShadowEffect.BlurRadiusProperty));

            var sb = new Storyboard();
            sb.Children.Add(fade);

            sb.Begin(this);
        }
    }
}