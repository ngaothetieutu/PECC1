using GS.Core.Domain.Works;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Works
{
    public interface IWorkFileService
    {
        /// <summary>
        /// Gets a WorkFile
        /// </summary>
        /// <param name="WorkFileId">WorkFile identifier</param>
        /// <returns>WorkFile</returns>
        WorkFile GetWorkFileById(int Id);
        List<WorkFile> GetWorkFileByIds(string Ids);

        /// <summary>
        /// Gets a WorkFile by GUID
        /// </summary>
        /// <param name="WorkFileGuid">WorkFile GUID</param>
        /// <returns>WorkFile</returns>
        WorkFile GetWorkFileByGuid(Guid guid);

        /// <summary>
        /// Deletes a WorkFile
        /// </summary>
        /// <param name="WorkFile">WorkFile</param>
        void DeleteWorkFile(WorkFile item);

        /// <summary>
        /// Inserts a WorkFile
        /// </summary>
        /// <param name="WorkFile">WorkFile</param>
        void InsertWorkFile(WorkFile item);

        /// <summary>
        /// Updates the WorkFile
        /// </summary>
        /// <param name="WorkFile">WorkFile</param>
        void UpdateWorkFile(WorkFile item);



        /// <summary>
        /// Gets the WorkFile binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>WorkFile binary array</returns>
        byte[] GetWorkFileBits(IFormFile file);
    }
}
