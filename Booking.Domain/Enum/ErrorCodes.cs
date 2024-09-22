namespace Booking.Domain.Enum;

public enum ErrorCodes
{
    // от 0 до 10 для Facilities
    FacilitiesNotFound = 0,
    FacilityNotFound = 1,
    FacilityAlreadyExists = 2,
    //от 11 до 20 для User
    UserNotFound = 11,
    UserAlreadyExists = 12,
    UsersNotFound = 13,
    //от 21 до 30 для Auth/Register
    PasswordNotEqualsPasswordConfirm = 21,
    PasswordIsWrong = 22,
    //от 31 до 40 для Order
    OrdersNotFound = 31,
    OrderNotFound = 32,
    //от 41 до 50 для Role
    RoleAlreadyExists = 41,
    RoleNotFound = 42,
    UserAlreadyExistsThisRole = 43,
    InternalServerError = 10
}