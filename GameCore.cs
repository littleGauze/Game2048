using System;
using System.Collections.Generic;

namespace Game2048 {
  public class GameCore {
    public static Random random = new Random();
    private int[, ] matrix;
    public int[, ] Matrix {
      get { return this.matrix; }
    }
    private int[, ] originMatrix;
    private int[] mergeArray;
    private int[] zeroFilterArray;
    private List<int[]> randomList;
    public int maxNumber = 0;
    private int opCount = 0;
    public int OpCount { get { return this.opCount; } }

    private bool isFull = false;

    private bool isChange = true;
    public bool IsChange { get { return this.isChange; } }

    public GameCore() {
      matrix = new int[4, 4];
      originMatrix = new int[4, 4];
      mergeArray = new int[4];
      zeroFilterArray = new int[4];
      randomList = new List<int[]>(16);
    }

    public bool IsWin(int goal) {
      return this.maxNumber == goal;
    }

    public void Move(ArrowKey key) {
      this.isChange = false;
      Array.Copy(matrix, originMatrix, 16);

      switch (key) {
        case ArrowKey.UP:
          MoveUp();
          break;
        case ArrowKey.DOWN:
          MoveDown();
          break;
        case ArrowKey.LEFT:
          MoveLeft();
          break;
        case ArrowKey.RIGHT:
          MoveRight();
          break;
      }

      for (int r = 0; r < matrix.GetLength(0); r++) {
        for (int c = 0; c < matrix.GetLength(1); c++) {
          if (originMatrix[r, c] != matrix[r, c]) {
            this.isChange = true;

            // count op times
            this.opCount++;
            return;
          }
        }
      }
    }

    private void MoveLeft() {
      for (int r = 0; r < matrix.GetLength(0); r++) {
        // get the row
        for (int c = 0; c < matrix.GetLength(1); c++)
          mergeArray[c] = matrix[r, c];

        // do the left actions
        NumberMerge();

        // set the row
        for (int c = 0; c < matrix.GetLength(1); c++)
          matrix[r, c] = mergeArray[c];
      }
    }

    private void MoveRight() {
      for (int r = 0; r < matrix.GetLength(0); r++) {
        // get the row
        for (int c = matrix.GetLength(1); c > 0; c--)
          mergeArray[matrix.GetLength(1) - c] = matrix[r, c - 1];

        // do the right action
        NumberMerge();

        // set the row
        for (int c = matrix.GetLength(1); c > 0; c--)
          matrix[r, c - 1] = mergeArray[matrix.GetLength(1) - c];
      }
    }

    private void MoveUp() {
      for (int c = 0; c < matrix.GetLength(1); c++) {
        // get the col
        for (int r = 0; r < matrix.GetLength(0); r++)
          mergeArray[r] = matrix[r, c];

        // do the up action
        NumberMerge();

        // set the col
        for (int r = 0; r < matrix.GetLength(0); r++)
          matrix[r, c] = mergeArray[r];
      }
    }

    private void MoveDown() {
      for (int c = 0; c < matrix.GetLength(1); c++) {
        // get the col
        for (int r = matrix.GetLength(0); r > 0; r--)
          mergeArray[matrix.GetLength(0) - r] = matrix[r - 1, c];

        // do the up action
        NumberMerge();

        // set the col
        for (int r = matrix.GetLength(0); r > 0; r--)
          matrix[r - 1, c] = mergeArray[matrix.GetLength(0) - r];
      }
    }

    public void GenerateRandomNumber() {
      int rowLen = matrix.GetLength(0);
      int colLen = matrix.GetLength(1);

      randomList.Clear();
      for (int i = 0; i < rowLen; i++) {
        for (int j = 0; j < colLen; j++) {
          if (matrix[i, j] == 0) {
            randomList.Add(new int[] { i, j });
          }
        }
      }

      if (randomList.Count == 0) {
        this.isFull = true;
        return;
      }

      this.isFull = false;
      if (this.isChange) {
        int idx = random.Next(0, randomList.Count);
        int[] item = randomList[idx];
        matrix[item[0], item[1]] = random.Next(1, 11) == 1 ? 4 : 2;
      }
    }

    public bool IsFailed() {
      if (!this.isFull) return false;

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

    private void ZeroFilter() {
      Array.Clear(zeroFilterArray, 0, 4);
      int counter = 0;
      for (int i = 0; i < mergeArray.Length; i++) {
        if (mergeArray[i] != 0) {
          zeroFilterArray[counter++] = mergeArray[i];
        }
      }
      zeroFilterArray.CopyTo(mergeArray, 0);
    }

    private void NumberMerge() {
      // remove zeros
      ZeroFilter();
      for (int i = 0; i < mergeArray.Length - 1; i++) {
        // merge
        if (mergeArray[i] != 0 && mergeArray[i] == mergeArray[i + 1]) {
          mergeArray[i] += mergeArray[i + 1];
          mergeArray[i + 1] = 0;

          // set max number
          if (maxNumber < mergeArray[i]) {
            maxNumber = mergeArray[i];
          }
        }
      }
      ZeroFilter();
    }
  }
}