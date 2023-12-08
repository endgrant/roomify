using Godot;
using System;

// In Godot Engine, autoloaded scripts can be used to make singleton objects.
// Since Godot uses the ready function instead of constructors the ready function
// is always public, singletons can't be created in the normal sense, but are
// instead implied through the game engine itself.
// This object is connected to an autoloaded script
public partial class ToggleHandler : ToggleSwitch {
    [ExportCategory("Attributes")]
	[Export] protected static bool isRed = true;
    	protected static Texture2D toggleSprite = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png");
    public static ToggleHandler instance;
    protected static Timer timer;

    public override void _Ready() {
        instance = this;
        base._Ready();
        timer = GetNode<Timer>("Timer");
    }

    public void ResetToggle() {
        isRed = true;
    }

    public void RequestToggle(bool isRed) {
		EmitSignal(SignalName.Toggle, isRed);
	}

    public bool IsTimerStopped() {
        return timer.IsStopped();
    }

    public void StartTimer() {
        timer.Start();
    }

    public bool GetIsRed() {
		return isRed;
	}

	public void SetIsRed(bool value) {
		isRed = value;
		RequestToggle(isRed);
	}
}
