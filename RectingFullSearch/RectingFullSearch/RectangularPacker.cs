using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;

namespace RectingFullSearch
{
    public struct Rect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string SName { get; set; }
        public string SSize { get { return Width.ToString() + "x" + Height.ToString(); } }
    }
    public class RectangularPacker
    {
        //[DllImport("RectFullSearchDll.dll", CharSet = CharSet.Auto)]
        //private static extern void RectingAlgorithm();
        public static MainWindow Parent;
        public static ItemCollection Rects;
        public static List<Rect>  Pack(int W, int H, ItemCollection rects)
        {
            var simple = _findSimpleSolutions(W,H,rects);
            if (simple != null)
            {
                return simple;
            }
            Rects = rects;
            string result="";
            result += W.ToString()+" "+H.ToString()+" "+ rects.Count.ToString()+"\r\n";
            foreach (Rect item in rects)
            { 
                result += item.Width + " " + item.Height+"\r\n";
            }
            File.WriteAllText("cSharpBridge.txt", result);
            Process p = Process.Start("RectingAlgorithmProcess.exe");
            p.WaitForExit();
            FileStream fs = new FileStream("cPlusPlusBridge.txt", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            int n = Convert.ToInt32(reader.ReadLine());
            List<Rect> temp = new List<Rect>();
            Rect t = new Rect();
            for (int i = 0; i < n; i++)
            {
                t.X = Convert.ToInt32(reader.ReadLine());
                t.Y = Convert.ToInt32(reader.ReadLine());
                int id = Convert.ToInt32(reader.ReadLine());
                t.Width = ((Rect)Rects[id]).Width;
                t.Height = ((Rect)Rects[id]).Height;
                t.SName = ((Rect)Rects[id]).SName;
                temp.Add(t);
            }
            return temp;         
        }

        private static List<Rect> _findSimpleSolutions(int W, int H, ItemCollection rects)
        {
            //made of one
            foreach (Rect a in rects)//перебор всех прямоугольников
            {
                if (W % a.Width == 0 && H % a.Height == 0)//проверка на возможность заполнения контейнера одним видом прямоугольника
                {
                    List<Rect> result = new List<Rect>();//список для хранения рашения
                    for (int i = 0; i < W / a.Width; i++)
                    {
                        for (int j = 0; j < H / a.Height; j++)
                        {
                            Rect t = new Rect();
                            t.Width = a.Width;
                            t.Height = a.Height;
                            t.X = i * a.Width;
                            t.Y = j * a.Height;
                            result.Add(t);//добавление прямоугольника к списку
                        }
                    }
                    return result;  
                }
            }
            //
            return _findMadeOfSome(W, H, rects);;
        }

        private static List<Rect> _findMadeOfSome(int W, int H, ItemCollection rects)
        {
            List<Rect> needW = new List<Rect>();
            List<Rect> needH = new List<Rect>();
            foreach (Rect r in rects)
            {
                if(W % r.Width==0)
                    needW.Add(r);
                if (H % r.Height == 0)
                    needH.Add(r);
            }
            var t = _findW(W, H, needW);
            if (t != null)
                return t;
            return _findH(W, H, needH);
        }

        private static List<Rect> _findH(int W, int H, List<Rect> needH,int offsetX=0)
        {
            for (int i = 0; i < needH.Count; i++)
            {
                if (W % needH[i].Width != 0 && W>needH[i].Width)
                {
                    var result = _findH(W % needH[i].Width, H, needH, offsetX + needH[i].Width);
                    if (result != null)
                    {
                        for (int j = 0; j < W / needH[i].Width; j++) 
                        {
                            for (int g = 0; g < H / needH[i].Height; g++)
                            {
                                Rect t = new Rect();
                                t.SName = needH[i].SName;
                                t.Width = needH[i].Width;
                                t.Height = needH[i].Height;
                                t.X = offsetX + needH[i].Width * j;
                                t.Y = g * needH[i].Height;
                                result.Add(t);
                            }
                        }
                        return result;
                    }
                }
                else if (W > needH[i].Width)
                {
                    List<Rect> result = new List<Rect>();
                    for (int j = 0; j < W / needH[i].Width; j++)
                    {
                        for (int g = 0; g < H / needH[i].Height; g++)
                        {
                            Rect t = new Rect();
                            t.SName = needH[i].SName;
                            t.Width = needH[i].Width;
                            t.Height = needH[i].Height;
                            t.X = offsetX+needH[i].Width*j;
                            t.Y = g * needH[i].Height;
                            result.Add(t);
                        }
                    }
                    return result;
                }
            }
            return null;
        }

        private static List<Rect> _findW(int W, int H, List<Rect> needW,int offsetY = 0)
        {
            for (int i = 0; i < needW.Count; i++)
            {
                if (H % needW[i].Height != 0 && H>needW[i].Height)
                {
                    var result = _findW(W, H % needW[i].Height, needW, offsetY + needW[i].Height);
                    if (result != null)
                    {
                        for (int g = 0; g < W / needW[i].Width; g++)
                        {
                            for (int j = 0; j < H / needW[i].Height; j++)
                            {
                                Rect t = new Rect();
                                t.SName = needW[i].SName;
                                t.Width = needW[i].Width;
                                t.Height = needW[i].Height;
                                t.X = g * needW[i].Width;
                                t.Y = offsetY+needW[i].Height*j;
                                result.Add(t);
                            }
                        }
                        return result;
                    }
                }
                else if (H > needW[i].Height)
                {
                    List<Rect> result = new List<Rect>();
                    for (int g = 0; g < W / needW[i].Width; g++)
                    {
                        for (int j = 0; j < H / needW[i].Height; j++)
                        {
                            Rect t = new Rect();
                            t.SName = needW[i].SName;
                            t.Width = needW[i].Width;
                            t.Height = needW[i].Height;
                            t.X = g * needW[i].Width;
                            t.Y = offsetY + needW[i].Height * j;
                            result.Add(t);
                        }
                    }
                    return result;
                }
            }
            return null;           
        }

    }
}
