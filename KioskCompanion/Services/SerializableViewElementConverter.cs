using System;
using KioskCompanion.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KioskCompanion.Services
{
    public class SerializableViewElementConverter : JsonConverter
    {
        public SerializableViewElementConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(SerializableViewElement).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string type = (string)jo["Type"];
            SerializableViewElement item;
            switch (type)
            {
                case "StackLayout":
                    item = new SerializableStackLayout();
                    break;
                case "Label":
                    item = new SerializableLabel();
                    break;
                default:
                    item = new SerializableViewElement();
                    break;
            }

            serializer.Populate(jo.CreateReader(), item);

            return item;
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
