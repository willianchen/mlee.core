﻿using FreeSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlee.Core.DataAccess.Db.Transaction
{
    public class UnitOfWorkManagerCloud
    {
        readonly Dictionary<string, UnitOfWorkManager> _managers = new Dictionary<string, UnitOfWorkManager>();
        readonly FreeSqlCloud _cloud;
        public UnitOfWorkManagerCloud(FreeSqlCloud cloud)
        {
            _cloud = cloud;
        }

        public UnitOfWorkManager GetUnitOfWorkManager(string dbKey)
        {
            if (_managers.TryGetValue(dbKey, out var uowm) == false)
            {
                _managers.Add(dbKey, uowm = new UnitOfWorkManager(_cloud.Use(dbKey)));
            }

            return uowm;
        }

        public void Dispose()
        {
            foreach (var uowm in _managers.Values) uowm.Dispose();
            _managers.Clear();
        }

        public IUnitOfWork Begin(string dbKey, Propagation propagation = Propagation.Required, IsolationLevel? isolationLevel = null)
        {
            return GetUnitOfWorkManager(dbKey).Begin(propagation, isolationLevel);
        }
    }
}