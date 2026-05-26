using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHub.Entities;
using UrbanHub.shared;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    [Authorize(Roles = "Owner")]

    public class ParkINWallet(ParkinWallet repo) : Controller
    {
        public async Task<IActionResult> MyWallet()
        {
            var result = await repo.GetWalletDetails();
            

            return View(result.Data ?? new WithdrawalModel());
        }

        [HttpPost]
        public IActionResult RequestWithdrawal(WithdrawalModel model)
        {
            ModelState.Remove("Withdrawals");
            ModelState.Remove("TotalEarnings");
            if (!ModelState.IsValid)
            {
                return View("MyWallet", model);
            }

            if (model.Amount <= 0){
                ModelState.AddModelError("Amount", "Enter a valid amount");
                return View("MyWallet", model);
            }
            else if ((model.Amount + model.CurrentWithdrawalRequest) > model.AccountBalance)
            {
                ModelState.AddModelError("Amount", "Insufficient balance");
                TempData["Error"] = true;
                TempData["Message"] = "Check If already pending request exists wait for approval";
                return RedirectToAction("MyWallet" , model);
            }

            var result = repo.ProcessPayment(model);
            if (!result.Error)
            {
                TempData["Error"] = true;
                TempData["Message"] = result.Message ?? "Withdrawal request failed";
            }

            TempData["Error"] = false;
            TempData["Message"] = result.Message ?? "Withdrawal request Successful";

            return RedirectToAction("MyWallet");
        }

    }
}

