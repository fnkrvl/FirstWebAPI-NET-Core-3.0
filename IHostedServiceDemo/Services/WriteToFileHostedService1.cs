using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FirstWebAPI.Services
{

    // Es un servicio que se ejecuta automaticamente, por ejemplo al consumir un web service y mantener actualizada nuestra base de datos
    // Se puede definir el tiempo en que se va a ejecutar el servicio (horas, días)
    // Es un interfaz la cual nos permite implementar dos funciones. Una función que se va a ejecutar al inicio de nuestra aplicación (inicio del servidor),
    // y otra que se va a ejecutar al final (apagado del servidor)
    public class WriteToFileHostedService1 : IHostedService, IDisposable /* Limpia los recursos del Timer */ 
    {
        private readonly IHostEnvironment _environment;  // IHostEnvironment sirve para obtener el directorio en donde nuestra aplicacion está ejecutándose
        private readonly string fileName = "File 1.txt";
        private Timer timer;

        // Inyección de dependencias
        public WriteToFileHostedService1(IHostEnvironment environment)  // Change "IHostingEnvironment" by "IHostEnvironmet", so, just Host.
        {
            _environment = environment;
        }

        // Se ejecuta una vez al momento de iniciar la aplicación (inicio del servidor)
        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService : Process Started");
            // Inicializando el Timer
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1)); // Método | Estado | Tiempo de inicio | Periodo 
            return Task.CompletedTask;
        }

        // Se ejecuta una vez al momento de cerrar la aplicación (apagado o reinicio del servidor)
        // Puede no ejecutarse nunca, si la aplicación se cierra por un error 
        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService : Process Stopped");
            // Detiene el Timer
            timer?.Change(Timeout.Infinite, 0); // ? por si acaso es nulo 
            return Task.CompletedTask;
        }

        private void WriteToFile(string message)
        {
            var path = $@"{_environment.ContentRootPath}\wwwroot\{fileName}";

            using (StreamWriter writer = new StreamWriter(path, append: true)) // StreamWriter -> Es una clase abstracta, permite escribir caracteres en secuencia
            {                                                // append permite agregar información al archivo y no eliminar lo que ya estaba.
                writer.WriteLine(message);
            }
        }
        
        // Method for the Timer
        private void DoWork(object state)
        {
            WriteToFile("WriteToFileHostedService : Doing some work " + DateTime.Now.ToString("dd/mm/yy hh:mm:ss"));
        }

        public void Dispose()
        {
            timer?.Dispose(); // Se pone el ? para que la función se ejecute cuando el Timer sea nulo
        }
    }
}
