using System.Diagnostics;

namespace PlanetPlayground.Game.Simulation;

[DebuggerDisplay("Seed: '{SpriteSeed}', Mass: {Mass}")]
public class CelestialBodyInitialization
{
    /// <summary>
    /// Initial position by physical coordinate in X assis. Visible values 0-10000.
    /// </summary>
    public float PlacementX { get; set; }
    /// <summary>
    /// Initial position by physical coordinate in Y assis. Visible values 0-10000.
    /// </summary>
    public float PlacementY { get; set; }
    /// <summary>
    /// Mass of the Body
    /// </summary>
    public float Mass { get; set; }
    /// <summary>
    /// Initial velocity by X assis.
    /// </summary>
    public float VelocityX { get; set; }
    /// <summary>
    /// Initial velocity by Y assis.
    /// </summary>
    public float VelocityY { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string SpriteSeed { get; set; }
}
