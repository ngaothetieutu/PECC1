using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractMonitorService
    {
        IList<ContractMonitor> GetAllContractsMonitor();
        ContractMonitor GetContractsMonitorById(int itemId);
        IList<ContractMonitor> GetListContractMonitorbylistId(List<int> listId);
        void InsertContractMonitor(ContractMonitor item);
        void UpdateContractMonitor(ContractMonitor item);
        void DeleteContractMonitor(ContractMonitor item);
        IList<Contract> GetContractsMonitorDelayPayment();
    }
}
