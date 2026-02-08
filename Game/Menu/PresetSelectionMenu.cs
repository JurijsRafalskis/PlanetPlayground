using Godot;
using PlanetPlayground.Configuration;
using PlanetPlayground.Presets;
using System;
using System.Linq;

public partial class PresetSelectionMenu : VBoxContainer
{
	private static readonly PackedScene PresetSelectionButtonScene = GD.Load<PackedScene>("res://Game/Menu/PresetSelectionButton.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Creating neccessary preset buttons.
		foreach(var preset in Presets.ExistingPresets)
		{
			var button = PresetSelectionButtonScene.Instantiate<PresetSelectionButton>();
			button.PresetName = preset.PresetName;
			button.Text = preset.Label;
			AddChild(button);
        }
        Position = (GetViewportRect().Size - this.Size) / 2;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	public void InitializeSimulation(string presetName)
	{
		var mainLoop = GetParent<MainLoop>();
        var space = mainLoop.GetNode<Space>(nameof(Space));
        mainLoop.HideSelectionMenu();
		mainLoop.HideMainMenu();
        var exisitngChildren = space.GetChildren().OfType<CelestialBody>();
        foreach (var child in exisitngChildren)
        {
            space.RemoveChild(child);
            child.QueueFree();
        }
        var preset = PresetFactory.CreateFromPreset(space, presetName);
        foreach (var body in preset)
        {
            space.AddChild(body);
        }
    }
}
