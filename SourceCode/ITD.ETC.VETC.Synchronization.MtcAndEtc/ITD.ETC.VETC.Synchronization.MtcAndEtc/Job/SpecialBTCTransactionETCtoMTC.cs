using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITD.ETC.VETC.Synchonization.Controller.ETC;
using ITD.ETC.VETC.Synchonization.Controller.MTCtoETC;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using Quartz;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class SpecialBTCTransactionETCtoMTC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process SpecialBTC Transaction from ETC->MTC");
                ProcessSpecialBTCTransaction();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void ProcessSpecialBTCTransaction()
        {
            try
            {
                string specialBTCTransactionTable = Resources.SpecialBTCTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var specialBTCJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.SpecialBTCTransactionDataJobName);

                    if (specialBTCJob != null)
                    {
                        BTCSpecialProcess oSpecialBTC = new BTCSpecialProcess(specialBTCTransactionTable, specialBTCJob.FullSourcePath,
                            specialBTCJob.FullDesticationPath);
                        if (config.IsRunOnEtcServer)
                        {
                            oSpecialBTC.ProcessBTCSpecialETC();
                        }
                        else
                        {
                            oSpecialBTC.ProcessBTCSpecialMTC();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public void ExecuteSpecialBTC()
        {
            ProcessSpecialBTCTransaction();
        }

        internal void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
