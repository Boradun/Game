using DbRepository;
using PlayerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Game
{
    class GameController
    {
        Player _player1;
        Player _player2;
        int MinNumber;
        int MaxNumber;
        int SecretNumber;

        internal GameController()
        {
            _player1 = new Player() { Name = "Гость" };
            _player2 = new Player() { Name = "Гость" };
        }

        //главное меню игры
        public void MainMenu()
        {
            string input;
            do
            {
                Console.Clear();
                Console.WriteLine($"1.Начать новую игру {_player1.Name}  против {_player2.Name}");
                Console.WriteLine($"2.Загрузить первого игрока \n3.Загрузить второго игрока");
                Console.WriteLine($"4.Создать нового игрока");
                Console.WriteLine($"5.Показать статистику первого игрока\n6.показать статистику второго игрока");
                Console.WriteLine($"7.Выход");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        {
                            StartNewGame();
                            break;
                        }
                    case "2":
                        {
                            _player1 = LoadPlayer();
                            break;
                        }
                    case "3":
                        {
                            _player2 = LoadPlayer();
                            break;
                        }
                    case "4":
                        {
                            CreateNewPlayer();
                            break;
                        }
                    case "5":
                        {
                            WritePlayer(_player1);
                            break;
                        }
                    case "6":
                        {
                            WritePlayer(_player2);
                            break;
                        }
                    case "7":
                        {
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Такого пункта нет, выберите корректный пункт меню:");
                            break;
                        }
                }
            } while (input != "7");

        }

        //начинает новую игру из 2 раундов
        void StartNewGame()
        {
            Console.Clear();
            Console.WriteLine($"Раунд 1:");
            Round(_player1, _player2);
            Console.Clear();
            Console.WriteLine($"Раунд 2:");
            Round(_player2, _player1);
        }

        //проводит раунд в игре
        void Round(Player playerComeUP, Player playerThatSolve)
        {
            InputNumbers(playerComeUP);
            Console.Clear();
            bool result = GuessNumber(playerThatSolve);
            if (result&&playerThatSolve.Name!="Гость")
            {
                SaveLoader.Save(playerThatSolve);
            }
        }

        //игрок отгадывет число
        //принимает игрока который отгадывает число
        bool GuessNumber(Player player)
        {
            Console.WriteLine($"{player.Name} вам нужно отгадать число x, где {MinNumber}<= x <={MaxNumber}: ");
            int attempt;
            int attemptLast = 1;
            for (int temp = MaxNumber - MinNumber; temp > 1; temp = temp / 2)
            {
                attemptLast++;
            }

            do
            {
                attempt = ReadNumberFromConsole();

                if (attempt == SecretNumber)
                {
                    Console.WriteLine($"Правильно! получено {attemptLast} очков, \nНажмите любую клавишу для продолжения...");
                    player.Score += attemptLast;
                    Console.ReadKey();
                    return true;
                }
                else
                {
                    if (attemptLast > 0)
                    {
                        Console.WriteLine($"Не правильно, попробуйте еще раз. Осталось попыток {attemptLast--}");
                        if (attempt > SecretNumber)
                        {
                            Console.WriteLine($"Загаданное число меньше {attempt}");
                        }
                        else
                        {
                            Console.WriteLine($"Загаданное число больше {attempt}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("К сожалению у вас не осталось попыток ;(\nНажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        return false;
                    }
                }
            } while (true);
        }

        //ввод загадываемого числа и диапазона
        //принимает игрока, который загадывает число
        void InputNumbers(Player player)
        {
            Console.Clear();
            Console.WriteLine($"Игрок {player.Name} введите минимальное число, которое Вы можете загадать. Если не ввести, присвоится число 0:");
            MinNumber = ReadNumberFromConsole();
            Console.WriteLine($"Игрок {player.Name} введите максимальное число, которое Вы можете загадать. Если не ввести, присвоится число 0:");
            do
            {
                MaxNumber = ReadNumberFromConsole();
                if (MaxNumber <= MinNumber)
                {
                    Console.WriteLine($"{player.Name}, максимальное число должно быть больше {MinNumber}\n попробуйте еще раз:");
                }
                else
                {
                    break;
                }
            } while (true);
            do
            {
                Console.WriteLine($"Игрок {player.Name} введите число, которое Вы загадываете,\n" +
                    $"оно должно быть больше или равно {MinNumber}, и меньше или равно {MaxNumber}:");
                SecretNumber = ReadNumberFromConsole();
                if (SecretNumber >= MinNumber && SecretNumber <= MaxNumber)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{player.Name}, Вы ввели число, которое за границами допустимого диапазона");
                }
            } while (true);
        }

        //создает нового игрока
        Player CreateNewPlayer()
        {
            Player _newPlayer = new Player();
            Console.Clear();
            do
            {
                Console.WriteLine("Введите имя нового игрока:");
                _newPlayer.Name = Console.ReadLine();
                if (!SaveLoader.IsPlayerExist(_newPlayer.Name))
                {
                    Console.WriteLine($"{_newPlayer.Name} ведите пароль:");
                    _newPlayer.Password = Console.ReadLine();
                    _newPlayer.Score = 0;
                    SaveLoader.Save(_newPlayer);
                    return _newPlayer;
                }
                else
                {
                    Console.WriteLine($"Пользователь с именем {_newPlayer.Name} уже существует, введите другое имя:");
                }
            } while (true);
        }

        //загрузка игрока из базы
        //вернет null если такого игрока нет
        Player LoadPlayer()
        {
            string name;
            string password;
            Player player;
            Console.Clear();
            Console.WriteLine("Введите имя загружаемого игрока:");
            name = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            password = Console.ReadLine();
            player = SaveLoader.Load(name, password);
            if (player != null)
            {
                return player;
            }
            else
            {
                return new Player(){ Name="Гость", Score=0};
            }
        }

        //cчитывает число с консоли
        int ReadNumberFromConsole()
        {
            ConsoleKeyInfo key;
            var rule = @"[0-9]";
            var sb = new StringBuilder();
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;

                if (Regex.IsMatch(key.KeyChar.ToString(), rule))
                {
                    sb.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (sb.Length == 0)
            {
                return 0;
            }
            return int.Parse(sb.ToString());
        }

        //выводит данные игрока
        void WritePlayer(Player player)
        {
            Console.Clear();
            Console.WriteLine($"Имя игрока: {player.Name}\nИгрок имеет {player.Score} очков\nнажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}
