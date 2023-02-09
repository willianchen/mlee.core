using mlee.Core.Admin.Repository.Consts;
using mlee.Core.DB.Transaction;
using mlee.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.Admin.Repository
{
    /// <summary>
    /// 权限库基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CommonRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
    {
        public CommonRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
        {

        }
    }
}