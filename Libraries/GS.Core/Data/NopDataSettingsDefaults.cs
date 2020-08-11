
namespace GS.Core.Data
{
    /// <summary>
    /// Represents default values related to data settings
    /// </summary>
    public static partial class GSDataSettingsDefaults
    {
        /// <summary>
        /// Gets a path to the file that was used in old nopCommerce versions to contain data settings
        /// </summary>
        public static string ObsoleteFilePath => "~/App_Data/Settings.txt";

        /// <summary>
        /// Gets a path to the file that contains data settings
        /// </summary>
        public static string FilePath => "~/App_Data/dataSettings.json";

        public static string FolderWorkFiles => "~/App_Data/WorkFiles/";
        public static string FolderTrashFiles => "~/App_Data/TrashFiles/";
        public static int PECCProcuringAgencyId => 1;
        /// <summary>
        /// Id loai tien te la Vàng
        /// </summary>
        public static int GoldCurrencyId => 12;
        /// <summary>
        /// Id phong ban giam doc, de lay tat ca user trong ban giam doc
        /// </summary>
        public static int ManagerUnitId => 37;
    }
}