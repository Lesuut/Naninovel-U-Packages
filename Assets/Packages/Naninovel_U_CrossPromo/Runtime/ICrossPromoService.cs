namespace Naninovel.U.CrossPromo
{
    public interface ICrossPromoService : IEngineService
    {
        public void ShowCrossPromo(LinkTransitionType linkTransitionType); // Показывает кросс-промо с переходом по типу ссылки
        public bool IsCGSlotValid(string unlockableKey); // Проверяет, является ли слот с заданным ключом валидным (детально описанно в CrossPromoCGSlotUI)
        public bool IsCrossPromoEnabled(); // Проверяет, активен ли кросс-промо сервис

        public void UnlockItem(int id); // Разблокирует элемент по указанному идентификатору
        public void UnlockRandomItem(); // Разблокирует случайный элемент
    }
}