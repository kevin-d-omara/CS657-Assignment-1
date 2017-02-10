using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    // Representation of 2D vectors and points.
    // Limited reproduction of the Vector2 class offered by Unity3D:
    // https://docs.unity3d.com/ScriptReference/Vector2.html
    public class Vector2
    {
        public readonly float x;
        public readonly float y;

        public Vector2(float X, float Y)
        {
            x = X;
            y = Y;
        }
    }
}