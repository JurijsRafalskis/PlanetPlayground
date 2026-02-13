using Godot;
using PlanetPlayground.Configuration;
using PlanetPlayground.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PlanetPlayground.Game.Simulation;

public partial class Space : Node2D
{
    /// <summary>
    /// Collection of currently active celestial bodies.
    /// </summary>
    public IReadOnlyCollection<CelestialBody> CelestialBodies { get { return _bodies; } }

    /// <summary>
    /// The factor for transforming from physics coordinates to viewport coordinates.
    /// </summary>
    public float PhysicalCoordinateScalingFactor { get; private set; }

    private readonly ConcurrentBag<CelestialBody> _bodies = [];


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        SetScaling();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	public void ClearChildCelestialObjects()
	{
        var exisitngChildren = _bodies;
        foreach (var child in exisitngChildren)
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        _bodies.Clear();
    }

    public void RegisterCelestialObject(CelestialBody body) 
	{
		if (_bodies.Contains(body)) return;
		_bodies.Add(body);
        //Forcing bodies to recalculate the current forces. 
        foreach (var existingBody in _bodies)
        {
			existingBody.RecalculateAcceleration();
        }
    }

    private void SetScaling()
    {
        var viewport = GetViewport();
        var rectangle = viewport.GetVisibleRect();
        var width = rectangle.GetWidth();
        PhysicalCoordinateScalingFactor = width / PhysicsConstants.MaxVisibleX;
    }
}
