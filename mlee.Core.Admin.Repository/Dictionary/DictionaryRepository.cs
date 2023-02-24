using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.Dictionary;

namespace mlee.Core.Repository;

public class DictionaryRepository : CommonRepositoryBase<DictionaryEntity>, IDictionaryRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}