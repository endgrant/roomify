using Godot;
using System;
using System.ComponentModel;

public partial class BasicBlock : AbstractBasic {
        // Entered scene tree
        public override void _Ready() {
                base._Ready();
                displayName = "Block";
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Basic/BasicBlock/basic_block.tscn"
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);  
        }
}
