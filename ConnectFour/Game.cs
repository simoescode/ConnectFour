using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class Game
    {
        public const int SCREENWIDTH = 71;
        public const int SCREENHEIGHT = 45;        
        private int[] _squares;
        private int _column;
        private bool _player;
        private string[] _names;
        private int _mode;
        private int[] _wins = new int[2];
        private bool _playing;
        private static readonly Random rand = new Random();

        public Game(int mode)
        {
            if (mode > 4 && mode < 16 && mode != 12 && mode != 8)
            {
                _squares = new int[42];
                _player = true;
                _mode = mode;
                _names = new string[2];
                ComputerName();
            }
            else
            {
                throw new NotImplementedException();
            }
            DrawBaord();
            _playing = true;
            NewToken();
        }

        public Game(string[] aliases)
        {
            _squares = new int[42];
            _player = true;
            _names = new string[2];
            if (aliases.Length > 1)
            {
                Names = aliases;
                _mode = 0;
            }
            else if (aliases.Length == 1)
            {
                Names = aliases;
                _mode = 2;
                ComputerName();
            }
            else
            {
                _mode = 10;
                ComputerName();
            }
            DrawBaord();
            _playing = true;
            NewToken();
        }

        public Game(string[] aliases, int mode)
        {
            if (aliases.Length == 1 && ((mode > 0 && mode < 5) || mode == 8 || mode == 12))
            {
                _squares = new int[42];
                _player = true;
                _mode = mode;
                _names = new string[2];
                if (mode < 4)
                {
                    Names = aliases;
                    ComputerName();
                }
                else
                {
                    string[] temp = { "", aliases[0] };
                    Names = temp;
                    ComputerName();
                }
            }
            else if (aliases.Length == 0 && mode > 4 && mode < 16 && mode != 12 && mode != 8)
            {
                _squares = new int[42];
                _player = true;
                _mode = mode;
                ComputerName();
            }
            else if (aliases.Length == 2 & mode == 0)
            {
                _squares = new int[42];
                _player = true;
                _mode = mode;
                Names = aliases;
            }
            else
            {
                throw new NotImplementedException();
            }
            DrawBaord();
            _playing = true;
            NewToken();
        }

        private string[] Names
        {
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Length > 16)
                    {
                        value[i] = value[i].Substring(0, 16).Trim();
                    }
                    if (i != 0 && value[i - 1] == value[i])
                    {
                        if (value[i].Length == 16)
                        {
                            value[i] = value[i].Substring(0, 15).Trim();
                        }
                        value[i] += (i + 1).ToString();
                    }
                    _names[i] = value[i];
                }
            }
        }

        public int Mode
        {
            get
            {
                return _mode;
            }
        } 

        public bool Playing
        {
            get
            {
                return _playing;
            }
        }

        public bool Player
        {
            get
            {
                return _player;
            }
        }

        private void ComputerName()
        {
            switch (_mode / 4)
            {
                case 1:
                    _names[0] = "HAL 9000 (E)";
                    break;
                case 2:
                    _names[0] = "HAL 9000 (M)";
                    break;
                case 3:
                    _names[0] = "HAL 9000 (H)";
                    break;
            }
            switch (_mode % 4)
            {
                case 1:
                    _names[1] = "Deep Thought (E)";
                    break;
                case 2:
                    _names[1] = "Deep Thought (M)";
                    break;
                case 3:
                    _names[1] = "Deep Thought (H)";
                    break;
            }
        }

        private void NewToken()
        {
            ConfirmWindowsSize();
            if (!_playing)
            {
                _column = -1;
                UpdatePlayerTurn();
                return;
            }
            else if (_squares[38] == 0)
            {
                _column = 3;
            }
            else if (_squares[39] == 0)
            {
                _column = 4;
            }
            else if (_squares[37] == 0)
            {
                _column = 2;
            }
            else if (_squares[40] == 0)
            {
                _column = 5;
            }
            else if (_squares[36] == 0)
            {
                _column = 1;
            }
            else if (_squares[41] == 0)
            {
                _column = 6;
            }
            else if (_squares[35] == 0)
            {
                _column = 0;
            }
            else
            {
                _column = -1;
                return;
            }
            UpdatePlayerTurn();

            if (_player)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            int leftX = _column * 10 + 1;
            for (int y = 1; y < 5; y++)
            {
                for (int x = leftX; x < leftX + 9; x++)
                {
                    if (!((y == 1 || y == 4) && (x < leftX + 1 || x > leftX + 7)))
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("@");
                    }
                }
            }
            if (_player && _mode > 3)
            {
                System.Threading.Thread.Sleep(1500);
                switch (_mode / 4)
                {
                    case 1:
                        MoveToken(EasyPlay(_squares));
                        break;
                    case 2:
                        MoveToken(MediumPlay(_squares));
                        break;
                    case 3:
                        MoveToken(HardPlay(_squares));
                        break;
                }
                System.Threading.Thread.Sleep(500);
                DropToken();
            }
            else if (!_player && _mode % 4 != 0)
            {
                System.Threading.Thread.Sleep(1500);
                switch (_mode % 4)
                {
                    case 1:
                        MoveToken(EasyPlay(_squares));
                        break;
                    case 2:
                        MoveToken(MediumPlay(_squares));
                        break;
                    case 3:
                        MoveToken(HardPlay(_squares));
                        break;
                }
                System.Threading.Thread.Sleep(500);
                DropToken();
            }            
        }

        public void MoveRight()
        {
            ConfirmWindowsSize();
            if (_column != 6 && _column != -1)
            {
                int column = _column + 36;

                for (int i = _column + 1; i < 7; i++)
                {
                    if (_squares[column] == 0)
                    {
                        MoveToken(i);
                        return;
                    }
                    column++;
                }
            }
        }

        public void MoveLeft()
        {
            ConfirmWindowsSize(); 
            if (_column > 0)
            {
                int column = _column + 34;

                for (int i = _column - 1; i > -1; i--)
                {
                    if (_squares[column] == 0)
                    {
                        MoveToken(i);
                        return;
                    }
                    column--;
                }
            }
        }

        private void MoveToken(int newColumn)
        {
            if (_player)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            if (_column != newColumn)
            {
                int leftX = _column * 10 + 1;
                for (int y = 1; y < 5; y++)
                {
                    Console.SetCursorPosition(leftX, y);
                    Console.Write("         ");
                }

                _column = newColumn;
                leftX = _column * 10 + 1;
                for (int y = 1; y < 5; y++)
                {
                    for (int x = leftX; x < leftX + 9; x++)
                    {
                        if (!((y == 1 || y == 4) && (x < leftX + 1 || x > leftX + 7)))
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write("@");
                        }
                    }
                }
            }
        }

        private void DrawBaord()
        {
            int leftX = 0;
            int topY = 5;
            int rightX = 70;
            int bottomY = 35;

            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Blue;


            Console.SetCursorPosition(leftX, topY);
            Console.Write("┌");
            Console.SetCursorPosition(rightX, topY);
            Console.Write("┐");
            Console.SetCursorPosition(leftX, bottomY);
            Console.Write("└");
            Console.SetCursorPosition(rightX, bottomY);
            Console.Write("┘");

            for (int x = leftX + 1; x < rightX; x++)
            {
                if (x % 10 != 0)
                {
                    Console.SetCursorPosition(x, topY);
                    Console.Write("─");
                    Console.SetCursorPosition(x, bottomY);
                    Console.Write("─");
                }
                else
                {
                    Console.SetCursorPosition(x, topY);
                    Console.Write("┬");
                    Console.SetCursorPosition(x, bottomY);
                    Console.Write("┴");
                }
            }
            for (int y = topY + 1; y < bottomY; y++)
            {
                if (y % 5 != 0)
                {
                    Console.SetCursorPosition(leftX, y);
                    Console.Write("│");
                    Console.SetCursorPosition(rightX, y);
                    Console.Write("│");
                }
                else
                {
                    Console.SetCursorPosition(leftX, y);
                    Console.Write("├");
                    Console.SetCursorPosition(rightX, y);
                    Console.Write("┤");
                }
            }

            for (int y = topY + 5; y < bottomY; y += 5)
            {
                for (int x = leftX + 1; x < rightX; x++)
                {
                    if (x % 10 != 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("─");
                    }
                    else
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("┼");
                    }
                }
            }

            for (int x = leftX + 10; x < rightX; x += 10)
            {
                for (int y = topY + 1; y < bottomY; y++)
                {
                    if (y % 5 != 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("│");
                    }
                }
            }
            Legend();
            ScoreBox();
        }

        public void DropToken()
        {
            int leftX = _column * 10 + 1;
            int bottomY = 10 + (LowestEmptyRow(_squares) * 5);

            if (_player)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            for (int topY = 1; topY < bottomY - 4; topY++)
            {
                for (int y = topY; y < topY + 4; y++)
                {
                    if (y % 5 != 0)
                    {
                        Console.SetCursorPosition(leftX, y);
                        Console.Write("         ");
                    }
                }
                for (int y = topY + 1; y < topY + 5; y++)
                {
                    for (int x = leftX; x < leftX + 9; x++)
                    {
                        if ((!((y == topY + 1 || y == topY + 4) && (x < leftX + 1 || x > leftX + 7))) && y % 5 != 0)
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write("@");
                        }
                    }
                }
                System.Threading.Thread.Sleep(15);
            }
            _squares[_column + (5 - LowestEmptyRow(_squares)) * 7] = Convert.ToInt32(_player) * 2 - 1;
            if (LowestEmptyRow(_squares) == -1)
            {
                if (_squares[35] != 0 && _squares[36] != 0 && _squares[37] != 0 && _squares[38] != 0 && _squares[39] != 0 && _squares[40] != 0 && _squares[41] != 0)
                {
                    _column = -1;
                }
            }
            if (Math.Abs(Evaluate(_squares)) > 900)
            {
                ShowWinner();
            }
            else if (_column != -1)
            {
                _player = !_player;
                NewToken();
            }
            else
            {
                _playing = false;
                UpdatePlayerTurn();
            }
            ConfirmWindowsSize();
        }

        private int LowestEmptyRow(int[] grid, int column = -2)
        {
            if (column == -2)
            {
                column = _column;
            }
            if (column > -1 && column < 7)
            {
                for (int i = 5; i > -1; i--)
                {
                    if (grid[column] == 0)
                    {
                        return i;
                    }
                    column += 7;
                }
            }
            return -1;
        }

        private void ShowWinner()
        {
            ConfirmWindowsSize(); 
            if (_player)
            {
                _wins[0]++;
                Console.SetCursorPosition(1, 42);
                Console.Write(_wins[0].ToString("D2"));
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else
            {
                _wins[1]++;
                Console.SetCursorPosition(41, 42);
                Console.Write(_wins[1].ToString("D2"));
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            Console.ForegroundColor = ConsoleColor.Black;

            for (int i = 0; i < 42; i++)
            {
                if (Math.Abs(_squares[i]) > 900)
                {
                    int upperY = (6 - (i / 7)) * 5 + 1;
                    int leftX = (i % 7) * 10 + 1;
                    for (int y = upperY; y < upperY + 4; y++)
                    {
                        for (int x = leftX; x < leftX + 9; x++)
                        {
                            if (!((y == upperY || y == upperY + 3) && (x < leftX + 1 || x > leftX + 7)))
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write("@");
                            }
                        }
                    }
                }
            }
            
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            _playing = false;
            UpdatePlayerTurn();
        }
        
        private void UpdateTokens()
        {
            for (int i = 0; i < 42; i++)
            {
                if (_squares[i] != 0)
                {
                    int upperY = (6 - (i / 7)) * 5 + 1;
                    int leftX = (i % 7) * 10 + 1;
                    if (_squares[i] > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    for (int y = upperY; y < upperY + 4; y++)
                    {
                        for (int x = leftX; x < leftX + 9; x++)
                        {
                            if (!((y == upperY || y == upperY + 3) && (x < leftX + 1 || x > leftX + 7)))
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write("@");
                            }
                        }
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Legend();
            ScoreBox(); 
        }

        private void Legend()
        { 
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetCursorPosition(49, 37);
            Console.Write("CURSOR KEY LEGEND");
            Console.SetCursorPosition(45, 39);
            Console.Write("← or →        move token");
            Console.SetCursorPosition(45, 40);
            Console.Write("↓ or [enter]  drop token");
            Console.SetCursorPosition(45, 41);
            Console.Write("R             Reset game");
            Console.SetCursorPosition(45, 42);
            Console.Write("Q             Quit");
            Quadrilateral box = new Quadrilateral(44, 36, 70, 43);
            box.DrawBox(true);
        }

        private void ScoreBox()
        {  
            Console.ForegroundColor = ConsoleColor.Gray;
            int vs = _names[0].Length + _names[1].Length + 3;
            Console.SetCursorPosition(1, 37);
            Console.Write("Playing:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(41, 42);
            Console.Write(_wins[1].ToString("D2"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(1, 42);
            Console.Write(_wins[0].ToString("D2"));
            Console.SetCursorPosition((42 - vs) / 2 + 1, 42);
            Console.Write(_names[0]);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(_names[1]);
            Console.ForegroundColor = ConsoleColor.Gray;
            Quadrilateral box = new Quadrilateral(0, 36, 43, 43);
            box.DrawBox(true);
        }

        private void UpdatePlayerTurn()
        {
            if (_playing)
            {
                Console.SetCursorPosition(18, 37);
                Console.Write("                ");
                if (_player)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition((34 - _names[0].Length) / 2 + 9, 37);
                    Console.Write(_names[0]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (_mode > 3)
                    {
                        Console.SetCursorPosition(3, 39);
                        Console.Write("keyboard disabled while computer plays");
                    }
                    else if (_mode != 0)
                    {
                        Console.SetCursorPosition(3, 39);
                        Console.Write("                                      ");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition((34 - _names[1].Length) / 2 + 9, 37);
                    Console.Write(_names[1]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (_mode % 4 != 0)
                    {
                        Console.SetCursorPosition(3, 39);
                        Console.Write("keyboard disabled while computer plays");
                    }
                    else if (_mode != 0)
                    {
                        Console.SetCursorPosition(3, 39);
                        Console.Write("                                      ");
                    }
                }
            }
            else if (Evaluate(_squares) > 900)
            {
                Console.SetCursorPosition(1, 37);
                Console.Write("                                 ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((42 - _names[0].Length - 6) / 2 + 1, 37);
                Console.Write(_names[0] + " WINS!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(3, 39);
                Console.Write("           [R] for Rematch            ");
                Console.SetCursorPosition(9, 40);
                Console.Write("[Q] to Start Over or Quit");
            }
            else if (Evaluate(_squares) < -900)
            {
                Console.SetCursorPosition(1, 37);
                Console.Write("                                 ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition((42 - _names[1].Length - 6) / 2 + 1, 37);
                Console.Write(_names[1] + " WINS!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(3, 39);
                Console.Write("           [R] for Rematch            ");
                Console.SetCursorPosition(9, 40);
                Console.Write("[Q] to Start Over or Quit");
            }
            else
            {
                Console.SetCursorPosition(1, 37);
                Console.Write("                                 ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(12, 37);
                Console.Write("This game is a DRAW");
                Console.SetCursorPosition(3, 39);
                Console.Write("           [R] for Rematch            ");
                Console.SetCursorPosition(9, 40);
                Console.Write("[Q] to Start Over or Quit");
            }
        }

        public void ResumeGame(bool startOver)
        {
            if (startOver)
            {
                Array.Clear(_squares, 0, _squares.Length);
                _player = true;
                _playing = true;
            }            
            if (ConfirmWindowsSize())
            {
                Console.Clear();
                DrawBaord();
                UpdateTokens();
            }
            NewToken();
        }

        public bool ConfirmWindowsSize()
        {
            if (Console.WindowWidth != Game.SCREENWIDTH || Console.WindowHeight != Game.SCREENHEIGHT)
            {
                Console.BufferWidth = Console.WindowWidth = SCREENWIDTH;
                Console.BufferHeight = Console.WindowHeight = SCREENHEIGHT;
                Console.Clear();
                DrawBaord();
                UpdateTokens();
                return false;
            }
            return true;
        }

        private int Evaluate(int[] grid)
        {
            int score;

            if (grid[0] != 0 && Math.Abs(grid[0]) != 99)
            {
                score = Grade(grid[0], grid[1], grid[2], grid[3]) +
                        Grade(grid[0], grid[7], grid[14], grid[21]) +
                        Grade(grid[0], grid[8], grid[16], grid[24]);
                if (score == 0)
                {
                    grid[0] = (Convert.ToInt32(grid[0] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[0] = score;
                }
            }
            if (grid[1] != 0 && Math.Abs(grid[1]) != 99)
            {
                score = Grade(grid[1], grid[0], grid[2], grid[3]) +
                        Grade(grid[1], grid[2], grid[3], grid[4]) +
                        Grade(grid[1], grid[8], grid[15], grid[22]) +
                        Grade(grid[1], grid[9], grid[17], grid[25]);
                if (score == 0)
                {
                    grid[1] = (Convert.ToInt32(grid[1] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[1] = score;
                }
            }
            if (grid[2] != 0 && Math.Abs(grid[2]) != 99)
            {
                score = Grade(grid[2], grid[0], grid[1], grid[3]) +
                        Grade(grid[2], grid[1], grid[3], grid[4]) +
                        Grade(grid[2], grid[3], grid[4], grid[5]) +
                        Grade(grid[2], grid[9], grid[16], grid[23]) +
                        Grade(grid[2], grid[10], grid[18], grid[26]);
                if (score == 0)
                {
                    grid[2] = (Convert.ToInt32(grid[2] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[2] = score;
                }
            }
            if (grid[3] != 0 && Math.Abs(grid[3]) != 99)
            {
                score = Grade(grid[3], grid[0], grid[1], grid[2]) +
                        Grade(grid[3], grid[1], grid[2], grid[4]) +
                        Grade(grid[3], grid[2], grid[4], grid[5]) +
                        Grade(grid[3], grid[4], grid[5], grid[6]) +
                        Grade(grid[3], grid[10], grid[17], grid[24]) +
                        Grade(grid[3], grid[9], grid[15], grid[21]) +
                        Grade(grid[3], grid[11], grid[19], grid[27]);
                if (score == 0)
                {
                    grid[3] = (Convert.ToInt32(grid[3] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[3] = score;
                }
            }
            if (grid[4] != 0 && Math.Abs(grid[4]) != 99)
            {
                score = Grade(grid[4], grid[1], grid[2], grid[3]) +
                        Grade(grid[4], grid[2], grid[3], grid[5]) +
                        Grade(grid[4], grid[3], grid[5], grid[6]) +
                        Grade(grid[4], grid[11], grid[18], grid[25]) +
                        Grade(grid[4], grid[10], grid[16], grid[22]);
                if (score == 0)
                {
                    grid[4] = (Convert.ToInt32(grid[4] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[4] = score;
                }
            }
            if (grid[5] != 0 && Math.Abs(grid[5]) != 99)
            {
                score = Grade(grid[5], grid[2], grid[3], grid[4]) +
                        Grade(grid[5], grid[3], grid[4], grid[6]) +
                        Grade(grid[5], grid[12], grid[19], grid[26]) +
                        Grade(grid[5], grid[11], grid[17], grid[23]);
                if (score == 0)
                {
                    grid[5] = (Convert.ToInt32(grid[5] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[5] = score;
                }
            }
            if (grid[6] != 0 && Math.Abs(grid[6]) != 99)
            {
                score = Grade(grid[6], grid[3], grid[4], grid[5]) +
                        Grade(grid[6], grid[13], grid[20], grid[27]) +
                        Grade(grid[6], grid[12], grid[18], grid[24]);
                if (score == 0)
                {
                    grid[6] = (Convert.ToInt32(grid[6] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[6] = score;
                }
            }
            if (grid[7] != 0 && Math.Abs(grid[7]) != 99)
            {
                score = Grade(grid[7], grid[8], grid[9], grid[10]) +
                        Grade(grid[7], grid[0], grid[14], grid[21]) +
                        Grade(grid[7], grid[14], grid[21], grid[28]) +
                        Grade(grid[7], grid[15], grid[23], grid[31]);
                if (score == 0)
                {
                    grid[7] = (Convert.ToInt32(grid[7] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[7] = score;
                }
            }
            if (grid[8] != 0 && Math.Abs(grid[8]) != 99)
            {
                score = Grade(grid[8], grid[7], grid[9], grid[10]) +
                        Grade(grid[8], grid[9], grid[10], grid[11]) +
                        Grade(grid[8], grid[1], grid[15], grid[22]) +
                        Grade(grid[8], grid[15], grid[22], grid[29]) +
                        Grade(grid[8], grid[0], grid[16], grid[24]) +
                        Grade(grid[8], grid[16], grid[24], grid[32]);
                if (score == 0)
                {
                    grid[8] = (Convert.ToInt32(grid[8] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[8] = score;
                }
            }
            if (grid[9] != 0 && Math.Abs(grid[9]) != 99)
            {
                score = Grade(grid[9], grid[7], grid[8], grid[10]) +
                        Grade(grid[9], grid[8], grid[10], grid[11]) +
                        Grade(grid[9], grid[10], grid[11], grid[12]) +
                        Grade(grid[9], grid[2], grid[16], grid[23]) +
                        Grade(grid[9], grid[16], grid[23], grid[30]) +
                        Grade(grid[9], grid[1], grid[17], grid[25]) +
                        Grade(grid[9], grid[17], grid[25], grid[33]) +
                        Grade(grid[9], grid[3], grid[15], grid[21]);
                if (score == 0)
                {
                    grid[9] = (Convert.ToInt32(grid[9] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[9] = score;
                }
            }
            if (grid[10] != 0 && Math.Abs(grid[10]) != 99)
            {
                score = Grade(grid[10], grid[7], grid[8], grid[9]) +
                        Grade(grid[10], grid[8], grid[9], grid[11]) +
                        Grade(grid[10], grid[9], grid[11], grid[12]) +
                        Grade(grid[10], grid[11], grid[12], grid[13]) +
                        Grade(grid[10], grid[3], grid[17], grid[24]) +
                        Grade(grid[10], grid[17], grid[24], grid[31]) +
                        Grade(grid[10], grid[2], grid[18], grid[26]) +
                        Grade(grid[10], grid[18], grid[26], grid[34]) +
                        Grade(grid[10], grid[4], grid[16], grid[22]) +
                        Grade(grid[10], grid[16], grid[22], grid[28]);
                if (score == 0)
                {
                    grid[10] = (Convert.ToInt32(grid[10] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[10] = score;
                }
            }
            if (grid[11] != 0 && Math.Abs(grid[11]) != 99)
            {
                score = Grade(grid[11], grid[8], grid[9], grid[10]) +
                        Grade(grid[11], grid[9], grid[10], grid[12]) +
                        Grade(grid[11], grid[10], grid[12], grid[13]) +
                        Grade(grid[11], grid[4], grid[18], grid[25]) +
                        Grade(grid[11], grid[18], grid[25], grid[32]) +
                        Grade(grid[11], grid[3], grid[19], grid[27]) +
                        Grade(grid[11], grid[5], grid[17], grid[23]) +
                        Grade(grid[11], grid[17], grid[23], grid[29]);
                if (score == 0)
                {
                    grid[11] = (Convert.ToInt32(grid[11] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[11] = score;
                }
            }
            if (grid[12] != 0 && Math.Abs(grid[12]) != 99)
            {
                score = Grade(grid[12], grid[9], grid[10], grid[11]) +
                        Grade(grid[12], grid[10], grid[11], grid[13]) +
                        Grade(grid[12], grid[5], grid[19], grid[26]) +
                        Grade(grid[12], grid[19], grid[26], grid[33]) +
                        Grade(grid[12], grid[6], grid[18], grid[24]) +
                        Grade(grid[12], grid[18], grid[24], grid[30]);
                if (score == 0)
                {
                    grid[12] = (Convert.ToInt32(grid[12] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[12] = score;
                }
            }
            if (grid[13] != 0 && Math.Abs(grid[13]) != 99)
            {
                score = Grade(grid[13], grid[10], grid[11], grid[12]) +
                        Grade(grid[13], grid[6], grid[20], grid[27]) +
                        Grade(grid[13], grid[20], grid[27], grid[34]) +
                        Grade(grid[13], grid[19], grid[25], grid[31]);
                if (score == 0)
                {
                    grid[13] = (Convert.ToInt32(grid[13] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[13] = score;
                }
            }
            if (grid[14] != 0 && Math.Abs(grid[14]) != 99)
            {
                score = Grade(grid[14], grid[15], grid[16], grid[17]) +
                        Grade(grid[14], grid[0], grid[7], grid[21]) +
                        Grade(grid[14], grid[7], grid[21], grid[28]) +
                        Grade(grid[14], grid[21], grid[28], grid[35]) +
                        Grade(grid[14], grid[22], grid[30], grid[38]);
                if (score == 0)
                {
                    grid[14] = (Convert.ToInt32(grid[14] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[14] = score;
                }
            }
            if (grid[15] != 0 && Math.Abs(grid[15]) != 99)
            {
                score = Grade(grid[15], grid[14], grid[16], grid[17]) +
                        Grade(grid[15], grid[16], grid[17], grid[18]) +
                        Grade(grid[15], grid[1], grid[8], grid[22]) +
                        Grade(grid[15], grid[8], grid[22], grid[29]) +
                        Grade(grid[15], grid[22], grid[29], grid[36]) +
                        Grade(grid[15], grid[7], grid[23], grid[31]) +
                        Grade(grid[15], grid[23], grid[31], grid[39]) +
                        Grade(grid[15], grid[3], grid[9], grid[21]);
                if (score == 0)
                {
                    grid[15] = (Convert.ToInt32(grid[15] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[15] = score;
                }
            }
            if (grid[16] != 0 && Math.Abs(grid[16]) != 99)
            {
                score = Grade(grid[16], grid[14], grid[15], grid[17]) +
                        Grade(grid[16], grid[15], grid[17], grid[18]) +
                        Grade(grid[16], grid[17], grid[18], grid[19]) +
                        Grade(grid[16], grid[2], grid[9], grid[23]) +
                        Grade(grid[16], grid[9], grid[23], grid[30]) +
                        Grade(grid[16], grid[23], grid[30], grid[37]) +
                        Grade(grid[16], grid[0], grid[8], grid[24]) +
                        Grade(grid[16], grid[8], grid[24], grid[32]) +
                        Grade(grid[16], grid[24], grid[32], grid[40]) +
                        Grade(grid[16], grid[4], grid[10], grid[22]) +
                        Grade(grid[16], grid[10], grid[22], grid[28]);
                if (score == 0)
                {
                    grid[16] = (Convert.ToInt32(grid[16] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[16] = score;
                }
            }
            if (grid[17] != 0 && Math.Abs(grid[17]) != 99)
            {
                score = Grade(grid[17], grid[14], grid[15], grid[16]) +
                        Grade(grid[17], grid[15], grid[16], grid[18]) +
                        Grade(grid[17], grid[16], grid[18], grid[19]) +
                        Grade(grid[17], grid[18], grid[19], grid[20]) +
                        Grade(grid[17], grid[3], grid[10], grid[24]) +
                        Grade(grid[17], grid[10], grid[24], grid[31]) +
                        Grade(grid[17], grid[24], grid[31], grid[38]) +
                        Grade(grid[17], grid[1], grid[9], grid[25]) +
                        Grade(grid[17], grid[9], grid[25], grid[33]) +
                        Grade(grid[17], grid[25], grid[33], grid[41]) +
                        Grade(grid[17], grid[5], grid[11], grid[23]) +
                        Grade(grid[17], grid[11], grid[23], grid[29]) +
                        Grade(grid[17], grid[23], grid[29], grid[35]);
                if (score == 0)
                {
                    grid[17] = (Convert.ToInt32(grid[17] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[17] = score;
                }
            }
            if (grid[18] != 0 && Math.Abs(grid[18]) != 99)
            {
                score = Grade(grid[18], grid[15], grid[16], grid[17]) +
                        Grade(grid[18], grid[16], grid[17], grid[19]) +
                        Grade(grid[18], grid[17], grid[19], grid[20]) +
                        Grade(grid[18], grid[4], grid[11], grid[25]) +
                        Grade(grid[18], grid[11], grid[25], grid[32]) +
                        Grade(grid[18], grid[25], grid[32], grid[39]) +
                        Grade(grid[18], grid[2], grid[10], grid[26]) +
                        Grade(grid[18], grid[10], grid[26], grid[34]) +
                        Grade(grid[18], grid[6], grid[12], grid[24]) +
                        Grade(grid[18], grid[12], grid[24], grid[30]) +
                        Grade(grid[18], grid[24], grid[30], grid[36]);
                if (score == 0)
                {
                    grid[18] = (Convert.ToInt32(grid[18] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[18] = score;
                }
            }
            if (grid[19] != 0 && Math.Abs(grid[19]) != 99)
            {
                score = Grade(grid[19], grid[16], grid[17], grid[18]) +
                        Grade(grid[19], grid[17], grid[18], grid[20]) +
                        Grade(grid[19], grid[5], grid[12], grid[26]) +
                        Grade(grid[19], grid[12], grid[26], grid[33]) +
                        Grade(grid[19], grid[26], grid[33], grid[40]) +
                        Grade(grid[19], grid[3], grid[11], grid[27]) +
                        Grade(grid[19], grid[13], grid[25], grid[31]) +
                        Grade(grid[19], grid[25], grid[31], grid[37]);
                if (score == 0)
                {
                    grid[19] = (Convert.ToInt32(grid[19] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[19] = score;
                }
            }
            if (grid[20] != 0 && Math.Abs(grid[20]) != 99)
            {
                score = Grade(grid[20], grid[17], grid[18], grid[19]) +
                        Grade(grid[20], grid[6], grid[13], grid[27]) +
                        Grade(grid[20], grid[13], grid[27], grid[34]) +
                        Grade(grid[20], grid[27], grid[34], grid[41]) +
                        Grade(grid[20], grid[26], grid[32], grid[38]);
                if (score == 0)
                {
                    grid[20] = (Convert.ToInt32(grid[20] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[20] = score;
                }
            }
            if (grid[21] != 0 && Math.Abs(grid[21]) != 99)
            {
                score = Grade(grid[21], grid[22], grid[23], grid[24]) +
                        Grade(grid[21], grid[0], grid[7], grid[14]) +
                        Grade(grid[21], grid[7], grid[14], grid[28]) +
                        Grade(grid[21], grid[14], grid[28], grid[35]) +
                        Grade(grid[21], grid[15], grid[9], grid[3]);
                if (score == 0)
                {
                    grid[21] = (Convert.ToInt32(grid[21] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[21] = score;
                }
            }
            if (grid[22] != 0 && Math.Abs(grid[22]) != 99)
            {
                score = Grade(grid[22], grid[21], grid[23], grid[24]) +
                        Grade(grid[22], grid[23], grid[24], grid[25]) +
                        Grade(grid[22], grid[1], grid[8], grid[15]) +
                        Grade(grid[22], grid[8], grid[15], grid[29]) +
                        Grade(grid[22], grid[15], grid[29], grid[36]) +
                        Grade(grid[22], grid[14], grid[30], grid[38]) +
                        Grade(grid[22], grid[4], grid[10], grid[16]) +
                        Grade(grid[22], grid[10], grid[16], grid[28]);
                if (score == 0)
                {
                    grid[22] = (Convert.ToInt32(grid[22] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[22] = score;
                }
            }
            if (grid[23] != 0 && Math.Abs(grid[23]) != 99)
            {
                score = Grade(grid[23], grid[21], grid[22], grid[24]) +
                        Grade(grid[23], grid[22], grid[24], grid[25]) +
                        Grade(grid[23], grid[24], grid[25], grid[26]) +
                        Grade(grid[23], grid[2], grid[9], grid[16]) +
                        Grade(grid[23], grid[9], grid[16], grid[30]) +
                        Grade(grid[23], grid[16], grid[30], grid[37]) +
                        Grade(grid[23], grid[7], grid[15], grid[31]) +
                        Grade(grid[23], grid[15], grid[31], grid[39]) +
                        Grade(grid[23], grid[5], grid[11], grid[17]) +
                        Grade(grid[23], grid[11], grid[17], grid[29]) +
                        Grade(grid[23], grid[17], grid[29], grid[35]);
                if (score == 0)
                {
                    grid[23] = (Convert.ToInt32(grid[23] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[23] = score;
                }
            }
            if (grid[24] != 0 && Math.Abs(grid[24]) != 99)
            {
                score = Grade(grid[24], grid[21], grid[22], grid[23]) +
                        Grade(grid[24], grid[22], grid[23], grid[25]) +
                        Grade(grid[24], grid[23], grid[25], grid[26]) +
                        Grade(grid[24], grid[25], grid[26], grid[27]) +
                        Grade(grid[24], grid[3], grid[10], grid[17]) +
                        Grade(grid[24], grid[10], grid[17], grid[31]) +
                        Grade(grid[24], grid[17], grid[31], grid[38]) +
                        Grade(grid[24], grid[0], grid[8], grid[16]) +
                        Grade(grid[24], grid[8], grid[16], grid[32]) +
                        Grade(grid[24], grid[16], grid[32], grid[40]) +
                        Grade(grid[24], grid[6], grid[12], grid[18]) +
                        Grade(grid[24], grid[12], grid[18], grid[30]) +
                        Grade(grid[24], grid[18], grid[30], grid[36]);
                if (score == 0)
                {
                    grid[24] = (Convert.ToInt32(grid[24] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[24] = score;
                }
            }
            if (grid[25] != 0 && Math.Abs(grid[25]) != 99)
            {
                score = Grade(grid[25], grid[22], grid[23], grid[24]) +
                        Grade(grid[25], grid[23], grid[24], grid[26]) +
                        Grade(grid[25], grid[24], grid[26], grid[27]) +
                        Grade(grid[25], grid[4], grid[11], grid[18]) +
                        Grade(grid[25], grid[11], grid[18], grid[32]) +
                        Grade(grid[25], grid[18], grid[32], grid[39]) +
                        Grade(grid[25], grid[1], grid[9], grid[17]) +
                        Grade(grid[25], grid[9], grid[17], grid[33]) +
                        Grade(grid[25], grid[17], grid[33], grid[41]) +
                        Grade(grid[25], grid[13], grid[19], grid[31]) +
                        Grade(grid[25], grid[19], grid[31], grid[37]);
                if (score == 0)
                {
                    grid[25] = (Convert.ToInt32(grid[25] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[25] = score;
                }
            }
            if (grid[26] != 0 && Math.Abs(grid[26]) != 99)
            {
                score = Grade(grid[26], grid[23], grid[24], grid[25]) +
                        Grade(grid[26], grid[24], grid[25], grid[27]) +
                        Grade(grid[26], grid[5], grid[12], grid[19]) +
                        Grade(grid[26], grid[12], grid[19], grid[33]) +
                        Grade(grid[26], grid[19], grid[33], grid[40]) +
                        Grade(grid[26], grid[2], grid[10], grid[18]) +
                        Grade(grid[26], grid[10], grid[18], grid[34]) +
                        Grade(grid[26], grid[20], grid[32], grid[38]);
                if (score == 0)
                {
                    grid[26] = (Convert.ToInt32(grid[26] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[26] = score;
                }
            }
            if (grid[27] != 0 && Math.Abs(grid[27]) != 99)
            {
                score = Grade(grid[27], grid[24], grid[25], grid[26]) +
                        Grade(grid[27], grid[6], grid[13], grid[20]) +
                        Grade(grid[27], grid[13], grid[20], grid[34]) +
                        Grade(grid[27], grid[20], grid[34], grid[41]) +
                        Grade(grid[27], grid[3], grid[11], grid[19]);
                if (score == 0)
                {
                    grid[27] = (Convert.ToInt32(grid[27] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[27] = score;
                }
            }
            if (grid[28] != 0 && Math.Abs(grid[28]) != 99)
            {
                score = Grade(grid[28], grid[29], grid[30], grid[31]) +
                        Grade(grid[28], grid[7], grid[14], grid[21]) +
                        Grade(grid[28], grid[14], grid[21], grid[35]) +
                        Grade(grid[28], grid[10], grid[16], grid[22]);
                if (score == 0)
                {
                    grid[28] = (Convert.ToInt32(grid[28] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[28] = score;
                }
            }
            if (grid[29] != 0 && Math.Abs(grid[29]) != 99)
            {
                score = Grade(grid[29], grid[28], grid[30], grid[31]) +
                        Grade(grid[29], grid[30], grid[31], grid[32]) +
                        Grade(grid[29], grid[8], grid[15], grid[22]) +
                        Grade(grid[29], grid[15], grid[22], grid[36]) +
                        Grade(grid[29], grid[11], grid[17], grid[23]) +
                        Grade(grid[29], grid[17], grid[23], grid[35]);
                if (score == 0)
                {
                    grid[29] = (Convert.ToInt32(grid[29] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[29] = score;
                }
            }
            if (grid[30] != 0 && Math.Abs(grid[30]) != 99)
            {
                score = Grade(grid[30], grid[28], grid[29], grid[31]) +
                        Grade(grid[30], grid[29], grid[31], grid[32]) +
                        Grade(grid[30], grid[31], grid[32], grid[33]) +
                        Grade(grid[30], grid[9], grid[16], grid[23]) +
                        Grade(grid[30], grid[16], grid[23], grid[37]) +
                        Grade(grid[30], grid[12], grid[18], grid[24]) +
                        Grade(grid[30], grid[18], grid[24], grid[36]) +
                        Grade(grid[30], grid[14], grid[22], grid[38]);
                if (score == 0)
                {
                    grid[30] = (Convert.ToInt32(grid[30] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[30] = score;
                }
            }
            if (grid[31] != 0 && Math.Abs(grid[31]) != 99)
            {
                score = Grade(grid[31], grid[28], grid[29], grid[30]) +
                        Grade(grid[31], grid[29], grid[30], grid[32]) +
                        Grade(grid[31], grid[30], grid[32], grid[33]) +
                        Grade(grid[31], grid[32], grid[33], grid[34]) +
                        Grade(grid[31], grid[10], grid[17], grid[24]) +
                        Grade(grid[31], grid[17], grid[24], grid[38]) +
                        Grade(grid[31], grid[13], grid[19], grid[25]) +
                        Grade(grid[31], grid[19], grid[25], grid[37]) +
                        Grade(grid[31], grid[7], grid[15], grid[23]) +
                        Grade(grid[31], grid[15], grid[23], grid[39]);
                if (score == 0)
                {
                    grid[31] = (Convert.ToInt32(grid[31] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[31] = score;
                }
            }
            if (grid[32] != 0 && Math.Abs(grid[32]) != 99)
            {
                score = Grade(grid[32], grid[29], grid[30], grid[31]) +
                        Grade(grid[32], grid[30], grid[31], grid[33]) +
                        Grade(grid[32], grid[31], grid[33], grid[34]) +
                        Grade(grid[32], grid[11], grid[18], grid[25]) +
                        Grade(grid[32], grid[18], grid[25], grid[39]) +
                        Grade(grid[32], grid[20], grid[26], grid[38]) +
                        Grade(grid[32], grid[8], grid[16], grid[24]) +
                        Grade(grid[32], grid[16], grid[24], grid[40]);
                if (score == 0)
                {
                    grid[32] = (Convert.ToInt32(grid[32] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[32] = score;
                }
            }
            if (grid[33] != 0 && Math.Abs(grid[33]) != 99)
            {
                score = Grade(grid[33], grid[30], grid[31], grid[32]) +
                        Grade(grid[33], grid[31], grid[32], grid[34]) +
                        Grade(grid[33], grid[12], grid[19], grid[26]) +
                        Grade(grid[33], grid[19], grid[26], grid[40]) +
                        Grade(grid[33], grid[9], grid[17], grid[25]) +
                        Grade(grid[33], grid[17], grid[25], grid[41]);
                if (score == 0)
                {
                    grid[33] = (Convert.ToInt32(grid[33] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[33] = score;
                }
            }
            if (grid[34] != 0 && Math.Abs(grid[34]) != 99)
            {
                score = Grade(grid[34], grid[31], grid[32], grid[33]) +
                        Grade(grid[34], grid[13], grid[20], grid[27]) +
                        Grade(grid[34], grid[20], grid[27], grid[41]) +
                        Grade(grid[34], grid[10], grid[18], grid[26]);
                if (score == 0)
                {
                    grid[34] = (Convert.ToInt32(grid[34] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[34] = score;
                }
            }
            if (grid[35] != 0 && Math.Abs(grid[35]) != 99)
            {
                score = Grade(grid[35], grid[36], grid[37], grid[38]) +
                        Grade(grid[35], grid[14], grid[21], grid[28]) +
                        Grade(grid[35], grid[17], grid[23], grid[29]);
                if (score == 0)
                {
                    grid[35] = (Convert.ToInt32(grid[35] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[35] = score;
                }
            }
            if (grid[36] != 0 && Math.Abs(grid[36]) != 99)
            {
                score = Grade(grid[36], grid[35], grid[37], grid[38]) +
                        Grade(grid[36], grid[37], grid[38], grid[39]) +
                        Grade(grid[36], grid[15], grid[22], grid[29]) +
                        Grade(grid[36], grid[18], grid[24], grid[30]);
                if (score == 0)
                {
                    grid[36] = (Convert.ToInt32(grid[36] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[36] = score;
                }
            }
            if (grid[37] != 0 && Math.Abs(grid[37]) != 99)
            {
                score = Grade(grid[37], grid[35], grid[36], grid[38]) +
                        Grade(grid[37], grid[36], grid[38], grid[39]) +
                        Grade(grid[37], grid[38], grid[39], grid[40]) +
                        Grade(grid[37], grid[16], grid[23], grid[30]) +
                        Grade(grid[37], grid[19], grid[25], grid[31]);
                if (score == 0)
                {
                    grid[37] = (Convert.ToInt32(grid[37] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[37] = score;
                }
            }
            if (grid[38] != 0 && Math.Abs(grid[38]) != 99)
            {
                score = Grade(grid[38], grid[35], grid[36], grid[37]) +
                        Grade(grid[38], grid[36], grid[37], grid[39]) +
                        Grade(grid[38], grid[37], grid[39], grid[40]) +
                        Grade(grid[38], grid[39], grid[40], grid[41]) +
                        Grade(grid[38], grid[17], grid[24], grid[31]) +
                        Grade(grid[38], grid[20], grid[26], grid[32]) +
                        Grade(grid[38], grid[14], grid[22], grid[30]);
                if (score == 0)
                {
                    grid[38] = (Convert.ToInt32(grid[38] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[38] = score;
                }
            }
            if (grid[39] != 0 && Math.Abs(grid[39]) != 99)
            {
                score = Grade(grid[39], grid[36], grid[37], grid[38]) +
                        Grade(grid[39], grid[37], grid[38], grid[40]) +
                        Grade(grid[39], grid[38], grid[40], grid[41]) +
                        Grade(grid[39], grid[18], grid[25], grid[32]) +
                        Grade(grid[39], grid[15], grid[23], grid[31]);
                if (score == 0)
                {
                    grid[39] = (Convert.ToInt32(grid[39] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[39] = score;
                }
            }
            if (grid[40] != 0 && Math.Abs(grid[40]) != 99)
            {
                score = Grade(grid[40], grid[37], grid[38], grid[39]) +
                        Grade(grid[40], grid[38], grid[39], grid[41]) +
                        Grade(grid[40], grid[19], grid[26], grid[33]) +
                        Grade(grid[40], grid[16], grid[24], grid[32]);
                if (score == 0)
                {
                    grid[40] = (Convert.ToInt32(grid[40] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[40] = score;
                }
            }
            if (grid[41] != 0 && Math.Abs(grid[41]) != 99)
            {
                score = Grade(grid[41], grid[38], grid[39], grid[40]) +
                        Grade(grid[41], grid[20], grid[27], grid[34]) +
                        Grade(grid[41], grid[17], grid[25], grid[33]);
                if (score == 0)
                {
                    grid[41] = (Convert.ToInt32(grid[41] > 0) * 2 - 1) * 99;
                }
                else
                {
                    grid[41] = score;
                }
            }

            score = 0;

            foreach (int edge in grid)
            {
                if (Math.Abs(edge) != 99)
                {
                    score += edge;
                }
            }

            return score;
        }

        private int Grade(int index, int a, int b, int c)
        {
            bool aSet = a != 0;
            bool bSet = b != 0;
            bool cSet = c != 0;
            bool indexPos = index > 0;
            bool aPos = a > 0;
            bool bPos = b > 0;
            bool cPos = c > 0;
            int indexValue = Convert.ToInt32(indexPos) * 2 - 1;

            if (aSet && bSet && cSet)
            {
                if ((indexPos && aPos && bPos && cPos) || (!indexPos && !aPos && !bPos && !cPos))
                {
                    return indexValue * 1000;
                }
                else
                {
                    return 0;
                }
            }
            else if (!aSet && !bSet && !cSet)
            {
                return indexValue;
            }
            else if (aSet && bSet && !cSet)
            {
                if ((indexPos && aPos && bPos) || (!indexPos && !aPos && !bPos))
                {
                    return indexValue * 16;
                }
                else
                {
                    return 0;
                }
            }
            else if (aSet && !bSet && cSet)
            {
                if ((indexPos && aPos && cPos) || (!indexPos && !aPos && !cPos))
                {
                    return indexValue * 16;
                }
                else
                {
                    return 0;
                }
            }
            else if (aSet && !bSet && !cSet)
            {
                if ((indexPos && aPos) || (!indexPos && !aPos))
                {
                    return indexValue * 4;
                }
                else
                {
                    return 0;
                }
            }
            else if (!aSet && bSet && cSet)
            {
                if ((indexPos && bPos && cPos) || (!indexPos && !bPos && !cPos))
                {
                    return indexValue * 16;
                }
                else
                {
                    return 0;
                }
            }
            else if (!aSet && bSet && !cSet)
            {
                if ((indexPos && bPos) || (!indexPos && !bPos))
                {
                    return indexValue * 4;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if ((indexPos && cPos) || (!indexPos && !cPos))
                {
                    return indexValue * 4;
                }
                else
                {
                    return 0;
                }
            }
        }

        private int ScoreMoves(int[] grid, int column)
        {
            int row = LowestEmptyRow(grid, column);

            if (row != -1)
            {
                int[] copy = (int[])grid.Clone();
                copy[column + (5 - row) * 7] = Convert.ToInt32(_player) * 2 - 1;
                _player = !_player;
                int nextMove = ScoreNextMove(copy, SelectNextComputerMove(copy));
                _player = !_player;
                return Evaluate(copy) + nextMove;
            }
            else return (Convert.ToInt32(_player) * 2 - 1) * -99999;
        }

        private int ScoreNextMove(int[] grid, int column)
        {
            int row = LowestEmptyRow(grid, column);

            if (row != -1)
            {
                int[] copy = (int[])grid.Clone();
                copy[column + (5 - row) * 7] = Convert.ToInt32(_player) * 2 - 1;
                return Evaluate(copy);
            }
            else return (Convert.ToInt32(_player) * 2 - 1) * -99999;
        }

        private int HardPlay(int[] grid)
        {
            int[] col = new int[7];

            for (int i = 0; i < 7; i++)
            {
                col[i] = ScoreMoves(grid, i);
            }
            int[] moves = Enumerable.Range(0, 7).ToArray();
            Array.Sort(col, moves);
            if (_player)
            {
                if (col[5] + 3 > col[6])
                {
                    if (rand.Next(10) < 6)
                    {
                        return moves[6];
                    }
                    else
                    {
                        return moves[5];
                    }
                }
                else
                {
                    return moves[6];
                }
            }
            else
            {
                if (col[1] - 3 < col[0])
                {
                    if (rand.Next(10) < 6)
                    {
                        return moves[0];
                    }
                    else
                    {
                        return moves[1];
                    }
                }
                else
                {
                    return moves[0];
                }
            }
        }

        private int SelectNextComputerMove(int[] grid)
        {
            int[] col = new int[7];
            int index = 0;

            for (int i = 0; i < 7; i++)
            {
                col[i] = ScoreNextMove(grid, i);
            }
            if (_player)
            {
                for (int i = 1; i < 7; i++)
                {
                    if (col[i] > col[index])
                    {
                        index = i;
                    }
                }
            }
            else
            {
                for (int i = 1; i < 7; i++)
                {
                    if (col[i] < col[index])
                    {
                        index = i;
                    }
                }
            }

            return index;
        }

        private int MediumPlay(int[] grid)
        {
            int move = SelectNextComputerMove(grid);

            if (Math.Abs(ScoreNextMove(grid, move)) > 1000)
            {
                return move;
            }
            else
            {
                int newMove;
                List<int> availableMoves = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
                availableMoves.Remove(move);
                do
                {
                    do
                    {
                        switch (rand.Next(16))
                        {
                            case 0:
                                newMove = 0;
                                break;
                            case 1:
                            case 2:
                                newMove = 1;
                                break;
                            case 3:
                            case 4:
                            case 5:
                                newMove = 2;
                                break;
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                                newMove = 3;
                                break;
                            case 10:
                            case 11:
                            case 12:
                                newMove = 4;
                                break;
                            case 13:
                            case 14:
                                newMove = 5;
                                break;
                            default:
                                newMove = 6;
                                break;
                        }
                    } while (!availableMoves.Remove(newMove));

                    int row = LowestEmptyRow(grid, newMove);

                    if (row != -1)
                    {
                        int[] copy = (int[])grid.Clone();
                        copy[newMove + (5 - row) * 7] = Convert.ToInt32(_player) * 2 - 1;
                        _player = !_player;
                        int opponentScore = ScoreNextMove(copy, SelectNextComputerMove(copy));
                        _player = !_player;
                        if (Math.Abs(opponentScore) < 1000)
                        {
                            return newMove;
                        }
                    }

                } while (availableMoves.Any());
                return move;
            }
        }

        private int EasyPlay(int[] grid)
        {
            int move = SelectNextComputerMove(grid);

            int newMove;
            List<int> availableMoves = new List<int>() { 0, 1, 2, 3, 4, 5, 6 };

            do
            {
                do
                {
                    newMove = rand.Next(7);                    
                } while (!availableMoves.Remove(newMove));

                int row = LowestEmptyRow(grid, newMove);

                if (row != -1)
                {
                    int[] copy = (int[])grid.Clone();
                    copy[newMove + (5 - row) * 7] = Convert.ToInt32(_player) * 2 - 1;
                    _player = !_player;
                    int opponentScore = ScoreNextMove(copy, SelectNextComputerMove(copy));
                    _player = !_player;
                    if (Math.Abs(opponentScore) < 1000)
                    {
                        return newMove;
                    }
                }

            } while (availableMoves.Any());
            return move;
        }

        public static void Splash()
        {
            int X = Console.WindowWidth / 2;
            int Y = Console.WindowHeight / 2;
            string name = "Connect Four";

            Console.Clear();
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetCursorPosition(X - 6, 2);
            Console.Write(name);
            Quadrilateral box = new Quadrilateral(X - 35, Y + 1, X + 33, Y + 4);
            box.DrawBox(true);
            Console.SetCursorPosition(X - 33, Y + 2);
            Console.Write("Connect Four™ is a registered trademark owned by Milton Bradley");
            Console.SetCursorPosition(X - 33, Y + 3);
            Console.Write("any profits generated from this free distribution belongs to them");


            System.Threading.Thread.Sleep(200);
            for (int i = 0; i < name.Length; i++)
            {
                Console.ForegroundColor = (ConsoleColor)rand.Next(1, 16);
                for (int y = 2; y < Y - 3; y++)
                {
                    Console.SetCursorPosition(X - 6 + i, y);
                    Console.Write(" ");
                    Console.SetCursorPosition(X - 6 + i, y + 1);
                    Console.Write(name[i]);
                    System.Threading.Thread.Sleep(20);
                }
                if (i == 6)
                {
                    i++;
                }
                Console.SetCursorPosition(X - 33 + i, Y + 2);
                Console.Write(name[i]);
            }
            System.Threading.Thread.Sleep(1200);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
