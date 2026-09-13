using System;
using System.Collections.Generic;
using System.Text;

namespace GOtica.Domain.Repositories.Treatment;

public interface ITreatmentWriteOnlyRepository
{
    Task Add(Entities.Treatment treatment);
}
