using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public interface ISessionRepository
    {
        List<SessionInfo> GetAll();
        void Add(SessionInfo session);
        void AssignSpeaker(Guid sessionId, Guid speakerId);
        void Update(SessionInfo session);
        void Delete(Guid id);
    }
}
