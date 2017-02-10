using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Driver
    {
        public static void Main(string[] args)
        {
            int width = 8;
            int height = 8;

            int startOffset = 0;
            int goalOffset = 0;
            Vector2 startPos = new Vector2(1 + startOffset, 1 + startOffset);
            Vector2 goalPos = new Vector2(width - goalOffset, height - goalOffset);

            float obstacleDensity = .25f;
            List<Cell.Type> obstacleTypes = new List<Cell.Type> { Cell.Type.Wall };

            GridParameters gridParams = new GridParameters(
                width, height, startPos, goalPos, obstacleDensity, obstacleTypes);
            Grid grid = new Grid(gridParams);

            grid.Display();



            Console.ReadKey();  // stop terminal from closing
        }
    }
}
