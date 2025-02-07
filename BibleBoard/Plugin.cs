using BepInEx;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BibleBoard
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private GameObject BibleBoard;
        private readonly Vector3 POS = new(-67.23f, 11.4845f, -82.6222f);
        private TextMeshProUGUI Text;
        private static readonly HttpClient httpClient = new();

        static Plugin() => httpClient.DefaultRequestHeaders.Add("user-agent", "BibleBoard");

        void Start()
        {
            BibleBoard = LoadAssetBundle("BibleBoard.Assets.bibleboard").LoadAsset<GameObject>("BibleBoard");
            GorillaTagger.OnPlayerSpawned(Init);
        }

        async void Init()
        {
            var boardInstance = Instantiate(BibleBoard, POS, Quaternion.identity);
            boardInstance.name = "BibleBoard";
            await Task.Delay(500);
            
            Text = boardInstance.transform.Find("Board/Backboard/Board/Canvas/Verse").GetComponent<TextMeshProUGUI>();
            Text.gameObject.SetActive(true);
            Text.text = await GetRandomVerse();
            Text.fontSize = 0.055f;
        }

        private async Task<string> GetRandomVerse()
        {
            try
            {
                var response = await httpClient.GetAsync("https://labs.bible.org/api/?passage=random&type=text");
                response.EnsureSuccessStatusCode();
                return (await response.Content.ReadAsStringAsync()).Trim();
            }
            catch
            {
                return "Failed to load verse.";
            }
        }

        private AssetBundle LoadAssetBundle(string path)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return AssetBundle.LoadFromStream(stream);
        }
    }
}
