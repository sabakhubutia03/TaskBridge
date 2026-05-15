using MassTransit;
using TaskBridge.Application.Messages;

namespace TaskBridge.Infrastructure.Consumers;

public class ApplicationSubmittedConsumer : IConsumer<ApplicationSubmittedMessage>
{
    public async Task Consume(ConsumeContext<ApplicationSubmittedMessage> context)
    {
        var message = context.Message;
        Console.WriteLine($"Application Accepted! " +
                          $"ApplicationId: {message.ApplicationId}, " +
                          $"TaskId: {message.TaskId}, " +
                          $"FreelancerId: {message.FreelancerId}");
    }
}