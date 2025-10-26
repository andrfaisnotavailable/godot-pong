namespace Pong
{
    public interface IScore
    {
        int Score { get; }

        void GoalScored();

        void ResetScore();
    }
}