using System;
using System.Linq;
using ITD.ETC.VETC.Synchonization.Controller.ETC;
using ITD.ETC.VETC.Synchonization.Controller.MTCtoETC;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using Quartz;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class SpecicalTransactionETCtoMTC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process Special Transaction from ETC->MTC");
                ProcessSpecialTransaction();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void ProcessSpecialTransaction()
        {
            try
            {
                string specialTransactionTable = Resources.SpecialTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var specialJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.SpecialTransactionDataJobName);

                    if (specialJob != null)
                    {
                        SpecialProcess oSpecialTransaction = new SpecialProcess(specialTransactionTable, specialJob.FullSourcePath,
                            specialJob.FullDesticationPath);
                        if (config.IsRunOnEtcServer)
                        {
                            oSpecialTransaction.ProcessSpecialETC();
                        }
                        else
                        {
                            oSpecialTransaction.ProcessSpecialMTC();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void ExecuteSpecial()
        {
            ProcessSpecialTransaction();
        }

        internal void Execute()
        {
            throw new NotImplementedException();
        }
    }
}