using mlee.Core.Admin.Repository;
using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.Document;

namespace mlee.Core.Repository;

public class DocumentRepository : CommonRepositoryBase<DocumentEntity>, IDocumentRepository
{
    public DocumentRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}