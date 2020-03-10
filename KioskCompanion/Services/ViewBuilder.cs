using System;
using KioskCompanion.Models;
using Xamarin.Forms;

namespace KioskCompanion.Services
{
    public class ViewBuilder
    {

        public static View BuildView(SerializableViewElement root)
        {
            View ToReturn = null;

            switch (root.Type)
            {
                case "StackLayout":
                    ToReturn = BuildStackLayout((SerializableStackLayout)root);
                    break;
                case "Label":
                    ToReturn = BuildLabel((SerializableLabel)root);
                    break;
            }
            return ToReturn;
        }

        private static StackLayout BuildStackLayout(SerializableStackLayout root)
        {
            StackLayout ToReturn = new StackLayout();
            ToReturn.Orientation = GetStackOrientation(root.Orientation);
            ToReturn.HorizontalOptions = GetLayoutOptions(root.HorizontalOptions);
            ToReturn.VerticalOptions = GetLayoutOptions(root.VerticalOptions);
            BuildChildren(ToReturn, root);
            return ToReturn;
        }

        private static Label BuildLabel(SerializableLabel element)
        {
            Label label = new Label();
            label.Text = element.Text;
            label.HorizontalOptions = GetLayoutOptions(element.HorizontalOptions);
            label.VerticalOptions = GetLayoutOptions(element.VerticalOptions);
            if(element.TextColor != null && element.TextColor != "")
                label.TextColor = GetColor(element.TextColor);
            label.FontSize = element.FontSize;
            label.FontAttributes = GetFontAttributes(element.FontAttributes);
            return label;
        }

        private static StackOrientation GetStackOrientation(string orientation)
        {
            switch (orientation)
            {
                case "Horizontal":
                    return StackOrientation.Horizontal;
                case "Vertical":
                    return StackOrientation.Vertical;
            }
            return StackOrientation.Vertical;
        }

        private static LayoutOptions GetLayoutOptions(string option)
        {
            LayoutOptionsConverter converter = new LayoutOptionsConverter();
            if (option != null && option != "")
                return (LayoutOptions)converter.ConvertFromInvariantString(option);

            return LayoutOptions.Fill;
        }

        private static void BuildChildren(StackLayout view, SerializableStackLayout root)
        {
            foreach(SerializableViewElement child in root.Children)
            {
                view.Children.Add(BuildView(child));
            }
        }

        private static Color GetColor(string colorName)
        {
            ColorTypeConverter converter = new ColorTypeConverter();
            return (Color)converter.ConvertFromInvariantString(colorName);
        }

        private static FontAttributes GetFontAttributes(string attribute)
        {
            if (attribute != null && attribute != "")
                return (FontAttributes)((new FontAttributesConverter()).ConvertFromInvariantString(attribute));
            return FontAttributes.None;
        }
    }
}
