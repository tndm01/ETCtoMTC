using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using System.Data;
using System.Data.SqlClient;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using System.IO;
using System.Reflection;
using ITD.ETC.VETC.Synchonization.Controller.MTCtoETC;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class TollTicketMTCtoETC:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process TollTicket from MTC -> ETC");
                ProcessTollTicketData();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        private void ProcessTollTicketData()
        {
            try
            {
                string tollTicketTable = Resources.TollTicketTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var etagJob =
                        config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.TollTicketDataJobName);

                    if (etagJob != null)
                    {
                        TollTicketProcess tollTickets = new TollTicketProcess(tollTicketTable, etagJob.FullSourcePath,
                            etagJob.FullDesticationPath);
                       
                        if (config.IsRunOnEtcServer)
                        {
                            tollTickets.ProcessTollTicket();
                        }
                        else
                        {
                            //Run cais moi tao
                            tollTickets.ProcessTollTicketMTC();
                        }
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
