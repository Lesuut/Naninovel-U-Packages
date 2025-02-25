namespace Naninovel.U.HotelManagement
{
    [ExpressionFunctions]
    public static class HotelManagementFunctions
    {
        public static bool IsHotelManagementWin()
        {
            return Engine.GetService<IHotelManagementManager>().IsHotelWin();
        }
    }
}