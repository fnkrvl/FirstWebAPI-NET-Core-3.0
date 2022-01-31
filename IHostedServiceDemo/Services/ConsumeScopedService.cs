using IHostedServiceDemo.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServiceDemo.Services
{
    public class ConsumeScopedService : IHostedService, IDisposable
    {
        private Timer _timer;
        public IServiceProvider Services { get; }

                                 // Inyección de dependencias
        public ConsumeScopedService(IServiceProvider services) 
        {
            Services = services;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1)); // Método | Estado | Tiempo de inicio | Periodo 
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        // Method for the Timer
        private void DoWork(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var message = "ConsumeScopedService. Receive message at" + DateTime.Now.ToString("dd:mm:yy hh:mm:ss");
                var log = new HostedServiceLog() { Message = message };
                context.HostedServiceLogs.Add(log);
                context.SaveChanges();
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
        
    }
}
