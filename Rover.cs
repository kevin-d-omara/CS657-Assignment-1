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
        // Events & Delegates
        public delegate void UsedSonar();
        public static event UsedSonar OnUsedSonar;
        public delegate void NoPathFound();
        public static event NoPathFound OnNoPathFound;

        // State
        public Vector2 Position { get; private set; }
        public Bearing Facing { get; private set; }

        public int MoveCount { get; private set; } = 0;
        public Dictionary<string, int> MoveBreakdown { get; private set; }
        public Stack<MoveRecord> PreviousMoves { get; private set; }
            = new Stack<MoveRecord>();
        public Queue<ActionRecord> ActionHistory { get; private set; }
            = new Queue<ActionRecord>();

        // Database
        public Grid Grid { get; private set; }
        public HashSet<Cell> DetectedCells { get; private set; }
            = new HashSet<Cell>();

        // Actual environment being traversed: used for Sonar detection.
        public readonly Grid environment;

        // Intelligent Control
        public readonly List<Sequence> allowedMovementSequences;
        public readonly List<Sequence> allowedSonarSequences;

        // Nested Classes
        public class MoveRecord
        {
            public Vector2 Position { get; private set; }
            public Bearing Facing { get; private set; }
            public MoveRecord(Vector2 position, Bearing facing)
            {
                Position = position;
                Facing = facing;
            }
        }

        public class ActionRecord
        {
            public Action.Type Action { get; private set; }
            public Vector2 Position { get; private set; }
            public Bearing Facing { get; private set; }
            public ActionRecord(Action.Type action, Vector2 position,
                Bearing facing)
            {
                Action = action;
                Position = position;
                Facing = facing;
            }
        }

        public Rover(RoverParameters roverParams, Grid environment)
        {
            this.environment = environment;

            var emptyGridParameters = new GridParameters(
                environment.width, environment.height,
                environment.startPosition, environment.goalPosition,
                0.0f, environment.obstacleTypes);

            Grid = new Grid(emptyGridParameters);
            Position = Grid.startPosition;
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

            // Data initialization for Database
            MoveBreakdown = new Dictionary<string, int>();
            MoveBreakdown.Add("Move_Forward", 0);
            MoveBreakdown.Add("Move_ForwardLeft", 0);
            MoveBreakdown.Add("Move_ForwardRight", 0);
            MoveBreakdown.Add("Rotate_Left", 0);
            MoveBreakdown.Add("Rotate_Right", 0);
            MoveBreakdown.Add("Revert_Move", 0);
            MoveBreakdown.Add("Revert_Rotate", 0);
            
            // Record starting location.
            ActionHistory.Enqueue(new ActionRecord(Action.Type.Start, Position,
                Facing));
        }

        public void Update()
        {
            DetectEnvironmentWithSonar();
            // Pause before advancing.
            Console.ReadKey();

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

                    if (!DetectedCells.Contains(cell))
                    {
                        DetectedCells.Add(cell);
                    }
                }
            }

            if (OnUsedSonar != null) { OnUsedSonar(); }
        }

        /// <summary>
        /// Consult Expert AI or User Input to determine the next action.
        /// </summary>
        private Action ChooseAction()
        {
            Action action;

            // Run A* search.
            var aStarSearch = new AStarSearch(Grid, Position, Facing,
                Grid.goalPosition, PreviousMoves);
            var shortestPath = aStarSearch.GetShortestPath();

            // Exit early if no path exists to the Goal.
            if (shortestPath.Count == 0)
            {
                if (OnNoPathFound != null) { OnNoPathFound(); }
                return new Action(Action.Type.Wait, Direction.Forward);
            }

            // Determine Direction relative to the Rover of the first move.
            var nextNode = shortestPath.Peek();
            var offset = new Vector2(nextNode.pos.x - Position.x,
                nextNode.pos.y - Position.y);
            var direction = Facing.ToDirection(offset);

            // Determine if nextNode is best reached by a Revert.
            var shouldRevert = false;
            foreach (Path path in nextNode.Paths)
            {
                if (path.wasRevertAction)
                {
                    shouldRevert = true;
                    break;
                }
            }

            // Consult Expert System Rules to determine the Action.
            if (shouldRevert)
            {
                action = new Action(Action.Type.Revert, Direction.Forward);
            }
            else if (direction == Direction.Forward
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
                case Action.Type.Wait:
                    // Rover does nothing.
                    ++MoveCount;
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
            PreviousMoves.Push(new MoveRecord(Position, Facing));
            ++MoveCount;
            switch(direction)
            {
                case Direction.Forward:
                    ++MoveBreakdown["Move_Forward"];
                    break;
                case Direction.ForwardLeft:
                    ++MoveBreakdown["Move_ForwardLeft"];
                    break;
                case Direction.ForwardRight:
                    ++MoveBreakdown["Move_ForwardRight"];
                    break;
                default:
                    break;
            }

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

            ActionHistory.Enqueue(new ActionRecord(Action.Type.Move, Position,
                Facing));
        }

        /// <summary>
        /// Rotate the rover 45 degrees in the direction specified.
        /// </summary>
        /// <param name="direction">Direction relative to the Rover's facing to
        /// move.</param>
        private void Rotate45(Direction direction)
        {
            // Record Action.
            PreviousMoves.Push(new MoveRecord(Position, Facing));
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
            switch (direction)
            {
                case Direction.ForwardLeft:
                    ++MoveBreakdown["Rotate_Left"];
                    break;
                case Direction.ForwardRight:
                    ++MoveBreakdown["Rotate_Right"];
                    break;
                default:
                    break;
            }

            //Uupdate Facing.
            Facing = Facing.ToBearing(direction);

            ActionHistory.Enqueue(new ActionRecord(Action.Type.Rotate, Position,
                Facing));
        }

        /// <summary>
        /// Reset Rover's Position & Facing to the last move made. Note: this
        /// counts as a move and does not place a new move on the stack.
        /// </summary>
        private void RevertToLastAction()
        {
            // Record Action
            ++MoveCount;

            if (PreviousMoves.Count > 0)
            {
                MoveRecord lastMove = PreviousMoves.Pop();

                if (lastMove.Position == Position)
                {
                    ++MoveBreakdown["Revert_Rotate"];
                }
                else
                {
                    ++MoveBreakdown["Revert_Move"];
                }

                Position = lastMove.Position;
                Facing = lastMove.Facing;
            }

            ActionHistory.Enqueue(new ActionRecord(Action.Type.Revert, Position,
                Facing));
        }
    }
}
