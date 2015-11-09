using System;

namespace Optim_Methods
{
    /// <summary>
    /// Недопустимая операция
    /// </summary>
    public class InvalidOperationException : Exception { }

    /// <summary>
    /// универсальный тип данных используемый при решении ЗЛП и М-задач
    /// </summary>
    public struct SimplexNumber
    {
        /// <summary>
        /// действительная часть
        /// </summary>
        private double _Real;

        /// <summary>
        /// аналог мнимой части (коэфициент при М)
        /// </summary>
        private double _Imaginary;

        /// <summary>
        /// свойство получающее действительную часть
        /// </summary>
        public double Real
        {
            set { }
            get { return _Real; }
        }

        /// <summary>
        /// свойство получающее аналог мнимой части (коэфициент при М)
        /// </summary>
        public double Imaginary
        {
            set { }
            get { return _Imaginary; }
        }

        /// <summary>
        /// создать новое число без мнимой части
        /// </summary>
        /// <param name="real">действительная часть</param>
        public SimplexNumber(double real) : this(real, 0d) { }

        /// <summary>
        /// создать новое число
        /// </summary>
        /// <param name="real">действительная часть</param>
        /// <param name="imaginary">мнимая часть</param>
        public SimplexNumber(double real, double imaginary)
        {
            _Real = real;
            _Imaginary = imaginary;
        }

        public static SimplexNumber operator -(SimplexNumber op1)
        {
            return new SimplexNumber {_Real = -op1._Real,_Imaginary=-op1._Imaginary };
        }

        public static SimplexNumber operator +(SimplexNumber op1, double op2)
        {
            return new SimplexNumber { _Imaginary = op1._Imaginary, _Real = op1._Real + op2 };
        }

        public static SimplexNumber operator +(double op1, SimplexNumber op2)
        {
            return new SimplexNumber { _Imaginary = op2._Imaginary, _Real = op2._Real + op1 };
        }

        public static SimplexNumber operator +(SimplexNumber op1, SimplexNumber op2)
        {
            return new SimplexNumber { _Real = op1._Real + op2._Real, _Imaginary = op1._Imaginary + op2._Imaginary };
        }

        public static SimplexNumber operator -(SimplexNumber op1, double op2)
        {
            return new SimplexNumber { _Imaginary = op1._Imaginary, _Real = op1._Real - op2 };
        }

        public static SimplexNumber operator -(double op1, SimplexNumber op2)
        {
            return new SimplexNumber { _Imaginary = -op2._Imaginary, _Real = op1 - op2._Real };
        }

        public static SimplexNumber operator -(SimplexNumber op1, SimplexNumber op2)
        {
            return new SimplexNumber { _Real = op1._Real - op2._Real, _Imaginary = op1._Imaginary - op2._Imaginary };
        }

        public static SimplexNumber operator *(SimplexNumber op1, double op2)
        {
            return new SimplexNumber { _Imaginary = op1._Imaginary * op2, _Real = op1._Real * op2 };
        }

        public static SimplexNumber operator *(double op1, SimplexNumber op2)
        {
            return new SimplexNumber { _Imaginary = op2._Imaginary * op1, _Real = op2._Real * op1 };
        }

        public static SimplexNumber operator *(SimplexNumber op1, SimplexNumber op2)
        {
            return new SimplexNumber
            {
                _Imaginary = op1._Imaginary * op2._Imaginary + op1._Real * op2._Imaginary + op1._Imaginary * op2._Real,
                _Real = op1._Real * op2._Real
            };
        }

        public static SimplexNumber operator /(SimplexNumber op1, double op2)
        {
            return new SimplexNumber { _Imaginary = op1._Imaginary / op2, _Real = op1._Real / op2 };
        }

        public static SimplexNumber operator /(double op1, SimplexNumber op2)
        {
            if (op2._Imaginary != 0)
                throw new InvalidOperationException();
            return new SimplexNumber { _Imaginary = 0d, _Real = op1 / op2._Real };
        }

        public static SimplexNumber operator /(SimplexNumber op1, SimplexNumber op2)
        {
            if (op2._Imaginary == 0)
                return new SimplexNumber { _Imaginary = op1._Imaginary / op2._Real, _Real = op1._Real / op2._Real };
            throw new InvalidOperationException();
        }

