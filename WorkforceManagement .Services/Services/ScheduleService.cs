using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WorkforceManagement.Services.Contracts;

namespace WorkforceManagement.Services.Services
{
    public class ScheduleService : IHostedService
    {
        private Timer _timer;
        public IServiceProvider _services;


        public ScheduleService(IServiceProvider services)
        {
            _services = services;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
            SetAutomaticSumOfPaidDaysOff,
            null,
            TimeSpan.Zero,
            TimeSpan.FromDays(30));

            return Task.CompletedTask;
        }

        private void SetAutomaticSumOfPaidDaysOff(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var scopedService =
                scope.ServiceProvider
                    .GetRequiredService<ITimeOffRequestService>();

                scopedService.SumOldWithNewPaidDaysOff();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
