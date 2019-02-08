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
namespace SeaBotWPF
{
    #region

    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    #endregion

    /// <summary>
    ///     Brush that lets you draw vertical linear gradient without banding.
    /// </summary>
    [MarkupExtensionReturnType(typeof(Brush))]
    public class SmoothLinearGradientBrush : MarkupExtension
    {
        private static readonly byte[,] bayerMatrix_ =
            {
                { 1, 9, 3, 11 }, { 13, 5, 15, 7 }, { 1, 9, 3, 11 }, { 16, 8, 14, 6 }
            };

        private static readonly PropertyInfo dpiX_;

        private static readonly PropertyInfo dpiY_;

        static SmoothLinearGradientBrush()
        {
            dpiX_ = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            dpiY_ = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
        }

        /// <summary>
        ///     Gradient color at the top
        /// </summary>
        public Color From { get; set; }

        /// <summary>
        ///     Gradient color at the bottom
        /// </summary>
        public Color To { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // If user changes dpi/virtual screen height during applicaiton lifetime,
            // wpf will scale the image up for us.
            var width = 20;
            var height = (int)SystemParameters.VirtualScreenHeight;
            var dpix = (int)dpiX_.GetValue(null);
            var dpiy = (int)dpiY_.GetValue(null);

            var stride = 4 * ((width * PixelFormats.Bgr24.BitsPerPixel + 31) / 32);

            // dithering parameters
            var bayerMatrixCoefficient = 1.0 / (bayerMatrix_.Length + 1);
            var bayerMatrixSize = bayerMatrix_.GetLength(0);

            // Create pixel data of image
            var buffer = new byte[height * stride];

            for (var line = 0; line < height; line++)
            {
                var scale = (double)line / height;

                for (var x = 0; x < width * 3; x += 3)
                {
                    // scaling of color
                    var blue = this.To.B * scale + this.From.B * (1.0 - scale);
                    var green = this.To.G * scale + this.From.G * (1.0 - scale);
                    var red = this.To.R * scale + this.From.R * (1.0 - scale);

                    // ordered dithering of color
                    // source: http://en.wikipedia.org/wiki/Ordered_dithering
                    buffer[x + line * stride] =
                        (byte)(blue + bayerMatrixCoefficient
                               * bayerMatrix_[x % bayerMatrixSize, line % bayerMatrixSize]);
                    buffer[x + line * stride + 1] =
                        (byte)(green + bayerMatrixCoefficient
                               * bayerMatrix_[x % bayerMatrixSize, line % bayerMatrixSize]);
                    buffer[x + line * stride + 2] =
                        (byte)(red + bayerMatrixCoefficient
                               * bayerMatrix_[x % bayerMatrixSize, line % bayerMatrixSize]);
                }
            }

            var image = BitmapSource.Create(width, height, dpix, dpiy, PixelFormats.Bgr24, null, buffer, stride);
            image.Freeze();
            var brush = new ImageBrush(image);
            brush.Freeze();
            return brush;
        }
    }
}