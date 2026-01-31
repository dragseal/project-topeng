extends Node2D
class_name Marker

func _enter_tree() -> void:
	MarkerManager.MarkerList[self.name] = self
	
func _exit_tree() -> void:
	if (MarkerManager.MarkerList[self.name] == self):
		MarkerManager.MarkerList[self.name] = null
