using LineTenTest.SharedKernel.ApiModels;

namespace LineTenTest.Api.Commands;

public class CreateProductCommand
{
    public CreateProductRequest Request { get; }

    public CreateProductCommand(CreateProductRequest request)
    {
        Request = request;
    }
}