using System;
using KioskCompanion.Models;

namespace KioskCompanion.ViewModels
{
    public partial class QRScanViewModel: BaseViewModel
    {
        public string Text { get; set; }

        public Message Transmission { get; set; }

        public QRScanViewModel()
        {

            Text = "Nothing scanned yet";

            InitializeMessage();
        }

        public void InitializeMessage()
        {
            MessageOptions options = new MessageOptions()
            {
                HeaderEncoding = MessageOptions.HeaderEncodingType.PlainText,
                TotalPacketHeaderSize = 4,
                NumberPacketHeaderSize = 4
            };

            Transmission = new Message(options);
        }
    }
}
