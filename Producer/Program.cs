using RabbitMQ.Client;
using System;
using System.Text;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                while (true)
                {
                    Console.Write("Enter your message: ");
                    string message = Console.ReadLine();
                    if (message == "stop")
                        break;

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(

                        exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body
                        );

                    Console.WriteLine("Message [{0}] sent!", message);
                    Console.WriteLine("....................................");
                }
            }
        }
    }
}
