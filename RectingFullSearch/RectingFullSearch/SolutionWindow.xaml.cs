using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RectingFullSearch
{
    /// <summary>
    /// Interaction logic for SolutionWindow.xaml
    /// </summary>
    public partial class SolutionWindow : Window
    {
        public const int MAX_WIDTH = 800;
        public const int MAX_HEIGHT = 600;
        public int Size;
        public SolutionWindow(int W,int H,List<Rect> solution)
        {
            InitializeComponent();
            int k;
            if (W > H)
            {
                k = MAX_WIDTH / W;
            }
            else 
            {
                k = MAX_HEIGHT / H;
            }
            this.Width = k * W+15;
            this.Height = k * H+35;
            Size = k;
            _showSolution(solution);
        }
        private void _showSolution(List<Rect> solution)
        {
            Brush[] Colors = { Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Black, Brushes.Blue, Brushes.Purple, Brushes.Orange };
            int index=0;
            foreach (Rect item in solution)
            {
                
                Button b = new Button();
                b.Background = Colors[index%Colors.Length];
                index++;
                XCanvas.Children.Add(b);
                TextBlock toolTip = new TextBlock();
                toolTip.Text = item.SName+" ("+item.Width+"x"+item.Height+")\r\n"+ "x=" + item.X + " y=" + item.Y;
                b.ToolTip = toolTip;
                b.Width = item.Width * Size;
                b.Height = item.Height * Size;
                Canvas.SetLeft(b, item.X * Size);
                Canvas.SetTop(b, item.Y * Size);
            }
        }
    }
}
