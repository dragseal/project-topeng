using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	public Sprite2D _sprite; 
	public float _alpha = 1;
	public float timer = 0;
	
	// Tambahkan variabel untuk menyimpan status warna
	public bool IsRed = false;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		
		// Logika tambahan yang diterapkan:
		AddToGroup("Bullets");
		
		// Momentum: Berganti setiap 3 detik (3000ms)
		IsRed = (Time.GetTicksMsec() % 6000) > 3000;

		if (IsRed)
		{
			_sprite.Modulate = new Color(1, 0, 0); // Merah
			CollisionLayer = 0;
			SetCollisionLayerValue(9, true);
		}
		else
		{
			_sprite.Modulate = new Color(1, 1, 1); // Putih
			CollisionLayer = 0;
			SetCollisionLayerValue(5, true); // Peluru Putih di Layer 1
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		timer += 1f;
		if (timer >= 240) {
			_alpha -= 0.2f;
			
			// Safety check untuk mencegah NullReference jika peluru dihapus
			if (_sprite != null) 
			{
				var color = _sprite.SelfModulate;
				color.A = _alpha;
				_sprite.SelfModulate = color;
			}

			if (_alpha <= 0) QueueFree();
		}
		
		MoveAndSlide();
	}
}
