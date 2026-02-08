using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetPlayground.Presets;

public static class TwoBodyPreset
{
    public static IEnumerable<CelestialBody> CreateTwoBodyPreset(Space space)
    {
        //Get viewPort width
        var viewPort = space.GetViewportRect();

        List<CelestialBody> result = new();
        CelestialBody body = new()
        {
            Position = new(viewPort.Position.X / 3, viewPort.Position.Y / 2),
            Mass = 1,
            Velocity = new(0, 40)
        };
        result.Add(body);

        body = new()
        {
            Position = new(2 * viewPort.Position.X / 3, viewPort.Position.Y / 2),
            Mass = 1,
            Velocity = new(0, -40)
        };
        result.Add(body);
        return result;
    }
}
