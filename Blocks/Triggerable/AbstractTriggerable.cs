using Godot;
using System;

public abstract partial class AbstractTriggerable : AbstractBlock {
	// Method used by all Triggerable blocks interact with an object through signals
	public abstract void Entered(Node2D activator);

	public override string Save() {
        throw new NotImplementedException();
    }
}
