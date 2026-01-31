extends Node

var IsCurrentMaskOn : bool = false

signal OnMaskChanged

func ToggleMaskMode() -> void :
	IsCurrentMaskOn = !IsCurrentMaskOn
	OnMaskChanged.emit()
	print(IsCurrentMaskOn)

func _ready() -> void:
	print("ready")
	
