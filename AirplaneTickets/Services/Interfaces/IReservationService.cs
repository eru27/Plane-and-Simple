using System.Threading.Tasks;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Models.ViewModels;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Enums;

namespace AirplaneTickets.Services.Interfaces
{
    public interface IReservationService
    {
        Task<(Reservation, OperationResult)> CreateReservationAsync(CreateReservationViewModel model);

        Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId);

        Task<IEnumerable<Reservation>> FetchAllReservationsAsync();

        Task<Reservation> GetReservationByIdAsync(Guid reservaidtionId);

        Task<(Reservation?, OperationResult)> GetFlightAndUserByIdsAsync(Guid flightId, Guid userId);

        Task<(Reservation, OperationResult)> ChangeReservationStatusByIdAsync(Guid reservationId, ReservationStatus newStatus);
    }
}
