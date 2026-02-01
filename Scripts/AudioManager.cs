using Godot;

public partial class AudioManager : Node
{
	// Singleton instance
	public static AudioManager Instance { get; private set; }

	private AudioStreamPlayer _bgmPlayer;
	private AudioStreamPlayer _ambiencePlayer;

	public override void _Ready()
	{
		Init();
	}
	
	public void Init(){
		if (Instance!=null){
			return;
		}
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
		Init();
		AudioStream ambience = GD.Load<AudioStream>(streamPath);
		
		if (_ambiencePlayer.Stream == ambience && _ambiencePlayer.Playing) return;

		_ambiencePlayer.Stream = ambience;
		_ambiencePlayer.Play();
	}


	public void PlayBGM(string streamPath)
	{
		Init();
		AudioStream music = GD.Load<AudioStream>(streamPath);
		
		// Jangan restart jika lagu yang sama sedang diputar
		if (_bgmPlayer.Stream == music && _bgmPlayer.Playing) return;

		_bgmPlayer.Stream = music;
		_bgmPlayer.Play();
	}

	public void StopBGM()
	{
		Init();
		_bgmPlayer.Stop();
	}

	public void SetVolume(float volumeDb)
	{
		Init();
		_bgmPlayer.VolumeDb = volumeDb;
	}
}
