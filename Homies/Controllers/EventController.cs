namespace Homies.Controllers
{
    using Homies.Models;
    using Homies.Services.Contract;
    using Microsoft.AspNetCore.Mvc;
    using System.Globalization;
    using System.Security.Claims;

    public class EventController : BaseController
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var events = await this.eventService.GetAllEventsAsync();
            return View(events);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddEventViewModel();
            model.Types = await this.eventService.GetAllTypesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventViewModel model)
        {
            
            if (!ModelState.IsValid)
            {                
                model.Types = await this.eventService.GetAllTypesAsync();
                return View(model);
            }

            bool isStartDateFormatValid = DateTime.TryParseExact(model.Start, "yyyy-MM-dd H:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime startDate);
            bool isEndDateFormatValid = DateTime.TryParseExact(model.End, "yyyy-MM-dd H:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime endDate);

            if (!isStartDateFormatValid || !isEndDateFormatValid)
            {
                model.Types = await this.eventService.GetAllTypesAsync();
                return View(model);
            }
            if (endDate < startDate)
            {               
                model.Types = await this.eventService.GetAllTypesAsync();
                return View(model);
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.eventService.AddEventAsync(model, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var events = await this.eventService.ListOfJoinedEventsAsync(userId);

            return View(events);
        }

        public async Task<IActionResult> Join(int id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == 0 || userId == null)
            {
                return RedirectToAction(nameof(All));
            }

            await this.eventService.AddUserToEventAsync(id, userId);

            return RedirectToAction(nameof(Joined));
        }

        public IActionResult Leave(int id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == 0 || userId == null)
            {
                return RedirectToAction(nameof(All));
            }

            this.eventService.LeaveUserFromEvent(id, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.eventService.ReturnAddEventViewModel(id);
            model.Types = await this.eventService.GetAllTypesAsync();
            return View(model);
         
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEventViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Types = await this.eventService.GetAllTypesAsync();
                return View(model);
            }

            bool isStartDateFormatValid = DateTime.TryParseExact(model.Start, "yyyy-MM-dd H:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime startDate);
            bool isEndDateFormatValid = DateTime.TryParseExact(model.End, "yyyy-MM-dd H:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime endDate);

            if (!isStartDateFormatValid || !isEndDateFormatValid)
            {
                model.Types = await this.eventService.GetAllTypesAsync();
                return View(model);
            }
                       

            await this.eventService.EditEvent(model);

            return RedirectToAction(nameof(All));            
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = this.eventService.GetDetails(id);
            return View(model);
        }

    }
}
