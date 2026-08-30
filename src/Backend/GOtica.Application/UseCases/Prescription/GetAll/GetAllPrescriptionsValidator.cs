using FluentValidation;
using GOtica.Communication.Requests.Prescription;
using GOtica.Exceptions.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace GOtica.Application.UseCases.Prescription.GetAll;

internal class GetAllPrescriptionsValidator : AbstractValidator<RequestGetAllPrescriptions>
{
    public GetAllPrescriptionsValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.PAGE_INVALID);

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(ResourceMessagesException.PAGE_SIZE_INVALID);
    }
}
