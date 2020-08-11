using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GS.Services.Contracts
{
    public class ContractMonitorService : IContractMonitorService
    {
        #region Fields
        private readonly IRepository<ContractMonitor> _itemRepository;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        #region Ctor
        public ContractMonitorService(IRepository<ContractMonitor> itemRepository,
            IRepository<Contract> contractRepository,
            IEventPublisher eventPublisher)
        {
            this._itemRepository = itemRepository;
            this._contractRepository = contractRepository;
            this._eventPublisher = eventPublisher;
        }
        #endregion
        #region Methods
        public virtual IList<ContractMonitor> GetAllContractsMonitor()
        {
            return _itemRepository.Table
                   .ToList();
        }
        public virtual ContractMonitor GetContractsMonitorById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _itemRepository.GetById(itemId);
        }
        public IList<ContractMonitor> GetListContractMonitorbylistId(List<int> listId)
        {
            return _itemRepository.Table.Where(c => listId.Contains(c.Id)).ToList();
        }
        public virtual void InsertContractMonitor (ContractMonitor item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public virtual void UpdateContractMonitor (ContractMonitor item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        public virtual void DeleteContractMonitor (ContractMonitor item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _itemRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }
        public virtual IList<Contract> GetContractsMonitorDelayPayment()
        {
            var query = _itemRepository.Table
                .Where(c => c.StatusIds.Contains(((int)ContractMonitorStatus.OverduePayment).ToString()))
                .Select(c => c.ContractId);
            var ls = _contractRepository.Table.Where(c => query.Contains(c.Id));

            return ls.ToList();
        }
        #endregion
    }
}
