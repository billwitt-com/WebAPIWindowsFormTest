using System;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestRiverWatchApi.Helpers;
using System.Text;
using System.Collections.Generic;

namespace TestRiverWatchApi
{
    public class Program
    {
        private static HttpClient client = new HttpClient();

        static void Main()
        {
            bool testAuth = false;
            RunWebAPITestsAsync(testAuth).Wait();
            Console.WriteLine("Press any key to close the program.");
            Console.ReadLine();
        }

        private static async Task RunWebAPITestsAsync(bool testAuth)
        {
            try
            {
                Console.WriteLine("Run WebAPI Tests Async started");
                Console.WriteLine("Run Test Auth: {0}", testAuth);
                InitializeHTTPClient(testAuth);
                
                if (testAuth)
                {
                    //************************************************************** Check if BarCodeExists (GetICPBarCodeExists) ******************************************************
                    string barcode = "RW11-12531";
                    var urlParametersICPBarCode = new Dictionary<string, string>();
                    urlParametersICPBarCode.Add("Bcode", barcode);
                    String parametersICPBarCode = BuildURLParametersString(urlParametersICPBarCode);
                    string path_GetICPBarCodeExists = "InboundICPFinals/GetICPBarCodeExists" + parametersICPBarCode;

                    bool iCPBarCodeExists = await CheckExists(path_GetICPBarCodeExists, "GetICPBarCodeExists");
                    Console.WriteLine("iCPBarCodeExists {0}", iCPBarCodeExists);
                }
                else
                {
                    //************************************************************** Check if BarCodeExists (GetICPBarCodeExists) ******************************************************
                    string barcode = "RW11-12531";
                    var urlParametersICPBarCode = new Dictionary<string, string>();
                    urlParametersICPBarCode.Add("Bcode", barcode);
                    String parametersICPBarCode = BuildURLParametersString(urlParametersICPBarCode);
                    string path_GetICPBarCodeExists = "InboundICPFinals/GetICPBarCodeExists" + parametersICPBarCode;

                    bool iCPBarCodeExists = await CheckExists(path_GetICPBarCodeExists, "GetICPBarCodeExists");
                    Console.WriteLine($"iCPBarCodeExists {iCPBarCodeExists}");

                    //**************************************************************Create InboundICPFinal (PostInboundICPFinal) ********************************************************
                    if (!iCPBarCodeExists)
                    {
                        string type = "10";
                        int tableSampleID = 126;
                        string path_PostInboundICPFinal = "InboundICPFinals/PostInboundICPFinal/";
                        MakeICPInbound MI = new MakeICPInbound();
                        InboundICPFinal inboundICPFinal = MI.makeICP(barcode, type, tableSampleID);

                        var updatedInboundICPFinal = await PostInboundICPFinal(path_PostInboundICPFinal, inboundICPFinal);
                        Console.WriteLine($"UpdatedInboundICPFinal ID: {updatedInboundICPFinal.ID.ToString()}");
                    }

                    //************************************************************** Check if Bar Code exists in New Exp table (GetNewExpBarCodeExists) **********************************
                    var urlParametersNewExpBarCode = new Dictionary<string, string>();
                    urlParametersNewExpBarCode.Add("Bcode", barcode);
                    String parametersNewExpBarCode = BuildURLParametersString(urlParametersNewExpBarCode);
                    string path_GetNewExpBarCodeExists = "InboundICPFinals/GetNewExpBarCodeExists" + parametersNewExpBarCode;

                    bool newExpBarCodeExists = await CheckExists(path_GetNewExpBarCodeExists, "GetNewExpBarCodeExists"); 
                    Console.WriteLine($"newExpBarCodeExists {newExpBarCodeExists}");

                    //************************************************************** Check if Sample exists in Samples table (GetSampleExists) **********************************
                    var urlParametersSampleNumber = new Dictionary<string, string>();
                    urlParametersSampleNumber.Add("samplenumber", "9876201604041035");
                    String parametersSampleNumber = BuildURLParametersString(urlParametersSampleNumber);
                    string path_GetSampleExists = "InboundICPFinals/GetSampleExists" + parametersSampleNumber;

                    bool sampleExists = await CheckExists(path_GetSampleExists, "GetSampleExists");
                    Console.WriteLine($"sampleExists {sampleExists}");
                }                
            }
            catch (Exception ex)
            {
                HandleErrors(0, "", "RunWebAPITestsAsync", ex.StackTrace, "Main");
            }
        }

        private static void InitializeHTTPClient(bool testAuth)
        {
            string username = Properties.Settings.Default.WebAPI_UserName;
            if (testAuth) { username = "bad user name"; }
            string password = Properties.Settings.Default.WebAPI_Password;
            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);  // build http user name and pwd, this is convention, not our doing... 
                                                                                 // add user name and password, after encoding, to request header, to base64, which is convention, we have no choice (??)
            client.BaseAddress = new Uri(Properties.Settings.Default.WebAPI_URI_Localhost); // WebAPI_URI_Bill_Local
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private static async Task<Boolean> CheckExists(string path, string webApiAction)
        {
            HttpResponseMessage response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                var boolResult = await response.Content.ReadAsAsync<Boolean>();
                return boolResult;
            }
            else
            {
                HandleErrors((int)response.StatusCode, response.ReasonPhrase, webApiAction, "", "APICall");
                return false;
            }
        }

        static async Task<InboundICPFinal> PostInboundICPFinal(string path, InboundICPFinal inboundICPFinal)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(path, inboundICPFinal);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the updated InboundICPFinal from the response body.
                inboundICPFinal = await response.Content.ReadAsAsync<InboundICPFinal>();
                return inboundICPFinal;
            }
            else
            {
                HandleErrors((int)response.StatusCode, response.ReasonPhrase, "PostInboundICPFinal", "", "APICall");                
                return null;
            }            
        }

        private static String BuildURLParametersString(Dictionary<string, string> parameters)
        {
            UriBuilder uriBuilder = new UriBuilder();
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var urlParameter in parameters)
            {
                query[urlParameter.Key] = urlParameter.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.Query;
        }

        private static void HandleErrors(int responseStatusCode, string responseReasonPhrase, string fromPage, string stackTrace, string errorType = "")
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(errorType) && errorType.Equals("Main"))
            {
                msg = string.Format("Error in Main. StackTrace: {0}", stackTrace);
            }
            else if (!string.IsNullOrEmpty(errorType) && errorType.Equals("APICall"))
            {
                msg = string.Format("Error in {0}. response.StatusCode: {1} response.ReasonPhrase {2}",
                                                    errorType, responseStatusCode, responseReasonPhrase);
            }

            Console.WriteLine(msg);
            string nam = "Program";
            LogErrror LE = new LogErrror();
            LE.LogError(msg, fromPage, stackTrace, nam, "");
        }
    }
}
