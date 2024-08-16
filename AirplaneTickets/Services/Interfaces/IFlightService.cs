using System.Threading.Tasks;
using System.Collections.Generic;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Models.ViewModels;

namespace AirplaneTickets.Services.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> SearchFlightsAsync(SearchFlightsViewModel model);

        Task<Flight> GetFlightByIdAsync(Guid id);

        Task<OperationResult> CreateFlightAsync(CreateFlightViewModel model);

        Task<IEnumerable<Flight>> FetchAllFlightsAsync();

        Task<(Flight, OperationResult)> CancelFlightByIdAsync(Guid id);

        Task<OperationResult> ReserveSeatsAsync(Guid flightId, int seats);
    }
}
