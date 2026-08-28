using GOtica.Communication.Requests.Client;
using GOtica.Communication.Requests.Invite;
using GOtica.Communication.Requests.OpticalStore;
using GOtica.Communication.Requests.Product;
using GOtica.Communication.Requests.Supplier;
using GOtica.Communication.Requests.User;
using GOtica.Communication.Requests.UserOpticalStore;
using GOtica.Communication.Response.Product;

namespace GOtica.Communication.Requests;

public static class RequestNormalizationExtensions
{
    extension(RequestRegisterClient request)
    {
        public RequestRegisterClient Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                PhoneNumber = DataNormalizer.PhoneNumber(request.PhoneNumber),
                Email = DataNormalizer.OptionalEmail(request.Email)
            };
        }
    }

    extension(RequestUpdateClient request)
    {
        public RequestUpdateClient Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                PhoneNumber = DataNormalizer.PhoneNumber(request.PhoneNumber),
                Email = DataNormalizer.OptionalEmail(request.Email)
            };
        }
    }

    extension(RequestInvite request)
    {
        public RequestInvite Normalize()
        {
            return request with
            {
                GuestEmail = DataNormalizer.Email(request.GuestEmail),
                Role = DataNormalizer.Role(request.Role)
            };
        }
    }

    extension(RequestOpticalStore request)
    {
        public RequestOpticalStore Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                Email = DataNormalizer.Email(request.Email),
                PhoneNumber = DataNormalizer.PhoneNumber(request.PhoneNumber),
                TaxNumber = DataNormalizer.TaxNumber(request.TaxNumber)
            };
        }
    }

    extension(RequestReactivateUser request)
    {
        public RequestReactivateUser Normalize()
        {
            return request with
            {
                Email = DataNormalizer.Email(request.Email)
            };
        }
    }

    extension(RequestRegisterUser request)
    {
        public RequestRegisterUser Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                Email = DataNormalizer.Email(request.Email)
            };
        }
    }

    extension(RequestUpdateUser request)
    {
        public RequestUpdateUser Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                Email = DataNormalizer.Email(request.Email)
            };
        }
    }

    extension(RequestChangeRole request)
    {
        public RequestChangeRole Normalize()
        {
            return request with
            {
                Role = DataNormalizer.Role(request.Role)
            };
        }
    }

    extension(RequestLogin request)
    {
        public RequestLogin Normalize()
        {
            return request with
            {
                Email = DataNormalizer.Email(request.Email)
            };
        }
    }

    extension(RequestRegisterProduct request)
    {
        public RequestRegisterProduct Normalize()
        {
            return request with
            {
                ProductCode = DataNormalizer.ProductCode(request.ProductCode)
            };
        }
    }

    extension(RequestUpdateProduct request)
    {
        public RequestUpdateProduct Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                ProductCode = DataNormalizer.ProductCode(request.ProductCode)
            };
        }
    }

    extension(RequestAdjustProductStock request)
    {
        public RequestAdjustProductStock Normalize()
        {
            return request with
            {
                Reason = DataNormalizer.Text(request.Reason)
            };
        }
    }

    extension(RequestRegisterSupplier request)
    {
        public RequestRegisterSupplier Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                PhoneNumber = DataNormalizer.OptionalPhoneNumber(request.PhoneNumber),
                Email = DataNormalizer.OptionalEmail(request.Email)
            };
        }
    }

    extension(RequestUpdateSupplier request)
    {
        public RequestUpdateSupplier Normalize()
        {
            return request with
            {
                Name = DataNormalizer.Text(request.Name),
                PhoneNumber = DataNormalizer.OptionalPhoneNumber(request.PhoneNumber),
                Email = DataNormalizer.OptionalEmail(request.Email)
            };
        }
    }
}
