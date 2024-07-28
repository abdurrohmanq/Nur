using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi;
using System.Globalization;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace Nur.Bot.BotServices.Location;

public class LocationService(string googleApiKey)
{
    private readonly string _googleApiKey = googleApiKey;

    public async Task<string> GetDrivingDistanceAsync(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
    {
        var request = new DistanceMatrixRequest
        {
            Origins = new[] { $"{originLatitude.ToString(CultureInfo.InvariantCulture)},{originLongitude.ToString(CultureInfo.InvariantCulture)}" },
            Destinations = new[] { $"{destinationLatitude.ToString(CultureInfo.InvariantCulture)},{destinationLongitude.ToString(CultureInfo.InvariantCulture)}" },
            Mode = (DistanceMatrixTravelModes?)TravelMode.Driving,
            ApiKey = _googleApiKey
        };

        var response = await GoogleMaps.DistanceMatrix.QueryAsync(request);

        // Logging the status and possible errors
        Console.WriteLine($"Distance Matrix API response status: {response.Status}");
        if (response.Status != DistanceMatrixStatusCodes.OK)
        {
            Console.WriteLine($"Error with Distance Matrix API request: {response.ErrorMessage}");
        }

        if (response.Status == DistanceMatrixStatusCodes.OK && response.Rows.Any())
        {
            var element = response.Rows.First().Elements.First();
            Console.WriteLine($"Element status: {element.Status}");
            if (element.Status == DistanceMatrixElementStatusCodes.OK)
            {
                return element.Distance.Text;
            }
            else
            {
                Console.WriteLine($"Element status error: {element.Status}");
                if (element.Status == DistanceMatrixElementStatusCodes.NOT_FOUND)
                {
                    Console.WriteLine("At least one of the origin, destination, or waypoints could not be geocoded.");
                }
                else if (element.Status == DistanceMatrixElementStatusCodes.ZERO_RESULTS)
                {
                    Console.WriteLine("No route could be found between the origin and destination.");
                }
                else if (element.Status == DistanceMatrixElementStatusCodes.MAX_ROUTE_LENGTH_EXCEEDED)
                {
                    Console.WriteLine("The requested route is too long and cannot be processed.");
                }
            }
        }

        return "Unable to calculate distance";
    }

    public async Task<GeocodingResponse> GetLocationInfoAsync(double latitude, double longitude)
    {
        var request = new GeocodingRequest
        {
            Location = new GoogleMapsApi.Entities.Common.Location(latitude, longitude),
            ApiKey = _googleApiKey
        };

        var response = await GoogleMaps.Geocode.QueryAsync(request);

        if (response.Status == Status.OK && response.Results.Any())
        {
            return response;
        }

        return null;
    }
}