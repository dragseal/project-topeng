using Godot;
using System;
using DialogueManagerRuntime;

public partial class Player : CharacterBody2D
{
	public float Speed = 125f;
	//private Area2D _detectionArea;
	private bool _isTalking;
	private ShapeCast2D _shapeCast;
	private Vector2 _lastDirection = Vector2.Down; // Default facing down
												   // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shapeCast = GetNode<ShapeCast2D>("ShapeCast2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		_isTalking = GetTree().GetNodesInGroup("DialogueBalloon").Count > 0;
		// Dynamically get the Area2D of Object1
		var detectionArea = GetNodeOrNull<Area2D>("../Object/Area2D");
		var global = GetNode<Simpleton>("/root/Simpleton");

		if (detectionArea != null &&
				detectionArea.OverlapsBody(this) &&
				Input.IsActionJustPressed("interactKey") &&
				!_isTalking &&
				!global.playerDead
				)
		{
			var interactable = detectionArea.GetParent<Object>();
			if (interactable != null && !interactable.Interacted)
			{
				if (interactable.ObjectType == "merah") {
					global.merahTaken = true; 
					interactable.Interacted = true;
					var dialogue = GD.Load<Resource>("res://Dialogue/Object1.dialogue");
					DialogueManager.ShowDialogueBalloon(dialogue, "start");
					interactable.QueueFree();
				}
				if (interactable.ObjectType == "toy") {
					global.toyTaken = true; 
					interactable.Interacted = true;
					var dialogue = GD.Load<Resource>("res://Dialogue/Object2.dialogue");
					DialogueManager.ShowDialogueBalloon(dialogue, "start");
					interactable.QueueFree();
				}
				if (interactable.ObjectType == "mask") {
					global.maskTaken = true; 
					interactable.Interacted = true;
					var dialogue = GD.Load<Resource>("res://Dialogue/Object3.dialogue");
					DialogueManager.ShowDialogueBalloon(dialogue, "start");
					interactable.QueueFree();
				}
			}
		}
		Vector2 direction = Vector2.Zero;
		if (!_isTalking && !global.playerDead)
		{
			direction = Input.GetVector("moveLeft", "moveRight", "moveUp", "moveDown");
		}

		// Only update the "aim" if we are actually moving
		if (direction != Vector2.Zero)
		{
			_lastDirection = direction.Normalized();
		}

		// The ShapeCast now always points where you LAST moved
		_shapeCast.TargetPosition = _lastDirection * 10;

		Velocity = direction.Normalized() * Speed;
		MoveAndSlide();
		if (direction.X != 0)
		{
			var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			sprite.Scale = new Vector2(direction.X < 0 ? 1 : -1, 1);
		}
		JitterCheck();
		// Check if the ShapeCast hit anything
		if (_shapeCast.IsColliding())
		{
			for (int i = 0; i < _shapeCast.GetCollisionCount(); i++)
			{
				var collider = _shapeCast.GetCollider(i);
				//var global = GetNode<Simpleton>("/root/Simpleton");
				if (collider is Door hitDoor && Input.IsActionJustPressed("interactKey"))
				{
					if (hitDoor.RoomNumber == 1 && !global.merahTaken && !_isTalking) {
						var dialogue = GD.Load<Resource>("res://Dialogue/Locked1.dialogue");
						DialogueManager.ShowDialogueBalloon(dialogue, "start");
					} 
					if (hitDoor.RoomNumber == 0 || hitDoor.RoomNumber == 1 && global.merahTaken) {
						// Using 'NextRoom' as per your export
						var _nextRoom = GD.Load<PackedScene>(hitDoor.NextRoom).Instantiate();
						GetTree().Root.AddChild(_nextRoom);
						// Reparenting safely
						this.GetParent().CallDeferred(Node.MethodName.RemoveChild, this);
						_nextRoom.CallDeferred(Node.MethodName.AddChild, this);
						GetTree().CurrentScene.QueueFree();
						GetTree().CurrentScene = _nextRoom;
						this.GlobalPosition = new Vector2(hitDoor.SpawnX, hitDoor.SpawnY).Round();
						return; // STOP everything so we don't load the room twice
					}
					if (hitDoor.RoomNumber == -1 && !_isTalking) {
						if (global.maskTaken && global.toyTaken) // ENDING HERE
						{
							var dialogue = GD.Load<Resource>("res://Dialogue/Ending.dialogue");
							DialogueManager.ShowDialogueBalloon(dialogue, "start");
						}
						else {
							var dialogue = GD.Load<Resource>("res://Dialogue/Locked2.dialogue");
							DialogueManager.ShowDialogueBalloon(dialogue, "start");
						}
					}
				}
			}
		}
	}
	private Vector2 _oldVector = Vector2.Zero;
	private Vector2 _inputVector = Vector2.Zero;

	private void JitterCheck() {
		if (_oldVector != _inputVector) {
			_oldVector = _inputVector;
			if (_inputVector != Vector2.Zero) {
				Position = Position.Round();
			}
		}
	}
}
