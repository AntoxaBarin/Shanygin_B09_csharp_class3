using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;
using System.Text;

namespace Task2
{
    public class Task2
    {

        /*
         * В этом задании необходимо написать программу, способную табулировать сразу несколько
         * функций одной вещественной переменной на одном заданном отрезке.
         */


        // Сформируйте набор как минимум из десяти вещественных функций одной переменной
        internal static Dictionary<FunctionName, OneVariableFunction> AvailableFunctions =
            new Dictionary<FunctionName, OneVariableFunction>
            {
                { "square", x => x * x },
                { "sin", Math.Sin },
                { "cos", Math.Cos },
                { "tan", Math.Tan },
                { "square root", Math.Sqrt },
                { "arcsin", Math.Asin },
                { "arcos", Math.Acos },
                { "arctan", Math.Atan },
                { "abs", x => Math.Abs(x) },
                { "log2", x => Math.Log2(x) }
            };

        // Тип данных для представления входных данных
        internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

        // Чтение входных данных из параметров командной строки
        private static InputData? prepareData(string[] args)
        {

            if (args.Length <= 3)
            {
                return null; // Если <= 3 аргументов, то возрвращаем null;
            }

            if (double.TryParse(args[0], out double fromX) == false) // проверяем, чтоб всё нормально преобразовалось
            {
                return null;
            }

            if (double.TryParse(args[1], out double toX) == false)
            {
                return null;
            }

            if (int.TryParse(args[2], out int numberOfPoints) == false)
            {
                return null;
            }
            //Если введенные данные удовлетворяют типам, то всё нормально, иначе null;

            var functionNames =
                args.AsEnumerable().Skip(3)
                    .ToList(); // Делаем список из параметров командной строки, исключая первые три элемента
            if (functionNames.Any(name => !AvailableFunctions.ContainsKey(name)))
            {
                return null;
            } // Если ввели функцию, которой нет => return null

            return new InputData(fromX, toX, numberOfPoints, functionNames);
        }

        // Тип данных для представления таблицы значений функций
        // с заголовками столбцов и строками (первый столбец --- значение x,
        // остальные столбцы --- значения функций). Одно из полей --- количество знаков
        // после десятичной точки.
        internal record FunctionTable
        {
            public double[,] Results;   // двумерный массив со значениями функций
            public int Precision;
            public string[] Top;   // шапка таблицы


            public FunctionTable(int precision, string[] top, double[,] results)
            {
                Top = top;
                Results = results;
                Precision = precision;
            }


            public static FunctionTable Create(InputData inputData)
            {
                var top = (inputData.FunctionNames.Prepend("x")).ToArray();

                var samples = new double[inputData.NumberOfPoints, top.Length];

                var step = (inputData.ToX - inputData.FromX) /
                           (inputData.NumberOfPoints == 1 ? 1 : inputData.NumberOfPoints - 1);

                for (int i = 0; i < samples.GetLength(0); i++)
                {
                    var x = inputData.FromX + i * step;
                    samples[i, 0] = x;

                    for (int j = 0; j < inputData.FunctionNames.Count; j++)
                    {
                        var name = inputData.FunctionNames[j];
                        var fn = AvailableFunctions[name];

                        samples[i, j + 1] = fn(x);
                    }
                }

                return new FunctionTable(precision: 6, top: top, results: samples);
            }


            // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
            // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
            // Для форматирования можно использовать функцию String.Format.
            public override string ToString()
            {
                var sizes = Top.Select(str => str.Length).ToArray();
                var strValues = new string[Results.GetLength(0), Results.GetLength(1)];

                for (int i = 0; i < Results.GetLength(0); i++)
                {
                    for (int j = 0; j < Top.Length; j++)
                    {
                        strValues[i, j] = FormatNum(Results[i, j]);
                        sizes[j] = Math.Max(sizes[j], strValues[i, j].Length);
                    }
                }

                var builder = new StringBuilder();

                for (int j = 0; j < Top.Length; j++)
                {
                    builder.Append(Top[j].PadLeft(sizes[j]));
                }

                builder.AppendLine();

                for (int i = 0; i < Results.GetLength(0); i++)
                {
                    for (int j = 0; j < Top.Length; j++)
                    {
                        builder.Append(strValues[i, j].PadLeft(sizes[j] + (j == 0 ? 0 : 1)));
                    }

                    if (i != Results.GetLength(0) - 1) builder.AppendLine();
                }

                return builder.ToString();
            }

            private string FormatNum(double num) => string.Format($"{{0:F{Precision}}}", num);
        }

        /*
         * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
         * с заданным количеством точек.
         */
        internal static FunctionTable tabulate(InputData input)
        {
            return FunctionTable.Create(input);
        }

        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            var input = prepareData(args);

            if (input == null)
            {
                return;
            }
            // Собственно табулирование и печать результата (что надо поменять в этой строке?):
            Console.WriteLine(tabulate(input));
        }




   

    }
}
