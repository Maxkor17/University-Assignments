using System.Net;
using System.Runtime.InteropServices;
using System.Xml;

int[,] array = new int[,]
{
    {0, 0, 0, 0, 0},
    {0, 0, 0, 1, 0},
    {0, 0, 1, 0, 0},
    {0, 0, 1, 1, 0},
    {0, 1, 0, 0, 1},
    {0, 1, 0, 1, 1},
    {0, 1, 1, 0, 1},
    {0, 1, 1, 1, 0},
    {1, 0, 0, 0, 0},
    {1, 0, 0, 1, 0},
    {1, 0, 1, 0, 1},
    {1, 0, 1, 1, 1},
    {1, 1, 0, 0, 0},
    {1, 1, 0, 1, 1},
    {1, 1, 1, 0, 1},
    {1, 1, 1, 1, 0}
};

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Таблиця істинності вхідних і вихідних сигналів:");
Console.WriteLine("X1\tX2\tX3\tX4\tY");
for (int i = 0; i <= array.GetUpperBound(0); i++)
{
    for (int j = 0;  j <= array.GetUpperBound(1); j++)
    {
        if (array[i, array.GetUpperBound(1)] == 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        Console.Write(array[i, j] + "\t");
    }
    Console.WriteLine();
}
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("ДДНФ: " + DDNF(array));

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("ДКНФ: " + DKNF(array));
Console.ResetColor();

int[,] karnaughMap = KarnaughMap(array);

Console.WriteLine();
Console.WriteLine("Karnaugh map:");
for (int i = 0; i <= karnaughMap.GetUpperBound(0); i++)
{
    for (int j = 0; j <= karnaughMap.GetUpperBound(1); j++)
    {
        Console.Write(karnaughMap[i, j] + "\t");
    }
    Console.WriteLine();
}

bool[,] status = new bool[(array.GetUpperBound(0) + 1) / 4, array.GetUpperBound(1)];
List<List<KeyValuePair<int, int>>> patterns = new();
bool patternFound;
FindAllPatterns();
OutputPatterns();

/*Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("ДДНФmin:");

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("ДКНФmin:");
Console.ResetColor();*/

void FindAllPatterns()
{
    for (int i = 0; i <= karnaughMap.GetUpperBound(0); i++)
    {
        for (int j = 0; j <= karnaughMap.GetUpperBound(1); j++)
        {
            if (status[i, j] == false)
            {
                FindPattern(i, j);
            }
        }
    }
}
void FindPattern(int x, int y)
{
    patternFound = false;
    if (!patternFound)
    {
        VerticalLong(x, y);
    }
    if (!patternFound)
    {
        HorizontalLong(x, y);
    }
    if (!patternFound)
    {
        Square(x, y);
    }
    if (!patternFound)
    {
        HorizontalShort(x, y);
    }
    if (!patternFound)
    {
        VerticalShort(x, y);
    }
}
void VerticalLong(int x, int y)
{
    List<KeyValuePair<int,int>> pattern = new();
    bool IsVerticalLong = true;
    for (int i = 0; i <= karnaughMap.GetUpperBound(0); i++)
    {
        if (karnaughMap[i, y] != karnaughMap[x, y])
        {
            IsVerticalLong = false;
            break;
        } 
    }
    if (IsVerticalLong == true)
    {
        for (int i = 0; i <= karnaughMap.GetUpperBound(0); i++)
        {
            pattern.Add(new KeyValuePair<int, int>(i, y));
            status[i, y] = true;
        }
        patterns.Add(pattern);
        patternFound = true;
    }
}
void HorizontalLong(int x, int y)
{
    List<KeyValuePair<int, int>> pattern = new();
    bool IsVerticalLong = true;
    for (int j = 0; j <= karnaughMap.GetUpperBound(1); j++)
    {
        if (karnaughMap[x, j] != karnaughMap[x, y])
        {
            IsVerticalLong = false;
            break;
        }
    }
    if (IsVerticalLong == true)
    {
        for (int j = 0; j <= karnaughMap.GetUpperBound(1); j++)
        {
            pattern.Add(new KeyValuePair<int, int>(x, j));
            status[x, j] = true;
        }
        patterns.Add(pattern);
        patternFound = true;
    }
}
void Square(int x, int y)
{
    List<KeyValuePair<int, int>> pattern = new();
    bool IsSquare = false;
    if (x+1 <= karnaughMap.GetUpperBound(0) && y+1 <= karnaughMap.GetUpperBound(1) && karnaughMap[x,y] == karnaughMap[x+1,y] && karnaughMap[x, y] == karnaughMap[x+1, y+1] && karnaughMap[x, y] == karnaughMap[x, y+1])
    {
        pattern.AddRange(new[]
        {
            new KeyValuePair<int, int>(x, y),
            new KeyValuePair<int, int>(x+1, y),
            new KeyValuePair<int, int>(x+1, y+1),
            new KeyValuePair<int, int>(x, y+1),
        });
        IsSquare = true;
    }
    else if (x-1 >= 0 && y+1 <= karnaughMap.GetUpperBound(1) && karnaughMap[x, y] == karnaughMap[x-1, y] && karnaughMap[x, y] == karnaughMap[x-1, y+1] && karnaughMap[x, y] == karnaughMap[x, y+1])
    {
        pattern.AddRange(new[]
{
            new KeyValuePair<int, int>(x, y),
            new KeyValuePair<int, int>(x-1, y),
            new KeyValuePair<int, int>(x-1, y+1),
            new KeyValuePair<int, int>(x, y+1),
        });
        IsSquare = true;
    }
    else if (x+1 <= karnaughMap.GetUpperBound(0) && y-1 >= 0 && karnaughMap[x, y] == karnaughMap[x, y-1] && karnaughMap[x, y] == karnaughMap[x+1, y-1] && karnaughMap[x, y] == karnaughMap[x+1, y])
    {
        pattern.AddRange(new[]
{
            new KeyValuePair<int, int>(x, y),
            new KeyValuePair<int, int>(x, y-1),
            new KeyValuePair<int, int>(x+1, y-1),
            new KeyValuePair<int, int>(x+1, y),
        });
        IsSquare = true;
    }
    else if (x-1 >= 0 && y-1 >= 0 && karnaughMap[x, y] == karnaughMap[x, y-1] && karnaughMap[x, y] == karnaughMap[x-1, y-1] && karnaughMap[x, y] == karnaughMap[x-1, y])
    {
        pattern.AddRange(new[]
        {
            new KeyValuePair<int, int>(x, y),
            new KeyValuePair<int, int>(x, y-1),
            new KeyValuePair<int, int>(x-1, y-1),
            new KeyValuePair<int, int>(x-1, y),
        });
        IsSquare = true;
    }
    else if (y == 0)
    {
        if(x - 1 >= 0 && karnaughMap[x - 1, y] == karnaughMap[x, y] && karnaughMap[x - 1, karnaughMap.GetUpperBound(1)] == karnaughMap[x, y] && karnaughMap[x, karnaughMap.GetUpperBound(1)] == karnaughMap[x, y])
        {
            pattern.AddRange(new[]
{
            new KeyValuePair<int, int>(x, y),
            new KeyValuePair<int, int>(x - 1, y),
            new KeyValuePair<int, int>(x, karnaughMap.GetUpperBound(1)),
            new KeyValuePair<int, int>(x - 1, karnaughMap.GetUpperBound(1)),
        });
            IsSquare = true;
        }
    }
    if (x == 0 && y == 0 || x == karnaughMap.GetUpperBound(0) && y == 0 || x == 0 && y == karnaughMap.GetUpperBound(1) || x == karnaughMap.GetUpperBound(0) && y == karnaughMap.GetUpperBound(1))
    {
        if (karnaughMap[0,0] == karnaughMap[karnaughMap.GetUpperBound(0),0] && karnaughMap[karnaughMap.GetUpperBound(0), 0] == karnaughMap[0, karnaughMap.GetUpperBound(1)] && karnaughMap[0, karnaughMap.GetUpperBound(1)] == karnaughMap[karnaughMap.GetUpperBound(0), karnaughMap.GetUpperBound(1)])
        {
            pattern.AddRange(new[]
            {
            new KeyValuePair<int, int>(0, 0),
            new KeyValuePair<int, int>(karnaughMap.GetUpperBound(0), 0),
            new KeyValuePair<int, int>(0, karnaughMap.GetUpperBound(1)),
            new KeyValuePair<int, int>(karnaughMap.GetUpperBound(0), karnaughMap.GetUpperBound(1)),
            });
            IsSquare = true;
        }
    }
    if (IsSquare == true)
    {
        foreach (KeyValuePair<int, int> pair in pattern)
        {
            status[pair.Key, pair.Value] = true;
        }
        patterns.Add(pattern);
        patternFound = true;
    }
}
void HorizontalShort(int x, int y)
{
    List<KeyValuePair<int, int>> pattern = new();
    bool IsHorizontalShort = false;
    if (y - 1 >= 0 && karnaughMap[x,y] == karnaughMap[x, y - 1])
    {
        pattern.AddRange(new[]
        {
            new KeyValuePair<int,int>(x,y),
            new KeyValuePair<int,int>(x,y - 1),
        });
        IsHorizontalShort = true;
    }
    else if (y + 1 <= karnaughMap.GetUpperBound(1) && karnaughMap[x, y] == karnaughMap[x, y + 1])
    {
        pattern.AddRange(new[]
        {
            new KeyValuePair<int,int>(x,y),
            new KeyValuePair<int,int>(x,y + 1),
        });
        IsHorizontalShort = true;
    }
    if (IsHorizontalShort == true)
    {
        foreach (KeyValuePair<int, int> pair in pattern)
        {
            status[pair.Key, pair.Value] = true;
        }
        patterns.Add(pattern);
        patternFound = true;
    }
}
void VerticalShort(int x, int y)
{
    List<KeyValuePair<int, int>> pattern = new();
    bool IsHorizontalShort = false;
    if (x - 1 >= 0 && karnaughMap[x, y] == karnaughMap[x - 1, y])
    {
        pattern.AddRange(new[]
        {
            new KeyValuePair<int,int>(x,y),
            new KeyValuePair<int,int>(x - 1,y),
        });
        IsHorizontalShort = true;
    }
    else if (x + 1 <= karnaughMap.GetUpperBound(0) && karnaughMap[x, y] == karnaughMap[x + 1, y])
    {
        pattern.AddRange(new[]
        {
            new KeyValuePair<int,int>(x,y),
            new KeyValuePair<int,int>(x + 1,y),
        });
        IsHorizontalShort = true;
    }
    if (IsHorizontalShort == true)
    {
        foreach (KeyValuePair<int, int> pair in pattern)
        {
            status[pair.Key, pair.Value] = true;
        }
        patterns.Add(pattern);
        patternFound = true;
    }
}
void OutputPatterns()
{
    Console.WriteLine("\nКонтури: ");
    foreach (List<KeyValuePair<int, int>> listKVP in patterns)
    {
        for (int i = 0; i <= karnaughMap.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= karnaughMap.GetUpperBound(1); j++)
            {
                KeyValuePair<int, int> kvp = new(i, j);
                if (listKVP.Contains(kvp) == true)
                {
                    if (karnaughMap[i,j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                }
                Console.Write(karnaughMap[i, j] + "\t");
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
string DDNF(int[,] array)
{
    string DDNF = "";
    for (int i = 0; i <= array.GetUpperBound(0); i++)
    {
        if (array[i, array.GetUpperBound(1) - 1] == 1)
        {
            for (int j = 0; j < array.GetUpperBound(1); j++)
            {
                if (array[i, j] == 1)
                {
                    DDNF += "X";
                }
                else
                {
                    DDNF += "!X";
                }
                DDNF += j + 1;
            }
            DDNF += " + ";
        }
    }
    return DDNF.Substring(0, DDNF.Length - 2);
}
string DKNF(int[,] array)
{
    string DKNF = "";
    for (int i = 0; i <= array.GetUpperBound(0); i++)
    {
        if (array[i, array.GetUpperBound(1) - 1] == 0)
        {
            DKNF += "(";
            for (int j = 0; j < array.GetUpperBound(1); j++)
            {
                if (array[i, j] == 0)
                {
                    DKNF += "X";
                }
                else
                {
                    DKNF += "!X";
                }
                DKNF += j + 1 ;
                if (j != array.GetUpperBound(1) - 1)
                {
                    DKNF += "+";
                }
            }
            DKNF += ") * ";
        }
    }
    return DKNF.Substring(0, DKNF.Length - 2);
}
int[,] KarnaughMap(int[,] array)
{
    int[,] karnaughMap = new int[(array.GetUpperBound(0) + 1) / 4, array.GetUpperBound(1)];
    for (int i = 0; i <= karnaughMap.GetUpperBound(0); i++)
    {
        for (int j = 0; j <= karnaughMap.GetUpperBound(1); j++)
        {
            karnaughMap[i, j] = KarnaughMapFindValue(array, i, j);
        }
    }
    return karnaughMap;
}
int KarnaughMapFindValue(int[,] array, int x, int y)
{
    int value = -1;
    string[] headers = new string[] { "00", "01", "11", "10" };
    int[] numbers = new int[] { headers[x][0] - '0', headers[x][1] - '0', headers[y][0] - '0', headers[y][1] - '0' };
    for (int i = 0; i <= array.GetUpperBound(0); i++)
    {
        if (array[i,0] == numbers[0] && array[i, 1] == numbers[1] && array[i, 2] == numbers[2] && array[i, 3] == numbers[3])
        {
            value = array[i, 4];
        }
    }
    return value;
}