namespace CakeCompany.Models;
using System.ComponentModel.DataAnnotations;

public class PaymentIn
{
    [Required]
    public bool IsSuccessful { get; set; }

    [Required]
    public bool HasCreditLimit { get; set; }
}
