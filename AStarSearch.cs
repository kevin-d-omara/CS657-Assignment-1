using System;
using System.Collections.Generic;

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
        public readonly Direction startDirection;

        private Dictionary<Cell, Node> nodes = new Dictionary<Cell, Node>();

        public AStarSearch(Grid grid, Vector2 start, Direction startDirection,
            Vector2 goal)
        {
            this.grid = grid;
            this.start = start;
            this.goal = goal;
            this.startDirection = startDirection;

            Search();
        }

        private void Search()
        {
            var startNode = new Node(start);
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
