using Godot;
using System;
using System.Collections.Generic;

public partial class Simpleton : Node
{
	public static Simpleton instance;
	
	public static bool isPlayerDead{
		get{
			return instance.playerDead;
		}
		set{
			instance.playerDead = value;
		}
	}
	
	public int playerRoom = 1;
	public bool playerSpawned = false;
	public bool playerDead = false;
	public int playerHP = 3;
	
	public HashSet<string> ActivatedTriggers = new HashSet<string>();
	public HashSet<string> ItemInInventory = new HashSet<string>();
	
	public override void _EnterTree(){
		instance = this;
	}
	
	public bool isHasItem (string itemName)
	{
		return ItemInInventory.Contains(itemName);
	}

	public void ResetStats() {
		playerRoom = 1;
		playerSpawned = false;
		playerDead = false;
		 ActivatedTriggers.Clear(); 
		ItemInInventory = new HashSet<string>();
		playerHP = 3;
	}
}
