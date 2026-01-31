using Godot;
using System;
using Godot.Collections;

public partial class World : Node2D
{
	// Tambahkan Singleton agar mudah diakses oleh DialogueManager dan script lain
	public static World Instance { get; private set; }

	[Export] public PackedScene PlayerScene;
	[Export] public PackedScene GhostScene;

	public override void _Ready()
	{
		Instance = this; // Inisialisasi singleton

		var global = GetNode<Simpleton>("/root/Simpleton");
		if (!global.playerSpawned)
		{
			SpawnPlayer(245, 64);
			global.playerSpawned = true;
		}
	}

	public override void _Process(double delta)
	{
		// Logic debug Anda tetap sama
		if (Input.IsActionJustPressed("debugKey")) 
		{
			var marker = GetNode<Node>("/root/MarkerManager");
			var spawnPos = (Vector2)marker.Call("GetMarkerPosition", "Marker1");
			SpawnGhost(spawnPos.X, spawnPos.Y);
		}
	}

	public void SpawnPlayer(float x, float y)
	{
		if (PlayerScene == null) return;
		var player = PlayerScene.Instantiate<CharacterBody2D>();
		AddChild(player);
		player.Position = new Vector2(x, y);
	}

	// Method ini sekarang siap dipanggil dari DialogueManager
	public void SpawnGhost(float x, float y)
	{
		if (GhostScene == null) 
		{
			GD.PrintErr("GhostScene belum dimasukkan di Inspector!");
			return;
		}
		
		var ghost = GhostScene.Instantiate<Area2D>();
		ghost.GlobalPosition = new Vector2(x, y);
		AddChild(ghost);
		GD.Print($"Ghost berhasil muncul di: {x}, {y}");
	}
}
