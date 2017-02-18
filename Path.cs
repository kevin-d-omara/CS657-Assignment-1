namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Records the Node leading to this Node, the cumulative cost to reach it,
    /// and the facing after moving into it.
    /// </summary>
    public class Path
    {
        // True if this Node was reached through a Revert Action.
        public bool wasRevertAction = false;

        public readonly Path from;
        public readonly int cost;
        public readonly Bearing facing;

        /// <param name="from">Prior step in Path to this Path.</param>
        /// <param name="cost">Cumulative cost to reach this Node.</param>
        /// <param name="facing">Facing after moving into this Node.</param>
        public Path(Path from, int cost, Bearing facing)
        {
            this.from = from;
            this.cost = cost;
            this.facing = facing;
        }
    }
}
