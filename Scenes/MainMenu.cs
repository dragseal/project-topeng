using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] public string FirstLevelPath = "res://Scenes/World.tscn";
	[Export] public Button playBtn;
	[Export] public Button exitBtn;
	public override void _Ready()
	{
		AudioManager.Instance.PlayAmbience("res://BGM/Main Menu.mp3");
		// Hubungkan Signal Pressed ke Method
		playBtn.Pressed += OnPlayButtonPressed;
		exitBtn.Pressed += OnExitButtonPressed;

		// Fokus otomatis ke tombol Play (untuk dukungan Controller/Keyboard)
		playBtn.GrabFocus();
	}

	private void OnPlayButtonPressed()
	{
		// Pindah ke scene game utama
		GetTree().ChangeSceneToFile(FirstLevelPath);
	}

	private void OnExitButtonPressed()
	{
		// Keluar dari game
		GetTree().Quit();
	}
}
