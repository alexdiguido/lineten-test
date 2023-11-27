namespace LineTenTest.SharedKernel.ApiModels;

public class CreateProductRequest
{
    public string Sku { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}