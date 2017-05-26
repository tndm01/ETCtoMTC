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
    public class TollTicketTransactionETCtoMTC : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process TollTicket Transaction from ETC->MTC");
                ProcessTollTicketTransaction();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        /// <summary>
        /// Hàm xử lý luồng TollTicketTransactionETCtoMTC, gọi đến ETC ở controller
        /// </summary>
        private void ProcessTollTicketTransaction()
        {
            try
            {
                string tollTicketTransactionTable = Resources.TollTicketTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var tollTicketJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.TollTicketTransactionDataJobName);

                    if (tollTicketJob != null)
                    {
                        TollTicketTransactionProcess oTollTicketTransaction = new TollTicketTransactionProcess(tollTicketTransactionTable, tollTicketJob.FullSourcePath,
                            tollTicketJob.FullDesticationPath);
                        oTollTicketTransaction.TollTicketProcessData();
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
            ProcessTollTicketTransaction();
        }
        internal void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
