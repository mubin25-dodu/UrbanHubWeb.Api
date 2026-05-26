using System.Drawing;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.shared;
using System.Text.Json;
using Microsoft.Data.SqlClient.DataClassification;

namespace UrbanHubManagement.repo
{
    public class ParkinWallet(UrbanHubDbContext context , UserCard userCard )
    {
        public async Task<Result<WithdrawalModel>> GetWalletDetails()
        {
            var result = new Result<WithdrawalModel>();
            try
            {
                var wallet = await context.Wallets.Where(w => w.UserID == userCard.UserId).ToListAsync();
                if (wallet == null || wallet.Count == 0)
                {
                    result.Error = false;
                    result.Message = "No Data Found";
                    return result;
                }
                var with = await context.Withdrawals.Where(w=> w.UserID == userCard.UserId ).
                    OrderByDescending(w => w.Date).ToListAsync();
                var TotalWithdrawals = with.Where(w => w.Status.ToLower() == "approved").Sum(w => w.Amount);
                var currentReq = with.Where(w => w.Status.ToLower() == "pending").Sum(w => w.Amount);
                var TotalEarnings = wallet.Where(w => w.Status == true).Sum(w => w.Amount);
                var data = new WithdrawalModel()
                {
                    Withdrawals = with,
                    TotalWithdrawals = TotalWithdrawals,
                    TotalEarnings = TotalEarnings,
                    AccountBalance = TotalEarnings - TotalWithdrawals,
                    CurrentWithdrawalRequest = currentReq
                };

                result.Data = data;
                result.Error = true;
                result.Message = "";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving parking spaces.";
                result.Error = false;
                throw;
            }
            return result;
        }
        
        public Result<ParkingDetailsModel> ProcessPayment(WithdrawalModel data)
        {
            var result = new Result<ParkingDetailsModel>();
            try
            {
                var apply = new Withdrawal()
                {
                    Amount = data.Amount,
                    AccountInfo = data.AccountNumber,
                    Method = data.PaymentMethod,
                    Status = "Pending",
                    UserID = userCard.UserId,
                    Date = DateTime.Now
                };

                var notification = new Notification()
                {
                    To = userCard.UserId,
                    Message = "A new withdrawal request has been submitted.",
                    Seen = false,
                    Title = "Withdrawal Request for " + data.Amount+"BDT",
                    From = "System",
                    Date = DateTime.Now
                };

                context.Withdrawals.Add(apply);
                context.Notifications.Add(notification);
                context.SaveChanges();
                result.Error = true;
                result.Message = "Application sent successfully";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving parking spaces.";
                result.Error = false;
                throw;
            }
            return result;
        }
    }
}
