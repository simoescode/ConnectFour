using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class Quadrilateral
    {
        private int _horizontalWest;
        private int _verticalNorth;
        private int _horizontalEast;
        private int _verticalSouth;

        public Quadrilateral(int horizontalWest, int verticalNorth, int horizontalEast, int verticalSouth)
        {
            HorizontalWest = horizontalWest;
            VerticalNorth = verticalNorth;
            HorizontalEast = horizontalEast;
            VerticalSouth = verticalSouth;
        }

        public int HorizontalWest
        {
            get
            {
                return _horizontalWest;
            }
            set
            {
                if (value >= 0 && value <= Console.WindowWidth)
                {
                    _horizontalWest = value;
                }
                else
                {
                    throw new System.ArgumentException("INVALID COORDINATE: ");
                }
            }
        }

        public int VerticalNorth
        {
            get
            {
                return _verticalNorth;
            }
            set
            {
                if (value >= 0 && value <= Console.WindowHeight)
                {
                    _verticalNorth = value;
                }
                else
                {
                    throw new System.ArgumentException("INVALID COORDINATE: ");
                }
            }
        }

        public int HorizontalEast
        {
            get
            {
                return _horizontalEast;
            }
            set
            {
                if (value >= 0 && value <= Console.WindowWidth && value > _horizontalWest)
                {
                    _horizontalEast = value;
                }
                else
                {
                    throw new System.ArgumentException("INVALID COORDINATE: ");
                }
            }
        }

        public int VerticalSouth
        {
            get
            {
                return _verticalSouth;
            }
            set
            {
                if (value >= 0 && value <= Console.WindowHeight && value > _verticalNorth)
                {
                    _verticalSouth = value;
                }
                else
                {
                    throw new System.ArgumentException("INVALID COORDINATE: ");
                }
            }
        }

        public void DrawBox(bool draw)
        {
            if (draw)
            {
                Console.SetCursorPosition(_horizontalWest, _verticalNorth);
                Console.Write("┌");
                Console.SetCursorPosition(_horizontalEast, _verticalNorth);
                Console.Write("┐");
                Console.SetCursorPosition(_horizontalWest, _verticalSouth);
                Console.Write("└");
                Console.SetCursorPosition(_horizontalEast, _verticalSouth);
                Console.Write("┘");
                for (int i = _horizontalWest + 1; i <= _horizontalEast - 1; i++)
                {
                    Console.SetCursorPosition(i, _verticalNorth);
                    Console.Write("─");
                    Console.SetCursorPosition(i, _verticalSouth);
                    Console.Write("─");
                }
                for (int i = _verticalNorth + 1; i <= _verticalSouth - 1; i++)
                {
                    Console.SetCursorPosition(_horizontalWest, i);
                    Console.Write("│");
                    Console.SetCursorPosition(_horizontalEast, i);
                    Console.Write("│");
                }
            }
            else
            {
                Console.SetCursorPosition(_horizontalWest, _verticalNorth);
                Console.Write(" ");
                Console.SetCursorPosition(_horizontalEast, _verticalNorth);
                Console.Write(" ");
                Console.SetCursorPosition(_horizontalWest, _verticalSouth);
                Console.Write(" ");
                Console.SetCursorPosition(_horizontalEast, _verticalSouth);
                Console.Write(" ");
                for (int i = _horizontalWest + 1; i <= _horizontalEast - 1; i++)
                {
                    Console.SetCursorPosition(i, _verticalNorth);
                    Console.Write(" ");
                    Console.SetCursorPosition(i, _verticalSouth);
                    Console.Write(" ");
                }
                for (int i = _verticalNorth + 1; i <= _verticalSouth - 1; i++)
                {
                    Console.SetCursorPosition(_horizontalWest, i);
                    Console.Write(" ");
                    Console.SetCursorPosition(_horizontalEast, i);
                    Console.Write(" ");
                }
            }
        }

        public static void Boxout()
        {
            int yRatio;
            int xRatio;
            Quadrilateral box = new Quadrilateral(2, 2, Console.WindowWidth - 2, Console.WindowHeight - 2);
            if (Console.WindowWidth > Console.WindowHeight)
            {
                yRatio = 1;
                xRatio = Console.WindowWidth / Console.WindowHeight;
            }
            else
            {
                xRatio = 1;
                yRatio = Console.WindowHeight / Console.WindowWidth;
            }

            Console.CursorVisible = false;
            int i = 0;

            while (box.VerticalSouth > box.VerticalNorth + 2 * yRatio && box.HorizontalEast > box.HorizontalWest + 2 * xRatio)
            {
                Console.ForegroundColor = (ConsoleColor)((i++ % 15) + 1);
                box.DrawBox(true);
                System.Threading.Thread.Sleep(25);
                box.DrawBox(false);

                box.VerticalNorth += yRatio;
                box.VerticalSouth -= yRatio;
                box.HorizontalWest += xRatio;
                box.HorizontalEast -= xRatio;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
