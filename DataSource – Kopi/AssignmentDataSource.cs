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
    class AssignmentDataSource
    {
        HttpClient ServiceClient;
        List<Assignment> Assignments;

        public void DateDataSource()
        {
            ServiceClient = new HttpClient();
            ServiceClient.BaseAddress = new Uri(@"http://localhost:17193/api/");
            ServiceClient.DefaultRequestHeaders.Clear();
            ServiceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Assignments = new List<Assignment>();
        }
        public async Task<Boolean> AddAssignment(Assignment assignment){
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

        public async Task<List<Assignment>> GetAssignmentsAsync()
        {
            String Response = await ServiceClient.GetStringAsync("Assignments");
            Assignment[] ArrayObject = JsonConvert.DeserializeObject<Assignment[]>(Response);
            Assignments = ArrayObject.ToList();

            return Assignments;
        }

        public async Task<Boolean> AddAssignmentToDate(Date date, int assignmentId)
        {
            Assignment assignment = Assignments.Find(c => c.AssignmentId == assignmentId);
            assignment.Dates.Add(date);

            StringContent StringObject = new StringContent(JsonConvert.SerializeObject(assignment), Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await ServiceClient.PutAsync("Assignments/"+assignment.AssignmentId, StringObject);
            if(Response.IsSuccessStatusCode){
                return true;
            }
            return false;
        }
    }
}
