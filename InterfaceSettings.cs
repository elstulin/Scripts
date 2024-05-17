using System;
using System.IO;
using System.Xml;
public static class InterfaceSettings
{
    public static string login;
    public static string password;
    public static int resolution = 2;
    public static bool window = false;
    public static bool vSync = false;
    public static int textureLevel = 3;
    public static bool postprocessing = true;
    public static int shadows = 500;
    public static int renderDistance = 600;
    public static int antialiasing = 0;
    public static int dynamicResolution = 0;
    public static int dynamicResolutionPercent = 100;
    public static int targetFrameRate = 60;
    public static bool dynamicResolutionAuto = false;
    public static bool classicControll = true;
    public static float cameraSensitive = 0.75f;
    public static int chatPosX = 2;
    public static int chatPosY = 30;
    public static int chatSizeX = 400;
    public static int chatSizeY = 340;
    public static string[] blockList = new string[0];
    public static bool[] channelsActive = new bool[] { true, true, true, true, true, true, true, true};
    public static void Load()
    {
        string filepath = "Settings.xml";
        if (!File.Exists(filepath)) return;
        XmlTextReader reader = new XmlTextReader(filepath);
        while (reader.Read())
        {
            if (reader.IsStartElement("ChatSettings"))
            {
                int.TryParse(reader.GetAttribute("PositionX"), out chatPosX);
                int.TryParse(reader.GetAttribute("PositionY"), out chatPosY);
                int.TryParse(reader.GetAttribute("SizeX"), out chatSizeX);
                int.TryParse(reader.GetAttribute("SizeY"), out chatSizeY);
                string value = reader.GetAttribute("BlockList");
                blockList = value.Split(",");
                for (int i = 0; i < channelsActive.Length; i++)
                {
                    bool channelActive;
                    bool.TryParse(reader.GetAttribute("ChannelsActive" + i), out channelActive);
                    channelsActive[i] = channelActive;
                }
            }
            if (reader.IsStartElement("GraphicSettings"))
            {
                login = reader.GetAttribute("Login");
                password = reader.GetAttribute("Password");
                int.TryParse(reader.GetAttribute("Resolution"), out resolution);
                bool.TryParse(reader.GetAttribute("Window"), out window);
                bool.TryParse(reader.GetAttribute("vSync"), out vSync);
                int.TryParse(reader.GetAttribute("TextureLevel"), out textureLevel);
                bool.TryParse(reader.GetAttribute("Postprocessing"), out postprocessing);
                int.TryParse(reader.GetAttribute("Shadows"), out shadows);
                int.TryParse(reader.GetAttribute("RenderDistance"), out renderDistance);
                int.TryParse(reader.GetAttribute("Antialiasing"), out antialiasing);
                int.TryParse(reader.GetAttribute("DynamicResolution"), out dynamicResolution);
                int.TryParse(reader.GetAttribute("DynamicResolutionPercent"), out dynamicResolutionPercent);
                int.TryParse(reader.GetAttribute("TargetFrameRate"), out targetFrameRate);
                bool.TryParse(reader.GetAttribute("DynamicResolutionAuto"), out dynamicResolutionAuto);
                bool.TryParse(reader.GetAttribute("ClassicControll"), out classicControll);
                float.TryParse(reader.GetAttribute("CameraSensitive"), out cameraSensitive);
            }
        }


        reader.Close();
    }
    public static void SaveGraphicSettings(string login,string password,int resolution, bool window, bool vSync, int textureLevel, bool postprocessing, int shadows, int renderDistance, int antialiasing, int dynamicResolution,int dynamicResolutionPercent,int targetFrameRate,bool dynamicResolutionAuto,bool classicControll,float cameraSensitive)
    {
        string filepath = "Settings.xml";
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        XmlElement element;

        // rootNode.RemoveAll();
        if (!File.Exists(filepath))
        {
            rootNode = xmlDoc.CreateElement("InterfaceSettings");
            xmlDoc.AppendChild(rootNode);

        }
        else
        {
            xmlDoc.Load(filepath);
            rootNode = xmlDoc.DocumentElement;

        }

        element = xmlDoc.CreateElement("GraphicSettings");
        element.SetAttribute("Login", login);
        element.SetAttribute("Password", password);
        element.SetAttribute("Resolution", resolution.ToString());
        element.SetAttribute("Window", window.ToString());
        element.SetAttribute("vSync", vSync.ToString());
        element.SetAttribute("TextureLevel", textureLevel.ToString());
        element.SetAttribute("Postprocessing", postprocessing.ToString());
        element.SetAttribute("Shadows", shadows.ToString());
        element.SetAttribute("RenderDistance", renderDistance.ToString());
        element.SetAttribute("Antialiasing", antialiasing.ToString());
        element.SetAttribute("DynamicResolution", dynamicResolution.ToString());
        element.SetAttribute("DynamicResolutionPercent", dynamicResolutionPercent.ToString());
        element.SetAttribute("TargetFrameRate", targetFrameRate.ToString());
        element.SetAttribute("DynamicResolutionAuto", dynamicResolutionAuto.ToString());
        element.SetAttribute("ClassicControll", classicControll.ToString());
        element.SetAttribute("CameraSensitive", cameraSensitive.ToString());
        XmlNodeList xmlNodeList = rootNode.ChildNodes;
        if (xmlNodeList.Item(1) != null)
            rootNode.ReplaceChild(element, xmlNodeList.Item(1));
        else
            rootNode.AppendChild(element);
        xmlDoc.Save(filepath);
    }
    public static void SaveChatSettings(int chatPosX, int chatPosY, int chatSizeX, int chatSizeY, bool[] channelsActive, string[] blockList)
    {
        string filepath = "Settings.xml";
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode rootNode = null;
        XmlElement element;

        // rootNode.RemoveAll();
        if (!File.Exists(filepath))
        {
            rootNode = xmlDoc.CreateElement("InterfaceSettings");
            xmlDoc.AppendChild(rootNode);

        }
        else
        {
            xmlDoc.Load(filepath);
            rootNode = xmlDoc.DocumentElement;

        }
        element = xmlDoc.CreateElement("ChatSettings");
        element.SetAttribute("PositionX", chatPosX.ToString());
        element.SetAttribute("PositionY", chatPosY.ToString());
        element.SetAttribute("SizeX", chatSizeX.ToString());
        element.SetAttribute("SizeY", chatSizeY.ToString());
        element.SetAttribute("BlockList", string.Join(",", blockList).ToString());
        for (int i = 0; i < channelsActive.Length; i++)
            element.SetAttribute("ChannelsActive" + i, channelsActive[i].ToString());
        XmlNodeList xmlNodeList = rootNode.ChildNodes;
        if (xmlNodeList.Item(0) != null)
            rootNode.ReplaceChild(element, xmlNodeList.Item(0));
        else
            rootNode.AppendChild(element);
        xmlDoc.Save(filepath);
    }
}
