using Procons.Durrah.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Procons.Durrah.Api.Services
{
    [ServiceContract]
    public interface IKnetService
    {
        [OperationContract]
        Transaction CallKnetGateway(Transaction transaction);

    }
}
