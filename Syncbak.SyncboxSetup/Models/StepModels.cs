using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Syncbak.SyncboxSetup.Models
{
    public class InstallStep : InstallStepDetail
    {
        public int InstallStepId { get; set; }
        public int StationId { get; set; }
        public string CallSign { get; set; }
        public string AccountOwner { get; set; }
        public bool IsComplete { get; set; }
    }

    public class InstallStepDetail : Step
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public bool CurrentStep { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public short SortOrder { get; set; }
    }

    public class Step : Category
    {
        public int StepId { get; set; }
        public string StepName { get; set; }
        public short SortOrder { get; set; }
    }

    public enum Owner
    {
        Reps,
        IT,
        MAC,
        QA
    }
}