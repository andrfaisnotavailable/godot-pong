using Godot;

namespace Pong
{
	public partial class GameManager : Node
	{
		[Export] private NodePath _bluePaddleNodePath;
		private BluePaddle _bluePaddle;
		[Export] private NodePath _orangePaddleNodePath;
		private OrangePaddle _orangePaddle;
		[Export] private NodePath _ballPath;
		private Ball _ball;

		public override void _Ready()
		{
			_orangePaddle = GetNode<OrangePaddle>(_orangePaddleNodePath);
			_bluePaddle = GetNode<BluePaddle>(_bluePaddleNodePath);
			_ball = GetNode<Ball>(_ballPath);

			_ball.GoalScored += OnGoalScored;
		}

		public void OnGoalScored(int hitSide)
		{
			Enums.GoalSide side = (Enums.GoalSide)hitSide;
			if (side == Enums.GoalSide.BLUE)
			{
				_orangePaddle.GoalScored();
			}
			else if (side == Enums.GoalSide.ORANGE)
			{
				_bluePaddle.GoalScored();
			}
		}
	}
}
