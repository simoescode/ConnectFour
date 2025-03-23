using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    public class Program
    {     
        public static void Main(string[] _args)
        {
            ConfirmWindowsSize();
            Console.Title = "Connect Four";
            int Y = Console.WindowHeight / 2;
            int X = Console.WindowWidth / 2;
            bool keepPlaying = true;
            bool quitCurrentGame = false;
            int players;
            string[] playerNames;
            int[] computerStrength;
            List<Game> gameStack = new List<Game>();
            string playersPrompt1 = "How Many Players (0-2):";
            string playersPrompt2 = "0 - Computer vs Computer";
            string playersPrompt3 = "1 - Player vs Computer";
            string playersPrompt4 = "2 - Player vs Player";
            string namePrompt1 = "Player's Name:";
            string namePrompt2 = "Second " + namePrompt1;
            string levelPrompt1 = "Computer's Playing Level:";
            string levelPrompt1a = "First " + levelPrompt1;
            string levelPrompt1b = "Second " + levelPrompt1;
            string levelPrompt2 = "(E)asy or (M)edium or (H)ard";
            string orderPrompt = "Do you want to play first?";
            string yesornoPrompt = "(Y)es or (N)o";
            string resetPrompt = "Are you sure you want to reset the game?";
            string quitPrompt = "Are you sure you want to quit this game?";
            string startOverPrompt = "Do you want to start another game?";
            String[] levels = { "E", "M", "H" };
            Random r = new Random();
            
            Game.Splash();

            do
            {
                ConfirmWindowsSize();
                Console.Clear();
                Console.SetCursorPosition(X - playersPrompt1.Length / 2, Y - 4);
                Console.Write(playersPrompt1);
                Console.SetCursorPosition(X - playersPrompt2.Length / 2, Y - 2);
                Console.Write(playersPrompt2);
                Console.SetCursorPosition(X - playersPrompt3.Length / 2, Y);
                Console.Write(playersPrompt3);
                Console.SetCursorPosition(X - playersPrompt4.Length / 2, Y + 2);
                Console.Write(playersPrompt4);
                Console.SetCursorPosition(X, Y + 4);
                players = NumbOfPlayers();
                ScrollAway(r, playersPrompt1, playersPrompt2, playersPrompt3, playersPrompt4, players.ToString());

                if (players != 0)
                {
                    playerNames = new string[players];
                    for (int i = 0; i < players; i++)
                    {
                        if (i == 0)
                        {
                            Console.SetCursorPosition(X - namePrompt1.Length / 2, Y - 1);
                            Console.Write(namePrompt1);
                        }
                        else
                        {
                            Console.SetCursorPosition(X - namePrompt2.Length / 2, Y - 1);
                            Console.Write(namePrompt2);
                        }
                        Console.SetCursorPosition(X - 4, Y + 1);
                        playerNames[i] = Console.ReadLine().Trim();
                        while (playerNames[i].Length == 0)
                        {
                            Console.SetCursorPosition(X - 4, Y + 1);
                            playerNames[i] = Console.ReadLine().Trim();
                        }
                        Console.SetCursorPosition(X - 4, Y + 1);
                        for (int pos = 0; pos < playerNames[i].Length; pos++)
                        {
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(X - playerNames[i].Length / 2, Y + 1);
                        if (i == 0)
                        {
                            ScrollAway(r, namePrompt1, playerNames[i]);
                        }
                        else
                        {
                            ScrollAway(r, namePrompt2, playerNames[i]);
                        }
                    }
                    if (players == 2)
                    {
                        gameStack.Add(new Game(playerNames));
                    }
                    else
                    {
                        computerStrength = new int[1];
                        Console.SetCursorPosition(X - levelPrompt1.Length / 2, Y - 2);
                        Console.Write(levelPrompt1);
                        Console.SetCursorPosition(X - levelPrompt2.Length / 2, Y);
                        Console.Write(levelPrompt2);
                        Console.SetCursorPosition(X, Y + 2);
                        computerStrength[0] = ComputerStrength();
                        ScrollAway(r, levelPrompt1, levelPrompt2, computerStrength[0].ToString());

                        Console.SetCursorPosition(X - orderPrompt.Length / 2, Y - 2);
                        Console.Write(orderPrompt);
                        Console.SetCursorPosition(X - yesornoPrompt.Length / 2, Y);
                        Console.Write(yesornoPrompt);
                        Console.SetCursorPosition(X, Y + 2);
                        if (YesOrNo())
                        {
                            ScrollAway(r, orderPrompt, yesornoPrompt, "Y");
                            gameStack.Add(new Game(playerNames, computerStrength[0] + 1));
                        }
                        else
                        {
                            ScrollAway(r, orderPrompt, yesornoPrompt, "N");
                            gameStack.Add(new Game(playerNames, (computerStrength[0] + 1) * 4));
                        }
                    }
                }
                else
                {
                    computerStrength = new int[2 - players];
                    for (int i = 0; i < 2 - players; i++)
                    {
                        if (i == 0)
                        {
                            Console.SetCursorPosition(X - levelPrompt1a.Length / 2, Y - 2);
                            Console.Write(levelPrompt1a);
                        }
                        else
                        {
                            Console.SetCursorPosition(X - levelPrompt1b.Length / 2, Y - 2);
                            Console.Write(levelPrompt1b);
                        }
                        Console.SetCursorPosition(X - levelPrompt2.Length / 2, Y);
                        Console.Write(levelPrompt2);
                        Console.SetCursorPosition(X, Y + 2);
                        computerStrength[i] = ComputerStrength();
                        if (i == 0)
                        {
                            ScrollAway(r, levelPrompt1a, levelPrompt2, levels[computerStrength[i]]);
                        }
                        else
                        {
                            ScrollAway(r, levelPrompt1b, levelPrompt2, levels[computerStrength[i]]);
                        }
                    }
                    gameStack.Add(new Game((computerStrength[0] + 1) * 4 + computerStrength[1] + 1));
                }

                ConsoleKeyInfo character;
                Console.CursorVisible = false;

                do
                {
                    //loops while computer plays 
                    while (gameStack[gameStack.Count - 1].Playing && ((gameStack[gameStack.Count - 1].Player && gameStack[gameStack.Count - 1].Mode > 3) ||
                          (!gameStack[gameStack.Count - 1].Player && gameStack[gameStack.Count - 1].Mode % 4 != 0))) ;
                    //clears the Console buffer effectively ignoring the keyboard while computer plays
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    character = Console.ReadKey(true);
                    if (character.Key == ConsoleKey.RightArrow)
                    {
                        gameStack[gameStack.Count - 1].MoveRight();
                    }
                    else if (character.Key == ConsoleKey.LeftArrow)
                    {
                        gameStack[gameStack.Count - 1].MoveLeft();
                    }
                    else if (character.Key == ConsoleKey.Enter || character.Key == ConsoleKey.DownArrow)
                    {
                        gameStack[gameStack.Count - 1].DropToken();
                    }
                    else if (character.Key == ConsoleKey.R)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        if (gameStack[gameStack.Count - 1].Playing)
                        {
                            ConfirmWindowsSize();
                            Console.Clear();
                            Console.SetCursorPosition(X - resetPrompt.Length / 2, Y - 2);
                            Console.Write(resetPrompt);
                            Console.SetCursorPosition(X - yesornoPrompt.Length / 2, Y);
                            Console.Write(yesornoPrompt);
                            Console.SetCursorPosition(X, Y + 2);
                            if (YesOrNo())
                            {
                                ScrollAway(r, resetPrompt, yesornoPrompt, "Y");
                                gameStack[gameStack.Count - 1].ResumeGame(true);
                            }
                            else
                            {
                                ScrollAway(r, resetPrompt, yesornoPrompt, "N");
                                gameStack[gameStack.Count - 1].ResumeGame(false);
                            }
                        }
                        else
                        {
                            gameStack[gameStack.Count - 1].ResumeGame(true);
                        }
                    }
                    else if (character.Key == ConsoleKey.Q)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        ConfirmWindowsSize();
                        Console.Clear();
                        Console.SetCursorPosition(X - quitPrompt.Length / 2, Y - 2);
                        Console.Write(quitPrompt);
                        Console.SetCursorPosition(X - yesornoPrompt.Length / 2, Y);
                        Console.Write(yesornoPrompt);
                        Console.SetCursorPosition(X, Y + 2);
                        quitCurrentGame = YesOrNo();
                        if (!quitCurrentGame)
                        {
                            ScrollAway(r, quitPrompt, yesornoPrompt, "N");
                            gameStack[gameStack.Count - 1].ResumeGame(false);
                        }
                        else
                        {
                            ScrollAway(r, quitPrompt, yesornoPrompt, "Y");
                        }
                    }
                    gameStack[gameStack.Count - 1].ConfirmWindowsSize();
                } while (!quitCurrentGame);

                Console.ForegroundColor = ConsoleColor.Gray;
                ConfirmWindowsSize();
                Console.Clear();
                Console.SetCursorPosition(X - startOverPrompt.Length / 2, Y - 2);
                Console.Write(startOverPrompt);
                Console.SetCursorPosition(X - yesornoPrompt.Length / 2, Y);
                Console.Write(yesornoPrompt);
                Console.SetCursorPosition(X, Y + 2);
                keepPlaying = YesOrNo();
                if (keepPlaying)
                {
                    ScrollAway(r, startOverPrompt, yesornoPrompt, "Y");
                    quitCurrentGame = false;
                }
            } while (keepPlaying);
        }

        public static int NumbOfPlayers()
        {
            ConsoleKeyInfo character;

            ConfirmWindowsSize();
            do
            {
                character = Console.ReadKey(true);
            } while (character.Key != ConsoleKey.D0 && character.Key != ConsoleKey.D1 && character.Key != ConsoleKey.D2 && character.Key != ConsoleKey.NumPad0 && character.Key != ConsoleKey.NumPad1 && character.Key != ConsoleKey.NumPad2);

            Console.Write(character.KeyChar);
            if (character.Key == ConsoleKey.D0 || character.Key == ConsoleKey.NumPad0)
            {
                return 0;
            }
            else if (character.Key == ConsoleKey.D1 || character.Key == ConsoleKey.NumPad1)
            {
                return 1;
            }
            else
                return 2;
        }

        public static bool YesOrNo()
        {
            ConsoleKeyInfo character;

            ConfirmWindowsSize();
            do
            {
                character = Console.ReadKey(true);

            } while (character.Key != ConsoleKey.Y && character.Key != ConsoleKey.N);

            return character.Key == ConsoleKey.Y;
        }

        public static int ComputerStrength()
        {
            ConsoleKeyInfo character;

            do
            {
                character = Console.ReadKey(true);
            } while (character.Key != ConsoleKey.E && character.Key != ConsoleKey.M && character.Key != ConsoleKey.H);

            Console.Write(character.KeyChar);
            if (character.Key == ConsoleKey.E)
            {
                return 0;
            }
            else if (character.Key == ConsoleKey.M)
            {
                return 1;
            }
            else
                return 2;
        }

        public static void ScrollDown(params string[] prompts)
        {
            int Y = Console.WindowHeight / 2;
            int X = Console.WindowWidth / 2;
            int lines = prompts.Length;

            for (int y = 0; y < Y + lines; y++)
            {
                for (int i = 0; i < lines; i++)
                {
                    if (Y - lines + 1 + i * 2 + y < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(X - prompts[i].Length / 2, Y - lines + 1 + i * 2 + y);
                        for (int d = 0; d < prompts[i].Length; d++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                for (int i = 0; i < lines; i++)
                {
                    if (Y - lines + 1 + i * 2 + y + 1 < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(X - prompts[i].Length / 2, Y - lines + 1 + i * 2 + y + 1);
                        Console.Write(prompts[i]);
                    }
                }
                System.Threading.Thread.Sleep(15);
            }
            Console.Clear();
        }

        public static void ScrollUp(params string[] prompts)
        {
            int Y = Console.WindowHeight / 2;
            int X = Console.WindowWidth / 2;
            int lines = prompts.Length;

            for (int y = 0; y < Y + lines; y++)
            {
                for (int i = 0; i < lines; i++)
                {
                    if (Y - lines + 1 + i * 2 - y >= 0)
                    {
                        Console.SetCursorPosition(X - prompts[i].Length / 2, Y - lines + 1 + i * 2 - y);
                        for (int d = 0; d < prompts[i].Length; d++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                for (int i = 0; i < lines; i++)
                {
                    if (Y - lines + 1 + i * 2 - y - 1 >= 0)
                    {
                        Console.SetCursorPosition(X - prompts[i].Length / 2, Y - lines + 1 + i * 2 - y - 1);
                        Console.Write(prompts[i]);
                    }
                }
                System.Threading.Thread.Sleep(15);
            }
            Console.Clear();
        }

        public static void ScrollRight(params string[] prompts)
        {
            int Y = Console.WindowHeight / 2;
            int X = Console.WindowWidth / 2;
            int lines = prompts.Length;
            int longest = prompts.OrderByDescending(str => str.Length).First().Length;
            int cutOff;

            for (int x = X; x < Console.WindowWidth + longest / 2 - 1; x++)
            {
                for (int i = 0; i < lines; i++)
                {
                    if (x - prompts[i].Length / 2 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(x - prompts[i].Length / 2, Y - lines + 1 + i * 2);
                        for (int d = 0; d < prompts[i].Length && x - prompts[i].Length / 2 + d < Console.WindowWidth; d++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                for (int i = 0; i < lines; i++)
                {
                    if (x - prompts[i].Length / 2 + 1 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(x - prompts[i].Length / 2 + 1, Y - lines + 1 + i * 2);
                        if (x + (prompts[i].Length / 2) + 1 < Console.WindowWidth - 1)
                        {
                            cutOff = prompts[i].Length;
                        }
                        else
                        {
                            cutOff = prompts[i].Length - ((x + (prompts[i].Length / 2) + 1) - (Console.WindowWidth - 1));
                        }
                        Console.Write(prompts[i].Substring(0, cutOff));
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
            Console.Clear();
        }

        public static void ScrollLeft(params string[] prompts)
        {
            int Y = Console.WindowHeight / 2;
            int X = Console.WindowWidth / 2;
            int lines = prompts.Length;
            int longest = prompts.OrderByDescending(str => str.Length).First().Length;

            for (int x = X; x > 0 - longest / 2; x--)
            {
                for (int i = 0; i < lines; i++)
                {
                    if (x + prompts[i].Length / 2 >= 0)
                    {
                        if (x - prompts[i].Length / 2 > 0)
                        {
                            Console.SetCursorPosition(x - prompts[i].Length / 2, Y - lines + 1 + i * 2);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, Y - lines + 1 + i * 2);
                        }
                        for (int d = 0; d < prompts[i].Length && d < prompts[i].Length / 2 + x + 2; d++)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                for (int i = 0; i < lines; i++)
                {
                    if (x + prompts[i].Length / 2 > 0)
                    {
                        if (x - prompts[i].Length / 2 - 1 >= 0)
                        {
                            Console.SetCursorPosition(x - prompts[i].Length / 2 - 1, Y - lines + 1 + i * 2);
                            Console.Write(prompts[i]);
                        }
                        else
                        {
                            Console.SetCursorPosition(0, Y - lines + 1 + i * 2);
                            Console.Write(prompts[i].Substring(-(x - prompts[i].Length / 2)));
                        }
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
            Console.Clear();
        }

        public static void ScrollAway(Random rand, params string[] prompts)
        {
            ConfirmWindowsSize();
            switch (rand.Next(4))
            {
                case 0:
                    ScrollDown(prompts);
                    break;
                case 1:
                    ScrollLeft(prompts);
                    break;
                case 2:
                    ScrollRight(prompts);
                    break;
                case 3:
                    ScrollUp(prompts);
                    break;
            }
        }
        public static void ConfirmWindowsSize()
        {
            if (Console.WindowWidth != Game.SCREENWIDTH || Console.WindowHeight != Game.SCREENHEIGHT)
            {
                Console.BufferWidth = Console.WindowWidth = Game.SCREENWIDTH;
                Console.BufferHeight = Console.WindowHeight = Game.SCREENHEIGHT;
            }
        }
    }
}
