using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class GridParameters
    {
        // All fields set to default value.
        public int width = 30;
        public int height = 30;

        public Vector2 startPos = new Vector2(1, 1);
        public Vector2 goalPos = new Vector2(30, 30);

        public float obstacleDensity = .25f;
        public List<Cell.Type> obstacleTypes = new List<Cell.Type>
            { Cell.Type.Wall };

        /// <summary>
        ///  Default constructor: all fields initialized to default values (see class definition).
        /// </summary>
        public GridParameters() { }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="width">Width of the grid.</param>
        /// <param name="height">Height of the grid.</param>
        /// <param name="startPosition">Rover start position w/ top left at [1,1].</param>
        /// <param name="endPosition">Rover goal position w/ bottom right at [width, height].</param>
        /// <param name="obstacleDensity">Percent of tiles with an obstacle. (value = 0.0 to 1.0)</param>
        /// <param name="obstacleTypes">Types of obstacles to randomize between, i.e. Rock, Pit, etc.</param>
        public GridParameters(
            int width, int height,
            Vector2 startPosition, Vector2 endPosition,
            float obstacleDensity,List<Cell.Type> obstacleTypes)
        {
            this.width = width;
            this.width = width;
            this.height = height;
            this.startPos = new Vector2(startPos.x, startPos.y);
            this.goalPos = new Vector2(goalPos.x, goalPos.y);
            this.obstacleDensity = obstacleDensity;
            this.obstacleTypes = new List<Cell.Type>();
            foreach(Cell.Type type in obstacleTypes)
            {
                this.obstacleTypes.Add(type);
            }
        }
    }
}
