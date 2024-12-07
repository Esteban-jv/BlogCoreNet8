using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ArticlesController : Controller
	{
		private readonly IWorkContainer _workContainer;
		private readonly IWebHostEnvironment _webHostingEnvironment;

		public ArticlesController(IWorkContainer workContainer, IWebHostEnvironment webHostingEnvironment)
		{
			_workContainer = workContainer;
			_webHostingEnvironment = webHostingEnvironment;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Create()
		{
			ArticleVM articleVM = new ArticleVM()
			{
				Article = new BlogCore.Models.Article(),
				CategoriesList = _workContainer.Category.GetCategoriesList()
			};
			return View(articleVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ArticleVM articleVM)
		{
			if (ModelState.IsValid)
			{
				string mainRoute = _webHostingEnvironment.WebRootPath;
				var files = HttpContext.Request.Form.Files;
				if (articleVM.Article.Id == 0 && files.Count > 0)
				{
					// Nuevo artículo
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(mainRoute, @"images\articles");
					var extension = Path.GetExtension(files[0].FileName);

					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStreams);
					}

					articleVM.Article.UrlImage = @"\images\articles\" + fileName + extension;
					articleVM.Article.CreateDate = DateTime.Now;

					_workContainer.Article.Add(articleVM.Article);
					_workContainer.Save();

					return RedirectToAction(nameof(Index));
				} else
				{
					ModelState.AddModelError("imagen", "Debes seleccionar una imagen");
				}
			}

			articleVM.CategoriesList = _workContainer.Category.GetCategoriesList();
			return View(articleVM);
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			ArticleVM articleVM = new ArticleVM()
			{
				Article = new BlogCore.Models.Article(),
				CategoriesList = _workContainer.Category.GetCategoriesList()
			};
			if (id != null)
			{
				articleVM.Article = _workContainer.Article.Get(id.GetValueOrDefault());
			}
			return View(articleVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ArticleVM articleVM)
		{
			if (ModelState.IsValid)
			{
				string mainRoute = _webHostingEnvironment.WebRootPath;
				var files = HttpContext.Request.Form.Files;

				var articleFromDb = _workContainer.Article.Get(articleVM.Article.Id);

				if (files.Count > 0)
				{
					// Caso nueva imagen para el artículo
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(mainRoute, @"images\articles");
					var extension = Path.GetExtension(files[0].FileName);
					var newExtension = Path.GetExtension(files[0].FileName);

					var imagePath = Path.Combine(mainRoute, articleFromDb.UrlImage.TrimStart('\\'));

					// Si ya había una imagen antes, hay que eliminarla
					if (System.IO.File.Exists(imagePath))
					{
						System.IO.File.Delete(imagePath);
					}

					// Subir la nueva imagen
					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStreams);
					}

					articleVM.Article.UrlImage = @"\images\articles\" + fileName + extension;
					articleVM.Article.CreateDate = DateTime.Now;

					//_workContainer.Article.Update(articleVM.Article);
					//_workContainer.Save();

					//return RedirectToAction(nameof(Index));
				}
				else
				{
					// Caso no se cambió la imagen
					articleVM.Article.UrlImage = articleFromDb.UrlImage;

				}

				_workContainer.Article.Update(articleVM.Article);
				_workContainer.Save();

				return RedirectToAction(nameof(Index));
			}

			articleVM.CategoriesList = _workContainer.Category.GetCategoriesList();
			return View(articleVM);
		}

		#region Llamadas a la API
		[HttpGet]
		public IActionResult GetAll()
		{
			return Json(new { data = _workContainer.Article.GetAll(includeProperties: "Category") });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var objFromDb = _workContainer.Article.Get(id);
			var routeMainPath = _webHostingEnvironment.WebRootPath;
			var imagePath = Path.Combine(routeMainPath, objFromDb.UrlImage.TrimStart('\\'));
			// Si ya había una imagen antes, hay que eliminarla
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}

			if (objFromDb == null)
			{
				return Json(new { success = false, message = "Error al borrar el artículo" });
			}
			_workContainer.Article.Remove(objFromDb);
			_workContainer.Save();
			return Json(new { success = true, message = "Artículo borrado exitosamente" });
		}
		#endregion
	}
}
