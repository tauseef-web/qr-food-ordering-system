namespace ScanAndSavor.Models;

public enum OrderType
{
    DineIn = 1,
    Takeaway = 2
}

public enum OrderStatus
{
    Draft = 0,
    PaymentPending = 1,
    Placed = 2,
    Accepted = 3,
    Preparing = 4,
    Ready = 5,
    Served = 6,
    Completed = 7,
    Cancelled = 8,
    Rejected = 9,
    PaymentFailed = 10
}

public enum PaymentStatus
{
    NotRequired = 0,
    Pending = 1,
    Paid = 2,
    Failed = 3,
    Refunded = 4
}
