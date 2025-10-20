using Godot;
using System;
using System.Diagnostics.Contracts;

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
		public bool stopOnWall = true;
		private int pixelBuffer = 4;
		private float SHSound_LastDirection = 0;
		private bool SHSound_Played = false;

		private AudioStreamPlayer screenHit;
		public override void _Ready()
		{
			screenHit = GetNode<AudioStreamPlayer>("ScreenHit");
		}

		public override void _PhysicsProcess(double delta)
		{
			float halfViewportHeight = GetViewport().GetVisibleRect().Size.Y / 2;
			float halfNodeHeight = GetChild<Sprite2D>(0, true).Texture.GetHeight() / 2;

			float availableSpace = halfViewportHeight - halfNodeHeight;

			Vector2 currentPosition = Position;
			Vector2 currentVelocity = Velocity;

			Vector2 currentDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (currentDirection != Vector2.Zero)
			{
				// check the proximity to the screen limits
				if (Math.Abs(currentPosition.Y) > (Math.Abs(availableSpace) - pixelBuffer) && stopOnWall)
				{
					SHSound_CheckDirectionChange(currentDirection.Y);

					// if the user still wants to go in the direction of the screen limit, stop the paddle
					if ((currentDirection.Y < 0 && currentPosition.Y < 0) || (currentDirection.Y > 0 && currentPosition.Y > 0))
					{
						currentVelocity.Y = 0;
						SHSound_TryPlay(SHSound_Played);
					}
					else
						currentVelocity.Y = currentDirection.Y * Speed;
				}
				else
					currentVelocity.Y = currentDirection.Y * Speed;
			}
			else
				currentVelocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);

			Velocity = currentVelocity;
			MoveAndSlide();
		}

		private void SHSound_CheckDirectionChange(float currDir)
		{
			if (SHSound_Played)
			{
				// if the sound has been played check for input direction switched since the last play
				if (SHSound_LastDirection != currDir)
				{
					SHSound_LastDirection = currDir;
					SHSound_Played = false;
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
				screenHit.Play();
				SHSound_Played = true;
			}
		}
	}
}
