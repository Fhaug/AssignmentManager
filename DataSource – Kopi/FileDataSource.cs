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
    class FileDataSource
    {
        HttpClient ServiceClient;
        List<Filee> Files;

        public FileDataSource()
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri(@"http://localhost:17193/api/");
            ServiceClient.DefaultRequestHeaders.Clear();
            ServiceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Files = new List<Filee>();
        }
        public async Task<Boolean> AddFile(Filee file){
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

        public async Task<List<Filee>> GetFilesAsync()
        {
            String Response = await ServiceClient.GetStringAsync("Files");
            Filee[] ArrayObject = JsonConvert.DeserializeObject<Filee[]>(Response);
            Files = ArrayObject.ToList();

            return Files;
        }
    }
}
