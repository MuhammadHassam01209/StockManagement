﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
var factory = new ConnectionFactory
{
    HostName = "localhost"
};

Console.WriteLine($"Stock message received: ");

var connection = factory.CreateConnection();

using
var channel = connection.CreateModel();

channel.QueueDeclare("stock", exclusive: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) => {
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Stock message received: {message}");
};

channel.BasicConsume(queue: "stock", autoAck: true, consumer: consumer);
Console.ReadKey();