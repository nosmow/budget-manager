using budget_manager.Models;
using budget_manager.Services;
using Microsoft.AspNetCore.Mvc;

namespace budget_manager.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserService userService;

        public CategoryController(ICategoryRepository categoryRepository,
            IUserService userService)
        {
            this.categoryRepository = categoryRepository;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userService.GetUserId();
            var category = await categoryRepository.Get(userId);
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var userId = userService.GetUserId();
            category.UserId = userId;
            await categoryRepository.Create(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = userService.GetUserId();
            var category = await categoryRepository.GetById(id, userId);

            if (category is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category categoryEdit) 
        {
            if (!ModelState.IsValid)
            {
                return View(categoryEdit);
            }

            var userId = userService.GetUserId();
            var category = await categoryRepository.GetById(categoryEdit.Id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            categoryEdit.UserId = userId;
            await categoryRepository.Update(categoryEdit);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = userService.GetUserId();
            var category = await categoryRepository.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = userService.GetUserId();
            var category = await categoryRepository.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
