namespace Homies.Services.Contract
{
    using Homies.Models;

    public interface IEventService
    {
        Task<IEnumerable<EventViewModel>> GetAllEventsAsync();
        Task<IEnumerable<TypeViewModel>> GetAllTypesAsync();
        Task AddEventAsync(AddEventViewModel model, string userId);

        Task<IEnumerable<EventViewModel>> ListOfJoinedEventsAsync(string userId);
        Task AddUserToEventAsync(int eventId, string userId);

        void LeaveUserFromEvent(int id, string userId);

        Task<AddEventViewModel> ReturnAddEventViewModel(int id);

        Task EditEvent(AddEventViewModel model);

        DetailsViewModel? GetDetails(int? id);
    }
}
