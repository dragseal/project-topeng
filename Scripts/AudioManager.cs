using Godot;

public partial class AudioManager : Node
{
	// Singleton instance
	public static AudioManager Instance { get; private set; }

	private AudioStreamPlayer _bgmPlayer;
	private AudioStreamPlayer _ambiencePlayer;

	public override void _Ready()
	{
		// Setup Singleton
		Instance = this;

		// Inisialisasi AudioStreamPlayer
		_bgmPlayer = new AudioStreamPlayer();
		_ambiencePlayer = new AudioStreamPlayer();
		AddChild(_bgmPlayer);
		AddChild(_ambiencePlayer);

		// Atur agar BGM tidak terpengaruh pause game
		_bgmPlayer.ProcessMode = ProcessModeEnum.Always;
		_ambiencePlayer.ProcessMode = ProcessModeEnum.Always;
	
	}

	public void PlayAmbience(string streamPath)
	{
		AudioStream music = GD.Load<AudioStream>(streamPath);
		
		// Jangan restart jika lagu yang sama sedang diputar
		if (_ambiencePlayer.Stream == music && _ambiencePlayer.Playing) return;

		_ambiencePlayer.Stream = music;
		_ambiencePlayer.Play();
	}


	public void PlayBGM(string streamPath)
	{
		AudioStream music = GD.Load<AudioStream>(streamPath);
		
		// Jangan restart jika lagu yang sama sedang diputar
		if (_bgmPlayer.Stream == music && _bgmPlayer.Playing) return;

		_bgmPlayer.Stream = music;
		_bgmPlayer.Play();
	}

	public void StopBGM()
	{
		_bgmPlayer.Stop();
	}

	public void SetVolume(float volumeDb)
	{
		_bgmPlayer.VolumeDb = volumeDb;
	}
}
