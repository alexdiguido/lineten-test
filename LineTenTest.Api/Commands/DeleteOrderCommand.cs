namespace LineTenTest.Api.Tests.Controllers;

public class DeleteOrderCommand
{
    public DeleteOrderRequest Request { get; }

    public DeleteOrderCommand(DeleteOrderRequest request)
    {
        Request = request;
    }
}