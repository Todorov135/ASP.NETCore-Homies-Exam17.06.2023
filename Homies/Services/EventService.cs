namespace Homies.Services
{
    using Homies.Data;
    using Homies.Data.Models;
    using Homies.Models;
    using Homies.Services.Contract;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    

    public class EventService : IEventService
    {
        private readonly HomiesDbContext data;

        public EventService(HomiesDbContext data)
        {
            this.data = data;
        }        

        public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync()
        {
            return await this.data.Events
                .Select(e => new EventViewModel()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Organiser = e.Organiser.UserName,
                        Start = e.Start.ToString("yyyy-MM-dd H:mm"),
                        Type = e.Type.Name
                    })
                .ToListAsync();
        }

        public async Task<IEnumerable<TypeViewModel>> GetAllTypesAsync()
        {
            return await this.data.Types
                .Select(t => new TypeViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                .ToListAsync();
        }

        public async Task AddEventAsync(AddEventViewModel model, string userId)
        {
            var eventToAdd = new Event()
            {
                Name = model.Name,
                Description = model.Description,                
                Start = DateTime.Parse(model.Start),
                End = DateTime.Parse(model.End),               
                TypeId = model.TypeId
            };
            eventToAdd.CreatedOn = DateTime.Now;

            eventToAdd.OrganiserId = userId;

            await data.Events.AddAsync(eventToAdd);
            await data.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventViewModel>> ListOfJoinedEventsAsync(string userId)
        {
            return await this.data.EventParticipants
                    .Where(h => h.HelperId == userId)
                    .Select(e => e.Event)
                    .Select(e => new EventViewModel()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Organiser = e.Organiser.UserName,
                        Start = e.Start.ToString("yyyy-MM-dd H:mm"),
                        Type = e.Type.Name
                    })
                    .ToListAsync();
        }

        public async Task AddUserToEventAsync(int eventId, string userId)
        {
            var ep = new EventParticipant()
            {
                EventId = eventId,
                HelperId = userId
            };
                       
            if (this.data.EventParticipants.Contains(ep))
            {
                return;
            }

            await this.data.EventParticipants.AddAsync(ep);
            await this.data.SaveChangesAsync();
        }

        public void LeaveUserFromEvent(int eventId, string userId)
        {
            var ep = new EventParticipant()
            {
                EventId = eventId,
                HelperId = userId
            };

             this.data.EventParticipants.Remove(ep);
             this.data.SaveChanges();
        }

        public async Task<AddEventViewModel?> ReturnAddEventViewModel(int id)
        {
            return await this.data.Events.Where(e => e.Id == id)
                .Select(e => new AddEventViewModel()
                {
                    Name = e.Name,
                    Description = e.Description,
                    End = e.End.ToString("yyyy-MM-dd H:mm"),
                    Start = e.Start.ToString("yyyy-MM-dd H:mm"),
                    TypeId = e.TypeId
                })
                .FirstOrDefaultAsync();
        }

        public async Task EditEvent(AddEventViewModel model)
        {
            Event? eventToEdit = await this.data.Events.FirstOrDefaultAsync(en => en.Id == model.Id); 

            if (eventToEdit == null)
            {
                return;
            }

            eventToEdit.Name = model.Name;
            eventToEdit.Description = model.Description;
            eventToEdit.Start = DateTime.Parse(model.Start);
            eventToEdit.End = DateTime.Parse(model.End);

            await this.data.SaveChangesAsync();

        }

        public DetailsViewModel? GetDetails(int? id)
        {
            return this.data.Events
                            .Where(e => e.Id == id)
                            .Select(e => new DetailsViewModel()
                            {
                                Id = e.Id,
                                Name = e.Name,
                                Description = e.Description,
                                Start = e.Start.ToString("yyyy-MM-dd H:mm"),
                                End = e.End.ToString("yyyy-MM-dd H:mm"),
                                CreatedOn = e.CreatedOn.ToString("yyyy-MM-dd H:mm"),
                                Organiser = e.Organiser.UserName,
                                Type = e.Type.Name
                            })
                            .FirstOrDefault();
        }

      
    }
}
