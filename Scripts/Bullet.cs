using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    public Sprite2D _sprite; 
    public float _alpha = 1;
    public float timer = 0;
    //public Vector2 _velocity;
    //public Vector2 Velocity { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        timer += 1f;
        if (timer >= 360) {
            _alpha -= 0.2f;
            var color = _sprite.SelfModulate;
            color.A = _alpha;
            _sprite.SelfModulate = color;
            if (_alpha <= 0) {
                QueueFree();
            }
        }
        //GlobalPosition += _velocity * (float)delta;
        MoveAndSlide();
    }
}
