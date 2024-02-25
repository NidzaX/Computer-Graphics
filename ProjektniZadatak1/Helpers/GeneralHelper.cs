using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjektniZadatak1.Helpers
{
    public static class GeneralHelper
    {
        public static int GetColorIndex(Color color)
        {
            var colors = typeof(Colors).GetProperties();
            
            int index = 0;
            foreach (var c in colors)
            {
                if ((Color)c.GetValue(1, null) == color)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public static void AdjustMarginToCenter(TextBlock textBlock, UIElement element)
        {
            // Get the width and height of the element
            double elementX;
            double elementY;
            double elementHeightRadius;
            double elementWidthRadius;
            double textBlockX;
            double textBlockY;
            double textBlockHeightRadius;
            double textBlockWidthRadius;

            MeasureTextBlockRadius(textBlock, out textBlockWidthRadius, out textBlockHeightRadius);

            if (element is Ellipse)
            {
                elementX = ((Ellipse)element).Margin.Left;
                elementY = ((Ellipse)element).Margin.Top;

                elementHeightRadius = ((Ellipse)element).Height / 2;
                elementWidthRadius = ((Ellipse)element).Width / 2;
            }
            else
            {
                // Calculate the bounding box of the polygon
                Rect bounds = GetPolygonBounds((Polygon)element);

                elementX = bounds.X;
                elementY = bounds.Y;

                elementHeightRadius = bounds.Height / 2;
                elementWidthRadius = bounds.Width / 2;
            }
            
            // Calculate the x and y values that point to the center of the element
            textBlockX = elementX + elementWidthRadius - textBlockWidthRadius;
            textBlockY = elementY + elementHeightRadius - textBlockHeightRadius;

            textBlock.Margin = new Thickness(textBlockX, textBlockY, 0, 0);
            textBlock.MaxHeight = elementHeightRadius * 2;
            textBlock.MaxWidth = elementWidthRadius * 2;
            textBlock.TextWrapping = TextWrapping.Wrap;
        }

        private static void MeasureTextBlockRadius(TextBlock textBlock, out double width, out double height)
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            FormattedText formattedText = new FormattedText(
                textBlock.Text,      // the text to measure
                CultureInfo.CurrentCulture,    // the culture to use
                FlowDirection.LeftToRight,     // the direction of the flow
                new Typeface("Arial"),         // the font family
                20,                            // the font size
                Brushes.Black                 // the font color
            );
            #pragma warning restore CS0618 // Type or member is obsolete

            // get the width and height of the text block
            width = formattedText.Width / 2;
            height = formattedText.Height / 2;
        }

        private static Rect GetPolygonBounds(Polygon polygon)
        {
            double minX = Double.MaxValue;
            double maxX = Double.MinValue;
            double minY = Double.MaxValue;
            double maxY = Double.MinValue;

            foreach (Point point in polygon.Points)
            {
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// From UTM to Latitude and longitude in decimal
        /// </summary>
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
    }
}
