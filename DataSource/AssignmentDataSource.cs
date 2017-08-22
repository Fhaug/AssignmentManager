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
    public class AssignmentDataSource
    {
        HttpClient ServiceClient;
        List<Assignment> Assignments;

        public AssignmentDataSource()
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri(@"http://localhost:36260/api/");
            ServiceClient.DefaultRequestHeaders.Clear();
            ServiceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Assignments = new List<Assignment>();
        }
        public async Task<Boolean> AddAssignment(Assignment assignment)
        {
            StringContent StringObject = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");
            HttpResponseMessage Response = await ServiceClient.PostAsync("Assignments", StringObject);
            //Response.EnsureSuccessStatusCode();
            if (Response.IsSuccessStatusCode)
            {
                Assignments.Add(assignment);
                return true;
            }
            return false;
        }
        public async Task<Boolean> RemoveAssignmentAsync(Assignment assignment)
        {
            HttpResponseMessage Response = await ServiceClient.DeleteAsync("Assignments/" + assignment.AssignmentId);
            //Response.EnsureSuccessStatusCode();
            if (Response.IsSuccessStatusCode)
            {
                Assignments.Remove(assignment);
                return true;
            }
            return false;
        }

        public async Task<List<Assignment>> GetAssignmentsAsync()
        {
            String Response = await ServiceClient.GetStringAsync("Assignments");
            Assignment[] ArrayObject = JsonConvert.DeserializeObject<Assignment[]>(Response);
            Assignments = ArrayObject.ToList();

            return Assignments;
        }

        /// <summary>
        /// Gets the assignment asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Assignment> GetAssignmentAsync(int id)
        {
            HttpResponseMessage Response = await ServiceClient.GetAsync("Assignments/" + id);
            if (Response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Assignment>(await Response.Content.ReadAsStringAsync());
            }
            return null;
        }



        public async Task<Boolean> UpdateAssignment(Assignment assignment)
        {
            StringContent StringObject = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await ServiceClient.PutAsync("Assignments/" + assignment.AssignmentId, StringObject);
            if (Response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }


    }
}
