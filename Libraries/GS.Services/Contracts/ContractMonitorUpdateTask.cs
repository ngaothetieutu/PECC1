using Castle.Core.Logging;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Data;
using GS.Services.Contracts;
using GS.Services.Events;
using GS.Services.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public partial class ContractMonitorUpdateTask : IScheduleTask
    {
        #region Fields
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        #endregion

        #region Ctor
        public ContractMonitorUpdateTask(
            IDbContext dbContext,
            IDataProvider dataProvider)
        {
            this._dbContext = dbContext;
            this._dataProvider = dataProvider;
        }
        #endregion

        #region Methods
        public virtual void Execute()
        {
            _dbContext.ExecuteSqlCommand("EXEC [SP_ContractMonitorTask]");
        }
        #endregion
    }
}
