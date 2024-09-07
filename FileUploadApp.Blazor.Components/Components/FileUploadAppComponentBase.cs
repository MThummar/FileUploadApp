using System;
using Microsoft.AspNetCore.Components;

namespace FileUploadApp.Blazor.Components
{
    public class FileUploadAppComponentBase : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> UserAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Style { get; set; }

    }
}

