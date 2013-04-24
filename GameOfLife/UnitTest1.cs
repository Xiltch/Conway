using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfLife {

  [TestClass]
  public class WorldTester {

    [TestMethod]
    public void CreateWorldWithOnlyOneCell() {
      var world = new World();
      world.Add_Cell(new Cell());

      Assert.IsTrue(world.Count_Live_Cells() == 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddCellFailsIfCellPositionAlreadyUsed() {
      var world = new World();

      world.Add_Cell(new Cell(new Position(1, 1)));
      world.Add_Cell(new Cell(new Position(1, 1))); // Should Throw Expected ArugmentException because position already taken

      Assert.Fail("Cannot Add Two Cells with the same Location");
    }

    [TestMethod]
    public void TwoCellsAreNeighbours() {
      var world = new World();
      var cell1 = world.Add_Cell(new Cell(new Position(0, 0)));
      var cell2 = world.Add_Cell(new Cell(new Position(0, 1)));

      Assert.IsTrue(world.Neighbours(cell1, cell2), "Cells are not neighbours");
    }

    [TestMethod]
    public void TwoCellsAreNotNeighbours() {
      var world = new World();
      var cell1 = world.Add_Cell(new Cell(new Position(0, 0)));
      var cell2 = world.Add_Cell(new Cell(new Position(0, 2)));

      Assert.IsFalse(world.Neighbours(cell1, cell2), "Cells are neighbours");
    }

    [TestMethod]
    public void GetCellByPosition() {
      var world = new World();
      var cell1 = world.Add_Cell(new Cell(new Position(0, 1)));
      var cell2 = world.Get_Cell(new Position(0, 1));

      Assert.AreEqual(cell1, cell2);
    }

    [TestMethod]
    public void CountCellsNeigbours() {
      var world = new World();
      var cell = world.Add_Cell(new Cell(new Position(0, 0)));
      world.Add_Cell(new Cell(new Position(0, 1)));
      world.Add_Cell(new Cell(new Position(0, 2)));
      world.Add_Cell(new Cell(new Position(1, 1)));
      world.Add_Cell(new Cell(new Position(2, 1)));

      Assert.IsTrue(world.CountNeighbours(cell) == 2);
    }

    [TestMethod]
    public void GetCellReturnsDeadCellIfPositionNotFound() {
      var world = new World();
      var cell = world.Get_Cell(new Position(0, 0));
      Assert.IsFalse(cell.IsAlive);
    }

    [TestMethod]
    public void GetCellDefaultReturnsCellWithNeighbours() {
      var world = new World();
      var position = new Position(1, 1);
      var cell = world.Get_Cell(position);
      Assert.IsTrue(cell.Position.Neighbours.Count() == 8);
    }

    [TestMethod]
    public void GetCellNeighbourReturnsCellWithNeighbours() {
      var world = new World();
      var cell = world.Add_Cell(new Cell(new Position(1, 1)));
      var neighbours = world.Cell_Neighbours(cell);

      Assert.IsNotNull(neighbours[0].Position.Neighbours);

    }

    [TestMethod]
    public void CollectListOfCellsDeadNeighbours() {
      var world = new World();
      world.Add_Cell(new Cell(new Position(0, 0), true));
      world.Add_Cell(new Cell(new Position(1, 0), true));
      world.Add_Cell(new Cell(new Position(2, 0), true));
      world.Add_Cell(new Cell(new Position(1, 2), true));
      var cell = world.Add_Cell(new Cell(new Position(1, 1), true));

      List<Cell> items = world.Dead_cells(cell);

      var dead_cell1 = items.FirstOrDefault(c => c.Position == new Position(0, 1) && !c.IsAlive);
      var dead_cell2 = items.FirstOrDefault(c => c.Position == new Position(2, 1) && !c.IsAlive);
      var dead_cell3 = items.FirstOrDefault(c => c.Position == new Position(0, 2) && !c.IsAlive);
      var dead_cell4 = items.FirstOrDefault(c => c.Position == new Position(2, 2) && !c.IsAlive);

      var result = dead_cell1 != null && dead_cell2 != null && dead_cell3 != null && dead_cell4 != null;

      Assert.IsTrue(result);

    }

    [TestMethod]
    public void ListDeadCellNeighbours() {
      var world = new World();
      world.Add_Cell(new Cell(new Position(0, 0), true));
      world.Add_Cell(new Cell(new Position(1, 0), true));
      world.Add_Cell(new Cell(new Position(2, 0), true));
      world.Add_Cell(new Cell(new Position(1, 2), true));
      world.Add_Cell(new Cell(new Position(1, 1), true));

      var dead_cell = world.Get_Cell(new Position(0, 1));

      var result = dead_cell.IsAlive == false && world.CountNeighbours(dead_cell) == 4;

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void WorldWithThreeCellsTicksToLeaveOnlyOneCell() {
      var world = new World();
      world.Add_Cell(new Cell(new Position(0, 0), true));
      world.Add_Cell(new Cell(new Position(1, 0), true));
      world.Add_Cell(new Cell(new Position(2, 0), true));

      world.Tick();

      var cell1 = world.Get_Cell(new Position(0, 0));
      var cell2 = world.Get_Cell(new Position(1, 0));
      var cell3 = world.Get_Cell(new Position(2, 0));

      var result = !cell1.IsAlive && cell2.IsAlive && !cell3.IsAlive && world.Count_Live_Cells() == 1;

      Assert.IsTrue(result);      
    }

    [TestMethod]
    public void WorldWithFourCellsTicksRemainsSame() {
      var world = new World();
      world.Add_Cell(new Cell(new Position(0, 0), true));
      world.Add_Cell(new Cell(new Position(1, 0), true));
      world.Add_Cell(new Cell(new Position(0, 1), true));
      world.Add_Cell(new Cell(new Position(1, 1), true));

      world.Tick();

      var cell1 = world.Get_Cell(new Position(0, 0));
      var cell2 = world.Get_Cell(new Position(1, 0));
      var cell3 = world.Get_Cell(new Position(0, 1));
      var cell4 = world.Get_Cell(new Position(1, 1));

      var result = cell1.IsAlive && cell2.IsAlive && cell3.IsAlive && cell4.IsAlive && world.Count_Live_Cells() == 4;

      Assert.IsTrue(result);   
    }

  }

  [TestClass]
  public class CellTester {

    [TestMethod]
    public void CreateCellWithAPosition() {
      var cell = new Cell(new Position());

      Assert.IsInstanceOfType(cell.Position, typeof(Position));
    }

    [TestMethod]
    public void NewCellGetsDefaultPositionOf00() {
      var cell = new Cell();
      var position = new Position(0, 0);

      Assert.IsTrue(cell.Position == position);
    }

    [TestMethod]
    public void NullCellIsEqualToNull() {
      Cell cell = null;
      Assert.IsTrue(cell == null);
    }

    [TestMethod]
    public void CellIsNotEqualToNull() {
      var cell = new Cell();
      Assert.IsTrue(cell != null);
    }

    [TestMethod]
    public void TwoCellsAreEqualUsingEquals() {
      var cell1 = new Cell();
      var cell2 = new Cell();
      Assert.IsTrue(cell1 == cell2);
    }

    [TestMethod]
    public void TwoCellsAreEqualUsingNotEquals() {
      var cell1 = new Cell();
      var cell2 = new Cell();
      Assert.IsFalse(cell1 != cell2);
    }

    [TestMethod]
    public void TwoCellsAreNotEqualsUsingEquals() {
      var cell1 = new Cell(new Position(0, 0));
      var cell2 = new Cell(new Position(0, 1));
      Assert.IsFalse(cell1 == cell2);
    }

    [TestMethod]
    public void TwoCellsAreNotEqualsUsingNotEquals() {
      var cell1 = new Cell(new Position(0, 0));
      var cell2 = new Cell(new Position(0, 1));
      Assert.IsTrue(cell1 != cell2);
    }

  }

  [TestClass]
  public class PositionTester {
    [TestMethod]
    public void PositionHasXValue() {
      var position = new Position();

      Assert.IsNotNull(position.x);
    }

    [TestMethod]
    public void PositionHasYValue() {
      var position = new Position();

      Assert.IsNotNull(position.y);
    }

    [TestMethod]
    public void GetNeighboursReturnsListOfPositions() {
      var position = new Position(0, 0);

      Assert.IsInstanceOfType(position.Get_Neighbours(), typeof(List<Position>));
    }

    [TestMethod]
    public void PositionWithNegativeXValueIsNotValid() {
      var position = new Position(-1, 1);

      Assert.IsTrue(position.IsValid() == false);
    }

    [TestMethod]
    public void PositionWithPositiveXValueIsValid() {
      var position = new Position(1, 1);

      Assert.IsTrue(position.IsValid() == true);
    }

    [TestMethod]
    public void PositionWithNegativeYValueIsNotValid() {
      var position = new Position(1, -1);

      Assert.IsTrue(position.IsValid() == false);
    }

    [TestMethod]
    public void PositionWithPositiveYValueIsValid() {
      var position = new Position(1, 1);

      Assert.IsTrue(position.IsValid() == true);
    }

    [TestMethod]
    public void TwoPositionsWithSameValuesAreEqual() {
      var position1 = new Position(1, 1);
      var position2 = new Position(1, 1);

      Assert.IsTrue(position1 == position2);
    }

    [TestMethod]
    public void TwoPositionsWithDifferentValuesAreNotEqualAssertTrue() {
      var position1 = new Position(1, 1);
      var position2 = new Position(2, 2);

      Assert.IsTrue(position1 != position2);
    }

    [TestMethod]
    public void TwoPositionsWithSameValuesAreNotEqualAssertFalse() {
      var position1 = new Position(1, 1);
      var position2 = new Position(1, 1);

      Assert.IsFalse(position1 != position2);
    }

    [TestMethod]
    public void PositionIsNeighbourOfAnotherPosition() {

      var position1 = new Position(1, 1);
      var position2 = new Position(0, 0);

      var Pos1InPos2 = position2.Neighbours.Where(p => p == position1);

      Assert.IsTrue(Pos1InPos2.Count() == 1, "Position 1 is not a Neighbour of Position 2");

    }

    [TestMethod]
    public void PositionIsNotNeighbourOfAnotherPosition() {

      var position1 = new Position(1, 2);
      var position2 = new Position(0, 0);

      var Pos1InPos2 = position2.Neighbours.Where(p => p == position1);

      Assert.IsTrue(Pos1InPos2.Count() == 0, "Position 1 is a Neighbour of Position 2");

    }

    [TestMethod]
    public void PositionCreatedUsingANeighbourPositionGeneratesListOfNeighbours() {

      var position1 = new Position(1, 1);
      var position2 = new Position(position1.Neighbours[0]);

      Assert.IsNotNull(position2.Neighbours);
    }

  }

  [TestClass]
  public class EvolverTester {

    [TestMethod]
    public void CellWithLessThan2NeighboursDies() {

      var world = new World();
      var cell1 = world.Add_Cell(new Cell(new Position(0, 0), true));
      var cell2 = world.Add_Cell(new Cell(new Position(0, 1), true));

      var evolver = new Evolver(world);

      evolver.ApplyRules();

      var result = !cell1.Lives && !cell2.Lives;

      Assert.IsTrue(result);

    }

    [TestMethod]
    public void CellWithTwoOrThreeNeighboursLives() {

      var world = new World();
      var cell1 = world.Add_Cell(new Cell(new Position(0, 0), true));
      var cell2 = world.Add_Cell(new Cell(new Position(1, 0), true));
      var cell3 = world.Add_Cell(new Cell(new Position(2, 0), true));

      var evolver = new Evolver(world);

      evolver.ApplyRules();

      var result = !cell1.Lives && cell2.Lives && !cell3.Lives;

      Assert.IsTrue(result);

    }

    [TestMethod]
    public void CellWithMoreThanThreeNeighboursDies() {

      var world = new World();
      var cell1 = world.Add_Cell(new Cell(new Position(0, 0), true));
      var cell2 = world.Add_Cell(new Cell(new Position(1, 0), true));
      var cell3 = world.Add_Cell(new Cell(new Position(2, 0), true));
      var cell4 = world.Add_Cell(new Cell(new Position(1, 1), true));
      var cell5 = world.Add_Cell(new Cell(new Position(1, 2), true));

      var evolver = new Evolver(world);

      evolver.ApplyRules();

      var result = cell1.Lives && cell2.Lives && cell3.Lives && !cell4.Lives && !cell5.Lives;

      Assert.IsTrue(result);

    }


    [TestMethod]
    public void DeadCellWithFourLiveCellsLives() {
      var world = new World();
      world.Add_Cell(new Cell(new Position(0, 0), true));
      world.Add_Cell(new Cell(new Position(1, 0), true));
      world.Add_Cell(new Cell(new Position(2, 0), true));
      world.Add_Cell(new Cell(new Position(1, 1), true));
      world.Add_Cell(new Cell(new Position(1, 2), true));

      var evolver = new Evolver(world);

      evolver.ApplyRules();

      var dead_cell1 = world.Get_Cell(new Position(0, 1));
      var dead_cell2 = world.Get_Cell(new Position(2, 1));
      var dead_cell3 = world.Get_Cell(new Position(0, 2));
      var dead_cell4 = world.Get_Cell(new Position(2, 2));

      var result = dead_cell1.Lives && dead_cell2.Lives && !dead_cell3.Lives && !dead_cell4.Lives;

      Assert.IsTrue(result);
    }

  }

}
