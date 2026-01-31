using Godot;
using System;
using System.Collections.Generic;

public partial class Simpleton : Node
{
	public int playerRoom = 1;
	public bool playerSpawned = false;
	public bool playerDead = false;
	public int playerHP = 3;
	
	public HashSet<string> ActivatedTriggers = new HashSet<string>();
	public HashSet<string> ItemInInventory = new HashSet<string>();

	public void ResetStats() {
		playerRoom = 1;
		playerSpawned = false;
		playerDead = false;
		ActivatedTriggers = new HashSet<string>();
		ItemInInventory = new HashSet<string>();
		playerHP = 3;
	}
}
