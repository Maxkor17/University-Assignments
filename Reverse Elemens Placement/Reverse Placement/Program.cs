Console.OutputEncoding = System.Text.Encoding.UTF8;
CustomLayout layout = new();

int[] elementOrder = { 2, 1, 4, 3, 5}; // Порядок розташування елементів
int[] ways = { 3, 2, 1, 1 }; // Шлях між ними

Dictionary<KeyValuePair<int, int>, int> connections = new() // Зв'язки між елементами
{
    {new KeyValuePair<int,int>(1,2), 1},
    {new KeyValuePair<int,int>(1,3), 2},
    {new KeyValuePair<int,int>(3,5), 1},
    {new KeyValuePair<int,int>(4,5), 1},
};

layout.SetElements(elementOrder);
layout.SetWays(ways);
layout.SetConnections(connections);

layout.BuildDistanceMatrix();
layout.BuildDistanceMatrixColumn();
layout.BuildConnectionMatrix();
layout.BuildConnectionMatrixColumn();

layout.DisplayAll();
class CustomLayout
{
    private int[] _elements;
    private int[] _ways;
    private Dictionary<KeyValuePair<int, int>, int> _connections;
    private int[,] _distanceMatrix;
    private int[] _distanceMatrixColumn;
    private int[,] _connectionMatrix;
    private int[] _connectionsMatrixColumn;

    #region Setters
    public void SetElements(int[] elements) => _elements = elements;
    public void SetWays(int[] ways) => _ways = ways;
    public void SetConnections(Dictionary<KeyValuePair<int, int>, int> connections) => _connections = connections;
    #endregion

    #region DistanceMatrix
    public void BuildDistanceMatrix()
    {
        _distanceMatrix = new int[_elements.Length, _elements.Length];

        for (int i = 0; i < _distanceMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _distanceMatrix.GetLength(1); j++)
            {
                _distanceMatrix[i, j] = (i == j) ? 0 : CalculateDistance(i + 1, j + 1);
            }
        }
    }

    private int CalculateDistance(int element1, int element2)
    {
        if (element1 == element2) return 0;

        int index1 = Array.IndexOf(_elements, element1);
        int index2 = Array.IndexOf(_elements, element2);

        if (Math.Min(index1, index2) != index1)
        {
            (index1, index2) = (index2, index1);
        }

        int distance = _ways.Skip(index1).Take(index2 - index1).Sum();
        return distance;
    }

    public void BuildDistanceMatrixColumn()
    {
        _distanceMatrixColumn = Enumerable.Range(0, _distanceMatrix.GetLength(0))
            .Select(i => Enumerable.Range(0, _distanceMatrix.GetLength(1)).Sum(j => _distanceMatrix[i, j]))
            .ToArray();
    }
    #endregion

    #region ConnectionMatrix
    public void BuildConnectionMatrix()
    {
        _connectionMatrix = new int[_elements.Length, _elements.Length];

        for (int i = 0; i < _connectionMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _connectionMatrix.GetLength(1); j++)
            {
                if (_connections.TryGetValue(new KeyValuePair<int, int>(i + 1, j + 1), out int value))
                {
                    _connectionMatrix[i, j] = value;
                }
                else if (_connections.TryGetValue(new KeyValuePair<int, int>(j + 1, i + 1), out value))
                {
                    _connectionMatrix[i, j] = value;
                }
            }
        }
    }

    public void BuildConnectionMatrixColumn()
    {
        _connectionsMatrixColumn = Enumerable.Range(0, _connectionMatrix.GetLength(0))
            .Select(i => Enumerable.Range(0, _connectionMatrix.GetLength(1)).Sum(j => _connectionMatrix[i, j]))
            .ToArray();
    }
    #endregion
    public void ReverseArrangement()
    {
        for (int i = 0; i < _elements.Length; i++)
        {
            int mostLoaded = Array.IndexOf(_connectionsMatrixColumn, _connectionsMatrixColumn.Max());
            _connectionsMatrixColumn[mostLoaded] = 0;

            int indexMostCloser = Array.IndexOf(_distanceMatrixColumn, _distanceMatrixColumn.Min());
            _distanceMatrixColumn[indexMostCloser] = int.MaxValue;

            _elements[indexMostCloser] = mostLoaded + 1;
        }
    }
    public int EvaluateQuality() => _connections.Sum(kvp => kvp.Value * CalculateDistance(kvp.Key.Key, kvp.Key.Value));

    #region Output
    public void DisplayAll()
    {
        Console.WriteLine("Матриця відстаней:");
        DisplayArray(_distanceMatrix);
        Console.WriteLine("Вектор-стовпець матриці відстаней:");
        DisplayArray(_distanceMatrixColumn);
        Console.WriteLine("Матриця зв'язків:");
        DisplayArray(_connectionMatrix);
        Console.WriteLine("Вектор-стовпець матриці зв'язків:");
        DisplayArray(_connectionsMatrixColumn);

        int startQuality = EvaluateQuality();
        ReverseArrangement();

        Console.WriteLine("Кінцевий результат розміщення: ");
        Console.WriteLine(string.Join(" ", _elements));

        int finishQuality = EvaluateQuality();
        double efficiency = (startQuality - finishQuality) / (double)startQuality * 100;

        Console.WriteLine($"Критерій якості початкового розміщення: {startQuality}");
        Console.WriteLine($"Критерій якості кінцевого розміщення: {finishQuality}");
        Console.WriteLine($"Ефективність алгоритму: {efficiency:0}%");
    }

    private void DisplayArray(int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Console.Write(array[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    private void DisplayArray(int[] array)
    {
        foreach (var item in array)
        {
            Console.WriteLine(item);
        }
    }
    #endregion
}