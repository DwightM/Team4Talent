namespace Quartz.Management.Domain.Model
{
    public class JobIdentity
    {
        // Properties.
        public string Name
        {
            get;
            private set;
        }


        public string GroupName
        {
            get;
            private set;
        }



        // Methods.
        public JobIdentity(string _name, string _groupName)
        {
            Name = _name;
            GroupName = _groupName;
        }
    }
}