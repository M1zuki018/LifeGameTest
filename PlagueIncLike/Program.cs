/// <summary>
/// ライフゲームのシミュレーションを派生させてみた
/// ①セルの状態は「健康 = " "」「感染 = "I"」とする
/// ②感染条件の定義
///     A.感染確率は5%
///     B.感染を起こす範囲は、隣接する8マスに限定される
///     C.2マス以上隣接するマスがあった場合は、感染確率は1%ずつ上昇する（1マスなら5％、2マスなら6%、3マスなら7%...）
///     D.ランダムで0.1％の確率で、感染状態のマスが健康状態になる（現在コメントアウトしています）
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
            
            // 全てのマスが感染しているか確認
            bool allInfected = true;
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    if (!nextGrid[x, y])
                    {
                        allInfected = false;
                        break;
                    }
                }
                if (!allInfected) break;
            }

            if (allInfected)
            {
                Console.Clear();
                PrintGrid(nextGrid); //最後の描画を行う
                Console.WriteLine("全人類が感染しました。");
                break;
            }
            
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
            grid[row, col] = true; //感染状態にする
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
        Random rand = new Random();
        int rows = grid.GetLength(0); //グリッドの行の配列の長さを取得
        int cols = grid.GetLength(1); //グリッドの列の配列の長さを取得

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j]) //判定中のセルが感染状態なら
                {
                    //0.1%の確率で、感染状態から健康状態になる
                    if (rand.Next(1000) < 1)
                    {
                        //nextGrid[i, j] = false; //健康状態に戻る
                    }
                    else
                    {
                        grid[i, j] = true; //感染状態を維持
                    }
                }
                else 
                {
                    // 健康状態の場合、感染の可能性を計算
                    int infectedNeighbors = CountInfectedNeighbors(grid, i, j, rows, cols);
                    int infectionChance = 5 + (infectedNeighbors - 1) * 1; // 感染確率を計算

                    // 感染するかどうかを判定
                    if (rand.Next(100) < infectionChance)
                    {
                        nextGrid[i, j] = true; // 感染状態にする
                    }
                    else
                    {
                        nextGrid[i, j] = false; // 健康状態を維持
                    }
                }
            }
        }

        // グリッドを次世代に更新
        // 次世代の配列nextGridを、現在の配列gridにコピーして、次のループへ進む
        Array.Copy(nextGrid, grid, rows * cols);
    }
    
    /// <summary>
    /// 隣接する感染状態（true）のセルを数える関数
    /// </summary>
    /// <param name="x">自分のセルの行数</param>
    /// <param name="y">自分のセルの列数</param>
    static int CountInfectedNeighbors(bool[,] grid, int x, int y, int rows, int cols)
    {
        int count = 0;
        
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // 自分自身をスキップ

                int ni = x + i;
                int nj = y + j;

                // 隣接セルがグリッド範囲内かつ感染状態かを確認
                if (ni >= 0 && ni < rows && nj >= 0 && nj < cols && grid[ni, nj])
                {
                    count++;
                }
            }
        }

        return count; // 感染している隣接セルの数を返す
    }
}
