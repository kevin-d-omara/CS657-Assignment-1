using System;
using System.Collections.Generic;
// Note: Priority_Queue is from GitHub. I received permission from the professor
// to use this external resource:
// https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
using Priority_Queue;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// A* Search modified to include restrictions on the Rover's allowed
    /// movement and rotations.
    /// </summary>
    // Each step of modified A* will mark the adjacent & diagonal open cells
    // ('.') with 3 pieces of data:
    // Cost   - cost to reach this cell
    // Facing - facing after moving into this cell
    // From   - position of cell prior to this
    // 
    // Example: The Rover is Facing South.
    //   123       Cost:     Facing:         From:
    // 1|...       343       NW, N, NE       [2,2][2,2][2,2]
    // 2|.R.  ==>  2R2  and   W,  ,  E  and  [2,2]     [2,2]
    // 3|...       111       SW, S, SE       [2,2][2,2][2,2]

    public class AStarSearch
    {
        public readonly Grid grid;
        public readonly Vector2 start;
        public readonly Vector2 goal;
        public readonly Bearing startFacing;

        private Dictionary<Cell, Node> nodes = new Dictionary<Cell, Node>();
        private class Move
        {
            public readonly int cost;
            public readonly Direction direction;
            public Move(int cost, Direction direction)
            {
                this.cost = cost;
                this.direction = direction;
            }
        }
        private readonly Move[] adjacentCells = new Move[]
        {
            new Move(1, Direction.Forward),
            new Move(1, Direction.ForwardRight),
            new Move(1, Direction.ForwardLeft),
            new Move(2, Direction.SideRight),
            new Move(2, Direction.SideLeft),
            new Move(3, Direction.BackwardRight),
            new Move(3, Direction.BackwardLeft)
        };

        public AStarSearch(Grid grid, Vector2 start, Bearing startFacing,
            Vector2 goal)
        {
            this.grid = grid;
            this.start = start;
            this.goal = goal;
            this.startFacing = startFacing;

            Search();
        }

        private void Search()
        {
            var startNode = new Node(start);
            var startPath = new Path(start, 0, startFacing);
            // Frontier is the open set of nodes to be explored.
            var frontier = new SimplePriorityQueue<Node, int>();
            frontier.Enqueue(startNode, 0);

            while (frontier.Count > 0)
            {
                var currentNode = frontier.Dequeue();

                if (currentNode.pos == goal)
                {
                    break;
                }

                foreach (Path path in currentNode.Paths)
                {
                    foreach (Move move in adjacentCells)
                    {
                        var moveBearing = path.facing.ToBearing(move.direction);
                        var offset = moveBearing.ToCoordinateOffset();

                        var targetCell = grid.Position[(int)(currentNode.pos.x
                            + offset.x), (int)(currentNode.pos.y + offset.y)];

                        if (targetCell.blocksMove) { continue; }

                        var newCost = path.cost + move.cost;
                        var newFacing = moveBearing;

                        if (nodes.TryGetValue(targetCell, out Node targetNode))
                        { }
                        else
                        {
                            var pos = new Vector2(targetCell.x, targetCell.y);
                            targetNode = new Node(pos);
                            nodes.Add(targetCell, targetNode);
                        }

                        if (targetNode.Paths.Count == 0)
                        {
                            targetNode.Append(new Path(currentNode.pos, newCost,
                                newFacing));
                            frontier.Enqueue(targetNode, newCost);
                        }
                        else // Count > 1
                        {
                            // All Paths in a Node have equal cost.
                            if (targetNode.Paths[0].cost > newCost)
                            {
                                targetNode.Replace(new Path(currentNode.pos,
                                    newCost, newFacing));
                                frontier.UpdatePriority(targetNode, newCost);
                            }
                            else if (targetNode.Paths[0].cost == newCost)
                            {
                                // Prevent duplicate Paths from being appended.
                                var shouldAppend = true;
                                foreach (Path targetPath in targetNode.Paths)
                                {
                                    if (targetPath.facing == newFacing)
                                    {
                                        shouldAppend = false;
                                        break;
                                    }
                                }
                                if (shouldAppend)
                                {
                                    targetNode.Append(new Path(currentNode.pos,
                                        newCost, newFacing));
                                }
                            }
                        }
                    }
                }
            }
        }

        public Stack<Vector2> GetShortestPath()
        {
            var shortestPath = new Stack<Vector2>();

            // push goal
            // push (goal.closestPath), repeat
            // stop @ start

            return shortestPath;
        }
    }
}
