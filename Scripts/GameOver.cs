using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	private float state = 0;
	private float timer = 0;

	public override void _Process(double delta)
	{
		var global = GetNode<Simpleton>("/root/Simpleton");

		if (global.playerDead && state == 0) 
		{
		   state = 1;
		}
		else if (state == 1) 
		{
			timer += 1f;
			// Menunggu 60 frame atau tombol interaksi ditekan
			if (timer >= 60 || Input.IsActionJustPressed("interactKey")) 
			{
				timer = 0;
				state = 2;
				Label label = new Label();
				label.Text = "GAME OVER\nTekan Interact untuk Menu";
				AddChild(label);
				label.GlobalPosition = new Vector2(100, 100);
			}
		}
		else if (state == 2) 
		{
			timer += 1f;
			if (timer >= 10) 
			{
				if (Input.IsActionJustPressed("interactKey")) 
				{
					// 1. Unpause game jika sebelumnya di-pause
					GetTree().Paused = false;
					
					// 2. Reset semua stats (termasuk playerDead = false)
					global.ResetStats();
					
					// 3. Pindah ke Scene Main Menu
					// Pastikan path filenya sesuai dengan project Anda
					GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
				}
			}
		}
	}
}
