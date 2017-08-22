using DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
  public class DateDataSource
    {
        HttpClient ServiceClient;
        List<Date> Dates;

        public DateDataSource()
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri(@"http://localhost:36260/api/");
            ServiceClient.DefaultRequestHeaders.Clear();
            ServiceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Dates = new List<Date>();
        }

        public async Task<Boolean> AddDate(Date date){
            StringContent StringObject = new StringContent(JsonConvert.SerializeObject(date), Encoding.UTF8, "application/json");
            HttpResponseMessage Response = await ServiceClient.PostAsync("Dates", StringObject);
            //Response.EnsureSuccessStatusCode();
            if (Response.IsSuccessStatusCode)
            {
                Dates.Add(date);
                return true;
            }
            return false;
        }
        public async Task<Boolean> RemoveDateAsync(Date date)
        {
            HttpResponseMessage Response = await ServiceClient.DeleteAsync("Dates/" + date.DateId);
            //Response.EnsureSuccessStatusCode();
            if (Response.IsSuccessStatusCode)
            {
                Dates.Remove(date);
                return true;
            }
            return false;
        }


        public async Task<List<Date>> GetDatesAsync()
        {
            String Response = await ServiceClient.GetStringAsync("Dates");
            Date[] ArrayObject = JsonConvert.DeserializeObject<Date[]>(Response);
            Dates = ArrayObject.ToList();
            
            return Dates;
        }
        public async Task<Date> GetDateAsync(int id)
        {
            HttpResponseMessage Response = await ServiceClient.GetAsync("Dates/" + id);
            if (Response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Date>(await Response.Content.ReadAsStringAsync());
            }
            return null;
        }
        public async Task<Boolean> UpdateDate(Assignment assignment, int dateId)
        {
            Date date = Dates.Find(c => c.DateId == dateId);
            date.Assignments.Add(assignment);

            StringContent StringObject = new StringContent(JsonConvert.SerializeObject(date), Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await ServiceClient.PutAsync("Dates/"+date.DateId, StringObject);
            if(Response.IsSuccessStatusCode){
                return true;
            }
            return false;
        }
    }
}
