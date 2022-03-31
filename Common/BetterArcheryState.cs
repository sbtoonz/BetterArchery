namespace BetterArchery.Common
{
    public static class BetterArcheryState
    {
        public const int RowCount = 2;

        public static bool QuiverEnabled;

        public static int RowStartIndex;

        public static int RowEndIndex;

        public static void UpdateRowIndex()
        {
            int quiverRowIndex = global::BetterArchery.BetterArchery.QuiverRowIndex;
            if (quiverRowIndex > 0)
            {
                QuiverEnabled = true;
                RowStartIndex = quiverRowIndex - 1;
                RowEndIndex = RowStartIndex + 2;
            }
            else
            {
                QuiverEnabled = false;
                RowStartIndex = 0;
                RowEndIndex = 0;
            }
        }
    }
}