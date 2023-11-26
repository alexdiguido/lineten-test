namespace LineTenTest.SharedKernel.ApiModels;

public class UpdateOrderRequest
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Status { get; set; }
}