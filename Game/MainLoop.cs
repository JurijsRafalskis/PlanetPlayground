using Godot;
using System;

public partial class MainLoop : Node
{
	/// <summary>
	/// Always retains the menu object, to attach and reattach.
	/// </summary>
	private MainMenu menu { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		menu = FindChild(nameof(MainMenu)) as MainMenu;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

    public override void _UnhandledInput(InputEvent @event)
    {
		//Returning menu if the escape is pressed.
        if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Escape)
		{
			ShowMenu();
		}
    }

	public void HideMenu()
	{
		if (menu.GetParent() == null) return;
		RemoveChild(menu);
	}

	public void ShowMenu()
	{
		if (FindChild(nameof(MainMenu)) is MainMenu) return;
		AddChild(menu);
	}
}
