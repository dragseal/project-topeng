using Godot;
using System;

public partial class GameOverTrigger : Node
{

	public void ShowGameOverScreen() 
	{
		var global = GetNode<Simpleton>("/root/Simpleton");
		
		if (!global.playerDead) 
		{
			global.playerDead = true;
			
			Error result = GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
			
			if (result != Error.Ok)
			{
				GD.PrintErr("Gagal memuat scene GameOver: " + result);
			}
		}
	}

}
