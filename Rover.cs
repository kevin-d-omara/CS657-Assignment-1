using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Rover is controlled by an Expert AI and incrementally updates it's
    /// knowledge of the surrounding terrain. During each Update() it sends out
    /// Sonar to find obstacles, then decides on the best move to get closer to
    /// the goal location.
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

        public readonly List<Sequence> allowedMovementSequences;
        public readonly List<Sequence> allowedSonarSequences;

        public Rover(GridParameters gridParameters, RoverParameters roverParams)
        {
            Grid = new Grid(gridParameters);
            Position = gridParameters.startPosition;

            Facing = roverParams.facing;

            // Deep copy lists.
            allowedMovementSequences = new List<Sequence>();
            foreach (Sequence sequence in roverParams.allowedMovementSequences)
            {
                allowedMovementSequences.Add(sequence);
            }
            allowedSonarSequences = new List<Sequence>();
            foreach (Sequence sequence in roverParams.allowedSonarSequences)
            {
                allowedSonarSequences.Add(sequence);
            }
        }

        public void Update()
        {
            DetectWithSonar();

            // DecideOnMove();

            MakeMove(Direction.Forward);
        }

        /// <summary>
        /// Rover sends out sonar signals to detect surrounding environment. The
        /// results are used to update the internal database.
        /// </summary>
        private void DetectWithSonar()
        {
            foreach (Sequence sequence in allowedSonarSequences)
            {

            }
        }

        /// <summary>
        /// Reset Rover's Position & Facing to the last move made. Note: this
        /// counts as a move and does not place a new move on the stack.
        /// </summary>
        private void ReverseMove()
        {
            ++MoveCount;

            if (PreviousMoves.Count > 0)
            {
                Move lastMove = PreviousMoves.Pop();
                Position = lastMove.Position;
                Facing = lastMove.Facing;
            }
        }

        /// <summary>
        /// Move the Rover one cell in the direction specified. I.e. if the
        /// Rover is facing South, and moves in the direction ForwardLeft,
        /// the Rover will move SouthEast.
        /// </summary>
        /// <param name="direction">Direction relative to the Rover's facing to move.</param>
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
