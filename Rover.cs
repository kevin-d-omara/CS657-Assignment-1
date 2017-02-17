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
        public Stack<ActionRecord> PreviousMoves { get; private set; }
            = new Stack<ActionRecord>();

        public class ActionRecord
        {
            public Vector2 Position { get; private set; }
            public Bearing Facing { get; private set; }
            public ActionRecord(Vector2 position, Bearing facing)
            {
                Position = position;
                Facing = facing;
            }
        }

        // Database
        public Grid Grid { get; private set; }

        // Actual environment being traversed.
        public readonly Grid environment;

        // Intelligent Control
        public readonly bool usingAI;

        public readonly List<Sequence> allowedMovementSequences;
        public readonly List<Sequence> allowedSonarSequences;

        public Rover(RoverParameters roverParams, Grid environment,
            GridParameters gridParameters, bool usingAI)
        {
            Grid = new Grid(gridParameters);
            Position = gridParameters.startPosition;
            this.environment = environment;

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

            this.usingAI = usingAI;
        }

        public void Update()
        {
            DetectEnvironmentWithSonar();

            Action action = ChooseAction();

            TakeAction(action);
        }

        /// <summary>
        /// Rover sends out sonar signals to detect surrounding environment. The
        /// results are used to update the internal database.
        /// </summary>
        private void DetectEnvironmentWithSonar()
        {
            // Use sonar in all allowed directions.
            foreach (Sequence sonarSequence in allowedSonarSequences)
            {
                List<Cell> hitCells = environment.RaycastFrom(Position, Facing,
                    sonarSequence);

                // Update database according to results.
                foreach (Cell cell in hitCells)
                {
                    if (cell.type != Grid.Position[cell.x, cell.y].type)
                    {
                        Grid.Position[cell.x, cell.y] = new Cell(cell.type,
                            cell.x, cell.y);
                    }
                }
            }
        }

        /// <summary>
        /// Consult Expert AI or User Input to determine the next action.
        /// </summary>
        private Action ChooseAction()
        {
            Action action;

            if (usingAI)
            {
                // Run A* search.
                var aStarSearch = new AStarSearch(Grid, Position, Facing,
                    Grid.goalPosition, PreviousMoves);
                var shortestPath = aStarSearch.GetShortestPath();

                if (shortestPath.Count == 0)
                {
                    Console.WriteLine("-----> No Path Found!"); // TODO: make this stop the simulation
                }

                // Determine Direction relative to the Rover of the first move.
                var nextNode = shortestPath.Pop();
                var offset = new Vector2(nextNode.pos.x - Position.x,
                    nextNode.pos.y - Position.y);

                var direction = Facing.ToDirection(offset);

                // Determine if nextNode is best reached by a Revert.
                var shouldRevert = false;
                {
                    if (PreviousMoves.Count > 0 &&
                        nextNode.pos.Equals(PreviousMoves.Peek().Position))
                    {
                        foreach (Path path in nextNode.Paths)
                        {
                            if (path.facing == PreviousMoves.Peek().Facing)
                            {
                                shouldRevert = true;
                            }
                        }
                    }
                }

                // Consult Expert System Rules to determine the Action.
                if (shouldRevert)
                {
                    action = new Action(Action.Type.Revert, Direction.Forward);
                }
                if (direction == Direction.Forward
                    || direction == Direction.ForwardLeft
                    || direction == Direction.ForwardRight)
                {
                    action = new Action(Action.Type.Move, direction);
                }
                else if (direction == Direction.SideLeft
                    || direction == Direction.SideRight
                    || direction == Direction.BackwardLeft
                    || direction == Direction.BackwardRight)
                {
                    action = new Action(Action.Type.Rotate, direction);
                }
                else // direction == Direction.Backward
                {
                    action = new Action(Action.Type.Rotate,
                        Direction.ForwardLeft);
                }
            }
            else
            {
                var keyInfo = Console.ReadKey();
                switch ((char)keyInfo.Key)
                {
                    case 'W':
                        action = new Action(Action.Type.Move, Direction.Forward);
                        break;
                    case 'A':
                        action = new Action(Action.Type.Move, Direction.ForwardLeft);
                        break;
                    case 'D':
                        action = new Action(Action.Type.Move, Direction.ForwardRight);
                        break;
                    case 'Q':
                        action = new Action(Action.Type.Rotate, Direction.ForwardLeft);
                        break;
                    case 'E':
                        action = new Action(Action.Type.Rotate, Direction.ForwardRight);
                        break;
                    case 'S':
                        action = new Action(Action.Type.Revert, Direction.ForwardLeft);
                        break;
                    default:
                        action = new Action(Action.Type.Move, Direction.Forward);
                        break;
                }
            }

            return action;
        }

        private void TakeAction(Action action)
        {
            switch (action.type)
            {
                case Action.Type.Move:
                    Move(action.direction);
                    break;
                case Action.Type.Rotate:
                    Rotate45(action.direction);
                    break;
                case Action.Type.Revert:
                    RevertToLastAction();
                    break;
            }
        }

        /// <summary>
        /// Move the Rover one cell in the direction specified. Rover rotates 45
        /// degrees if direction is not Forward. I.e. if the Rover is facing
        /// South, and moves in the direction ForwardLeft, the Rover will move
        /// SouthEast and rotate 45 degrees CCW.
        /// </summary>
        /// <param name="direction">Direction relative to the Rover's facing to
        /// move.</param>
        private void Move(Direction direction)
        {
            // Record Action.
            PreviousMoves.Push(new ActionRecord(Position, Facing));
            ++MoveCount;

            Bearing moveBearing = Facing.ToBearing(direction);
            Vector2 offset = moveBearing.ToCoordinateOffset();

            // Check actual environment to see if target Cell blocks movement.
            // If so, don't move. This represents the rover bumping into a Wall.
            if (!Grid.Position[(int)(Position.x + offset.x),
                (int)(Position.y + offset.y)].blocksMove)
            {
                Position = new Vector2(Position.x + offset.x,
                    Position.y + offset.y);
                Facing = moveBearing;
            }
        }

        /// <summary>
        /// Rotate the rover 45 degrees in the direction specified.
        /// </summary>
        /// <param name="direction">Direction relative to the Rover's facing to
        /// move.</param>
        private void Rotate45(Direction direction)
        {
            // Record Action.
            PreviousMoves.Push(new ActionRecord(Position, Facing));
            ++MoveCount;

            // Exit early if unallowed direction.
            if (direction == Direction.Forward
                || direction == Direction.Backward) { return; }

            // Hard limit the rotation to 45 degrees.
            if (direction == Direction.ForwardLeft
                || direction == Direction.SideLeft
                || direction == Direction.BackwardLeft)
            {
                direction = Direction.ForwardLeft;
            }
            else
            {
                direction = Direction.ForwardRight;
            }

            //Uupdate Facing.
            Facing = Facing.ToBearing(direction);
        }

        /// <summary>
        /// Reset Rover's Position & Facing to the last move made. Note: this
        /// counts as a move and does not place a new move on the stack.
        /// </summary>
        private void RevertToLastAction()
        {
            ++MoveCount;

            if (PreviousMoves.Count > 0)
            {
                ActionRecord lastMove = PreviousMoves.Pop();
                Position = lastMove.Position;
                Facing = lastMove.Facing;
            }
        }
    }
}
