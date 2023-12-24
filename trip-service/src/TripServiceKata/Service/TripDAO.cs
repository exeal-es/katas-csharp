using System.Collections.Generic;
using TripServiceKata.Entity;
using TripServiceKata.Exception;

namespace TripServiceKata.Service
{
    public class TripDAO
    {
        public static List<Trip> FindTripsByUser(User user)
        {
            throw new DependendClassCallDuringUnitTestException(
                "TripDAO should not be invoked on an unit test.");
        }
    }
}