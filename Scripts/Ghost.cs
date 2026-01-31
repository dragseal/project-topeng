using Godot;
using System;

public partial class Ghost : Area2D
{
    [Export] public float Speed;
	Vector2 Position;
    [Export] public float Amplitude = 20.0f;
    [Export] public float Frequency = 2.0f;
    private float _startY;
    private double _time;
    private Sprite2D childNode;

    public float timer = 0;


    public override void _Ready()
	{
        _startY = Position.Y;
        Position = GlobalPosition;
        BodyEntered += OnBodyEntered;
        childNode = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
        var marker = GetNode<Node>("/root/MarkerManager");
        var playerPos = (Vector2)marker.Call("GetMarkerPosition", "PlayerMarker");
        
        timer += 1f;
        if (timer % 30 == 0) {
            var _scene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
            CreateBullets(_scene, GlobalPosition, playerPos, 100f);
        }


        //Position = Position.MoveToward(playerPos, (float)delta * Speed);
	    GlobalPosition = Position;
        _time += delta;
        float offset = Mathf.Sin((float)_time * Frequency) * Amplitude;
        childNode.Position = new Vector2(childNode.Position.X, _startY + offset);
    }
    private void OnBodyEntered(Node body)
    {
        var global = GetNode<Simpleton>("/root/Simpleton");
        if (body is Player player && !global.playerDead) {
            global.playerDead = true;
            var scene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
            var _gameOver = scene.Instantiate<CanvasLayer>();
            AddChild(_gameOver);
        }
    }
    public void CreateBullets(PackedScene scene, Vector2 pos, Vector2 target, float speed)
    {
        var instance = scene.Instantiate<Bullet>();
        GetTree().Root.AddChild(instance);
        instance.GlobalPosition = pos;
        instance.LookAt(target);
        instance.Velocity = instance.Transform.X * speed;
    }
}
