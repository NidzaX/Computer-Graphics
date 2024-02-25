using ProjektniZadatak1.Enums;
using ProjektniZadatak1.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjektniZadatak1.Windows
{
    /// <summary>
    /// Interaction logic for PolygonWindow.xaml
    /// </summary>
    public partial class PolygonWindow : Window
    {
        private EditMode editMode;

        private List<Point> points;

        private Polygon polygon;

        private TextBlock textBlock;

        public PolygonWindow(List<Point> points)
        {
            InitializeComponent();

            this.points = points;
            cmbBoxBorderColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxFillColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxTextColor.ItemsSource = typeof(Colors).GetProperties();
            btnExecute.Content = "Draw";
            editMode = EditMode.New;
        }

        public PolygonWindow(Polygon polygon, TextBlock textBlock)
        {
            InitializeComponent();

            this.polygon = polygon;
            this.textBlock = textBlock;
            cmbBoxBorderColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxFillColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxTextColor.ItemsSource = typeof(Colors).GetProperties();
            btnExecute.Content = "Update";
            editMode = EditMode.Edit;

            FillExistingData();
        }

        private void FillExistingData()
        {
            txtBoxBorderThickness.Text = polygon.StrokeThickness.ToString();

            if (polygon.Stroke is SolidColorBrush strokeBrush)
            {
                cmbBoxBorderColor.SelectedIndex = Helpers.GeneralHelper.GetColorIndex(strokeBrush.Color);
            }
            if (polygon.Fill is SolidColorBrush fillBrush)
            {
                cmbBoxFillColor.SelectedIndex = Helpers.GeneralHelper.GetColorIndex(fillBrush.Color);
            }
            if (polygon.Opacity == 0.5)
            {
                chkBoxTransparent.IsChecked = true;
            }

            if (textBlock == null)
            {
                cmbBoxTextColor.IsEnabled = false;
                txtBoxBorderThickness.IsEnabled = false;
                return;
            }

            txtBoxText.Text = textBlock.Text;
            if (textBlock.Foreground is SolidColorBrush textBrush)
            {
                cmbBoxTextColor.SelectedIndex = GeneralHelper.GetColorIndex(textBrush.Color);
            }
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate() || polygon == null) return;

            MainWindow.polygon = polygon;
            polygon = null;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool Validate()
        {
            double borderThickness;
            bool isBorderThicknessValid = double.TryParse(txtBoxBorderThickness.Text, out borderThickness);
            bool hasText = !String.IsNullOrEmpty(txtBoxText.Text);

            if (!isBorderThicknessValid || borderThickness <= 0)
            {
                MessageBox.Show("Please enter a valid number for Border Thickness.");
                return false;
            }

            if (cmbBoxBorderColor.SelectedIndex == -1 || cmbBoxFillColor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an option for Border Color and Fill Color.");
                return false;
            }

            if (!String.IsNullOrEmpty(txtBoxText.Text) && cmbBoxFillColor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an option for Text Color.");
                return false;
            }

            // Set values of private fields
            switch (editMode)
            {
                case EditMode.New:
                    polygon = new Polygon
                    {
                        Points = new PointCollection(points),
                        StrokeThickness = borderThickness,
                        Stroke = new SolidColorBrush((Color)(cmbBoxBorderColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                        Fill = new SolidColorBrush((Color)(cmbBoxFillColor.SelectedItem as PropertyInfo).GetValue(1, null))
                    };
                    if (chkBoxTransparent.IsChecked == true)
                    {
                        polygon.Opacity = 0.5;
                    }
                    else
                    {
                        polygon.Opacity = 1.0;
                    }

                    if (hasText)
                    {
                        textBlock = new TextBlock()
                        {
                            Text = txtBoxText.Text,
                            Foreground = new SolidColorBrush((Color)(cmbBoxTextColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                            FontSize = 30
                        };
                        if (chkBoxTransparent.IsChecked == true)
                        {
                            polygon.Opacity = 0.5;
                        }
                        else
                        {
                            polygon.Opacity = 1.0;
                        }

                        GeneralHelper.AdjustMarginToCenter(textBlock, polygon);
                        MainWindow.shapeText = textBlock;
                    }
                    break;
                case EditMode.Edit:
                    polygon.StrokeThickness = borderThickness;
                    polygon.Stroke = new SolidColorBrush((Color)(cmbBoxBorderColor.SelectedItem as PropertyInfo).GetValue(1, null));
                    polygon.Fill = new SolidColorBrush((Color)(cmbBoxFillColor.SelectedItem as PropertyInfo).GetValue(1, null));
                    if (chkBoxTransparent.IsChecked == true)
                    {
                        polygon.Opacity = 0.5;
                    }
                    else
                    {
                        polygon.Opacity = 1.0;
                    }

                    if (hasText)
                    {
                        textBlock = new TextBlock()
                        {
                            Text = txtBoxText.Text,
                            Foreground = new SolidColorBrush((Color)(cmbBoxTextColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                            FontSize = 30
                        };

                        GeneralHelper.AdjustMarginToCenter(textBlock, polygon);
                        MainWindow.shapeText = textBlock;
                    }
                    break;
            }

            return true;
        }
    }
}
