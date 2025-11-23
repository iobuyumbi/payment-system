using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Helpers;

public class SystemIdComparer : IEqualityComparer<Farmer>
{
    public int GetHashCode(Farmer co)
    {
        if (co == null)
        {
            return 0;
        }
        return co.SystemId.GetHashCode();
    }

    public bool Equals(Farmer x1, Farmer x2)
    {
        if (object.ReferenceEquals(x1, x2))
        {
            return true;
        }
        if (object.ReferenceEquals(x1, null) ||
            object.ReferenceEquals(x2, null))
        {
            return false;
        }
        return x1.SystemId == x2.SystemId;
    }
}

public class ApplicationIdComparer : IEqualityComparer<LoanApplication>
{
    public int GetHashCode(LoanApplication co)
    {
        if (co == null)
        {
            return 0;
        }
        return co.FarmerId.GetHashCode();
    }

    public bool Equals(LoanApplication x1, LoanApplication x2)
    {
        if (object.ReferenceEquals(x1, x2))
        {
            return true;
        }
        if (object.ReferenceEquals(x1, null) ||
            object.ReferenceEquals(x2, null))
        {
            return false;
        }
        return x1.FarmerId == x2.FarmerId;
    }
}
