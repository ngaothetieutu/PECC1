using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    public enum WorkFileType
    {
        /// <summary>
        /// File binh thuong
        /// </summary>
        Normal=1,
        /// <summary>
        /// File ket qua cong viec
        /// </summary>
        Result=2
    }
    public class WorkFile : BaseEntity
    {
        public WorkFile()
        {
            CreatedDate = DateTime.Now;
            this.FileGuid = Guid.NewGuid();
        }
        public Guid FileGuid { get; set; }
        public String FileName { get; set; }
        public String MimeType { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public String Note { get; set; }
        public Boolean Deleted { get; set; }
        public Int32 TypeId { get; set; }
        public WorkFileType fileType
        {
            get => (WorkFileType)TypeId;
            set => TypeId = (int)value;
        }
        public byte[] FileContent { get; set; }
        public String Extension { get; set; }
        public int ContentLength { get; set; }
    }
}
