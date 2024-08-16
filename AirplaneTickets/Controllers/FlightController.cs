using Microsoft.AspNetCore.Mvc;
using AirplaneTickets.Services.Interfaces;
using AirplaneTickets.Models.ViewModels;
using AirplaneTickets.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AirplaneTickets.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public IActionResult SearchFlights()
        {
            return View(new SearchFlightsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SearchFlights(SearchFlightsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var flights = await _flightService.SearchFlightsAsync(model);

                return View("ListFlights", flights);

            }

            return View(model);
        }

        public async Task<IActionResult> ShowFlight(Guid id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);

            return View(flight);
        }

        [Authorize(Roles="Agent")]
        [HttpGet]
        public IActionResult CreateFlight()
        {
            return View();
        }

        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IActionResult> CreateFlight(CreateFlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _flightService.CreateFlightAsync(model);

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Flight added successfully.";

                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Errors[0]);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> ListFlights()
        {
            var flights = await _flightService.FetchAllFlightsAsync();

            return View(flights);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CancelFlight(Guid id)
        {
            var (flight, result) = await _flightService.CancelFlightByIdAsync(id);

            if (!result.Succeeded)
            {
                ViewBag.Error = result.Errors[0];
            }

            return View("ShowFlight", flight);
        }
    }
}
