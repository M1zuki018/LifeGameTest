/// <summary>
/// ライフゲームのシミュレーションを作ってみた記録
/// そもそもライフゲ－ムとは -> 人工知能や生物学の研究過程から生まれた細菌増殖や人口増加にヒントを得たシミュレ－ション。
/// 要件①「セル」2次元平面に展開される。区切られた部分を「セル」と呼ぶ
/// 要件②「セルには"死"と"生"の2つの状態が存在し、それを表示する」今回"生"の場合、セルに「0」を表示する。
///        （グリッドの外側＝外周部分は、「死」状態として扱う）
/// 要件③「セルの状態変化の条件」
///     A.隣接する生きたセルが3個だったら「生」（「誕生」）
///     B.2個だったら「変化しない」（「維持」）
///     C.それ以外なら「死」（「過疎」「過密」）
/// 要件④「現在のグリッドの状態を元に次世代の状態を更新する」途中で元のグリッドの状態が変わらないように、配列は2つ用意する必要がある
/// </summary>
class LifeGame
{
    /// <summary>
    /// ライフゲームの根幹の処理
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
            Console.Clear(); // コンソールウィンドウをクリアしておく
            PrintGrid(grid); // 現在のグリッドを表示
            UpdateGrid(grid, nextGrid); // 次世代を計算
            Thread.Sleep(200); // 表示間隔
        }
    }
    
    /// <summary>
    /// グリッドをランダムに初期化する
    /// </summary>
    /// <param name="grid">作成したグリッド</param>
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

    /// <summary>
    /// グリッドを表示する
    /// </summary>
    static void PrintGrid(bool[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Console.Write(grid[i, j] ? "O" : " "); // true＝生なら「O」を，false＝死なら「 」を出力
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
