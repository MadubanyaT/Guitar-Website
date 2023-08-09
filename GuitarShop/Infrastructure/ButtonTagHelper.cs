using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GuitarShop.Infrastructure
{
    [HtmlTargetElement("button", Attributes = "[type=submit]")] //being more specific of which button to apply this addtaghelper
    public class ButtonTagHelper : TagHelper
    {
        //we're targerting buttons
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //output => 
            output.Attributes.SetAttribute("class", "btn btn-success");
        }
    }
}

