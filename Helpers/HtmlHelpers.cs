using Microsoft.AspNetCore.Html;

namespace product_inventory_system.Helpers;

public static class HtmlHelpers
{
    public static IHtmlContent RenderOption(string value, string display, string selectedValue)
    {
        var isSelected = value == selectedValue ? "selected" : "";
        return new HtmlString($"<option value=\"{value}\" {isSelected}>{display}</option>");
    }
}

