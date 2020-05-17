using System;
using System.Globalization;
using System.ServiceModel;
using System.Windows.Data;
using System.Windows.Media;
using WpfApplication.CurrencyService;
using WpfApplication.Model;

namespace WpfApplication.Converter
{
    public class CurrencyNumberToWordsConverter : IValueConverter
    {
        #region Impl
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NumberToCurrencyWordsServiceClient client = new NumberToCurrencyWordsServiceClient();
            try
            {
                var words = client.GetCurrencyWords(value.ToString());
                return new WCFResponse { Words = words, Color = new SolidColorBrush(Colors.Black) };
            }
            catch (FaultException<ServiceFault> ex)
            {
                return new WCFResponse { Words = ex.Message, Color = new SolidColorBrush(Colors.Red) };
            }
            finally
            {
                client.Close();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        #endregion
    }
}
