﻿using System;
using System.ServiceModel;
using PS.Mothership.Core.Common.Dto.Iridium;

namespace PS.Mothership.Core.Common.Contracts
{
    [ServiceContract]
    public interface IIridiumService : IQuartzJobBase
    {
        [OperationContract]
        void SubmitToIridium(Guid opportunityGuid);

        [OperationContract]
        IridiumConfirmResultsDto ConfirmIridiumAccount(Guid gatewayAccountGuid);

    }
}
