using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Rover
    {
        // State
        public Vector2 Position { get; private set; }
        public Bearing Facing { get; private set; }

        public int MoveCount { get; private set; } = 0;
        public Stack<Vector2> Moves { get; private set; }
            = new Stack<Vector2>();

        // Database
        private Grid grid;

        // Intelligent Control
        // todo

        // Movement Set
        // todo

        // Sonar Set
        // todo

        public Rover(GridParameters gridParameters)
        {
            grid = new Grid(gridParameters);
            Position = gridParameters.startPosition;
            
            // !TODO: replace South w/ direction facing goal
            Facing = Bearing.South;
            
        }

        public void Update()
        {
            // DetectWithSonar()
            // DecideOnMove()
            MakeMove();
        }

        private void MakeMove()
        {
            Moves.Push(Position);
            ++MoveCount;

            // update Position
            Vector2 offset = Utility.ConvertBearingToCoordinateOffset(Facing);
            Position = new Vector2(Position.x + offset.x, Position.y + offset.y);

            // update Facing

        }
    }
}
