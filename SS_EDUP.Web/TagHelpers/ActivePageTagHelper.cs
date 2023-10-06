﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SS_EDUP.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-active-page")]
    public class ActivePageTagHelper : AnchorTagHelper
    {
        public ActivePageTagHelper(IHtmlGenerator generator)
        : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routeData = ViewContext.RouteData.Values;
            var currentController = routeData["controller"] as string;
            var currentAction = routeData["action"] as string;
            var result = false;

            if (!string.IsNullOrWhiteSpace(Controller) && !String.IsNullOrWhiteSpace(Action))
            {
                result = string.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);
            }
            else if (!string.IsNullOrWhiteSpace(Action))
            {
                result = string.Equals(Action, currentAction, StringComparison.OrdinalIgnoreCase);
            }
            else if (!string.IsNullOrWhiteSpace(Controller))
            {
                result = string.Equals(Controller, currentController, StringComparison.OrdinalIgnoreCase);
            }

            if (result)
            {
                var existingClasses = output.Attributes["class"].Value.ToString();
                if (output.Attributes["class"] != null)
                {
                    output.Attributes.Remove(output.Attributes["class"]);
                }

                output.Attributes.Add("class", $"{existingClasses} active");
            }
        }
    }
}

