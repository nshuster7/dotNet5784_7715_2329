//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Dal;
internal static class Config
{
    static string s_data_config_xml = "data-config";
    // internal static int NextCourseId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextCourseId"); }
    // internal static int NextLinkId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextLinkId"); }
    public static int NextTaskId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "nextTaskId");

        set => XMLTools.SetNextId(s_data_config_xml, "nextTaskId", value);

    }

    public static int NextDependencyId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "nextDependencyId");

        set => XMLTools.SetNextId(s_data_config_xml, "nextDependencyId", value);

    }   
    }
