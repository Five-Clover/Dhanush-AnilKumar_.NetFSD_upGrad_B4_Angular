using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public interface ISpeakerRepository
    {
        List<SpeakersDetails> GetAll();
        void Add(SpeakersDetails speaker);
        void Delete(Guid id);
    }
}
