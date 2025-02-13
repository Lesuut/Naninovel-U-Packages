using System.Text.RegularExpressions;
using UnityEngine;

namespace Naninovel.U.Reception
{
    /// <summary>
    /// Contains configuration data for the Reception systems.
    /// </summary>
    [EditInProjectSettings]
    public class ReceptionConfiguration : Configuration
    {
        public const string DefaultPathPrefix = "Reception";

        public bool[] Pairs;
    }
}