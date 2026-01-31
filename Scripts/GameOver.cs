using Godot;
using System;

public partial class GameOver : CanvasLayer
{
    private float state = 0;
    private float timer = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        var global = GetNode<Simpleton>("/root/Simpleton");
        if (global.playerDead && state == 0) {
           state = 1;
        }
        else if (state == 1) {
            timer+= 1f;
            if (timer == 60 || Input.IsActionJustPressed("interactKey")) {
                timer = 0;
                state = 2;
                Label label = new Label();
                label.Text = "Hello World";
                AddChild(label);
                label.GlobalPosition = new Vector2(100, 100);
            }
        }
        else if (state == 2) {
            timer += 1f;
            if (timer >= 10) {
                if (Input.IsActionJustPressed("interactKey")) {
                    GetTree().Paused = false;
                    GetTree().ReloadCurrentScene();
                    global.ResetStats();
                }
            }
        }
    }
}
