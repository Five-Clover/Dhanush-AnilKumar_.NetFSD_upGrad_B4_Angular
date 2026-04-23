using EMS.DAL.Data;
using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly EMSDbContext _context;

        public ParticipantRepository(EMSDbContext context)
        {
            _context = context;
        }

        public void RegisterEvent(ParticipantEventDetails data)
        {
            _context.ParticipantEvents.Add(data);
            _context.SaveChanges();
        }

        public List<ParticipantEventDetails> GetByUser(string email)
        {
            return _context.ParticipantEvents
                .Where(p => p.ParticipantEmailId == email)
                .ToList();
        }

        public void RemoveRegistration(string email, Guid eventId)
        {
            var data = _context.ParticipantEvents
                .FirstOrDefault(p => p.ParticipantEmailId == email && p.EventId == eventId);

            if (data != null)
            {
                _context.ParticipantEvents.Remove(data);
                _context.SaveChanges();
            }
        }

        public List<SessionInfo> GetSessionsByUser(string email)
        {
            return _context.ParticipantEvents
                .Where(p => p.ParticipantEmailId == email)
                .Join(_context.Sessions,
                      p => p.EventId,
                      s => s.EventId,
                      (p, s) => s)
                .ToList();
        }
    }
}
