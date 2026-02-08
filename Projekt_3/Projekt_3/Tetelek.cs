using System;
using System.Collections.Generic;

namespace Projekt_3
{
    internal class Tetelek
    {
        public static double Megszamolas(double[] tomb, double keresett)
        {
            int c = 0;
            for (int i = 0; i < tomb.Length; i++)
                if (tomb[i] == keresett)
                    c++;
            return c;
        }
        public static bool Eldontes(double[] tomb, double keresett)
        {
            bool vanE = false;
            foreach (var item in tomb)
            {
                if (item == keresett)
                {
                    vanE = true;
                }
            }
            return vanE;
        }
        public static double[] Masolas(double[] tomb)
        {
            double[] ujTomb = new double[tomb.Length];
            for (int i = 0; i < tomb.Length; i++)
            {
                ujTomb[i] = tomb[i];
            }
            return ujTomb;
        }
        public static double[] Metszet(double[] A, double[] B)
        {
            List<double> metszet = new List<double>();
            for (int i = 0; i < A.Length; i++)
            {
                foreach (var item in B)
                {
                    if (A[i] == item)
                    {
                        metszet.Add(item);
                    }
                }
            }
            return metszet.ToArray();
        }
        public static double[] egyszeruCseres(double[] tomb)
        {
            double temp;
            for (int i = 0; i < tomb.Length - 1; i++)
            {
                for (int j = i + 1; j < tomb.Length; j++)
                {
                    if (tomb[i] > tomb[j])
                    {
                        temp = tomb[j];
                        tomb[j] = tomb[i];
                        tomb[i] = temp;
                    }
                }
            }
            return tomb;
        }
        public static double[] MinMax(double[] tomb)
        {
            int index;
            double seged;
            for (int i = 0; i < tomb.Length - 1; i++)
            {
                index = i;
                for (int j = i + 1; j < tomb.Length; j++)
                {
                    if (tomb[index] > tomb[j])
                    {
                        index = j;
                    }
                    seged = tomb[i];
                    tomb[i] = tomb[index];
                    tomb[index] = seged;
                }
            }
            return tomb;
        }
        public static int LinearisKereses(double[] tomb, double elem)
        {
            int index = -1;
            for (int i = 0; i < tomb.Length; i++)
            {
                if (tomb[i] == elem)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public static double BinarisKereses(double[] tomb, double keresettertek)
        {
            Array.Sort(tomb);
            int eleje = 0;
            int vege = tomb.Length - 1;
            while (eleje <= vege)
            {
                int i = (eleje + vege) / 2;
                if (tomb[i] == keresettertek) return i;
                else if (tomb[i] < keresettertek)
                {
                    eleje = i + 1;
                }
                else if (tomb[i] > keresettertek)
                {
                    vege = i - 1;
                }
            }
            return -1;
        }
    }
}