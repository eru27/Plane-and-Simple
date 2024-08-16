using System.Threading.Tasks;
using System.Collections.Generic;
using AirplaneTickets.Services.Interfaces;
using AirplaneTickets.Repositories.Interfaces;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.ViewModels;
using AirplaneTickets.Models.Shared;

namespace AirplaneTickets.Services.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<IEnumerable<Flight>> SearchFlightsAsync(SearchFlightsViewModel model)
        {
            var flights = await _flightRepository.SearchFlightsAsync(model);

            if (flights == null)
            {
                return Enumerable.Empty<Flight>();
            }

            return flights.OrderBy(f => f.DepartureDate).ThenBy(f => f.Departure).ThenBy(f => f.Destination);
        }

        public async Task<Flight> GetFlightByIdAsync(Guid id)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(id);

            return flight;
        }

        public async Task<OperationResult> CreateFlightAsync(CreateFlightViewModel model)
        {
            if (model.Departure == model.Destination)
            {
                return OperationResult.Failure("Departure and destination cannot be the same.");
            }

            if (model.DepartureDate < DateOnly.FromDateTime(DateTime.Now))
            {
                return OperationResult.Failure("Departure date cannot be in the past.");
            }

            var flight = new Flight
            {
                Departure = model.Departure,
                Destination = model.Destination,
                DepartureDate = model.DepartureDate,
                Connections = model.Connections,
                TotalSeats = model.TotalSeats,
                AvailableSeats = model.TotalSeats,
                IsCanceled = false
            };

            var result = await _flightRepository.CreateFlightAsync(flight);

            return result;
        }
        
        public async Task<IEnumerable<Flight>> FetchAllFlightsAsync()
        {
            var flights = await _flightRepository.FetchAllFlightsAsync();

            if (flights == null)
            {
                return Enumerable.Empty<Flight>();
            }

            return flights.OrderBy(f => f.DepartureDate).ThenBy(f => f.Departure).ThenBy(f => f.Destination);

        }

        public async Task<(Flight, OperationResult)> CancelFlightByIdAsync(Guid id)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(id);

            if (flight == null)
            {
                return (flight, OperationResult.Failure("Flight not found."));
            }

            var result = await _flightRepository.CancelFlightAsync(flight);

            return (flight, result);
        }

        public async Task<OperationResult> ReserveSeatsAsync(Guid flightId, int seats)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);

            if (flight == null)
            {
                return OperationResult.Failure("Flight not found.");
            }

            if (flight.AvailableSeats < seats)
            {
                return OperationResult.Failure("Not enough seats available.");
            }

            flight.AvailableSeats -= seats;

            return await _flightRepository.UpdateAsync(flight);
        }

    }
}
