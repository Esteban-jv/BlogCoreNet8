using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Slider slider)
        {
            var objFromDb = _db.Sliders.FirstOrDefault(s => s.Id == slider.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = slider.Name;
                objFromDb.Status = slider.Status;
                objFromDb.UrlImage = slider.UrlImage;
                
                //_db.SaveChanges();
            }
        }
    }
}
