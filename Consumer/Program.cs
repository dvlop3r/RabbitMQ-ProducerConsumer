using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            while (true)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(counter + 1 + ". Message received: {0}", message);
                        Console.WriteLine(".............................................");
                        counter++;
                    };
                    channel.BasicConsume(queue: "hello",
                        autoAck: true,
                        consumer: consumer);

                    Console.WriteLine("Press [enter] to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
