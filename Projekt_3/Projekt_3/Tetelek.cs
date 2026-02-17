using System;
using System.Collections.Generic;

namespace Projekt_3
{
    internal class Tetelek
    {
        public static int Megszamolas(int[] tomb, int keresett)
        {
            int c = 0;
            for (int i = 0; i < tomb.Length; i++)
                if (tomb[i] == keresett)
                    c++;
            return c;
        }
        public static bool Eldontes(int[] tomb, int keresett)
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
        public static int[] Masolas(int[] tomb)
        {
            int[] ujTomb = new int[tomb.Length];
            for (int i = 0; i < tomb.Length; i++)
            {
                ujTomb[i] = tomb[i];
            }
            return ujTomb;
        }
        public static int[] Metszet(int[] A, int[] B)
        {
            List<int> metszet = new List<int>();
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
        public static int[] egyszeruCseres(int[] tomb)
        {
            int temp;
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
        public static int[] MinMax(int[] tomb)
        {
            int index;
            int seged;
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
        public static int LinearisKereses(int[] tomb, int elem)
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
        public static int BinarisKereses(int[] tomb, int keresettertek)
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