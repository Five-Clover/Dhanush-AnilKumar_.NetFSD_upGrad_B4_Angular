using EMS.DAL.Data;
using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EMSDbContext _context;

        public EventRepository(EMSDbContext context)
        {
            _context = context;
        }

        public List<EventDetails> GetAll()
        {
            return _context.Events.ToList();
        }

        public EventDetails? GetById(Guid id)
        {
            return _context.Events.Find(id);
        }

        public void Add(EventDetails eventObj)
        {
            _context.Events.Add(eventObj);
            _context.SaveChanges();
        }

        public void Update(EventDetails model)
        {
            var existing = _context.Events.Find(model.EventId);

            if (existing != null)
            {
                existing.EventName = model.EventName;
                existing.EventCategory = model.EventCategory;
                existing.EventDate = model.EventDate;
                existing.Description = model.Description;
                existing.Status = model.Status;

                _context.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var ev = _context.Events.Find(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                _context.SaveChanges();
            }
        }
    }
}
