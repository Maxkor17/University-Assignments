Console.OutputEncoding = System.Text.Encoding.UTF8;
List<Element> elements = new()
{
    new ('H', 7, 5, -1), // 0 H
    new ('C', 4, 1, -2), // 1 C
    new ('B', 2, 6, -3), // 2 B
    new ('F', 5, 7, -4), // 3 F
    new ('E', 8, 2, -5), // 4 E
    new ('D', 9, 7, -6), // 5 D
    new ('A', 1, 1, -7), // 6 A
};

Connections.connections = new List<(Element, Element, bool)>
{
    (elements[4], elements[5], false), // E-D 1
    (elements[4], elements[5], false), // E-D 2
    (elements[2], elements[6], false), // A-B 1
    (elements[2], elements[6], false), // A-B 2
    (elements[0], elements[3], false), // F-H 1
    (elements[0], elements[3], false), // F-H 2
    (elements[2], elements[5], false), // D-B 1
    (elements[2], elements[5], false), // D-B 2
    (elements[0], elements[6], false), // H-A
    (elements[1], elements[3], false), // F-C
    (elements[1], elements[4], false), // E-C  
    (elements[3], elements[4], false), // E-F  
    (elements[1], elements[6], false), // C-A 
};


List<(int, ConsoleColor)> colors = new()
{
    (-8, ConsoleColor.Green),
    (-9, ConsoleColor.Yellow),
    (-10, ConsoleColor.Red),
    (-11, ConsoleColor.Blue),
    (-12, ConsoleColor.Cyan),
    (-13, ConsoleColor.DarkBlue),
    (-14, ConsoleColor.DarkCyan),
    (-15, ConsoleColor.Magenta),
    (-16, ConsoleColor.DarkGreen),
    (-17, ConsoleColor.DarkMagenta),
    (-18, ConsoleColor.DarkRed),
    (-19, ConsoleColor.DarkYellow),
    (-20, ConsoleColor.Blue),
    (-21, ConsoleColor.Magenta)
};

int num = (elements.Count * -1) - 1;
int k = 0;
List<int[,]> boards = new();

while (Connections.connections.Any(tuple => !tuple.Item3))
{
    int[,] array = new int[Constants.rows, Constants.columns];

    foreach (Element element in elements)
    {
        array[element.x, element.y] = element.ID;
    }

    boards.Add(array);
    for (int i = 0; i < Connections.connections.Count; i++)
    {
        if (Connections.connections[i].Item3 == false)
        {
            bool pathFound = Algorithms.LeeTracingAlgorithm(boards[k], Connections.connections[i].Item1.x, Connections.connections[i].Item1.y, Connections.connections[i].Item2.x, Connections.connections[i].Item2.y, num);

            if (pathFound == true)
            {
                Connections.connections[i] = (Connections.connections[i].Item1, Connections.connections[i].Item2, true);
                --num;
            }
        }
    }
    k++;
}

foreach (var board in boards)
{
    OutputArrayColors(board, elements, colors);
    Console.WriteLine();
}

Console.WriteLine("Lee K: " + Algorithms.GetLeeK());

for (int i = 0; i < Connections.connections.Count; i++)
{
    Connections.connections[i] = (Connections.connections[i].Item1, Connections.connections[i].Item2, false);
}

num = (elements.Count * -1) - 1;
k = 0;
boards.Clear();

while (Connections.connections.Any(tuple => !tuple.Item3))
{
    int[,] array = new int[Constants.rows, Constants.columns];

    foreach (Element element in elements)
    {
        array[element.x, element.y] = element.ID;
    }

    boards.Add(array);
    for (int i = 0; i < Connections.connections.Count; i++)
    {
        if (Connections.connections[i].Item3 == false)
        {
            bool pathFound = Algorithms.RayTracingAlgorithm(boards[k], Connections.connections[i].Item1.x, Connections.connections[i].Item1.y, Connections.connections[i].Item2.x, Connections.connections[i].Item2.y, num, Connections.connections[i].Item2.ID);

            if (pathFound == true)
            {
                Connections.connections[i] = (Connections.connections[i].Item1, Connections.connections[i].Item2, true);
                --num;
            }
        }
    }
    k++;
}

foreach (var board in boards)
{
    OutputArrayColors(board, elements, colors);
    Console.WriteLine();
}

Console.WriteLine("Ray K: " + Algorithms.GetRayK());

Console.WriteLine();
Console.WriteLine("E = " + Math.Round(Algorithms.DifferenceE(),2) + "%");
static void OutputArrayColors(int[,] array, List<Element> elements, List<(int, ConsoleColor)> colors)
{
    for (int i = 0; i < Constants.rows; i++)
    {
        for (int j = 0; j < Constants.columns; j++)
        {
            if (array[i, j] < 0 && array[i, j] > -8)
            {
                foreach (Element element in elements)
                {
                    if (element.ID == array[i, j])
                    {
                        Console.Write(element.name + " ");
                    }
                }
            }
            else if (array[i, j] <= -8)
            {
                foreach ((int, ConsoleColor) color in colors)
                {
                    if (array[i, j] == color.Item1)
                    {
                        Console.ForegroundColor = color.Item2;
                        Console.Write("•" + " ");
                    }
                }
            }
            else if (array[i, j] == 0)
            {
                Console.Write("·" + " ");
            }
            Console.ResetColor();
        }
        Console.WriteLine();
    }
}