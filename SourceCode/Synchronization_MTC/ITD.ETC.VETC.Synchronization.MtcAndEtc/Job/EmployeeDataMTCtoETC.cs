using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.MTCtoETC;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class EmployeeDataMTCtoETC : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process Employee from MTC -> ETC");
                ProcessEmployeeData();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
    
        private void ProcessEmployeeData()
        {
            try
            {
                //EtagTransactionProcess oETagTransaction = new EtagTransactionProcess();
               // string etagTransactionTable = Resources.EtagTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var employeeJob =
                        config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.EmployeeDataJobName);

                    if (employeeJob != null)
                    {
                        EmployeeProcess employees = new EmployeeProcess( employeeJob.FullSourcePath,
                            employeeJob.FullDesticationPath);
                        employees.ProcessEmployee();
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
    }
}
