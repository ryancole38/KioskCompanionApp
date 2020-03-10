using System;
using System.Collections.Generic;

namespace KioskCompanion.Models
{
    public class SerializableStackLayout : SerializableViewElement
    {
        public List<SerializableViewElement> Children { get; set; }
        public string Orientation { get; set; }
    }
}
