using System.Runtime.Serialization;

namespace WCFService
{
    [DataContract]
    public class ServiceFault
    {
        #region Vars
        private string _message;
        #endregion
        #region Ctors
        public ServiceFault(string message) => _message = message;
        #endregion
        #region Props
        [DataMember]
        public string Message { get { return _message; } set { _message = value; } } 
        #endregion
    }
}
