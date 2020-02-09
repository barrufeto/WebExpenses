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

            //s'executa al acabar el dia
            //RecurringJob.AddOrUpdate(() => Scheduled.AcumulatedGastos(), "55 23 * * *");
            RecurringJob.AddOrUpdate(() => Scheduled.AcumulatedGastos(), Cron.Daily);

            //s'executa cada dia al començar
            RecurringJob.AddOrUpdate(() => Scheduled.SchedulesGastos(),Cron.Daily);
            //RecurringJob.AddOrUpdate(() => Scheduled.SchedulesGastos(), "0 1 * * *");

            //S'executa el dia 2 de cada mes
            RecurringJob.AddOrUpdate(() => Scheduled.ResumsGastos(), "0 0 2 * *");

            //S'executa el dia 3 de cada mes
            RecurringJob.AddOrUpdate(() => Scheduled.SendMonthlyEmails(), "0 0 3 * *");
            
            //Scheduled.SendMonthlyEmails();

        }
    }
}
