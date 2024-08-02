using Elements_Permutation;
using System.Xml.Linq;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Кількість рядків і стовпців для розміщення елементів
int rows = 5, cols = 4;

// Ініціалізація елементів з вказуванням їх статусу (чи закріплений)
List<Element> elements = new()
{
    new Element(1),
    new Element(2, true),
    new Element(3),
    new Element(4, true),
    new Element(5),
    new Element(6),
    new Element(7),
    new Element(8, true),
    new Element(9),
    new Element(10),
    new Element(11),
    new Element(12)
};

// Ініціалізація зв'язків між елементами
Connections.AddConnection(elements[0].ID, elements[4].ID, 1); // 1-5 1
Connections.AddConnection(elements[0].ID, elements[7].ID, 1); // 1-8 1
Connections.AddConnection(elements[0].ID, elements[11].ID, 1); // 1-12 1

Connections.AddConnection(elements[1].ID, elements[2].ID, 1); // 2-3 1
Connections.AddConnection(elements[1].ID, elements[10].ID, 1); // 2-11 1

Connections.AddConnection(elements[2].ID, elements[3].ID, 2); // 3-4 2
Connections.AddConnection(elements[2].ID, elements[4].ID, 1); // 3-5 1

Connections.AddConnection(elements[3].ID, elements[9].ID, 1); // 4-10 1

Connections.AddConnection(elements[4].ID, elements[8].ID, 1); // 5-9 1

Connections.AddConnection(elements[5].ID, elements[7].ID, 1); // 6-8 1
Connections.AddConnection(elements[5].ID, elements[8].ID, 2); // 6-9 2

Connections.AddConnection(elements[6].ID, elements[8].ID, 1); // 7-9 1
Connections.AddConnection(elements[6].ID, elements[11].ID, 1); // 7-12 1

Connections.AddConnection(elements[9].ID, elements[10].ID, 3); // 10-11 3

Connections.AddConnection(elements[10].ID, elements[11].ID, 1); // 11-12 1

// Ініціалізація матриці (плати) для розміщення елементів
int[,] board = new int[rows, cols];
board[1, 1] = elements[1].ID;
board[2, 3] = elements[7].ID;
board[4, 1] = elements[3].ID;
Output.OutputArray(board);

bool allIsPlaced = false;
int countIteration = 0;

