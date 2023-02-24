using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.DocumentImage;

namespace mlee.Core.Repository;

public class DocumentImageRepository : CommonRepositoryBase<DocumentImageEntity>, IDocumentImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}