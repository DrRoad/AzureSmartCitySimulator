using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

using ColossalFramework.Plugins;

namespace SmartCitySimulator
{
    public static class Debug
    {
        public static void Out(PluginManager.MessageType messageType, bool useComma, params System.Object[] o)
        {
            string s = "";
            for (int i = 0; i < o.Length; i++)
            {
                s += o[i].ToString();
                if (i < o.Length - 1 && useComma)
                    s += "  ,  ";
            }
            switch (messageType)
            {
                case PluginManager.MessageType.Message:
                    UnityEngine.Debug.Log(s);
                    break;
                case PluginManager.MessageType.Warning:
                    UnityEngine.Debug.LogWarning(s);
                    break;
                case PluginManager.MessageType.Error:
                    UnityEngine.Debug.LogError(s);
                    break;
            }

            //DebugOutputPanel.AddMessage(messageType, s);
        }

        public static void Log(params System.Object[] o)
        {
            Message(o);
        }

        public static void Message(params System.Object[] o)
        {
            Message(true, o);
        }

        public static void Message(bool useComma, params System.Object[] o)
        {
            Out(PluginManager.MessageType.Message, useComma, o);
        }

        public static void Warning(params System.Object[] o)
        {
            Warning(true, o);
        }

        public static void Warning(bool useComma, params System.Object[] o)
        {
            Out(PluginManager.MessageType.Warning, useComma, o);
        }

        public static void Error(params System.Object[] o)
        {
            Error(true, o);
        }

        public static void Error(bool useComma, params System.Object[] o)
        {
            Out(PluginManager.MessageType.Error, useComma, o);
        }

        public static void dumpObject(object myObject)
        {
            string myObjectDetails = "";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myObject))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(myObject);
                myObjectDetails += name + ": " + value + "\n";
            }

            Message(true, myObjectDetails);
        }
    }
}
