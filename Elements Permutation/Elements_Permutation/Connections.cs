using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements_Permutation
{
    static class Connections
    {
        static Dictionary<KeyValuePair<int,int>, int> connections = new();

        public static void AddConnection(int ID1, int ID2, int connection)
        {
            connections.Add(new KeyValuePair<int, int>(ID1, ID2), connection);
        }

        public static Dictionary<KeyValuePair<int, int>, int> FindAllConnections(int ID)
        {
            Dictionary<KeyValuePair<int, int>, int> connectionsID = new();

            foreach (KeyValuePair<int,int> kvp in connections.Keys)
            {
                if (kvp.Key == ID || kvp.Value == ID)
                {
                    connectionsID.Add(kvp, connections.GetValueOrDefault(kvp));
                }
            }
            return connectionsID;
        }

        private static int FindWay(int[,] array, int ID1, int ID2)
        {
            int way = 0, x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            
            for (int i = 0; i < array.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < array.GetUpperBound(1) + 1; j++)
                {
                    if (array[i,j] == ID1)
                    {
                        x1 = i; 
                        y1 = j;
                    }
                    if (array[i, j] == ID2)
                    {
                        x2 = i;
                        y2 = j;
                    }
                }
            }

            if (x1 > x2)
            {
                way += x1 - x2;
            }
            else
            {
                way += x2 - x1;
            }

            if (y1 > y2)
            {
                way += y1 - y2;
            }
            else
            {
                way += y2 - y1;
            }

            return way;
        }

        public static int FindK(int[,] array)
        {
            int k = 0;
            foreach (KeyValuePair<int, int> kvp in connections.Keys)
            {
                k += connections.GetValueOrDefault(kvp) * FindWay(array, kvp.Key, kvp.Value);
            }
            return k;
        }
    }
}