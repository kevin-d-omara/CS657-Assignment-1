using System;
using System.Collections.Generic;
using Priority_Queue;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Node of a graph. Holds a list of Path's leading to it. Note: all Paths
    /// owned by a Node have the same cost and different facings.
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
}
