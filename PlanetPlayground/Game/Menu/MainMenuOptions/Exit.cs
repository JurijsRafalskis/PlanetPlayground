using Godot;
using System;

namespace PlanetPlayground.Game.Menu.MainMenuOptions;

public partial class Exit : Button
{
	private Action<int> Quit { get; set; } = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Quit = GetTree().Quit;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Pressed()
    {
		Quit(0);
    }
}
