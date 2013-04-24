using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife {

  class Position {

    private List<Position> _neighbours { get; set; }

    public int x { get; set; }
    public int y { get; set; }

    public List<Position> Neighbours {
      get {
        return this._neighbours;
      }
    }

    public Position()
      : this(0, 0) {
    }

    public Position(int x, int y)
      : this(x, y, null) {
      _neighbours = new List<Position>();
      Set_Neighbours();
    }

    public Position(int x, int y, object AvoidSetNeighbours) {
      this.x = x;
      this.y = y;
    }

    public Position(Position position)
      : this(position.x, position.y) {
    }

    private void Set_Neighbours() {
      for ( int y = this.y - 1 ; y <= this.y + 1 ; y++ )
        for ( int x = this.x - 1 ; x <= this.x + 1 ; x++ )
          _neighbours.Add(new Position(x, y, null)); // Skip Default Constructors to avoid stack overflows
      _neighbours = _neighbours.Where(p => p != this && p.IsValid()).ToList();
    }

    internal List<Position> Get_Neighbours() {
      return this.Neighbours;
    }

    internal bool IsValid() {
      var result = this.x >= 0 && this.y >= 0;
      return result;
    }

    public override bool Equals(object obj) {
      return Equals(obj as Position);
    }

    public bool Equals(Position obj) {
      return obj.x == this.x && obj.y == this.y;
    }

    public override int GetHashCode() {
      int hash = 17;
      hash = hash * 23 + this.x.GetHashCode();
      hash = hash * 23 + this.y.GetHashCode();
      return hash;
    }

    public static bool operator ==(Position position1, Position position2) {
      return position1.Equals(position2);
    }

    public static bool operator !=(Position position1, Position position2) {
      return !position1.Equals(position2);
    }

  }

}
