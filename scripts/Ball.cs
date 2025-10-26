using Godot;
using System;

namespace Pong
{
	public partial class Ball : CharacterBody2D
	{
		[Signal] public delegate void GoalScoredEventHandler(int hitSide);
		[Export] public float Speed = 300f;
		private Vector2 _direction;

		public override void _Ready()
		{
			GenerateRandomDirection();
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = _direction * Speed * (float)delta;

			KinematicCollision2D collision = MoveAndCollide(velocity);
			if (collision != null)
			{
				AudioManager.Instance.PlaySound("BallHit");
				if (collision.GetCollider() is PhysicsBody2D)
				{
					PhysicsBody2D collidedBody = collision.GetCollider() as PhysicsBody2D;
					if (IsOnLayer(collidedBody, (int)Enums.CollisionLayers.BLUEGOAL))
					{
						EmitSignal(SignalName.GoalScored, (int)Enums.GoalSide.BLUE);
					}
					if (IsOnLayer(collidedBody, (int)Enums.CollisionLayers.ORANGEGOAL))
                    {
                        EmitSignal(SignalName.GoalScored, (int)Enums.GoalSide.ORANGE);
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
