using AirplaneTickets.Data;
using AirplaneTickets.Models.Entities;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirplaneTickets.Repositories.Implementations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReservationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> CreateAsync(Reservation reservation)
        {
            try
            {
                _dbContext.Reservations.Add(reservation);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (DbUpdateException ex)
            {
                return OperationResult.Failure("Unknown error.");
            }
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByIdAsync(Guid userId)
        {
            var reservations = await _dbContext.Reservations.Where(r => r.UserId == userId).ToListAsync();
            return reservations;
        }

        public async Task<IEnumerable<Reservation>> FetchAllReservationsAsync()
        {
            var reservations = await _dbContext.Reservations.ToListAsync();
            return reservations;
        }

        public async Task<Reservation> GetReservationByIdAsync(Guid reservationId)
        {
            var reservation = await _dbContext.Reservations.FirstOrDefaultAsync(r => r.Id == reservationId);

            return reservation;
        }

        public async Task<OperationResult> UpdateAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                return OperationResult.Failure("Reservation not found.");
            }

            try
            {
                _dbContext.Reservations.Update(reservation);
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
