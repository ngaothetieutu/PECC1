using GS.Core;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Messages;
using GS.Core.Domain.Works;
using GS.Core.Infrastructure;
using GS.Services.Contracts;
using GS.Services.Messages;
using GS.Services.Security;
using GS.Services.Works;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Components;
using GS.Web.Framework.Controllers;
using GS.Web.Models.Contracts;
using GS.Web.Models.Works;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class WorkFileController: BaseWorksController
    {
        #region fields
        
        private readonly IWorkContext _workContext;
        private readonly IWorkFileService _workfileService;
        private readonly IGSFileProvider _fileProvider;
        private readonly IContractService _contractService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IPermissionService _permissionService;
        
        #endregion
        public WorkFileController(IPermissionService permissionService,
            IContractService contractService,
            IWorkTaskService workTaskService,
            IWorkContext workContext,
            IWorkFileService workfileService,
            IGSFileProvider fileProvider
            )
        {
            this._permissionService = permissionService;
            this._workTaskService = workTaskService;
            this._contractService = contractService;
            this._workContext = workContext;
            this._workfileService = workfileService;
            this._fileProvider = fileProvider;
        }
        public virtual IActionResult Index()
        {
           return View();
        }
        #region Methods
        /// <summary>
        /// Luu work file 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fileContent"></param>
        void SaveWorkFileOnDisk(WorkFile item, byte[] fileContent)
        {
            string _pathStore = item.CreatedDate.ToPathFolderStore();
            _pathStore=_fileProvider.Combine(_fileProvider.MapPath(GSDataSettingsDefaults.FolderWorkFiles), _pathStore);
            _fileProvider.CreateDirectory(_pathStore);
            var _fileStore = _fileProvider.Combine(_pathStore, item.FileGuid.ToString()+item.Extension);
            _fileProvider.WriteAllBytes(_fileStore, fileContent);
        }
        byte[] GetWorkFileContentOnDisk(WorkFile item)
        {
            var _fileStore = _fileProvider.Combine(_fileProvider.MapPath(GSDataSettingsDefaults.FolderWorkFiles), item.CreatedDate.ToPathFolderStore(), item.FileGuid.ToString()+ item.Extension);
            return _fileProvider.ReadAllBytes(_fileStore);
        }
        void DeleteWorkFileOnDisk(WorkFile item)
        {
            var _fileStore = _fileProvider.Combine(_fileProvider.MapPath(GSDataSettingsDefaults.FolderWorkFiles), item.CreatedDate.ToPathFolderStore(), item.FileGuid.ToString()+ item.Extension);
            _fileProvider.DeleteFile(_fileStore,true);
        }
        public virtual IActionResult DownloadFile(Guid downloadGuid)
        {
            var download = _workfileService.GetWorkFileByGuid(downloadGuid);
            if (download == null)
                return Content("No download record found with the specified id");

            download.FileContent = GetWorkFileContentOnDisk(download);
            //use stored data
            if (download.FileContent == null || download.FileContent.Length < 2) 
                return Content($"Download data is not available any more. Download GD={download.Id}");

            var fileName = !string.IsNullOrWhiteSpace(download.FileName) ? download.FileName : download.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.MimeType)
                ? download.MimeType
                : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.FileContent, contentType)
            {
                FileDownloadName = fileName + download.Extension
            };
        }
        [HttpPost]
        public ActionResult DeleteWorkFile(int Id,string ContractGuid, string TaskGuid)
        {
            //xoa thong tin lien ket file hop dong, neu co
            _contractService.DeleteContractFile(Id);
           
            //xoa thong tin lien ket file cong viec (task)
            if (!string.IsNullOrEmpty(TaskGuid))
            {
                
            }
            var item = _workfileService.GetWorkFileById(Id);            
            _workfileService.DeleteWorkFile(item);
            DeleteWorkFileOnDisk(item);
            return JsonSuccessMessage();
        }
        #region Upload file 
        [HttpPost]
        public ActionResult SaveDropzoneJsUploadedFiles()
        {
            List<WorkFileModel> ls = new List<WorkFileModel>();
            foreach (var file in Request.Form.Files)
            {
                string fname = file.FileName;

                //You can Save the file content here
                var item=UploadFile(file);
                if(item!=null)
                {
                    var model= item.ToModel<WorkFileModel>();                    
                    ls.Add(model);
                }

            }

            return Json(ls);

        }
        private WorkFile UploadFile(IFormFile httpPostedFile)
        {
            if (httpPostedFile == null)
            {
                return null;
            }
            
            var fileBinary = _workfileService.GetWorkFileBits(httpPostedFile);
            
            var qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                fileName = Request.Form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var fwork = new WorkFile
            {
                FileGuid = Guid.NewGuid(),
                FileContent = null,
                MimeType = contentType,
                //we store filename without extension for downloads
                FileName = _fileProvider.GetFileNameWithoutExtension(fileName),
                Extension = fileExtension,
                CreatedDate = DateTime.Now,
                CreatorId = _workContext.CurrentCustomer.Id,
                ContentLength =Convert.ToInt32(fileBinary.LongLength / 1024) //luu thanh kb
            };
            _workfileService.InsertWorkFile(fwork);
            //luu file content ra ngoai
            SaveWorkFileOnDisk(fwork, fileBinary);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return fwork;
        }
        [HttpPost]
        //do not validate request token (XSRF)
        public virtual IActionResult AsyncUpload()
        {
            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded",
                    downloadGuid = Guid.Empty
                });
            }

            var fileBinary = _workfileService.GetWorkFileBits(httpPostedFile);

            var qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                fileName = Request.Form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var fwork = new WorkFile
            {
                FileGuid = Guid.NewGuid(),
                FileContent = fileBinary,
                MimeType = contentType,
                //we store filename without extension for downloads
                FileName = _fileProvider.GetFileNameWithoutExtension(fileName),
                Extension = fileExtension,
                CreatedDate=DateTime.Now,
                CreatorId=_workContext.CurrentCustomer.Id
            };
            _workfileService.InsertWorkFile(fwork);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(fwork.ToModel<WorkFileModel>());
        }
        #endregion
        #endregion
        #region  Contract
        [HttpPost]
        public IActionResult UpdateContractWorkFiles(ContractFileModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return JsonErrorMessage();
            if (string.IsNullOrEmpty(model.FileIds))
                return JsonErrorMessage();
            var contract = _contractService.GetContractByGuid(model.ContractGuid);
            if(contract==null)
                return JsonNotFoundMessage();
            var ids = model.FileIds.Split(',');
            foreach(var id in ids)
            {
                var item = new ContractFile();
                item.ContractId = contract.Id;
                item.FileId = Convert.ToInt32(id);
                item.contractFileType = model.contractFileType;
                if (item.contractFileType == ContractFileType.All)
                    item.contractFileType = ContractFileType.Common;
                item.EntityId = model.EntityId;
                _contractService.InsertContractFile(item);
            }
            return JsonSuccessMessage();
        }
        #endregion
    }

}
