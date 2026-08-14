using FluentValidation;
using FluentValidation.Validators;
using GOtica.Exceptions.Resources;
using PhoneNumbers;

namespace GOtica.Application.SharedValidators;

public class PhoneNumberValidator<T> : PropertyValidator<T, string>
{
    private readonly PhoneNumberUtil _phoneNumberUtil;
    public PhoneNumberValidator()
    {
        _phoneNumberUtil = PhoneNumberUtil.GetInstance();
    }

    public override string Name => "PhoneNumberValidator";

    public override bool IsValid(ValidationContext<T> context, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PHONE_NUMBER_EMPTY);

            return false;
        }

        if (!IsValidPhoneNumber(phoneNumber))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PHONE_NUMBER_INVALID);
            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        try
        {
            // null = exige +351 na request
            var parsedPhoneNumber = _phoneNumberUtil.Parse(phoneNumber, null);

            return _phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}
