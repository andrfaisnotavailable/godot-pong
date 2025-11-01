using Godot;
using System.Threading.Tasks;

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
		[Export] private NodePath _countdownLabelPath;
		private Label _countdownLabel;

		public override void _Ready()
		{
			_orangePaddle = GetNode<OrangePaddle>(_orangePaddleNodePath);
			_bluePaddle = GetNode<BluePaddle>(_bluePaddleNodePath);
			_ball = GetNode<Ball>(_ballPath);
			_countdownLabel = GetNode<Label>(_countdownLabelPath);

			_ball.GoalScored += OnGoalScored;
		}

		public async void OnGoalScored(int hitSide)
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
			_ball.ResetBall(Vector2.Zero);

			await ShowCountdownAndRestart();
		}

		private async Task ShowCountdownAndRestart()
		{
			int seconds = 3;
			_countdownLabel.Visible = true;

			while (seconds > 0)
			{
				_countdownLabel.Text = seconds.ToString();
				await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
				seconds--;
			}

			_countdownLabel.Visible = false;
			_ball.GenerateRandomDirection();
		}
	}
}
