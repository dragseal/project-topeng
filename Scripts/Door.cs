using Godot;
using System;

public partial class Door : StaticBody2D
{

	[Export] public string NextRoom;
	[Export] public int RoomNumber;
	[Export] public float SpawnX;
	[Export] public float SpawnY;
	// GetTree().ChangeSceneToPacked(NextRoom);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var global = GetNode<Simpleton>("/root/Simpleton");
		if (Name == "DoorFinal") {
			if (global.maskTaken && global.toyTaken) {
				Modulate = Colors.Green;
			}
			else {
				Modulate = Colors.Orange;
			}
		}
		else {
			Modulate = Colors.Green;
			if (!global.merahTaken) {
				Modulate = Colors.Orange;
			}
		}
	}
}
