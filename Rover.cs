using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Rover is controlled by an Expert AI and incrementally updates it's
    /// knowledge of the surrounding terrain. Each Update() it sends out Sonar
    /// to find obstacles, then decides on the best move to get closer to the
    /// goal location.
    /// </summary>
    public class Rover
    {
        // State
        public Vector2 Position { get; private set; }
        public Bearing Facing { get; private set; }

        public int MoveCount { get; private set; } = 0;
        public Stack<Move> PreviousMoves { get; private set; }
            = new Stack<Move>();

        public class Move
        {
            public Vector2 Position { get; private set; }
            public Bearing Facing { get; private set; }
            public Move(Vector2 position, Bearing facing)
            {
                Position = position;
                Facing = facing;
            }
        }

        // Database
        public Grid Grid { get; private set; }

        // Intelligent Control
        // todo

        // Movement Set
        // todo

        // Sonar Set
        // todo

        public Rover(GridParameters gridParameters)
        {
            Grid = new Grid(gridParameters);
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
            PreviousMoves.Push(new Move(Position, Facing));
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
