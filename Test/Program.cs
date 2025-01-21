using System;
using System.Threading;

class LifeGame
{
    static void Main()
    {
        //グリッドを作成する
        const int rows = 20; // グリッドの行数
        const int cols = 40; // グリッドの列数
        bool[,] grid = new bool[rows, cols];
        bool[,] nextGrid = new bool[rows, cols];

        InitializeGrid(grid); // 初期化

        while (true)
        {
            Console.Clear();
            PrintGrid(grid); // 現在のグリッドを表示
            UpdateGrid(grid, nextGrid); // 次世代を計算
            Thread.Sleep(200); // 表示間隔
        }
    }

    // グリッドをランダムに初期化
    static void InitializeGrid(bool[,] grid)
    {
        Random rand = new Random();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = rand.Next(2) == 0; // 50%の確率で生
            }
        }
    }

    // グリッドを表示
    static void PrintGrid(bool[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j] ? "O" : " "); // 生は "O"、死は " "
            }
            Console.WriteLine();
        }
    }

    // 次世代のグリッドを計算
    static void UpdateGrid(bool[,] grid, bool[,] nextGrid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int aliveNeighbors = CountAliveNeighbors(grid, i, j);

                if (grid[i, j])
                {
                    // 生存しているセルのルール
                    nextGrid[i, j] = aliveNeighbors == 2 || aliveNeighbors == 3;
                }
                else
                {
                    // 死んでいるセルのルール
                    nextGrid[i, j] = aliveNeighbors == 3;
                }
            }
        }

        // グリッドを次世代に更新
        Array.Copy(nextGrid, grid, rows * cols);
    }

    // 生存している隣接セルを数える
    static int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
        int count = 0;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // 自分自身は無視
                int ni = x + i;
                int nj = y + j;

                // グリッドの範囲を超えないように
                if (ni >= 0 && ni < rows && nj >= 0 && nj < cols && grid[ni, nj])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
