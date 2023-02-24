using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities.DictionaryType;

namespace mlee.Core.Repository;

public class DictionaryTypeRepository : CommonRepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
{
    public DictionaryTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}