extends Node

@export var _on_color : Color
@export var _off_color : Color

func _ready() -> void:
	MaskModeManager.OnMaskChanged.connect(_on_mask_changed)
	_update_color()
	
func _exit_tree() -> void:
	MaskModeManager.OnMaskChanged.disconnect(_on_mask_changed)

func _on_mask_changed() -> void:
	print("mask changed")
	_update_color()
	
func _update_color() -> void:
	if (MaskModeManager.IsCurrentMaskOn):
		_set_all_texture_color(self,_on_color)
	else:
		_set_all_texture_color(self,_off_color)

func _set_all_texture_color(node: Node, color: Color) -> void:
	for child in node.get_children():
		if child is TextureRect:
			child.self_modulate = color
		
		# Recurse into grandchildren
		if child.get_child_count() > 0:
			_set_all_texture_color(child, color)
