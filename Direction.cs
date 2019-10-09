namespace Game2048 {
  public struct Direction {
    private int RIndex { get; set; }
    private int CIndex { get; set; }

    public Direction(int rIndex, int cIndex) {
      this.RIndex = rIndex;
      this.CIndex = cIndex;
    }

    public static Direction Up {
      get {
        return new Direction(-1, 0);
      }
    }

    public static Direction Down {
      get {
        return new Direction(1, 0);
      }
    }

    public static Direction Left {
      get {
        return new Direction(0, -1);
      }
    }

    public static Direction Right {
      get {
        return new Direction(0, 1);
      }
    }

  }
}