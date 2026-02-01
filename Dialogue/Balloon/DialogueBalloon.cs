using Godot;
using Godot.Collections;

namespace DialogueManagerRuntime
{
  public partial class DialogueBalloon : CanvasLayer
  {
	[Export] public Resource DialogueResource;
	[Export] public string StartFromTitle = "";
	[Export] public bool AutoStart = false;
	[Export] public string NextAction = "ui_accept";
	[Export] public string SkipAction = "ui_cancel";

	Control balloon;
	RichTextLabel characterLabel;
	RichTextLabel dialogueLabel;
	VBoxContainer responsesMenu;
	Polygon2D progress;
	AudioStreamPlayer dialogueSound; 
	TextureRect fullCG;

	Array<Variant> temporaryGameStates = new Array<Variant>();
	bool isWaitingForInput = false;
	bool willHideBalloon = false;

	DialogueLine dialogueLine;
	DialogueLine DialogueLine
	{
	  get => dialogueLine;
	  set
	  {
		// Dialogue has finished so close the balloon
		if (value == null)
		{
		  if (Owner == null)
		  {
			QueueFree();
		  }
		  else
		  {
			Hide();
		  }
		  return;
		}

		dialogueLine = value;
		ApplyDialogueLine();
	  }
	}

	Timer MutationCooldown = new Timer();


	public override void _Ready()
	{
	  balloon = GetNode<Control>("%Balloon");
	  characterLabel = GetNode<RichTextLabel>("%CharacterLabel");
	  dialogueLabel = GetNode<RichTextLabel>("%DialogueLabel");
	  responsesMenu = GetNode<VBoxContainer>("%ResponsesMenu");
	  progress = GetNode<Polygon2D>("%Progress");
	  dialogueSound = GetNode<AudioStreamPlayer>("%DialogueSound");
	  fullCG = GetNode<TextureRect>("%FullCG");
	
		dialogueLabel.Connect("spoke", Callable.From((string letter, int index, float speed) => 
		{
			OnDialogueLabelSpoke(letter, index, speed);
		}));

	  balloon.Hide();

	  balloon.GuiInput += (@event) =>
	  {
		if ((bool)dialogueLabel.Get("is_typing"))
		{
		  bool mouseWasClicked = @event is InputEventMouseButton && (@event as InputEventMouseButton).ButtonIndex == MouseButton.Left && @event.IsPressed();
		  bool skipButtonWasPressed = @event.IsActionPressed(SkipAction);
		  if (mouseWasClicked || skipButtonWasPressed)
		  {
			GetViewport().SetInputAsHandled();
			dialogueLabel.Call("skip_typing");
			return;
		  }
		}

		if (!isWaitingForInput) return;
		if (dialogueLine.Responses.Count > 0) return;

		GetViewport().SetInputAsHandled();

		if (@event is InputEventMouseButton && @event.IsPressed() && (@event as InputEventMouseButton).ButtonIndex == MouseButton.Left)
		{
		  Next(dialogueLine.NextId);
		}
		else if (@event.IsActionPressed(NextAction) && GetViewport().GuiGetFocusOwner() == balloon)
		{
		  Next(dialogueLine.NextId);
		}
	  };

	  if (string.IsNullOrEmpty((string)responsesMenu.Get("next_action")))
	  {
		responsesMenu.Set("next_action", NextAction);
	  }
	  responsesMenu.Connect("response_selected", Callable.From((DialogueResponse response) =>
	  {
		Next(response.NextId);
	  }));


	  // Hide the balloon when a mutation is running
	  MutationCooldown.Timeout += () =>
	  {
		if (willHideBalloon)
		{
		  willHideBalloon = false;
		  balloon.Hide();
		}
	  };
	  AddChild(MutationCooldown);

	  DialogueManager.Mutated += OnMutated;

	  if (AutoStart)
	  {
		if (!IsInstanceValid(DialogueResource))
		{
		  throw new System.Exception(DialogueManager.GetErrorMessage(143));
		}
		Start();
	  }
	}
	
	public void OpenImage(string ImageName)
	{
		var objectImage = GD.Load<Texture2D>("res://Sprites/Items/"+ImageName+".png");
				GD.Print("Image terbaca ="+ objectImage);
				if (objectImage!=null){
					GD.Print("Image terbaca yeay="+ objectImage);
					var globalImage = GetNode<Node>("/root/ItemShowingManager");
					globalImage.Call("_show_item",ImageName);
				}
	}
	public void CloseImage()
	{
		var globalImage = GetNode<Node>("/root/ItemShowingManager");
		globalImage.Call("_hide_item");	
	}

	public void EnableMask()
	{
		// Mencari node MaskController di dalam scene tree
		var maskCtrl = GetTree().Root.FindChild("MaskController", true, false);
		
		if (maskCtrl != null)
		{
			maskCtrl.Call("enable_mask_system");
			GD.Print("MaskController diaktifkan via Dialog");
		}
		else
		{
			GD.PrintErr("MaskController tidak ditemukan! Pastikan sudah ada di Scene atau Autoload.");
		}
	}

	public override void _ExitTree()
	{
	  DialogueManager.Mutated -= OnMutated;
	}

