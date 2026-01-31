extends Node

var IsCurrentMaskOn : bool = false

signal OnMaskChanged

func ToggleMaskMode() -> void :
	IsCurrentMaskOn = !IsCurrentMaskOn
	OnMaskChanged.emit()
	
	# Di GDScript, panggil langsung nama Autoload-nya (AudioManager)
	# Tidak perlu menggunakan .Instance
	if IsCurrentMaskOn:
		AudioManager.PlayAmbience("res://BGM/Masked.mp3")
	else:
		AudioManager.PlayAmbience("res://BGM/Ambience.mp3")
		
	print("Mask Status: ", IsCurrentMaskOn)

func _ready() -> void:
	# Pastikan AudioManager sudah terdaftar di Project Settings -> Autoload
	if IsCurrentMaskOn:
		AudioManager.PlayAmbience("res://BGM/Masked.mp3")
	else:
		AudioManager.PlayAmbience("res://BGM/Ambience.mp3")
