namespace Dong
{
    public interface IScore
    {
        int Score { get; }

        void GoalScored();

        void ResetScore();
    }
}