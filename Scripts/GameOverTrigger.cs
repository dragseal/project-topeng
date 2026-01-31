using Godot;
using System;

public partial class GameOverTrigger : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void ShowGameOverScreen() {
        var global = GetNode<Simpleton>("/root/Simpleton");
        if (!global.playerDead) {
            global.playerDead = true;
            var scene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
            var _gameOver = scene.Instantiate<CanvasLayer>();
            var _world = GetNode<Node2D>("/root/World");
            _world.AddChild(_gameOver);
        }
    }
}
