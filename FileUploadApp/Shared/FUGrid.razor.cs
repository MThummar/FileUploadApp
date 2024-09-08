using System;
using FileUploadApp.Models;
using Microsoft.AspNetCore.Components;

namespace FileUploadApp.Shared
{
    public partial class FUGrid
    {
        [Parameter]
        public IList<FileUploadModel> FileUploadModels { get; set; }

        [Parameter]
        public RenderFragment? Columns { get; set; }
    }
}

