using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public interface IEventRepository
    {
        List<EventDetails> GetAll();
        EventDetails? GetById(Guid id);
        void Add(EventDetails eventObj);
        void Update(EventDetails eventObj);
        void Delete(Guid id);
    }
}
