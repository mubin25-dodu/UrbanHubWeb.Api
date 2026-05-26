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
    public class AdminTransactions(UrbanHubDbContext context , UserCard userCard)
    {
        public async Task<Result<List<Withdrawal>>> Get(string searchTerm)
        {
            var result = new Result<List<Withdrawal>>();
            try
            {
                var getwithdrawals = new List<Withdrawal>();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    getwithdrawals = await context.Withdrawals.Where(w 
                        => w.ID != userCard.UserId && (w.User.Name.Contains(searchTerm) || 
                                                       w.User.Email.Contains(searchTerm) || 
                                                       w.User.Phone.Contains(searchTerm)||
                                                       w.AccountInfo.Contains(searchTerm))||
                           w.Status.Contains(searchTerm)||
                           w.Method.Contains(searchTerm)).Include(u=>u.User).ToListAsync();
                }
                else
                {
                    getwithdrawals = await context.Withdrawals.Where(w => w.ID != userCard.UserId ).
                        Include(u => u.User).ToListAsync();
                }
                if (getwithdrawals == null)
                {
                    result.Error = false;
                    result.Message = "No Withdrawals Found";
                    return result;
                }
                result.Data = getwithdrawals;
                result.Error = true;

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
        public async Task<Result<Withdrawal>> PrecoessTask(int id , string task)
        {
            var result = new Result<Withdrawal>();
            try
            {
                var getwithdrawals = await context.Withdrawals.FindAsync(id);


                if (getwithdrawals == null)
                {
                    result.Error = false;
                    result.Message = "No Withdrawals Found";
                    return result;
                }
                var logupdate = new Log();
                var notification = new Notification();

                if (task.ToLower() == "approved")
                {
                    getwithdrawals.Status = "Approved";
                    notification.Message = $"Your withdrawal request For {getwithdrawals.Amount} has been Approved " +
                                           $"and Send to Your account";
                    result.Message = $"withdrawal request For {getwithdrawals.Amount} has been Approved " +
                                     $"and Send to the account";
                    var wallet = new Wallet()
                    {
                        Amount = getwithdrawals.Amount,
                        UserID = getwithdrawals.UserID,
                        Date = DateTime.Now
                    };
                    context.Wallets.Update(wallet);
                }
                else
                {
                    notification.Message = $"Your withdrawal request For {getwithdrawals.Amount} has been Declined ";
                    result.Message = $"Withdrawal request For {getwithdrawals.Amount} has been Declined ";
                    getwithdrawals.Status = "Declined";
                }

                logupdate.Message = $"Withdrawal with ID {getwithdrawals.ID} has been processed by " +
                                    $"admin with UserID {userCard.UserId}.";
                logupdate.UpdatedAt = DateTime.Now;
                logupdate.UpdatedBy = userCard.UserId;

                notification.Date = DateTime.Now;
                notification.To = userCard.UserId;
                notification.From = "UrbanHUb";

                

                context.Logs.Add(logupdate);
                context.Notifications.Add(notification);
                context.Withdrawals.Update(getwithdrawals);
                await context.SaveChangesAsync();

                result.Data = getwithdrawals;
                result.Error = true;
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
