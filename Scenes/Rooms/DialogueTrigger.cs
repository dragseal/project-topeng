using Godot;
using System;
using DialogueManagerRuntime;

public partial class DialogueTrigger : Area2D
{
	[Export] public string DialogueId = "Dialogue1";
	[Export] public bool IsOneShot = true;

	public override void _Ready()
	{
		var global = GetNode<Simpleton>("/root/Simpleton");

		// Cek apakah ID ini sudah pernah diaktifkan sebelumnya
		// Pastikan di Simpleton.cs Anda punya: public System.Collections.Generic.HashSet<string> ActivatedTriggers = new();
		if (IsOneShot && global.ActivatedTriggers.Contains(DialogueId))
		{
			QueueFree(); // Hapus seketika sebelum bisa disentuh player
			return;
		}

		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		// Pastikan hanya Player yang memicu (asumsi Player masuk group "Player")
		if (!body.IsInGroup("Player")) return;

		var global = GetNode<Simpleton>("/root/Simpleton");
		
		GD.Print($"Starting dialogue: {DialogueId}");
		var dialogue = GD.Load<Resource>("res://Dialogue/" + DialogueId + ".dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "start");

		if (IsOneShot)
		{
			// Simpan status ke Singleton agar tidak muncul lagi saat scene direload
			global.ActivatedTriggers.Add(DialogueId);
			QueueFree();
		}
	}
}
