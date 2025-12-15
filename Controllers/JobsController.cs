using Client_server_project.Module;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

// להגדיר את הרישיון פעם אחת, בדרך כלל בתחילת Controller או Program.cs


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Client_server_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class JobsController : ControllerBase
    {
        public static List<Job> listJobs = new List<Job>
        {
            new Job(1,"matrix","program",1000),
            new Job(2,"maccabi","secretariat",2000),
            new Job(3,"shefer","Kindergarten teacher",3000),
            new Job(4,"porush","teaching",4000),
            new Job(5,"mishcnot dat","teaching",6000),
            new Job(6,"perach","Kindergarten teacher",8000),
            new Job(7,"snif","teaching",80000),
            new Job(8,"muchedet","secretariat",9000),
            new Job(9,"aws","secretariat",900000),

        };



        // GET: api/<JobsController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(listJobs);
        }

        //1

        // GET api/<JobsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Job current = listJobs.First(x => x.JobID == id);
                return Ok(current);
            }
            catch
            {
                return NotFound("the id is not exists");

            }

        }


        //2

        // POST api/<JobsController>
        [HttpPost]
        public IActionResult Post([FromBody] Job newJob)
        {
            // יצירת JobID אוטומטי
            newJob.JobID = listJobs.Count > 0 ? listJobs.Max(j => j.JobID) + 1 : 1;

            listJobs.Add(newJob);
            return Ok(newJob);
        }

        //3

        // PUT api/<JobsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Job newJob)
        {
            try
            {
                int index = listJobs.FindIndex(x => x.JobID == id);
                listJobs[index] = newJob;
                return Ok(listJobs[index]);

            }
            catch
            {
                return NotFound("the id is not exists");
            }



        }

        //4

        // DELETE api/<JobsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                int index = listJobs.FindIndex(x => x.JobID == id);
                Job newJob = listJobs[index];
                listJobs.RemoveAt(index);
                return Ok(newJob);

            }
            catch
            {
                return NotFound("the id is not exists");
            }

        }


        //5

        // Post api/<JobsController>/createDataSave/path
        [HttpGet("statistics")]
        public IActionResult GetSalaryStats([FromQuery] string field)
        {
            if (listJobs.Count == 0)
                return Ok("No jobs available");

            if (string.IsNullOrWhiteSpace(field))
                return BadRequest("Query parameter 'field' is required.");

            if (field.Equals("JobName", StringComparison.OrdinalIgnoreCase))
            {
                var stats = listJobs
                    .GroupBy(job => job.JobName)
                    .ToDictionary(
                        g => g.Key ?? "null",
                        g => new
                        {
                            MinSalary = g.Min(job => job.JobSalary),
                            MaxSalary = g.Max(job => job.JobSalary)
                        });
                return Ok(stats);
            }
            else if (field.Equals("JobType", StringComparison.OrdinalIgnoreCase))
            {
                var stats = listJobs
                    .GroupBy(job => job.JobType)
                    .ToDictionary(
                        g => g.Key ?? "null",
                        g => new
                        {
                            MinSalary = g.Min(job => job.JobSalary),
                            MaxSalary = g.Max(job => job.JobSalary),
                            AverageSalary = g.Average(job => job.JobSalary)
                        });
                return Ok(stats);
            }
            else
            {
                return BadRequest("Invalid field. Must be 'JobName' or 'JobType'.");
            }
        }


        

[HttpGet("export-to-excel")]
    public IActionResult ExportToExcel()
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Jobs");
            worksheet.Cell(1, 1).Value = "JobName";
            worksheet.Cell(1, 2).Value = "JobSalary";
            worksheet.Cell(1, 3).Value = "JobType";

            int row = 2;
            foreach (var job in listJobs)
            {
                worksheet.Cell(row, 1).Value = job.JobName;
                worksheet.Cell(row, 2).Value = job.JobSalary;
                worksheet.Cell(row, 3).Value = job.JobType;
                row++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Jobs.xlsx");
            }
        }
    }






}



    }




      