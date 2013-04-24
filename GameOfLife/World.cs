using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife {

  class World {

    public Dictionary<Position, Cell> Current_Generation { get; set; }

    public World() {
      Current_Generation = new Dictionary<Position, Cell>();
    }

    internal Cell Add_Cell(Cell cell) {
      this.Current_Generation.Add(cell.Position, cell);
      return cell;
    }

    internal int Count_Live_Cells() {
      return Current_Generation.Count();
    }

    internal bool Neighbours(Cell cell1, Cell cell2) {
      var test = from c in cell1.Position.Neighbours
                 where c == cell2.Position
                 select c;
      return test.Count() == 1;
    }

    internal int CountNeighbours(Cell cell) {
      int neighbours = 0;
      foreach ( Position p in cell.Position.Neighbours )
        neighbours += Current_Generation.Keys.Where(k => k == p).Count();
      return neighbours;
    }

    internal Cell Get_Cell(Position position) {
      return this.Current_Generation
        .Where(k => k.Key == position)
        .Select(k => k.Value)
        .DefaultIfEmpty(new Cell(new Position(position)))
        .First();
      // return Current_Generation.FirstOrDefault(k => k.Key == position) .Value ?? new Cell(position);      
    }

    internal List<Cell> Dead_cells(Cell cell) {
      var items = Cell_Neighbours(cell)
        .Where(c => c.IsAlive == false)
        .Select(c => c).ToList();
      return items;
    }

    internal List<Cell> Cell_Neighbours(Cell cell) {
      var items = new List<Cell>();
      foreach ( Position neighbour in cell.Position.Neighbours )
        items.Add(Get_Cell(neighbour));
      return items;
    }


    internal void Tick() {
      Evolver god = new Evolver(this);
      god.ApplyRules();
      Current_Generation = Current_Generation.Select(item => item.Value).Where(cell => cell.Lives).ToDictionary(cell => cell.Position, cell => cell);
    }
  }

}
