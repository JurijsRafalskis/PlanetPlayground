using Godot;
using PlanetPlayground.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class CelestialBody : Node2D
{
    [Export]
    public float Mass { get; set; }
    [Export]
    public Vector2 Velocity { get; set; }
    [Export]
    public Vector2 Acceleration { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
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
		//GD.Print($"Process called for {this.GetType().Name}. Current Position: {Position}. Current Velocity: {Velocity}. Current Acceleration = {Acceleration}.");
		//Find out if we can separate drawing frame from physics frame.
		//Updating acceleration. Possibly  take an average acceleration too?
		float time = (float)delta;
		//Compare with previous acceleration too?
        Acceleration = CalculateAcceleration();
		Position += (Velocity + Acceleration * time / 2) * time;
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
			var distanceVector = body.Position - this.Position;
			var accelerationScalar = PhysicsConstants.GravitationalConstant * body.Mass /** this.Mass*/ / distanceVector.LengthSquared();
			var accelerationVector = distanceVector.Normalized() * accelerationScalar;
            accumulate += accelerationVector;
        }

		return accumulate;
	}
}
