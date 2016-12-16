using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectangles
{
    
    public class Rectangle//класс прямоугольников
    {
        public double X = 0;
        public double Y = 0;
        public double Height = 0;
        public double Width = 0;
        public Rectangle() { }
        public Rectangle(double x, double y, double height, double width)//конструктор, ставит значения точек и параметров
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
    
}
