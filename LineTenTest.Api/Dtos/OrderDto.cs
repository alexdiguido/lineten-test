namespace LineTenTest.Api.Dtos;

public class OrderDto
{
    public int OrderId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public ProductDto Product { get; set; }
    public CustomerDto Customer { get; set; }
}