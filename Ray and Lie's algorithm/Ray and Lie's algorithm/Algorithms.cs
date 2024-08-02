static class Algorithms
{
    static int kLee = 0;
    static int kRay = 0;
    public static bool LeeTracingAlgorithm(int[,] matrix, int startX, int startY, int endX, int endY, int num)
    {
        int x = startX;
        int y = startY;
        bool pathFound = false;

        for (int i = 0; i < Constants.rows; i++)
        {
            for (int j = 0; j < Constants.columns; j++)
            {
                if (matrix[i, j] == 0)
                {
                    matrix[i, j] = Math.Abs(i - x) + Math.Abs(j - y);
                }
            }
        }

        List<(int, int)> shortestPath = FindShortestPath(matrix, startX, startY, endX, endY);
        if (shortestPath.Count > 0)
        {
            shortestPath.RemoveAt(0);
            shortestPath.RemoveAt(shortestPath.Count - 1);
            kLee += shortestPath.Count;
            pathFound = true;

            for (int i = 0; i < Constants.rows; i++)
            {
                for (int j = 0; j < Constants.columns; j++)
                {
                    if (shortestPath.Exists(p => p.Item1 == i && p.Item2 == j))
                    {
                        matrix[i, j] = num;
                    }
                }
            }
        }

        for (int i = 0; i < Constants.rows; i++)
        {
            for (int j = 0; j < Constants.columns; j++)
            {
                if (matrix[i, j] > 0)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        return pathFound;
    }
    static List<(int, int)> FindShortestPath(int[,] matrix, int startRow, int startCol, int endRow, int endCol)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        bool[,] visited = new bool[rows, cols];
        int[,] distances = new int[rows, cols];
        int[,] parentsRow = new int[rows, cols];
        int[,] parentsCol = new int[rows, cols];

        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((startRow, startCol));
        visited[startRow, startCol] = true;
        distances[startRow, startCol] = 0;

        int[] dRow = { -1, 0, 1, 0 };
        int[] dCol = { 0, 1, 0, -1 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            int currentRow = current.Item1;
            int currentCol = current.Item2;

            if (currentRow == endRow && currentCol == endCol)
            {
                break;
            }

            for (int i = 0; i < 4; i++)
            {
                int newRow = currentRow + dRow[i];
                int newCol = currentCol + dCol[i];

                if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols &&
                    (matrix[newRow, newCol] > 0 || (newRow == endRow && newCol == endCol)) && !visited[newRow, newCol])
                {
                    queue.Enqueue((newRow, newCol));
                    visited[newRow, newCol] = true;
                    distances[newRow, newCol] = distances[currentRow, currentCol] + 1;
                    parentsRow[newRow, newCol] = currentRow;
                    parentsCol[newRow, newCol] = currentCol;

                    if (newRow == endRow && newCol == endCol)
                    {
                        break;
                    }
                }
            }

        }

        if (distances[endRow, endCol] == 0)
        {
            return new List<(int, int)>();
        }

        List<(int, int)> shortestPath = new List<(int, int)>();
        int row = endRow;
        int col = endCol;

        while (row != startRow || col != startCol)
        {
            shortestPath.Add((row, col));
            int tempRow = parentsRow[row, col];
            int tempCol = parentsCol[row, col];
            row = tempRow;
            col = tempCol;
        }

        shortestPath.Add((startRow, startCol));
        shortestPath.Reverse();

        return shortestPath;
    }
    public static bool RayTracingAlgorithm(int[,] grid, int startX, int startY, int endX, int endY, int num, int ID)
    {
        int currentX = startX;
        int currentY = startY;

        int moveX = (endX > startX) ? 1 : -1;
        int moveY = (endY > startY) ? 1 : -1;

        bool startDirection = true, direction = startDirection;

        int gridUpperBoundX = grid.GetUpperBound(0);
        int gridUpperBoundY = grid.GetUpperBound(1);

        while (true)
        {
            bool canMoveFirstDirection = (currentX > 0 && currentX <= gridUpperBoundX && currentX != endX)
                                      && (grid[currentX + moveX, currentY] == 0 || grid[currentX + moveX, currentY] == ID);

            bool canMoveSecondDirection = (currentY > 0 && currentY <= gridUpperBoundY && currentY != endY)
                                       && (grid[currentX, currentY + moveY] == 0 || grid[currentX, currentY + moveY] == ID);

            if (!canMoveFirstDirection && !canMoveSecondDirection)
            {
                if (currentX == endX && currentY == endY)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i <= gridUpperBoundX; i++)
                    {
                        for (int j = 0; j <= gridUpperBoundY; j++)
                        {
                            if (grid[i, j] == num)
                            {
                                grid[i, j] = 0;
                            }
                        }
                    }
                    if (startDirection)
                    {
                        startDirection = !startDirection;
                        direction = startDirection;

                        currentX = startX;
                        currentY = startY;
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (direction)
            {
                if (canMoveFirstDirection)
                {
                    currentX += moveX;
                }
                else
                {
                    direction = false;
                }
            }
            else
            {
                if (canMoveSecondDirection)
                {
                    currentY += moveY;
                }
                else
                {
                    direction = true;
                }
            }

            if ((currentX != startX || currentY != startY) && (currentX != endX || currentY != endY))
            {
                grid[currentX, currentY] = num;
            }
        }


        for (int i = 0; i <= gridUpperBoundX; i++)
        {
            for (int j = 0; j <= gridUpperBoundY; j++)
            {
                if (grid[i, j] == num)
                {
                    kRay++;
                }
            }
        }
        return true;
    }

    public static int GetLeeK()
    {
        return kLee;
    }
    public static int GetRayK()
    {
        return kRay;
    }
    public static decimal DifferenceE()
    {
        return ((Math.Max(kLee, kRay) - Math.Min(kRay, kLee)) / (decimal)Math.Max(kLee, kRay)) * 100;
    }
}