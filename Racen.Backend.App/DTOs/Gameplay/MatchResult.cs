using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.DTOs.Gameplay
{
    public class MatchResult
    {
        public required Motorcycle Initiator { get; set; }
        public required Motorcycle Opponent { get; set; }
        public bool IsInitiatorWinner { get; set; }
        public int PointsEarned { get; set; }
    }
}