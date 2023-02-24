using AspectCore.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace mlee.Core.DB.Transaction
{

    public class TransactionInterceptor : IInterceptor
    {
        private readonly TransactionAsyncInterceptor _transactionAsyncInterceptor;

        public TransactionInterceptor(TransactionAsyncInterceptor transactionAsyncInterceptor)
        {
            _transactionAsyncInterceptor = transactionAsyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            _transactionAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}
