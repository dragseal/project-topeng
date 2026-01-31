using Godot;
using System;
using DialogueManagerRuntime;

public partial class DialogueTrigger : Area2D
{
	// Variable to identify which conversation to show
	// You can set this in the Inspector window
	[Export]
	public string DialogueId = "Dialogue1";

	// Should this trigger happen only once?
	[Export]
	public bool IsOneShot = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect the signal "BodyEntered" to our method
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print($"Starting dialogue: {DialogueId}");
			var dialogue = GD.Load<Resource>("res://Dialogue/"+ DialogueId +".dialogue");
			DialogueManager.ShowDialogueBalloon(dialogue, "start");
			if (IsOneShot)
			{
				QueueFree();
			}
	}
}
