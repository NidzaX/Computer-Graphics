using ProjektniZadatak1.Model;
using System.Collections.Generic;

namespace ProjektniZadatak1.Helpers
{
    internal class BFSPath
    {
        public static FieldPosition[,] BFSFind(LineEntity line, char[,] BFSlines)
        {
            BFSlines[(int)line.PocetakX, (int)line.PocetakY] = 'P';
            BFSlines[(int)line.KrajX, (int)line.KrajY] = 'K';

            FieldPosition pocetak = new FieldPosition((int)line.PocetakX, (int)line.PocetakY);
            Queue<FieldPosition> queue = new Queue<FieldPosition>();
            queue.Enqueue(pocetak);

            bool[,] poseceno = new bool[301, 301]; //dimenzije matrice
            for (int i = 0; i <= 300; i++)
            {
                for (int j = 0; j <= 300; j++)
                {
                    poseceno[i, j] = false;
                }
            }
            poseceno[pocetak.PozX, pocetak.PozY] = true;

            FieldPosition[,] prev = new FieldPosition[301, 301];
            for (int i = 0; i <= 300; i++)
            {
                for (int j = 0; j <= 300; j++)
                {
                    prev[i, j] = null;
                }
            }

            while (queue.Count != 0)
            {
                FieldPosition polje = queue.Dequeue();

                if (BFSlines[polje.PozX, polje.PozY] == 'K')
                {
                    BFSlines[(int)line.PocetakX, (int)line.PocetakY] = ' ';
                    BFSlines[(int)line.KrajX, (int)line.KrajY] = ' ';
                    return prev;
                }

                if (isValid(polje.PozX - 1, polje.PozY, poseceno, BFSlines))
                {
                    queue.Enqueue(new FieldPosition(polje.PozX - 1, polje.PozY));
                    poseceno[polje.PozX - 1, polje.PozY] = true;
                    prev[polje.PozX - 1, polje.PozY] = polje;
                }

                if (isValid(polje.PozX + 1, polje.PozY, poseceno, BFSlines))
                {
                    queue.Enqueue(new FieldPosition(polje.PozX + 1, polje.PozY));
                    poseceno[polje.PozX + 1, polje.PozY] = true;
                    prev[polje.PozX + 1, polje.PozY] = polje;
                }

                if (isValid(polje.PozX, polje.PozY - 1, poseceno, BFSlines))
                {
                    queue.Enqueue(new FieldPosition(polje.PozX, polje.PozY - 1));
                    poseceno[polje.PozX, polje.PozY - 1] = true;
                    prev[polje.PozX, polje.PozY - 1] = polje;
                }

                if (isValid(polje.PozX, polje.PozY + 1, poseceno, BFSlines))
                {
                    queue.Enqueue(new FieldPosition(polje.PozX, polje.PozY + 1));
                    poseceno[polje.PozX, polje.PozY + 1] = true;
                    prev[polje.PozX, polje.PozY + 1] = polje;
                }
            }
            BFSlines[(int)line.PocetakX, (int)line.PocetakY] = ' ';
            BFSlines[(int)line.KrajX, (int)line.KrajY] = ' ';
            return prev;
        }

        private static bool isValid(int pozX, int pozY, bool[,] poseceno, char[,] bFSlinije)
        {
            if (pozX >= 0 && pozY >= 0 && pozX < 300 && pozY < 300 && bFSlinije[pozX, pozY] != 'X' && poseceno[pozX, pozY] == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<FieldPosition> ReconstructingPath(FieldPosition pocetak, FieldPosition kraj, FieldPosition[,] prev)
        {
            List<FieldPosition> path = new List<FieldPosition>();
            FieldPosition field;

            for (field = kraj; field != null; field = prev[field.PozX, field.PozY])
            {
                path.Add(field);
            }
            path.Reverse();

            if (path[0].PozX == pocetak.PozX && path[0].PozY == pocetak.PozY)
            {
                return path;
            }
            else
            {
                return null;
            }
        }
    }
}
