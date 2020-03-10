using System;
using KioskCompanion.Models;
using Xamarin.Forms;

namespace KioskCompanion.Services
{
    public class ViewBuilder
    {
        public ViewBuilder()
        {
        }

        public static View BuildView(ViewElement root)
        {
            View ToReturn = null;

            switch (root.Type)
            {
                case "StackLayout":
                    ToReturn = BuildStackLayout(root);
                    break;
                case "Label":
                    ToReturn = BuildLabel(root);
                    break;
            }
            return ToReturn;
        }

        private static StackLayout BuildStackLayout(ViewElement root)
        {
            StackLayout ToReturn = new StackLayout();
            ToReturn.Orientation = GetStackOrientation(root.Orientation);
            ToReturn.HorizontalOptions = GetLayoutOptions(root.HorizontalOptions);
            ToReturn.VerticalOptions = GetLayoutOptions(root.VerticalOptions);
            BuildChildren(ToReturn, root);
            return ToReturn;
        }

        private static Label BuildLabel(ViewElement element)
        {
            Label label = new Label();
            label.Text = element.Text;
            label.HorizontalOptions = GetLayoutOptions(element.HorizontalOptions);
            label.VerticalOptions = GetLayoutOptions(element.VerticalOptions);
            if(element.TextColor != null && element.TextColor != "")
                label.TextColor = GetColor(element.TextColor);
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
            switch (option) {
                case "Center":
                    return LayoutOptions.Center;
                case "CenterAndExpand":
                    return LayoutOptions.CenterAndExpand;
                case "End":
                    return LayoutOptions.End;
                case "EndAndExpand":
                    return LayoutOptions.EndAndExpand;
                case "Fill":
                    return LayoutOptions.Fill;
                case "FillAndExpand":
                    return LayoutOptions.FillAndExpand;
                case "Start":
                    return LayoutOptions.Start;
                case "StartAndExpand":
                    return LayoutOptions.StartAndExpand;
            }
            return LayoutOptions.Fill;
        }

        private static void BuildChildren(StackLayout view, ViewElement root)
        {
            foreach(ViewElement child in root.Children)
            {
                view.Children.Add(BuildView(child));
            }
        }

        private static Color GetColor(string colorName)
        {
            ColorTypeConverter converter = new ColorTypeConverter();
            return (Color)converter.ConvertFromInvariantString(colorName);
        }
    }
}
