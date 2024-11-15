using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Models.Gameplay;
using Racen.Backend.App.DTOs.Gameplay;

namespace Racen.Backend.App.Models.Gameplay
{
    public class NewMatch
    {
        public required Motorcycle Motorcycle1 { get; set; }
        public required Motorcycle Motorcycle2 { get; set; }
        public GameMode GameMode { get; set; }
        public Motorcycle? Winner { get; set; }

        public MatchResult CalculateWinnerFromInitiatorPerspective(Motorcycle initiatorMotorcycle)
        {
            var player1Score = GetTotalScore(Motorcycle1);
            var player2Score = GetTotalScore(Motorcycle2);

            if (player1Score > player2Score)
            {
                Winner = Motorcycle1;
            }
            else if (player2Score > player1Score)
            {
                Winner = Motorcycle2;
            }
            else
            {
                Winner = null;
            }

            bool isInitiatorWinner = Winner == initiatorMotorcycle;
            int pointsEarned = isInitiatorWinner ? 10 : 5; // Example points logic

            return new MatchResult
            {
                Initiator = initiatorMotorcycle,
                Opponent = initiatorMotorcycle == Motorcycle1 ? Motorcycle2 : Motorcycle1,
                IsInitiatorWinner = isInitiatorWinner,
                PointsEarned = pointsEarned
            };
        }

        private int GetTotalScore(Motorcycle motorcycle)
        {
            decimal totalScore = motorcycle.Speed + motorcycle.Power + motorcycle.Handling;

            switch (GameMode)
            {
                case GameMode.Drag:
                    return (int)(totalScore + (decimal)motorcycle.Power * 0.3m);
                case GameMode.Circuit:
                    return (int)(totalScore + (decimal)motorcycle.Speed * 0.3m);
                case GameMode.Offroad:
                    return (int)(totalScore + (decimal)motorcycle.Handling * 0.3m);
            }

            return (int)totalScore;
        }
    }
}