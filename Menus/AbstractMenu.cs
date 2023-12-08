using Godot;
using System;
[Tool]

public abstract partial class AbstractMenu : Control {
    protected static AbstractMenu previousMenu;
    protected static string lastButtonPressed;
}
