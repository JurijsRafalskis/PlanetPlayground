using Godot;
using PlanetPlayground.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace PlanetPlayground.Game.Simulation;

public partial class CelestialBody : Node2D
{
    //Recache this on changes?
    private float _physiscsScalingFactor = 1;
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

    private Space Parent { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        Parent = GetParent<Space>();
        //Is this reasonable, or should we just use the parent directly?
        _physiscsScalingFactor = Parent.PhysicalCoordinateScalingFactor;
#pragma warning disable CA2245 // Forcing recalculation of the real position.
        PhysicalPosition = PhysicalPosition;
#pragma warning restore CA2245
        //This will recalcculate acceleration
        Parent.RegisterCelestialObject(this);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {}

    public override void _PhysicsProcess(double delta)
    {
        //Updating acceleration. Possibly  take an average acceleration too?
        float time = (float)delta;
        //Compare with previous acceleration too?
        RecalculateAcceleration();
        PhysicalPosition += (Velocity + Acceleration * time / 2) * time;
        Velocity = Velocity + Acceleration * time;
    }

    public void RecalculateAcceleration() => Acceleration = CalculateAcceleration();
	
	private Vector2 CalculateAcceleration()
	{
		//Itterate through all of the existing Celestial Bodies.
		Vector2 accumulate = Vector2.Zero;
		foreach(var body in Parent.CelestialBodies)
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
