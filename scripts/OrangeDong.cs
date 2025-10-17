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
		public const float Speed = 300.0f;
		private bool exitingScreen = false;

		public override void _PhysicsProcess(double delta)
		{
			Vector2 tempVelocity = Velocity;
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (direction != Vector2.Zero)
			{
				if (!exitingScreen)
					tempVelocity.Y = direction.Y * Speed;
				else
				{
					tempVelocity.Y = -direction.Y * Speed * 2;
					exitingScreen = false;
				}
			}
			else
			{
				tempVelocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			}

			Velocity = tempVelocity;
			MoveAndSlide();

		}

		private void OnScreenEntered(Node2D body)
		{
			if (body is OrangeDong)
			{
				exitingScreen = true;
			}
		}
	}
}
