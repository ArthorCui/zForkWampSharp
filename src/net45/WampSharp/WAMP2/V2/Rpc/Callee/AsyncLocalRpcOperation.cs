﻿#if NET45

using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public abstract class AsyncLocalRpcOperation: LocalRpcOperation
    {
        protected AsyncLocalRpcOperation(string procedure) : base(procedure)
        {
        }

        protected async override void InnerInvoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {
                Task<object> task =
                    InvokeAsync(caller,
                               formatter,
                               options,
                               arguments,
                               argumentsKeywords);

                object result = await task;

                caller.Result(options, new object[] {result});
            }
            catch (WampException ex)
            {
                caller.Error(ex.Details, ex.ErrorUri);
            }
        }

        protected abstract Task<object> InvokeAsync<TMessage>(IWampRpcOperationCallback caller,
                                                              IWampFormatter<TMessage> formatter,
                                                              TMessage options,
                                                              TMessage[] arguments,
                                                              IDictionary<string, TMessage> argumentsKeywords);
    }
}

#endif