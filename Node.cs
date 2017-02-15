using System;
using System.Collections.Generic;
using Priority_Queue;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Node of a graph. Holds a list of Path's leading to it.
    /// </summary>
    public class Node
    {
        public readonly Vector2 pos;
        public List<Path> Paths { get; private set; }
        public Node(Vector2 position)
        {
            pos = position;
            Paths = new List<Path>();
        }

        /// <summary>
        /// Add a new Path leading to this Node.
        /// </summary>
        public void Append(Path path) { Paths.Add(path); }

        /// <summary>
        /// Replace all Paths leading to this Node with a single new path.
        /// </summary>
        public void Replace(Path path) { Paths = new List<Path> { path }; }
    }

    /// <summary>
    /// Holds a single step leading to the owner's Node.
    /// </summary>
    public class Path
    {
        public readonly Vector2 from;
        public readonly int cost;
        public readonly Bearing facing;

        public Path(Vector2 from, int cost, Bearing facing)
        {
            this.from = from;
            this.cost = cost;
            this.facing = facing;
        }
    }
}
