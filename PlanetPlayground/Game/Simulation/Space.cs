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

    private readonly ConcurrentBag<CelestialBody> _bodies = [];
    private CoordinatesLabel _coordinates;
    private bool _coordinatesWritten = true; 
    private Vector2 _currentPositionOffset = new ();

    private bool _upPressed = false, _downPressed = false, _leftPressed = false, _rightPressed = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        SetScaling();
        _coordinates = GetParent<GameLoop>().FindChild(nameof(CoordinatesLabel)) as CoordinatesLabel;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is not InputEventKey eventKey) return;
        if (!eventKey.Pressed) return;
        _upPressed |= eventKey.IsAction(InputMapping.Up);
        _downPressed |= eventKey.IsAction(InputMapping.Down);
        _leftPressed |= eventKey.IsAction(InputMapping.Left);
        _rightPressed |= eventKey.IsAction(InputMapping.Right);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
    {
        if (!_coordinatesWritten) FlushCoordinates(); //Every physics frame? Maybe on input reading?
    }

    public override void _PhysicsProcess(double delta)
    {
        //Keypresses invered.
        var accumulatedOffset = 
            (_upPressed ? Vector2.Down : Vector2.Zero) + 
            (_downPressed ? Vector2.Up : Vector2.Zero) + 
            (_leftPressed ? Vector2.Right : Vector2.Zero) + 
            (_rightPressed ? Vector2.Left : Vector2.Zero);
        //Should we round this here?
        PositionOffset += accumulatedOffset * (float)(PhysicsConstants.ScrollSpeed * delta);
        _upPressed = _downPressed = _leftPressed = _rightPressed = false;
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
