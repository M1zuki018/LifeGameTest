/// <summary>
/// ライフゲームのシミュレーションを派生させてみた
/// ①セルの状態は「健康 = " "」「感染 = "I"」とする
/// ②感染条件、死亡条件の定義を仕様に合わせて変更する
/// </summary>
class PlagueIncLike
{
    /// <summary>
    /// 根幹の処理
    /// </summary>
    static void Main()
    {
        //グリッドを作成する。グリッドは二次元配列を作成する
        const int rows = 20; // 行数
        const int cols = 40; // 列数
        bool[,] grid = new bool[rows, cols];
        bool[,] nextGrid = new bool[rows, cols];

        InitializeGrid(grid); // 初期化

        while (true)
        {
            Console.Clear(); // コンソールウィンドウをクリアする
            PrintGrid(grid); // 現在のグリッドを表示
            UpdateGrid(grid, nextGrid); // 次世代を計算
            Thread.Sleep(200); // 表示間隔
        }
    }
    
    /// <summary>
    /// 入力に合わせて最初の感染マスを反映する
    /// </summary>
    static void InitializeGrid(bool[,] grid)
    {
        int input = int.Parse(Console.ReadLine()); //入力される行の数を取得する
        for (int i = 0; i < input; i++)
        {
            int[] line = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
            int row = line[0]; //行数の指定
            int col = line[1]; //列数の指定
            grid[row, col] = true; //感染状態にす
        }
    }

    /// <summary>
    /// グリッドを表示する
    /// </summary>
    static void PrintGrid(bool[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j] ? "I" : " ");
            }
            Console.WriteLine(); //1行分終わったら改行する
        }
    }

    /// <summary>
    /// 次世代のグリッドを計算
    /// </summary>
    static void UpdateGrid(bool[,] grid, bool[,] nextGrid)
    {
        int rows = grid.GetLength(0); //グリッドの行の配列の長さを取得
        int cols = grid.GetLength(1); //グリッドの列の配列の長さを取得

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int aliveNeighbors = CountAliveNeighbors(grid, i, j, rows, cols); //生存している隣接セルを数える

                if (grid[i, j]) //判定中のセルが「生」状態なら
                {
                    // 隣接する「生」のマスが2個か3個ならtrue＝「生」状態、それ以外ならfalse＝「死」状態とする
                    nextGrid[i, j] = aliveNeighbors == 2 || aliveNeighbors == 3;
                }
                else // 判定中のセルが「死」状態なら
                {
                    //  隣接する「生」のマスが3個ならtrue＝「生」状態、それ以外ならfalse＝「死」状態とする
                    nextGrid[i, j] = aliveNeighbors == 3;
                }
            }
        }

        // グリッドを次世代に更新
        // 次世代の配列nextGridを、現在の配列gridにコピーして、次のループへ進む
        Array.Copy(nextGrid, grid, rows * cols);
    }
    
    /// <summary>
    /// 生存している隣接セルを数える
    /// </summary>
    /// <param name="x">自分のセルの行数</param>
    /// <param name="y">自分のセルの列数</param>
    static int CountAliveNeighbors(bool[,] grid, int x, int y, int rows, int cols)
    {
        int count = 0;

        // 近傍のセルをチェックするために、-1から+1までの範囲を走査
        for (int i = -1; i <= 1; i++) //上下方向
        {
            for (int j = -1; j <= 1; j++) //左右方向
            {
                if (i == 0 && j == 0) continue; // (0, 0)の場合は自分自身を指すためスキップ
                
                int ni = x + i;
                int nj = y + j;

                // 確認①チェック対象のセルがグリッドの範囲外でないか
                // 「ni >= 0」 → 行インデックスが0以上
                // 「ni < rows」 → 行インデックスが行数未満
                // 「nj >= 0」 → 列インデックスが0以上
                // 「nj < cols」 → 列インデックスが列数未満
                // 確認②grid[ni, nj] → 対象のセルが 「生」状態かどうか
                if (ni >= 0 && ni < rows && nj >= 0 && nj < cols && grid[ni, nj])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
