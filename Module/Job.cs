namespace Client_server_project.Module
{
    public class Job
    {

        public int JobID { get; set; }
        public string JobName { get; set; } = " ";
        public string JobType { get; set; } = " ";
        public double JobSalary { get; set; } 


        public Job(int jobID, string jobName, string jobType,double jobSalary) {

            JobID = jobID;
            JobName = jobName;
            JobType = jobType;
            JobSalary = jobSalary;
        }
        public override string ToString()
        {
            return $"JobID: {JobID}, Name: {JobName}, Type: {JobType}, Salary: {JobSalary}";
        }

    }
}
