using mlee.Core.Admin.Repository;
using mlee.Core.DB.Transaction;
using mlee.Core.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Repository.User
{

    public class UserRepository : CommonRepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
        {

        }
    }
}