while (allIsPlaced == false) {
    // Рахуємо кількість зв'язків з незакріпленими і закріпленими елементами
    foreach (Element element in elements)
    {
        if (element.pinned == false) // Нам не потрібно рахувати кількість зв'язків у вже закріплених елементів
        {
            Dictionary<KeyValuePair<int, int>, int> connections = Connections.FindAllConnections(element.ID);

            foreach (KeyValuePair<int, int> kvp in connections.Keys)
            {
                if (!elements[kvp.Key - 1].pinned && !elements[kvp.Value - 1].pinned)
                {
                    element.numConnections += connections.GetValueOrDefault(kvp);
                }
                else
                {
                    element.numPinned += connections.GetValueOrDefault(kvp);
                }
            }
        }
    }

    // Шукаємо елемент для розміщення
    Element elementPlace = new(elements.Count);

    foreach (Element element in elements)
    {
        if (element.pinned == false)
        {
            if (element.numPinned > elementPlace.numPinned) // Знаходимо елемент з найбільшою кількістю зв'язків із закріпленими елементами
            {
                elementPlace = element;
            }
            else if (element.numPinned == elementPlace.numPinned) // Якщо у елементів одинакова кількість зв'язків із закріпленими елементами
            {
                if (element.numConnections < elementPlace.numConnections) // Знаходимо елемент з найменьшою кількістю зв'язків із незакріпленими елементами
                {
                    elementPlace = element;
                }
                else if (element.numConnections == elementPlace.numConnections) // Якщо у елементів одинакова кількість зв'язків із незакріпленими елементами
                {
                    if (element.ID < elementPlace.ID) // Обримаємо елемент з найменьшим номером
                    {
                        elementPlace = element;
                    }
                }
            }
        }
    }

    // Шукаємо для елемента, який будемо розміщувати, закріплений елемент з яким в нього найбільше зв'язків
    Dictionary<KeyValuePair<int, int>, int> elementPlaceConnections = Connections.FindAllConnections(elementPlace.ID);
    int numPinned = 0, pinnedID = 0;
    foreach (KeyValuePair<int, int> kvp in elementPlaceConnections.Keys)
    {
        if (elements[kvp.Key - 1].pinned == true || elements[kvp.Value - 1].pinned == true)
        {
            if (elementPlaceConnections.GetValueOrDefault(kvp) > numPinned)
            {
                numPinned = elementPlaceConnections.GetValueOrDefault(kvp);
                if (elements[kvp.Key - 1].ID == elementPlace.ID)
                {
                    pinnedID = kvp.Value;
                }
                else
                {
                    pinnedID = kvp.Key;
                }
            }
        }
    }
    int pinnedX = 0, pinnedY = 0;

    // Шукаємо номер стовпця і рядка цього закріпленого елементу
    for (int i = 0; i < board.GetUpperBound(0); i++)
    {
        for (int j = 0; j < board.GetUpperBound(1); j++)
        {
            if (board[i, j] == pinnedID)
            {
                pinnedX = i;
                pinnedY = j;
            }
        }
    }

    // Шукаємо позицію для елемента, який будемо розміщувати, відносно позиції розміщеного елементу, з яким в нього найбільше зв'язків
    int maxIndex = board.GetUpperBound(0);
    int minIndex = 0;

    for (int i = 1; i < maxIndex; i++)
    {
        if (TryPlaceElement(board, elementPlace.ID, pinnedX - i, pinnedY, minIndex))
            break;
        if (TryPlaceElement(board, elementPlace.ID, pinnedX, pinnedY - i, minIndex))
            break;
        if (TryPlaceElement(board, elementPlace.ID, pinnedX + i, pinnedY, cols))
            break;
        if (TryPlaceElement(board, elementPlace.ID, pinnedX, pinnedY + i, rows))
            break;
    }  

    Console.WriteLine("".PadLeft(7) + "Ітерація №" + ++countIteration);
    Output.OutputArray(board);
    elements[elementPlace.ID - 1].pinned = true;

    // Перевіряємо чи розмістили ми всі елементи
    foreach (Element element in elements)
    {
        if (element.pinned == false)
        {
            allIsPlaced = false;
            break;
        }
        allIsPlaced = true;
    }
}

// Виводимо оптимізоване розміщення елементів і його К
Console.WriteLine("\nОптимізоване розміщення елементів:");
Output.OutputArray(board);
double boardK = Connections.FindK(board);
Console.WriteLine("Kкін = " + boardK);

// Створюємо початку матрицю розмістивши елементи рандомно
int[,] board2 = new int[rows, cols];
board2[1, 1] = elements[1].ID;
board2[2, 3] = elements[7].ID;
board2[4, 1] = elements[3].ID;
elements[1].pinned = true;
elements[3].pinned = true;
elements[3].pinned = true;

for (int i = 0; i < elements.Count; i++)
{
    bool find = false;
    for (int j = 0; j < rows; j++)
    {
        for (int k = 0; k < cols; k++)
        {
            if (board2[j,k] == elements[i].ID)
            {
                find = true;
                break;
            }
        }
    }
    if (!find)
    {
        Random random = new();
        int x, y;

        do
        {
            x = random.Next(rows);
            y = random.Next(cols);
        } while (board2[x, y] != 0);

        board2[x, y] = elements[i].ID;
    }
}

// Виводимо початкове розміщення елементів і його К
Console.WriteLine("\nПочаткове розміщення елементів:");
Output.OutputArray(board2);
double board2K = Connections.FindK(board2);
Console.WriteLine("Kпоч = " + board2K);

// Порінюємо значення критеріїв
double E = 0;
if (boardK > board2K)
{
    E = ((boardK - board2K) / boardK) * 100;
}
else if (boardK < board2K)
{
    E = ((board2K - boardK) / board2K) * 100;
}
else
{
    E = 0;
}
Console.WriteLine("\nE = {0:0.00}%", E);

static bool TryPlaceElement(int[,] board, int elementID, int x, int y, int boundary)
{
    if (x >= 0 && x < boundary && y >= 0 && y < boundary && board[x, y] == 0)
    {
        board[x, y] = elementID;
        return true;
    }
    return false;
}