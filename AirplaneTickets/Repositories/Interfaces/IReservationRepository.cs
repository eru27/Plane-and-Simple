using System.Threading.Tasks;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;

namespace AirplaneTickets.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<OperationResult> CreateAsync(Reservation reservation);

        Task<IEnumerable<Reservation>> GetReservationsByIdAsync(Guid userId);

        Task<IEnumerable<Reservation>> FetchAllReservationsAsync();

        Task<Reservation> GetReservationByIdAsync(Guid reservationId);

        Task<OperationResult> UpdateAsync(Reservation reservation);
    }
}
