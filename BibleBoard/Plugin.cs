using BepInEx;
using System.Collections.Generic;
using System.IO;
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
        private Vector3 POS = new Vector3(-67.23f, 11.4845f, -82.6222f);
        private TextMeshProUGUI Text;
        private List<string> Verses = new List<string>
        {
            "\"I can do all things through Christ who strengthens me.\" \n- Philippians 4:13",
            "\"Trust in the Lord with all your heart and lean not on your own understanding.\" \n- Proverbs 3:5",
            "\"The Lord is my shepherd; I shall not want.\" - Psalm 23:1",
            "\"Be strong and courageous. Do not be afraid; do not be discouraged.\" \n- Joshua 1:9",
            "\"Come to me, all you who are weary and burdened, and I will give you rest.\" \n- Matthew 11:28",
            "\"But those who hope in the Lord will renew their strength. They will soar on wings like eagles.\" \n- Isaiah 40:31",
            "\"The Lord is near to all who call on Him, to all who call on Him in truth.\" \n- Psalm 145:18",
            "\"Cast all your anxiety on Him because He cares for you.\" \n- 1 Peter 5:7",
            "\"Rejoice always, pray continually, give thanks in all circumstances.\" \n- 1 Thessalonians 5:16-18",
            "\"God is our refuge and strength, an ever-present help in trouble.\" \n- Psalm 46:1",
            "\"With man this is impossible, but with God all things are possible.\" \n- Matthew 19:26",
            "\"The Lord will fight for you; you need only to be still.\" \n- Exodus 14:14",
            "\"The earth is the Lord’s, and everything in it, the world, and all who live in it.\" \n- Psalm 24:1",
            "\"The Lord is my light and my salvation—whom shall I fear? The Lord is the stronghold of my life—of whom shall I be afraid?\" \n- Psalm 27:1",
            "\"Blessed are those who are persecuted because of righteousness, for theirs is the kingdom of heaven.\" \n- Matthew 5:10",
            "\"The fear of the Lord is the beginning of wisdom; all who follow His precepts have good understanding.\" \n- Psalm 111:10",
            "\"And we know that in all things God works for the good of those who love Him, who have been called according to His purpose.\" \n- Romans 8:28",
            "\"The Lord is good to those who wait for Him, to the soul who seeks Him.\" \n- Lamentations 3:25",
            "\"He heals the brokenhearted and binds up their wounds.\" \n- Psalm 147:3",
            "\"Let everything that has breath praise the Lord.\" \n- Psalm 150:6",
            "\"I will never leave you nor forsake you.\" \n- Hebrews 13:5"
        };

        void Start()
        {
            var bundle = LoadAssetBundle("BibleBoard.Assets.bibleboard");
            BibleBoard = bundle.LoadAsset<GameObject>("BibleBoard");

            GorillaTagger.OnPlayerSpawned(Init);
        }

        async void Init()
        {
            BibleBoard = Instantiate(BibleBoard);
            BibleBoard.transform.position = POS;
            BibleBoard.name = "BibleBoard";

            await Task.Delay(500);

            foreach (Transform child in BibleBoard.GetComponentsInChildren<Transform>(true))
            {
                Debug.Log($"Found object: {child.name}");
            }

            Transform verseTransform = BibleBoard.transform.Find("Board/Backboard/Board/Canvas/Verse");
            verseTransform.gameObject.SetActive(true);

            Text = verseTransform.GetComponent<TextMeshProUGUI>();
            Text.text = GetRandomVerse();
        }

        private string GetRandomVerse()
        {
            return Verses[Random.Range(0, Verses.Count)];
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
    }
}
