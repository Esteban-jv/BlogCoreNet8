﻿using BlogCore.Data;
using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.DataAccess.Data.Repository
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

		public IEnumerable<SelectListItem> GetCategoriesList()
		{
			return _db.Categories.Select(i => new SelectListItem()
			{
				Text = i.Name,
				Value = i.Id.ToString()
			});
		}

		public void Update(Category category)
        {
            var objFromDb = _db.Categories.FirstOrDefault(s => s.Id == category.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                objFromDb.Order = category.Order;
                // _db.SaveChanges();
            }
        }
    }
}
