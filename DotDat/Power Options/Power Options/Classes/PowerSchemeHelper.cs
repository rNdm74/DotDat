﻿using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;

namespace Power_Options.Classes
{
    public class PowerPlanInfo
    {
        public string FriendlyName;
        public Guid SchemeGuid;
        public object Tag;

        public bool Set()
        {
            return PowerSchemeHelper.SetPowerScheme(SchemeGuid);
        }
    }

    public class PowerSchemeHelper
    {
        public static bool SetPowerScheme(Guid schemeGuid)
        {
            Guid currentSchemeGuid = PowrProfHelper.GetPowerActiveScheme();
            if (currentSchemeGuid == schemeGuid)
                return true;
            PowrProfHelper.SetPowerScheme(schemeGuid);
            currentSchemeGuid = PowrProfHelper.GetPowerActiveScheme();
            return currentSchemeGuid == schemeGuid;
        }

        public static Dictionary<Guid, PowerPlanInfo> GetAllPowerSchemas()
        {
            Guid schemaGuid;

            uint i = 0;

            var ret = new Dictionary<Guid, PowerPlanInfo>(3);

            do
            {
                schemaGuid = PowrProfHelper.GetPowerSchemeGuid(i);

                if (schemaGuid != Guid.Empty)
                {
                    ret.Add(schemaGuid,new PowerPlanInfo { SchemeGuid = schemaGuid, FriendlyName = PowrProfHelper.GetPowerSchemeFriendlyName(schemaGuid) });
                }
                i++;
            } while (schemaGuid != Guid.Empty);

            return ret;
        }

        public static Guid GetPowerActiveScheme()
        {
            return PowrProfHelper.GetPowerActiveScheme();
        }

        private class PowrProfHelper
        {
            #region pinvoke
            private const UInt32 ERROR_SUCCESS = 0;
            private const UInt32 ERROR_MORE_DATA = 234;
            private const UInt32 ERROR_NO_MORE_ITEMS = 259;
            private const UInt32 ERROR_FILE_NOT_FOUND = 2;

            [Flags]
            private enum POWER_DATA_ACCESSOR
            {
                /// <summary>
                /// Check for overrides on AC power settings.
                /// </summary>
                ACCESS_AC_POWER_SETTING_INDEX = 0x0,
                /// <summary>
                /// Check for overrides on DC power settings.
                /// </summary>
                ACCESS_DC_POWER_SETTING_INDEX = 0x1,
                /// <summary>
                /// Check for restrictions on specific power schemes.
                /// </summary>
                ACCESS_SCHEME = 0x10,
                /// <summary>
                /// Check for restrictions on active power schemes.
                /// </summary>
                ACCESS_ACTIVE_SCHEME = 0x13,
                /// <summary>
                /// Check for restrictions on creating or restoring power schemes.
                /// </summary>
                ACCESS_CREATE_SCHEME = 0x14
            };

            [DllImport("PowrProf.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.U4)]
            private static extern UInt32 PowerEnumerate(
                IntPtr RootPowerKey,
                IntPtr SchemeGuid,
                IntPtr SubGroupOfPowerSettingsGuid,
                /*
                [MarshalAs(UnmanagedType.LPStruct)] Guid SchemeGuid,
                [MarshalAs(UnmanagedType.LPStruct)] Guid SubGroupOfPowerSettingsGuid,
                 * */
                POWER_DATA_ACCESSOR AccessFlags,
                UInt32 Index,
                IntPtr Buffer,
                ref UInt32 BufferSize
             );


            [DllImport("PowrProf.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.U4)]
            private static extern UInt32 PowerReadFriendlyName(
                IntPtr RootPowerKey,
                ref Guid SchemeGuid,
                IntPtr SubGroupOfPowerSettingsGuid,
                /*[MarshalAs(UnmanagedType.LPStruct)] Guid SchemeGuid,
                [MarshalAs(UnmanagedType.LPStruct)] Guid SubGroupOfPowerSettingsGuid,*/
                IntPtr PowerSettingGuid,
                IntPtr Buffer,
                ref UInt32 BufferSize
            );

            [DllImport("PowrProf.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.U4)]
            private static extern UInt32 PowerSetActiveScheme(
                IntPtr UserRootPowerKey,
                ref Guid SchemeGuid
            );

            [DllImport("PowrProf.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.U4)]
            private static extern UInt32 PowerGetActiveScheme(
                IntPtr UserRootPowerKey,
                ref IntPtr ActivePolicyGuid
            );
            #endregion

            public static Guid GetPowerSchemeGuid(UInt32 index)
            {
                uint buffSize = 16;//it should be ok
                uint res = 0;
                Guid ret = Guid.Empty;
                IntPtr buffer = buffer = Marshal.AllocHGlobal((int)buffSize);
                res = PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, POWER_DATA_ACCESSOR.ACCESS_SCHEME, index, buffer, ref buffSize);
                if (res == ERROR_MORE_DATA)
                {
                    Marshal.FreeHGlobal(buffer);
                    buffer = Marshal.AllocHGlobal((int)buffSize);
                    res = PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, POWER_DATA_ACCESSOR.ACCESS_SCHEME, index, buffer, ref buffSize);
                }
                if (res == ERROR_SUCCESS)
                    ret = (Guid)Marshal.PtrToStructure(buffer, typeof(Guid));
                else if (res != ERROR_NO_MORE_ITEMS)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                Marshal.FreeHGlobal(buffer);
                return ret;
            }

            public static string GetPowerSchemeFriendlyName(Guid schemeGuid)
            {
                uint buffSize = 255;//it should be ok
                uint res = 0;
                string ret = null;
                IntPtr buffer = buffer = Marshal.AllocHGlobal((int)buffSize);

                res = PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, buffer, ref buffSize);
                if (res == ERROR_MORE_DATA)
                {
                    Marshal.FreeHGlobal(buffer);
                    buffer = Marshal.AllocHGlobal((int)buffSize);
                    res = PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, buffer, ref buffSize);
                }
                if (res == ERROR_SUCCESS)
                    ret = Marshal.PtrToStringUni(buffer);
                else if (res == ERROR_FILE_NOT_FOUND)
                    ret = null;
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                Marshal.FreeHGlobal(buffer);
                return ret;
            }

            public static bool SetPowerScheme(Guid schemeGuid)
            {
                UInt32 res = PowerSetActiveScheme(IntPtr.Zero, ref schemeGuid);
                if (res == ERROR_SUCCESS)
                    return true;
                return false;
                //throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            public static Guid GetPowerActiveScheme()
            {
                IntPtr buffer = Marshal.AllocHGlobal(16);
                Guid ret = Guid.Empty;
                UInt32 res = PowerGetActiveScheme(IntPtr.Zero, ref buffer);
                if (res == ERROR_SUCCESS)
                    ret = (Guid)Marshal.PtrToStructure(buffer, typeof(Guid));
                Marshal.FreeHGlobal(buffer);
                return ret;
            }
        }
    }
}
