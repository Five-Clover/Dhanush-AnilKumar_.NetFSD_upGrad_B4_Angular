using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public interface IParticipantRepository
    {
        void RegisterEvent(ParticipantEventDetails data);
        List<ParticipantEventDetails> GetByUser(string email);
        void RemoveRegistration(string email, Guid eventId);
        List<SessionInfo> GetSessionsByUser(string email);
    }
}
