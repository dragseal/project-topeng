extends Node

func _ready() -> void :
	await get_tree().create_timer(1.0).timeout
	ItemShowingManager._show_item("testItem")
	await get_tree().create_timer(3.0).timeout
	ItemShowingManager._hide_item()
	pass
