using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFService;
using WCFService.Extensions;

namespace UnitTests
{
    [TestClass]
    public class CurrencyServiceTests
    {
        #region Var
        CurrencyService.INumberToCurrencyWordsService _wcfClient;
        #endregion
        #region Initializer
        [TestInitialize]
        public void Setup()
        {
            _wcfClient = new CurrencyService.NumberToCurrencyWordsServiceClient().ChannelFactory.CreateChannel();
        }
        #endregion

        #region Converter Tests
        [TestMethod]
        [DataRow("", "", DisplayName = "Empty string")]
        [DataRow("0", "zero dollars", DisplayName = "0")]
        [DataRow("1", "one dollar", DisplayName = "1")]
        [DataRow("12", "twelve dollars", DisplayName = "12")]
        [DataRow("123", "one hundred twenty-three dollars", DisplayName = "123")]
        [DataRow("123,2", "one hundred twenty-three dollars and twenty cents")]
        [DataRow("123,15", "one hundred twenty-three dollars and fiveteen cents")]
        [DataRow("1000", "one thousand dollars")]
        [DataRow("45 100", "forty-five thousand one hundred dollars")]
        [DataRow("12,01", "twelve dollars and one cent")]
        [DataRow("0,01", "zero dollars and one cent")]
        [DataRow("12,03", "twelve dollars and three cents")]
        [DataRow("999 999 999", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars")]
        public void GetWordsForCorrectNumberFromConverter(string numberString, string expectedWords)
        {
            Assert.AreEqual(expectedWords, numberString.ConvertStringNumberToWords());
        }
        [TestMethod]
        [DataRow("-1")]
        [DataRow("9 999 999 999.99")]
        [DataRow("1000000000")]
        public void CheckInvalidNumber_NotSupportExceptionDetection(string numberString)
        {
            Assert.ThrowsException<NotSupportedException>(() => numberString.ConvertStringNumberToWords());
        }

        [TestMethod]
        [DataRow("1d1")]
        [DataRow("1,991")]
        [DataRow("1,215")]
        [DataRow("1,2,15")]
        [DataRow("1,2.15")]
        public void CheckInvalidNumber_FormatExceptionDetection(string numberString)
        {
            Assert.ThrowsException<FormatException>(() => numberString.ConvertStringNumberToWords());
        }
        #endregion
        #region WCF Tests
        [TestMethod]
        [DataRow("", "", DisplayName = "Empty string")]
        [DataRow("0", "zero dollars", DisplayName = "0")]
        [DataRow("1", "one dollar", DisplayName = "1")]
        [DataRow("12", "twelve dollars", DisplayName = "12")]
        [DataRow("123", "one hundred twenty-three dollars", DisplayName = "123")]
        [DataRow("123,2", "one hundred twenty-three dollars and twenty cents")]
        [DataRow("123,15", "one hundred twenty-three dollars and fiveteen cents")]
        [DataRow("1000", "one thousand dollars")]
        [DataRow("45 100", "forty-five thousand one hundred dollars")]
        [DataRow("12,01", "twelve dollars and one cent")]
        [DataRow("0,01", "zero dollars and one cent")]
        [DataRow("12,03", "twelve dollars and three cents")]
        [DataRow("999 999 999", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars")]
        public void GetWordsForCorrectNumberFromWCF(string numberString, string expectedWords)
        {
            Assert.AreEqual(expectedWords, _wcfClient.GetCurrencyWords(numberString));
        }
        [TestMethod]
        [DataRow("-1")]
        [DataRow("1,215")]
        [DataRow("1d1")]
        [DataRow("1,991")]
        [DataRow("9 999 999 999.99")]
        [DataRow("1000000000")]
        public void CheckInvalidNumberFromWCF(string invalidNumberString)
        {
            Assert.ThrowsException<FaultException<ServiceFault>>(() => _wcfClient.GetCurrencyWords(invalidNumberString));
        }
        #endregion
    }
}
