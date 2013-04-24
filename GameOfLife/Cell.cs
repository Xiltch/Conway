using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife {
  class Cell {

    private bool StateCurrent { get; set; }
    private bool StateNext { get; set; }

    public Position Position { get; set; }

    public Cell()
      : this(new Position()) {
    }

    public Cell(Position position)
      : this(position, false) {
    }

    public Cell(GameOfLife.Position position, bool IsAlive) {
      this.Position = position; // 
      this.StateCurrent = IsAlive;
    }

    internal bool IsAlive {
      get { return this.StateCurrent; }
    }

    internal bool Lives {
      get { return this.StateNext; }
    }

    internal void LiveOn(bool state) {
      this.StateNext = state;
    }

    public override bool Equals(object obj) {
      return Equals(obj as Cell);
    }

    public bool Equals(Cell obj) {
      return !Object.ReferenceEquals(obj, null) && this.Position == obj.Position && this.IsAlive == obj.IsAlive && this.Lives == obj.Lives;
    }

    public override int GetHashCode() {
      int hash = this.Position.GetHashCode();
      hash = hash * 23 + Convert.ToInt32(this.IsAlive);
      hash = hash * 23 + Convert.ToInt32(this.Lives);
      return hash;
    }

    public static bool operator ==(Cell cell1, Cell cell2) {
      return ( Object.ReferenceEquals(cell1, null) && Object.ReferenceEquals(cell1, cell2) ) || cell1.Equals(cell2);
    }

    public static bool operator !=(Cell cell1, Cell cell2) {
      return !cell1.Equals(cell2);
    }

  }

}
