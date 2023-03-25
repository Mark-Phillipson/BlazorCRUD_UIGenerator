using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace BlazorAppEditTable.Services
{
	[Keyless]
	public partial class TableStructure
	{
		//Even though this is not a table that exists, for use with the table structure service!

		public string? FieldLabel { get; set; }
		public string? Column { get; set; }
		public string Lookup { get; set; } = "";
		public string? OptionText { get; set; }
		public int? Order { get; set; }
		public string Type { get; set; } = "";
		public int IsIdentity { get; set; }
		public short colorder { get; set; }
		public int AdjustedOrder { get; set; }
		public string? HelpText { get; set; }
		public string? Property { get; set; }
		public bool Exclude { get; set; }
		public string? InitialValue { get; set; }
		public string? Group { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public bool TextEditor { get; set; }
		public short MaxLength { get; set; }
	}
}
