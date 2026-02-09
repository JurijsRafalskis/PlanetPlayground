using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetPlayground.Extensions;

public static class RectExtensions
{
    public static float GetWidth(this Rect2 rect)
    {
        return Math.Abs(rect.End.X - rect.Position.X);
    }

    public static float GetHeight(this Rect2 rect)
    {
        return Math.Abs(rect.End.Y - rect.Position.Y);
    }
}
