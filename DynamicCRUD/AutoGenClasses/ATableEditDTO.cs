
using System.ComponentModel.DataAnnotations;

namespace ARM_BlazorServer.DTOs
{
    public partial class ATableEditDTO
    {
        [Key]
        public int TableEditId { get; set; }        [StringLength(50)]
        public string? Table { get; set; }
        [StringLength(50)]
        public string? Column { get; set; }
        [StringLength(50)]
        public string? Label { get; set; }
        public int? Order { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        [StringLength(255)]
        public string? InitialValue { get; set; }
        [StringLength(50)]
        public string? Property { get; set; }
        [Required]
        public bool Exclude { get; set; }        [StringLength(2000)]
        public string? HelpText { get; set; }
        [StringLength(5)]
        public string? ListColWidth { get; set; }
        public bool TextEditor { get; set; }
        [StringLength(255)]
        public string? Group { get; set; }
        public bool TrackChange { get; set; }
        public bool BatchExclude { get; set; }
        public bool QuickSearch { get; set; }
        public bool UploadExclude { get; set; }
        public bool UploadCriteria { get; set; }
        [StringLength(100)]
        public string? DataType { get; set; }
        public bool ClosedList { get; set; }
    }   
}