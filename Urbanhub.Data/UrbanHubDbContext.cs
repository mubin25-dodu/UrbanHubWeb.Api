using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UrbanHub.Entities;

namespace UrbanHub.Data;

public partial class UrbanHubDbContext(DbContextOptions<UrbanHubDbContext> options) : DbContext(options)
{

    public virtual DbSet<Email> Emails { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ParkingSpace> ParkingSpaces{ get; set; }
    public virtual DbSet<Log> Logs { get; set; }
    public virtual DbSet<ParkingBooking> ParkingBookings { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Wallet> Wallets { get; set; }
    public virtual DbSet<PlatformWallet> PlatformWallets { get; set; }
    public virtual DbSet<Withdrawal> Withdrawals { get; set; }
    public virtual DbSet<Services> Services{ get; set; }

}
