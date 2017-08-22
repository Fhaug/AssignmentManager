using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DataModel;

namespace DataSource
{
    public class FileDataSource
    {
        HttpClient ServiceClient;
        List<File> Files;

        public FileDataSource()
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri(@"http://localhost:36260/api/");
            ServiceClient.DefaultRequestHeaders.Clear();
            ServiceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Files = new List<File>();
        }

        public async Task<Boolean> AddFile(File file)
        {
            StringContent StringObject = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");
            HttpResponseMessage Response = await ServiceClient.PostAsync("Files", StringObject);
            //Response.EnsureSuccessStatusCode();
            if (Response.IsSuccessStatusCode)
            {
                Files.Add(file);
                return true;
            }
            return false;
        }
        public async Task<List<File>> GetFilesAsync()
        {
            String Response = await ServiceClient.GetStringAsync("Files");
            File[] ArrayObject = JsonConvert.DeserializeObject<File[]>(Response);
            Files = ArrayObject.ToList();
            return Files;
        }
    }
}
   


        

