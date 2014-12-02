﻿using System.ServiceModel;
using PS.Mothership.Core.Common.Dto;
using Quartz;

namespace PS.Mothership.Core.Common.Contracts
{
    [ServiceContract(Name="DiallerTaskNotificationService", CallbackContract = typeof (IDiallerTaskNotificationCallback))]
    public interface IDiallerTaskNotificationService
    {
        [OperationContract(IsOneWay = false)]
        void Subscribe(string applicationName);

        [OperationContract(IsOneWay = true)]
        void EndSubscribe(string applicationName);

        [OperationContract]
        void AddNewTask(TaskDto taskDto);

        [OperationContract]
        void UpdateTask(TaskDto taskDto);

        [OperationContract]
        void CompleteTask(long pendingNotificationId);

        [OperationContract]
        void PushPendingTaskNotifications(string userName);

        void ExecuteJob(IJobExecutionContext context);
    }
}
