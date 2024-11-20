namespace PiecesAutoYoussefApp.NotificationCenter
{
    public static class InfoHandler
    {
        public static void NotifyAddSave(int rowsAffected, string? elementName = null)
        {
            NotifyInfo($"{rowsAffected} {elementName ?? "item(s)"} added successfully!");
        }

        public static void NotifyUpdateSave(int rowsAffected, string? elementName = null)
        {
            NotifyInfo($"{rowsAffected} {elementName ?? "item(s)"} updated successfully!");
        }

        public static bool AssureDeletion(int rowsSelected, string? elementName = null)
        {
            return MessageBox.Show(
                $"Are you sure you want to delete {rowsSelected} {elementName ?? "item(s)"}?",
                "Delete Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        public static void NotifyDelete(int rowsAffected, string? elementName = null)
        {
            NotifyInfo($"{rowsAffected} {elementName ?? "item(s)"} deleted successfully!");
        }

        public static void NotifyInfo(string msg)
        {
            MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
