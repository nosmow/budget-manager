using AutoMapper;
using budget_manager.Models;
using budget_manager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace budget_manager.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountTypeRepository accountTypeRepository;
        private readonly IUserService userService;
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;

        public AccountController(IAccountTypeRepository accountTypeRepository,
            IUserService userService, IAccountRepository accountRepository, IMapper mapper)
        {
            this.accountTypeRepository = accountTypeRepository;
            this.userService = userService;
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = userService.GetUserId();
            var accountWithAccountType = await accountRepository.Search(userId);

            var model = accountWithAccountType
                .GroupBy(x => x.AccountType)
                .Select(group => new IndexAccountViewModel
                {
                    AccountType = group.Key,
                    Accounts = group.AsEnumerable()
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = userService.GetUserId();
            var accountType = await accountTypeRepository.Get(userId);
            var model = new CreationAccountViewModel();
            model.AccountType = await GetAccountType(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreationAccountViewModel account)
        {
            var userId = userService.GetUserId();
            var accountType = accountTypeRepository.GetById(account.AccountTypeId, userId);
            
            if (accountType is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                account.AccountType = await GetAccountType(userId);

                return View(account);
            }

            await accountRepository.Create(account);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = userService.GetUserId();
            var account = await accountRepository.GetById(id, userId);

            if (account is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = mapper.Map<CreationAccountViewModel>(account);

            model.AccountType = await GetAccountType(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreationAccountViewModel accountEdit)
        {
            var userId = userService.GetUserId();
            var account = await accountRepository.GetById(accountEdit.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var accountType = await accountTypeRepository.GetById(accountEdit.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await accountRepository.Update(accountEdit);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = userService.GetUserId();
            var account = await accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = userService.GetUserId();
            var account = await accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await accountRepository.Delete(id);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountType(int userId)
        {
            var accountType = await accountTypeRepository.Get(userId);
            return accountType.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}