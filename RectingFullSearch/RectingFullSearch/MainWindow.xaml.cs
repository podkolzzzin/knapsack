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

namespace RectingFullSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                Rect temp = new Rect();
                temp.SName = XObjName.Text;
                temp.Height = Convert.ToInt32(XObjHeight.Text);
                temp.Width = Convert.ToInt32(XObjWidth.Text);
                XObjectsList.Items.Add(temp);
            }
            catch
            { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int W,H;
            try
            {
                W = Convert.ToInt32(XContainerWidth.Text);
                H = Convert.ToInt32(XContainerHeight.Text);
            }
            catch
            {
                return;
            }
            var t=RectangularPacker.Pack(W,H,XObjectsList.Items);
            SolutionWindow window=new SolutionWindow(W,H,t);
            window.Show();
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
            public Task(TextBox tb,string text,int interval)
            {
                this.tb = tb;
                this.text = text;
                this.interval = interval;
            }
        }
        private void SetText(TextBox tb,string text,int pause)
        {
            if (t == null)
            {
                t = new Timer(_timerFunc);
                t.Change(200, 200);
            }
            Tasks.Enqueue(new Task(tb, text,pause));
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
                Dispatcher.BeginInvoke(DispatcherPriority.Background, t.Func, t , new RoutedEventArgs());
            }
            else
                t.tb.Dispatcher.BeginInvoke(DispatcherPriority.Background,new TextDeleg(_setTextDeleg), t.tb, t.text);
        }

        private void _setTextDeleg(TextBox tb, string text)
        {
            tb.Text = text;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //test1
            XObjectsList.Items.Clear();
            SetText(XContainerWidth, "8", 1);
            SetText(XContainerHeight, "8", 1);

            SetText(XObjName, "Заготовка 1", 1);
            SetText(XObjWidth, "2", 1);
            SetText(XObjHeight, "3", 1);
            Eval(Button_Click_1, 1);
            

            SetText(XObjWidth, "", 1);
            SetText(XObjHeight, "", 1);
            SetText(XObjName, "", 1);

            SetText(XObjName, "Заготовка 2", 1);
            SetText(XObjWidth, "3", 1);
            SetText(XObjHeight, "2", 1);
            
            Eval(Button_Click_1, 1);
            Eval(Button_Click_2, 1);
            
        }

        private void Eval(Action<object, RoutedEventArgs> Button_Click_1, int p)
        {
            Tasks.Enqueue(new Task(Button_Click_1,p));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //test2
            XObjectsList.Items.Clear();
            SetText(XContainerWidth, "59", 1);
            SetText(XContainerHeight, "24", 1);

            SetText(XObjName, "Заготовка 1", 1);
            SetText(XObjWidth, "2", 1);
            SetText(XObjHeight, "2", 1);
            Eval(Button_Click_1, 1);


            SetText(XObjWidth, "", 1);
            SetText(XObjHeight, "", 1);
            SetText(XObjName, "", 1);

            SetText(XObjWidth, "8", 1);
            SetText(XObjHeight, "3", 1);
            SetText(XObjName, "Заготовка 2", 1);
            Eval(Button_Click_1, 1);

            SetText(XObjWidth, "", 1);
            SetText(XObjHeight, "", 1);
            SetText(XObjName, "", 1);

            SetText(XObjWidth, "33", 1);
            SetText(XObjHeight, "4", 1);
            SetText(XObjName, "Заготовка 3", 1);
            Eval(Button_Click_1, 1);

            Eval(Button_Click_2, 1);   
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //test3
            XObjectsList.Items.Clear();
            SetText(XContainerWidth, "5", 1);
            SetText(XContainerHeight, "5", 1);

            SetText(XObjName, "Заготовка 1", 1);
            SetText(XObjWidth, "2", 1);
            SetText(XObjHeight, "3", 1);
            Eval(Button_Click_1, 1);


            SetText(XObjWidth, "", 1);
            SetText(XObjHeight, "", 1);
            SetText(XObjName, "", 1);

            SetText(XObjName, "Заготовка 2", 1);
            SetText(XObjWidth, "3", 1);
            SetText(XObjHeight, "2", 1);

            Eval(Button_Click_1, 1);
            Eval(Button_Click_2, 1);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            XObjectsList.Items.Clear();
            XContainerWidth.Clear();
            XContainerHeight.Clear();
            XObjHeight.Clear();
            XObjWidth.Clear();
            XContainerWidth.Clear();
            XObjName.Clear();
        }
    }
}
