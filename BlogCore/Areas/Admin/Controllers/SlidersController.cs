using BlogCore.DataAccess.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SlidersController : Controller
	{
		private readonly IWorkContainer _workContainer;
		private readonly IWebHostEnvironment _webHostingEnvironment;

		public SlidersController(IWorkContainer workContainer, IWebHostEnvironment webHostingEnvironment)
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
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Slider slider)
		{
			if (ModelState.IsValid)
			{
				string mainRoute = _webHostingEnvironment.WebRootPath;
				var files = HttpContext.Request.Form.Files;
				if (slider.Id == 0 && files.Count > 0)
				{
					// Nuevo artículo
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(mainRoute, @"images\sliders");
					var extension = Path.GetExtension(files[0].FileName);

					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStreams);
					}

					slider.UrlImage = @"\images\sliders\" + fileName + extension;

					_workContainer.Slider.Add(slider);
					_workContainer.Save();

					return RedirectToAction(nameof(Index));
				} else
				{
					ModelState.AddModelError("imagen", "Debes seleccionar una imagen");
				}
			}
			return View(slider);
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			Slider slider = new Slider();
			slider = _workContainer.Slider.Get(id);
			if (slider == null)
			{
				return NotFound();
			}
			return View(slider);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Slider slider)
		{
			if (ModelState.IsValid)
			{
				string mainRoute = _webHostingEnvironment.WebRootPath;
				var files = HttpContext.Request.Form.Files;

				var sliderFromDB = _workContainer.Slider.Get(slider.Id);

				if (files.Count > 0)
				{
					// Caso nueva imagen para el artículo
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(mainRoute, @"images\sliders");
					var extension = Path.GetExtension(files[0].FileName);
					var newExtension = Path.GetExtension(files[0].FileName);

					var imagePath = Path.Combine(mainRoute, sliderFromDB.UrlImage.TrimStart('\\'));

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

					slider.UrlImage = @"\images\sliders\" + fileName + extension;

					//_workContainer.Article.Update(articleVM.Article);
					//_workContainer.Save();

					//return RedirectToAction(nameof(Index));
				}
				else
				{
					// Caso no se cambió la imagen
					slider.UrlImage = sliderFromDB.UrlImage;

				}

				_workContainer.Slider.Update(slider);
				_workContainer.Save();

				return RedirectToAction(nameof(Index));
			}

			return View(slider);
		}

		#region Llamadas a la API
		[HttpGet]
		public IActionResult GetAll()
		{
			return Json(new { data = _workContainer.Slider.GetAll() });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var objFromDb = _workContainer.Slider.Get(id);
			var routeMainPath = _webHostingEnvironment.WebRootPath;
			var imagePath = Path.Combine(routeMainPath, objFromDb.UrlImage.TrimStart('\\'));
			// Si ya había una imagen antes, hay que eliminarla
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}

			if (objFromDb == null)
			{
				return Json(new { success = false, message = "Error al borrar el slider" });
			}
			_workContainer.Slider.Remove(objFromDb);
			_workContainer.Save();
			return Json(new { success = true, message = "Slider borrado exitosamente" });
		}
		#endregion
	}
}
