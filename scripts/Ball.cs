using Godot;
using System;

namespace Dong
{
	public partial class Ball : CharacterBody2D
	{
		[Export]
		public float Speed = 300f;
		private Vector2 _direction;
		private AudioStreamPlayer _hit;

		public override void _Ready()
		{
			_hit = GetNode<AudioStreamPlayer>("Hit");

			// Direzione casuale iniziale (±x, ±y)
			var rng = new Random();
			float angle = (float)(rng.NextDouble() * Math.PI / 2 - Math.PI / 4); // ±45°
			int dirX = rng.Next(0, 2) == 0 ? -1 : 1;

			_direction = new Vector2(dirX, MathF.Sin(angle)).Normalized();
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = _direction * Speed * (float)delta;

			var collision = MoveAndCollide(velocity);
			if (collision != null)
			{
				_hit.Play();
				HandleCollision(collision);
			}
		}

		private void HandleCollision(KinematicCollision2D collision)
		{
			// Riflette la direzione in base alla normale della superficie colpita
			_direction = _direction.Bounce(collision.GetNormal()).Normalized();

			// Piccolo offset per evitare “attaccamenti” ai bordi
			Position += collision.GetNormal() * 2f;
		}

		public void ResetBall(Vector2 position)
		{
			GlobalPosition = position;

			// Reimposta una direzione casuale
			var rng = new Random();
			float angle = (float)(rng.NextDouble() * Math.PI / 2 - Math.PI / 4);
			int dirX = rng.Next(0, 2) == 0 ? -1 : 1;
			_direction = new Vector2(dirX, MathF.Sin(angle)).Normalized();
		}
	}
}
