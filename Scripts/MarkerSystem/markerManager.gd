extends Node

@export var MarkerList : Dictionary[String,Node2D]

func GetMarkerPosition (markerName : String) -> Vector2 :
	if (MarkerList.has(markerName)):
		var markerGet = MarkerList[markerName] as Node2D
		return markerGet.global_position
	return Vector2.ZERO
