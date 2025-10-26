using Godot;
using System;

// Godot classes need 'partial' modifier cause godot uses source generation instead of reflection.
// Source generation is supposed to create code during compilation instead reflections manipulates
// it at runtime. So godot inspects user created code during compilation and generates 
// additional code on the fly.
// Source generators will be able to read the contents of the compilation before running, as well 
// as access any additional files, enabling generators to introspect both user C# code and 
// generator -specific files 
// https://posts.specterops.io/dotnet-source-generators-in-2024-part-1-getting-started-76d619b633f5

namespace Dong
{
	public partial class OrangeDong : CharacterBody2D, IScore
	{
		[Export]
		public float Speed { get; set; } = 300.0f;
		private Vector2 _inputDir = Vector2.Zero;
    	private bool _touchingWall = false;
		private float _lastInputDirY = 0;
		public override void _PhysicsProcess(double delta)
		{
			_inputDir.Y = Input.GetAxis("orange_move_up", "orange_move_down");

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
				if (collision.GetCollider() is PhysicsBody2D collidedBody && collidedBody.CollisionLayer == (int)Enums.CollisionLayers.WALLS)
				{
					AudioManager.Instance.PlaySound("PlayerHit");
					_touchingWall = true;

					// Take back the player to the correct position
					var remainder = collision.GetRemainder();
					Position -= remainder;
				}
			}
		}
		
		//////////////////// ISCORE BLOCK //////////////////////////
		private int _score = 0;
		public int Score => _score;

		public void GoalScored()
		{
			_score += 1;
		}

		public void ResetScore()
		{
			_score = 0;
		}
		////////////////////////////////////////////////////////////
	}
}
