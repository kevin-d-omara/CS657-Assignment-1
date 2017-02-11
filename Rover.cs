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
        public Stack<Vector2> PreviousMoves { get; private set; }
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
            Facing = Bearing.SouthEast;
            
        }

        public void Update()
        {
            // DetectWithSonar()
            // DecideOnMove()
            MakeMove(Direction.Forward);
        }

        private void MakeMove(Direction direction)
        {
            // record action
            PreviousMoves.Push(Position);
            ++MoveCount;

            // update Position
            Bearing moveBearing = Facing.ToBearing(direction);
            Vector2 offset = moveBearing.ToCoordinateOffset();
            Position = new Vector2(Position.x + offset.x, Position.y + offset.y);

            // update Facing
            Facing = moveBearing;
        }
    }
}
