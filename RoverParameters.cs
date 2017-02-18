using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class RoverParameters
    {
        // All fields set to default value.
        public Bearing facing = Bearing.SouthEast;
        public List<Sequence> allowedMovementSequences = new List<Sequence>
        {
            new Sequence(Direction.Forward, Sequence.Mode.Terminate, Sequence.Type.Movement),
            new Sequence(Direction.ForwardRight, Sequence.Mode.Terminate, Sequence.Type.Movement),
            new Sequence(Direction.ForwardLeft, Sequence.Mode.Terminate, Sequence.Type.Movement)
        };
        public List<Sequence> allowedSonarSequences = new List<Sequence>
        {
            new Sequence(Direction.Forward, Sequence.Mode.Repeat, Sequence.Type.Sonar),
            new Sequence(Direction.ForwardRight, Sequence.Mode.Repeat, Sequence.Type.Sonar),
            new Sequence(Direction.ForwardLeft, Sequence.Mode.Repeat, Sequence.Type.Sonar)
        };

        /// <summary>
        /// Default constructor. all fields initialized to default values (see
        /// class definition).
        /// </summary>
        public RoverParameters() { }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        public RoverParameters(
            Bearing initialFacing,
            List<Sequence> allowedMovement, List<Sequence> allowedSonar)
        {
            facing = initialFacing;

            // Deep copy lists.
            allowedMovementSequences = new List<Sequence>();
            foreach (Sequence sequence in allowedMovement)
            {
                allowedMovementSequences.Add(sequence);
            }
            allowedSonarSequences = new List<Sequence>();
            foreach (Sequence sequence in allowedSonar)
            {
                allowedSonarSequences.Add(sequence);
            }
        }
    }
}
