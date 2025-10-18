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
				currentVelocity.Y = currentDirection.Y * Speed;
			}
			else
			{
				currentVelocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			}
			
			Velocity = currentVelocity;
			MoveAndSlide();

			if (Math.Abs(currentPosition.Y) > Math.Abs(availableSpace))
			{
				Vector2 stubPosition;
				stubPosition.X = currentPosition.X;
				stubPosition.Y = (currentPosition.Y > 0) ? (currentPosition.Y - 1) : (currentPosition.Y + 1);
				Position = stubPosition;
			}
		}
	}
}
