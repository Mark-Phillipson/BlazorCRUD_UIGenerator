﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace DynamicCRUD.T4Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class GenericTable : GenericTableBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\n");
            
            #line 11 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
     if (!string.IsNullOrWhiteSpace(ForeignKeyName)) {
            
            #line default
            #line hidden
            this.Write("@page \"/");
            
            #line 12 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PluralTablename.ToLower()));
            
            #line default
            #line hidden
            this.Write("/{");
            
            #line 12 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ForeignKeyName));
            
            #line default
            #line hidden
            this.Write(":int}\"\r\n");
            
            #line 13 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
     } else { 
            
            #line default
            #line hidden
            this.Write("@page \"/");
            
            #line 14 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PluralTablename.ToLower()));
            
            #line default
            #line hidden
            this.Write("\"\r\n");
            
            #line 15 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
 }
            
            #line default
            #line hidden
            this.Write("<div class=\"row\">\r\n<div class=\"col-md-12\">\r\n");
            
            #line 18 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
           if (!UseBlazored) {
            
            #line default
            #line hidden
            this.Write(@"        @if (ApplicationState!= null &&  !string.IsNullOrWhiteSpace(ApplicationState.Message))
        {
            <div class=""alert alert-@ApplicationState.MessageType alert-dismissible fade show"" role=""alert"">
                <strong>Note!</strong> @ApplicationState.Message @DateTime.Now.ToShortTimeString()
                <button type=""button"" class=""btn-close"" @onclick=""@(() => HideMessage())"" aria-label=""Close""></button>
            </div>
        }
");
            
            #line 26 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
           }
            
            #line default
            #line hidden
            this.Write("    </div>\r\n    <div class=\"col-md-8\">\r\n        <h5 class=\"m-1\">@Title</h5>\r\n    " +
                    "</div>\r\n");
            
            #line 31 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
if (!string.IsNullOrWhiteSpace(ForeignKeyName)) {
            
            #line default
            #line hidden
            this.Write("@*    <div class=\"col-md-1\">\r\n        <a class=\"btn btn-primary btn-sm m-2\" href=" +
                    "\"/ParentTable/@");
            
            #line 33 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ForeignKeyName));
            
            #line default
            #line hidden
            this.Write("\"><i class=\"fas fa-arrow-left\"></i></a>\r\n   </div>*@\r\n");
            
            #line 35 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
} 
            
            #line default
            #line hidden
            this.Write("    <div class=\"col-md-4\">\r\n        <input @ref=\"SearchInput\" accesskey=\"i\" class" +
                    "=\"form-control my-1\" style=\"float:right\" @bind=\"@SearchTerm\" \r\n        @bind:eve" +
                    "nt=\"oninput\" id=\"SearchInput\" name=\"SearchTerm\" placeholder=\"Filter by ");
            
            #line 38 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FilterColumns));
            
            #line default
            #line hidden
            this.Write(" (Alt+I)\" />\r\n    </div> \r\n</div>\r\n\r\n@if (Filtered");
            
            #line 42 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write(@"DTO == null && _loadFailed == true)
{
    <h2 class=""text-danger"">The data failed to load please try again in a little while..</h2>
    <button class=""btn btn-danger"" type=""button"" @onclick=""@( () => LoadData() )"">Reload</button>
    <h6 class=""text-danger"">@ExceptionMessage</h6>
}
else if (Filtered");
            
            #line 48 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("DTO == null)\r\n{\r\n    <div style=\"display:normal;margin:auto\" class=\"loader\"></div" +
                    ">\r\n}\r\nelse if (Filtered");
            
            #line 52 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("DTO.Count == 0)\r\n{\r\n    <p>No ");
            
            #line 54 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameWithSpaces));
            
            #line default
            #line hidden
            this.Write(" match the criteria</p>\r\n    <button @onclick=\"AddNew");
            
            #line 55 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("\" class=\"btn btn-outline-success\">Add Record <i class=\"fas fa-plus\"></i></button>" +
                    "\r\n}\r\nelse\r\n{\r\n    <table style=\"font-size:small\" class=\"table table-bordered tab" +
                    "le-striped table-sm table-responsive-sm\">\r\n        <thead>\r\n            <tr>\r\n");
            
            #line 62 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
 
            foreach (var property in DatabaseColumns)  {

            
            #line default
            #line hidden
            
            #line 65 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
 
                if (property.Sort) {

            
            #line default
            #line hidden
            this.Write("              <th>\r\n                    <button class=\"btn btn-sm\" @onclick=\"@(()" +
                    " => Sort");
            
            #line 68 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("(\"");
            
            #line 68 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyName));
            
            #line default
            #line hidden
            this.Write("\"))\">\r\n                        <i class=\"fas fa-sort-alpha-up\"></i>\r\n            " +
                    "        </button>\r\n                    <button class=\"btn btn-sm\" @onclick=\"@(()" +
                    " => Sort");
            
            #line 71 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("(\"");
            
            #line 71 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyName));
            
            #line default
            #line hidden
            this.Write(" Desc\"))\">\r\n                        <i class=\"fas fa-sort-alpha-up-alt\"></i>\r\n   " +
                    "                 </button>\r\n                    ");
            
            #line 74 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Label));
            
            #line default
            #line hidden
            this.Write("\r\n                </th>\r\n");
            
            #line 76 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"

                } else {

            
            #line default
            #line hidden
            this.Write("              <th>");
            
            #line 78 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Label));
            
            #line default
            #line hidden
            this.Write("</th>\r\n");
            
            #line 79 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"

                }
            }

            
            #line default
            #line hidden
            this.Write("                <th>\r\n                    <button accesskey=\"+\" @onclick=\"AddNew");
            
            #line 84 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("\" class=\"btn btn-outline-success btn-sm\">\r\n                        <i class=\"fas " +
                    "fa-plus\"></i>\r\n                    </button>\r\n                </th>\r\n           " +
                    " </tr>\r\n        </thead>\r\n        <tbody>\r\n            @foreach (var ");
            
            #line 91 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameCamelCase));
            
            #line default
            #line hidden
            this.Write(" in Filtered");
            
            #line 91 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("DTO)\r\n            {\r\n                <tr>\r\n");
            
            #line 94 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
 
            foreach (var property in DatabaseColumns)  {

            
            #line default
            #line hidden
            
            #line 97 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"

                if (property.PropertyName==PrimaryKeyName) {

            
            #line default
            #line hidden
            this.Write("                    <td>\r\n                        <button class=\"btn btn-sm btn-l" +
                    "ink\" @onclick=\"@(() => Edit");
            
            #line 101 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 101 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameCamelCase));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 101 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PrimaryKeyName));
            
            #line default
            #line hidden
            this.Write("))\">\r\n                            @");
            
            #line 102 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameCamelCase));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 102 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PrimaryKeyName));
            
            #line default
            #line hidden
            this.Write("\r\n                        </button>\r\n                    </td>\r\n");
            
            #line 105 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
              } else {

            
            #line default
            #line hidden
            this.Write("                    <td>@");
            
            #line 107 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameCamelCase));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 107 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyName));
            
            #line default
            #line hidden
            this.Write("</td>\r\n");
            
            #line 108 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
              } 
             }

            
            #line default
            #line hidden
            this.Write("                    <td class=\"btn-group-vertical\">\r\n                        <but" +
                    "ton class=\"btn btn-sm btn-outline-warning\" @onclick=\"@(() => Edit");
            
            #line 112 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 112 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameCamelCase));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 112 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PrimaryKeyName));
            
            #line default
            #line hidden
            this.Write("))\">\r\n                            <i class=\"fas fa-edit\"></i>\r\n                  " +
                    "      </button>\r\n                        <button class=\"btn btn-sm btn-outline-d" +
                    "anger\" @onclick=\"@(() => Delete");
            
            #line 115 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 115 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameCamelCase));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 115 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PrimaryKeyName));
            
            #line default
            #line hidden
            this.Write("))\">\r\n                            <i class=\"fas fa-trash\"></i>\r\n                 " +
                    "       </button>\r\n                    </td>\r\n                </tr>\r\n            " +
                    "}\r\n        </tbody>\r\n    </table>\r\n}\r\n");
            
            #line 124 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
           if (!UseBlazored) {
            
            #line default
            #line hidden
            this.Write(@"@if (ShowEdit)
{
    <dialog open=""true"" style=""position:absolute;left: 0px;top: 200px;z-index: 900"" class=""border border-danger"">
        <button class=""btn btn-outline-secondary m-2"" style=""float:right"" type=""submit"" @onclick=""@( ( ) => ToggleModal(  ) )"">x</button>
        <div>
        <");
            
            #line 130 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write("AddEdit Title=\"@EditTitle\" ");
            
            #line 130 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(PrimaryKeyName));
            
            #line default
            #line hidden
            this.Write("=\"@");
            
            #line 130 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelName));
            
            #line default
            #line hidden
            this.Write(@"Id"" CloseModal=""@CloseModalAsync"" ParentId=""@ParentId"" />
        </div>
    </dialog>

}

@if (ShowDeleteConfirm)
{
    <dialog open=""true"" style=""position:absolute;left: 0px;top: 200px;z-index: 900"" class=""border border-danger"">
        <button class=""btn btn-outline-secondary m-2"" style=""float:right"" type=""submit"" @onclick=""@( ( ) => ToggleShowDeleteConfirm(  ) )"">x</button>
        <BlazoredModalConfirmDialogMVC Confirm=""@CloseConfirmDeletion"" ButtonColour=""danger"" Icon=""fa fa-trash"" Message=""Please confirm that you wish to delete a record."" Title=""Deletion Confirmation ");
            
            #line 140 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ModelNameWithSpaces));
            
            #line default
            #line hidden
            this.Write(" Table\" />\r\n        <form method=\"dialog\">\r\n        </form>\r\n    </dialog>\r\n}\r\n");
            
            #line 145 "C:\Users\MPhil\source\repos\BlazorCRUD_UIGenerator\DynamicCRUD\T4Templates\GenericTable.tt"
           }
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class GenericTableBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
