using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FestivalLibAdmin.Model;
using ZXing;
using ZXing.QrCode;

namespace FestivalAdministratie.Converter
{
    class QrConverter:IValueConverter//credits to Axel Jonckheere 2NMCT2 for sharing his idea and code on qrcodes
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try{
                return CreateQRCode((Contactperson)value);
            }catch(Exception)
            {
                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static BitmapImage CreateQRCode(Contactperson contact)
        {
            if (contact != null)
            {
                var sb = new StringBuilder();
                sb.AppendLine("BEGIN:VCARD");
                sb.AppendLine("N:" + contact.Name + ";");
                sb.AppendLine("ORG:" + contact.Company);
                sb.AppendLine("TITLE:" + contact.JobRole.Name);
                sb.AppendLine("EMAIL:" + contact.Email);
                sb.AppendLine("END:VCARD");

                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions { Width = 100, Height = 100 }
                };

                var bitmap = writer.Write(sb.ToString());

                BitmapImage bmp = ConvertBitmapToBitmapImage(bitmap);

                return bmp;
            }
            else
                return new BitmapImage();
        }

        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);

            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.StreamSource = new MemoryStream(ms.ToArray());
            bmpImg.EndInit();

            return bmpImg;
        }
    }
}
