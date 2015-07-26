using ICities;
using UnityEngine;
using System.Reflection;

namespace SmartCitySimulator
{

    public class ModInfo: IUserMod
    {

        public string Name 
        {
            get { return "Smart City Simulator"
                 //Print the version number in the description if in debug build
                #if DEBUG
                 + " v." + Assembly.GetExecutingAssembly().GetName().Version.ToString()
                #endif
                    ;
            }
        }

        public string Description 
        {
            get
            {
                return "Smart City Simulator for Microsoft Azure Internet of Things Services"

                              ;
            }
        }
    }

    // Inherit interfaces and implement your mod logic here
    // You can use as many files and subfolders as you wish to organise your code, as long
    // as it remains located under the Source folder.
}
