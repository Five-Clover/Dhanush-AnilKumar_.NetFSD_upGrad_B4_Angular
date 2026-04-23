using EMS.DAL.Data;
using EMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly EMSDbContext _context;

        public SessionRepository(EMSDbContext context)
        {
            _context = context;
        }

        public List<SessionInfo> GetAll()
        {
            return _context.Sessions
                .Include(s => s.Event)
                .Include(s => s.Speaker)
                .ToList();
        }
        public void Add(SessionInfo session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public void AssignSpeaker(Guid sessionId, Guid speakerId)
        {
            var session = _context.Sessions.Find(sessionId);

            if (session != null)
            {
                session.SpeakerId = speakerId;
                _context.SaveChanges();
            }
        }

        public void Update(SessionInfo model)
        {
            var existing = _context.Sessions.Find(model.SessionId);

            if (existing != null)
            {
                existing.SessionTitle = model.SessionTitle;
                existing.Description = model.Description;
                existing.EventId = model.EventId;
                existing.SpeakerId = model.SpeakerId;
                existing.SessionStart = model.SessionStart;
                existing.SessionEnd = model.SessionEnd;
                existing.SessionUrl = model.SessionUrl;

                _context.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var s = _context.Sessions.Find(id);
            if (s != null)
            {
                _context.Sessions.Remove(s);
                _context.SaveChanges();
            }
        }
    }
}
