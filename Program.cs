using System;
using System.Collections;

namespace Game2048 {
  class Program {
    static Random random = new Random();

    static int maxNumber = 0;
    static int opCount = 0;

    static bool isFull = false;

    static void Main(string[] args) {
      int[, ] matrix = new int[4, 4];

      // init data
      matrix[0, 0] = 2;
      GenerateRandomNumber(matrix);

      Console.WriteLine("请使用上下左右键进行游戏: ");
      PrintMatrix(matrix);

      do {
        ConsoleKeyInfo info = Console.ReadKey();
        string key = info.Key.ToString();

        switch (key) {
          case "UpArrow":
            DoUp(matrix);
            break;
          case "DownArrow":
            DoDown(matrix);
            break;
          case "LeftArrow":
            DoLeft(matrix);
            break;
          case "RightArrow":
            DoRight(matrix);
            break;
        }

        // generate random number for every step
        GenerateRandomNumber(matrix);

        // print the matrix
        PrintMatrix(matrix);

        // count the op times
        opCount++;

        // check failed
        if (isFull && CheckFail(matrix)) {
          Console.WriteLine("很遗憾，你输了，一共进行了{0}次操作。", opCount);
          break;
        }

        if (maxNumber == 1024) {
          Console.WriteLine("恭喜你，你赢了，一共进行了{0}次操作。", opCount);
          break;
        }
      } while (true);
    }

    private static void DoLeft(int[, ] matrix) {
      int rowLen = matrix.GetLength(0);
      for (int i = 0; i < rowLen; i++) {
        // get the row
        int[] row = GetRowByIndex(matrix, i);

        // do the left action
        NumberMerge(row);

        // set the row
        SetRowByIndex(matrix, i, row);
      }
    }

    private static void DoRight(int[, ] matrix) {
      int rowLen = matrix.GetLength(0);
      for (int i = 0; i < rowLen; i++) {
        // get the row
        int[] row = GetRowByIndex(matrix, i);
        Array.Reverse(row);

        // do the right action
        NumberMerge(row);

        // set the row
        Array.Reverse(row);
        SetRowByIndex(matrix, i, row);
      }
    }

    private static void DoUp(int[, ] matrix) {
      int colLen = matrix.GetLength(1);
      for (int i = 0; i < colLen; i++) {
        // get the col
        int[] col = GetColByIndex(matrix, i);

        // do the up action
        NumberMerge(col);

        // set the col
        SetColByIndex(matrix, i, col);
      }
    }

    private static void DoDown(int[, ] matrix) {
      int colLen = matrix.GetLength(1);
      for (int i = 0; i < colLen; i++) {
        // get the col
        int[] col = GetColByIndex(matrix, i);
        Array.Reverse(col);

        // do the up action
        NumberMerge(col);

        // set the col
        Array.Reverse(col);
        SetColByIndex(matrix, i, col);
      }
    }

    private static void GenerateRandomNumber(int[,] matrix) {
      int[] pos = GetEmptyPos(matrix);

      if (pos[0] == -1) return;
      matrix[pos[0], pos[1]] = 2;
    }

    private static int[] GetEmptyPos(int[,] matrix) {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);

      ArrayList pos = new ArrayList();
      for (int i = 0; i < rowLen; i++) {
        for (int j = 0; j < colLen; j++) {
          if (matrix[i, j] == 0) {
            pos.Add(new int[] { i, j });
          }
        }
      }

      if (pos.Count == 0) {
        isFull = true;
        return new int[] { -1, -1 };
      } else {
        isFull = false;
      }

      int idx = random.Next(0, pos.Count);
      return (int [])pos.ToArray()[idx];
    }

    private static bool CheckFail(int[,] matrix) {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);

      // check row
      for (int row = 0; row < rowLen; row++) {
        for (int col = 0; col < colLen - 1; col++) {
          if (matrix[row, col] == matrix[row, col + 1]) return false;
        }
      }

      // check col
      for (int col = 0; col < colLen; col++) {
        for (int row = 0; row < rowLen - 1; row++) {
          if (matrix[row, col] == matrix[row + 1, col]) return false;
        }
      }

      return true;
    }

    private static int[] GetRowByIndex(int[,] matrix, int idx) {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);
      int[] row = new int[colLen];
      for (int i = 0; i < colLen; i++) {
        row[i] = matrix[idx, i];
      }
      return row;
    }

    private static void SetRowByIndex(int[,] matrix, int idx, int[] row) {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);
      for (int i = 0; i < colLen; i++) {
        matrix[idx, i] = row[i];
      }
    }

    private static int[] GetColByIndex(int[,] matrix, int idx) {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);
      int[] col = new int[rowLen];
      for (int i = 0; i < rowLen; i++) {
        col[i] = matrix[i, idx];
      }
      return col;
    }

    private static void SetColByIndex(int[,] matrix, int idx, int[] col) {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);
      for (int i = 0; i < rowLen; i++) {
        matrix[i, idx] = col[i];
      }
    }

    private static void PrintMatrix(int[, ] matrix) {
      Console.WriteLine("-----------------------------操作次数：{0}----------------------------------", opCount);
      for (int i = 0; i < matrix.GetLength(0); i++) {
        for (int j = 0; j < matrix.GetLength(1); j++) {
          Console.Write(matrix[i, j] + "\t");
        }
        Console.WriteLine();
      }
      Console.WriteLine("---------------------------------------------------------------------------");
    }

    private static void ZeroFilter(int[] arr) {
      int[] newArr = new int[arr.Length];
      int counter = 0;
      for (int i = 0; i < arr.Length; i++) {
        if (arr[i] != 0) {
          newArr.SetValue(arr[i], counter++);
        }
      }
      newArr.CopyTo(arr, 0);
    }

    private static void NumberMerge(int[] arr) {
      // remove zeros
      ZeroFilter(arr);

      for (int i = 0; i < arr.Length - 1; i++) {
        // merge
        if (arr[i] != 0 && arr[i] == arr[i + 1]) {
          arr[i] += arr[i + 1];
          arr[i + 1] = 0;

          // set max number
          if (maxNumber < arr[i]) {
            maxNumber = arr[i];
          }
        }
      }

      // remove zeros
      ZeroFilter(arr);
    }
  }
}