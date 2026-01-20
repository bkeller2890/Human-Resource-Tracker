using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HRTracker.TagHelpers
{
    [HtmlTargetElement("create-button")]
    public class CreateButtonTagHelper : TagHelper
    {
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string ClassAttributeName = "class";

    private const string HrefAttributeName = "href";

        [HtmlAttributeName(ActionAttributeName)]
        public string? Action { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string? Controller { get; set; }

        [HtmlAttributeName(ClassAttributeName)]
        public string? Class { get; set; }

    [HtmlAttributeName(HrefAttributeName)]
    public string? Href { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;

            // Render a plain anchor with the desired class and content. We avoid leaving mvc 'asp-*' attributes
            // which would otherwise be processed by the AnchorTagHelper and can result in unexpected output
            // in the test host. If desired, href can be computed here.
            if (!string.IsNullOrEmpty(Class))
            {
                output.Attributes.SetAttribute("class", Class);
            }
            else
            {
                output.Attributes.SetAttribute("class", "btn btn-success");
            }

            if (!string.IsNullOrEmpty(Href))
            {
                output.Attributes.SetAttribute("href", Href);
            }

            output.Content.SetContent("Create New");
        }
    }
}
