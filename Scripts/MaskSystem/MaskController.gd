extends Node
class_name MaskController

var is_enabled: bool = false

func _ready() -> void:
	MaskModeManager.ActiveMaskController = self
	if (!Simpleton.isHasItem("mask")):
		set_process(false)
	else:
		enable_mask_system()
		
	MaskModeManager.OnMaskChanged.connect(_on_mask_changed)

func enable_mask_system() -> void:
	if is_enabled: return
	is_enabled = true
	set_process(true)
	spawn_once()
	_on_mask_changed()
	print("Mask System Enabled!")

func spawn_once():
	var persistent_scene = load("res://Scripts/MaskSystem/mask_system_ui.tscn")
	if not persistent_scene: return
		
	var uiInstance = persistent_scene.instantiate()
	
	# Gunakan CanvasLayer sebagai parent agar UI selalu di depan
	var canvas_layer = CanvasLayer.new()
	canvas_layer.layer = 100 # Angka tinggi agar paling depan
	get_tree().root.add_child(canvas_layer)
	canvas_layer.add_child(uiInstance)

func _on_mask_changed() -> void:
	# Pastikan Player berada di dalam group "Player" di Editor
	var player = get_tree().get_first_node_in_group("Player")
	
	if player:
		# Jika IsCurrentMaskOn = true, maka canCollide = false (Uncheck Layer 8)
		# Jika IsCurrentMaskOn = false, maka canCollide = true (Check Layer 8)
		var can_collide = not MaskModeManager.IsCurrentMaskOn
		player.SetRedCollision(can_collide)
	else:
		print("Error: Node Player tidak ditemukan di group 'Player'")



func _process(_delta: float) -> void:
	if Input.is_action_just_pressed("changeMask"):
		MaskModeManager.ToggleMaskMode()
