using Godot;
using System;
using DialogueManagerRuntime;

public partial class DialogueTrigger : Area2D
{
	[Export]
	public string DialogueId = "Dialogue1";
	[Export]
	public bool IsOneShot = true;

	public override void _Ready()
	{
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
