using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITD.ETC.VETC.Synchonization.Controller.ETC;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using Quartz;
using Quartz.Impl;
namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Job
{
    public class LaneImageETCtoMTC : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                NLogHelper.Info("Start Process Image from ETC->MTC");
                ImageDataUpload();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void ImageDataUpload()
        {
            try
            {
                ConfigModel config = MainProvider.GetInstance().ConfigInstance;
                if (config != null)
                {
                    var imageJob =
                        config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.ImageDataJobName);

                    if (imageJob != null)
                    {
                        ImageDataProcess imageProcess = new ImageDataProcess(imageJob.FullSourcePath,
                            imageJob.FullDesticationPath, config.ImageLocalFolderFormat, config.ImageFtpFolderFormat);
                        if (config.IsRunOnEtcServer)
                        {
                            imageProcess.ImageUploadProcess();
                        }
                        else
                        {
                            imageProcess.ImageDownloadProcess();
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
