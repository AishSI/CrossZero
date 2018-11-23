using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossZero
{
    class Program
    {

        public enum Mark
        {
            Empty,
            Cross,
            Circle
        }

        public enum GameResult
        {
            CrossWin,
            CircleWin,
            Draw
        }

        public static void Main()
        {
            Run("XXX OO. ...");
            Run("OXO XO. .XO");
            Run("OXO XOX OX.");
            Run("XOX OXO OXO");
            Run("... ... ...");
            Run("XXX OOO ...");
            Run("XOO XOO XX.");
            Run(".O. XO. XOX");
        }

        private static void Run(string description)
        {
            Console.WriteLine(description.Replace(" ", Environment.NewLine));
            Console.WriteLine(GetGameResult(CreateFromString(description)));
            Console.WriteLine();
        }

        private static Mark[,] CreateFromString(string str)
        {
            var field = str.Split(' ');
            var ans = new Mark[3, 3];
            for (int x = 0; x < field.Length; x++)
                for (var y = 0; y < field.Length; y++)
                    ans[x, y] = field[x][y] == 'X' ? Mark.Cross : (field[x][y] == 'O' ? Mark.Circle : Mark.Empty);
            return ans;
        }

        public static GameResult GetGameResult(Mark[,] field)
        {
            var Winner = new int[field.GetLength(0) + field.GetLength(1) + 2]; //Одномерный массив, в ячейках хранит результаты по горизонталям (3 шт.), вертикалям (3 шт.) и диагоналям (2 шт.): победа "Х" = 3, победа "О" = -3, остальное - ничья или пустота 

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (var y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y] == Mark.Cross)
                    {
                        Winner[x]++; //считаем результаты по горизонтали
                        Winner[y + field.GetLength(1)]++; //считаем результаты по вертикали
                    }
                    else if (field[x, y] == Mark.Circle)
                    {
                        Winner[x]--;//считаем результаты по горизонтали
                        Winner[y + field.GetLength(1)]--; //считаем результаты по вертикали
                    }
                }

                // Также пробежимся под диагоналям:
                // 1-я диагональ
                if (field[x, x] == Mark.Cross)
                    Winner[Winner.Length - 2]++; // кол-во "Х" по 1-ой диагонали
                else if (field[x, x] == Mark.Circle)
                    Winner[Winner.Length - 2]--; // кол-во "О" по 1-ой диагонали

                //2 - я диагональ
                if (field[x, field.GetLength(0) - 1 - x] == Mark.Cross)
                    Winner[Winner.Length - 1]++; // кол-во "Х" по 2-ой диагонали
                else if (field[x, field.GetLength(0) - 1 - x] == Mark.Circle)
                    Winner[Winner.Length - 1]--; // кол-во "О" по 2-ой диагонали
            }

            /// Выводим результаты: если встречаются "-3" и нет 3, значит, выиграли "О".
            /// Если встречаются "3" и при этом нет "-3", выиграли "Х".
            /// В остальных случаях победила дружба! (или лень, если игроки просто не сделали ходов)
            if (Array.Exists(Winner, element => element == -3) && !Array.Exists(Winner, element => element == 3))
                return GameResult.CircleWin;
            else if (Array.Exists(Winner, element => element == 3) && !Array.Exists(Winner, element => element == -3))
                return GameResult.CrossWin;
            else
                return GameResult.Draw;
        }



    }
}
