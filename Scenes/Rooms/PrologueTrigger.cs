using Godot;
using System;
using DialogueManagerRuntime;

public partial class PrologueTrigger : Node2D
{
	[Export] public string DialogueId = "Dialogue1";
	[Export] public bool IsOneShot = true;

	public override void _Ready()
	{
		var global = GetNode<Simpleton>("/root/Simpleton");

		// Cek apakah ID ini sudah pernah dijalankan sebelumnya
		if (IsOneShot && global.ActivatedTriggers.Contains(DialogueId))
		{
			QueueFree(); // Hapus node dan jangan jalankan dialog
			return;
		}

		// Jalankan dialog
		var dialogue = GD.Load<Resource>("res://Dialogue/" + DialogueId + ".dialogue");
		DialogueManager.ShowDialogueBalloon(dialogue, "start");

		if (IsOneShot)
		{
			// Catat ID ini ke dalam global state agar tidak diulang
			global.ActivatedTriggers.Add(DialogueId);
			QueueFree();
		}
	}
}
