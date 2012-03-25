using System;

namespace Quartz.Management.Domain.Model
{
    public class TriggerGroup
    {
        public TriggerGroup(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }
}