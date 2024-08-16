using System.Threading.Tasks;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Models.ViewModels;
using AirplaneTickets.Repositories.Interfaces;
using AirplaneTickets.Services.Interfaces;
using AirplaneTickets.Models.Enums;

namespace AirplaneTickets.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IUserService _userService;
        private readonly IFlightService _flightService;
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IUserService userService, IFlightService flightService, IReservationRepository reservationRepository)
        {
            _userService = userService;
            _flightService = flightService;
            _reservationRepository = reservationRepository;
        }

        private async Task PopulateFlightAndUserAsync(IEnumerable<Reservation> reservations)
        {
            if (reservations == null)
            {
                return;
            }

            foreach (var reservation in reservations)
            {
                var (populatedReservation, result) = await GetFlightAndUserByIdsAsync(reservation.FlightId, reservation.UserId);

                if (result.Succeeded)
                {
                    reservation.Flight = populatedReservation.Flight;
                    reservation.User = populatedReservation.User;
                }
                
            }

        }

        private async Task PopulateFlightAndUserAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                return;
            }

            var (populatedReservation, result) = await GetFlightAndUserByIdsAsync(reservation.FlightId, reservation.UserId);

            if (result.Succeeded)
            {
                reservation.Flight = populatedReservation.Flight;
                reservation.User = populatedReservation.User;
            }
        }

        private void CheckForCanceledFlight(IEnumerable<Reservation> reservations)
        {
            foreach (var reservation in reservations)
            {
                if (reservation.Flight.IsCanceled)
                {
                    reservation.Status = ReservationStatus.Canceled;
                }
            }
        }

        private void CheckForCanceledFlight(Reservation reservation)
        {
            if (reservation.Flight.IsCanceled)
            {
                reservation.Status = ReservationStatus.Canceled;
            }
        }

        public async Task<(Reservation, OperationResult)> CreateReservationAsync(CreateReservationViewModel model)
        {
            var (reservation, result) = await GetFlightAndUserByIdsAsync(model.FlightId, model.UserId);

            if (!result.Succeeded)
            {
                return (reservation, result);
            }

            if (model.NumberOfSeats > reservation.Flight.AvailableSeats)
            {
                return (reservation, OperationResult.Failure("Not enough available seats."));
            }
            if (reservation.Flight.DepartureDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber < 3)
            {
                return (reservation, OperationResult.Failure("You can't reserve seats for this flight because it's too close to the departure date."));
            }
            if (reservation.Flight.IsCanceled)
            {
                return (reservation, OperationResult.Failure("This flight has been canceled."));
            }

            reservation.FlightId = model.FlightId;
            reservation.UserId = model.UserId;
            reservation.NumberOfSeats = model.NumberOfSeats;
            reservation.Status = ReservationStatus.Pending;

            result = await _reservationRepository.CreateAsync(reservation);

            return (reservation, result);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            var reservations = await _reservationRepository.GetReservationsByIdAsync(userId);
            await PopulateFlightAndUserAsync(reservations);
            CheckForCanceledFlight(reservations);

            return reservations.OrderByDescending(r => r.Flight.DepartureDate);
        }

        public async Task<IEnumerable<Reservation>> FetchAllReservationsAsync()
        { 
            var reservations = await _reservationRepository.FetchAllReservationsAsync();
            await PopulateFlightAndUserAsync(reservations);
            CheckForCanceledFlight(reservations);

            return reservations.OrderBy(r => r.Status).ThenBy(r => r.FlightId).ThenBy(r => r.UserId);
        }

        public async Task<Reservation> GetReservationByIdAsync(Guid reservationId)
        {             
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            await PopulateFlightAndUserAsync(reservation);
            CheckForCanceledFlight(reservation);

            return reservation;
        }

        public async Task<(Reservation?, OperationResult)> GetFlightAndUserByIdsAsync(Guid flightId, Guid userId)
        {
            var flight = await _flightService.GetFlightByIdAsync(flightId);
            var user = await _userService.GetAccountByIdAsync(userId);

            if (flight == null || user == null)
            {
                return (null, OperationResult.Failure("Flight or user not found"));
            }

            var reservation = new Reservation
            {
                FlightId = flightId,
                UserId = userId,
                Flight = flight,
                User = user
            };

            return (reservation, OperationResult.Success());
        }

        public async Task<(Reservation, OperationResult)> ChangeReservationStatusByIdAsync(Guid reservationId, ReservationStatus newStatus)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);

            if (reservation == null)
            {
                return (reservation, OperationResult.Failure("No reservation found."));
            }

            await PopulateFlightAndUserAsync(reservation);

            if (reservation.Flight == null || reservation.User == null)
            {
                return (reservation, OperationResult.Failure("Missing flight or user."));
            }

            if (reservation.Flight.IsCanceled)
            {
                return (reservation, OperationResult.Failure("The flight is canceled."));
            }

            if (reservation.Status != ReservationStatus.Pending)
            {
                return (reservation, OperationResult.Failure("Reservation is already confirmed/ canceled."));
            }

            if (newStatus == ReservationStatus.Confirmed && reservation.NumberOfSeats > reservation.Flight.AvailableSeats)
            {
                return (reservation, OperationResult.Failure("Not enough seats available."));
            }

            var result = OperationResult.Success();

            if (newStatus == ReservationStatus.Confirmed)
            {
                result = await _flightService.ReserveSeatsAsync(reservation.FlightId, reservation.NumberOfSeats);

                if (!result.Succeeded)
                {
                    return (reservation, result);
                }
            }

            reservation.Status = newStatus;

            result = await _reservationRepository.UpdateAsync(reservation);

            if (newStatus == ReservationStatus.Confirmed && !result.Succeeded)
            {
                // cursed
                await _flightService.ReserveSeatsAsync(reservation.FlightId, -reservation.NumberOfSeats);
            }

            return (reservation, result);
        }
    }
}
