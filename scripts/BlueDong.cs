using Godot;
using System;

namespace Dong
{
	public partial class BlueDong : CharacterBody2D
	{
		[Export]
		public float Speed { get; set; } = 300.0f;
		private Vector2 _inputDir = Vector2.Zero;
    	private bool _touchingWall = false;
    	private float _lastInputDirY = 0;

		public override void _PhysicsProcess(double delta)
		{
			_inputDir.Y = Input.GetAxis("blue_move_up", "blue_move_down");
		
			// If player is touching the wall and the input direction is not changed stay still
			if (_touchingWall && Math.Sign(_inputDir.Y) == Math.Sign(_lastInputDirY))
			{
				Velocity = Vector2.Zero;
				return;
			}

			// Unlock movement if input direction is changed
			if (Math.Sign(_inputDir.Y) != Math.Sign(_lastInputDirY))
			{
				_touchingWall = false;
			}

			// Take in memory the last direction for next execution compare
			_lastInputDirY = _inputDir.Y;

			Velocity = _inputDir * Speed;

			KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
			if (collision != null)
			{
				// CollisionLayer 2 = Walls
                if (collision.GetCollider() is PhysicsBody2D collidedBody && collidedBody.CollisionLayer == 2)
				{
					AudioManager.Instance.PlaySound("PlayerHit");
                    _touchingWall = true;

                    // Take back the player to the correct position
                    var remainder = collision.GetRemainder();
                    Position -= remainder;
                }
            }
		}
	}
}
