using AutoRep.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AutoRep.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<SUser> _userManager;
        private readonly SignInManager<SUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<SUser> userManager,
            SignInManager<SUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [DataType(DataType.Password)]
            [Display(Name = "Текущий пароль")]
            [Required(ErrorMessage = "Данное поле необхдимо")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Данное поле необхдимо")]
            [StringLength(100, ErrorMessage = "{0} должен быть длинной от {2} до {1}.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите новый пароль")]
            [Compare("NewPassword", ErrorMessage = "Новый и старый пароли на совпадают.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("Пароль пользователя был успешно изменён.");
            StatusMessage = "Ваш пароль был изменён.";

            return RedirectToPage();
        }
    }
}