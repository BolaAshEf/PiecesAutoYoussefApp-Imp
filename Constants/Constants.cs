namespace PiecesAutoYoussefApp.Utils
{
    public static class Constants
    {
        public const string UNIQUE_APP_MUTEX_NAME = "PIECESAUTOYOUSSEF";

        public static readonly float INIT_FONT_SCALE = 1.0F;

        public static string BASE_APP_DIR_PATH { get; private set; }

        public static string DB_DIR_PATH { get; private set; }
        public static string DB_FILE_PATH { get; private set; }


        public static string TEMPLATES_DIR_PATH { get; private set; }
        public static string PRINT_HTML_TEMPLATE_PATH { get; private set; }
        public static string PRINT_TEMP_HTML_PATH { get; private set; }
        public static string PRINT_OUT_PDF_FILE_PATH { get; private set; }

        public static string TOOLS_DIR_PATH { get; private set; }
        public static string PRINT_HTML2PDF_CMD_PATH { get; private set; }
        public static string PRINT_PDF_CMD_CACHE_PATH { get; private set; }
        public static string PRINT_PDF_CMD_PATH { get; private set; }


        static Constants()
        {
            BASE_APP_DIR_PATH = Path.Combine(Directory.GetCurrentDirectory(), "PiecesAutoYoussef");
            //BASE_APP_DIR_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PiecesAutoYoussef");

            DB_DIR_PATH = Path.Combine(BASE_APP_DIR_PATH, "db");
            DB_FILE_PATH = Path.Combine(DB_DIR_PATH, "pieceAutoYssfDB.mdf");

            TEMPLATES_DIR_PATH = Path.Combine(BASE_APP_DIR_PATH, "templates");
            PRINT_HTML_TEMPLATE_PATH = Path.Combine(TEMPLATES_DIR_PATH, "orders-bill-templated.html");
            PRINT_TEMP_HTML_PATH = Path.Combine(TEMPLATES_DIR_PATH, "out-bill.html");
            PRINT_OUT_PDF_FILE_PATH = Path.Combine(TEMPLATES_DIR_PATH, "out-bill.pdf");

            TOOLS_DIR_PATH = Path.Combine(BASE_APP_DIR_PATH, "tools");
            PRINT_HTML2PDF_CMD_PATH = Path.Combine(TOOLS_DIR_PATH, "wkhtmltopdf.exe");
            PRINT_PDF_CMD_CACHE_PATH = Path.Combine(TOOLS_DIR_PATH, "sumatrapdfcache");
            PRINT_PDF_CMD_PATH = Path.Combine(TOOLS_DIR_PATH, "sumatra.exe");
        }
    }
}
