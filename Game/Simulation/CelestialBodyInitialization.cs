using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetPlayground.Game.Simulation;

public class CelestialBodyInitialization
{
    /// <summary>
    /// Initial position by coordinate X in % of screen.
    /// </summary>
    public float PlacementX { get; set; }
    /// <summary>
    /// Initial position by coordinate Y in % of screen.
    /// </summary>
    public float PlacementY { get; set; }
    /// <summary>
    /// Mass of the Body
    /// </summary>
    public float Mass { get; set; }
    /// <summary>
    /// Initial velocity by coordinate X in % of screen.
    /// </summary>
    public float VelocityX { get; set; }
    /// <summary>
    /// Initial velocity by coordinate Y in % of screen.
    /// </summary>
    public float VelocityY { get; set; }
}
