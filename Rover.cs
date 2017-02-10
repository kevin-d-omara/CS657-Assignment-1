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
        public Stack<Vector2> Moves { get; private set; } = new Stack<Vector2>();

        // Database
        private Grid grid;

        // Intelligent Control
        // todo

        // Movement Set
        // todo

        // Sonar Set
        // todo

        public Rover()
        {

        }

        public void Update()
        {
            // DetectWithSonar()
            // DecideOnMove()
            MakeMove();
        }

        private void MakeMove()
        {
            // update Position
            // update Facing
            ++MoveCount;
            // update Moves
        }
    }
}
