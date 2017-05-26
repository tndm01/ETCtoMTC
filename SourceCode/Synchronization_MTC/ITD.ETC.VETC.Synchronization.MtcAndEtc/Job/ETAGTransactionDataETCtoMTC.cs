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
    public class ETAGTransactionDataETCtoMTC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process Etag Transaction from ETC->MTC");
                ProcessEtagTransaction();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        /// <summary>
        /// Hàm xử lý luồng ETAGTransactionDataETCtoMTC, gọi đến ETC ở controller
        /// </summary>
        private void ProcessEtagTransaction()
        {
            try
            {
                //EtagTransactionProcess oETagTransaction = new EtagTransactionProcess();
                string etagTransactionTable = Resources.EtagTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var etagJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.EtagTransactionDataJobName);

                    if (etagJob != null)
                    {
                        EtagTransactionProcess oETagTransaction = new EtagTransactionProcess(etagTransactionTable, etagJob.FullSourcePath,
                            etagJob.FullDesticationPath);
                        oETagTransaction.EtagProcessData();
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
            ProcessEtagTransaction();
        }

        internal void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
