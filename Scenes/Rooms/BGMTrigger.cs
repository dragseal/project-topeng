using Godot;
using System;

public partial class BGMTrigger : Node2D
{
	public override void _Ready()
	{
		AudioManager.Instance.PlayBGM("res://BGM/Ambience.mp3");
	}
}
