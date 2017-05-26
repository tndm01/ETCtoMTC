﻿using System;
using System.Linq;
using ITD.ETC.VETC.Synchonization.Controller.MTCtoETC;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using Quartz;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class CommuterTicketMTCtoETC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process CommuterTicket from MTC -> ETC");
                ProcessCommuterTicketData();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void ProcessCommuterTicketData()
        {
            try
            {
                string commuterTicketTable = Resources.CommuterTicketTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var commuterTicketJob =
                        config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.ComuterTicketDataJobName);

                    if (commuterTicketJob != null)
                    {
                        CommuterTicketProcess commuterTickets = new CommuterTicketProcess(commuterTicketTable,commuterTicketJob.FullSourcePath,
                            commuterTicketJob.FullDesticationPath);
                        if (config.IsRunOnEtcServer)
                        {
                            commuterTickets.ProcessCommuterTicket();
                        }else
                        {
                            //Run cais moi tao
                            commuterTickets.ProcessCommuterTicketMTC();
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