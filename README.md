# WpfApplicationWithWcfService
A simple WPF application with WCF service to convert currency numbers to words on the server-side.


## Parts
* **WpfApplication** - WFP Client application, get input numeric currency and send it to the server with WCF and get currency words from the server
* **WCFService** - A host-independent WCF service to calculating currency words.
* **Host** - A console application to run & host WCF service.

## Run
To run the client application you need to a running host application or reconfigure WCF service and get data from service in the solution.