using LineTenTest.Api.Dtos;

namespace LineTenTest.Api.Commands;

public class CreateOrderCommand
{
    public CreateOrderDto CreateOrderDto { get; }

    public CreateOrderCommand(CreateOrderDto createOrderDto)
    {
        CreateOrderDto = createOrderDto;
    }
}