using System;
using System.ServiceModel;
using WCFService.Extensions;

namespace WCFService
{
    public class NumberToCurrencyWordsService : INumberToCurrencyWordsService
    {
        public string GetCurrencyWords(string amount)
        {
            try
            {
                //if you run the Host console application, You can read this logs on server side
                Console.WriteLine($"Received amount: {amount}");

                return amount.ConvertStringNumberToWords();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new FaultException<ServiceFault>(new ServiceFault(ex.Message), new FaultReason(ex.Message),new FaultCode("Sender"));
            }

        }
    }
}