        public static bool operator <(double op1, SimplexNumber op2)
        {
            return (op2._Imaginary > 0) || (op2._Imaginary == 0 && op2._Real > op1);
        }

        public static bool operator <(SimplexNumber op1, double op2)
        {
            return (op1._Imaginary < 0) || (op1._Imaginary == 0 && op1._Real < op2);
        }

        public static bool operator <(SimplexNumber op1, SimplexNumber op2)
        {
            return (op1._Imaginary < op2._Imaginary) || (op1._Imaginary == op2._Imaginary  && op1._Real < op2._Real);
        }

        public static bool operator >(double op1, SimplexNumber op2)
        {
            return (op2._Imaginary < 0) || (op2._Imaginary == 0 && op2._Real < op1);
        }

        public static bool operator >(SimplexNumber op1, double op2)
        {
            return (op1._Imaginary > 0) || (op1._Imaginary == 0 && op1._Real > op2);
        }

        public static bool operator >(SimplexNumber op1, SimplexNumber op2)
        {
            return (op1._Imaginary > op2._Imaginary) || (op1._Imaginary == op2._Imaginary && op1._Real > op2._Real);
        }

        public static bool operator <=(double op1, SimplexNumber op2)
        {
            return (op2._Imaginary > 0) || (op2._Imaginary == 0 && op2._Real >= op1);
        }

        public static bool operator <=(SimplexNumber op1, double op2)
        {
            return (op1._Imaginary < 0) || (op1._Imaginary == 0 && op1._Real <= op2);
        }

        public static bool operator <=(SimplexNumber op1, SimplexNumber op2)
        {
            return (op1._Imaginary <= op2._Imaginary && op1._Imaginary !=  op2._Imaginary) ||
                (op1._Imaginary == op2._Imaginary && op1._Real <= op2._Real);
        }

        public static bool operator >=(double op1, SimplexNumber op2)
        {
            return (op2._Imaginary < 0) || (op2._Imaginary == 0 && op2._Real <= op1);
        }

        public static bool operator >=(SimplexNumber op1, double op2)
        {
            return (op1._Imaginary > 0) || (op1._Imaginary == 0 && op1._Real >= op2);
        }

        public static bool operator >=(SimplexNumber op1, SimplexNumber op2)
        {
            return (op1._Imaginary >= op2._Imaginary && op1._Imaginary !=  op2._Imaginary) ||
                (op1._Imaginary == op2._Imaginary && op1._Real >= op2._Real);
        }

        public static bool operator ==(double op1, SimplexNumber op2)
        {
            return op2._Imaginary == 0 && op2._Real == op1;
        }

        public static bool operator ==(SimplexNumber op1, double op2)
        {
            return op1._Imaginary == 0 && op1._Real == op2;
        }

        public static bool operator ==(SimplexNumber op1, SimplexNumber op2)
        {
            return op1._Imaginary == op2._Imaginary && op1._Real == op2._Real;
        }

        public static bool operator !=(double op1, SimplexNumber op2)
        {
            return op2._Imaginary == 0 && op2._Real != op1;
        }

        public static bool operator !=(SimplexNumber op1, double op2)
        {
            return op1._Imaginary == 0 && op1._Real != op2;
        }

        public static bool operator !=(SimplexNumber op1, SimplexNumber op2)
        {
            return op1._Imaginary != op2._Imaginary || op1._Real != op2;
        }

        /// <summary>
        /// возвращает строковое представление числа
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return 
                this._Imaginary==0?
                    this._Real.ToString():
                this._Real < 0?
                    String.Format("{0}M - {1}",this._Imaginary,Math.Abs(this._Real)):
                this._Real > 0?
                    String.Format("{0}M + {1}", this._Imaginary, this._Real):
                    String.Format("{0}M", this._Imaginary);
        }

        /// <summary>
        /// округляет одновременно действительную и мнимую части до заданого количества дробных разрядов
        /// </summary>
        /// <param name="digits">количество разрядов до которых производится округление</param>
        /// <param name="midpointRounding">способ округления</param>
        /// <returns></returns>
        public SimplexNumber Round(int digits, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
        {
            return new SimplexNumber
            {
                _Imaginary = Math.Round(this._Imaginary, digits, midpointRounding),
                _Real = Math.Round(this._Real, digits, midpointRounding)
            };
        }
    }
}