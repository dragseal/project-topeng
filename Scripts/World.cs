using Godot;
using System;
using Godot.Collections;
public partial class World : Node2D
{
	// 1. Export the Player scene so you can drag the Player.tscn here
	[Export] public PackedScene PlayerScene;
	[Export] public PackedScene GhostScene;

	public override void _Ready()
	{
		var global = GetNode<Simpleton>("/root/Simpleton");
		if (!global.playerSpawned)
		{
			SpawnPlayer(245, 64); // Example coordinates
			global.playerSpawned = true;
		}
	}
	public override void _Process(double delta)
	{
		var marker = GetNode<Node>("/root/MarkerManager");
		var spawnPos = (Vector2)marker.Call("GetMarkerPosition", "Marker1");
		if (Input.IsActionJustPressed("debugKey")) {
		   SpawnGhost(spawnPos.X, spawnPos.Y);
		}
	}
	public void SpawnPlayer(float x, float y)
	{
		// 2. Create the instance
		var player = PlayerScene.Instantiate<CharacterBody2D>();
		// 3. Set the position BEFORE adding it to the tree
		player.GlobalPosition = new Vector2(x, y);
		// 4. Add it to the current room
		AddChild(player);
	}
	public void SpawnGhost(float x, float y)
	{
		// 2. Create the instance
		var ghost = GhostScene.Instantiate<Area2D>();
		// 3. Set the position BEFORE adding it to the tree
		ghost.GlobalPosition = new Vector2(x, y);
		// 4. Add it to the current room
		AddChild(ghost);
	}


}
