namespace KevinDOMara.SDSU.CS657.Assignment1
{
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
