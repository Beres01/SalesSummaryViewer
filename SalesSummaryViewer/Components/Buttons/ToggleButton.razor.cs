using Microsoft.AspNetCore.Components;
using System.Drawing;


namespace SalesSummaryViewer.Components.Buttons
{
    public partial class ToggleButton
    {
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        //public Action OnClick { get; set; }
        public bool IsPressed {get; set; } = false;
        private string ButtonColour => IsPressed ? "#0d6efd" : "#6c757d";

        private void HandleButtonClicked()
        { 
        }

    }
}
