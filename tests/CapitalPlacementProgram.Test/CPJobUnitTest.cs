using CapitalPlacementProgram.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CapitalPlacementProgram.Test
{
    public class CPJobUnitTest: WebApplicationFactory<Program>
    {
        [Fact]
        public async void CreateJob()
        {
            // Arrange
            using var client = this.CreateClient();
            var jobDetails = new JobDetails
            {
                Title = "Test",
                Summary = "Test",
                Description = "Test",
                KeySkills = new List<string> { "a","b","c" },
                Benefits = "Test",
                Criteria = "Test",
                Type = "Test",
                DateStart = "Test",
                DateCloseApplication = "Test",
                DateOpenApplication = "Test",
                Duration = 0,
                Location = "Test",
                MinimalQualifications = "Test",
                MaxApplicants = 1
            };

            var jobString = JsonConvert.SerializeObject(jobDetails);
            var content = new StringContent(jobString);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            // Act
            var response = await client.PostAsync("/api/jobs/create", content);
            var data = await response.Content.ReadAsStringAsync();
            var dataJson = JsonConvert.DeserializeObject<JobItem>(data);

            // Delete test item
            await client.DeleteAsync("/api/jobs/" + dataJson.Id);

            // Assert
            Assert.Equal(jobString, JsonConvert.SerializeObject(dataJson.Details));
        }
    }
}