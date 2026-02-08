using Godot;
using System;

public partial class MainLoop : Node
{
	private MainMenu Menu { get; set; }
	private PresetSelectionMenu SelectionMenu { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Menu = FindChild(nameof(MainMenu)) as MainMenu;
		SelectionMenu = FindChild(nameof(PresetSelectionMenu)) as PresetSelectionMenu;
		HideSelectionMenu();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

    public override void _UnhandledInput(InputEvent @event)
    {
		//Returning menu if the escape is pressed.
        if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Keycode == Key.Escape)
		{
			HideSelectionMenu();
            ShowMainMenu();
		}
    }

	public void HideMainMenu()
	{
		if (Menu.GetParent() == null) return;
		RemoveChild(Menu);
	}

	public void ShowMainMenu()
	{
		if (FindChild(nameof(MainMenu)) is MainMenu) return;
		AddChild(Menu);
	}

	public void HideSelectionMenu()
	{
        if(SelectionMenu.GetParent() == null) return;
        RemoveChild(SelectionMenu);
    }

	public void ShowSelectionMenu()
	{
        if (FindChild(nameof(PresetSelectionMenu)) is PresetSelectionMenu) return;
        AddChild(SelectionMenu);
    }
}
