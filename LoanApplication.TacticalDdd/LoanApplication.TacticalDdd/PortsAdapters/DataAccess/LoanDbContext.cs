using LoanApplication.TacticalDdd.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess
{
    public class LoanDbContext : DbContext
    {
        public DbSet<DomainModel.LoanApplication> LoanApplications { get; set; }

        public DbSet<Operator> Operators { get; set; }

        public LoanDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region LoadApplication.Id

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Id)
                .HasConversion(x => x.Value, x => new LoanApplicationId(x));

            #endregion

            #region LoadApplication.Number

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Number);

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Number)
                .HasConversion(x => x.Number, x => new LoanApplicationNumber(x));

            #endregion
            
            #region LoadApplication.Status

            var loanAppStatusConverter = new EnumToStringConverter<LoanApplicationStatus>();

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .Property(l => l.Status)
                .HasConversion(loanAppStatusConverter);

            #endregion

            #region LoadApplication.Score

            var appScoreConverter = new EnumToStringConverter<ApplicationScore>();

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Score, s =>
                {
                    s.Property(x => x.Explanation);

                    s.Property(x => x.Score)
                        .HasConversion(appScoreConverter);
                });

            #endregion

            #region LoadApplication.Customer

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Customer, c =>
                {
                    c.Property(x => x.Birthdate);

                    c.OwnsOne(x => x.Address, ca =>
                    {
                        ca.Property(x => x.Country);

                        ca.Property(x => x.City);

                        ca.Property(x => x.ZipCode);

                        ca.Property(x => x.Street);
                    });

                    c.OwnsOne(x => x.NationalIdentifier, ni =>
                    {
                        ni.Property(x => x.Value);
                    });

                    c.OwnsOne(x => x.Name, cn =>
                    {
                        cn.Property(x => x.First);

                        cn.Property(x => x.Last);
                    });

                    c.OwnsOne(x => x.MonthlyIncome, ma =>
                    {
                        ma.Property(x => x.Amount);
                    });
                });

            #endregion

            #region LoanApplication.Property

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Property, p =>
                {
                    p.OwnsOne(x => x.Address, pa =>
                    {
                        pa.Property(x => x.Country);

                        pa.Property(x => x.City);

                        pa.Property(x => x.ZipCode);

                        pa.Property(x => x.Street);
                    });
                });

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Property, p =>
                {
                    p.OwnsOne(x => x.Value, pv =>
                    {
                        pv.Property(x => x.Amount);
                    });
                });

            #endregion

            #region LoadApplication.Loan

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Loan, l =>
                {
                    l.Property(x => x.LoanNumberOfYears);

                    l.OwnsOne(x => x.InterestRate, ir =>
                    {
                        ir.Property(x => x.Value);
                    });

                    l.OwnsOne(x => x.LoanAmount, la =>
                    {
                        la.Property(x => x.Amount);
                    });
                });

            #endregion

            #region LoadApplication.Decision

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Decision, d =>
                {
                    d.OwnsOne(x => x.DecisionBy, db =>
                    {
                        db.Property(y => y.Value);
                    });

                    d.Property(x => x.DecisionDate);
                });

            #endregion

            #region LoadApplication.Registration

            modelBuilder.Entity<DomainModel.LoanApplication>()
                .OwnsOne(a => a.Registration, r =>
                {
                    r.OwnsOne(x => x.RegisteredBy, db =>
                    {
                        db.Property(y => y.Value);
                    });

                    r.Property(x => x.RegistrationDate);
                });

            #endregion



            #region Operator.Id

            modelBuilder.Entity<Operator>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<Operator>()
                .Property(l => l.Id)
                .HasConversion(x => x.Value, x => new OperatorId(x));

            #endregion

            #region Operator.CompetenceLevel

            modelBuilder.Entity<Operator>()
                .OwnsOne(o => o.CompetenceLevel, cl =>
                {
                    cl.Property(c => c.Amount);

                });

            #endregion

            #region Operator.Login

            modelBuilder.Entity<Operator>()
                .Property(l => l.Login)
                .HasConversion(x => x.Value, x => new Login(x));

            #endregion

            #region Operator.Password

            modelBuilder.Entity<Operator>()
                .Property(l => l.Password)
                .HasConversion(x => x.Value, x => new Password(x));

            #endregion

            #region Operator.Name

            modelBuilder.Entity<Operator>()
                .OwnsOne(
                    l => l.Name,
                    n =>
                    {
                        n.Property(o => o.First).HasColumnName("FirstName");
                        n.Property(o => o.Last).HasColumnName("LastName");
                    });

            #endregion
        }
    }
}