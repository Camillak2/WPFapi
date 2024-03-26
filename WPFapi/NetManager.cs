using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace WPFapi
{
    public static class NetManager
    {
        public static string URL = "https://localhost:44341/";
        public static HttpClient httpClient = new HttpClient();   
        
        
        
        public static async Task<T> Get<T>(string controller)
        {
            var response = await httpClient.GetAsync(URL + controller);
            var data = 
        }



    }

 
}
