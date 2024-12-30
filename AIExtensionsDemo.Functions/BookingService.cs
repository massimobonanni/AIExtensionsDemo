using System.ComponentModel;

namespace AIExtensionsDemo.Functions
{
    internal class BookingService
    {
        private static readonly Dictionary<int, float> _roomPrices = new()
        {
            { 1, 80.0f },
            { 2, 80.0f },
            { 3, 100.0f },
            { 4, 110.0f },
            { 5, 120.0f },
            { 6, 120.0f },
            { 7, 130.0f },
            { 8, 130.0f },
            { 9, 140.0f },
            { 10, 130.0f },
            { 11, 100.0f },
            { 12, 80.0f }
        };

        public float TotalCost { get; set; }

        public void BookRoom(int numOfPeople, int monthNumber)
        {
            var currentCost= GetRoomPrice(numOfPeople, monthNumber);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"The cost of the room for {numOfPeople} people in month {monthNumber} is {currentCost}.");
            TotalCost += currentCost;
            Console.WriteLine($"The total cost of the booking is {TotalCost}.");
            Console.WriteLine("------------------------------------");
            Console.ResetColor();
        }

        [Description("Get the price of a room based on the number of people and the month number.")]
        public float GetRoomPrice(
            [Description("The number of people in the room")] int numOfPeople,
            [Description("the month number of the booking for the room")] int monthNumber)
        {
            if (monthNumber < 1 || monthNumber > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month number must be between 1 and 12.");
            }
            if (numOfPeople < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(numOfPeople), "Number of people must be greater than 0.");
            }

            var roomPrice = _roomPrices[monthNumber];

            var actualCost = roomPrice + roomPrice / 10 * (numOfPeople - 1);

            return actualCost;
        }
    }
}
