using System;
namespace KioskCompanion.Models
{
    public class MessageOptions
    {
        public int NumberPacketHeaderSize { get; set; }

        public int TotalPacketHeaderSize { get; set; }

        public bool ReplaceDuplicatePackets { get; set; }

        public HeaderEncodingType HeaderEncoding { get; set; }

        public enum HeaderEncodingType
        {
            Base64,
            PlainText
        }

        public MessageOptions()
        {
            NumberPacketHeaderSize = 4;

            TotalPacketHeaderSize = 4;

            ReplaceDuplicatePackets = false;

            HeaderEncoding = HeaderEncodingType.PlainText;
        }
    }
}
