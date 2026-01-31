using Godot;
using System;
using System.Collections.Generic;

public partial class Simpleton : Node
{
	public int playerRoom = 1;
	public bool playerSpawned = false;
	public bool merahTaken = false;
	public bool toyTaken = false;
	public bool maskTaken = false;
	public bool playerDead = false;

	public HashSet<string> ActivatedTriggers = new HashSet<string>();

	public void ResetStats() {
		playerRoom = 1;
		playerSpawned = false;
		merahTaken = false;
		toyTaken = false;
		maskTaken = false;
		playerDead = false;
	}
}
