using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Rectangles
{
    public partial class Form1 : Form
    {
        private Rectangle first, second;
        private const int RECTS_COUNT = 2;
        private const int MARGIN = 10;
        double minX=0, minY=0, maxX=0, maxY=0;
        public Form1()
        {
            InitializeComponent();
            //first = new Rectangle();
            //second = new Rectangle();
        }

        private bool rectsAreSetted()
        {
            if (first != null && second != null) return true;
            return false;
        }

        private void saveRectanglesToXMl(string fileName)//сохранение треугольников
        {
            Rectangle[] rectangles = { first, second };
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml("<rectangles></rectangles>");//создаем корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            for (int i=0;i<RECTS_COUNT;i++)
            {
                XmlElement xRect = xDoc.CreateElement("rectangle");//создаем элементы треугольник и координаты и параметров
                XmlElement xX = xDoc.CreateElement("X");
                XmlElement xY = xDoc.CreateElement("Y");
                XmlElement xWidth = xDoc.CreateElement("Width");
                XmlElement xHeight = xDoc.CreateElement("Height");
                XmlText xText = xDoc.CreateTextNode(rectangles[i].X.ToString());
                XmlText yText = xDoc.CreateTextNode(rectangles[i].Y.ToString());
                XmlText widthText = xDoc.CreateTextNode(rectangles[i].Width.ToString());
                XmlText heightText = xDoc.CreateTextNode(rectangles[i].Height.ToString());
                xX.AppendChild(xText);
                xY.AppendChild(yText);
                xWidth.AppendChild(widthText);
                xHeight.AppendChild(heightText);
                xRect.AppendChild(xX);
                xRect.AppendChild(xY);
                xRect.AppendChild(xWidth);
                xRect.AppendChild(xHeight);
                xRoot.AppendChild(xRect);//пакуем наш XML
            }
            xDoc.Save(fileName);//и сохраняем

        }

        private void saveXMLToolStripMenuItem_Click(object sender, EventArgs e)//функция на пункт меню записать в xml
        {
            if (!rectsAreSetted())
            {
                MessageBox.Show("You should set rectangles!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XMl Files (*.xml)|*.xml|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)//открываем окно сохранения файла и если все ок, то
            {
                saveRectanglesToXMl(sfd.FileName);//сохраняем в XML
            }
        }

        private void readXMLFile(string fileName)
        {
            first = new Rectangle();
            second = new Rectangle();
            Rectangle[] rectangles = { first, second };
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);//создаем корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            int i = 0;
            foreach(XmlNode rects in xRoot)
            {
                foreach(XmlNode param in rects.ChildNodes)
                {
                    if (param.Name == "X") rectangles[i].X = double.Parse(param.InnerText);
                    if (param.Name == "Y") rectangles[i].Y = double.Parse(param.InnerText);
                    if (param.Name == "Width") rectangles[i].Width = double.Parse(param.InnerText);
                    if (param.Name == "Height") rectangles[i].Height = double.Parse(param.InnerText);
                }
                i++;
            }
        }

        private void openXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XMl Files (*.xml)|*.xml|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                readXMLFile(ofd.FileName);
            }
            setUpMinMaxValues();
            paintRectangles();

        }

        private void setUpMinMaxValues()
        {
            Rectangle[] rectangles = { first, second };
            for (int i=0;i<RECTS_COUNT;i++)
            {
                if (rectangles[i].X < minX) minX = rectangles[i].X;
                if (rectangles[i].X > maxX) maxX = rectangles[i].X;
                if (rectangles[i].X + rectangles[i].Width < minX) minX = rectangles[i].X + rectangles[i].Width;
                if (rectangles[i].X + rectangles[i].Width > maxX) maxX = rectangles[i].X + rectangles[i].Width;
                if (rectangles[i].Y < minY) minY = rectangles[i].Y;
                if (rectangles[i].Y > maxY) maxY = rectangles[i].Y;
                if (rectangles[i].Y + rectangles[i].Height < minY) minY = rectangles[i].Y + rectangles[i].Height;
                if (rectangles[i].Y + rectangles[i].Height > maxY) maxY = rectangles[i].Y + rectangles[i].Height;
            }
        }

        private int scaledX(double oldX)
        {
            return (int)(((oldX - minX) * (pGraphics.Width - 2 * MARGIN)) / (maxX - minX) + MARGIN);
        }

        private int scaledY(double oldY)
        {
            return (int)(((oldY - minY) * (pGraphics.Height - 2 * MARGIN)) / (maxY - minY) + MARGIN);
        }

        private void paintRectangles()
        {
            Graphics grfx = pGraphics.CreateGraphics();
            grfx.Clear(Color.White);
            Pen firstPen = new Pen(Color.Green, 2);
            Pen secondPen = new Pen(Color.Red, 2);
            //Draw first rectangle
            grfx.DrawLine(firstPen, scaledX(first.X), scaledY(first.Y), scaledX(first.X + first.Width), scaledY(first.Y));
            grfx.DrawLine(firstPen, scaledX(first.X + first.Width), scaledY(first.Y), scaledX(first.X + first.Width), scaledY(first.Y+first.Height));
            grfx.DrawLine(firstPen, scaledX(first.X + first.Width), scaledY(first.Y + first.Height), scaledX(first.X), scaledY(first.Y + first.Height));
            grfx.DrawLine(firstPen, scaledX(first.X), scaledY(first.Y + first.Height), scaledX(first.X), scaledY(first.Y));
            //Draw second rectangle
            grfx.DrawLine(secondPen, scaledX(second.X), scaledY(second.Y), scaledX(second.X + second.Width), scaledY(second.Y));
            grfx.DrawLine(secondPen, scaledX(second.X + second.Width), scaledY(second.Y), scaledX(second.X + second.Width), scaledY(second.Y + second.Height));
            grfx.DrawLine(secondPen, scaledX(second.X + second.Width), scaledY(second.Y + second.Height), scaledX(second.X), scaledY(second.Y + second.Height));
            grfx.DrawLine(secondPen, scaledX(second.X), scaledY(second.Y + second.Height), scaledX(second.X), scaledY(second.Y));
            grfx.Dispose();
        }
    }
}
