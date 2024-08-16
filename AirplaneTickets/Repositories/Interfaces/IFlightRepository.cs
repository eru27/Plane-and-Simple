using System.Threading.Tasks;
using System.Collections.Generic;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Models.ViewModels;

namespace AirplaneTickets.Repositories.Interfaces
{
    public interface IFlightRepository
    {
        Task<OperationResult> CreateFlightAsync(Flight flight);

        Task<OperationResult> CancelFlightAsync(Flight flight);

        Task<Flight> GetFlightByIdAsync(Guid id);

        Task<IEnumerable<Flight>> FetchAllFlightsAsync();

        Task<IEnumerable<Flight>> SearchFlightsAsync(SearchFlightsViewModel model);

        Task<OperationResult> UpdateAsync(Flight flight);
    }
}
