using Godot;
using PlanetPlayground.Configuration;
using PlanetPlayground.Extensions;
using PlanetPlayground.Game.Interface;
using System;
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

    public Vector2 PositionOffset { get => _currentPositionOffset; private set {
            _coordinatesWritten = false;
            _currentPositionOffset = value;
        }
    }

    private Vector2 _accumulatedPositionOffset = Vector2.Zero;

    private readonly ConcurrentBag<CelestialBody> _bodies = [];
    private CoordinatesLabel _coordinates;
    private bool _coordinatesWritten = true; 
    private Vector2 _currentPositionOffset = new ();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        SetScaling();
        _coordinates = GetParent<GameLoop>().FindChild(nameof(CoordinatesLabel)) as CoordinatesLabel;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is not InputEventKey eventKey) return;
        if (!eventKey.Pressed) return;
        Vector2 accumulate = Vector2.Zero;
        if (eventKey.IsAction(InputMapping.Up)) accumulate += Vector2.Up;
        if (eventKey.IsAction(InputMapping.Down)) accumulate += Vector2.Down;
        if (eventKey.IsAction(InputMapping.Left)) accumulate += Vector2.Left;
        if (eventKey.IsAction(InputMapping.Right)) accumulate += Vector2.Right;
        _accumulatedPositionOffset = accumulate;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
    {
        if (!_coordinatesWritten) FlushCoordinates(); //Every physics frame? Maybe on input reading?
    }

    public override void _PhysicsProcess(double delta)
    {
        //Should we round this here?
        PositionOffset +=  _accumulatedPositionOffset * (float)(PhysicsConstants.ScrollSpeed * delta);
        _accumulatedPositionOffset = Vector2.Zero;
    }

	public void ClearChildCelestialObjects()
	{
        var exisitngChildren = _bodies;
        foreach (var child in exisitngChildren)
        {
            this.RemoveChild(child);
            child.QueueFree();
        }
        _bodies.Clear();
        PositionOffset = new Vector2();
        FlushCoordinates();
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

    private void FlushCoordinates()
    {
        _coordinatesWritten = true;
        _coordinates.Text = $"X: {Math.Round(PositionOffset.X)}  Y: {Math.Round(PositionOffset.Y)}";
    }

    private void SetScaling()
    {
        var viewport = GetViewport();
        var rectangle = viewport.GetVisibleRect();
        var width = rectangle.GetWidth();
        PhysicalCoordinateScalingFactor = width / PhysicsConstants.MaxVisibleX;
    }
}
