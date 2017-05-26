using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using Quartz;
using Quartz.Impl;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using ITD.ETC.VETC.Synchonization.Controller.MTCtoETC;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{

    public class VehiclePlateDataMTCtoETC : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process Vehicle from MTC -> ETC");
                ProcessVehicleData();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void ProcessVehicleData()
        {
            try
            {
                string vehicelPlateTransactionTable = Resources.VehicelPlateTransactionTableName;
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var vehicleplateJob =
                        config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.VehiclePlateDataJobName);

                    if (vehicleplateJob != null)
                    {
                        VehiclePlate vehicle = new VehiclePlate(vehicelPlateTransactionTable, vehicleplateJob.FullSourcePath,
                            vehicleplateJob.FullDesticationPath);
                        if (config.IsRunOnEtcServer)
                        {
                            vehicle.ProcessVehiclePlateETC();
                        }
                        else
                        {
                            vehicle.ProcessVehiclePlateMTC();
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
