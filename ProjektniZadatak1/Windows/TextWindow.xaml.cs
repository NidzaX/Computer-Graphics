using ProjektniZadatak1.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjektniZadatak1.Windows
{
    /// <summary>
    /// Interaction logic for TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        private EditMode editMode;
        public static List<string> Size = new List<string>() { "2", "4", "6", "8", "10" };
        public static TextBox textBox;
        public static Thickness position;

        public TextWindow(Point position)
        {
            InitializeComponent();

            cmbBoxTextColor.ItemsSource = typeof(Colors).GetProperties();
            TextWindow.position = new Thickness(position.X, position.Y, 0, 0);
            btnExecute.Content = "Draw";
            editMode = EditMode.New;
        }

        public TextWindow(TextBox textBox)
        {
            InitializeComponent();

            cmbBoxTextColor.ItemsSource = typeof(Colors).GetProperties();
            TextWindow.textBox = textBox;
            TextWindow.position = textBox.Margin;
            btnExecute.Content = "Update";
            editMode = EditMode.Edit;

            FillExistingData();
        }

        private void FillExistingData()
        {
            txtBoxContent.Text = textBox.Text;
            txtBoxTextSize.Text = textBox.FontSize.ToString();

            if (textBox.Foreground is SolidColorBrush strokeBrush)
            {
                cmbBoxTextColor.SelectedIndex = Helpers.GeneralHelper.GetColorIndex(strokeBrush.Color);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            Close();
        }

        private bool Validate()
        {
            double size;
            bool isSizeValid = double.TryParse(txtBoxTextSize.Text, out size);

            if (!isSizeValid || String.IsNullOrEmpty(txtBoxContent.Text) || size <= 0)
            {
                MessageBox.Show("Please enter a valid information in the fields.");
                return false;
            }

            if (cmbBoxTextColor.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an option for Text Color.");
                return false;
            }

            switch (editMode)
            {
                case EditMode.New:
                    MainWindow.text = new TextBox()
                    {
                        IsReadOnly = true,
                        Margin = position,
                        Text = txtBoxContent.Text,
                        Foreground = new SolidColorBrush((Color)(cmbBoxTextColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                        FontSize = size
                    };
                    break;
                case EditMode.Edit:
                    MainWindow.text = new TextBox()
                    {
                        IsReadOnly = true,
                        Margin = position,
                        Text = txtBoxContent.Text,
                        Foreground = new SolidColorBrush((Color)(cmbBoxTextColor.SelectedItem as PropertyInfo).GetValue(1, null)),
                        FontSize = size
                    };
                    break;
            }

            return true;
        }
    }
}
