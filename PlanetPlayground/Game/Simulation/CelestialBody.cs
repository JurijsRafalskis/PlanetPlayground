using Godot;
using PlanetPlayground.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace PlanetPlayground.Game.Simulation;

public partial class CelestialBody : Node2D
{
    //Recache this on changes?
    private float _physiscsScalingFactor;
    private Vector2 _physicalPosition;

    [Export]
    public float Mass { get; set; }
	[Export]
	public Vector2 PhysicalPosition { 
		get => _physicalPosition; 
		set {
            Position = value * _physiscsScalingFactor;
            _physicalPosition = value; 
		} 
	}
    [Export]
    public Vector2 Velocity { get; set; }
    [Export]
    public Vector2 Acceleration { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _physiscsScalingFactor = (FindParent(nameof(GameLoop)) as GameLoop).PhysicalCoordinateScalingFactor;
#pragma warning disable CA2245 // Recalculating real position.
        PhysicalPosition = PhysicalPosition;
#pragma warning restore CA2245

        //Might be neccessary to call full recalc of those on creation?
        Acceleration = CalculateAcceleration();
		//Introduction of new Celestial body requires full reclculation of all in moment accelerations.
        foreach (var item in GetExistingSpaceBodies())
        {
			item.RecalculateAcceleration();
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        /*var texture = new ImageTexture();
        texture.SetImage();
        var image = new Image();
        image.loa*/
    }

    public override void _PhysicsProcess(double delta)
    {
        //Updating acceleration. Possibly  take an average acceleration too?
        float time = (float)delta;
        //Compare with previous acceleration too?
        Acceleration = CalculateAcceleration();
        PhysicalPosition += (Velocity + Acceleration * time / 2) * time;
        Velocity = Velocity + Acceleration * time;
    }

    public void RecalculateAcceleration() => Acceleration = CalculateAcceleration();

    private IEnumerable<CelestialBody> GetExistingSpaceBodies()
	{
        //Type operations are expensive? Maybe get some kind of bus or explicit list in the parent?
        return GetParent<Space>().GetChildren().OfType<CelestialBody>();
	}
	
	private Vector2 CalculateAcceleration()
	{
		//Itterate through all of the existing Celestial Bodies.
		var bodies = GetExistingSpaceBodies();
		Vector2 accumulate = Vector2.Zero;
		foreach(var body in bodies)
		{
			//Skipping this body by reference.
			if (body == this) continue;
			var distanceVector = body.PhysicalPosition - this.PhysicalPosition;
			var accelerationScalar = PhysicsConstants.GravitationalConstant * body.Mass /** this.Mass*/ / distanceVector.LengthSquared();
			var accelerationVector = distanceVector.Normalized() * accelerationScalar;
            accumulate += accelerationVector;
        }

		return accumulate;
	}
}
