using Godot;
using System;

public partial class Teleporter : AbstractLinked {
        private Sprite2D locator;
        
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Teleporter";
                locator = GetNode<Sprite2D>("Location");
                ((LevelEditor)root).NewEdit += HideLocator;
        }


        public override void Entered(Node2D activator) {
                base.Entered(activator);
                Warp((Player)activator);
        }


        private void Warp(Player player) {
                player.Position = locator.GlobalPosition;
        }

        public override void Edit() {
                base.Edit();
                locator.Visible = true;
                Button button = ((LevelEditor)root).CreateButton("Reset Location");
                button.Pressed += ResetLocation;
                HSlider sliderX = ((LevelEditor)root).CreateSlider(GlobalPosition.X, "X Location", 32, 1504, 64);
                sliderX.ValueChanged += SetXLocation;
                HSlider sliderY = ((LevelEditor)root).CreateSlider(GlobalPosition.Y, "Y Location", 32, 864, 64);
                sliderY.ValueChanged += SetYLocation;
        }


        public void SetXLocation(double value) {
                locator.GlobalPosition = new Vector2((float)value, locator.GlobalPosition.Y);
        }


        public void SetYLocation(double value) {
                locator.GlobalPosition = new Vector2(locator.GlobalPosition.X, (float)value);
        }


        public void ResetLocation() {
                HideLocator(null);
                locator.Position = new Vector2(0, 0);
        }


        public void HideLocator(AbstractBlock block) {
                locator.Visible = false;
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Linked/Teleporter/teleporter.tscn",                     
                        ["TargetX"] = locator.GlobalPosition.X,
                        ["TargetY"] = locator.GlobalPosition.Y
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);            
        }
}
