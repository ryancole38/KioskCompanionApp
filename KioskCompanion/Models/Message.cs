using System;
using System.Text;

namespace KioskCompanion.Models
{

    public interface IPackageObject
    {
        event EventHandler OnMessageCompleted;
    }

    public class Message : IPackageObject
    {
        private MessageOptions Options;

        public string LastMessage { get; internal set; } = "";

        public int TotalPacketsExpected { get; internal set; } = 0;

        public int PacketsReceived { get; internal set; } = 0;

        public string[] Packets { get; internal set; }

        public event EventHandler OnMessageCompleted;

        public Message()
        {
            Options = new MessageOptions();
        }

        public Message(MessageOptions Options)
        {
            this.Options = Options;
        }

        public void AddPacket(string Packet)
        {
            int Index = GetPacketNumber(Packet);
            if (Packets == null)
                InitializePacketArray(Packet);

            if (Packets[Index] == null || Options.ReplaceDuplicatePackets)
                AddPacketToArray(Packet);
            
            LastMessage = Packet;
            if (MissingPackets() == 0) OnMessageCompleted?.Invoke(this, EventArgs.Empty);
        }

        public string BuildCompletedMessage()
        {
            if (MissingPackets() > 0)
                throw new Exception("Cannot build message if not all packets have been received.");
            StringBuilder CompletedMessage = new StringBuilder();
            foreach(string Packet in Packets)
                CompletedMessage.Append(Packet);

            return CompletedMessage.ToString();
        }

        public bool EqualsLastPacket(string Packet)
        {
            return LastMessage.Equals(Packet);
        }

        public int MissingPackets()
        {
            return TotalPacketsExpected - PacketsReceived;
        }

        private int GetPacketNumber(string Packet)
        {
            string IndexString = Packet.Substring(0, Options.NumberPacketHeaderSize);

            int Index = DecodeHeaderSection(IndexString) - 1;
            return Index;
        }

        private int GetTotalPacketsExpected(string Packet)
        {
            string IndexString = Packet.Substring(Options.NumberPacketHeaderSize + 1, Options.TotalPacketHeaderSize);
            int Index = DecodeHeaderSection(IndexString);
            
            return Index;
        }

        private int DecodeHeaderSection(string headerSection)
        {
            int value;
            if (Options.HeaderEncoding == MessageOptions.HeaderEncodingType.Base64)
                value = ConvertBase64ToInt(headerSection);
            else if (Options.HeaderEncoding == MessageOptions.HeaderEncodingType.PlainText)
                value = Convert.ToInt32(headerSection);
            else throw new Exception("Header encoding type unspecified");

            return value;
        }

        private int ConvertBase64ToInt(string ToConvert)
        {
            return BitConverter.ToInt32(Convert.FromBase64String(ToConvert));
        }

        private void InitializePacketArray(string Packet)
        {
            int NumberOfPackets = GetTotalPacketsExpected(Packet);
            TotalPacketsExpected = NumberOfPackets;
            Packets = new string[NumberOfPackets];
        }

        private string RemoveMetaDataFromPacket(string Packet)
        {
            string Trimmed = Packet.Substring(Options.NumberPacketHeaderSize + Options.TotalPacketHeaderSize + 1);
            return Trimmed;
        }

        private void AddPacketToArray(string Packet)
        {
            int Index = GetPacketNumber(Packet);
            if (Packets[Index] == null)
                PacketsReceived++;
            Packets[Index] = RemoveMetaDataFromPacket(Packet);
        }

        #region IPackageObjectOnMessageCompleted
        event EventHandler IPackageObject.OnMessageCompleted
        {
            add
            {
                lock (Packets)
                {
                    OnMessageCompleted += value;
                }
            }

            remove
            {
                lock (Packets)
                {
                    OnMessageCompleted -= value;
                }
            }
        }
        #endregion
    }
}