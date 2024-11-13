namespace Racen.Backend.App.Models.MotorcycleRelated
{
    public class DefaultProperties
    {
        private static readonly Random random = new Random();

        public static void SetDefaultProperties(Motorcycle motorcycle)
        {
            motorcycle.Level = 1;
            motorcycle.Enabled = true;

            int totalSum = motorcycle.Rarity switch
            {
                Rarity.Basic => 100,
                Rarity.Common => 150,
                Rarity.Rare => 200,
                Rarity.VeryRare => 250,
                Rarity.Super => 300,
                Rarity.Hyper => 350,
                Rarity.Legendary => 400,
                _ => 100
            };

            // Define the maximum values for each property based on the original class
            int maxSpeed = motorcycle.Rarity switch
            {
                Rarity.Basic => 79,
                Rarity.Common => 110,
                Rarity.Rare => 160,
                Rarity.VeryRare => 210,
                Rarity.Super => 260,
                Rarity.Hyper => 310,
                Rarity.Legendary => 400,
                _ => 60
            };

            int maxPower = motorcycle.Rarity switch
            {
                Rarity.Basic => 35,
                Rarity.Common => 56,
                Rarity.Rare => 75,
                Rarity.VeryRare => 96,
                Rarity.Super => 120,
                Rarity.Hyper => 160,
                Rarity.Legendary => 300,
                _ => 35
            };

            int maxHandling = motorcycle.Rarity switch
            {
                Rarity.Basic => 22,
                Rarity.Common => 33,
                Rarity.Rare => 44,
                Rarity.VeryRare => 55,
                Rarity.Super => 66,
                Rarity.Hyper => 77,
                Rarity.Legendary => 88,
                _ => 10
            };

            // Distribute the total sum among speed, power, and handling
            motorcycle.Speed = random.Next(20, Math.Min(totalSum - 40, maxSpeed));
            motorcycle.Power = random.Next(10, Math.Min(totalSum - motorcycle.Speed - 20, maxPower));
            motorcycle.Handling = totalSum - motorcycle.Speed - motorcycle.Power;

            // Ensure the properties do not exceed their maximum values
            if (motorcycle.Handling > maxHandling)
            {
                motorcycle.Handling = random.Next(10, maxHandling);
                int remainingSum = totalSum - motorcycle.Handling;
                motorcycle.Speed = random.Next(20, Math.Min(remainingSum - 20, maxSpeed));
                motorcycle.Power = remainingSum - motorcycle.Speed;
            }

            // Adjust the values to ensure the total sum is correct
            int currentSum = motorcycle.Speed + motorcycle.Power + motorcycle.Handling;
            if (currentSum != totalSum)
            {
                int difference = totalSum - currentSum;
                if (difference > 0)
                {
                    motorcycle.Speed = Math.Min(motorcycle.Speed + difference, maxSpeed);
                }
                else
                {
                    motorcycle.Speed = Math.Max(motorcycle.Speed + difference, 0);
                }
            }

            motorcycle.FuelCapacity = motorcycle.Rarity switch
            {
                Rarity.Basic => 3,
                Rarity.Common => 4,
                Rarity.Rare => 5,
                Rarity.VeryRare => 6,
                Rarity.Super => 7,
                Rarity.Hyper => 8,
                Rarity.Legendary => 9,
                _ => 3
            };
        }
    }
}