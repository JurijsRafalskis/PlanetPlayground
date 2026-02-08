using Godot;
using PlanetPlayground.Extensions;
using System;

public partial class MainMenu : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Position = (GetViewportRect().Size  - this.Size) / 2;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
