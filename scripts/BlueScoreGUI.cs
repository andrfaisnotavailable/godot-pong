using Godot;

namespace Pong
{
	public partial class BlueScoreGUI : Label
	{
		[Export] private NodePath _bluePaddleNodePath;
		private BluePaddle _bluePaddle;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_bluePaddle = GetNode<BluePaddle>(_bluePaddleNodePath);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			Text = "Score " + _bluePaddle.Score;
        }
	}
}
