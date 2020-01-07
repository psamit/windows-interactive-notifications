﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace InteractiveManagedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            var appId = "Felix.Lol.Test";
            var toastTemplate = @"
            <toast launch=""developer-defined-string"">
                <visual>
                <binding template=""ToastGeneric"">
                    <text>Andrew B.</text>
                    <text>Shall we meet up at 8?</text>
                    <image placement=""appLogoOverride"" src=""https://unsplash.it/64?image=883"" hint-crop=""circle"" />
                </binding>
                </visual>
                <actions>
                <input id=""message"" type=""text"" placeHolderContent=""Type a reply"" />
                <action activationType=""background"" content=""Reply"" arguments=""reply"" />
                <action activationType=""foreground"" content=""Video call"" arguments=""video"" />
                </actions>
            </toast>";

            CRegisterForNotificationSupport();
            CRegisterActivator();

            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(toastTemplate);

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier(appId).Show(toast);


            Console.WriteLine("Waiting...");
            var name = Console.ReadLine();
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                CUnregisterActivator();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        [DllImport(@"C:\Users\felixr\Code\InteractiveNotifications\Debug\InteractiveNotifications.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void CRegisterForNotificationSupport();

        [DllImport(@"C:\Users\felixr\Code\InteractiveNotifications\Debug\InteractiveNotifications.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void CRegisterActivator();

        [DllImport(@"C:\Users\felixr\Code\InteractiveNotifications\Debug\InteractiveNotifications.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void CUnregisterActivator();
    }
}
