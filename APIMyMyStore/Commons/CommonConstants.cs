namespace APIMyMyStore
{
    public class CommonConstants
    {
        //SECURITY
        public static string TOKEN_SECURITY_KEY = "SDGD$E^&Ư#RBSDGFGJ*IY^&ÉDQQWRWF#$%#SGSAS";
        public static byte[] SECURITY_KEY = System.Text.ASCIIEncoding.UTF8.GetBytes(TOKEN_SECURITY_KEY).Take(16).ToArray();// min 16 chars
        public static byte[] SECURITY_IV = System.Text.ASCIIEncoding.UTF8.GetBytes("@&AA#$D@ILYXE@2022").Take(16).ToArray(); // min 16 chars

        public const int TOKEN_DURATION = 1;                // 1 day
        public const int TOKEN_DURATION_APP = 30;                // 30 days
        public const string TOKEN_HEADER_NAME = "Authorization";
        public const string TOKEN_HEADER_HOST = "PrivateHost";

        public const string HEADER_IPADDRESS = "IPAddress";
        public const string HEADER_DEVICEID = "DeviceId";
        public const string HEADER_DEVICENAME = "DeviceName";
        public const string HEADER_OSNAME = "OSName";
        public const string HEADER_LOCATION = "Location";
        public const string HEADER_FCMTOKEN = "FCMToken";


        // CAPTCHA
        public const string CAPTCHA_NAME = "captcha";
        public const string CAPTCHA_CODE = "captchacode";
        public const string CAPTCHA_VALUE = "captcha";
        public const int CAPTCHA_MAX_INPUT = 2;               // 3 lan se bat Captcha
        public const int CAPTCHA_DURATION = 10;              // 10 minute
        public const int CAPTCHA_SESSION_TIME = 10;              // 10 minute
        public const int CAPTCHA_LENGTH = 6;
        public const int CAPTCHA_HEIGHT = 25;
        public const int CAPTCHA_WIDTH = 10;
        public const int CAPTCHA_FONTSIZE = 16;
        public const string CAPTCHA_FONTNAME = "Tahoma";
        public const string CAPTCHA_NOT_VALID = "Mã xác nhận không hợp lệ. Vui lòng nhập lại!";

        public const int LOGIN_DURATION = 30;              // 30 minute
        public const int LOGIN_MAX_TIMES = 4;

        public const int MAX_LENGTH_UNIQUEID = 7;
        public const int MAX_DOMAIN = 6;
        public const int MAX_LENGTH_DETAIL_CONTENT = 1048576;      // 1MB = 1 * 1024 * 1024
        public const int MAX_LENGTH_DESCRIPTION_PRODUCT = 1000;

        // COOKIES
        public const string COOKIE_UID = "uid";

        // VERIFY
        public const int VERIFY_DURATION_HOUR = 24;              // 24 Hour
        public const int VERIFY_LENGTH = 7;
        public const int VERIFY_EMAIL_LENGTH = 6;

        public const int PASSWORD_LENGTH = 9;

        // Design
        public const int DESIGN_MAX_BACKUP_TIMES = 3;

        // Files
        public const int IMAGE_MAX_WIDTH = 1920;
        public const int IMAGE_DEFAULT_WIDTH = 300;
        public const int IMAGE_DEFAULT_HEIGHT = 300;
        public const int FILE_MAX_SIZE = 4194304; // 4MB
        public const string IMAGE_NOT_FOUND = "https://cdn.gianhangvn.com/image/hinh-anh-khong-ton-tai.jpg";
        public const int DAYS_STORAGE_IN_TRASH = 30;

        // MESSAGE TOKEN
        public const string MESSAGE_TOKEN_INVALID = "Token không hợp lệ!";
        public const string MESSAGE_TOKEN_EXPIRED = "Token hết hạn !";

        // MESSAGE USER

        public const string MESSAGE_USER_NOT_VALID = "Tài khoản tạm khóa hoặc đã bị xóa";
        public const string MESSAGE_USER_NOT_EXIST = "Tài khoản không tồn tại";
        public const string MESSAGE_USER_NOT_PERMISSON = "Bạn không có quyền thay đổi thông tin này!";


        // MESSAGE COMMON
        public const string MESSAGE_DATA_NOT_FOUND = "Không tìm thấy dữ liệu!";
        public const string MESSAGE_DATA_NOT_VALID = "Dữ liệu không hợp lệ!";
        public const string MESSAGE_INSERT_UNSUCCESS = "Thêm không thành công!";
        public const string MESSAGE_UPDATE_UNSUCCESS = "Cập nhật không thành công!";

        // MESSAGE LOGIN
        public const string MESSAGE_LOGIN_FAIL = "Thông tin đăng nhập không đúng!";

        // MESSAGE FILE
        public const string MESSAGE_FILE_NAME_NOT_EMPTY = "Dữ liệu không được để trống!";
        public const string MESSAGE_FOLDER_NAME_EXIST = "Folder này đã tồn tại!";
        public const string MESSAGE_FILE_OVER_QUOTA = "File vượt quá dung lượng cho phép!";
        public const string MESSAGE_FOLDER_HAS_CONTENT = "Folder này còn nội dung, không thể xóa!";
        public const string MESSAGE_FILE_NAME_EXIST = "File này đã tồn tại!";

        public const string YYYYMMDD = "yyyyMMdd";
        public const string YYYYMMDDHHMMSS = "yyyyMMddHHmmss";
        public const string YYYY_MM_DD = "yyyy-MM-dd";
        public const string DD_MM_YYYY = "dd-MM-yyyy";
        public const string DD_MM_YYYY_HH_MM = "dd-MM-yyyy HH:mm";
        public const string DD_MM_YYYY_HH_MM_SS = "dd-MM-yyyy HH:mm:ss";
        public const string DD_MM_YYYY_SEO = "yyyy-MM-ddTHH:mm:ss.mmm"; //yyyy-mm-ddThh:mi:ss.mmm
        public const string DD_MM_YYYY_HH_MM_SS_EMAIL = "yyyy-MM-dd'T'HH:mm:ss";

        // Permission
        public const string PERMISSION_FULL = "full";
        public const string PERMISSION_ACCESS = "access";
        public const string PERMISSION_CREATE = "create";
        public const string PERMISSION_EDIT = "edit";
        public const string PERMISSION_DELETE = "delete";
        public const string PERMISSION_DISPLAY = "display";
        public const string PERMISSION_HIDDEN = "hidden";
        public const string PERMISSION_SAVE = "save";

        // FOLDER DATA
        public const string FOLDER_BRAND = "Brands";
        public const string FOLDER_DEALER = "Dealers";
        public const string FOLDER_MODEL = "Models";
        public const string FOLDER_VEHICLE = "Vehicles";
        public const string FOLDER_PRICELIST = "PriceList";
        public const string FOLDER_CATEGORY = "Categories";
        public const string FOLDER_POST = "Posts";
        public const string FOLDER_NEWS = "News";
        public const string FOLDER_VIDEO = "Videos";
        public const string FOLDER_VIDEO_PLAYLIST = "VideoPlayList";

        // TYPE IN INPUT OR LIST
        public const string INPUT_CATEGORY = "category";
        public const string INPUT_BRAND_CATEGORY = "brandcategory";
        public const string INPUT_BODYTYPE = "bodytype";
        public const string INPUT_BRAND = "brand";
        public const string INPUT_DEALER = "dealer";
        public const string INPUT_PRICELIST = "pricelist";
        public const string INPUT_MODEL = "model";
        public const string INPUT_VEHICLE = "vehicle";
        public const string INPUT_NEWS = "news";
        public const string INPUT_SUGGEST_NEWS = "suggestnews";
        public const string INPUT_CONTENT = "content";
        public const string INPUT_ALBUM = "album";
        public const string INPUT_VIDEO = "video";
        public const string INPUT_IMAGE = "image";
        public const string INPUT_IMAGES = "images";
        public const string INPUT_EDITOR = "editor";
        public const string INPUT_LIST = "list";                   // Dung cho du lieu khong co luu trong database. Nhap truc tiep len Widget: Ho tro truc tuyen, Lien ket website,...

        // VALIDATE DATA
        public const int MIN_LENGTH_NAME = 2;
        public const int MAX_LENGTH_NAME = 150;
        public const int MIN_LENGTH_DESCRIPTION = 2;
        public const int MAX_LENGTH_DESCRIPTION = 400;
        public const int MIN_LENGTH_SEOTITLE = 2;
        public const int MAX_LENGTH_SEOTITLE = 80;
        public const int MIN_LENGTH_SEODESCRIPTION = 2;
        public const int MAX_LENGTH_SEODESCRIPTION = 200;

        public const int MIN_LENGTH_PHONE = 9;
        public const int MAX_LENGTH_PHONE = 10;

        public const int MIN_LENGTH_PRICE = 2;
        public const int MAX_LENGTH_PRICE = 18;

        public const int MIN_VALUE_PRICE = 1000;
        public const int MAX_VALUE_PRICE = 100000000;

        // DEFAULT VALUES
        public const int MAX_QUERY_SELECT_TOP = 1500;
        public const int MAX_QUERY_WHERE_IN = 1500;
        public const int MAX_ITEM_WIDGET = 20;
        public const int MAX_ITEMS_ON_PAGE = 24;
        public const int MAX_ITEMS_MORE = 9;
        public const int SELECT_TOP_VIDEOPLAYLIST = 10;

        // FIX REWRITEURL

    }
}