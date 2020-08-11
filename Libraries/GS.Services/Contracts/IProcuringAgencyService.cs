using GS.Core;
using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IProcuringAgencyService
    {
        #region ProcuringAgency

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">ProcuringAgency type</param>
        void DeleteProcuringAgency(ProcuringAgency item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>ProcuringAgency types</returns>
        IList<ProcuringAgency> GetAllProcuringAgencys(string Name="");

        IPagedList<ProcuringAgency> GetProcuringAgencys(int TypeId=0, string Name = "", bool isInEVN = true, int pageIndex = 0,int pageSize = int.MaxValue);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">ProcuringAgency type identifier</param>
        /// <returns>ProcuringAgency type</returns>
        ProcuringAgency GetProcuringAgencyById(int itemId);
        ProcuringAgency GetProcuringAgencyByPECC1();

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">ProcuringAgency type</param>
        void InsertProcuringAgency(ProcuringAgency item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">ProcuringAgency type</param>
        void UpdateProcuringAgency(ProcuringAgency item);

        Decimal GetTotalProcuringAgency(int year = 0);

        Decimal GetTotalProcuringAgencyIsInEVN(bool isInEVN);

        #endregion
    }
}
