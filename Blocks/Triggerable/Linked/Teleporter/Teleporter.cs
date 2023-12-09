using Godot;
using System;

public partial class Teleporter : AbstractLinked {
        private Vector2 location;
        private Sprite2D locator;
        
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Teleporter";
                locator = GetNode<Sprite2D>("Location");
                if(root is LevelEditor)
                        ((LevelEditor)root).Connect(LevelEditor.SignalName.NewEdit, new Callable(this, "HideLocator"));
                locator.Position = Vector2.Zero;
        }


        public override void Entered(Node2D activator) {
                base.Entered(activator);
                Warp((Player)activator);
        }


        private void Warp(Player player) {
                player.GlobalPosition = location;
        }

        public void SensorEntered(Node2D activator) {
                if(locator.Position == Vector2.Zero)
                        locator.GlobalPosition = location;
                if(activator is Player)
                        locator.Visible = true;
        }

        public void SensorExited(Node2D activator) {
                if(activator is Player)
                        locator.Visible = false;
        }

        public override void Edit() {
                base.Edit();
                locator.Visible = true;
                if(location != Vector2.Zero)
                        locator.GlobalPosition = location;
                Button button = ((LevelEditor)root).CreateButton("Reset Location");
                button.Connect(Button.SignalName.Pressed, new Callable(this, "ResetLocation"));
                HSlider sliderX = ((LevelEditor)root).CreateSlider(locator.GlobalPosition.X, "X Location", 32, 1504, 64);
                sliderX.Connect(HSlider.SignalName.ValueChanged, new Callable(this, "SetXLocation"));
                HSlider sliderY = ((LevelEditor)root).CreateSlider(locator.GlobalPosition.Y, "Y Location", 32, 864, 64);
                sliderY.Connect(HSlider.SignalName.ValueChanged, new Callable(this, "SetYLocation"));
        }


        public void SetXLocation(double value) {
                location = new Vector2((float)value, locator.GlobalPosition.Y);
                locator.GlobalPosition = location;
        }


        public void SetYLocation(double value) {
                location = new Vector2(locator.GlobalPosition.X, (float)value);
                locator.GlobalPosition = location;
        }


        public void ResetLocation() {
                HideLocator(null);
                locator.Position = new Vector2(0, 0);
                location = locator.GlobalPosition;
        }


        public void HideLocator(AbstractBlock block) {
                locator.Visible = false;
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Linked/Teleporter/teleporter.tscn",                     
                        ["TargetX"] = location.X,
                        ["TargetY"] = location.Y
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);
                Vector2 target = new Vector2((float)data["TargetX"], (float)data["TargetY"]);
                location = target;
        }
}
