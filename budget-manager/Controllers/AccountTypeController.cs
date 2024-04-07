using budget_manager.Models;
using budget_manager.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace budget_manager.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly IAccountTypeRepository accounTypeRepository;
        private readonly IUserService userService;

        public AccountTypeController(IAccountTypeRepository accounTypeRepository,
                                     IUserService userService)
        {
            this.accounTypeRepository = accounTypeRepository;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userService.GetUserId();
            var accountType = await accounTypeRepository.Get(userId);
            return View(accountType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);          
            }

            accountType.UserId = userService.GetUserId();

            var AlreadyExistsAccountType = 
                await accounTypeRepository.Exists(accountType.Name, accountType.UserId);

            if (AlreadyExistsAccountType)
            {
                ModelState.AddModelError(nameof(accountType.Name), 
                    $"El nombre {accountType.Name} ya existe.");

                return View(accountType);
            }

            await accounTypeRepository.Create(accountType);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = userService.GetUserId();
            var accountType = await accounTypeRepository.GetById(id, userId);

            if (accountType is null) 
            { 
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AccountType accountType)
        {
            var userId = userService.GetUserId();
            var existsAccountType = await accounTypeRepository.GetById(accountType.Id, userId);
        
            if (existsAccountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await accounTypeRepository.Edit(accountType);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = userService.GetUserId();
            var account_type = await accounTypeRepository.GetById(id, userId);

            if (account_type is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(account_type);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountType(int id)
        {
            var userId = userService.GetUserId();
            var account_type = await accounTypeRepository.GetById(id, userId);

            if (account_type is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await accounTypeRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistsAccountType(string name)
        {
            var userId = userService.GetUserId();
            var AlreadyExistsAccountType = await accounTypeRepository.Exists(name, userId);
        
            if (AlreadyExistsAccountType)
            {
                return Json($"El nombre {name} ya existe");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromBody] int[] ids)
        {
            var userId = userService.GetUserId();
            var accountType = await accounTypeRepository.Get(userId);
            var idsAccountType = accountType.Select(x => x.Id);

            var idsAccountTypeDoesNotUser = ids.Except(idsAccountType).ToList();

            if (idsAccountTypeDoesNotUser.Count > 0)
            {
                return Forbid();
            }

            var accountTypeOrder = ids.Select((value, index) => new AccountType { Id = value, Orden = index + 1}).AsEnumerable();

            await accounTypeRepository.Order(accountTypeOrder);

            return Ok();
        }
    }
}