extends Node

	
var uiInstance: ItemShowingUI = null
func _show_item(itemName : String) -> void :
	_check_intance()
	if uiInstance:
		uiInstance.show("res://Sprites/Items/"+itemName+".png")
	pass
	
func _hide_item() -> void :
	_check_intance()
	if uiInstance:
		uiInstance.hide()
	pass

func _check_intance() -> void :
	if uiInstance:
		return
	var persistent_scene: PackedScene = load("res://Scripts/ItemShowcaseSystem/item_showing_ui.tscn")
	if not persistent_scene:
		push_error("Persistent scene not assigned")
		return
	print("UI instantiated")
	uiInstance = persistent_scene.instantiate()
	var world := get_tree().root.get_node_or_null("World")
	if world:
		world.add_child(uiInstance)
