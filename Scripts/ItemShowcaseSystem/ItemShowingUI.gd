extends Node
class_name ItemShowingUI

@export var ItemImage: TextureRect
@export var ShownPosition: Vector2
@export var HidePosition: Vector2

var _tween: Tween
	
func show(image_path: String) -> void:
	if not ItemImage:
		return

	# Load and assign texture
	if image_path != "":
		var texture := load(image_path)
		if texture is Texture2D:
			ItemImage.texture = texture
		else:
			push_warning("Invalid texture at path: " + image_path)

	ItemImage.visible = true
	ItemImage.position = HidePosition

	_kill_tween()
	_tween = get_tree().create_tween()
	_tween.tween_property(
		ItemImage,
		"position",
		ShownPosition,
		1.0
	).set_trans(Tween.TRANS_EXPO).set_ease(Tween.EASE_OUT)


func hide() -> void:
	if not ItemImage:
		return

	ItemImage.position = ShownPosition

	_kill_tween()
	_tween = get_tree().create_tween()
	_tween.tween_property(
		ItemImage,
		"position",
		HidePosition,
		1
	).set_trans(Tween.TRANS_EXPO).set_ease(Tween.EASE_OUT)

	_tween.finished.connect(_on_hide_finished)

func _on_hide_finished() -> void:
	if ItemImage:
		ItemImage.visible = false

func _kill_tween() -> void:
	if _tween and _tween.is_running():
		_tween.kill()
