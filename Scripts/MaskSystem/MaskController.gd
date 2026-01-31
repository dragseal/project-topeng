extends Node
class_name MaskController

func _ready() -> void:
	spawn_once()

func spawn_once():
	var uiInstance: Node = null
	var persistent_scene: PackedScene = load("res://Scripts/MaskSystem/mask_system_ui.tscn")
	if not persistent_scene:
		push_error("Persistent scene not assigned")
		return
	print("UI instantiated")
	uiInstance = persistent_scene.instantiate()
	var world := get_tree().root.get_node_or_null("World")
	if world:
		world.add_child(uiInstance)

func _process(delta: float) -> void:
	if Input.is_action_just_pressed("changeMask"):
		MaskModeManager.ToggleMaskMode()
