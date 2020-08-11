using GS.Core.Data;
using GS.Core.Domain.Works;
using GS.Services.Events;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GS.Services.Works
{
    public class WorkFileService:IWorkFileService
    {
        #region Fields

        private readonly IEventPublisher _eventPubisher;
        private readonly IRepository<WorkFile> _workFileRepository;

        #endregion

        #region Ctor

        public WorkFileService(IEventPublisher eventPubisher,
            IRepository<WorkFile> workFileRepository)
        {
            _eventPubisher = eventPubisher;
            _workFileRepository = workFileRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a WorkFile
        /// </summary>
        /// <param name="WorkFileId">WorkFile identifier</param>
        /// <returns>WorkFile</returns>
        public virtual WorkFile GetWorkFileById(int Id)
        {
            if (Id == 0)
                return null;

            return _workFileRepository.GetById(Id);
        }
        public virtual List<WorkFile> GetWorkFileByIds(string Ids)
        {
            if (string.IsNullOrEmpty(Ids))
                return new List<WorkFile>();
            var lsids = Ids.Split(',').Select(c => Convert.ToInt32(c)).ToArray();
            return _workFileRepository.Table.Where(c => lsids.Contains(c.Id)).ToList();
        }

        /// <summary>
        /// Gets a WorkFile by GUID
        /// </summary>
        /// <param name="WorkFileGuid">WorkFile GUID</param>
        /// <returns>WorkFile</returns>
        public virtual WorkFile GetWorkFileByGuid(Guid guid)
        {
            if (guid == Guid.Empty)
                return null;

            var query = _workFileRepository.Table.Where(c => c.FileGuid == guid);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Deletes a WorkFile
        /// </summary>
        /// <param name="WorkFile">WorkFile</param>
        public virtual void DeleteWorkFile(WorkFile item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _workFileRepository.Delete(item);

            _eventPubisher.EntityDeleted(item);
        }

        /// <summary>
        /// Inserts a WorkFile
        /// </summary>
        /// <param name="WorkFile">WorkFile</param>
        public virtual void InsertWorkFile(WorkFile item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _workFileRepository.Insert(item);

            _eventPubisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates the WorkFile
        /// </summary>
        /// <param name="WorkFile">WorkFile</param>
        public virtual void UpdateWorkFile(WorkFile item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _workFileRepository.Update(item);

            _eventPubisher.EntityUpdated(item);
        }



        /// <summary>
        /// Gets the WorkFile binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>WorkFile binary array</returns>
        public virtual byte[] GetWorkFileBits(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return fileBytes;
                }
            }
        }

        #endregion
    }
}
