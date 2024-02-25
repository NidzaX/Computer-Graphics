using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ProjektniZadatak1.Enums;
using ProjektniZadatak1.Helpers;

namespace ProjektniZadatak1.Windows
{
    /// <summary>
    /// Interaction logic for EllipseWindow.xaml
    /// </summary>
    public partial class EllipseWindow : Window
    {
        private EditMode editMode;

        private Point position;

        private static Ellipse ellipse;

        private static TextBlock textBlock;

        public EllipseWindow(Point position)
        {
            InitializeComponent();

            cmbBoxBorderColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxFillColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxTextColor.ItemsSource = typeof(Colors).GetProperties();
            editMode = EditMode.New;
            this.position = position;
        }

        public EllipseWindow(Ellipse ellipse, TextBlock textBlock)
        {
            InitializeComponent();

            EllipseWindow.ellipse = ellipse;
            EllipseWindow.textBlock = textBlock;

            cmbBoxBorderColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxFillColor.ItemsSource = typeof(Colors).GetProperties();
            cmbBoxTextColor.ItemsSource = typeof(Colors).GetProperties();
            editMode = EditMode.Edit;

            FillExistingData();
        }

        private void FillExistingData()
        {
            txtBoxHeight.Text = ellipse.Height.ToString();
            txtBoxWidth.Text = ellipse.Width.ToString();
            txtBoxBorderThickness.Text = ellipse.StrokeThickness.ToString();

            if (ellipse.Stroke is SolidColorBrush strokeBrush)
            {
                cmbBoxBorderColor.SelectedIndex = Helpers.GeneralHelper.GetColorIndex(strokeBrush.Color);
            }
            if (ellipse.Fill is SolidColorBrush fillBrush)
            {
                cmbBoxFillColor.SelectedIndex = Helpers.GeneralHelper.GetColorIndex(fillBrush.Color);
            }
            if (ellipse.Opacity == 0.5)
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
            if (!Validate() || ellipse == null) return;

            MainWindow.ellipse = ellipse;
            ellipse = null;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool Validate()
        {
            double height, width, borderThickness;
            bool isHeightValid = double.TryParse(txtBoxHeight.Text, out height);
            bool isWidthValid = double.TryParse(txtBoxWidth.Text, out width);
            bool isBorderThicknessValid = double.TryParse(txtBoxBorderThickness.Text, out borderThickness);
            bool hasText = !String.IsNullOrEmpty(txtBoxText.Text);

            if (!isHeightValid || !isWidthValid || !isBorderThicknessValid ||
                 height <= 0 || width <= 0 || borderThickness <= 0)
            {
                MessageBox.Show("Please enter a valid number for Height, Width and Border Thickness.");
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
                    ellipse = new Ellipse
                    {
                        Height = height,
                        Width = width,
                        Margin = new Thickness(position.X, position.Y, 0, 0),
                        StrokeThickness = borderThickness,
                        Stroke = new SolidColorBrush((Color)(cmbBoxBorderColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                        Fill = new SolidColorBrush((Color)(cmbBoxFillColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                    };
                    if (chkBoxTransparent.IsChecked == true)
                    {
                        ellipse.Opacity = 0.5;
                    }
                    else
                    {
                        ellipse.Opacity = 1.0;
                    }

                    if (hasText)
                    {
                        textBlock = new TextBlock()
                        {
                            Text = txtBoxText.Text,
                            Foreground = new SolidColorBrush((Color)(cmbBoxTextColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                            FontSize = 30
                        };

                        GeneralHelper.AdjustMarginToCenter(textBlock, ellipse);
                        MainWindow.shapeText = textBlock;
                    }
                    break;
                case EditMode.Edit:
                    ellipse = new Ellipse
                    {
                        Height = height,
                        Width = width,
                        StrokeThickness = borderThickness,
                        Stroke = new SolidColorBrush((Color)(cmbBoxBorderColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                        Fill = new SolidColorBrush((Color)(cmbBoxFillColor.SelectedItem as PropertyInfo).GetValue(1, null))
                    };
                    if (chkBoxTransparent.IsChecked == true)
                    {
                        ellipse.Opacity = 0.5;
                    }
                    else
                    {
                        ellipse.Opacity = 1.0;
                    }

                    if (hasText)
                    {
                        textBlock = new TextBlock()
                        {
                            Text = txtBoxText.Text,
                            Foreground = new SolidColorBrush((Color)(cmbBoxTextColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                            FontSize = 30
                        };

                        GeneralHelper.AdjustMarginToCenter(textBlock, ellipse);
                        MainWindow.shapeText = textBlock;
                    }
                    break;
            }

            return true;
        }
    }
}
