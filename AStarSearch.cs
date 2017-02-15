using System;
using System.Collections.Generic;
using Priority_Queue;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// A* Search modified to include restrictions on the Rover's allowed
    /// movement and rotations.
    /// </summary>
    public class AStarSearch
    {
        public readonly Grid grid;
        public readonly Vector2 start;
        public readonly Vector2 goal;
        public readonly Bearing startFacing;

        private Dictionary<Cell, Node> nodes = new Dictionary<Cell, Node>();

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
