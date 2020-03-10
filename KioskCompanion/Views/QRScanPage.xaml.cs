using System;
using System.Collections.Generic;
using KioskCompanion.ViewModels;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing;
using KioskCompanion.Models;
using Newtonsoft.Json;
using KioskCompanion.Services;

namespace KioskCompanion.Views
{
    public partial class QRScanPage : ContentPage
    {
        QRScanViewModel viewModel;

        MobileBarcodeScanner Scanner;

        public QRScanPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new QRScanViewModel();
        }

        void Scan_Clicked(object sender, EventArgs e)
        {
            #if __ANDROID__
	        MobileBarcodeScanner.Initialize (Application);
            #endif

            viewModel.Text = "";

            viewModel.Transmission = new Message();
            Action<Result> Handler = new Action<Result>(ScanHandler);
            viewModel.Transmission.OnMessageCompleted += CancelScanning;

            Scanner = new MobileBarcodeScanner();
            MobileBarcodeScanningOptions Options = new MobileBarcodeScanningOptions() { DelayBetweenAnalyzingFrames = 50, DelayBetweenContinuousScans = 50 };
            Scanner.ScanContinuously(Options, Handler);
        }

        void BuildDynamicContent(string viewString)
        {
            /*
             string viewString = @"{
	                            'Type': 'StackLayout',
	                            'Orientation': 'Vertical',
	                            'Children':[
		                            {
			                            'Type': 'StackLayout',
			                            'Orientation': 'Horizontal',
			                            'HorizontalOptions': 'CenterAndExpand',
			                            'VerticalOptions': 'CenterAndExpand',
			                            'Children':[
				                            {
					                            'Type': 'Label',
					                            'Text': 'Hello world!',
					                            'HorizontalOptions': 'CenterAndExpand'
				                            },
				                            {
					                            'Type': 'Label',
					                            'Text': 'Another label!',
					                            'HorizontalOptions': 'CenterAndExpand'
				                            }
			                            ]
		                            },
		                            {
			                            'Type': 'Label',
			                            'HorizontalOptions': 'CenterAndExpand',
			                            'VerticalOptions': 'CenterAndExpand',
			                            'Text': 'This label is not a child'
		                            }
	                            ]
                            }";
            */
            ViewElement deserializedView = JsonConvert.DeserializeObject<ViewElement>(viewString);
            StackLayout content = (StackLayout)ViewBuilder.BuildView(deserializedView);

            Button scanButton = new Button(){ HorizontalOptions = LayoutOptions.CenterAndExpand, Text = "Start Scanning" };
            scanButton.Clicked += Scan_Clicked;

            content.Children.Add(scanButton);

            ScrollView scroll = new ScrollView();
            scroll.Content = content;

            Device.BeginInvokeOnMainThread(() =>
            {
                Content = scroll;
            });
        }

        void ScanHandler(Result result)
        {
            viewModel.Transmission.AddPacket(result.Text);
        }

        void CancelScanning(object sender, EventArgs e)
        {
            Scanner.Cancel();
            //viewModel.Text = viewModel.Transmission.BuildCompletedMessage();
            BuildDynamicContent(viewModel.Transmission.BuildCompletedMessage());
        }
    }
}
