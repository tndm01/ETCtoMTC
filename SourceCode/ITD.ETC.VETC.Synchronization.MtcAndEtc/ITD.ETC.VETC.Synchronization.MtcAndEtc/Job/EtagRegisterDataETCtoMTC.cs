using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using ITD.ETC.VETC.Synchonization.Controller.ETC;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class EtagRegisterDataETCtoMTC : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process Etag Register from ETC->MTC");
                ProcessEtagRegister();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        /// <summary>
        /// Hàm xử lý luồng ETAGRegisterDataETCtoMTC, gọi đến ETC ở controller
        /// </summary>
        private void ProcessEtagRegister()
        {
            try
            {
                
                string etagRegisterTable = Resources.EtagRegisterTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var etagJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.EtagRegisterDataJobName);

                    if (etagJob != null)
                    {
                        EtagRegisterProcess oETagRegister = new EtagRegisterProcess(etagRegisterTable, etagJob.FullSourcePath,
                            etagJob.FullDesticationPath);
                        
                        if (config.IsRunOnEtcServer)
                        {
                            oETagRegister.EtagProcessData();
                        }
                        else
                        {
                            oETagRegister.ProcessEtagRegisterMTC();
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

