using Godot;

namespace Pong
{
	public partial class OrangeScoreGUI : Label
	{
		[Export] private NodePath _orangePaddleNodePath;
		private OrangePaddle _orangePaddle;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_orangePaddle = GetNode<OrangePaddle>(_orangePaddleNodePath);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			Text = "Score " + _orangePaddle.Score;
        }
	}
}
