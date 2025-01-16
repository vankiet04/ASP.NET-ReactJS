

using System.Collections.Generic;
using System.Data.OracleClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaypalServerSdk.Standard.Controllers;
using PaypalServerSdk.Standard.Models;
namespace PayPalAdvancedIntegration;
public class CheckOutController : Controller
{
    private readonly OrdersController _ordersController;
    private readonly PaymentsController _paymentController;
    private readonly Dictionary<string, CheckoutPaymentIntent> _paymentIntentMap;
    private IConfiguration _configuration { get; }
    private string _paypalClientId
    {
        get { return "AWvxrJl51eVxPbXUPf5N0RAbQc6LyZ71V2XLXv5zyzl0Z9eSbrZi8zpbnyyqaSs5JyyYxi7C1nV33WVa"; }
    }
    private string _paypalClientSecret
    {
        get { return System.Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET"); }
    }
}