	public void ShowCG(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			fullCG.Hide();
			return;
		}
		
		var texture = GD.Load<Texture2D>(path);
		fullCG.Texture = texture;
		fullCG.Show();
	}

	public void HideCG()
	{
		fullCG.Hide();
	}


	public override void _UnhandledInput(InputEvent @event)
	{
	  // Only the balloon is allowed to handle input while it's showing
	  GetViewport().SetInputAsHandled();
	}


	public override async void _Notification(int what)
	{
	  // Detect a change of locale and update the current dialogue line to show the new language
	  if (what == NotificationTranslationChanged && IsInstanceValid(dialogueLabel))
	  {
		float visibleRatio = dialogueLabel.VisibleRatio;
		DialogueLine = await DialogueManager.GetNextDialogueLine(DialogueResource, DialogueLine.Id, temporaryGameStates);
		if (visibleRatio < 1.0f)
		{
		  dialogueLabel.Call("skip_typing");
		}
	  }
	}


	public override void _Process(double delta)
	{
	  base._Process(delta);

	  if (IsInstanceValid(dialogueLine))
	  {
		progress.Visible = !(bool)dialogueLabel.Get("is_typing") && dialogueLine.Responses.Count == 0 && !dialogueLine.HasTag("voice");
	  }
	}


	public async void Start(Resource dialogueResource = null, string title = "", Array<Variant> extraGameStates = null)
	{
		// Inisialisasi list state dasar dengan 'this' (DialogueBalloon)
		var states = new Array<Variant> { this };

		// Tambahkan World.Instance jika tersedia agar bisa panggil SpawnGhost
		if (World.Instance != null) states.Add(World.Instance);

		// Tambahkan AudioManager.Instance jika tersedia agar bisa panggil PlayBGM
		if (AudioManager.Instance != null) states.Add(AudioManager.Instance);

		// Gabungkan dengan extraGameStates jika ada
		if (extraGameStates != null)
		{
			foreach (var state in extraGameStates)
			{
				states.Add(state);
			}
		}

		temporaryGameStates = states;
		isWaitingForInput = false;

		if (IsInstanceValid(dialogueResource))
		{
			DialogueResource = dialogueResource;
		}
		
		if (!string.IsNullOrEmpty(title))
		{
			StartFromTitle = title;
		}

		// Ambil baris dialog pertama menggunakan states yang sudah diperbarui
		DialogueLine = await DialogueManager.GetNextDialogueLine(DialogueResource, StartFromTitle, temporaryGameStates);
		Show();
	}



	public async void Next(string nextId)
	{
	  DialogueLine = await DialogueManager.GetNextDialogueLine(DialogueResource, nextId, temporaryGameStates);
	}


	#region Helpers


	private async void ApplyDialogueLine()
	{
	  MutationCooldown.Stop();

	  isWaitingForInput = false;
	  balloon.FocusMode = Control.FocusModeEnum.All;
	  balloon.GrabFocus();

	  // Set up the character name
	  characterLabel.Visible = !string.IsNullOrEmpty(dialogueLine.Character);
	  characterLabel.Text = Tr(dialogueLine.Character, "dialogue");

	  // Set up the dialogue
	  dialogueLabel.Hide();
	  dialogueLabel.Set("dialogue_line", dialogueLine);

	  // Set up the responses
	  responsesMenu.Hide();
	  responsesMenu.Set("responses", dialogueLine.Responses);

	  // Type out the text
	  balloon.Show();
	  willHideBalloon = false;
	  dialogueLabel.Show();
	  if (!string.IsNullOrEmpty(dialogueLine.Text))
	  {
		dialogueLabel.Call("type_out");
		await ToSignal(dialogueLabel, "finished_typing");
	  }

	  // Wait for input
	  if (dialogueLine.Responses.Count > 0)
	  {
		balloon.FocusMode = Control.FocusModeEnum.None;
		responsesMenu.Show();
	  }
	  else if (!string.IsNullOrEmpty(dialogueLine.Time))
	  {
		float time = 0f;
		if (!float.TryParse(dialogueLine.Time, out time))
		{
		  time = dialogueLine.Text.Length * 0.02f;
		}
		await ToSignal(GetTree().CreateTimer(time), "timeout");
		Next(dialogueLine.NextId);
	  }
	  else
	  {
		isWaitingForInput = true;
		balloon.FocusMode = Control.FocusModeEnum.All;
		balloon.GrabFocus();
	  }
	}


	#endregion


	#region signals

private void OnDialogueLabelSpoke(string letter, int index, float speed)
{
	if (!string.IsNullOrWhiteSpace(letter))
	{
		// Paksa berhenti jika sedang berputar agar bisa mulai dari awal lagi
		if (dialogueSound.Playing) 
		{
			dialogueSound.Stop();
		}
		
		dialogueSound.PitchScale = (float)GD.RandRange(0.8, 1.2);
		dialogueSound.Play();
	}
}




	private void OnMutated(Dictionary _mutation)
	{
	  isWaitingForInput = false;
	  willHideBalloon = true;
	  MutationCooldown.Start(0.1f);
	}


	#endregion
  }
}
