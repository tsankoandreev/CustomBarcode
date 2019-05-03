//using CustomMarker.Generators;
using CustomBarcode.Objects;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Svg;
using CustomBarcode.Common;
using CustomBarcode;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Numerics;

namespace CustomBarcode.Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        BaseBarcode marker = null;
        public MainWindow()
        {
            InitializeComponent();
            this.comboBox.ItemsSource = Enum.GetValues(typeof(BarcodeShapesEnum)).Cast<BarcodeShapesEnum>();
            this.comboBox.SelectedIndex = (int)BarcodeShapesEnum.Circle;

            this.comboBoxCountX.ItemsSource = Enumerable.Range(1, 50).ToArray();
            this.comboBoxCountX.SelectedIndex = 10;
            this.comboBoxCountY.ItemsSource = Enumerable.Range(1, 50).ToArray();
            this.comboBoxCountY.SelectedIndex = 10;
        }
        

        private void button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string pattern = this.textBox.Text.Replace(" ","");

            if ((BarcodeShapesEnum)comboBox.SelectedItem == BarcodeShapesEnum.Circle)
            {
                marker = new CircleBarcode((int)this.comboBoxCountX.SelectedItem, (int)this.comboBoxCountY.SelectedItem);
            }
            else if ((BarcodeShapesEnum)comboBox.SelectedItem == BarcodeShapesEnum.Square)
            {
                marker = new SquareBarcode((int)this.comboBoxCountX.SelectedItem, (int)this.comboBoxCountY.SelectedItem);
            }
            else if ((BarcodeShapesEnum)comboBox.SelectedItem == BarcodeShapesEnum.Triangle)
            {
                marker = new TriangleBarcode((int)this.comboBoxCountX.SelectedItem, (int)this.comboBoxCountY.SelectedItem);
            }

            if (marker.segments.Count() != pattern.Length)
            {
                MessageBoxResult result = MessageBox.Show("Pattern length mismatch ...", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var svg = SVGManager.DrawSVGPoligonsfromSegments(marker.segments.ToArray(), pattern);
            
            SVGManager.SaveSVGtoPNG(svg);
            
            using (var stream = File.OpenRead("bitmap.jpg"))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                image.Source = bmp;
            }
        }
        private void comboBoxCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.comboBoxCountX.SelectedItem == null || this.comboBoxCountY.SelectedItem == null)
                return;
            image.Source = new BitmapImage();
            Random binaryrand = new Random();
            List<int> l = new List<int>();

            while (l.Find(x => x == 1) != 1)
            {
                for (int i = 0; i < ((int)this.comboBoxCountX.SelectedItem * (int)this.comboBoxCountY.SelectedItem) ; i++)
                {
                    l.Add(binaryrand.Next(0, 2));
                }
            }

            string s = string.Join("", l);
            this.Number.Text = BinToDec(s);

            string newS = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (i != 0 && i % 4 == 0)
                    newS += " ";
                newS += s[i];
            }
            this.textBox.Text = newS;
        }
        
        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            long value = 0;
            long.TryParse(this.Number.Text, out value);

            string s = Convert.ToString(value, 2);
            string newS = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (i % 4 == 0)
                    newS += " ";
                newS += s[i];
            }
            this.textBox.Text = newS;
        }
        public string BinToDec(string value)
        {
            BigInteger res = 0;
            
            foreach (char c in value)
            {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }

            return res.ToString();
        }
    }
}
