using EMS.DAL.Data;
using EMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Repositories
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly EMSDbContext _context;

        public SpeakerRepository(EMSDbContext context)
        {
            _context = context;
        }

        public List<SpeakersDetails> GetAll()
        {
            return _context.Speakers.ToList();
        }

        public void Add(SpeakersDetails speaker)
        {
            _context.Speakers.Add(speaker);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var sp = _context.Speakers.Find(id);
            if (sp != null)
            {
                _context.Speakers.Remove(sp);
                _context.SaveChanges();
            }
        }
    }
}
