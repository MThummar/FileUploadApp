using System;
using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace FileUploadApp.Blazor.Components
{
    public partial class FUButton : FileUploadAppComponentBase
    {
        /// <summary>
        /// Defines general style of button.
        /// </summary>
        [Parameter]
        public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Primary;

        [Parameter]
        public string? Text { get; set; }

        /// <summary>
        /// Set Icon on the left side of the text. If icon is null, bydefault it is null.
        /// </summary>
        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public bool IsFullWidth { get; set; }

        
        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

       

        private string _buttonClasses => new CssBuilder()
                                                .AddClass("button")
                                                .AddClass("primary", ButtonStyle == ButtonStyle.Primary)
                                                .AddClass("secondary", ButtonStyle == ButtonStyle.Secondary)
                                                .AddClass("outlined", ButtonStyle == ButtonStyle.Outlined)
                                                .AddClass("disabled, IsDisabled")
                                                .AddClass(Class, !string.IsNullOrWhiteSpace(Class))
                                                .Build();

        private string _buttonStyle => new StyleBuilder()
                                                .AddStyle("width", "100%", IsFullWidth)
                                                .Build();
    }
    public enum ButtonStyle
    {
        Primary,
        Secondary,
        Outlined
    }
}

