using Microsoft.AspNetCore.Mvc;
using AirplaneTickets.Services.Interfaces;
using AirplaneTickets.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AirplaneTickets.Models.Entities;
using Microsoft.Identity.Client;
using AirplaneTickets.Models.Enums;


namespace AirplaneTickets.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize(Roles = "Visitor")]
        [HttpGet]
        public async Task<IActionResult> CreateReservation(Guid flightId)
        { 
            var (reservation, result) = await _reservationService.GetFlightAndUserByIdsAsync(flightId, Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

            if (!result.Succeeded)
            {
                return NotFound(result.Errors[0]);
            }

            ViewBag.Flight = reservation.Flight;
            ViewBag.User = reservation.User;

            var model = new CreateReservationViewModel
            {
                FlightId = reservation.FlightId,
                UserId = reservation.UserId
            };

            return View(model);
        }

        [Authorize(Roles = "Visitor")]
        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationViewModel model)
        {
            var (reservation, result) = await _reservationService.GetFlightAndUserByIdsAsync(model.FlightId, model.UserId);

            ViewBag.Flight = reservation.Flight;
            ViewBag.User = reservation.User;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            (reservation, result) = await _reservationService.CreateReservationAsync(model);

            if (result.Succeeded)
            {
                return View("ShowReservation", reservation);
            }
            else
            {
                ViewBag.ErrorMessage = result.Errors[0];
            }

            return View(model);
        }

        [Authorize(Roles = "Visitor")]
        [HttpGet]
        public async Task<IActionResult> MyReservations()
        {
            var reservations = await _reservationService.GetReservationsByUserIdAsync(Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

            return View("ListReservations", reservations);
        }

        [Authorize(Roles = "Agent")]
        [HttpGet]
        public async Task<IActionResult> ListReservations()
        {
            var reservations = await _reservationService.FetchAllReservationsAsync();

            return View(reservations);
        }
        

        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IActionResult> ChangeReservationStatus(Guid reservationId, ReservationStatus newStatus)
        {
            var (reservation, result) = await _reservationService.ChangeReservationStatusByIdAsync(reservationId, newStatus);

            if (!result.Succeeded)
            {
                ViewBag.ErrorMessage = result.Errors[0];
            }

            return View("ShowReservation", reservation);
        }

        [Authorize(Roles = "Agent, Visitor")]
        [HttpGet]
        public async Task<IActionResult> ShowReservation(Guid id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            return View(reservation);
        }

    }
}
