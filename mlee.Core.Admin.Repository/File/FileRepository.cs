using  mlee.Core.DB.Transaction;
using  mlee.Core.Infrastructure.Entities.File;
using mlee.Core.Admin.Repository;

namespace mlee.Core.Repository;

public class FileRepository : CommonRepositoryBase<FileEntity>, IFileRepository
{
    public FileRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}