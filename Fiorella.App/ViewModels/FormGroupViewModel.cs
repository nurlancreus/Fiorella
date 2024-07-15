namespace Fiorella.App.ViewModels
{
    public class FormGroupViewModel
    {
        public string LabelFor { get; set; } = string.Empty;
        public string LabelText { get; set; } = string.Empty;
        public string InputType { get; set; } = string.Empty;
        public string InputClass { get; set; } = string.Empty;
        public string ValidationClass { get; set; } = string.Empty;
        public string Accept { get; set; } = string.Empty; // Default to empty string for non-file inputs
    }

}
