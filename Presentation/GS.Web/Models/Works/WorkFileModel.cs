using GS.Core;
using GS.Core.Domain.Works;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Works
{
    public class WorkFileModel : BaseGSEntityModel
    {
        public Guid FileGuid { get; set; }
        public String FileName { get; set; }
        public String MimeType { get; set; }
        public MimeTypeGroup mimeTypeGroup { get {
                return MimeType.getMimeTypeGroup();
            } }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public string CreatorName { get; set; }
        public String Note { get; set; }
        public Boolean Deleted { get; set; }
        public Int32 TypeId { get; set; }
        public WorkFileType fileType
        {
            get => (WorkFileType)TypeId;
            set => TypeId = (int)value;
        }
        public byte[] FileContent { get; set; }
        /// <summary>
        /// theo Kb
        /// </summary>
        public int ContentLength { get; set; }
        public string ContentLengthText { get
            {
                if (ContentLength > 1024)
                    return Convert.ToDecimal((decimal)ContentLength / 1024m).ToString("###.#0") + "M";
                return ContentLength.ToString() + "KB";
            } }
        public String Extension { get; set; }
        public String DownloadUrl
        {
            get
            {
                return string.Format("/WorkFile/DownloadFile?downloadGuid={0}",this.FileGuid);
            }
        }
       

    }
    public class TestModel
    {
        [GSResourceDisplayName("AppWork.WorkFile.Fields.Test")]
        [UIHint("WorkFile")]
        public string WorkFileIds { get; set; }
        [UIHint("Date")]
        public DateTime WorkDate { get; set; }
        [UIHint("DateNullable")]
        public DateTime? WorkDateNull { get; set; }
        public Decimal WorkNumber { get; set; }
        public Decimal? WorkNumberNull { get; set; }
        [UIHint("WorkCurrency")]
        public String WorkCurrency { get; set; }
    }

}
