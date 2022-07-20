using System.ComponentModel.DataAnnotations;

namespace CakeCompany.Models;

public class Product
{
    [Required]
    public Guid Id { get; set; }

    [Required] 
    public Cake Cake { get; set; }

    [Required]
    [MinLength(0)]
    public double Quantity { get; set; }

    [Required]
    public int OrderId { get; set; }
}
