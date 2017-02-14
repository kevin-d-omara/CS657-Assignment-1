﻿using System;
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
        // todo

        public readonly List<Sequence> allowedMovementSequences;
        public readonly List<Sequence> allowedSonarSequences;

        public Rover(RoverParameters roverParams, Grid environment,
            GridParameters gridParameters)
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
            var keyInfo = Console.ReadKey();
            switch((char)keyInfo.Key)
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

            return action;

            //return new Action(Action.Type.Move, Direction.Forward);
        }

        private void TakeAction(Action action)
        {
            switch (action.type)
            {
                case Action.Type.Move:
                    Move(action.direction);
                    break;
                case Action.Type.Rotate:
                    Rotate(action.direction);
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
            // record action
            PreviousMoves.Push(new ActionRecord(Position, Facing));
            ++MoveCount;

            // update Position
            Bearing moveBearing = Facing.ToBearing(direction);
            Vector2 offset = moveBearing.ToCoordinateOffset();
            Position = new Vector2(Position.x + offset.x, Position.y + offset.y);

            // update Facing
            Facing = moveBearing;
        }

        /// <summary>
        /// Rotate the rover 45 degrees in the direction specified.
        /// </summary>
        /// <param name="direction">Direction relative to the Rover's facing to
        /// move.</param>
        private void Rotate(Direction direction)
        {
            // record action
            PreviousMoves.Push(new ActionRecord(Position, Facing));
            ++MoveCount;

            // update Facing
            Bearing moveBearing = Facing.ToBearing(direction);
            Facing = moveBearing;
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
