using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using UrbanHub.DTO;
using UrbanHub.Entities;

namespace UrbanHub.shared;

public class WithdrawalModel
{
   public List<Withdrawal>? Withdrawals { get; set; } 
   public decimal AccountBalance { get; set; }
   public decimal TotalEarnings { get; set; }
   public decimal TotalWithdrawals { get; set; }
   public decimal CurrentWithdrawalRequest { get; set; }
    [Required]
   public decimal Amount { get; set; }
    [Required]

    public string AccountNumber { get; set; }
    [Required]
    public string PaymentMethod { get; set; }
}