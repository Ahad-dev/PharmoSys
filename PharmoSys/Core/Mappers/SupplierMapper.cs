using PharmoSys.Core.Models;
using PharmoSys.Data.Entities;

namespace PharmoSys.Core.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier ToModel(this SupplierEntity entity)
        {
            if (entity == null) return null;
            return new Supplier
            {
                SupplierId = entity.SupplierId,
                SupplierName = entity.SupplierName,
                Contact = entity.Contact,
                Address = entity.Address
            };
        }
    }
}
