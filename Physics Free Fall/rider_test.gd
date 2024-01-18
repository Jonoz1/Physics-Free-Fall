extends Node


func _ready() -> void:
	pass

func _process(delta: float) -> void:
	var input := preload("res://Physics Free Fall/rider_test.gd").new()
