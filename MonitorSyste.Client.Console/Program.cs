using Microsoft.AspNet.SignalR.Client;
using MonitorSystem.Core.Model;
using MonitorSystem.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MonitorSystem.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var connection = new HubConnection("http://localhost:52482/signalr");
                var hub = connection.CreateHubProxy("monitor");

                connection.Start().ContinueWith(task =>
                {
                    if (!task.IsFaulted) Console.WriteLine("Connected");
                    else Console.WriteLine("Aconteceu algum erro durante a conexão com o servidor", task.Exception.GetBaseException());
                }).Wait();

                hub.On<string>("ReceiveMessage", data => Console.WriteLine(data));

                Thread job = new Thread(() => SendProcessToServer(hub));
                job.Start();

                Console.Read();
                connection.Stop();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.ReadLine();
        }

        private static void SendProcessToServer(IHubProxy hub)
        {
            var computer = new Computer
            {
                Ipv4 = SystemHelper.GetIPV4(),
                NameComputer = SystemHelper.GetNameComputer(),
                UserName = SystemHelper.GetUserName(),
                Processes = SystemHelper.GetProcesses(Process.GetProcesses())
            };
            var message = new MessageReturnModel
            {
                Sucesso = true,
                Mensagem = new List<string> { "Informações obtidas com sucesso." },
                Retorno = computer
            };

            hub.Invoke<string>("SendProcess", message).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    Console.WriteLine("Ocorreu algum durante o envio da mensagem ao servidor: {0}", task.Exception.GetBaseException());
            }).Wait();
        }
    }
}
