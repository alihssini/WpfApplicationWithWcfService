using System.ServiceModel;

namespace WCFService
{
    [ServiceContract]
    public interface INumberToCurrencyWordsService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceFault))]
        string GetCurrencyWords(string amount);
    }
}
