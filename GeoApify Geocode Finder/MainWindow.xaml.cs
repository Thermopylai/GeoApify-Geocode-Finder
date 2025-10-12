using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeoApify_Geocode_Finder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Root root = new Root();
        public const string API_KEY = "8d53629cc46741fb9e6c97eb48d66533";
        public string searchString = string.Empty;
        public string searchURL => $"https://api.geoapify.com/v1/geocode/search?text={searchString}&format=json&apiKey={API_KEY}";

        public MainWindow()
        {
            InitializeComponent();
            Clear();
        }

        private async void Button_Click_Send(object sender, EventArgs e)
        {
            searchString = Convert(txtAddr.Text);
            if (!string.IsNullOrEmpty(searchString))
            {
                btnSend.IsEnabled = false; // Disable button
                lblStatus.Content = "Loading...";
                await GetCoordsAsync();
                ShowCoords();
                if (root.results[0].lat is not null && root.results[0].lon is not null)
                {
                    lblStatus.Content = "Coordinates retrieved successfully!";
                }
                else
                {
                    lblStatus.Content = "No coordinates found.";
                }
                btnSend.IsEnabled = true; // Re-enable button
            }
            else
                MessageBox.Show("Please, enter an identifier!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async Task GetCoordsAsync()
        {
            var result = await GetDataGetMethod<Root>(searchURL);
            if (result == null || result.results[0].lat is null || result.results[0].lon is null)
            {
                MessageBox.Show("Failed to retrieve valid coordinates. Please check the identifier or try again later.", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                root = new Root(); // Reset to empty
            }
            else
            {
                root = result;
            }
        }

        public static async Task<Root> GetDataGetMethod<T>(string url)
        {
            var ss = new Root();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<Root>(responseString);
                        return responseObject;
                    }
                    return ss;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching coordinates: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return ss;
            }
        }
    

        private void ShowCoords()
            {
                lblLat.Content = root.results[0].lat;
                lblLong.Content = root.results[0].lon;
            }
        private void Clear()
            {
                lblLat.Content = string.Empty;
                lblLong.Content = string.Empty;
                lblStatus.Content = string.Empty;
                txtAddr.Text = string.Empty;
            }

        private void Button_Click_Clear(object sender, EventArgs e)
        {
            Clear();
        }

        private string Convert(string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if (utf8_http_conversion.TryGetValue(c.ToString(), out string s))
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public class Root
        {
            public List<Results> results { get; set; }
            public Query query { get; set; }
            public Root()
            {
                results = new List<Results>();
                query = new Query();
            }
        }
        public class Results
        {
            public string country_code { get; set; }
            public string housenumber { get; set; }
            public string street { get; set; }
            public string country { get; set; }
            public string county { get; set; }
            public Datasource datasource { get; set; }
            public string postcode { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public double? lon { get; set; }
            public double? lat { get; set; }
            public string result_type { get; set; }
            public string formatted { get; set; }
            public string address_line1 { get; set; }
            public string address_line2 { get; set; }
            public Timezone timezone { get; set; }
            public string plus_code { get; set; }
            public string plus_code_short { get; set; }
            public Rank rank { get; set; }
        }


        public class Datasource
        {
            public string sourcename { get; set; }
            public string attribution { get; set; }
            public string license { get; set; }
        }
        public class Timezone
        {
            public string name { get; set; }
            public string offset_STD { get; set; }
            public int offset_STG_seconds { get; set; }
            public string offset_DST { get; set; }
            public int offset_DST_seconds { get; set; }
            public string abbreviation_STD { get; set; }
            public string abbreviation_DST { get; set; }
        }
        public class Rank
        {
            public double popularity { get; set; }
            public int confidence { get; set; }
            public int confidence_city_level { get; set; }
            public int confidence_street_level { get; set; }
            public int confidence_building_level { get; set; }
            public string match_type { get; set; }
            public string place_id { get; set; }
        }
        public class Query
        {
            public string text { get; set; }
            public Parsed parsed { get; set; }
        }
        public class Parsed
        {
            public string housenumber { get; set; }
            public string street { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string expected_type { get; set; }
        }

        Dictionary<string, string> utf8_http_conversion = new Dictionary<string, string>()
        {
            {@" ",  @"%20"},
            {@"!",  @"%21"},
            {"\"",  @"%22"},
            {@"#",  @"%23"},
            {@"$",  @"%24"},
            {@"%",  @"%25"},
            {@"&",  @"%26"},
            {@"'",  @"%27"},
            {@"(",  @"%28"},
            {@")",  @"%29"},
            {@"*",  @"%2A"},
            {@"+",  @"%2B"},
            {@",",  @"%2C"},
            {@"-",  @"%2D"},
            {@".",  @"%2E"},
            {@"/",  @"%2F"},
            {@":",  @"%3A"},
            {@";",  @"%3B"},
            {@"<",  @"%3C"},
            {@"=",  @"%3D"},
            {@">",  @"%3E"},
            {@"?",  @"%3F"},
            {@"@",  @"%40"},
            {@"[",  @"%5B"},
            {@"\",  @"%5C"},
            {@"]",  @"%5D"},
            {@"^",  @"%5E"},
            {@"_",  @"%5F"},
            {@"`",  @"%60"},
            {@"{",  @"%7B"},
            {@"|",  @"%7C"},
            {@"}",  @"%7D"},
            {@"~",  @"%7E"},
            {@"€",  @"%E2%82%AC"},
            {@"‚",  @"%E2%80%9A"},
            {@"ƒ",  @"%C6%92"},
            {@"„",  @"%E2%80%9E"},
            {@"…",  @"%E2%80%A6"},
            {@"†",  @"%E2%80%A0"},
            {@"‡",  @"%E2%80%A1"},
            {@"ˆ",  @"%CB%86"},
            {@"‰",  @"%E2%80%B0"},
            {@"Š",  @"%C5%A0"},
            {@"‹",  @"%E2%80%B9"},
            {@"Œ",  @"%C5%92"},
            {@"Ž",  @"%C5%BD"},
            {@"‘",  @"%E2%80%98"},
            {@"’",  @"%E2%80%99"},
            {@"“",  @"%E2%80%9C"},
            {@"”",  @"%E2%80%9D"},
            {@"•",  @"%E2%80%A2"},
            {@"–",  @"%E2%80%93"},
            {@"—",  @"%E2%80%94"},
            {@"˜",  @"%CB%9C"},
            {@"™",  @"%E2%84"},
            {@"š",  @"%C5%A1"},
            {@"›",  @"%E2%80"},
            {@"œ",  @"%C5%93"},
            {@"ž",  @"%C5%BE"},
            {@"Ÿ",  @"%C5%B8"},
            {@"¡",  @"%C2%A1"},
            {@"¢",  @"%C2%A2"},
            {@"£",  @"%C2%A3"},
            {@"¤",  @"%C2%A4"},
            {@"¥",  @"%C2%A5"},
            {@"¦",  @"%C2%A6"},
            {@"§",  @"%C2%A7"},
            {@"¨",  @"%C2%A8"},
            {@"©",  @"%C2%A9"},
            {@"ª",  @"%C2%AA"},
            {@"«",  @"%C2%AB"},
            {@"¬",  @"%C2%AC"},
            {@"®",  @"%C2%AE"},
            {@"¯",  @"%C2%AF"},
            {@"°",  @"%C2%B0"},
            {@"±",  @"%C2%B1"},
            {@"²",  @"%C2%B2"},
            {@"³",  @"%C2%B3"},
            {@"´",  @"%C2%B4"},
            {@"µ",  @"%C2%B5"},
            {@"¶",  @"%C2%B6"},
            {@"·",  @"%C2%B7"},
            {@"¸",  @"%C2%B8"},
            {@"¹",  @"%C2%B9"},
            {@"º",  @"%C2%BA"},
            {@"»",  @"%C2%BB"},
            {@"¼",  @"%C2%BC"},
            {@"½",  @"%C2%BD"},
            {@"¾",  @"%C2%BE"},
            {@"¿",  @"%C2%BF"},
            {@"À",  @"%C3%80"},
            {@"Á",  @"%C3%81"},
            {@"Â",  @"%C3%82"},
            {@"Ã",  @"%C3%83"},
            {@"Ä",  @"%C3%84"},
            {@"Å",  @"%C3%85"},
            {@"Æ",  @"%C3%86"},
            {@"Ç",  @"%C3%87"},
            {@"È",  @"%C3%88"},
            {@"É",  @"%C3%89"},
            {@"Ê",  @"%C3%8A"},
            {@"Ë",  @"%C3%8B"},
            {@"Ì",  @"%C3%8C"},
            {@"Í",  @"%C3%8D"},
            {@"Î",  @"%C3%8E"},
            {@"Ï",  @"%C3%8F"},
            {@"Ð",  @"%C3%90"},
            {@"Ñ",  @"%C3%91"},
            {@"Ò",  @"%C3%92"},
            {@"Ó",  @"%C3%93"},
            {@"Ô",  @"%C3%94"},
            {@"Õ",  @"%C3%95"},
            {@"Ö",  @"%C3%96"},
            {@"×",  @"%C3%97"},
            {@"Ø",  @"%C3%98"},
            {@"Ù",  @"%C3%99"},
            {@"Ú",  @"%C3%9A"},
            {@"Û",  @"%C3%9B"},
            {@"Ü",  @"%C3%9C"},
            {@"Ý",  @"%C3%9D"},
            {@"Þ",  @"%C3%9E"},
            {@"ß",  @"%C3%9F"},
            {@"à",  @"%C3%A0"},
            {@"á",  @"%C3%A1"},
            {@"â",  @"%C3%A2"},
            {@"ã",  @"%C3%A3"},
            {@"ä",  @"%C3%A4"},
            {@"å",  @"%C3%A5"},
            {@"æ",  @"%C3%A6"},
            {@"ç",  @"%C3%A7"},
            {@"è",  @"%C3%A8"},
            {@"é",  @"%C3%A9"},
            {@"ê",  @"%C3%AA"},
            {@"ë",  @"%C3%AB"},
            {@"ì",  @"%C3%AC"},
            {@"í",  @"%C3%AD"},
            {@"î",  @"%C3%AE"},
            {@"ï",  @"%C3%AF"},
            {@"ð",  @"%C3%B0"},
            {@"ñ",  @"%C3%B1"},
            {@"ò",  @"%C3%B2"},
            {@"ó",  @"%C3%B3"},
            {@"ô",  @"%C3%B4"},
            {@"õ",  @"%C3%B5"},
            {@"ö",  @"%C3%B6"},
            {@"÷",  @"%C3%B7"},
            {@"ø",  @"%C3%B8"},
            {@"ù",  @"%C3%B9"},
            {@"ú",  @"%C3%BA"},
            {@"û",  @"%C3%BB"},
            {@"ü",  @"%C3%BC"},
            {@"ý",  @"%C3%BD"},
            {@"þ",  @"%C3%BE"},
            {@"ÿ",  @"%C3%BF"}
        };
    }
}