using Godot;
using PlanetPlayground.Presets;
using System;
using System.Linq;

public partial class StartSimulation : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

    public override void _Pressed()
    {
		//Initializing new game
		var mainLoop = FindParent(nameof(MainLoop)) as MainLoop;
        var space = mainLoop.GetNode<Space>(nameof(Space));
		mainLoop.HideMenu();

        //Clean currently existing nodes.
        var exisitngChildren = space.GetChildren().OfType<CelestialBody>();
        foreach (var child in exisitngChildren)
        {
			space.RemoveChild(child);
			child.QueueFree();
        }
		//Loading the preset.
		var preset = TwoBodyPreset.CreateTwoBodyPreset(space);
		foreach(var body in preset)
		{
			space.AddChild(body);
        }
    }
}
