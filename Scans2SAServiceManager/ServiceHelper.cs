using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;

namespace Scans2SAServiceManager
{
    public class ServiceHelper
    {
        public static string ErrorMessage { get; private set; }

        private static void logError(Exception exc)
        {
            ErrorMessage = exc.Message;
            if (exc.InnerException != null)
            {
                ErrorMessage += " / " + exc.InnerException.Message;
            }
        }

        public static bool InstallService(string filePath, string serviceName)
        {
            if (ServiceHelper.ServiceExists(serviceName))
                return false;

            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { filePath });

                string mgmtObjPath = string.Format("Win32_Service.Name='{0}'", serviceName);

                using (ManagementObject service = new ManagementObject(new ManagementPath(mgmtObjPath)))
                {
                    object[] wmiParams = new object[11];
                    wmiParams[6] = "LocalSystem";
                    wmiParams[7] = "";
                    service.InvokeMethod("Change", wmiParams);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                logError(ex);
                return false;
            }

        }

        public static bool UninstallService(string serviceName)
        {
            if (!ServiceHelper.ServiceExists(serviceName))
                return false;

            try
            {
                StopService(serviceName);
                string mgmtObjPath = string.Format("Win32_Service.Name='{0}'", serviceName);

                using (ManagementObject service = new ManagementObject(new ManagementPath(mgmtObjPath)))
                {
                    service.InvokeMethod("delete",null);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                logError(ex);
                return false;
            }

        }

        public static bool StartService(string serviceName)
        {
            ServiceController sc = ServiceHelper.GetController(serviceName);
            if (sc == null)
                return false;
          
            if (sc.Status != ServiceControllerStatus.Running && !sc.Status.ToString().Contains("Pending"))
            {
                sc.Start();
                return true;
            }
            return false;
        }
        
        public static bool StopService(string serviceName)
        {
            ServiceController sc = ServiceHelper.GetController(serviceName);
            if (sc == null)
                return false;

            if (sc.Status != ServiceControllerStatus.Stopped && !sc.Status.ToString().Contains("Pending"))
            {
                sc.Stop();
           
                return true;
            }
            return false;
        }

        public static bool ServiceExists(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            var ourService = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return ourService != null;
        }

        public static ServiceController GetController(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            var ourService = services.FirstOrDefault(s => s.ServiceName == serviceName);
            if (ourService == null)
                return null;
            return new ServiceController(ourService.ServiceName);
        }
    }
}
