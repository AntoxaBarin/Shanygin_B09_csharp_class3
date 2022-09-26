using OneVariableFunction = System.Func<double, double>;
using FunctionName = System.String;

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
                { TODO<String>(), TODO<Func<double, double>>() }
            };

// Тип данных для представления входных данных
internal record InputData(double FromX, double ToX, int NumberOfPoints, List<string> FunctionNames);

// Чтение входных данных из параметров командной строки
        private static InputData? prepareData(string[] args)
        {
            return TODO<InputData>();
        }

// Тип данных для представления таблицы значений функций
// с заголовками столбцов и строками (первый столбец --- значение x,
// остальные столбцы --- значения функций). Одно из полей --- количество знаков
// после десятичной точки.
internal record FunctionTable
{
            // Код, возвращающий строковое представление таблицы (с использованием StringBuilder)
            // Столбец x выравнивается по левому краю, все остальные столбцы по правому.
            // Для форматирования можно использовать функцию String.Format.
            public override string ToString()
            {
                throw new NotImplementedException();
            }
        }

/*
 * Возвращает таблицу значений заданных функций на заданном отрезке [fromX, toX]
 * с заданным количеством точек.
 */
        internal static FunctionTable tabulate(InputData input)
        {
            throw new NotImplementedException();
        }
        
        public static void Main(string[] args)
        {
            // Входные данные принимаются в аргументах командной строки
            // fromX fromY numberOfPoints function1 function2 function3 ...

            var input = prepareData(args);

            // Собственно табулирование и печать результата (что надо поменять в этой строке?):
            Console.WriteLine(tabulate(input));
        }
        
        private static T TODO<T>()
        {
            throw new NotImplementedException();
        }

    }
}