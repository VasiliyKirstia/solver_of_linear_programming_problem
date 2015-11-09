using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optim_Methods
{
    /// <summary>
    /// режим отображения
    /// </summary>
    public enum DisplayMode
    {
        /// <summary>
        /// вывести в отдельном окне
        /// </summary>
        InWindow,

        /// <summary>
        /// вывести в консоль
        /// </summary>
        InConsole
    }

    /// <summary>
    /// Симплексная таблица в каноническом виде
    /// </summary>
    public class CanonicalSimplexTable
    {
        /// <summary>
        /// количество учитываемых разрядов после десятичной точки при сравнении с нулём (SimplexNumber * 10^(-roundTo-1) === 0)
        /// </summary>
        private static int roundTo = 15; 

        /// <summary>
        /// номера базисных переменных
        /// </summary>
        private int[] X_I;

        /// <summary>
        /// симплексная таблица
        /// </summary>
        private SimplexNumber[,] SimplexTable;

        /// <summary>
        /// Симплексная таблица в каноническом виде
        /// </summary>
        /// <param name="x_i">вектор номеров базисных элементов</param>
        /// <param name="c_j">коефициенты при неизвестных в целевой функции</param>
        /// <param name="equationSystem">ситема уравнений</param>
        /// <param name="b">вектор свободных членов</param>
        public CanonicalSimplexTable(int[] x_i, double[] c_j, double[,] equationSystem, double[] b)
            : this(x_i, (from c in c_j select new SimplexNumber(c)).ToArray<SimplexNumber>(), equationSystem, b) { }

        /// <summary>
        /// Симплексная таблица в каноническом виде
        /// </summary>
        /// <param name="x_i">вектор номеров базисных элементов</param>
        /// <param name="c_j">коефициенты при неизвестных в целевой функции</param>
        /// <param name="equationSystem">ситема уравнений</param>
        /// <param name="b">вектор свободных членов</param>
        public CanonicalSimplexTable(int[] x_i, SimplexNumber[] c_j, double[,] equationSystem, double[] b)
        {
            //сохраняем номера базисных переменых
            X_I = x_i;
            //создаем симплекс-таблицу
            SimplexTable = new SimplexNumber[equationSystem.GetLength(0)+1, equationSystem.GetLength(1)+1];
            //добавляем в таблицу a_ij
            for (int i = 0; i < equationSystem.GetLength(0); i++)
                for (int j = 0; j < equationSystem.GetLength(1); j++)
                    SimplexTable[i, j] = new SimplexNumber(equationSystem[i, j]);
            //... b_i
            for (int i = 0; i < b.Length; i++)
                SimplexTable[i, equationSystem.GetLength(1)] = new SimplexNumber(b[i]);
            //... delta_j
            for (int j = 0; j < equationSystem.GetLength(1); j++)
            {
                SimplexTable[equationSystem.GetLength(0), j] = -c_j[j];
                for (int i = 0; i < equationSystem.GetLength(0); i++)
                    SimplexTable[equationSystem.GetLength(0), j] += c_j[x_i[i]] * SimplexTable[i, j];
            }
            //... Z(b_i)
            for (int i = 0; i < b.Length; i++)
                SimplexTable[equationSystem.GetLength(0), equationSystem.GetLength(1)] += c_j[x_i[i]] * b[i];
        }

        /// <summary>
        /// находит значения независимых переменных при которых целевая функция максимизируется
        /// </summary>
        public void Maximize(bool showSteps = true)
        {
            while (true) {
                bool rowWasFound = false;
                bool colWasFound = false;
                //отображаем текущее состояние если нужно
                if (showSteps)
                {
                    ShowTable(DisplayMode.InConsole);
                    Console.WriteLine();
                }
                //находим номер столбца с отрицательной delta
                int col_min = 0;
                for (int j = 0; j < SimplexTable.GetLength(1) - 1; j++)
                    if (SimplexTable[SimplexTable.GetLength(0) - 1, j].Round(roundTo) < 0 && SimplexTable[SimplexTable.GetLength(0) - 1, j] <= SimplexTable[SimplexTable.GetLength(0) - 1, col_min])
                    {
                        var t = SimplexTable[SimplexTable.GetLength(0) - 1, j];
                        var vg = SimplexTable[SimplexTable.GetLength(0) - 1, j].Round(roundTo) < 0;
                        col_min = j;
                        colWasFound = true;
                    }
                //если полученая на предыдущем шаге таблица оптимальна выводим результат и выходим из функции
                if (!colWasFound)
                {
                    SimplexNumber[] rez = new SimplexNumber[SimplexTable.GetLength(1)-1];
                    for (int j = 0; j < SimplexTable.GetLength(1)-1; j++)
                    {
                        int k = -1;
                        for (int i = 0; i < X_I.Length; i++)
                        {
                            if (X_I[i] == j)
                                k = i;
                        }
                        rez[j] = k == -1 ? new SimplexNumber(0) : SimplexTable[k, SimplexTable.GetLength(1) - 1].Round(4);
                    }
                    MessageBox.Show("Целевая функция имеет максимум при X=(" + String.Join("; ",rez) + "), при этом Z(X)=" + SimplexTable[SimplexTable.GetLength(0) - 1, SimplexTable.GetLength(1) - 1] + ".", "Вот так вот!", MessageBoxButtons.OK);
                    return;
                }

                //находим номер строки с минимальной b_i/a_is
                int row_min = 0;
                for (int i = 0; i < SimplexTable.GetLength(0) - 1; i++)
                {
                    if (SimplexTable[i, col_min].Round(roundTo) > 0 && (SimplexTable[i, SimplexTable.GetLength(1) - 1] / SimplexTable[i, col_min] <= SimplexTable[row_min, SimplexTable.GetLength(1) - 1] / SimplexTable[row_min, col_min] || SimplexTable[row_min, col_min].Round(roundTo)<=0))
                    {
                        row_min = i;
                        rowWasFound = true;
                    }
                }
                //если все a_is не положительны выводим соответствующее сообщение
                if (!rowWasFound)
                {
                    MessageBox.Show("Функция неограничена в даной области решений.", "Вот так вот!", MessageBoxButtons.OK);
                    return;
                }
                //в противном случае делаем еще один шаг
                MakeStep(row_min,col_min);
            }
        }

        /// <summary>
        /// выполняет одну итерацию симплекс-метода
        /// </summary>
        /// <param name="row_min">номер строки разрешающего элемента</param>
        /// <param name="col_min">номер столбца разрешающего элемента</param>
        public void MakeStep(int row_min, int col_min)
        {
            SimplexNumber[,] newSimplexTable = new SimplexNumber[SimplexTable.GetLength(0), SimplexTable.GetLength(1)]; 
            //выполняем одну итерацию симплексного метода
            SimplexNumber rElement = SimplexTable[row_min, col_min];
            for (int i = 0; i < SimplexTable.GetLength(0); i++)
                for (int j = 0; j < SimplexTable.GetLength(1); j++)
                {
                    if (i == row_min)
                    {
                        newSimplexTable[i, j] = SimplexTable[i, j] / rElement;
                        continue;
                    }
                    newSimplexTable[i, j] = SimplexTable[i, j] - SimplexTable[row_min, j] * SimplexTable[i, col_min] / rElement;//from caet
                }
            //устанавливаем новый базис
            X_I[row_min] = col_min;
            //фиксируем полученый результат
            SimplexTable = newSimplexTable;
        }

        /// <summary>
        /// отобразить текущее состояние симплексной таблицы
        /// </summary>
        /// <param name="displayMode">режим отображения</param>
        public void ShowTable(DisplayMode displayMode = DisplayMode.InWindow)
        {
            //отображаем таблицу
            switch (displayMode)
            {
                case DisplayMode.InWindow:
                    MessageBox.Show(this.ToString(), String.Join(", ", X_I), MessageBoxButtons.OK);
                    break;
                case DisplayMode.InConsole:
                    Console.Write(this.ToString());
                    break;
                default:
                    Console.Write(this.ToString());
                    break;
            }
        }

        public override string ToString()
        {
            //резервируем память с расчетом что каждый элемент таблицы занимает на меньше 4х символов
            StringBuilder table = new StringBuilder(SimplexTable.GetLength(0) * SimplexTable.GetLength(1) * 4);
            //подготавливаем таблицу для красивого отображения 
            for (int i = 0; i < SimplexTable.GetLength(0); i++)
            {
                for (int j = 0; j <= SimplexTable.GetLength(1); j++)
                {
                    if (0 == j && X_I.Length > i)
                        table.Append(X_I[i]+1 + "\t");
                    if (0 == j && i == SimplexTable.GetLength(0)-1)
                        table.Append("Z\t");
                    if(0 < j)
                        table.Append(SimplexTable[i, j-1].Round(4) + "\t");
                }
                table.Append("\n");
            }
            return table.ToString();
        }
    }
}