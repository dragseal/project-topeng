using Godot;
using System;

public partial class Object : Node2D
{
	[Export] public string ObjectType;
	public bool Interacted = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var global = GetNode<Simpleton>("/root/Simpleton");
		if (ObjectType == "merah" && global.merahTaken) {
			QueueFree();
		}
		if (ObjectType == "toy" && global.toyTaken) {
			QueueFree();
		}
		if (ObjectType == "mask" && global.maskTaken) {
			QueueFree();
		}
		Interacted = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		 //var _isTalking = GetTree().GetNodesInGroup("DialogueBalloon").Count > 0;
		if (Interacted) {
			QueueFree();
		}
	}
}
