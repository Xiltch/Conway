using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife {

  class Evolver {

    private World world { get; set; }

    public Evolver(World world) {
      this.world = world;
    }

    internal void ApplyRules() {
      Rule2();
      Rule4();
    }

    // Rule is depreciated as by default we can assume all cells will die unless
    // they are explicityly told to live.
    internal void Rule1() {
      foreach ( Cell cell in world.Current_Generation.Where(k => k.Value.IsAlive && world.CountNeighbours(k.Value) < 2 ).Select(k => k.Value) )
        cell.LiveOn(false);
    }

    internal void Rule2() {
      foreach ( Cell cell in world.Current_Generation.Where(k => k.Value.IsAlive && ( world.CountNeighbours(k.Value) == 2 ) || (world.CountNeighbours(k.Value) == 3) ).Select(k => k.Value) )
        cell.LiveOn(true);
    }

    internal void Rule4() {
      var dead_cells = CollectDeadCells();
      foreach ( Cell cell in dead_cells ) {
        cell.LiveOn(true);
        world.Add_Cell(cell);
      }        
    }

    private List<Cell> CollectDeadCells() {
      List<Cell> items = new List<Cell>();
      foreach ( Cell cell in world.Current_Generation.Select(k => k.Value) )
        items.AddRange(world.Dead_cells(cell));
      return items.Select(cell => cell).Where(cell => world.CountNeighbours(cell) == 4).Distinct().ToList();
    }

  }

}
