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
        public delegate int DistanceHeuristic(Cell a, Cell b);
        public readonly DistanceHeuristic heuristic;
        public readonly Grid grid;
        public readonly Vector2 start;
        public readonly Vector2 goal;
        public readonly Bearing startFacing;
        public readonly Stack<Rover.MoveRecord> previousMoves;

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
        // Store the allowed movements and their associated costs. Ex: SideRight
        // Costs 2 because it consists of Rotate +45 and Move ForwardRight.
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
            Vector2 goal, Stack<Rover.MoveRecord> previousMoves,
            DistanceHeuristic heuristic)
        {
            this.grid = grid;
            this.start = start;
            this.goal = goal;
            this.startFacing = startFacing;
            // Twice nested "new Stack<>()" to get correct ordering.
            this.previousMoves = new Stack<Rover.MoveRecord>(
                new Stack<Rover.MoveRecord>(previousMoves));
            this.heuristic = heuristic;

            Search();
        }

        private void Search()
        {
            var startNode = new Node(start);
            var startPath = new Path(startNode, 0, startFacing);
            startNode.Append(startPath);
            var startCell = grid.Position[(int)start.x, (int)start.y];
            nodes.Add(startCell, startNode);
            // Frontier is the open set of nodes to be explored.
            var frontier = new SimplePriorityQueue<Node, int>();
            frontier.Enqueue(startNode, 0);

            // Create and Enqueue all Nodes the Rover has been to prior.
            // This allows the Rover to benefit from the Revert action.
            {
                var revertCount = 0;
                var currentNode = startNode;
                while (previousMoves.Count > 0)
                {
                    ++revertCount;
                    var priorMove = previousMoves.Pop();
                    var priorCell = grid.Position[(int)priorMove.Position.x,
                        (int)priorMove.Position.y];

                    if(nodes.TryGetValue(priorCell, out Node priorNode))
                    {
                        // TODO - handle this case
                    }
                    else
                    {
                        priorNode = new Node(priorMove.Position);
                        priorNode.Append(new Path(currentNode, revertCount,
                            priorMove.Facing));
                        priorNode.Paths[0].wasRevertAction = true;
                        nodes.Add(priorCell, priorNode);
                        frontier.Enqueue(priorNode, revertCount);
                    }

                    currentNode = priorNode;
                }
            }

            // Create and Enqueue the Node directly Backward from the
            // starting Node. This allows the Rover to "back out" of a
            // blocked start.
            {
                var rearFacing = startFacing.ToBearing(Direction.Backward);
                var offset = rearFacing.ToCoordinateOffset();
                var rearPos = new Vector2((start.x + offset.x),
                    (start.y + offset.y));
                var rearCell = grid.Position[(int)rearPos.x, (int)rearPos.y];
                if (!rearCell.blocksMove)
                {
                    if (nodes.TryGetValue(rearCell, out Node rearNode))
                    { }
                    else
                    {
                        rearNode = new Node(rearPos);
                        rearNode.Append((new Path(startNode, 4, rearFacing)));
                        frontier.Enqueue(rearNode, 4);
                    }
                }
            }

            // Explore the Frontier Nodes with priority given to the
            // cheapest-to-reach Nodes.
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
                        var newCost = path.cost + move.cost;
                        var newFacing = path.facing.ToBearing(move.direction);
                        var offset = newFacing.ToCoordinateOffset();

                        var nextCell = grid.Position[(int)(currentNode.pos.x
                            + offset.x), (int)(currentNode.pos.y + offset.y)];

                        if (nextCell.blocksMove) { continue; }

                        if (nodes.TryGetValue(nextCell, out Node nextNode))
                        { }
                        else
                        {
                            var pos = new Vector2(nextCell.x, nextCell.y);
                            nextNode = new Node(pos);
                            nodes.Add(nextCell, nextNode);
                        }

                        if (nextNode.Paths.Count == 0)
                        {
                            nextNode.Append(new Path(currentNode, newCost,
                                newFacing));
                            frontier.Enqueue(nextNode, newCost);
                        }
                        else // Count > 1
                        {
                            // All Paths in a Node have equal cost.
                            if (nextNode.Paths[0].cost > newCost)
                            {
                                nextNode.Replace(new Path(currentNode,
                                    newCost, newFacing));
                                frontier.UpdatePriority(nextNode, newCost);
                            }
                            else if (nextNode.Paths[0].cost == newCost)
                            {
                                // Prevent duplicate Paths from being appended.
                                var shouldAppend = true;
                                foreach (Path targetPath in nextNode.Paths)
                                {
                                    if (targetPath.facing == newFacing)
                                    {
                                        shouldAppend = false;
                                        break;
                                    }
                                }
                                if (shouldAppend)
                                {
                                    nextNode.Append(new Path(currentNode,
                                        newCost, newFacing));
                                }
                            }
                        }
                    }
                }
            }
            // Prevent GetShortestPath() from hitting an infinite loop.
            startNode.ClearPaths();
        }

        /// <summary>
        /// Returns a Stack with the shortest path to the Goal. Note: Stack will
        /// be empty if there is no path.
        /// </summary>
        /// <returns>If a path exists, returns a Stack with the shortest path to
        /// the goal. Else, returns an empty Stack.</returns>
        public Stack<Node> GetShortestPath()
        {
            var shortestPath = new Stack<Node>();

            // Start with goal Node.
            if (nodes.TryGetValue(grid.Position[(int)goal.x, (int)goal.y],
                out Node nextNode))
            { }
            // Exit early if goal wasn't reached.
            else
            {
                return shortestPath;
            }

            while (nextNode.Paths.Count > 0)
            {
                shortestPath.Push(nextNode);

                // Heuristic:
                // If an available Path has 'wasReverAction' flagged, take it.
                // Otherwise, take the first available path (Paths[0]). Note:
                // All Paths on a Node have equal cost, but different facing.
                var wasRevertAction = false;
                foreach (Path path in nextNode.Paths)
                {
                    if (path.wasRevertAction)
                    {
                        nextNode = path.from;
                        wasRevertAction = true;
                        break;
                    }
                    if (path.from.Paths.Count > 0)
                    {
                        foreach (Path innerPath in path.from.Paths)
                        {
                            if (innerPath.wasRevertAction)
                            {
                                nextNode = path.from;
                                wasRevertAction = true;
                                break;
                            }
                        }
                    }
                }
                if (!wasRevertAction)
                {
                    nextNode = nextNode.Paths[0].from;
                }
            }

            return shortestPath;
        }
    }
}
