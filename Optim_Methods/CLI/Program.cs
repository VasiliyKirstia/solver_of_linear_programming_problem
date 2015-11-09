using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optim_Methods;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            CanonicalSimplexTable st = new CanonicalSimplexTable(new int[] { 2,3,4,6 }, new SimplexNumber[] { 
                new SimplexNumber(-5),new SimplexNumber(-1),new SimplexNumber(0),new SimplexNumber(0),new SimplexNumber(0),new SimplexNumber(0),new SimplexNumber(0,-1)
            },
                new double[,]{
                {-1,3,1,0,0,0,0},
                {2,3,0,1,0,0,0},
                {2,-1,0,0,1,0,0},
                {4,2,0,0,0,-1,1}
                }, new double[] { 16,28,20,24 });

            //CanonicalSimplexTable st = new CanonicalSimplexTable(new int[] { 0, 1, 6 }, new SimplexNumber[] { 
            //    new SimplexNumber(20),new SimplexNumber(20),new SimplexNumber(2),new SimplexNumber(0),new SimplexNumber(0),new SimplexNumber(0),new SimplexNumber(0,-1),
            //},
            //    new double[,]{
            //    {1,0,-8/9.0,1/9.0,-5/9.0,0,0},
            //    {0,1,2,0,1,0,0},
            //    {0,0,1/9.0,1/9.0,4/9.0,-1,1}
            //    }, new double[] { 215/9.0,8,8/9.0 });

            //CanonicalSimplexTable st = new CanonicalSimplexTable(new int[] { 2,3, 4 }, new double[] { 4,2,0,0,0},
            //    new double[,]{
            //    {-1,3,1,0,0},
            //    {2,3,0,1,0},
            //    {2,-1,0,0,1},
            //    }, new double[] { 16,28,20 });
            
            st.Maximize();
            Console.ReadKey();
        }
    }
}
