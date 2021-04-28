using System;
using System.Diagnostics;
using static System.Console;
namespace Forward
{
    class Program
    {
        const string X = "_HFFFHH";
        static double[,] forwardMatrix = new double[3, 7];
        static double[,] backwardMatrix = new double[3, 7];
        static void Main(string[] args)
        {
            forwardMatrix[1, 1] = 0.375d;
            forwardMatrix[2, 1] = 0.125d;
            _ = getForwardValue(6, 1);
            _ = getForwardValue(6, 2);
            Console.WriteLine("Forward:");
            printMatrix(forwardMatrix);
            backwardMatrix[1, 6] = 1d;
            backwardMatrix[2, 6] = 1d;
            _ = getBackwardValue(1, 1);
            _ = getBackwardValue(1, 2);
            Console.WriteLine("Backward:");
            printMatrix(backwardMatrix);


        }
        static double getBackwardValue(int i, int j)
        {
            if (i == 6) return 1d;
            double ret = 0.0d;
            for(int s = 1; s <= 2; s++)
            {
                var bv = Math.Round(backwardMatrix[s, i + 1], 9);
                if (bv == 0d)
                {
                    bv = Math.Round(getBackwardValue(i + 1, s), 9);
                    backwardMatrix[s, i + 1] = bv;
                }

                ret += bv * stateProb(s, j) * emitProb(i + 1, s);
                Debug.WriteLine(bv +" "+ stateProb(s, j) + " " + emitProb(i + 1, s));

            }
            ret = Math.Round(ret, 9);
            backwardMatrix[j, i] = ret;
            return ret;
            
        }
        static double getForwardValue(int i, int j)
        {
            if(i == 1)
            {
                if (j == 1) return 0.375d;
                if (j == 2) return 0.125d;
                throw new Exception();
            }
            double summation = 0.0d;
            for(int s = 1; s <= 2; s++)
            {
                var previousValue = forwardMatrix[s, i-1];
                if(previousValue == 0d)
                {
                    previousValue = getForwardValue(i - 1, s);
                    // fill DP table
                    forwardMatrix[s, i - 1] = previousValue;
                }
                previousValue *= stateProb(j, s);
                summation += previousValue;
            }
            var ret = Math.Round( emitProb(i, j) * summation , 9);
            forwardMatrix[j, i] = ret;
            //Debug.WriteLine(j +", "+i+":"+ ret);
            return ret;
        }
        static double emitProb(int charIndex, int stateNo)
        {
            return (X[charIndex], stateNo) switch
            {
                ('H', 1) => 0.75d,
                ('F', 1) => 0.25d,
                ('H', 2) => 0.25d,
                ('F', 2) => 0.75d,
                _ => throw new ArgumentException()
            };

        }
        static double stateProb(int dst, int src)
        {
            return (dst, src) switch
            {
                (1, 1)=>0.8d,
                (2, 1)=>0.2d,
                (1, 2)=>0.1d,
                (2, 2)=>0.9d,
                _ => throw new ArgumentException()
            };
        }
        static void printMatrix(double[,] mat)
        {
            for (int j = 1; j <= 2; j++)
            {
                for (int i = 1; i <= 6; i++)
                {
                    Write( String.Format("{0:0.000000000}",mat[j, i]) 
                        + ", ");
                }
                WriteLine("");
            }
        }
    }
}
