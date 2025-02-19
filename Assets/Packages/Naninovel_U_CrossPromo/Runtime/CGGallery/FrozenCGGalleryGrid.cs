using Naninovel;


public class FrozenCGGalleryGrid : CGGalleryGrid
{
    private CGGalleryGridSlot[] allSlots;

    protected override void OnEnable()
    {
        base.OnEnable();
        FindAllSlots();
        Initialize(allSlots.Length);
    }

    protected override void Paginate()
    {
        if (Slots == null) throw new System.Exception("The grid is not initialized.");

        // Disable all slots before paginating
        foreach (var slot in allSlots)
            slot.gameObject.SetActive(false);

        // Activate slots for the current page
        for (int i = 0; i < Slots.Count; i++)
        {
            var itemIndex = (CurrentPage - 1) * ItemsPerPage + i;
            if (itemIndex < allSlots.Length)
            {
                var slot = allSlots[itemIndex];
                slot.gameObject.SetActive(true);
                BindSlot(slot, itemIndex);
            }
        }

        // Update navigation buttons
        if (PreviousPageButton)
            PreviousPageButton.interactable = CurrentPage > 1;
        if (NextPageButton)
            NextPageButton.interactable = CurrentPage < PageCount;
    }

    private void FindAllSlots()
    {
        // Automatically find all CGGalleryGridSlot components in children
        allSlots = GetComponentsInChildren<CGGalleryGridSlot>(true);
        if (allSlots == null || allSlots.Length == 0)
            UnityEngine.Debug.LogError("No CGGalleryGridSlot components found in children.");
    }
}
