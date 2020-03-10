using System;
namespace KioskCompanion.Models
{
    public class SerializableLabel : SerializableViewElement
    {
        public string Text { get; set; }
        public string TextColor { get; set; }
        public double FontSize { get; set; }
        public string FontAttributes { get; set; }
    }
}
