public struct AppInfo
{
    public string name;
    public string path;

    public AppInfo(string name, string path)
    {
        this.name = name;
        this.path = path;
    }
}

public static class AppCatalog
{
    public static AppInfo[] catalog = new AppInfo[]
    {
        new AppInfo("Adobe Acrobat Reader", "logos/adobe acrobat reader"),
        new AppInfo("Adobe Photoshop Express", "logos/adobe photoshop express"),
        new AppInfo("Amazon Prime Video", "logos/amazon prime video"),
        new AppInfo("Apple Music", "logos/apple music"),
        new AppInfo("Audible", "logos/audible"),
        new AppInfo("Deezer", "logos/deezer"),
        new AppInfo("Discord", "logos/discord"),
        new AppInfo("Disney +", "logos/disney plus"),
        new AppInfo("DSB", "logos/dsb"),
        new AppInfo("Facebook", "logos/facebook"),
        new AppInfo("Firefox", "logos/firefox"),
        new AppInfo("Github", "logos/github"),
        new AppInfo("Google Calculator", "logos/google calculator"),
        new AppInfo("Google Calendar", "logos/google calendar"),
        new AppInfo("Google chrome", "logos/google chrome"),
        new AppInfo("Google Contacts", "logos/google contacts"),
        new AppInfo("Google Drive", "logos/google drive"),
        new AppInfo("Google Maps", "logos/google maps"),
        new AppInfo("Google Phone", "logos/google phone"),
        new AppInfo("Google Translate", "logos/google translate"),
        new AppInfo("HBO Max", "logos/hbo max"),
        new AppInfo("Hulu", "logos/hulu"),
        new AppInfo("Instagram", "logos/instagram"),
        new AppInfo("LinkedIn", "logos/linkedin"),
        new AppInfo("McDonald's", "logos/mcdonalds"),
        new AppInfo("Messenger", "logos/messenger"),
        new AppInfo("Microsoft 365", "logos/microsoft 365"),
        new AppInfo("Microsoft OneDrive", "logos/microsoft onedrive"),
        new AppInfo("Microsoft Teams", "logos/microsoft teams"),
        new AppInfo("Minecraft", "logos/minecraft"),
        new AppInfo("MobilePay", "logos/mobilepay"),
        new AppInfo("Netflix", "logos/netflix"),
        new AppInfo("Nord VPN", "logos/nord vpn"),
        new AppInfo("Pinterest", "logos/pinterest"),
        new AppInfo("PlayStation", "logos/playstation"),
        new AppInfo("Reddit", "logos/reddit"),
        new AppInfo("Settings", "logos/settings"),
        new AppInfo("SkyShowtime", "logos/skyshowtime"),
        new AppInfo("Slack", "logos/slack"),
        new AppInfo("Snapchat", "logos/snapchat"),
        new AppInfo("Spotify", "logos/spotify"),
        new AppInfo("Steam", "logos/steam"),
        new AppInfo("TikTok", "logos/tiktok"),
        new AppInfo("Tinder", "logos/tinder"),
        new AppInfo("Twitch", "logos/twitch"),
        new AppInfo("Twitter", "logos/twitter"),
        new AppInfo("VLC", "logos/vlc"),
        new AppInfo("Wolt", "logos/wolt"),
        new AppInfo("XBOX", "logos/xbox"),
        new AppInfo("YouTube", "logos/youtube"),
    };
}
