using Godot;
using System;
[Tool]

public abstract partial class AbstractMenu : Control {
    [Signal]
    public delegate void MenuChangedEventHandler(int menuType);
    protected static AbstractMenu previousMenu;
    protected static string lastButtonPressed;

    public override void _Ready() {
        base._Ready();
        Connect(SignalName.MenuChanged, new Callable(PauseOverlay.instance, "ChangedMenu"));
        ToggleHandler.instance.SetIsRed(true);
    }
}
