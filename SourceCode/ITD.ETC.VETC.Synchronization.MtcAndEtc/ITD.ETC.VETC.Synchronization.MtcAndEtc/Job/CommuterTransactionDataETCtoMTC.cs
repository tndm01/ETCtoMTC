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
using ITD.ETC.VETC.Synchonization.Controller.ETC;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class CommuterTransactionDataETCtoMTC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process CommuterTicket Transaction from ETC->MTC");
                ProcessCommuterTicketTransaction();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        /// <summary>
        /// Hàm xử lý luồng CommuterTransactionDataETCtoMTC, gọi đến ETC ở controller
        /// </summary>
        private void ProcessCommuterTicketTransaction()
        {
            try
            {
                string commuterTicketTransactionTable = Resources.CommuterTicketTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var commuterTicketJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.CommuterTicketTransactionDataJobName);

                    if (commuterTicketJob != null)
                    {
                        CommuterTicketTransactionProcess oCommuterTicketTransaction = new CommuterTicketTransactionProcess(commuterTicketTransactionTable, commuterTicketJob.FullSourcePath,
                            commuterTicketJob.FullDesticationPath);
                         if (config.IsRunOnEtcServer)
                        {
                            oCommuterTicketTransaction.CommuterTicketProcessData();
                        }
                        else
                        {
                            oCommuterTicketTransaction.ProcessCommuterTicketTransactionMTC();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void Execute2()
        {
            ProcessCommuterTicketTransaction();
        }
        internal void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
