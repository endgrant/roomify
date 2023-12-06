using Godot;
using System;

public partial class JumpCharger : AbstractTriggerable {
        [ExportCategory("Attributes")]
        [Export(PropertyHint.Range, "0, 20")] protected int extraJumps = 3;


        // Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Jump Charger";
        }


        // Grant more jumps to the player
        private void GiveMoreJumps(Player player) {
                player.SetExtraJumps(extraJumps);
        }


        public override void Edit() {
                base.Edit();
                SpinBox slider = root.CreateIntSelector(extraJumps, "Jumps", 0, 20, 1);
                Callable callable = new Callable(this, "JumpsChanged");
                slider.Connect("value_changed", callable);
        }

        public void JumpsChanged(float value) {
                extraJumps = (int)value;
        }


        public override void Entered(Node2D activator) {
                if(activator is Player)
                GiveMoreJumps((Player)activator);
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/JumpCharger/jump_charger.tscn",
                        ["PosX"] = Position.X,
                        ["PosY"] = Position.Y,
                        ["Jumps"] = extraJumps
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);            
        }
}
