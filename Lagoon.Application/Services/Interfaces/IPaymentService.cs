using Lagoon.Domain.Entities;
using Stripe.Checkout;

namespace Lagoon.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        SessionCreateOptions CreateStripeSessionOptions(Booking booking, Villa villa, string domain);
        Session CreateStripeSession(SessionCreateOptions options);

    }
}
