using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Syncbak.SyncboxSetup.Models;
using Syncbak.SyncboxSetup.Data;
using System.Data.SqlClient;
using Syncbak.SyncboxSetup.Data.Extensions;

namespace Syncbak.SyncboxSetup.Repositories
{
    public class StepsRepository : BaseDataMapper
    {
        public StepsRepository() : base("Syncbak") { }

        public IEnumerable<Category> GetCategories()
        {
            var result = new List<Category>();
            var parameters = new List<SqlParameter>();

            ExecuteDataReader("syncbak.CategoryGet", parameters, reader =>
            {
                while (reader.Read())
                {
                    result.Add(new Category
                    {
                        CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        CategoryName = reader.GetString(reader.GetOrdinal("Name")),
                        SortOrder = reader.GetInt16(reader.GetOrdinal("SortOrder"))
                    });
                }
            });
            return result;
        }

        public IEnumerable<Step> GetSteps(int? categoryId)
        {
            var result = new List<Step>();
            var parameters = new List<SqlParameter>();

            if (categoryId != null && categoryId > 0)
            {
                parameters.Add(new SqlParameter("@CategoryId", categoryId));
            }

            ExecuteDataReader("syncbak.StepGet", parameters, reader =>
            {
                while (reader.Read())
                {
                    result.Add(new Step
                    {
                        StepId = reader.ReadInt("StepId"),
                        CategoryId = reader.ReadInt("CategoryId"),
                        CategoryName = reader.ReadString("CategoryName"),
                        StepName = reader.ReadString("StepName")
                    });
                }
            });
            return result;
        }
        
        public IEnumerable<InstallStep> GetInstallSteps(int stationId)
        {
            var result = new List<InstallStep>();
            var parameters = new List<SqlParameter>();

            if (stationId > 0)
            {
                parameters.Add(new SqlParameter("@StationId", stationId));
            }
                        
            ExecuteDataReader("syncbak.InstallStepGet", parameters, reader =>
            {
                while (reader.Read())
                {
                    var step = new InstallStep();
                    step.InstallStepId = reader.ReadInt("InstallStepId");
                    step.StepId = reader.ReadInt("StepId");
                    step.AccountOwner = reader.ReadString("AccountOwner");
                    step.StepName = reader.ReadString("StepName");
                    step.StartDate = reader.ReadDateTime("StartDate");
                    step.EndDate = reader.ReadDateTime("EndDate");
                    step.Status = reader.ReadString("Status");
                    step.Owner = reader.ReadString("Owner");
                    step.CategoryId = reader.ReadInt("CategoryId");
                    step.CategoryName = reader.ReadString("CategoryName");
                    step.StationId = reader.ReadInt("StationId");
                    step.CallSign = reader.ReadString("CallSign");
                    step.CurrentStep = reader.ReadBool("CurrentStep");
                    result.Add(step);
                }
            });
            return result;
        }

        public InstallStep GetStepInfo(int stationId)
        {
            var result = new InstallStep();
            var parameters = new List<SqlParameter>();

            if (stationId > 0)
            {
                parameters.Add(new SqlParameter("@StationId", stationId));
            }

            ExecuteDataReader("syncbak.InstallStepInfoGet", parameters, reader =>
            {
                if (reader.Read())
                {
                    result.InstallStepId = reader.ReadInt("InstallStepId");
                    result.AccountOwner = reader.ReadString("AccountOwner");
                    result.StationId = reader.ReadInt("StationId");
                    result.IsComplete = reader.ReadBool("IsComplete");
                }
            });
            return result;
        }

        public string SaveInstallStep(int installStepId, string accountOwner, int stepId, DateTimeOffset? startDate, DateTimeOffset? endDate, string status, string owner, int stationId, bool currentStep)
        {
            var result = new List<InstallStep>();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@InstallStepId", installStepId));
            parameters.Add(new SqlParameter("@AccountOwner", accountOwner));
            parameters.Add(new SqlParameter("@StepId", stepId));
            parameters.Add(new SqlParameter("@StartDate", startDate));
            parameters.Add(new SqlParameter("@EndDate", endDate));
            parameters.Add(new SqlParameter("@Status", status));
            parameters.Add(new SqlParameter("@Owner", owner));
            parameters.Add(new SqlParameter("@StationId", stationId));
            parameters.Add(new SqlParameter("@CurrentStep", currentStep));

            try
            {
                ExecuteNonQuery("syncbak.InstallStepSet", parameters);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SaveInstallStepInfo(int installStepId, int stationId, string accountOwner, bool isComplete)
        {
            var result = new List<InstallStep>();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@InstallStepId", installStepId));
            parameters.Add(new SqlParameter("@AccountOwner", accountOwner));
            parameters.Add(new SqlParameter("@StationId", stationId));
            parameters.Add(new SqlParameter("@IsComplete", isComplete));

            try
            {
                ExecuteNonQuery("syncbak.InstallStepInfoSet", parameters);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public string AddInitialSteps(int stationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@StationId", stationId));

            try
            {
                ExecuteNonQuery("syncbak.InstallStepsAdd", parameters);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}