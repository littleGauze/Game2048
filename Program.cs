using System;

namespace Game2048 {
  class Program {
    static Random random = new Random();
    static int Goal = 2048;

    static void Main(string[] args) {
      GameCore core = new GameCore();

      core.GenerateRandomNumber();
      core.GenerateRandomNumber();

      do {
        Draw(core);

        // game over
        if (core.IsWin(Goal) || core.IsFailed()) break;

        ConsoleKeyInfo info = Console.ReadKey();
        ArrowKey key = (ArrowKey) info.Key.GetHashCode();

        core.Move(key);
        core.GenerateRandomNumber();

      } while (true);
    }

    public static void Draw(GameCore core) {
      Console.Clear();
      Console.WriteLine("请使用上下左右键进行游戏: 当前已操作 {0} 次", core.OpCount);
      Console.WriteLine("---------------------------------------------------------------------------");
      for (int i = 0; i < core.Matrix.GetLength(0); i++) {
        for (int j = 0; j < core.Matrix.GetLength(1); j++) {
          Console.Write(core.Matrix[i, j] + "\t");
        }
        Console.WriteLine();
      }
      Console.WriteLine("---------------------------------------------------------------------------");
      if (core.IsWin(Goal)) {
        Console.WriteLine("恭喜你，你赢了，一共进行了 {0} 次操作。", core.OpCount);
      } else if (core.IsFailed()) {
        Console.WriteLine("很遗憾，你输了，一共进行了 {0} 次操作。", core.OpCount);
      }
    }
  }
}