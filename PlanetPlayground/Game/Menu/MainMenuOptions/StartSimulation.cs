using Godot;
using PlanetPlayground.Game;

namespace PlanetPlayground.Game.Menu.MainMenuOptions;

public partial class StartSimulation : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

    public override void _Pressed()
    {
		//Initializing new game
		var mainLoop = FindParent(nameof(GameLoop)) as GameLoop;
		mainLoop.HideMainMenu();
		mainLoop.ShowSelectionMenu();
    }
}
