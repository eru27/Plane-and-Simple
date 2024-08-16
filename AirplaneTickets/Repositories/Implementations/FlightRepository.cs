using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AirplaneTickets.Repositories.Interfaces;
using AirplaneTickets.Data;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Models.ViewModels;

namespace AirplaneTickets.Repositories.Implementations
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FlightRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> CreateFlightAsync(Flight flight)
        {
            try
            {
                _dbContext.Flights.Add(flight);
                await _dbContext.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Failure("Unknown error.");
            }
        }

        public async Task<OperationResult> CancelFlightAsync(Flight flight)
        {
            if (flight == null)
            {
                return OperationResult.Failure("Flight not found.");
            }

            try
            {
                flight.IsCanceled = true;

                _dbContext.Flights.Update(flight);
                await _dbContext.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Failure("Unknown error.");
            }
            
        }

        public async Task<Flight> GetFlightByIdAsync(Guid id)
        {
            var flight = await _dbContext.Flights.FirstOrDefaultAsync(f => f.Id == id);

            return flight;
        }

        public async Task<IEnumerable<Flight>> FetchAllFlightsAsync()
        {
            var flights = await _dbContext.Flights.ToListAsync();

            return flights;
        }

        public async Task<IEnumerable<Flight>> SearchFlightsAsync(SearchFlightsViewModel model)
        {
            var flights = await _dbContext.Flights
                .Where(f => f.IsCanceled == false &&
                            f.Departure == model.Departure &&
                            f.Destination == model.Destination &&
                            f.DepartureDate >= DateOnly.FromDateTime(DateTime.Now) &&
                            !(!model.ConnectingFlights && (f.Connections > 0)) &&
                            f.AvailableSeats > 0)
                .ToListAsync();

            return flights;
        }

        public async Task<OperationResult> UpdateAsync(Flight flight)
        {
            if (flight == null)
            {
                return OperationResult.Failure("Flight not found.");
            }

            try
            {
                _dbContext.Flights.Update(flight);
                await _dbContext.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Failure("Unknown error.");
            }

        }
    }
}
