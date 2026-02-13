using Godot;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PlanetPlayground.Game.Simulation;

public partial class Space : Node2D
{
	private ConcurrentBag<CelestialBody> Bodies { get; } = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	public void ClearChildren()
	{
        var exisitngChildren = Bodies;
        foreach (var child in exisitngChildren)
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        Bodies.Clear();
    }

    public void RegisterChild(CelestialBody body) 
	{
		if (Bodies.Contains(body)) return;
		Bodies.Add(body);
        //Forcing bodies to recalculate the current forces. 
        foreach (var existingBody in Bodies)
        {
			existingBody.RecalculateAcceleration();
        }
    }


}
