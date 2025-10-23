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
	public partial class OrangeDong : CharacterBody2D
	{
		[Export]
		public float Speed { get; set; } = 300.0f;
		[Export]
		public bool StopOnWall = true;
		private int _pixelBuffer = 4;
		private float _SHSound_lastDirection = 0;
		private bool _SHSound_played = false;
		private bool _shootingStarsPlayed = false;

		public override void _PhysicsProcess(double delta)
		{
			float halfViewportHeight = GetViewport().GetVisibleRect().Size.Y / 2;
			float halfNodeHeight = GetChild<Sprite2D>(0, true).Texture.GetHeight() * GetChild<Sprite2D>(0, true).Transform.Scale.Y * Transform.Scale.Y / 2;

			float availableSpace = halfViewportHeight - halfNodeHeight;

			float currentYPosition = Position.Y;
			Vector2 currentVelocity = Velocity;

			float currentYDirection = Input.GetAxis("orange_move_up", "orange_move_down");
			if (currentYDirection != 0)
			{
				// check the proximity to the screen limits
				if (Math.Abs(currentYPosition) > (Math.Abs(availableSpace) - _pixelBuffer) && StopOnWall)
				{
					SHSound_CheckDirectionChange(currentYDirection);

					// if the user still wants to go in the direction of the screen limit, stop the paddle
					if ((currentYDirection < 0 && currentYPosition < 0) || (currentYDirection > 0 && currentYPosition > 0))
					{
						currentVelocity.Y = 0;
						SHSound_TryPlay(_SHSound_played);
					}
					else
						currentVelocity.Y = currentYDirection * Speed;
				}
				else
					currentVelocity.Y = currentYDirection * Speed;
			}
			else
				currentVelocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);

			Velocity = currentVelocity;
			MoveAndSlide();

			////////////// TOTALLY USELESS BUT WHY NOT /////////////////
			if (Position.X != 565 && !_shootingStarsPlayed)
			{
				AudioManager.Instance.PlaySound("ShootingStars");
				_shootingStarsPlayed = true;
			}
			////////////////////////////////////////////////////////////
		}

		private void SHSound_CheckDirectionChange(float currDir)
		{
			if (_SHSound_played)
			{
				// if the sound has been played check for input direction switched since the last play
				if (_SHSound_lastDirection != currDir)
				{
					_SHSound_lastDirection = currDir;
					_SHSound_played = false;
				}
			}
		}

		// The conditions to play the sound are:
		// - the player hasn't yet hit the screen
		// - the input direction is changed at least once since the last hit
		private void SHSound_TryPlay(bool playable)
		{
			if (!playable)
			{
				AudioManager.Instance.PlaySound("PlayerHit");
				_SHSound_played = true;
			}
		}
	}
}
