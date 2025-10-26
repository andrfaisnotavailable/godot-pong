using Godot;

namespace Dong
{
	public partial class GameManager : Node
	{
		[Export] private NodePath _playerBluePath;
		private BlueDong _playerBlue;
		[Export] private NodePath _playerOrangePath;
		private OrangeDong _playerOrange;
		[Export] private NodePath _ballPath;
		private Ball _ball;

		public override void _Ready()
		{
			_playerOrange = GetNode<OrangeDong>(_playerOrangePath);
			_playerBlue = GetNode<BlueDong>(_playerBluePath);
			_ball = GetNode<Ball>(_ballPath);

			_ball.GoalScored += OnGoalScored;
		}

		public void OnGoalScored(string hitSide)
		{
			if (hitSide == "blue")
			{
				_playerOrange.GoalScored();
			}
			else if (hitSide == "orange")
			{
				_playerBlue.GoalScored();
			}

			GD.Print($"Blue score: {_playerBlue.Score}; Orange score: {_playerOrange.Score}");
		}
	}
}
