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
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _db;

        public ArticleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Article article)
        {
            var objFromDb = _db.Articles.FirstOrDefault(s => s.Id == article.Id);

            if (objFromDb != null)
            {
                objFromDb.Title = article.Title;
                objFromDb.Description = article.Description;
                objFromDb.UrlImage = article.UrlImage;
                objFromDb.CategoryId = article.CategoryId;
                
                //_db.SaveChanges();
            }
        }
    }
}
