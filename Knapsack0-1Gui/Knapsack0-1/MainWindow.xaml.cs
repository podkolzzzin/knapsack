using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Knapsack0_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum Algorithms
    { 
        FullSearch1, FullSearch2, GreedySearch
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                KnapsackObject t = new KnapsackObject();
                t.Name = XObjName.Text;
                t.Price = Convert.ToInt32(XObjPrice.Text);
                t.Weight = Convert.ToInt32(XObjWeight.Text);
                XObjectsList.Items.Add(t);
            }
            catch (Exception)
            {
                
            }
        }
        Algorithms algs;
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            algs = Algorithms.FullSearch1;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            algs = Algorithms.FullSearch2;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            algs = Algorithms.GreedySearch;
        }
        private void _showSolution(KnapsackSolution solution)
        {
            new SolutionShower(solution).Show();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (algs == Algorithms.FullSearch1)
                {
                    _showSolution(Knapsack.FindSolutionFullSearchV1(XObjectsList.Items, Convert.ToInt32(XMaxWeight.Text)));
                }
                else if (algs == Algorithms.FullSearch2)
                {
                    _showSolution(Knapsack.FindSolutionFullSearchV2(XObjectsList.Items, Convert.ToInt32(XMaxWeight.Text)));
                }
                else
                {
                    _showSolution(Knapsack.FindSolutionGreedySearch(XObjectsList.Items, Convert.ToInt32(XMaxWeight.Text)));
                }
            }
            catch(FormatException) { }
        }

        Timer t;
        Queue<Task> Tasks = new Queue<Task>();
        class Task
        {
            public TextBox tb;
            public string text;
            public int interval;
            public Action<object, RoutedEventArgs> Func;
            public Task(Action<object, RoutedEventArgs> Func, int interval)
            {
                this.Func = Func;
                this.interval = interval;
            }
            public Task(TextBox tb, string text, int interval)
            {
                this.tb = tb;
                this.text = text;
                this.interval = interval;
            }
        }
        private void SetText(TextBox tb, string text, int pause)
        {
            if (t == null)
            {
                t = new Timer(_timerFunc);
                t.Change(200, 200);
            }
            Tasks.Enqueue(new Task(tb, text, pause));
        }

        delegate void TextDeleg(TextBox tb, string text);
        private void _timerFunc(object state)
        {
            if (Tasks.Count == 0)
                return;

            Task t = Tasks.Peek();
            if (t.interval > 1)
            {
                t.interval--;
                return;
            }
            Tasks.Dequeue();
            if (t.Func != null)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, t.Func, t, new RoutedEventArgs());
            }
            else
                t.tb.Dispatcher.BeginInvoke(DispatcherPriority.Background, new TextDeleg(_setTextDeleg), t.tb, t.text);
        }

        private void Eval(Action<object, RoutedEventArgs> Button_Click_1, int p)
        {
            Tasks.Enqueue(new Task(Button_Click_1, p));
        }

        private void _setTextDeleg(TextBox tb, string text)
        {
            tb.Text = text;
        }

        private string sl(string s)
        {
            return s;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OnBtnClear(null, null);
            SetText(XMaxWeight, "100", 1);

            SetText(XObjName, sl("Мяч"), 1);
            SetText(XObjPrice, "41", 1);
            SetText(XObjWeight, "56", 1);
            Eval(Button_Click_1,1);

            SetText(XObjName, sl("Клюшка"), 1);
            SetText(XObjPrice, "38", 1);
            SetText(XObjWeight, "25", 1);
            Eval(Button_Click_1, 3);
           
            SetText(XObjName, sl("Книга"), 1);
            SetText(XObjPrice, "69", 1);
            SetText(XObjWeight, "37", 1);    
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Ноутбук"), 1);
            SetText(XObjPrice, "53", 1);
            SetText(XObjWeight, "40", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Фотоаппарат"), 1);
            SetText(XObjPrice, "66", 1);
            SetText(XObjWeight, "23", 1);
            Eval(Button_Click_1, 3);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OnBtnClear(null, null);
            SetText(XMaxWeight, "20", 1);

            SetText(XObjName, sl("Мяч"), 1);
            SetText(XObjPrice, "10", 1);
            SetText(XObjWeight, "10", 1);
            Eval(Button_Click_1, 1);

            SetText(XObjName, sl("Клюшка"), 1);
            SetText(XObjPrice, "20", 1);
            SetText(XObjWeight, "20", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Книга"), 1);
            SetText(XObjPrice, "30", 1);
            SetText(XObjWeight, "5", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Ноутбук"), 1);
            SetText(XObjPrice, "50", 1);
            SetText(XObjWeight, "6", 1);
            Eval(Button_Click_1, 3);

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            OnBtnClear(null, null);
            SetText(XMaxWeight, "100", 1);

            SetText(XObjName, sl("Мяч"), 1);
            SetText(XObjPrice, "31", 1);
            SetText(XObjWeight, "86", 1);
            Eval(Button_Click_1, 1);

            SetText(XObjName, sl("Клюшка"), 1);
            SetText(XObjPrice, "28", 1);
            SetText(XObjWeight, "35", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Книга"), 1);
            SetText(XObjPrice, "59", 1);
            SetText(XObjWeight, "27", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Ноутбук"), 1);
            SetText(XObjPrice, "23", 1);
            SetText(XObjWeight, "20", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Фотоаппарат"), 1);
            SetText(XObjPrice, "66", 1);
            SetText(XObjWeight, "53", 1);
            Eval(Button_Click_1, 3);

            SetText(XObjName, sl("Покерный_набор"), 1);
            SetText(XObjPrice, "68", 1);
            SetText(XObjWeight, "13", 1);
            Eval(Button_Click_1, 3);           
        }

        private void OnBtnClear(object sender, RoutedEventArgs e)
        {
            XObjectsList.Items.Clear();
            XMaxWeight.Clear();
            XObjName.Clear();
            XObjPrice.Clear();
            XObjWeight.Clear();
        }
    }
}
