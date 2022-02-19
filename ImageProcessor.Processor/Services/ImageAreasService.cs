using ImageProcessor.Processor.Interfaces;
using ImageProcessor.Processor.Services.Support;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Processor.Services
{
    public class ImageAreasService : IImageAreasService
    {
        private Bitmap _imageBitmap;

        public byte[] MagicWand(byte[] image, int X, int Y, int level)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);

            if (_imageBitmap == null) { return null; }

            Color picked = _imageBitmap.GetPixel(X, Y);
            int[,] B = new int[_imageBitmap.Width, _imageBitmap.Height];

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    Color curPixel = _imageBitmap.GetPixel(iX, iY);
                    B[iX, iY] = (ColorDistance(picked, curPixel) <= level) ? 1 : 0;
                }
            }
            FloodFill(X, Y, ref B);
            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    if (B[iX, iY] == 2) _imageBitmap.SetPixel(iX, iY, Color.FromArgb(255, 150, 150, 150));
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }

        private static double ColorDistance(Color first, Color second)
        {
            return Math.Sqrt(Math.Pow((second.R - first.R), 2) + Math.Pow((second.G - first.G), 2) + Math.Pow((second.B - first.B), 2));
        }

        private void FloodFill(int x, int y, ref int[,] B)
        {
            Stack<Point> pixelStack = new();
            pixelStack.Push(new Point(x, y));

            while (pixelStack.Count > 0)
            {
                Point current = pixelStack.Pop();
                if (current.X < 0 || current.Y < 0 || current.X >= _imageBitmap.Width - 1 || current.Y >= _imageBitmap.Height - 1)
                {
                    continue;
                }
                if (B[current.X, current.Y] == 1)
                {
                    B[current.X, current.Y] = 2;
                    pixelStack.Push(new Point(current.X, current.Y + 1));
                    pixelStack.Push(new Point(current.X, current.Y - 1));
                    pixelStack.Push(new Point(current.X + 1, current.Y));
                    pixelStack.Push(new Point(current.X - 1, current.Y));
                }

            }


        }

        public byte[] Dekstra(byte[] image, int X1, int Y1, int X2, int Y2)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);
            if (_imageBitmap == null) { return null; }

            List<Rebro> rebra = new();
            List<Vertex> vertexes = new();

            int iY1 = Math.Min(Y1, Y2)-10;
            int iY2 = Math.Max(Y1, Y2)+10;
            int iX1 = Math.Min(X1, X2)-10;
            int iX2 = Math.Max(X1, X2)+10;

            iY1 = (iY1 < 0) ? 0 : iY1;
            iY2 = (iY2 > _imageBitmap.Height) ? _imageBitmap.Height-1 : iY2;
            iX1 = (iX1 < 0) ? 0 : iX1;
            iX2 = (iX2 > _imageBitmap.Width) ? _imageBitmap.Width-1 : iX2;

            int iHeight = iY2 - iY1+1;
            int iWidth = iX2 - iX1+1;

            for (int iY = iY1; iY < iY1 + iHeight; iY++)
            {
                for (int iX = iX1; iX < iX1 + iWidth; iX++)
                {
                    Vertex vertex = new(9999, false, new Point(iX, iY));
                    if (iX == X1 && iY == Y1) vertex.ValueMetka = 0;
                    vertexes.Add(vertex);
                }
            }

            for (int iY = iY1; iY < iY1 + iHeight; iY++)
            {
                for (int iX = iX1 + 1; iX < iX1 + iWidth; iX++)
                {
                    Color point1 = _imageBitmap.GetPixel(iX - 1, iY);
                    Color point2 = _imageBitmap.GetPixel(iX, iY);
                    float weight = (float)(1 / ColorDistance(point1, point2));

                    Vertex vertexFirst = vertexes[((iY - iY1) * iWidth) + (iX - iX1 - 1)];
                    Vertex vertexSecond = vertexes[((iY - iY1) * iWidth) + (iX - iX1)];
                    vertexFirst.NeighborsVertex.Add(vertexSecond);
                    vertexSecond.NeighborsVertex.Add(vertexFirst);
                    rebra.Add(new Rebro(vertexFirst, vertexSecond, weight));
                }
            }
            for (int iY = iY1 + 1; iY < iY1 + iHeight; iY++)
            {
                for (int iX = iX1; iX < iX1 + iWidth; iX++)
                {
                    Color point1 = _imageBitmap.GetPixel(iX, iY - 1);
                    Color point2 = _imageBitmap.GetPixel(iX, iY);
                    float weight = (float)(1 / ColorDistance(point1, point2));

                    Vertex vertexFirst = vertexes[((iY - iY1 - 1) * iWidth) + (iX - iX1)];
                    Vertex vertexSecond = vertexes[((iY - iY1) * iWidth) + (iX - iX1)];
                    vertexFirst.NeighborsVertex.Add(vertexSecond);
                    vertexSecond.NeighborsVertex.Add(vertexFirst);
                    rebra.Add(new Rebro(vertexFirst, vertexSecond, weight));
                }
            }

            DekstraAlgorim dekstraAlgorim = new(vertexes.ToArray(), rebra.ToArray());
            dekstraAlgorim.AlgoritmRun(vertexes[((Y1 - iY1) * iWidth) + (X1 - iX1)]);
            var contur = dekstraAlgorim.MinPath(vertexes[((Y2 - iY1) * iWidth) + (X2 - iX1)]);

            foreach(Vertex vert in contur)
            {
                Point point = vert.PixelCords;
                _imageBitmap.SetPixel(point.X, point.Y, Color.FromArgb(255, 255, 0, 0));
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }

        public byte[] Clustarization(byte[] image)
        {
            _imageBitmap = Conventor.BytesToBitmap(image);

            if (_imageBitmap == null) { return null; }
            int[,] B = new int[_imageBitmap.Width, _imageBitmap.Height];

            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    if (B[iX, iY] != 0) continue;
                    FindAreas(iX, iY, ref B);
                    FillAreas(iX, iY, ref B);
                }
            }

            return Conventor.BitmapToBytes(_imageBitmap);
        }

        private void FindAreas(int x, int y, ref int[,] B)
        {
            Color picked = _imageBitmap.GetPixel(x, y);
            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    if (B[iX, iY] != 0) continue;
                    Color curPixel = _imageBitmap.GetPixel(iX, iY);
                    B[iX, iY] = (ColorDistance(picked, curPixel) <= 50) ? 1 : 0;
                }
            }
        }

        private void FillAreas(int x, int y, ref int[,] B)
        {
            Color picked = _imageBitmap.GetPixel(x, y);
            for (int iX = 0; iX < _imageBitmap.Width; iX++)
            {
                for (int iY = 0; iY < _imageBitmap.Height; iY++)
                {
                    if (B[iX, iY] == 1) 
                    {
                        B[iX, iY] = 2;
                        _imageBitmap.SetPixel(iX, iY, picked);
                    } 
                }
            }
        }

    }

    public class Vertex
    {
        public float ValueMetka { get; set; }
        public bool IsChecked { get; set; }
        public Vertex PrevVertex { get; set; }
        public Point PixelCords { get; set; }
        public List<Vertex> NeighborsVertex { get; set; } = new();

        public Vertex(int value, bool ischecked)
        {
            ValueMetka = value;
            IsChecked = ischecked;
            PrevVertex = new Vertex();
        }
        public Vertex(int value, bool ischecked, Point cords)
        {
            ValueMetka = value;
            IsChecked = ischecked;
            PixelCords = cords;
            PrevVertex = new Vertex();
        }
        public Vertex()
        {
        }
    }

    public class Rebro
    {
        public Vertex FirstVertex { get; private set; }
        public Vertex SecondPoint { get; private set; }
        public float Weight { get; private set; }

        public Rebro(Vertex first, Vertex second, float valueOfWeight)
        {
            FirstVertex = first;
            SecondPoint = second;
            Weight = valueOfWeight;
        }

    }

    class DekstraAlgorim
    {
        public Vertex[] Vertexes { get; private set; }
        public Rebro[] Rebra { get; private set; }
        public Vertex StartVertex { get; private set; }

        public DekstraAlgorim(Vertex[] pointsOfgraph, Rebro[] rebraOfgraph)
        {
            Vertexes = pointsOfgraph;
            Rebra = rebraOfgraph;
        }


        public void AlgoritmRun(Vertex beginPoint)
        {
            if (Vertexes.Length == 0 || Rebra.Length == 0) return;

            StartVertex = beginPoint;
            OneStep(beginPoint);

            while (true)
            {
                Vertex anotherP = GetAnotherUncheckedPoint();
                if (anotherP == null)break;
                OneStep(anotherP);
            }
        }


        public void OneStep(Vertex beginPoint)
        {
            foreach (Vertex neighbor in Neighbors(beginPoint))
            {
                if (neighbor.IsChecked == false)//не отмечена
                {
                    float newMetka = beginPoint.ValueMetka + GetMyRebro(neighbor, beginPoint).Weight;
                    if (neighbor.ValueMetka > newMetka)
                    {
                        neighbor.ValueMetka = newMetka;
                        neighbor.PrevVertex = beginPoint;
                    }
                }
            }
            beginPoint.IsChecked = true;//вычеркиваем
        }


        private IEnumerable<Vertex> Neighbors(Vertex currpoint)
        {
            //IEnumerable<Vertex> firstpoints = Rebra.Where(fp => fp.FirstVertex == currpoint).Select(fp => fp.SecondPoint);
            //IEnumerable<Vertex> secondpoints = Rebra.Where(sp => sp.SecondPoint == currpoint).Select(sp => sp.FirstVertex);
            //IEnumerable<Vertex> totalpoints = firstpoints.Concat<Vertex>(secondpoints);
            //return totalpoints;
            return currpoint.NeighborsVertex;
        }


        private Rebro GetMyRebro(Vertex a, Vertex b)
        {//ищем ребро по 2 точкам
            IEnumerable<Rebro> myr = Rebra.Where(reb => (reb.FirstVertex == a & reb.SecondPoint == b) || (reb.SecondPoint == a & reb.FirstVertex == b));
            if (myr.Count() > 1 || !myr.Any())
            {
                throw new Exception("Не найдено ребро между соседями!");
            }
            else
            {
                return myr.First();
            }
        }
        

        /// <summary>
        /// Получаем очередную неотмеченную вершину, "ближайшую" к заданной.
        /// </summary>
        /// <returns></returns>
        private Vertex GetAnotherUncheckedPoint()
        {
            IEnumerable<Vertex> pointsUncheck = from p in Vertexes where p.IsChecked == false select p;

            if (!pointsUncheck.Any()) return null;
            
            Vertex minPoint = pointsUncheck.First();
            float minVal = minPoint.ValueMetka;
            
            foreach (Vertex p in pointsUncheck)
            {
                if (p.ValueMetka < minVal)
                {
                    minVal = p.ValueMetka;
                    minPoint = p;
                }
            }
            return minPoint;
        }


        public List<Vertex> MinPath(Vertex end)
        {
            List<Vertex> listOfpoints = new();
            Vertex temp = end;
            while (temp != StartVertex)
            {
                listOfpoints.Add(temp);
                temp = temp.PrevVertex;
            }

            return listOfpoints;
        }
    }
}
