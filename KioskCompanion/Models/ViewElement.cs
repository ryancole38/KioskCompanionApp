using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KioskCompanion.Models
{
    public class ViewElement
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string Orientation { get; set; }
        public string VerticalOptions { get; set; }
        public string HorizontalOptions { get; set; }
        public List<ViewElement> Children { get; set; }
    }
}
