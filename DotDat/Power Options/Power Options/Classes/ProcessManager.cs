using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Power_Options.Classes
{
    public class ProcessManager : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessManager()
        {
            Power.Schemes = new List<Scheme>();

            foreach (var plan in PowerSchemeHelper.GetAllPowerSchemas())
            {
                Power.Schemes.Add(Power.CreateScheme(plan.Value.FriendlyName,
                                                     plan.Value.SchemeGuid,
                                                     Tool.Tip(plan.Value.SchemeGuid)));
            }
        }

        /// <summary>
        /// Query all power schemes
        /// </summary>
        public static void CreatePowerPlansList()
        {
            
        }

        /// <summary>
        /// Returns process start information
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <param name="redirectStandardOutput"></param>
        /// <param name="useShellExecute"></param>
        /// <returns></returns>
        private static ProcessStartInfo StartInfo 
        (
            string filename, 
            string arguments, 
            bool redirectStandardOutput, 
            bool useShellExecute 
        )
        {
            return new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = redirectStandardOutput,
                    UseShellExecute = useShellExecute,
                    CreateNoWindow = true,
                    FileName = filename,
                    Arguments = arguments,
                }; 
        }

        /// <summary>
        /// Start a generic process
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        public void Start(string filename, string arguments)
        {
            using (var proc = Process.Start(StartInfo(filename, arguments, false, true)))
            {
                proc.WaitForExit();
                proc.Close();
            }
        }
        
        /// <summary>
        /// Returns active power scheme
        /// </summary>
        /// <returns></returns>
        public static string GetActiveScheme()
        {
            return PowerSchemeHelper.GetAllPowerSchemas().Where
                (scheme => scheme.Value.SchemeGuid == PowerSchemeHelper.GetPowerActiveScheme()).Select
                (scheme => scheme.Value.FriendlyName).FirstOrDefault();
        }

        /// <summary>
        /// Cleans up unused resources
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}

