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

namespace Knapsack0_1
{
    /// <summary>
    /// Interaction logic for SolutionShower.xaml
    /// </summary>
    public partial class SolutionShower : Window
    {
        private KnapsackSolution _solution;
        public SolutionShower(KnapsackSolution solution)
        {
            _solution = solution;
            InitializeComponent();
            XNumObjects.Text = solution.NumObjects.ToString();
            XTotalPrice.Text = solution.TotalPrice.ToString()+" $";
            XTotalWeight.Text = solution.TotalWeight.ToString()+" kg";
            XAttitude.Text = Math.Round(((decimal)solution.TotalWeight / solution.MaxWeight),3).ToString();
            var temp=solution.GetFirst();
            while (temp != null)
            {
                XList.Items.Add(temp);
                temp = solution.GetNextObject();
            }
        }
    }
}
