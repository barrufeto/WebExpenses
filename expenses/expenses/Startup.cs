using Microsoft.Owin;
using Owin;
using Hangfire;
using expenses.Models;

[assembly: OwinStartupAttribute(typeof(expenses.Startup))]
namespace expenses
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            HangFireController Scheduled = new HangFireController();

            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //BackgroundJob.Enqueue(() => Scheduled.SchedulesGastos());

            //s'executa cada dia
            RecurringJob.AddOrUpdate(() => Scheduled.SchedulesGastos(),Cron.Daily);

            //S'executa el dia 2 de cada mes
            RecurringJob.AddOrUpdate(() => Scheduled.ResumsGastos(), "0 0 2 * *");

            //S'executa el dia 3 de cada mes
            RecurringJob.AddOrUpdate(() => Scheduled.SendMonthlyEmails(), "0 0 3 * *");
            
            //Scheduled.SendMonthlyEmails();

        }
    }
}
