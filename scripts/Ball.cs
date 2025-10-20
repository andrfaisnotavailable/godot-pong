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

			GenerateRandomDirection();
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = _direction * Speed * (float)delta;

			KinematicCollision2D collision = MoveAndCollide(velocity);
			if (collision != null)
			{
				_hit.Play();
				if (collision.GetCollider() is PhysicsBody2D)
				{
					PhysicsBody2D collidedBody = collision.GetCollider() as PhysicsBody2D;
					// 3: BlueGoal
					// 4: OrangeGoal
					if (IsOnLayer(collidedBody, 3) || IsOnLayer(collidedBody, 4))
                    {
						ResetBall(Vector2.Zero);
                    }
				}
				HandleCollision(collision);
			}
		}

		private void HandleCollision(KinematicCollision2D collision)
		{
			// Reflects the direction based on the normal line to the surface hit
			_direction = _direction.Bounce(collision.GetNormal()).Normalized();

			// Little offset to avoid intersections within borders
			Position += collision.GetNormal() * 2f;
		}

		public void ResetBall(Vector2 position)
		{
			GlobalPosition = position;
			GenerateRandomDirection();
		}

		private void GenerateRandomDirection()
        {
            Random rng = new();
			float angle = (float)(rng.NextDouble() * Math.PI / 2 - Math.PI / 4); // ±45°
			int dirX = rng.Next(0, 2) == 0 ? -1 : 1;

			_direction = new Vector2(dirX, MathF.Sin(angle)).Normalized();
        }

		private static bool IsOnLayer(PhysicsBody2D body, int layer)
		{
			return (body.CollisionLayer & (1u << (layer - 1))) != 0;
		}
	}
}
